using System;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CMS.Core.Domain;
using CMS.Modules.Gallery.Domain;
using CMS.ServerControls.FileUpload;
using CMS.Web.UI;
using CMS.Web.Util;
using FredCK.FCKeditorV2;

namespace CMS.Modules.Gallery.Web
{
    public class AdminAlbum : ModuleAdminBasePage
    {
        private Album _album;

        private AlbumService _albumService;
        private GalleryModule _galleryModule;
        private PhotoService _photoService;
        protected HtmlInputButton btnCancel;

        protected Button btnDelete;
        protected Button btnMassImport;
        protected Button btnSave;
        protected CheckBox chkActive;

        protected HtmlGenericControl divFastImport;
        protected HtmlForm Form1;
        protected RequiredFieldValidator rfvTitle;
        protected FCKeditor fckEditorDescription;
        protected TextBox txtTitle;
        protected TextBox txtAlbumId;
        protected FileUploaderAJAX FileUploaderAJAXPhotos;


        protected void Page_Load(object sender, EventArgs e)
        {
            _galleryModule = Module as GalleryModule;
            if (_galleryModule==null)
            {
                _galleryModule = _moduleLoader.GetModuleFromClassName("CMS.Modules.Gallery.GalleryModule") as GalleryModule;
            }
            _albumService = _galleryModule.GetAlbumService();
            _photoService = _galleryModule.GetPhotoService();

            if (Module != null)
            {
                btnCancel.Attributes.Add("onclick",
                                         String.Format("document.location.href='AdminGallery.aspx{0}'",
                                                       GetBaseQueryString()));
                btnMassImport.Click += btnMassImport_Click;

                if (Request.QueryString["AlbumId"] == null)
                    return;

                int albumId = Int32.Parse(Request.QueryString["AlbumId"]);

                if (albumId == 0 || albumId == -1)
                    return;

                _album = _albumService.GetAlbumById(albumId);

                if (!IsPostBack)
                    BindAlbum();

                btnDelete.Visible = true;
                btnDelete.Attributes.Add("onclick", "return confirm('Are you sure?');");

                if (!IsPostBack)
                    DisplayFastImportPanel();

                if (_album.Id > 0)
                {
                    FileUploaderAJAXPhotos.Visible = true;
                }
                else
                {
                    FileUploaderAJAXPhotos.Visible = false;
                }
                Session["GalleryPath"] = _galleryModule.PathBuilder.GetAlbumVirtualDirectory(_album.Id);
            }

            if (FileUploaderAJAXPhotos.IsPosting && Session["GalleryPath"]!=null)
            {
                FileHelper.ManageAjaxPost(FileUploaderAJAXPhotos, 2000, Session["GalleryPath"] + "\\ToImport", HttpPostedFileAJAX.fileType.image, false, false);
            }
        }

        private void btnMassImport_Click(object sender, EventArgs e)
        {
            if (_album == null)
            {
                ShowError("You have to create an album first");
                return;
            }

            try
            {
                DirectoryInfo directoryInfo = _album.GetDirectoryMassImport();

                foreach (FileInfo file in directoryInfo.GetFiles())
                {
                    if (checkFileType(file.Extension))
                    {
                        if (_album == null)
                            SaveAlbum();

                        Photo photo = new Photo();
                        photo.Title = file.Name;
                        photo.FileName = PhotoService.CreateServerFilename(file.Name);
                        photo.Size = (int) file.Length;
                        photo.CreatedBy = (User) User.Identity;
                        photo.Section = Section;
                        photo.Album = _album;

                        MemoryStream stream = new MemoryStream(File.ReadAllBytes(file.FullName), true);

                        _photoService.SavePhoto(photo, stream);
                        file.Delete();
                    }
                }
                divFastImport.Visible = false;
            }
            catch (Exception ex)
            {
                ShowException(ex);
            }
        }

        private void DisplayFastImportPanel()
        {
            divFastImport.Visible = false;
            //if (_album.PhotoCount == 0)
            //{
            DirectoryInfo directoryInfo = _album.GetDirectoryMassImport();

            if (!directoryInfo.Exists)
                return;

            int fileCount = 0;

            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                if (checkFileType(file.Extension))
                    fileCount++;
            }

            if (fileCount > 0)
                divFastImport.Visible = true;
            //}
        }

        private void BindAlbum()
        {
            txtTitle.Text = _album.Title;
            fckEditorDescription.Value = _album.Description;
            chkActive.Checked = _album.Active;
        }

        private void SaveAlbum()
        {
            try
            {
                _album.Title = txtTitle.Text;

                if (fckEditorDescription.Value.Length > 0)
                    _album.Description = fckEditorDescription.Value;
                else
                    _album.Description = null;

                _album.Active = chkActive.Checked;

                _album.ModifiedBy = (User) User.Identity;
                _album.UpdateTimestamp = DateTime.Now;

                _albumService.SaveAlbum(_album);

                Response.Redirect(String.Format("AdminGallery.aspx{0}", GetBaseQueryString()));
            }
            catch (Exception ex)
            {
                ShowException(ex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                if (_album == null)
                {
                    _album = new Album();
                    _album.Section = _galleryModule.Section;
                    _album.Site = _galleryModule.Section.Node.Site;
                    _album.CreatedBy = (User) User.Identity;
                }

                SaveAlbum();
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (_album != null)
            {
                try
                {
                    _albumService.DeleteAlbum(_album);
                    Response.Redirect(String.Format("AdminGallery.aspx{0}", GetBaseQueryString()), true);
                }
                catch (Exception ex)
                {
                    ShowError(ex.Message);
                }
            }
            else
            {
                ShowError("No Album found to delete");
            }
        }

        private bool checkFileType(string extension)
        {
            return extension.ToLower() == ".jpg" ||
                   extension.ToLower() == ".png" ||
                   extension.ToLower() == ".jpeg";
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
        }

        #endregion
    }
}