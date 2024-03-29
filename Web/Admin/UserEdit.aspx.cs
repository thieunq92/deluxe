using System;
using System.Collections;
using System.Web.UI.WebControls;
using CMS.Core.Domain;
using CMS.Core.Util;
using CMS.Web.Admin.UI;

namespace CMS.Web.Admin
{
    /// <summary>
    /// Summary description for UserEdit.
    /// </summary>
    public class UserEdit : AdminBasePage
    {
        private User _activeUser;

        protected Button btnCancel;
        protected Button btnDelete;
        protected Button btnSave;
        protected CheckBox chkActive;
        protected CompareValidator covPassword;
        protected DropDownList ddlTimeZone;
        protected Label lblUsername;
        protected RegularExpressionValidator revEmail;
        protected RequiredFieldValidator rfvEmail;
        protected RequiredFieldValidator rfvUsername;
        protected Repeater rptRoles;
        protected TextBox txtEmail;
        protected TextBox txtFirstname;
        protected TextBox txtLastname;
        protected TextBox txtPassword1;
        protected TextBox txtPassword2;
        protected TextBox txtUsername;
        protected TextBox txtWebsite;
        protected CheckBox chkIsAgency;

        private void Page_Load(object sender, EventArgs e)
        {
            Title = "Sửa người dùng";

            if (Context.Request.QueryString["UserId"] != null)
            {
                if (Int32.Parse(Context.Request.QueryString["UserId"]) == -1)
                {
                    // Create a new user instance
                    _activeUser = new User();
                }
                else
                {
                    // Get user data
                    _activeUser = (User) base.CoreRepository.GetObjectById(typeof (User)
                                                                           ,
                                                                           Int32.Parse(
                                                                               Context.Request.QueryString["UserId"]));
                }

                if (! IsPostBack)
                {
                    BindTimeZones();
                    BindUserControls();
                    BindRoles();
                }
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
            if (_activeUser.Id > 0)
            {
                txtUsername.Visible = false;
                lblUsername.Text = _activeUser.UserName;
                lblUsername.Visible = true;
                rfvUsername.Enabled = false;
            }
            else
            {
                txtUsername.Text = _activeUser.UserName;
                txtUsername.Visible = true;
                lblUsername.Visible = false;
                rfvUsername.Enabled = true;
            }
            txtFirstname.Text = _activeUser.FirstName;
            txtLastname.Text = _activeUser.LastName;
            txtEmail.Text = _activeUser.Email;
            txtWebsite.Text = _activeUser.Website;
            ddlTimeZone.Items.FindByValue(_activeUser.TimeZone.ToString()).Selected = true;
            chkActive.Checked = _activeUser.IsActive;
            chkIsAgency.Checked = _activeUser.IsAgency;
            btnDelete.Visible = (_activeUser.Id > 0);
            btnDelete.Attributes.Add("onclick", "return confirmDeleteUser();");
        }

        private void BindRoles()
        {
            IList roles = base.CoreRepository.GetAll(typeof (Role), "PermissionLevel");
            FilterAnonymousRoles(roles);
            rptRoles.ItemDataBound += rptRoles_ItemDataBound;
            rptRoles.DataSource = roles;
            rptRoles.DataBind();
        }

        /// <summary>
        /// Filter the anonymous roles from the list.
        /// </summary>
        private void FilterAnonymousRoles(IList roles)
        {
            int roleCount = roles.Count;
            for (int i = roleCount - 1; i >= 0; i--)
            {
                Role role = (Role) roles[i];
                if (role.PermissionLevel == (int) AccessLevel.Anonymous)
                {
                    roles.Remove(role);
                }
            }
        }

        private void SetRoles()
        {
            _activeUser.Roles.Clear();

            foreach (RepeaterItem ri in rptRoles.Items)
            {
                // HACK: RoleId is stored in the ViewState because the repeater doesn't have a DataKeys property.
                // Another HACK: we're only using the role id's to save database roundtrips.
                CheckBox chkRole = (CheckBox) ri.FindControl("chkRole");
                if (chkRole.Checked)
                {
                    int roleId = (int) ViewState[ri.ClientID];
                    Role role = (Role) base.CoreRepository.GetObjectById(typeof (Role), roleId);
                    _activeUser.Roles.Add(role);
                }
            }

            // Check if the Adminstrator role is not accidently deleted for the superuser.
            string adminRole = Config.GetConfiguration()["AdministratorRole"];
            if (_activeUser.UserName == Config.GetConfiguration()["SuperUser"]
                && ! _activeUser.IsInRole(adminRole))
            {
                throw new Exception(String.Format("Người dùng '{0}' phải đóng vai trò '{1}'."
                                                  , _activeUser.UserName, adminRole));
            }
        }

        private void SaveUser()
        {
            try
            {
                SetRoles();
                if (_activeUser.Id == -1)
                {
                    base.CoreRepository.SaveObject(_activeUser);
                    Context.Response.Redirect("UserEdit.aspx?UserId=" + _activeUser.Id +
                                              "&message=User created successfully");
                }
                else
                {
                    base.CoreRepository.UpdateObject(_activeUser);
                    ShowMessage("Thông tin người dùng đã lưu");
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Context.Response.Redirect("Users.aspx");
        }

        private void rptRoles_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Role role = e.Item.DataItem as Role;
            if (role != null)
            {
                CheckBox chkRole = (CheckBox) e.Item.FindControl("chkRole");
                chkRole.Checked = _activeUser.IsInRole(role);
                // Add RoleId to the ViewState with the ClientID of the repeateritem as key.
                ViewState[e.Item.ClientID] = role.Id;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                if (_activeUser.Id == -1)
                {
                    _activeUser.UserName = txtUsername.Text;
                }
                else
                {
                    _activeUser.UserName = lblUsername.Text;
                }
                if (txtFirstname.Text.Length > 0)
                {
                    _activeUser.FirstName = txtFirstname.Text;
                }
                if (txtLastname.Text.Length > 0)
                {
                    _activeUser.LastName = txtLastname.Text;
                }
                _activeUser.Email = txtEmail.Text;
                _activeUser.Website = txtWebsite.Text;
                _activeUser.IsActive = chkActive.Checked;
                _activeUser.IsAgency = chkIsAgency.Checked;
                _activeUser.TimeZone = Int32.Parse(ddlTimeZone.SelectedValue);

                if (txtPassword1.Text.Length > 0 && txtPassword2.Text.Length > 0)
                {
                    try
                    {
                        _activeUser.Password = Core.Domain.User.HashPassword(txtPassword1.Text);
                    }
                    catch (Exception ex)
                    {
                        ShowError(ex.Message);
                    }
                }

                if (_activeUser.Id == -1 && _activeUser.Password == null)
                {
                    ShowError("Phải có mật khẩu");
                }
                else
                {
                    SaveUser();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_activeUser.Id > 0)
            {
                if (_activeUser.Id != ((User) Page.User.Identity).Id)
                {
                    if (_activeUser.UserName != Config.GetConfiguration()["SuperUser"])
                    {
                        try
                        {
                            base.CoreRepository.DeleteObject(_activeUser);
                            Context.Response.Redirect("Users.aspx");
                        }
                        catch (Exception ex)
                        {
                            ShowError(ex.Message);
                        }
                    }
                    else
                    {
                        ShowError("Bạn không thể xóa người dùng này.");
                    }
                }
                else
                {
                    ShowError("Bạn không thể tự xóa chính mình.");
                }
            }
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            this.Load += new System.EventHandler(this.Page_Load);
        }

        #endregion
    }
}