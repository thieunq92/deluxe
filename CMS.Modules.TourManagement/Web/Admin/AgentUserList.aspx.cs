using System;
using System.Web.UI.WebControls;
using CMS.Core.Domain;
using CMS.Modules.TourManagement.Web.UI;
using CMS.ServerControls;

namespace CMS.Modules.TourManagement.Web.Admin
{
    public partial class AgentUserList : TourAdminBasePage
    {
        #region -- PAGE EVENTS --

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!UserIdentity.HasPermission(AccessLevel.Administrator))
            {
                panelContent.Visible = false;
                ShowError(Resources.textAccessDenied);
                return;
            }
            if (!IsPostBack)
            {
                Role role = Module.RoleGetById(Convert.ToInt32(Request.QueryString["RoleId"]));
                Title = string.Format(Resources.titleAdminAgencyUserList, role.Name);
                rptUsers.DataSource = role.Users;
                rptUsers.DataBind();
                lblRoleName.Text = role.Name;
            }
        }

        #endregion

        #region -- Repeater --

        protected void rptUsers_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            User user = e.Item.DataItem as User;
            if (user != null)
            {
                using (Label lbl = e.Item.FindControl("lblDate") as Label)
                {
                    if (lbl != null)
                    {
                        lbl.Text = user.InsertTimestamp.ToString("dd/MM/yyyy");
                    }
                }

                using (HyperLink hpl = e.Item.FindControl("UserEditLink") as HyperLink)
                {
                    if (hpl != null)
                    {
                        hpl.NavigateUrl = string.Format("/Admin/UserEdit.aspx?UserId={0}", user.Id);
                    }
                }
            }
        }

        protected void PagerUser_PageChanged(object sender, PageChangedEventArgs e)
        {
            Role role = Module.RoleGetById(Convert.ToInt32(Request.QueryString["RoleId"]));
            rptUsers.DataSource = role.Users;
            rptUsers.DataBind();
        }

        #endregion
    }
}