using System;
using System.Collections;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CMS.Core;
using CMS.Core.Domain;
using CMS.Core.Service.SiteStructure;
using CMS.Web.Components;
using CMS.Web.Util;
using log4net;

namespace CMS.Web.UI
{
    /// <summary>
    /// Page engine. This class loads all content based on url parameters and merges
    /// the content with the template.
    /// </summary>
    public class PageEngine : PortalPage
    {
        private bool _isRedirected;
        private readonly IDictionary _metaTags;
        private readonly ILog logger = LogManager.GetLogger(typeof (PageEngine));

        private readonly ModuleLoader _moduleLoader;
        private readonly INodeService _nodeService;
        private readonly ISectionService _sectionService;
        private readonly ISiteService _siteService;
        private readonly IDictionary _stylesheets;
        private readonly IList _navigationPath;
        private Node _activeNode;
        private Section _activeSection;
        private Site _currentSite;
        private Node _rootNode;
        private bool _shouldLoadContent;
        private BaseTemplate _templateControl;
        private Hashtable _globalvar;

        #region properties

        /// <summary>
        /// Flag to indicate if the engine should load content (Templates, Nodes and Sections).
        /// </summary>
        protected bool ShouldLoadContent
        {
            set { _shouldLoadContent = value; }
        }

        /// <summary>
        /// Property RootNode (Node)
        /// </summary>
        public Node RootNode
        {
            get { return _rootNode; }
        }

        /// <summary>
        /// Property ActiveNode (Node)
        /// </summary>
        public Node ActiveNode
        {
            get { return _activeNode; }
        }

        /// <summary>
        /// Property ActiveSection (Section)
        /// </summary>
        public Section ActiveSection
        {
            get { return _activeSection; }
        }

        /// <summary>
        /// Property TemplateControl (BaseTemplate)
        /// </summary>
        public new BaseTemplate TemplateControl
        {
            get { return _templateControl; }
            set { _templateControl = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public User CuyahogaUser
        {
            get { return User.Identity as User; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Site CurrentSite
        {
            get { return _currentSite; }
        }

        public string PageTitle
        {
            get { return _templateControl.Title; }
            set { _templateControl.Title = value; }
        }

        public IList NavigationPath
        {
            get { return _navigationPath; }
        }

        public Hashtable GlobalVar
        {
            get
            {
                if (_globalvar!=null)
                {
                    return _globalvar;
                }
                _globalvar = new Hashtable();
                return _globalvar;
            }
        }

        #endregion

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PageEngine()
        {
            _activeNode = null;
            _activeSection = null;
            _templateControl = null;
            _shouldLoadContent = true;
            _stylesheets = new Hashtable();
            _metaTags = new Hashtable();
            _navigationPath = new ArrayList();

            // Get services from the container. Ideally, it should be possible to register the aspx page in the container
            // to automatically resolve dependencies but there were memory issues with registering pages in the container.
            _moduleLoader = Container.Resolve<ModuleLoader>();
            _nodeService = Container.Resolve<INodeService>();
            _siteService = Container.Resolve<ISiteService>();
            _sectionService = Container.Resolve<ISectionService>();

            _isRedirected = false;
        }

        /// <summary>
        /// Register stylesheets.
        /// </summary>
        /// <param name="key">The unique key for the stylesheet. Note that Cuyahoga already uses 'maincss' as key.</param>
        /// <param name="absoluteCssPath">The path to the css file from the application root (starting with /).</param>
        public void RegisterStylesheet(string key, string absoluteCssPath)
        {
            if (_stylesheets[key] == null)
            {
                _stylesheets.Add(key, absoluteCssPath);
            }
        }

        /// <summary>
        /// Register a meta tag. The values can be overriden.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="content"></param>
        public void RegisterMetaTag(string name, string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                _metaTags[name] = content;
            }
        }

        public void RegisterKeywords(params string[] keywords)
        {
            if (!_metaTags.Contains("keywords"))
            {
                RegisterMetaTag("keywords", string.Join(",",keywords));
                //return;
            }
            string oldKeywords = (string) _metaTags["keywords"];
            string newKeywords = string.Empty;
            foreach (string key in keywords)
            {
                if (oldKeywords.IndexOf(key) < 0)
                {
                    newKeywords += key + ",";
                }
            }
            RegisterMetaTag("keywords",newKeywords + oldKeywords);
        }

        /// <summary>
        /// Đăng ký node với control Navigation Path
        /// </summary>
        /// <param name="name"></param>
        /// <param name="url"></param>
        public void RegisterNavigationPath(string name, string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                string[] item = new string[2];
                item[0] = name;
                item[1] = url;
                _navigationPath.Add(item);
            }
        }

        /// <summary>
        /// Đăng ký node với control Navigation Path
        /// </summary>
        /// <param name="name"></param>
        /// <param name="url"></param>
        /// <param name="enabled"></param>
        public void RegisterNavigationPath(string name, string url, bool enabled)
        {
            if (!string.IsNullOrEmpty(url))
            {
                string[] item = new string[2];
                item[0] = name;
                if (enabled)
                {
                    item[1] = url;
                }
                _navigationPath.Add(item);
            }
        }

        /// <summary>
        /// Load the content and the template as early as possible, so everything is in place before 
        /// modules handle their own ASP.NET lifecycle events.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreInit(EventArgs e)
        {
            // Load the current site
            Node entryNode = null;
            string siteUrl = UrlHelper.GetSiteUrl();
            SiteAlias currentSiteAlias = _siteService.GetSiteAliasByUrl(siteUrl);
            if (currentSiteAlias != null)
            {
                _currentSite = currentSiteAlias.Site;
                entryNode = currentSiteAlias.EntryNode;
            }
            else
            {
                _currentSite = _siteService.GetSiteBySiteUrl(siteUrl);
            }
            if (_currentSite == null)
            {
                throw new SiteNullException("No site found at " + siteUrl);
            }

            // Load the active node
            // Query the cache by SectionId, ShortDescription and NodeId.
            if (Context.Request.QueryString["SectionId"] != null)
            {
                try
                {
                    _activeSection =
                        _sectionService.GetSectionById(Int32.Parse(Context.Request.QueryString["SectionId"]));
                    _activeNode = _activeSection.Node;
                }
                catch
                {
                    throw new SectionNullException("Section not found: " + Context.Request.QueryString["SectionId"]);
                }
            }
            else if (Context.Request.QueryString["ShortDescription"] != null)
            {
                _activeNode =
                    _nodeService.GetNodeByShortDescriptionAndSite(Context.Request.QueryString["ShortDescription"],
                                                                  _currentSite);
            }
            else if (Context.Request.QueryString["NodeId"] != null)
            {
                _activeNode = _nodeService.GetNodeById(Int32.Parse(Context.Request.QueryString["NodeId"]));
            }
            else if (entryNode != null)
            {
                _activeNode = entryNode;
            }
            else
            {
                // Can't load a particular node, so the root node has to be the active node
                // Maybe we have culture information stored in a cookie, so we might need a different 
                // root Node.
                string currentCulture = _currentSite.DefaultCulture;
                if (Context.Request.Cookies["CuyahogaCulture"] != null)
                {
// ReSharper disable PossibleNullReferenceException
                    currentCulture = Context.Request.Cookies["CuyahogaCulture"].Value;
// ReSharper restore PossibleNullReferenceException
                }
                _activeNode = _nodeService.GetRootNodeByCultureAndSite(currentCulture, _currentSite);
            }
            // Raise an exception when there is no Node found. It will be handled by the global error handler
            // and translated into a proper 404.
            if (_activeNode == null)
            {
                throw new NodeNullException(
                    String.Format(
                        @"No node found with the following parameters: 
					NodeId: {0},
					ShortDescription: {1},
					SectionId: {2}"
                        , Context.Request.QueryString["NodeId"]
                        , Context.Request.QueryString["ShortDescription"]
                        , Context.Request.QueryString["SectionId"]));
            }
            _rootNode = _activeNode.NodePath[0];

            // Set culture
            // TODO: fix this because ASP.NET pages are not guaranteed to run in 1 thread (how?).
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(_activeNode.Culture);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(_activeNode.Culture);

            // Check node-level security
            if (! _activeNode.ViewAllowed(User.Identity))
            {
                throw new AccessForbiddenException("You are not allowed to view this page.");
            }            

            if (_shouldLoadContent)
            {
                LoadContent();
                LoadMenus();
            }
            base.OnPreInit(e);
        }

        /// <summary>
        /// Override lại hàm RaisePostBackEvent, chỉ gọi đến các hàm sự kiện khi trang chưa bị
        /// redirect, nếu đã redirect, bỏ qua để tiết kiệm bộ nhớ
        /// </summary>
        /// <param name="sourceControl"></param>
        /// <param name="eventArgument"></param>
        protected override void RaisePostBackEvent(IPostBackEventHandler sourceControl, string eventArgument)
        {
            if (_isRedirected == false)
            {
                try
                {
                    base.RaisePostBackEvent(sourceControl, eventArgument);
                }
                catch (Exception ex)
                {
                    logger.Error("Error in " + sourceControl + eventArgument,ex);
                }
            }
        }

        /// <summary>
        /// Use a custom HtmlTextWriter to render the page if the url is rewritten, to correct the form action.
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (_isRedirected)
            {
                return;
            }
            InsertStylesheets();
            InsertMetaTags();

            //if (Context.Items["VirtualUrl"] != null)
            //{
            //    writer = new FormFixerHtmlTextWriter(writer.InnerWriter, "", Context.Items["VirtualUrl"].ToString());
            //}
            base.Render(writer);
        }

        private void LoadContent()
        {
            // ===== Load templates  =====

            string appRoot = UrlHelper.GetApplicationPath();
            // We know the active node so the template can be loaded.
            if (_activeNode.Template != null)
            {
                string templatePath = appRoot + _activeNode.Template.Path;
                _templateControl = (BaseTemplate) LoadControl(templatePath);
                // Explicitly set the id to 'p' to save some bytes (otherwise _ctl0 would be added).
                _templateControl.ID = "p";
                _templateControl.Title = _activeNode.Site.Name + " - " + _activeNode.Title;
                // Register stylesheet that belongs to the template.
                RegisterStylesheet("maincss",
                                   appRoot + _activeNode.Template.BasePath + "/Css/" + _activeNode.Template.Css);
                //Register the metatags
                if (ActiveNode.MetaKeywords != null)
                {
                    RegisterMetaTag("keywords", ActiveNode.MetaKeywords);
                }
                else
                {
                    RegisterMetaTag("keywords", ActiveNode.Site.MetaKeywords);
                }
                if (ActiveNode.MetaDescription != null)
                {
                    RegisterMetaTag("description", ActiveNode.MetaDescription);
                }
                else
                {
                    RegisterMetaTag("description", ActiveNode.Site.MetaDescription);
                }

                // Add node vào navigation path
                if (ActiveNode!=null)
                {
                    foreach (Node node in ActiveNode.NodePath)
                    {
                        RegisterNavigationPath(node.Title, UrlHelper.GetUrlFromNode(node), node.Sections.Count > 0);
                    }
                }

                // Load sections that are related to the template
                foreach (DictionaryEntry sectionEntry in ActiveNode.Template.Sections)
                {
                    string placeholder = sectionEntry.Key.ToString();
                    Section section = sectionEntry.Value as Section;
                    if (section != null)
                    {
                        BaseModuleControl moduleControl = CreateModuleControlForSection(section);
                        if (moduleControl != null)
                        {
                            ((PlaceHolder) _templateControl.Containers[placeholder]).Controls.Add(moduleControl);
                        }
                    }
                }
            }
            else
            {
                throw new Exception("No template associated with the current Node.");
            }

            // ===== Load sections and modules =====
            foreach (Section section in _activeNode.Sections)
            {
                BaseModuleControl moduleControl = CreateModuleControlForSection(section);
                if (moduleControl != null)
                {
                    ((PlaceHolder) _templateControl.Containers[section.PlaceholderId]).Controls.Add(moduleControl);
                }
            }

            Controls.AddAt(0, _templateControl);
            // remove html that was in the original page (Default.aspx)
            for (int i = Controls.Count - 1; i < 0; i --)
                Controls.RemoveAt(i);
        }

        private BaseModuleControl CreateModuleControlForSection(Section section)
        {
            // Check view permissions before adding the section to the page.
            if (section.ViewAllowed(User.Identity))
            {
                // Create the module that is connected to the section.
                ModuleBase module = _moduleLoader.GetModuleFromSection(section);                
                //this._moduleLoader.NHibernateModuleAdded -= new EventHandler(ModuleLoader_ModuleAdded);

                if (module != null)
                {                    
                    if (Context.Request.PathInfo.Length > 0 && section == _activeSection)
                    {
                        // Parse the PathInfo of the request because they can be the parameters 
                        // for the module that is connected to the active section.
                        module.ModulePathInfo = Context.Request.PathInfo;
                    }
                    module.SaveGlobalSettings(this);
                    return LoadModuleControl(module);
                }
            }
            return null;
        }

        private BaseModuleControl LoadModuleControl(ModuleBase module)
        {
            BaseModuleControl ctrl =
                (BaseModuleControl) LoadControl(UrlHelper.GetApplicationPath() + module.CurrentViewControlPath);
            ctrl.Module = module;
            return ctrl;
        }

        private void LoadMenus()
        {
            IList menus = _nodeService.GetMenusByRootNode(_rootNode);
            foreach (CustomMenu menu in menus)
            {
                PlaceHolder plc = _templateControl.Containers[menu.Placeholder] as PlaceHolder;
                if (plc != null)
                {
                    // rabol: [#CUY-57] fix.
                    Control menuControlList = GetMenuControls(menu);
                    if (menuControlList != null)
                    {
                        plc.Controls.Add(menuControlList);
                    }
                }
            }
        }

        private Control GetMenuControls(CustomMenu menu)
        {
            if (menu.Nodes.Count > 0)
            {
                // The menu is just a simple <ul> list.
                HtmlGenericControl listControl = new HtmlGenericControl("ul");
                foreach (Node node in menu.Nodes)
                {
                    if (node.ViewAllowed(CuyahogaUser))
                    {
                        HtmlGenericControl listItem = new HtmlGenericControl("li");
                        HyperLink hpl = new HyperLink();
                        hpl.NavigateUrl = UrlHelper.GetUrlFromNode(node);
                        UrlHelper.SetHyperLinkTarget(hpl, node);
                        hpl.Text = node.Title;
                        listItem.Controls.Add(hpl);
                        listControl.Controls.Add(listItem);
                        if (node.Id == ActiveNode.Id)
                        {
                            hpl.CssClass = "selected";
                        }
                    }
                }
                return listControl;
            }
            return null;
        }

        private void InsertStylesheets()
        {
            string[] stylesheetLinks = new string[_stylesheets.Count];
            int i = 0;
            foreach (string stylesheet in _stylesheets.Values)
            {
                stylesheetLinks[i] = stylesheet;
                i++;
            }
            TemplateControl.RenderCssLinks(stylesheetLinks);
        }

        private void InsertMetaTags()
        {
            TemplateControl.RenderMetaTags(_metaTags);
        }

        #region -- PRIVATE METHODS --

        /// <summary>
        /// Redirect đến trang khác, sử dụng phương pháp tiết kiệm tài nguyên hơn Response.Redirect
        /// </summary>
        /// <param name="url">Đường dẫn cần redirect tới</param>
        public void PageRedirect(string url)
        {
            Response.Redirect(url, false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
            _isRedirected = true;
        }

        #endregion
    }
}