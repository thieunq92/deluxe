using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CMS.ServerControls;
using CMS.Web.UI;
using NHibernate.Criterion;

namespace CMS.Modules.User
{
    public partial class UserSelector : KitModuleAdminBasePage
    {
        private UserModule module;

        protected void Page_Load(object sender, EventArgs e)
        {
            module = _moduleLoader.GetModuleFromClassName("CMS.Modules.User.UserModule") as UserModule;
            if (!IsPostBack)
            {
                if (module != null)
                {
                    GetDataSource();
                    rptUsers.DataBind();
                }
            }
        }

        protected override bool CanByPassCanEditCheck()
        {
            return true;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ViewState["Name"] = textBoxName.Text;
            GetDataSource();
            rptUsers.DataBind();
        }

        protected void GetDataSource()
        {
            if (ViewState["Name"] != null)
            {
                rptUsers.DataSource = module.CommonDao.GetObjectByCriterion(typeof (Core.Domain.User),
                                                                            Expression.Like("UserName",
                                                                                            ViewState["Name"].ToString(),
                                                                                            MatchMode.Anywhere));
            }
            else
            {
                rptUsers.DataSource = module.CommonDao.GetAll(typeof (Core.Domain.User));
            }
        }

        protected void rptUsers_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (Request.QueryString["Command"]!=null && e.Item.DataItem is Core.Domain.User)
            {
                Core.Domain.User user = (Core.Domain.User) e.Item.DataItem;
                string command = Request.QueryString["Command"];
                HtmlAnchor processData = e.Item.FindControl("processData") as HtmlAnchor;
                if (processData!=null)
                {
                    processData.Attributes.Add("onClick", "window.opener."+command+"('"+user.Id+"','"+user.UserName+"'); self.close();");
                }
            }
        }

        protected void pagerUsers_PageChanged(object sender, PageChangedEventArgs e)
        {
            GetDataSource();
            rptUsers.DataBind();
        }
    }
}