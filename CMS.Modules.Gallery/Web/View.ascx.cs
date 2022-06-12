using System;
using System.Collections;
using System.Web.UI.WebControls;
using CMS.Modules.Gallery.Domain;
using CMS.Modules.Gallery.Web.UI;
using CMS.Web.Util;

namespace CMS.Modules.Gallery.Web
{
    public class View : BaseGalleryControl
    {
        private Album _album;
        private int _initialPhoto;
        private IList _photolist;
        protected ImageButton FirstImageButton1;
        protected ImageButton FirstImageButton2;
        protected HyperLink hplBack;
        protected HyperLink hplGalleryImage1;
        protected HyperLink hplGalleryImage2;

        protected Image imPhoto;
        protected ImageButton LastImageButton1;
        protected ImageButton LastImageButton2;
        protected Literal litTitle;
        protected ImageButton NextImageButton1;
        protected ImageButton NextImageButton2;
        protected Repeater PhotoDetailsRepeater;

        protected ImageButton PreviousImageButton1;
        protected ImageButton PreviousImageButton2;

        public Album Album
        {
            get { return _album; }
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.PhotoDetailsRepeater.ItemDataBound +=
                new System.Web.UI.WebControls.RepeaterItemEventHandler(this.PhotoDetailsRepeater_ItemDataBound);

            //FirstImageButton1.Click += new System.Web.UI.ImageClickEventHandler(FirstImageButton1_Click);
            this.FirstImageButton1.Click += new System.Web.UI.ImageClickEventHandler(this.cmdFirst_Click);
            this.PreviousImageButton1.Click += new System.Web.UI.ImageClickEventHandler(this.cmdPrev_Click);
            this.NextImageButton1.Click += new System.Web.UI.ImageClickEventHandler(this.cmdNext_Click);
            this.LastImageButton1.Click += new System.Web.UI.ImageClickEventHandler(this.cmdLast_Click);

            this.FirstImageButton2.Click += new System.Web.UI.ImageClickEventHandler(this.cmdFirst_Click);
            this.PreviousImageButton2.Click += new System.Web.UI.ImageClickEventHandler(this.cmdPrev_Click);
            this.NextImageButton2.Click += new System.Web.UI.ImageClickEventHandler(this.cmdNext_Click);
            this.LastImageButton2.Click += new System.Web.UI.ImageClickEventHandler(this.cmdLast_Click);

            this.Load += new System.EventHandler(this.Page_Load);
        }

        private void FirstImageButton1_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Write("hello");
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            _album = AlbumService.GetAlbumById(base.GalleryModule.CurrentAlbumId);

            // build in check to see if photo to load is part of an active album

            // deal with the Photo.Id passed on by the Module base ParsePathInfo() function
            if (!Page.IsPostBack)
            {
                _initialPhoto = base.GalleryModule.CurrentPhotoId;
            }
            else
            {
                _initialPhoto = 0;
            }

            if ((!base.HasCachedOutput || Page.IsPostBack) || Page.User.Identity.IsAuthenticated)
            {
                BindRepeater();

                // use the stored current photo page for the "back" link, if set
                string _photoPage = "";
                if (CurrentPhotoPage > 0)
                {
                    _photoPage = "/page/" + CurrentPhotoPage;
                }

                // set links of 2 "back" buttons
                hplGalleryImage1.NavigateUrl = UrlHelper.GetUrlFromSection(base.GalleryModule.Section) + "/album/" +
                                               (base.GalleryModule.CurrentAlbumId) + _photoPage;
                hplGalleryImage2.NavigateUrl = UrlHelper.GetUrlFromSection(base.GalleryModule.Section) + "/album/" +
                                               (base.GalleryModule.CurrentAlbumId) + _photoPage;

                // possibly add text buttons in the future
                if (base.GalleryModule.AlbumSettings.ShowGraphicalButtonsInViewer)
                {
                    // set the images for the view pager buttons
                    hplGalleryImage1.ImageUrl = ResolveUrl("~/Modules/Gallery/images/photogallery/button-gallery.gif");
                    FirstImageButton1.ImageUrl = ResolveUrl("~/Modules/Gallery/images/photogallery/button-first.gif");
                    PreviousImageButton1.ImageUrl = ResolveUrl("~/Modules/Gallery/images/photogallery/button-prev.gif");
                    NextImageButton1.ImageUrl = ResolveUrl("~/Modules/Gallery/images/photogallery/button-next.gif");
                    LastImageButton1.ImageUrl = ResolveUrl("~/Modules/Gallery/images/photogallery/button-last.gif");

                    hplGalleryImage2.ImageUrl = ResolveUrl("~/Modules/Gallery/images/photogallery/button-gallery.gif");
                    FirstImageButton2.ImageUrl = ResolveUrl("~/Modules/Gallery/images/photogallery/button-first.gif");
                    PreviousImageButton2.ImageUrl = ResolveUrl("~/Modules/Gallery/images/photogallery/button-prev.gif");
                    NextImageButton2.ImageUrl = ResolveUrl("~/Modules/Gallery/images/photogallery/button-next.gif");
                    LastImageButton2.ImageUrl = ResolveUrl("~/Modules/Gallery/images/photogallery/button-last.gif");

                    // set the texts for the buttons
                    hplGalleryImage1.ToolTip = base.GetText("strBack");
                    hplGalleryImage1.Attributes["alt"] = base.GetText("strBack");

                    FirstImageButton1.ToolTip = base.GetText("strFirstPhoto");
                    PreviousImageButton1.ToolTip = base.GetText("strPreviousPhoto");
                    NextImageButton1.ToolTip = base.GetText("strNextPhoto");
                    LastImageButton1.ToolTip = base.GetText("strLastPhoto");

                    hplGalleryImage2.ToolTip = base.GetText("strBack");
                    hplGalleryImage2.Attributes["alt"] = base.GetText("strBack");

                    FirstImageButton2.ToolTip = base.GetText("strFirstPhoto");
                    PreviousImageButton2.ToolTip = base.GetText("strPreviousPhoto");
                    NextImageButton2.ToolTip = base.GetText("strNextPhoto");
                    LastImageButton2.ToolTip = base.GetText("strLastPhoto");
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (Module.Section.Node.Id == PageEngine.ActiveNode.Id)
            {
                string[] item = new string[2];
                item[0] = Album.Title;
                item[1] = string.Format("{0}/album/{1}", UrlHelper.GetUrlFromSection(Module.Section), Album.Id);
                PageEngine.NavigationPath.Add(item);
            }

            base.OnPreRender(e);
        }

        private void BindRepeater()
        {
            PagedDataSource objPds = new PagedDataSource();

            _photolist = base.PhotoService.GetPhotoList(base.GalleryModule.CurrentAlbumId);
            objPds.DataSource = _photolist;

            // clumsy way to get the right photo in the list from the id in the link
            if (_initialPhoto > 0)
            {
                foreach (Photo p in _photolist)
                {
                    if (p.Id == _initialPhoto)
                    {
                        CurrentPage = _photolist.IndexOf(p);
                    }
                }
            }

            // set up this pager
            objPds.AllowPaging = true;
            objPds.PageSize = 1;
            objPds.CurrentPageIndex = CurrentPage;
            UpperBounds = objPds.PageCount - 1;

            // enable/disable buttons based on current image
            FirstImageButton1.Visible = !objPds.IsFirstPage;
            FirstImageButton2.Visible = !objPds.IsFirstPage;
            PreviousImageButton1.Visible = !objPds.IsFirstPage;
            PreviousImageButton2.Visible = !objPds.IsFirstPage;

            NextImageButton1.Visible = !objPds.IsLastPage;
            NextImageButton2.Visible = !objPds.IsLastPage;
            LastImageButton1.Visible = !objPds.IsLastPage;
            LastImageButton2.Visible = !objPds.IsLastPage;

            PhotoDetailsRepeater.DataSource = objPds;
            PhotoDetailsRepeater.DataBind();
        }


        private void PhotoDetailsRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            Photo photo = e.Item.DataItem as Photo;
            if (photo == null) return;

            Literal litImage = e.Item.FindControl("litImage") as Literal;
            if (litImage != null)
            {
                litImage.Text = "<img";
                litImage.Text += " src=\"" + base.Page.ResolveUrl(
                                                 base.GalleryModule.VirtualPath(
                                                     base.GalleryModule.PathBuilder.GetPath(photo))) + "\"";
                litImage.Text += " width=\"" + photo.ImageWidth + "\"";
                litImage.Text += " height=\"" + photo.ImageHeight + "\"";
                litImage.Text += " alt=\"" + photo.Size + " " + base.GetText("strBytes") + "\"";
                litImage.Text += " class=\"photo_198\"";
                litImage.Text += " style=\"border:4px solid white\"";
                litImage.Text += " />";
            }

            litTitle = e.Item.FindControl("litTitle") as Literal;

            if (litTitle != null)
            {
                litTitle.Text = photo.DisplayTitle;
                litTitle.Text += " (" + Convert.ToString(CurrentPage + 1) + '/' + _photolist.Count + ")";
            }

            using (HyperLink hplDownload = e.Item.FindControl("hplDownload") as HyperLink)
            {
                if (hplDownload != null)
                {
                    hplDownload.NavigateUrl = base.Page.ResolveUrl(GalleryModule.VirtualPath(GalleryModule.PathBuilder.GetPhotoOriginPath(photo)));
                }
            }

            base.PhotoService.IncreaseNrOfViews(photo);
        }

        #region buttonevents

        private void cmdFirst_Click(object sender, EventArgs e)
        {
            CurrentPage = 0;
            BindRepeater();
        }

        private void cmdPrev_Click(object sender, EventArgs e)
        {
            CurrentPage -= 1;
            BindRepeater();
        }

        private void cmdNext_Click(object sender, EventArgs e)
        {
            CurrentPage += 1;
            BindRepeater();
        }

        private void cmdLast_Click(object sender, EventArgs e)
        {
            CurrentPage = UpperBounds;
            BindRepeater();
        }

        #endregion

        #region properties

        public virtual int CurrentPage
        {
            get
            {
                if (ViewState["_CurrentPage"] != null)
                {
                    return Convert.ToInt16(ViewState["_CurrentPage"]);
                }
                else
                {
                    return 0;
                }
            }

            set { ViewState["_CurrentPage"] = value; }
        }

        public virtual int UpperBounds
        {
            get
            {
                if (ViewState["_UpperBounds"] != null)
                {
                    return Convert.ToInt16(ViewState["_UpperBounds"]);
                }
                else
                {
                    return 0;
                }
            }

            set { ViewState["_UpperBounds"] = value; }
        }

        #endregion
    }
}