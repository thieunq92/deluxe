using System;
using CMS.Core.Util;
using Portal.Modules.OrientalSails.Web.UI;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class AgencyUserDetail : SailsAgencyAdminBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindTimeZones();
                BindUserControls();
            }
        }

        private void BindTimeZones()
        {
            ddlTimeZone.DataSource = TimeZoneUtil.GetTimeZones();
            ddlTimeZone.DataValueField = "Key";
            ddlTimeZone.DataTextField = "Value";
            ddlTimeZone.DataBind();
        }

        private void BindUserControls()
        {
            txtUsername.Visible = false;
            lblUsername.Text = UserIdentity.UserName;
            lblUsername.Visible = true;
            rfvUsername.Enabled = false;
            txtFirstname.Text = UserIdentity.FirstName;
            txtLastname.Text = UserIdentity.LastName;
            txtEmail.Text = UserIdentity.Email;
            txtWebsite.Text = UserIdentity.Website;
            ddlTimeZone.Items.FindByValue(UserIdentity.TimeZone.ToString()).Selected = true;
            chkActive.Checked = UserIdentity.IsActive;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                if (UserIdentity.Id == -1)
                {
                    UserIdentity.UserName = txtUsername.Text;
                }
                else
                {
                    UserIdentity.UserName = lblUsername.Text;
                }
                if (txtFirstname.Text.Length > 0)
                {
                    UserIdentity.FirstName = txtFirstname.Text;
                }
                if (txtLastname.Text.Length > 0)
                {
                    UserIdentity.LastName = txtLastname.Text;
                }
                UserIdentity.Email = txtEmail.Text;
                UserIdentity.Website = txtWebsite.Text;
                UserIdentity.IsActive = chkActive.Checked;
                UserIdentity.TimeZone = Int32.Parse(ddlTimeZone.SelectedValue);

                if (txtPassword.Text.Length > 0 && txtPassword2.Text.Length > 0)
                {
                    try
                    {
                        UserIdentity.Password = CMS.Core.Domain.User.HashPassword(txtPassword.Text);
                    }
                    catch (Exception ex)
                    {
                        ShowError(ex.Message);
                    }
                }

                if (UserIdentity.Id == -1 && UserIdentity.Password == null)
                {
                    ShowError("Phải có mật khẩu");
                }
                else
                {
                    Module.SaveOrUpdate(UserIdentity);
                }
            }
        }
    }
}