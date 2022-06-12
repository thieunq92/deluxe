using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Web;
using Castle.Facilities.NHibernateIntegration;
using Castle.Services.Transaction;
using ImageHelper;
using CMS.Core;
using CMS.Core.Communication;
using CMS.Core.Domain;
using CMS.Core.Service.Files;
using CMS.Modules.Gallery.Domain;
using CMS.Web.Util;
using NHibernate;
using NHibernate.Criterion;

namespace CMS.Modules.Gallery
{
    /// <summary>
    /// The Gallery Module provides photo gallery for users.
    /// </summary>
    [Transactional]
    public class GalleryModule : ModuleBase, INHibernateModule, IActionConsumer, IActionProvider
    {
        public static string BACK = "Back";
        private readonly IFileService _fileService;
        private readonly GalleryPathBuilder _galleryPathBuilder;
        private readonly ActionCollection _inboundActions;
        private readonly ISessionManager _sessionManager;
        private AlbumService _albumService;
        private AlbumSettings _albumSettings;
        private CMS.Core.Communication.Action _currentAction;

        private int _currentAlbumId;
        private GalleryModuleActions _currentGalleryAction;
        private int _currentPhotoId;
        private string _gallerythemepath;

        private PhotoService _photoService;

        /// <summary>
        /// GalleryModule constructor.
        /// </summary>
        /// <param name="fileService">FileService dependency.</param>
        /// <param name="sessionManager">NHibernate session manager dependency.</param>
        public GalleryModule(IFileService fileService, ISessionManager sessionManager)
        {
            _fileService = fileService;
            _sessionManager = sessionManager;
            _gallerythemepath = UrlHelper.GetApplicationPath() + "Modules/Gallery/Css/";
            _galleryPathBuilder = new GalleryPathBuilder(fileService);

            // Init inbound actions
            _inboundActions = new ActionCollection();
            _inboundActions.Add(new CMS.Core.Communication.Action("Album", new string[0]));
            _currentAction = _inboundActions[0];
        }

        public AlbumSettings AlbumSettings
        {
            get { return _albumSettings; }
        }

        public GalleryPathBuilder PathBuilder
        {
            get { return _galleryPathBuilder; }
        }

        public IFileService FileService
        {
            get { return _fileService; }
        }

        public new ISessionManager SessionManager
        {
            get { return _sessionManager; }
        }

        /// <summary>
        /// The current view user control based on the action that was set while parsing the pathinfo.
        /// </summary>
        public override string CurrentViewControlPath
        {
            get
            {
                string basePath = "Modules/Gallery/";
                switch (_currentGalleryAction)
                {
                    case GalleryModuleActions.Albums:
                        return basePath + "Albums.ascx";
                    case GalleryModuleActions.BrowseAlbum:
                        if (GetAlbumService().GetAlbumById(CurrentAlbumId).UseSimpleViewer)
                        return basePath + "SimpleViewer.ascx";                        
                        return basePath + "Photos.ascx";
                    case GalleryModuleActions.ViewPhoto:
                        return basePath + "View.ascx";
                    default:
                        return basePath + "Albums.ascx";
                }
            }
        }

        #region properties

        /// <summary>
        /// Return the module's current Album id.
        /// </summary>
        public int CurrentAlbumId
        {
            get { return _currentAlbumId; }
        }

        /// <summary>
        /// Return the module's current Photo id.
        /// </summary>
        public int CurrentPhotoId
        {
            get { return _currentPhotoId; }
        }

        /// <summary>
        /// A specific action that has to be done by the module.
        /// </summary>
        public GalleryModuleActions CurrentAction
        {
            get { return _currentGalleryAction; }
        }

        public string ThemePath
        {
            get { return _gallerythemepath; }
            set { _gallerythemepath = value; }
        }

        #endregion

        #region IActionConsumer Members

        public ActionCollection GetInboundActions()
        {
            return _inboundActions;
        }

        #endregion

        #region IActionProvider Members

        public ActionCollection GetOutboundActions()
        {
            ActionCollection _outboundActions = new ActionCollection();
            _outboundActions.Add(new CMS.Core.Communication.Action(BACK, new string[0]));
            return _outboundActions;
        }

        #endregion

        public AlbumService GetAlbumService()
        {
            if (_albumService == null)
                _albumService = new AlbumService(this);

            return _albumService;
        }

        public PhotoService GetPhotoService()
        {
            if (_photoService == null)
                _photoService = new PhotoService(this);

            return _photoService;
        }


        /// <summary>
        /// Read the settings set for this Section in the Administration interface
        /// </summary>
        public override void ReadSectionSettings()
        {
            base.ReadSectionSettings();

            // Set dynamic module settings
            string physicalDir = Convert.ToString(Section.Settings["PHYSICAL_DIR"]);

            if (physicalDir != String.Empty)
                _galleryPathBuilder.PhysicalDir = physicalDir;


            _albumSettings = new AlbumSettings();

            _albumSettings.ShowNumberOfViews = Convert.ToBoolean(Section.Settings["SHOW_VIEWS"]);
            _albumSettings.NumberOfColumns = Convert.ToInt32(Section.Settings["NUMBER_OF_COLUMNS"]);
            _albumSettings.NumberOfItemsOnPage = Convert.ToInt32(Section.Settings["NUMBER_OF_ITEMS_ON_PAGE"]);
            _albumSettings.MaxImageDimension = Convert.ToInt32(Section.Settings["MAX_IMAGE_DIMENSION"]);
            _albumSettings.MaxThumbDimension = Convert.ToInt32(Section.Settings["MAX_THUMB_DIMENSION"]);
            _albumSettings.ShowGraphicalButtonsInViewer = true; // unused for now
        }

        /// <summary>
        /// Validate module settings.
        /// </summary>
        public override void ValidateSectionSettings()
        {
            // Check if the virtual directory exists.
            // We need to do this based on the section setting because it might be possible that the related section 
            // isn't saved yet.
            if (Section != null)
            {
                string physicalDir = Convert.ToString(Section.Settings["PHYSICAL_DIR"]);
                if (!String.IsNullOrEmpty(physicalDir))
                {
                    physicalDir = HttpContext.Current.Server.MapPath(physicalDir);
                    _galleryPathBuilder.CheckPhysicalDirectory(physicalDir);
                }

                int maxImageWidth = Convert.ToInt32(Section.Settings["MAX_IMAGE_DIMENSION"]);
                int maxThumbWidth = Convert.ToInt32(Section.Settings["MAX_THUMB_DIMENSION"]);
                ResizeAllImages(maxImageWidth, maxThumbWidth);
            }

            base.ValidateSectionSettings();
        }

        /// <summary>
        /// On settings changes
        /// </summary>
        public void ResizeAllImages(int maxImageWidth, int maxThumbWidth)
        {
            _albumService = GetAlbumService();
            _photoService = GetPhotoService();

            foreach (Album album in _albumService.GetAlbumList(false))
            {
                foreach (Photo photo in _photoService.GetPhotoList(album.Id))
                {
                    _fileService.DeleteFile(_galleryPathBuilder.GetThumbPath(photo));
                    //_fileService.DeleteFile(_galleryPathBuilder.GetPath(photo));

                    Image image = Image.FromStream(_fileService.ReadFile(_galleryPathBuilder.GetPhotoOriginPath(photo)));

                    ImageResizeUtil imageResizeUtilThumb = new ImageResizeUtil(image, maxThumbWidth);
                    //ImageResizeUtil imageResizeUtil = new ImageResizeUtil(image, maxImageWidth);

                    //_photoService.ResizeImage(photo, imageResizeUtil);
                    _photoService.ResizeThumbnail(photo, imageResizeUtilThumb);

                    _photoService.SavePhotoInfo(photo);
                }
            }
        }

        protected override void ParsePathInfo()
        {
            base.ParsePathInfo();

            if (ModuleParams != null)
            {
                try
                {
                    // try to find a albumid/photoid
                    string expression = @"^\/(\d+)\/(\d+)";
                    if (Regex.IsMatch(ModulePathInfo, expression,
                                      RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled))
                    {
                        _currentAlbumId = Int32.Parse(Regex.Match(ModulePathInfo, expression).Groups[1].Value);
                        _currentPhotoId = Int32.Parse(Regex.Match(ModulePathInfo, expression).Groups[2].Value);
                        _currentGalleryAction = GalleryModuleActions.ViewPhoto;
                        return;
                    }

                    // try to find a albumyid
                    expression = @"^\/album\/(\d+)";
                    if (Regex.IsMatch(ModulePathInfo, expression,
                                      RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled))
                    {
                        _currentAlbumId = Int32.Parse(Regex.Match(ModulePathInfo, expression).Groups[1].Value);
                        _currentGalleryAction = GalleryModuleActions.BrowseAlbum;
                    }
                }

                catch (ArgumentException ex)
                {
                    throw new Exception("Error when parsing module action: " + ModuleParams[0], ex);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error when parsing module parameters: " + ModulePathInfo, ex);
                }
            }
        }

        /// <summary>
        /// Construct a webserver virtual path based on a physical directory
        /// </summary>
        /// <param name="physicalDir">A physical directory.</param>
        /// <returns>The virtual path.</returns>
        public string VirtualPath(string physicalDir)
        {
            // this is no doubt a very, very (very!) bad way to do this
            string TempPath = physicalDir.Trim().ToLower();
            string AppPath = HttpContext.Current.Request.PhysicalApplicationPath;
            TempPath = TempPath.Replace(AppPath.ToLower(), "");
            TempPath = "~/" + TempPath.Replace("\\", "/");

            return TempPath;
        }

        public bool CheckPhoto(string name)
        {
            if (name.Contains("-origin.jpg"))
                return false;
            if (name.Contains("-thumb.jpg"))
                return false;
            ICriteria criteria = _sessionManager.OpenSession().CreateCriteria(typeof (Photo))
                .Add(Expression.Eq("FileName", PhotoService.CreateServerFilename(name)))
                .SetMaxResults(1);
            return criteria.List().Count == 0;
        }
    }

    public enum GalleryModuleActions
    {
        Albums,
        BrowseAlbum,
        ViewPhoto
    }
}