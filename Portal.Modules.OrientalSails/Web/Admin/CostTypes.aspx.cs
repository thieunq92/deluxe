using System;
using System.Web.UI.WebControls;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.UI;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class CostTypes : SailsAdminBase
    {
        #region -- PRIVATE MEMBERS --

        private CostType _activeCost;

        /// <summary>
        /// Biến ViewState lưu Service hiện tại
        /// </summary>
        private CostType ActiveCost
        {
            get
            {
                if (_activeCost != null)
                {
                    return _activeCost;
                }
                if (ViewState["serviceId"] != null && Convert.ToInt32(ViewState["serviceId"]) > 0)
                {
                    return Module.CostTypeGetById(Convert.ToInt32(ViewState["serviceId"]));
                }
                _activeCost = new CostType();
                return _activeCost;
            }
            set
            {
                _activeCost = value;
                ViewState["serviceId"] = value.Id;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = @"Services";
            if (!IsPostBack)
            {
                rptServices.DataSource = Module.CostTypeGetAll();
                rptServices.DataBind();
                labelFormTitle.Text = @"New service";
                btnDelete.Visible = false;
                btnDelete.Enabled = false;

                ddlSuppliers.DataSource = Module.SupplierGetAll(ActiveCost.Id);
                ddlSuppliers.DataTextField = "Name";
                ddlSuppliers.DataValueField = "Id";
                ddlSuppliers.DataBind();
                ddlSuppliers.Items.Insert(0, "-- Default suppliers --");

                ddlServices.DataSource = Module.ExtraOptionGetAll();
                ddlServices.DataTextField = "Name";
                ddlServices.DataValueField = "Id";
                ddlServices.DataBind();
                ddlServices.Items.Insert(0, "-- Associated service --");
            }
        }

        protected void rptServices_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            CostType service = e.Item.DataItem as CostType;
            if (service != null)
            {
                using (LinkButton lbtEdit = e.Item.FindControl("lbtEdit") as LinkButton)
                {
                    if (lbtEdit != null)
                    {
                        // Gán text và command argument, điều này cũng có thể làm ngay trên aspx
                        lbtEdit.Text = service.Name;
                        lbtEdit.CommandArgument = service.Id.ToString();
                    }
                }
            }
        }

        protected void rptServices_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName.ToLower())
            {
                case "edit":

                    #region -- Lấy thông tin dịch vụ

                    ActiveCost = Module.CostTypeGetById(Convert.ToInt32(e.CommandArgument));
                    txtServiceName.Text = ActiveCost.Name;
                    txtGroupName.Text = ActiveCost.GroupName;
                    chkIsPayNow.Checked = ActiveCost.IsPayNow;
                    chkIsDailyInput.Checked = ActiveCost.IsDailyInput;
                    chkIsCustomType.Checked = ActiveCost.IsCustomType;
                    chkIsSupplier.Checked = ActiveCost.IsSupplier;
                    chkIsMonthly.Checked = ActiveCost.IsMonthly;
                    chkIsYearly.Checked = ActiveCost.IsYearly;
                    chkIsDaily.Checked = ActiveCost.IsDaily;
                    if (ActiveCost.DefaultAgency != null)
                    {
                        ListItem item = ddlSuppliers.Items.FindByValue(ActiveCost.DefaultAgency.Id.ToString());
                        ddlSuppliers.SelectedItem.Selected = false;
                        if (item != null)
                            item.Selected = true;
                    }

                    if (ActiveCost.Service != null)
                    {
                        ddlServices.SelectedValue = ActiveCost.Service.Id.ToString();
                    }
                    else
                    {
                        ddlServices.SelectedIndex = 0;
                    }

                    #endregion

                    btnDelete.Visible = true;
                    btnDelete.Enabled = true;
                    labelFormTitle.Text = ActiveCost.Name;
                    break;
                default:
                    break;
            }
        }

        #region -- Insert, Update, Delete --

        /// <summary>
        /// Khi ấn nút Save, lưu Service nếu đang edit, insert nếu đang thêm mới
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            ActiveCost.Name = txtServiceName.Text;
            ActiveCost.GroupName = txtGroupName.Text;
            ActiveCost.IsPayNow = chkIsPayNow.Checked;
            ActiveCost.IsDailyInput = chkIsDailyInput.Checked;
            ActiveCost.IsCustomType = chkIsCustomType.Checked;
            ActiveCost.IsSupplier = chkIsSupplier.Checked;
            ActiveCost.IsMonthly = chkIsMonthly.Checked;
            ActiveCost.IsDaily = chkIsDaily.Checked;
            ActiveCost.IsYearly = chkIsYearly.Checked;
            if (ddlSuppliers.SelectedIndex > 0)
            {
                ActiveCost.DefaultAgency = Module.AgencyGetById(Convert.ToInt32(ddlSuppliers.SelectedValue));
            }
            else
            {
                ActiveCost.DefaultAgency = null;
            }

            if (ddlServices.SelectedIndex > 0)
            {
                ActiveCost.Service = Module.ExtraOptionGetById(Convert.ToInt32(ddlServices.SelectedValue));
            }
            else
            {
                ActiveCost.Service = null;
            }

            // Kiểm tra trong View State
            Module.SaveOrUpdate(ActiveCost);
            ActiveCost = ActiveCost;
            labelFormTitle.Text = ActiveCost.Name;
            rptServices.DataSource = Module.CostTypeGetAll();
            rptServices.DataBind();
        }

        /// <summary>
        /// Khi ấn nút Add New, đưa trạng thái trở về thêm mới, xóa các textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ActiveCost = new CostType();
            txtServiceName.Text = String.Empty;
            txtGroupName.Text = string.Empty;
            labelFormTitle.Text = @"New service";
            chkIsPayNow.Checked = false;
            chkIsDailyInput.Checked = false;
            chkIsCustomType.Checked = false;
            chkIsSupplier.Checked = true;
            chkIsMonthly.Checked = false;
            chkIsDaily.Checked = false;
            chkIsYearly.Checked = false;
            ddlSuppliers.SelectedIndex = 0;
            ddlServices.SelectedIndex = 0;
            btnDelete.Visible = false;
            btnDelete.Enabled = false;
        }

        /// <summary>
        /// Khi ấn nút Delete, xóa service hiện thời (lưu trong ViewState)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Module.Delete(ActiveCost);
            btnAdd_Click(sender, e);
            rptServices.DataSource = Module.CostTypeGetAll();
            rptServices.DataBind();
        }

        #endregion
    }
}
