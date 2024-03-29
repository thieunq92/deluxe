using System;
using System.Web.UI.WebControls;
using CMS.Core.Domain;
using CMS.ServerControls;
using log4net;
using Portal.Modules.OrientalSails.Web.UI;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class AgentUserList : SailsAdminBasePage
    {
        #region -- Private Member --

        private readonly ILog _logger = LogManager.GetLogger(typeof (AgentUserList));

        #endregion

        #region -- PAGE EVENTS --

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!UserIdentity.HasPermission(AccessLevel.Administrator))
                {
                    panelContent.Visible = false;
                    ShowError(Resources.stringAccessDenied);
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
            catch (Exception ex)
            {
                _logger.Error("Error when Page_Load in AgentUserList", ex);
                ShowError(ex.Message);
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