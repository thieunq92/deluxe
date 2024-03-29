using System;
using System.Web.UI.WebControls;
using CMS.Modules.TourManagement.Web.UI;
using CMS.Core.Domain;

namespace CMS.Modules.TourManagement.Web.Admin
{
    public partial class AgentList : TourAdminBasePage
    {
        #region -- PAGE EVENTS --
        protected void Page_Load(object sender, EventArgs e)
        {
            // Kiểm tra quyền admin
            if (!UserIdentity.HasPermission(AccessLevel.Administrator))
            {
                panelContent.Visible = false;
                ShowError(Resources.textAccessDenied);
                return;
            }
            Title = Resources.titleAdminAgentList;
            RepeaterArticles.DataSource = Module.RoleGetAll();
            RepeaterArticles.DataBind();
            labelUserCount.Text = string.Format(Resources.stringFormatUserCount, Module.UserCount());
        }

        #endregion

        #region -- Repeater --

        protected void RepeaterArticles_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
        }

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
                        hpl.NavigateUrl = string.Format("AgentPriceList.aspx?NodeId={0}&SectionId={1}&RoleId={2}&ModuleType={3}",
                                                        Node.Id, Section.Id, role.Id,Request.QueryString["ModuleType"]);
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
    }
}