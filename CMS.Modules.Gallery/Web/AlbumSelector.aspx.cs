using System;
using CMS.ServerControls;
using CMS.Web.Admin.UI;
using CMS.Web.UI;

namespace CMS.Modules.Gallery.Web
{
    public partial class AlbumSelector : ModuleAdminBasePage
    {
        private new GalleryModule Module
        {
            get { return base.Module as GalleryModule; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            GetDataSource();
            rptAlbums.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            GetDataSource();
            rptAlbums.DataBind();
        }

        protected void pagerAlbums_PageChanged(object sender, PageChangedEventArgs e)
        {
            GetDataSource();
            rptAlbums.DataBind();
        }

        private void GetDataSource()
        {
            rptAlbums.DataSource = Module.GetAlbumService().GetAlbumList(true, textBoxName.Text);
        }
    }
}
