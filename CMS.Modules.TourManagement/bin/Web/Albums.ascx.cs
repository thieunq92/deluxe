using System;
using System.Collections;
using System.Web.UI.WebControls;
using CMS.Modules.Gallery.Domain;
using CMS.Modules.Gallery.Web.UI;
using CMS.ServerControls;
using CMS.Web.Util;

namespace CMS.Modules.Gallery.Web
{
    /// <summary>
    ///		Summary description for Albums.
    /// </summary>
    public class Albums : BaseGalleryControl
    {
        private IList _albumList;

        protected DataList AlbumDataList;
        protected Pager pgrAlbums;
        protected Panel pnlAlbumList;

        protected void Page_Load(object sender, EventArgs e)
        {
            // set number of columns for datalist based on preferences
            AlbumDataList.RepeatColumns = base.GalleryModule.AlbumSettings.NumberOfColumns;

            // set up paging
            pgrAlbums.PageSize = base.GalleryModule.AlbumSettings.NumberOfItemsOnPage;
            pgrAlbums.PageUrl = UrlHelper.GetUrlFromSection(base.GalleryModule.Section);
            // this.pgrAlbums.PagerLinkMode = CMS.ServerControls.PagerLinkMode.HyperLinkPathInfo;

            if (!IsPostBack && ((!base.HasCachedOutput) || Page.User.Identity.IsAuthenticated))
            {
                BindAlbums();
            }
        }

        private void BindAlbums()
        {
            AlbumService albumService = GalleryModule.GetAlbumService();

            if (_albumList == null)
                _albumList = albumService.GetAlbumList(true);


            if (_albumList != null)
            {
                AlbumDataList.DataSource = _albumList;
                AlbumDataList.DataBind();
            }
        }

        protected void pgrAlbums_PageChanged(object sender, PageChangedEventArgs e)
        {
            BindAlbums();
            if (IsPostBack)
            {
                // remember the current album page for future use
                CurrentAlbumPage = pgrAlbums.CurrentPageIndex + 1;
                // reset the current photo page variable, we are at the album page not a photo page
                CurrentPhotoPage = 0;
            }
        }

        protected void AlbumDataList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HyperLink hplAlbum;

                Album album = (Album) e.Item.DataItem;

                hplAlbum = (HyperLink) e.Item.FindControl("hplImage");
                if (album.Id != -1)
                {
                    if (album.PhotoCount > 0)
                    {
                        hplAlbum.NavigateUrl = UrlHelper.GetUrlFromSection(Module.Section) +
                                               String.Format("/album/{0}", album.Id);
                    }

                    Literal litImage = (Literal) e.Item.FindControl("litImage");
                    litImage.Text = "<img";

                    Photo tempPhoto = base.PhotoService.GetFirstPhotoOfAlbum(album.Id);
                    if (tempPhoto != null)
                    {
                        litImage.Text += " src=\"" +
                                         base.Page.ResolveUrl(
                                             base.GalleryModule.VirtualPath(
                                                 base.GalleryModule.PathBuilder.GetThumbPath(tempPhoto))) + "\"";
                        litImage.Text += " width=\"" + tempPhoto.ThumbWidth + "\"";
                        litImage.Text += " height=\"" + tempPhoto.ThumbHeight + "\"";
                        litImage.Text += " alt=\"" + album.Title + "\"";
                    }
                    else
                    {
                        litImage.Text += " src=\"" +
                                         base.Page.ResolveUrl("~/Modules/Gallery/images/placeholder-200.jpg") + "\"";
                        litImage.Text += " width=\"200\"";
                        litImage.Text += " height=\"150\"";
                    }

                    litImage.Text += " />";

                    if (album.Description != null)
                    {
                        hplAlbum.ToolTip = album.Description;
                    }
                }
                else
                {
                    hplAlbum.Text = String.Empty;
                }

                hplAlbum = (HyperLink) e.Item.FindControl("hplAlbum");

                if (album.Id != -1)
                {
                    hplAlbum.Text = album.Title;

                    if (album.PhotoCount > 0)
                    {
                        hplAlbum.NavigateUrl = UrlHelper.GetUrlFromSection(Module.Section) +
                                               String.Format("/album/{0}", album.Id);

                        hplAlbum.Text += " (" + album.PhotoCount + " " +
                                         (album.PhotoCount == 1 ? base.GetText("strPhoto") : base.GetText("strPhotos")) +
                                         ")";
                    }
                }
                else
                {
                    hplAlbum.Text = String.Empty;
                }
            }
        }

        protected void pgrAlbums_CacheEmpty(object sender, EventArgs e)
        {
            AlbumService albumService = GalleryModule.GetAlbumService();

            if (_albumList == null)
                _albumList = albumService.GetAlbumList(true);


            if (_albumList != null)
            {
                AlbumDataList.DataSource = _albumList;
            }
        }
    }
}