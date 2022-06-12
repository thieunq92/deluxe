using System;
using System.Collections;
using System.Web.UI.WebControls;
using CMS.Core.Domain;
using CMS.Modules.Gallery.Domain;
using CMS.Modules.Gallery.Web.UI;
using CMS.ServerControls;
using CMS.Web.UI;
using CMS.Web.Util;

namespace CMS.Modules.Gallery.Web
{
    public class Photos : BaseGalleryControl
    {
        private Album _checkAlbum;
        private IList _photolist;

        protected HyperLink hplBack;
        protected Label labelAlbumDescription;
        protected Pager pgrPhotos;
        protected DataList PhotoDataList;
        protected HyperLink hplBackToSection;

        public Album Album
        {
            get { return _checkAlbum; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            _checkAlbum = AlbumService.GetAlbumById(GalleryModule.CurrentAlbumId);

            // only go ahead if album exists
            if (_checkAlbum != null)
            {
                // if album is not active, go back to main section page
                if (_checkAlbum.Active == false)
                {
                    Response.Redirect(UrlHelper.GetUrlFromSection(GalleryModule.Section));
                }

                // set number of columns for datalist based on preferences
                PhotoDataList.RepeatColumns = GalleryModule.AlbumSettings.NumberOfColumns;

                // set up paging
                pgrPhotos.PageSize = GalleryModule.AlbumSettings.NumberOfItemsOnPage;
                //this.pgrPhotos.PageUrl = UrlHelper.GetUrlFromSection(base.GalleryModule.Section);

                labelAlbumDescription.Text = _checkAlbum.Description;

                if (!IsPostBack && ((!HasCachedOutput) || Page.User.Identity.IsAuthenticated))
                {
                    Section sectionBack = Module.Section.Connections[GalleryModule.BACK] as Section;
                    if (sectionBack!=null)
                    {
                        hplBackToSection.NavigateUrl = string.Format("{0}/album/{1}", UrlHelper.GetUrlFromSection(sectionBack), _checkAlbum.Id);
                    }
                    else
                    {
                        hplBackToSection.Visible = false;
                    }
                    BindPhotos();
                }
            }
            else
            {
                Response.Redirect(UrlHelper.GetUrlFromSection(GalleryModule.Section));
            }

            // this.hplBack.NavigateUrl = UrlHelper.GetUrlFromSection(base.GalleryModule.Section) + "/page/" + (base.GalleryModule.ActiveAlbumPage + 1);
            hplBack.Text = base.GetText("strBack");
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (PageEngine==null)
            {
                return;
            }
            if (Module.Section.Node.Id == PageEngine.ActiveNode.Id)
            {
                PageEngine.RegisterNavigationPath(Album.Title,Request.RawUrl);
            }

            base.OnPreRender(e);
        }

        private void BindPhotos()
        {
            // photo list view
            if (_photolist == null)
            {
                _photolist = PhotoService.GetPhotoList(GalleryModule.CurrentAlbumId);
            }

            if (_photolist != null)
            {
                PhotoDataList.DataSource = _photolist;
                PhotoDataList.DataBind();
            }

            hplBack.NavigateUrl = UrlHelper.GetUrlFromSection(GalleryModule.Section);

            if (CurrentAlbumPage > 0)
            {
                hplBack.NavigateUrl += "/page/" + CurrentAlbumPage;
            }
        }

        protected void pgrPhoto_PageChanged(object sender, PageChangedEventArgs e)
        {
            if (IsPostBack || ((!HasCachedOutput) || Page.User.Identity.IsAuthenticated))
            {
                BindPhotos();

                // remember the current album page for future use
                CurrentPhotoPage = pgrPhotos.CurrentPageIndex + 1;
            }
        }

        protected void PhotoDataList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Photo photo = (Photo) e.Item.DataItem;

                string vieuwUrl = UrlHelper.GetUrlFromSection(GalleryModule.Section) + "/" + photo.Album.Id + "/" +
                                  photo.Id;
                HyperLink hplFile = (HyperLink) e.Item.FindControl("hplFile");
                hplFile.NavigateUrl = vieuwUrl;

                if (!(Page is PageEngine))
                {
                    hplFile.Target = "_blank";
                }

                Literal litImage = (Literal) e.Item.FindControl("litImage");
                litImage.Text = "<img";
                litImage.Text += " src=\"" + base.Page.ResolveUrl(
                                                 GalleryModule.VirtualPath(
                                                     GalleryModule.PathBuilder.GetThumbPath(photo))) + "\"";
                litImage.Text += " width=\"" + photo.ThumbWidth + "\"";
                litImage.Text += " height=\"" + photo.ThumbHeight + "\"";
                litImage.Text += " alt=\"" + photo.Title + "\"";
                litImage.Text += " />";

                Literal litImageLabel = (Literal) e.Item.FindControl("litImageLabel");
                litImageLabel.Text = photo.DisplayTitle;

                // only add number of views if configuration is set to show it
                if (GalleryModule.AlbumSettings.ShowNumberOfViews)
                {
                    // no point showing it when there's no views
                    if (photo.NrOfViews > 0)
                    {
                        litImageLabel.Text += " (" + photo.NrOfViews + base.GetText("strViews") + ")";
                    }
                }
            }
        }
    }
}