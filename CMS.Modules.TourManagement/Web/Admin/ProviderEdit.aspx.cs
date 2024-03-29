using System;
using System.Collections;
using System.Web.UI.WebControls;
using CMS.Core.Domain;
using CMS.Modules.TourManagement.Domain;
using CMS.Modules.TourManagement.Web.UI;
using CMS.ServerControls.FileUpload;
using CMS.Web.Util;
using log4net;

namespace CMS.Modules.TourManagement.Web.Admin
{
    public partial class ProviderEdit : TourAdminBasePage
    {
        #region --Private Member--

        private Provider _provider;
        private Hashtable _hashtable;
        private readonly ILog logger = LogManager.GetLogger(typeof (ProviderEdit));
        private IList _providerCategories;

        #endregion

        #region --Page Event--

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!UserIdentity.HasPermission(AccessLevel.Administrator))
                {
                    ShowError(Resources.textAccessDenied);
                    panelContent.Visible = false;
                    return;
                }
                Title = Resources.titleProviderEdit;

                if (Request.QueryString["ProviderID"] != null && Convert.ToInt32(Request.QueryString["ProviderID"]) > 0)
                {
                    _provider = Module.ProviderGetByID(Convert.ToInt32(Request.QueryString["ProviderID"]));
                }
                _providerCategories = Module.Provider_CategoryGetByProvider(_provider);
                _hashtable = new Hashtable(_providerCategories.Count);
                foreach (Provider_Category item in _providerCategories)
                {
                    _hashtable.Add(item.Category.Id, item.Id);
                }
                if (!IsPostBack)
                {
                    BindCountry();
                    ddlTypes.DataSource = Enum.GetNames(typeof(ProviderType));
                    ddlTypes.DataBind();
                    LoadInfo();

                    #region -- Ajax Image --

                    FileUploaderImage.addCustomJS(FileUploaderAJAX.customJSevent.postUpload,
                                                  FileHelper.InsertImagePostUploadJS("divImage", textBoxHiddenImage));
                    FileUploaderImage.addCustomJS(FileUploaderAJAX.customJSevent.postDelete,
                                                  FileHelper.ClearData("divImage", textBoxHiddenImage));
                    FileUploaderImage.addCustomJS(FileUploaderAJAX.customJSevent.postHide,
                                                  FileHelper.ClearData("divImage", textBoxHiddenImage));

                    #endregion

                    #region -- Ajax Map --

                    FileUploaderMap.addCustomJS(FileUploaderAJAX.customJSevent.postUpload,
                                                FileHelper.InsertImagePostUploadJS("divMap", textBoxHiddenMap));
                    FileUploaderMap.addCustomJS(FileUploaderAJAX.customJSevent.postDelete,
                                                FileHelper.ClearData("divMap", textBoxHiddenMap));
                    FileUploaderMap.addCustomJS(FileUploaderAJAX.customJSevent.postHide,
                                                FileHelper.ClearData("divMap", textBoxHiddenMap));

                    #endregion
                }

                if (FileUploaderImage.IsPosting)
                {
                    FileHelper.ManageAjaxPost(FileUploaderImage, 0, "Image\\TourManagement\\",
                                              HttpPostedFileAJAX.fileType.image);
                }

                if (FileUploaderMap.IsPosting)
                {
                    FileHelper.ManageAjaxPost(FileUploaderMap, 0, "Image\\TourManagement\\",
                                              HttpPostedFileAJAX.fileType.image);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error when Page_Load in ProviderEdit", ex);
                throw;
            }
        }

        #endregion

        #region --Private Method--

        private void LoadInfo()
        {
            if (_provider!=null && _provider.Id> 0)
            {
                _provider = Module.ProviderGetByID(Convert.ToInt32(Request.QueryString["ProviderID"]));

                Title = Resources.titleProviderEdit;
                textBoxName.Text = _provider.Name;
                textBoxAddress.Text = _provider.Address;
                textBoxWebsite.Text = _provider.Website;
                textBoxTel.Text = _provider.Tel;
                textBoxMobile.Text = _provider.Mobile;
                textBoxFax.Text = _provider.Fax;
                textBoxHiddenMap.Text = _provider.Map;
                textBoxHiddenImage.Text = _provider.Image;
                FCKDescription.Value = _provider.Description;
                ddlTypes.SelectedIndex = (int) _provider.ProviderType;
                rptCategory.DataSource = Module.ProviderCategoryGetAll();
                rptCategory.DataBind();

                FileUploaderImage.addCustomJS(FileUploaderAJAX.customJSevent.preLoad,
                              FileHelper.InsertImagePostloadJS("divImage", textBoxHiddenImage, _provider.Image));
                FileUploaderMap.addCustomJS(FileUploaderAJAX.customJSevent.preLoad,
                                            FileHelper.InsertImagePostloadJS("divMap", textBoxHiddenMap, _provider.Map));

                #region - Load Location -
                
                Location temp, tmpCountry, tmpRegion, tmpCity;
                temp = _provider.Location;

                switch (_provider.Location.Level)
                {
                        //Nếu Level là 3 thì LocationFrom chính là Country
                    case 3:
                        BindCountry();
                        ddlCountry.SelectedValue = temp.Id.ToString();
                        BindRegion(temp.Id);
                        break;
                        //Nếu Level là 4 thì LocationFrom chính là Region
                    case 4:
                        tmpCountry = temp.Parent;
                        BindCountry();
                        ddlCountry.SelectedValue = tmpCountry.Id.ToString();
                        BindRegion(tmpCountry.Id);
                        ddlRegion.SelectedValue = temp.Id.ToString();
                        BindCity(temp.Id);
                        break;
                        //Nếu Level là 5 thì LocationFrom chính là City
                    case 5:
                        tmpRegion = temp.Parent;
                        tmpCountry = tmpRegion.Parent;
                        BindCountry();
                        ddlCountry.SelectedValue = tmpCountry.Id.ToString();
                        BindRegion(tmpCountry.Id);
                        ddlRegion.SelectedValue = tmpRegion.Id.ToString();
                        BindCity(tmpRegion.Id);
                        ddlCity.SelectedValue = temp.Id.ToString();
                        BindLocation(temp.Id);
                        break;
                        //Nếu Level là 6 thì LocationFrom chính là District
                    case 6:
                        tmpCity = temp.Parent;
                        tmpRegion = tmpCity.Parent;
                        tmpCountry = tmpRegion.Parent;
                        BindCountry();
                        ddlCountry.SelectedValue = tmpCountry.Id.ToString();
                        BindRegion(tmpCountry.Id);
                        ddlRegion.SelectedValue = tmpRegion.Id.ToString();
                        BindCity(tmpRegion.Id);
                        ddlCity.SelectedValue = tmpCity.Id.ToString();
                        BindLocation(tmpCity.Id);
                        ddlLocation.SelectedValue = temp.Id.ToString();
                        break;
                        //Hiện tại Không có trường hợp LocationFrom >6 vì khi tạo TransportRoute người dùng chỉ được chọn Location Cấp thấp nhất là 6
                    default:
                        break;
                }
                #endregion
            }
        }

        #region - Drop Down List -
        // Các hàm sử lý Dropdownlist Location

        public void BindCountry()
        {
            IList countries = Module.LocationGetAllByLevel(3);
            countries.Insert(0, new Location(Resources.ChooseCountry, null, 0, "", ""));
            ddlCountry.DataSource = countries;
            ddlCountry.DataTextField = "Name";
            ddlCountry.DataValueField = "ID";
            ddlCountry.DataBind();
        }

        public void BindRegion(int countryID)
        {
            IList region = Module.LocationGetByParentID(countryID);
            region.Insert(0, new Location(Resources.ChooseRegion, null, 0, "", ""));

            if (region.Count > 1)
            {
                ddlRegion.Enabled = true;
                ddlRegion.DataSource = region;
                ddlRegion.DataTextField = "Name";
                ddlRegion.DataValueField = "Id";
                ddlRegion.DataBind();
            }
            else
            {
                ddlRegion.SelectedIndex = 0;
                ddlRegion.Enabled = false;
            }
        }

        public void BindCity(int regionID)
        {
            IList cities = Module.LocationGetByParentID(regionID);
            cities.Insert(0, new Location(Resources.ChooseCity, null, 0, "", ""));
            if (cities.Count > 1)
            {
                ddlCity.Enabled = true;
                ddlCity.DataSource = cities;
                ddlCity.DataTextField = "Name";
                ddlCity.DataValueField = "Id";
                ddlCity.DataBind();
            }
            else
            {
                ddlCity.SelectedIndex = 0;
                ddlCity.Enabled = false;
            }
        }

        public void BindLocation(int cityID)
        {
            IList locations = Module.LocationGetByParentID(cityID);
            locations.Insert(0, new Location(Resources.ChooseDistrict, null, 0, "", ""));
            if (locations.Count > 1)
            {
                ddlLocation.Enabled = true;
                ddlLocation.DataSource = locations;
                ddlLocation.DataTextField = "Name";
                ddlLocation.DataValueField = "Id";
                ddlLocation.DataBind();
            }
            else
            {
                ddlLocation.SelectedIndex = 0;
                ddlLocation.Enabled = false;
            }
        }

        #endregion

        #endregion

        #region --Control Event--

        protected void buttonSubmit_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["ProviderID"] != null && Convert.ToInt32(Request.QueryString["ProviderID"]) > 0)
                {
                    _provider = Module.ProviderGetByID(Convert.ToInt32(Request.QueryString["ProviderID"]));
                }
                else
                {
                    _provider = new Provider();
                    _provider.CreatedBy = ((User) Page.User.Identity).Id;
                }
                _provider.ModifiedBy = ((User) Page.User.Identity).Id;

                _provider.Name = textBoxName.Text;
                _provider.Address = textBoxAddress.Text;
                _provider.Website = textBoxWebsite.Text;
                _provider.Tel = textBoxTel.Text;
                _provider.Mobile = textBoxMobile.Text;
                _provider.Fax = textBoxFax.Text;
                _provider.Description = FCKDescription.Value;
                _provider.Map = textBoxHiddenMap.Text;
                _provider.Image = textBoxHiddenImage.Text;
                _provider.ProviderType = (ProviderType) ddlTypes.SelectedIndex;

                #region - Location
                
                if (ddlLocation.Enabled && ddlLocation.SelectedIndex > 0)
                {
                    _provider.Location = Module.LocationGetById(Convert.ToInt32(ddlLocation.SelectedValue));
                }
                else if (ddlCity.Enabled && ddlCity.SelectedIndex > 0)
                {
                    _provider.Location = Module.LocationGetById(Convert.ToInt32(ddlCity.SelectedValue));
                }
                else if (ddlRegion.Enabled && ddlRegion.SelectedIndex > 0)
                {
                    _provider.Location = Module.LocationGetById(Convert.ToInt32(ddlRegion.SelectedValue));
                }
                else if (ddlCountry.Enabled && ddlCountry.SelectedIndex > 0)
                {
                    _provider.Location = Module.LocationGetById(Convert.ToInt32(ddlCountry.SelectedValue));
                }
                else
                {
                    labelValidLocation.Visible = true;
                    return;
                }
                #endregion

                //foreach // Xử lý lưu dữ liệu
                //Đối với từng repeater
                foreach (RepeaterItem item in rptCategory.Items)
                {
                    HiddenField hiddenId = item.FindControl("hiddenId") as HiddenField;
                    CheckBox chkCategory = item.FindControl("chkCategory") as CheckBox;
                    if (hiddenId!=null && chkCategory!=null)
                    {
                        if (_hashtable[Convert.ToInt32(hiddenId.Value)]!=null && !chkCategory.Checked)
                        {
                            Module.Delete(Module.Provider_CategoryGetById(Convert.ToInt32(_hashtable[Convert.ToInt32(hiddenId.Value)])));
                        }
                        if (_hashtable[Convert.ToInt32(hiddenId.Value)]==null && chkCategory.Checked)
                        {
                            ProviderCategory category = Module.ProviderCategoryGetById(Convert.ToInt32(hiddenId.Value));
                            Module.SaveProviderCategory(_provider,category);
                        }
                    }
                }

                if (_provider.Id > 0)
                {
                    Module.Update(_provider);
                    labelUpdateStatus.Visible = true;
                }
                else
                {
                    Module.Save(_provider);
                    PageRedirect(string.Format("ProviderList.aspx?NodeId={0}&SectionId={1}", Node.Id, Section.Id));
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error when buttonSubmit_OnClick in ProviderEdit", ex);
                throw;
            }
        }

        /// <summary>
        /// Click vào cancel chuyển sang trang provider list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void buttonCancel_OnClick(object sender, EventArgs e)
        {
            PageRedirect(string.Format("ProviderList.aspx?NodeId={0}&SectionId={1}", Node.Id, Section.Id));
        }

        #region -- Drop Down List --

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlCountry.SelectedIndex > 0)
                {
                    if (Convert.ToInt32(ddlCountry.SelectedValue) > 0)
                    {
                        if (Module.LocationGetById(Convert.ToInt32(ddlCountry.SelectedValue)).Children.Count > 0)
                        {
                            ddlRegion.Enabled = true;
                            BindRegion(Convert.ToInt32(ddlCountry.SelectedValue));
                            ddlRegion.SelectedIndex = 0;
                        }
                        else
                        {
                            ddlRegion.Enabled = false;
                            ddlCity.Enabled = false;
                            ddlLocation.Enabled = false;
                        }
                        ddlCity.Enabled = false;
                        ddlLocation.Enabled = false;
                    }
                    else
                    {
                        ddlRegion.Enabled = false;
                        ddlCity.Enabled = false;
                        ddlLocation.Enabled = false;
                    }
                }
                else
                {
                    ddlRegion.Enabled = false;
                    ddlCity.Enabled = false;
                    ddlLocation.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                logger.Error("error when ddlCountry_SelectedIndexChanged in ProviderEdit", ex);
                throw;
            }
        }

        protected void ddlRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlRegion.SelectedIndex > 0)
                {
                    if (Convert.ToInt32(ddlRegion.SelectedValue) > 0)
                    {
                        if (Module.LocationGetById(Convert.ToInt32(ddlRegion.SelectedValue)).Children.Count > 0)
                        {
                            ddlCity.Enabled = true;
                            BindCity(Convert.ToInt32(ddlRegion.SelectedValue));
                            ddlCity.SelectedIndex = 0;
                        }
                        else
                        {
                            ddlCity.Enabled = false;
                            ddlLocation.Enabled = false;
                        }
                        ddlLocation.Enabled = false;
                    }
                    else
                    {
                        ddlCity.Enabled = false;
                        ddlLocation.Enabled = false;
                    }
                }
                else
                {
                    ddlCity.Enabled = false;
                    ddlLocation.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error when ddlRegion_SelectedIndexChanged in ProviderEdit", ex);
                throw;
            }
        }

        protected void ddlCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlCity.SelectedIndex > 0)
                {
                    if (Convert.ToInt32(ddlCity.SelectedValue) > 0)
                    {
                        if (Module.LocationGetById(Convert.ToInt32(ddlCity.SelectedValue)).Children.Count > 0)
                        {
                            ddlLocation.Enabled = true;
                            BindLocation(Convert.ToInt32(ddlCity.SelectedValue));
                            ddlLocation.SelectedIndex = 0;
                        }
                        else
                        {
                            ddlLocation.Enabled = false;
                        }
                    }
                    else
                    {
                        ddlLocation.Enabled = false;
                    }
                }
                else
                {
                    ddlLocation.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error when ddlCity_SelectedIndexChanged in ProviderEdit", ex);
                throw;
            }
        }

        #endregion


        #endregion

        protected void rptCategory_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is ProviderCategory)
            {
                ProviderCategory category = (ProviderCategory) e.Item.DataItem;
                CheckBox chkCategory = e.Item.FindControl("chkCategory") as CheckBox;
                if (chkCategory!=null)
                {
                    chkCategory.Text = category.Name;
                    if (_hashtable[category.Id]!=null)
                    {
                        chkCategory.Checked = true;
                    }
                    else
                    {
                        chkCategory.Checked = false;
                    }
                }
            }
        }
    }
}