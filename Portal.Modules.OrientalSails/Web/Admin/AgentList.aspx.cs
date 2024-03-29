using System;
using System.Web.UI.WebControls;
using CMS.Core.Domain;
using log4net;
using Portal.Modules.OrientalSails.Web.UI;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class AgentList : SailsAdminBase
    {
        #region -- Private Member --

        private readonly ILog _logger = LogManager.GetLogger(typeof (AgentList));
        #endregion

        #region -- PAGE EVENTS --

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra quyền admin
                if (!UserIdentity.HasPermission(AccessLevel.Administrator))
                {
                    panelContent.Visible = false;
                    ShowError(Resources.stringAccessDenied);
                    return;
                }
                Title = Resources.titleAdminAgentList;
                RepeaterArticles.DataSource = Module.RoleGetAll();
                RepeaterArticles.DataBind();
                labelUserCount.Text = string.Format(Resources.stringFormatUserCount, Module.UserCount());

                if (!IsPostBack)
                {
                    if (Module.ModuleSettings("AgencySupplement") != null)
                    {
                        txtSingle.Text = Module.ModuleSettings("AgencySupplement").ToString();
                    }
                    else
                    {
                        txtSingle.Text = "0";
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error when Page_Load in AgentList", ex);
                ShowError(ex.Message);
            }
        }

        #endregion

        #region -- Repeater --

        protected void RepeaterArticles_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Role role = e.Item.DataItem as Role;
            if (role != null)
            {
                using (Label lbl = e.Item.FindControl("lblUserNumber") as Label)
                {
                    if (lbl != null)
                    {
                        lbl.Text = role.Users.Count.ToString();
                    }
                }

                using (HyperLink hpl = e.Item.FindControl("hplEdit") as HyperLink)
                {
                    if (hpl != null)
                    {
                        hpl.NavigateUrl = string.Format("AgentPriceList.aspx?NodeId={0}&SectionId={1}&RoleId={2}",
                                                        Node.Id, Section.Id, role.Id);
                    }
                }

                using (HyperLink hpl = e.Item.FindControl("AgentViewLink") as HyperLink)
                {
                    if (hpl != null)
                    {
                        hpl.NavigateUrl = string.Format("AgentUserList.aspx?NodeId={0}&SectionId={1}&RoleId={2}",
                                                        Node.Id, Section.Id, role.Id);
                    }
                }
            }
        }

        #endregion

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Module.SaveModuleSetting("AgencySupplement", txtSingle.Text);
        }
    }
}