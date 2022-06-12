using System;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using CMS.Modules.Gallery.Domain;
using CMS.ServerControls;
using CMS.Web.UI;

namespace CMS.Modules.Gallery.Web
{
    public partial class AlbumSelectorPage : ModuleAdminBasePage
    {
        private GalleryModule module;

        protected void Page_Load(object sender, EventArgs e)
        {
            module = Module as GalleryModule;
            //module = _moduleLoader.GetModuleFromClassName("CMS.Modules.User.UserModule") as UserModule;
            if (!IsPostBack)
            {
                if (module != null)
                {
                    GetDataSource();
                    rptAlbums.DataBind();
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ViewState["Name"] = textBoxName.Text;
            GetDataSource();
            rptAlbums.DataBind();
        }

        protected void GetDataSource()
        {
            if (ViewState["Name"] != null)
            {
                rptAlbums.DataSource = module.GetAlbumService().GetAlbumList(true, ViewState["Name"].ToString());
            }
            else
            {
                rptAlbums.DataSource = module.GetAlbumService().GetAlbumList(true);
            }
        }

        protected void rptAlbums_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (Request.QueryString["Command"] != null && e.Item.DataItem is Album)
            {
                Album user = (Album)e.Item.DataItem;
                string command = Request.QueryString["Command"];
                HtmlAnchor processData = e.Item.FindControl("processData") as HtmlAnchor;
                if (processData != null)
                {
                    processData.Attributes.Add("onClick", "window.opener." + command + "('" + user.Id + "','" + user.Title + "'); self.close();");
                }
            }
        }

        protected void pagerAlbums_PageChanged(object sender, PageChangedEventArgs e)
        {
            GetDataSource();
            rptAlbums.DataBind();
        }
    }
}
