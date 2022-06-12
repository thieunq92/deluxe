using System;
using System.Collections;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.UI;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class Organizations : SailsAdminBasePage
    {
        #region -- PRIVATE MEMMBERS --

        private Organization _activeOrganization;

        /// <summary>
        /// Biến ViewState lưu Service hiện tại
        /// </summary>
        private Organization ActiveOrganization
        {
            get
            {
                if (_activeOrganization != null)
                {
                    return _activeOrganization;
                }
                if (ViewState["boatTypeId"] != null && Convert.ToInt32(ViewState["boatTypeId"]) > 0)
                {
                    return Module.OrganizationGetById(Convert.ToInt32(ViewState["boatTypeId"]));
                }
                _activeOrganization = new Organization();
                return _activeOrganization;
            }
            set
            {
                _activeOrganization = value;
                ViewState["boatTypeId"] = value.Id;
            }
        }

        #endregion

        #region -- PAGE EVENTS --

        /// <summary>
        /// Khi load, đưa trạng thái về add new
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = @"Region management"; //Resources.labelOrganizations;
            if (!IsPostBack)
            {
                repeaterServices.DataSource = Module.OrganizationGetAllRoot();
                repeaterServices.DataBind();
                labelFormTitle.Text = @"New region";//Resources.labelNewOrganization;
                BindParent();
                btnDelete.Visible = false;
                btnDelete.Enabled = false;
            }
        }

        private void BindParent()
        {
            ddlParent.Items.Clear();
            ddlParent.Items.Add("-- Root --");
            IList roots = Module.OrganizationGetAllRoot();
            foreach (Organization region in roots)
            {
                ddlParent.Items.Add(new ListItem(region.Name, region.Id.ToString()));
                foreach (Organization child in region.Children)
                {
                    ddlParent.Items.Add(new ListItem(" |-- " + child.Name, child.Id.ToString()));
                }
            }
        }

        #endregion

        #region -- CONTROL EVENTS --

        #region -- Repeater --

        protected void repeaterServices_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var service = e.Item.DataItem as Organization;
            if (service != null)
            {
                using (var linkServiceEdit = e.Item.FindControl("linkButtonServiceEdit") as LinkButton)
                {
                    if (linkServiceEdit != null)
                    {
                        // Gán text và command argument, điều này cũng có thể làm ngay trên aspx
                        linkServiceEdit.Text = service.Name;
                        linkServiceEdit.CommandArgument = service.Id.ToString();
                    }
                }

                var repeaterSubCategories = e.Item.FindControl("repeaterSubCategories") as Repeater;
                if (repeaterSubCategories != null)
                {
                    if (service.Children.Count > 0)
                    {
                        repeaterSubCategories.DataSource = service.Children;
                        repeaterSubCategories.DataBind();
                    }
                }
            }
        }

        protected void repeaterServices_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName.ToLower())
            {
                case "edit":

                    #region -- Lấy thông tin dịch vụ

                    ActiveOrganization = Module.OrganizationGetById(Convert.ToInt32(e.CommandArgument));
                    textBoxServiceName.Text = ActiveOrganization.Name;
                    if (ActiveOrganization.Parent != null)
                    {
                        ddlParent.SelectedValue = ActiveOrganization.Parent.Id.ToString();
                    }
                    else
                    {
                        ddlParent.SelectedIndex = 0;
                    }

                    #endregion

                    btnDelete.Visible = true;
                    btnDelete.Enabled = true;
                    labelFormTitle.Text = ActiveOrganization.Name;
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region -- Insert, Update, Delete --

        /// <summary>
        /// Khi ấn nút Save, lưu Service nếu đang edit, insert nếu đang thêm mới
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            ActiveOrganization.Name = textBoxServiceName.Text;
            if (ddlParent.SelectedIndex > 0 && ActiveOrganization.Id.ToString() != ddlParent.SelectedValue)
            {
                ActiveOrganization.Parent = Module.OrganizationGetById(Convert.ToInt32(ddlParent.SelectedValue));
            }
            else
            {
                ActiveOrganization.Parent = null;
            }
            // Kiểm tra trong View State
            Module.SaveOrUpdate(ActiveOrganization);
            ActiveOrganization = ActiveOrganization;
            labelFormTitle.Text = ActiveOrganization.Name;
            repeaterServices.DataSource = Module.OrganizationGetAllRoot();
            repeaterServices.DataBind();
        }

        /// <summary>
        /// Khi ấn nút Add New, đưa trạng thái trở về thêm mới, xóa các textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            ActiveOrganization = new Organization();
            textBoxServiceName.Text = String.Empty;
            labelFormTitle.Text = ""; //Resources.labelNewOrganization;
            fckEditorDescription.Value = string.Empty;
            btnDelete.Visible = false;
            btnDelete.Enabled = false;
            BindParent();
        }

        /// <summary>
        /// Khi ấn nút Delete, xóa service hiện thời (lưu trong ViewState)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Module.Delete(ActiveOrganization);
            btnAddNew_Click(sender, e);
            repeaterServices.DataSource = Module.OrganizationGetAllRoot();
            repeaterServices.DataBind();
        }

        #endregion

        #endregion
    }
}