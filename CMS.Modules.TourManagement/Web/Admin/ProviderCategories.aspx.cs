using System;
using System.Web.UI.WebControls;
using CMS.Modules.TourManagement.Domain;
using CMS.Modules.TourManagement.Web.UI;

namespace CMS.Modules.TourManagement.Web.Admin
{
    public partial class ProviderCategories : TourAdminBasePage
    {
        #region -- PRIVATE MEMMBERS --

        private ProviderCategory _activeTourRegion;

        /// <summary>
        /// Biến ViewState lưu Service hiện tại
        /// </summary>
        private ProviderCategory ActiveTourRegion
        {
            get
            {
                if (_activeTourRegion != null)
                {
                    return _activeTourRegion;
                }
                if (ViewState["boatTypeId"] != null && Convert.ToInt32(ViewState["boatTypeId"]) > 0)
                {
                    return Module.ProviderCategoryGetById(Convert.ToInt32(ViewState["boatTypeId"]));
                }
                _activeTourRegion = new ProviderCategory();
                return _activeTourRegion;
            }
            set
            {
                _activeTourRegion = value;
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
            Title = Resources.labelTourRegions;
            if (!IsPostBack)
            {
                repeaterServices.DataSource = Module.ProviderCategoryGetAll(); //Module.TourRegionGetAllRoot();
                repeaterServices.DataBind();
                labelFormTitle.Text = Resources.labelNewTourRegion;
                BindParent();
                btnDelete.Visible = false;
                btnDelete.Enabled = false;
            }
        }

        private void BindParent()
        {
            //ddlParent.DataSource = Module.TourRegionGetAllRoot();
            //ddlParent.DataTextField = "Name";
            //ddlParent.DataValueField = "Id";
            //ddlParent.DataBind();
            //ddlParent.Items.Insert(0, new ListItem(" -- Parent --", "0"));
        }

        #endregion

        #region -- CONTROL EVENTS --

        #region -- Repeater --

        protected void repeaterServices_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            ProviderCategory service = e.Item.DataItem as ProviderCategory;
            if (service != null)
            {
                using (LinkButton linkServiceEdit = e.Item.FindControl("linkButtonServiceEdit") as LinkButton)
                {
                    if (linkServiceEdit != null)
                    {
                        // Gán text và command argument, điều này cũng có thể làm ngay trên aspx
                        linkServiceEdit.Text = service.Name;
                        linkServiceEdit.CommandArgument = service.Id.ToString();
                    }
                }

                //Repeater repeaterSubCategories = e.Item.FindControl("repeaterSubCategories") as Repeater;
                //if (repeaterSubCategories != null)
                //{
                //    if (service.Children.Count > 0)
                //    {
                //        repeaterSubCategories.DataSource = service.Children;
                //        repeaterSubCategories.DataBind();
                //    }
                //}
            }
        }

        protected void repeaterServices_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName.ToLower())
            {
                case "edit":

                    #region -- Lấy thông tin dịch vụ

                    ActiveTourRegion = Module.ProviderCategoryGetById(Convert.ToInt32(e.CommandArgument));
                    textBoxServiceName.Text = ActiveTourRegion.Name;
                    textBoxOrder.Text = ActiveTourRegion.Order.ToString();
                    //if (ActiveTourRegion.Parent != null)
                    //{
                    //    ddlParent.SelectedValue = ActiveTourRegion.Parent.Id.ToString();
                    //}
                    //else
                    //{
                    //    ddlParent.SelectedIndex = 0;
                    //}

                    #endregion

                    btnDelete.Visible = true;
                    btnDelete.Enabled = true;
                    labelFormTitle.Text = ActiveTourRegion.Name;
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
            ActiveTourRegion.Name = textBoxServiceName.Text;
            ActiveTourRegion.Order = Convert.ToInt32(textBoxOrder.Text);
            //if (ddlParent.SelectedIndex > 0 && ActiveTourRegion.Id.ToString() != ddlParent.SelectedValue)
            //{
            //    ActiveTourRegion.Parent = Module.TourRegionGetById(Convert.ToInt32(ddlParent.SelectedValue));
            //}
            //else
            //{
            //    ActiveTourRegion.Parent = null;
            //}
            // Kiểm tra trong View State
            Module.SaveOrUpdate(ActiveTourRegion);
            ActiveTourRegion = ActiveTourRegion;
            labelFormTitle.Text = ActiveTourRegion.Name;
            repeaterServices.DataSource = Module.ProviderCategoryGetAll(); //Module.TourRegionGetAllRoot();
            repeaterServices.DataBind();
        }

        /// <summary>
        /// Khi ấn nút Add New, đưa trạng thái trở về thêm mới, xóa các textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            ActiveTourRegion = new ProviderCategory();
            textBoxServiceName.Text = String.Empty;
            labelFormTitle.Text = Resources.labelNewTourRegion;
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
            Module.Delete(ActiveTourRegion);
            btnAddNew_Click(sender, e);
            repeaterServices.DataSource = Module.ProviderCategoryGetAll(); //Module.TourRegionGetAllRoot();
            repeaterServices.DataBind();
        }

        #endregion

        #endregion
    }
}
