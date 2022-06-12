using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CMS.Web.UI;

namespace CMS.Modules.Gallery.Web.UI
{
	/// <summary>
	/// The base user control for the gallery user controls
	/// </summary>
	public class BaseGalleryControl : BaseModuleControl
	{
		private GalleryModule _galleryModule;

		protected GalleryModule GalleryModule{ get { return _galleryModule; } }
        protected AlbumService AlbumService { get { return _galleryModule.GetAlbumService(); } }
        protected PhotoService PhotoService { get { return _galleryModule.GetPhotoService(); } }

		protected override void OnLoad(EventArgs e)
		{
			this._galleryModule = base.Module as GalleryModule;

			// Register stylesheet
			string cssfile = GalleryModule.ThemePath + "gallery.css";
			RegisterStylesheet("gallerycss", cssfile);

			base.OnLoad(e);
		}

        /// <summary>
        /// The current Album page to be used for "back" buttons on lower level pages (photos/view)
        /// </summary>
        /// <remarks>
        /// Storing it as a Session variable seems the most convenient way right now.
        /// </remarks>
        public virtual int CurrentAlbumPage
        {
            get
            {
                if (Session["_CurrentAlbumPage"] != null)
                {
                    return Convert.ToInt16(this.Session["_CurrentAlbumPage"]);
                }
                else
                {
                    return 0;
                }
            }

            set { this.Session["_CurrentAlbumPage"] = value; }
        }

        /// <summary>
        /// The current Photo page to be used for "back" buttons on lower level pages (view)
        /// </summary>
        /// <remarks>
        /// Storing it as a Session variable seems the most convenient way right now.
        /// </remarks>
        public virtual int CurrentPhotoPage
        {
            get
            {
                if (this.Session["_CurrentPhotoPage"] != null)
                {
                    return Convert.ToInt16(this.Session["_CurrentPhotoPage"]);
                }
                else
                {
                    return 0;
                }
            }

            set { this.Session["_CurrentPhotoPage"] = value; }
        }
	}
}
