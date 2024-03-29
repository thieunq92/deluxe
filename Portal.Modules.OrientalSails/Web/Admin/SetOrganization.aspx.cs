using System;
using System.Collections;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using CMS.Core.Domain;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.UI;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class SetOrganization : SailsAdminBasePage
    {
        private string _currentGroup = string.Empty;
        private Role _role;
        private User _user;

        private IList _organizations;
        private IList _fixedPermission;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!UserIdentity.HasPermission(AccessLevel.Administrator))
            {
                ShowError("You must be administrator to use this function");
            }

            if (Request.QueryString["roleid"]!=null)
            {
                _role = Module.RoleGetById(Convert.ToInt32(Request.QueryString["roleid"]));
                litTitle.Text = string.Format("Permission for role {0}", _role.Name);
            }
            else if (Request.QueryString["userid"]!=null)
            {
                _user = Module.UserGetById(Convert.ToInt32(Request.QueryString["userid"]));
                litTitle.Text = string.Format("Permission for user {0}", _user.UserName);
            }
            else
            {
                ShowError("Bad request");
                return;
            }

            //if (_role != null)
            //{
            //    _permissions = Module.PermissionsGetByRole(_role);
            //}
            //else
            //{

            _organizations = Module.OrganizationGetByUser(_user);


            //    _fixedPermission = Module.PermissionsGetByUserRole(_user);
            //}

            if (!IsPostBack)
            {
                rptOrganizations.DataSource = Module.OrganizationGetAllRoot();
                rptOrganizations.DataBind();
            }
        }

        protected void rptOrganizations_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is Organization)
            {
                //ModulePermission permission = (ModulePermission) e.Item.DataItem;
                var org = (Organization) e.Item.DataItem;
                var chkPermission = (CheckBox)e.Item.FindControl("chkPermission");
                chkPermission.Text = org.Name;
                chkPermission.Checked = false;
                foreach (UserOrganization organization in _organizations)
                {
                    if (organization.Organization.Id == org.Id)
                    {
                        chkPermission.Checked = true;
                        break;
                    }
                }
                //if (_organizations.Contains(org))
                //{
                //    chkPermission.Checked = true;
                //}
                //else
                //{
                //    chkPermission.Checked = false;
                //}

                //if (_fixedPermission != null)
                //{
                //    if (_fixedPermission.Contains(permission.Name))
                //    {
                //        chkPermission.Checked = true;
                //        chkPermission.Enabled = false;
                //    }
                //}

                //if (!string.IsNullOrEmpty(permission.GroupName) && permission.GroupName!=_currentGroup)
                //{
                //    _currentGroup = permission.GroupName;
                //    HtmlGenericControl liClear = e.Item.FindControl("liClear") as HtmlGenericControl;
                //    if (liClear!=null)
                //    {
                //        liClear.Visible = true;
                //        liClear.InnerHtml = string.Format("<strong>{0}</strong>", _currentGroup);
                //    }
                //}
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            foreach (RepeaterItem item in rptOrganizations.Items)
            {
                CheckBox chkPermission = (CheckBox)item.FindControl("chkPermission");
                HiddenField hiddenId = (HiddenField)item.FindControl("hiddenId");
                if (_user!=null)
                {
                    UserOrganization uo = null;
                    foreach (UserOrganization org in _organizations)
                    {
                        if (org.Organization.Id.ToString() == hiddenId.Value)
                        {
                            uo = org;
                            break;
                        }
                    }

                    if (uo!=null && !chkPermission.Checked) //có mà bỏ check
                    {
                        Module.Delete(uo);
                    }
                    if (uo==null && chkPermission.Checked)
                    {
                        uo = new UserOrganization();
                        uo.User = _user;
                        uo.Organization = Module.OrganizationGetById(Convert.ToInt32(hiddenId.Value));
                        Module.SaveOrUpdate(uo);
                    }
                    //if (_organizations.Contains(hiddenName.Value) && !chkPermission.Checked) // Nếu có quyền và không có check
                    //{
                    //    SpecialPermission permission = Module.PermissionGetByRole(hiddenName.Value, _role);
                    //    if (permission!=null)
                    //    {
                    //        Module.Delete(permission);
                    //    }
                    //}
                    //else if(!_organizations.Contains(hiddenName.Value) && chkPermission.Checked)
                    //{
                    //    SpecialPermission permission = Module.PermissionGetByRole(hiddenName.Value, _role);
                    //    if (permission==null)
                    //    {
                    //        permission = new SpecialPermission
                    //                         {Name = hiddenName.Value, Role = _role, ModuleType = Section.ModuleType};
                    //        Module.SaveOrUpdate(permission);
                    //    }
                    //}
                }
                
            }
            PageRedirect(string.Format("SetPermission.aspx?NodeId={0}&SectionId={1}", Node.Id, Section.Id));
        }
    }
}
