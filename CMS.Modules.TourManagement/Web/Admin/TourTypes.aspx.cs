using System;
using System.Collections;
using System.Web.UI.WebControls;
using CMS.Modules.TourManagement.Domain;
using CMS.Modules.TourManagement.Web.UI;

namespace CMS.Modules.TourManagement.Web.Admin
{
    public partial class TourTypes : TourAdminBasePage
    {
        #region -- PRIVATE MEMMBERS --

        private TourType _activeServices;

        /// <summary>
        /// Biến ViewState lưu Service hiện tại
        /// </summary>
        private TourType ActiveService
        {
            get
            {
                if (_activeServices != null)
                {
                    return _activeServices;
                }
                if (ViewState["serviceId"] != null && Convert.ToInt32(ViewState["serviceId"]) > 0)
                {
                    return Module.TourTypeGetById(Convert.ToInt32(ViewState["serviceId"]));
                }
                _activeServices = new TourType();
                return _activeServices;
            }
            set
            {
                _activeServices = value;
                ViewState["serviceId"] = value.Id;
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
            Title = Resources.labelTourTypeManagement;
            if (!IsPostBack)
            {
                repeaterServices.DataSource = Module.TourTypeGetAllRoot();
                repeaterServices.DataBind();
                labelFormTitle.Text = Resources.labelNewTourType;
                btnDelete.Visible = false;
                btnDelete.Enabled = false;

                BindParent();
            }
        }

        protected void BindParent()
        {
            ddlParent.Items.Clear();
            ddlParent.Items.Add("-- Root --");
            IList roots = Module.TourTypeGetAllRoot();
            foreach (TourType region in roots)
            {                
                ddlParent.Items.Add(new ListItem(region.Name, region.Id.ToString()));
                foreach (TourType child in region.Children)
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
            TourType service = e.Item.DataItem as TourType;
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

                Repeater repeaterSubCategories = e.Item.FindControl("repeaterSubCategories") as Repeater;
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

                    ActiveService = Module.TourTypeGetById(Convert.ToInt32(e.CommandArgument));
                    textBoxServiceName.Text = ActiveService.Name;
                    fckEditorDescription.Value = ActiveService.Description;

                    #endregion

                    btnDelete.Visible = true;
                    btnDelete.Enabled = true;
                    labelFormTitle.Text = ActiveService.Name;
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
            ActiveService.Name = textBoxServiceName.Text;
            ActiveService.Description = fckEditorDescription.Value;
            if (ddlParent.SelectedIndex > 0 && ActiveService.Id.ToString() != ddlParent.SelectedValue)
            {
                ActiveService.Parent = Module.TourTypeGetById(Convert.ToInt32(ddlParent.SelectedValue));
            }
            else
            {
                ActiveService.Parent = null;
            }
            // Kiểm tra trong View State
            if (ActiveService.Id > 0)
            {
                Module.Update(ActiveService);
            }
            else
            {
                Module.Save(ActiveService);
            }
            ActiveService = ActiveService;
            repeaterServices.DataSource = Module.TourTypeGetAllRoot();
            repeaterServices.DataBind();
        }

        /// <summary>
        /// Khi ấn nút Add New, đưa trạng thái trở về thêm mới, xóa các textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            ActiveService = new TourType();
            textBoxServiceName.Text = String.Empty;
            fckEditorDescription.Value = String.Empty;
            labelFormTitle.Text = Resources.labelNewTourType;
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
            Module.Delete(ActiveService);
            btnAddNew_Click(sender, e);
            repeaterServices.DataSource = Module.TourTypeGetAllRoot();
            repeaterServices.DataBind();
        }

        #endregion

        #endregion
    }
}
