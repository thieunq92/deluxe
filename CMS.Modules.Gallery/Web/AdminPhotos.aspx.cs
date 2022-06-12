using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CMS.Core.Util;
using CMS.Modules.Gallery.Domain;
using CMS.Web.UI;

namespace CMS.Modules.Gallery.Web
{
    /// <summary>
    /// Summary description for EditDownloads.
    /// </summary>
    public class AdminPhotos : ModuleAdminBasePage
    {
        private int _albumid;
        private GalleryModule _galleryModule;
        private PhotoService _photoService;

        protected Button btnBack;
        protected HtmlInputButton btnNew;
        protected Repeater rptPhotos;

        private void Page_Load(object sender, EventArgs e)
        {
            // The base page has already created the module, we only have to cast it here to the right type.
            _galleryModule = Module as GalleryModule;

            _photoService = _galleryModule.GetPhotoService();

            if (Request.QueryString["AlbumId"] != null)
            {
                _albumid = Int32.Parse(Request.QueryString["AlbumId"]);
            }

            if (_albumid > 0)
            {
                btnNew.Attributes.Add("onclick",
                                      String.Format(
                                          "document.location.href='AdminPhoto.aspx{0}&AlbumId={1}&PhotoId=-1'",
                                          GetBaseQueryString(), _albumid));

                if (!IsPostBack)
                {
                    BindFiles();
                }
            }
        }

        private void BindFiles()
        {
            rptPhotos.DataSource = _photoService.GetPhotoList(_albumid);
            rptPhotos.DataBind();
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Context.Response.Redirect("AdminGallery.aspx" + GetBaseQueryString());
        }

        private void rptPhotos_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Photo photo = (Photo) e.Item.DataItem;

            Literal litDateModified = e.Item.FindControl("litDateModified") as Literal;
            if (litDateModified != null)
            {
                litDateModified.Text =
                    TimeZoneUtil.AdjustDateToUserTimeZone(photo.DateModified, User.Identity).ToString();
            }

            HyperLink hplEdit = e.Item.FindControl("hpledit") as HyperLink;
            if (hplEdit != null)
            {
                hplEdit.NavigateUrl = String.Format("~/Modules/Gallery/AdminPhoto.aspx{0}&AlbumId={1}&PhotoId={2}",
                                                    GetBaseQueryString(), photo.Album.Id, photo.Id);
            }


            Literal litThumb = e.Item.FindControl("litThumb") as Literal;
            if (litThumb != null)
            {
                litThumb.Text = "<img";
                litThumb.Text += " src=\"" + base.Page.ResolveUrl(
                                                 _galleryModule.VirtualPath(
                                                     _galleryModule.PathBuilder.GetThumbPath(photo))) + "\"";
                // half sized thumbs the dirty way
                litThumb.Text += " width=\"" + Convert.ToInt16(photo.ThumbWidth/2) + "\"";
                litThumb.Text += " height=\"" + Convert.ToInt16(photo.ThumbHeight/2) + "\"";
                litThumb.Text += " />";
            }
        }

        protected void rptPhotos_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            Photo photo;
            Photo switchedPhoto;
            //HiddenField hiddenOrder;
            HiddenField hiddenId;
            switch (e.CommandName.ToLower())
            {
                case "up":
                    if (e.Item.ItemIndex == 0)
                    {
                        return;
                    }
                    //hiddenOrder = rptPhotos.Items[e.Item.ItemIndex].FindControl("hiddenOrder") as HiddenField;
                    hiddenId = (HiddenField) rptPhotos.Items[e.Item.ItemIndex-1].FindControl("hiddenId");
                    switchedPhoto = _photoService.GetPhotoById(Convert.ToInt32(hiddenId.Value));
                    photo = _photoService.GetPhotoById(Convert.ToInt32(e.CommandArgument));
                    photo.Order = switchedPhoto.Order + photo.Order;
                    switchedPhoto.Order = photo.Order - switchedPhoto.Order;
                    photo.Order = photo.Order - switchedPhoto.Order;                    
                    _photoService.SavePhotoInfo(photo);
                    _photoService.SavePhotoInfo(switchedPhoto);
                    BindFiles();
                    break;
                case "down":
                    if (e.Item.ItemIndex == rptPhotos.Items.Count - 1)
                    {
                        return;
                    }
                    //hiddenOrder = rptPhotos.Items[e.Item.ItemIndex].FindControl("hiddenOrder") as HiddenField;
                    hiddenId = (HiddenField)rptPhotos.Items[e.Item.ItemIndex + 1].FindControl("hiddenId");
                    switchedPhoto = _photoService.GetPhotoById(Convert.ToInt32(hiddenId.Value));
                    photo = _photoService.GetPhotoById(Convert.ToInt32(e.CommandArgument));
                    photo.Order = switchedPhoto.Order + photo.Order;
                    switchedPhoto.Order = photo.Order - switchedPhoto.Order;
                    photo.Order = photo.Order - switchedPhoto.Order;
                    _photoService.SavePhotoInfo(photo);
                    _photoService.SavePhotoInfo(switchedPhoto);
                    BindFiles();
                    break;
            }
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
            this.rptPhotos.ItemDataBound +=
                new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rptPhotos_ItemDataBound);
            this.Load += new System.EventHandler(this.Page_Load);
        }

        #endregion
    }
}