using System;
using System.Web.UI.WebControls;
using CMS.Core.Domain;
using CMS.ServerControls;
using Portal.Modules.OrientalSails.Web.UI;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class SetPermission : SailsAdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!UserIdentity.HasPermission(AccessLevel.Administrator))
            {
                ShowError("You must be administrator to use this function");
                return;
            }
            if (!IsPostBack)
            {
                rptRoles.DataSource = Module.RoleGetAll();
                rptRoles.DataBind();
                rptUsers.DataSource = Module.UserGetAll();
                rptUsers.DataBind();
            }
        }

        protected void pagerRoles_PageChanged(object sender, PageChangedEventArgs e)
        {
            rptRoles.DataSource = Module.RoleGetAll();
            rptRoles.DataBind();
        }

        protected void pagerUser_PageChanged(object sender, PageChangedEventArgs e)
        {
            rptUsers.DataSource = Module.UserGetAll();
            rptUsers.DataBind();
        }

        protected void rptRoles_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is Role)
            {
                Role role = (Role) e.Item.DataItem;
                if (role.HasPermission(AccessLevel.Administrator))
                {
                    e.Item.Visible = false;
                    return;
                }
                Literal litName = e.Item.FindControl("litName") as Literal;
                if (litName!=null)
                {
                    litName.Text = role.Name;
                }
                HyperLink hplSetPermission = e.Item.FindControl("hplSetPermission") as HyperLink;
                if (hplSetPermission!=null)
                {
                    hplSetPermission.NavigateUrl = string.Format("Permissions.aspx?NodeId={0}&SectionId={1}&roleid={2}",
                                                          Node.Id, Section.Id, role.Id);
                }                
            }
        }

        protected void rptUsers_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is User)
            {
                User role = (User)e.Item.DataItem;
                if (role.HasPermission(AccessLevel.Administrator))
                {
                    e.Item.Visible = false;
                    return;
                }
                Literal litName = e.Item.FindControl("litName") as Literal;
                if (litName != null)
                {
                    litName.Text = role.UserName;
                }
                HyperLink hplSetPermission = e.Item.FindControl("hplSetPermission") as HyperLink;
                if (hplSetPermission != null)
                {
                    hplSetPermission.NavigateUrl = string.Format("Permissions.aspx?NodeId={0}&SectionId={1}&userid={2}",
                                                          Node.Id, Section.Id, role.Id);
                }

                var hplSetOrganization = e.Item.FindControl("hplSetOrganization") as HyperLink;
                if (hplSetOrganization != null)
                {
                    hplSetOrganization.NavigateUrl = string.Format("SetOrganization.aspx?NodeId={0}&SectionId={1}&userid={2}",
                                                          Node.Id, Section.Id, role.Id);
                }
            }
        }
    }
}
