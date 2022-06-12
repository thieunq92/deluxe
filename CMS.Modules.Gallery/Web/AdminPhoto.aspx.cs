using System;
using System.IO;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CMS.Core.Domain;
using CMS.Modules.Gallery.Domain;
using CMS.Web.UI;

namespace CMS.Modules.Gallery.Web
{
	public class AdminPhoto : ModuleAdminBasePage
	{
		private GalleryModule _galleryModule;
	    private AlbumService _albumService;
	    private PhotoService _photoService;

		private Photo _photo;
        private int _photoId;
        private int _albumId;

		protected Panel pnlFileName;
        protected Image imThumb;
        protected TextBox txtFile;
        protected RequiredFieldValidator rfvFile;
        protected HtmlInputFile filUpload;
        protected TextBox txtTitle;
        protected Button btnUpload;
        protected Button btnSave;
        protected Button btnDelete;
        protected Button btnCancel;
	
		private void Page_Load(object sender, EventArgs e)
		{
            this._galleryModule = base.Module as GalleryModule;

		    _albumService = _galleryModule.GetAlbumService();
		    _photoService = _galleryModule.GetPhotoService();

            if (Request.QueryString["AlbumId"] != null) { this._albumId = Int32.Parse(Request.QueryString["AlbumId"]); }
            if (Request.QueryString["PhotoId"] != null) { this._photoId = Int32.Parse(Request.QueryString["PhotoId"]); }
			
			if (this._photoId > 0)
			{
                this._photo = _photoService.GetPhotoById(this._photoId);
				if (! this.IsPostBack)
				{
					BindPhoto();
				}

			    imThumb.ImageUrl = base.Page.ResolveUrl(
			        _galleryModule.VirtualPath(
			            _galleryModule.PathBuilder.GetThumbPath(_photo)));
            				
				this.btnDelete.Visible = true;
				this.btnDelete.Attributes.Add("onclick", "return confirm('Are you sure?');");
			}
			else
			{
				// It is possible that a new file is already uploaded and in the database. The
				// tempPhotoId parameter in the viewstate should indicate this.
				if (ViewState["tempPhotoId"] != null)
				{
					int tempPhotoId = (int)ViewState["tempPhotoId"];
                    this._photo = _photoService.GetPhotoById(tempPhotoId);
				}
				else
				{
					// New file.
					this._photo = new Photo();
				}

				this.btnDelete.Visible = false;
                this.imThumb.Visible = false;
			}
		}

		private void BindPhoto()
		{
			this.txtFile.Text = this._photo.FileName;
			this.txtTitle.Text = this._photo.Title;
		}



		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
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
			this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnUpload_Click(object sender, EventArgs e)
		{
			HttpPostedFile postedFile = this.filUpload.PostedFile;
			if (postedFile.ContentLength > 0)
			{
				_photo.Title = txtTitle.Text;
				_photo.FileName = PhotoService.CreateServerFilename(postedFile.FileName);
				_photo.Size = postedFile.ContentLength;
                _photo.CreatedBy = (User)this.User.Identity;
				_photo.Section = base.Section;
			    _photo.Album = _albumService.GetAlbumById(_albumId);

				// Save the file
				try
				{
                    _photoService.SavePhoto(_photo, postedFile.InputStream);

					if (_photoId <= 0 && this._photo.Id > 0)
					{
						// This appears to be a new file. Store the id of the file in the viewstate
						// so the file can be deleted if the user decides to cancel.
						ViewState["tempPhotoId"] = _photo.Id;
					}
					BindPhoto();
				}
				catch (Exception ex)
				{
					// Something went wrong
					ShowError("Error saving the file: " + ex.Message);
				}
			}
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			if (this.IsValid)
			{
                this._photo.CreatedBy = this.User.Identity as User;
				this._photo.Title = this.txtTitle.Text;

				try
				{
					// Only save meta data.
                    _photoService.SavePhotoInfo(this._photo);
					Context.Response.Redirect("AdminPhotos.aspx" + base.GetBaseQueryString() + "&AlbumId=" + this._albumId);
				}
				catch (Exception ex)
				{
					ShowError("Error saving file: " + ex.Message);
				}
			}
		}

		private void btnDelete_Click(object sender, EventArgs e)
		{
			try
			{
                _photoService.DeletePhoto(_photo);
                Context.Response.Redirect("AdminPhotos.aspx" + base.GetBaseQueryString() + "&AlbumId=" + this._albumId);
			}
			catch (Exception ex)
			{
				ShowError("Error deleting file: " + ex.Message);
			}
		}
		private void btnCancel_Click(object sender, EventArgs e)
		{
			// Check if there is a new file pending. This has to be deleted
			if (ViewState["tempPhotoId"] != null)
			{
                _photoService.DeletePhoto(_photo);
			}
            Context.Response.Redirect("AdminPhotos.aspx" + base.GetBaseQueryString() + "&AlbumId=" + this._albumId);
		}

	}
}
