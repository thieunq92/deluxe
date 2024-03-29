using System;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CMS.Core.Util;
using CMS.Modules.Gallery.Domain;
using CMS.ServerControls;
using CMS.Web.UI;
using log4net;

namespace CMS.Modules.Gallery.Web
{
    public class AdminGallery : ModuleAdminBasePage
    {
        private AlbumService _albumService;
        private GalleryModule _galleryModule;
        protected HtmlInputButton btnNew;
        protected Button btnSearch;

        protected HtmlForm Form1;
        protected Pager pgrAlbums;
        protected Repeater rptAlbums;
        protected TextBox textBoxTitle;
        private readonly ILog _logger = LogManager.GetLogger(typeof (AdminGallery));

        protected void Page_Load(object sender, EventArgs e)
        {
            // The base page has already created the module, we only have to cast it here to the right type.
            _galleryModule = base.Module as GalleryModule;

            _albumService = _galleryModule.GetAlbumService();

            btnNew.Attributes.Add("onclick",
                                  String.Format("document.location.href='AdminAlbum.aspx{0}&AlbumId=-1'",
                                                base.GetBaseQueryString()));

            if (! IsPostBack)
            {
                textBoxTitle.Text = Request.QueryString["title"];
                rptAlbums.DataSource = _albumService.GetAlbumList(false, Request.QueryString["title"]);
                rptAlbums.DataBind();
            }

            DefaultButton.SetDefault(Page, textBoxTitle, btnSearch);
        }

        private void rptAlbums_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Album album = e.Item.DataItem as Album;

            CheckBox chkActive = e.Item.FindControl("chkActive") as CheckBox;
            if (chkActive != null)
            {
                chkActive.Checked = album.Active;
                chkActive.Enabled = false;
            }

            HyperLink hplEdit = e.Item.FindControl("hpledit") as HyperLink;
            if (hplEdit != null)
            {
                hplEdit.NavigateUrl = String.Format("~/Modules/Gallery/AdminAlbum.aspx{0}&AlbumId={1}",
                                                    base.GetBaseQueryString(), album.Id);
            }

            HyperLink hplPhotos = e.Item.FindControl("hplphotos") as HyperLink;
            if (hplPhotos != null)
            {
                hplPhotos.NavigateUrl = String.Format("~/Modules/Gallery/AdminPhotos.aspx{0}&AlbumId={1}",
                                                      base.GetBaseQueryString(), album.Id);
            }

            Literal litDateModified = e.Item.FindControl("litDateModified") as Literal;
            if (litDateModified != null)
            {
                litDateModified.Text =
                    TimeZoneUtil.AdjustDateToUserTimeZone(album.UpdateTimestamp, User.Identity).ToString();
            }

            LinkButton lbtConvert = e.Item.FindControl("lbtConvert") as LinkButton;
            if (lbtConvert!=null)
            {
                if (album.UseSimpleViewer)
                {
                    lbtConvert.Enabled = false;
                    lbtConvert.Text = "Compatible";
                }
            }
        }

        private void pgrAlbums_PageChanged(object sender, PageChangedEventArgs e)
        {
            rptAlbums.DataBind();
        }

        protected void pgrAlbums_CacheEmpty(object sender, EventArgs e)
        {
            rptAlbums.DataSource = _albumService.GetAlbumList(false, Request.QueryString["title"]);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("AdminGallery.aspx?NodeId={0}&SectionId={1}&title={2}", Node.Id, Section.Id,
                                            Server.UrlEncode(textBoxTitle.Text)));
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
            this.rptAlbums.ItemDataBound +=
                new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rptAlbums_ItemDataBound);
            this.pgrAlbums.PageChanged += new CMS.ServerControls.PageChangedEventHandler(this.pgrAlbums_PageChanged);
        }

        #endregion

        protected void btnConvertGallery_Click(object sender, EventArgs e)
        {
            foreach (Album album in _albumService.GetAlbumList(false))
            {
                if (!album.UseSimpleViewer)
                {
                    _logger.Error("Processing album id="+album.Id);
                    ConvertAlbum(album);
                }
            }
        }

        protected void ConvertAlbum(Album album)
        {
            // Tạo thư mục thumbnail
            DirectoryInfo thumb= new DirectoryInfo(_galleryModule.PathBuilder.GetAlbumDirectory(album.Id)+"\\thumbnails");
            if (!thumb.Exists)
            {
                thumb.Create();
            }            
                
            foreach (Photo photo in album.Photos)
            {
                // Tạo thumbnail cho photo (copy từ thư mục gốc sang                    
                try
                {
                    FileInfo file = new FileInfo(_galleryModule.PathBuilder.GetOldThumbPath(photo));
                    file.MoveTo(thumb.FullName+@"\"+photo.FileName);
                }
                catch (FileNotFoundException)
                {
                    //_galleryModule.GetPhotoService().DeletePhoto(photo);
                    continue;
                }

                // Xóa trên thư mục gốc
                try
                {
                    FileInfo view = new FileInfo(_galleryModule.PathBuilder.GetPath(photo));
                    view.Delete();
                }
                catch (FileNotFoundException)
                {}
                catch(Exception ex)
                {
                    if (!(ex is FileNotFoundException))
                    {
                        ShowException(ex);
                        _logger.Error("Convert album failed",ex);
                        return;
                    }
                }

                // Đổi tên từ origin thành tên gốc
                try
                {
                    FileInfo origin = new FileInfo(_galleryModule.PathBuilder.GetOldPhotoOriginPath(photo));
                    origin.MoveTo(_galleryModule.PathBuilder.GetPath(photo));
                }
                catch (FileNotFoundException)
                {}
                catch (Exception ex)
                {
                    if (!(ex is FileNotFoundException))
                    {
                        ShowException(ex);
                        _logger.Error("Convert album failed", ex);
                        return;
                    }
                }
            }

            album.UseSimpleViewer = true;
            _galleryModule.GetAlbumService().SaveAlbumInfo(album);
            _logger.Error("Album converted successully");
        }

        protected void rptAlbums_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            Album album = _galleryModule.GetAlbumService().GetAlbumById(Convert.ToInt32(e.CommandArgument));
            ConvertAlbum(album);
        }
    }
}