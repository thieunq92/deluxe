using System;
using System.Collections;
using System.Web.UI.WebControls;
using CMS.Core.Domain;
using CMS.Core.Util;
using CMS.Modules.TourManagement.Domain;
using CMS.Modules.TourManagement.Web.UI;
using CMS.ServerControls.FileUpload;
using CMS.Web.Util;
using log4net;

namespace CMS.Modules.TourManagement.Web.Admin
{
    public partial class TourEdit : TourAdminBasePage
    {
        #region -- Private Method --

        private readonly ILog logger = LogManager.GetLogger(typeof (TourEdit));
        private Tour _tour;

        #endregion

        #region -- Page Event --

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region -- Ajax uploader init --

                if (!IsPostBack)
                {
                    #region -- Ajax Map --

                    FileUploaderMap.addCustomJS(FileUploaderAJAX.customJSevent.postUpload,
                                                FileHelper.InsertImagePostUploadJS("divMap", txtHiddenMap));
                    FileUploaderMap.addCustomJS(FileUploaderAJAX.customJSevent.postDelete,
                                                FileHelper.ClearData("divMap", txtHiddenMap));
                    FileUploaderMap.addCustomJS(FileUploaderAJAX.customJSevent.postHide,
                                                FileHelper.ClearData("divMap", txtHiddenMap));

                    #endregion

                    #region -- Ajax Image --

                    FileUploaderImage.addCustomJS(FileUploaderAJAX.customJSevent.postUpload,
                                                  FileHelper.InsertImagePostUploadJS("divImage", txtHiddenImage));
                    FileUploaderImage.addCustomJS(FileUploaderAJAX.customJSevent.postDelete,
                                                  FileHelper.ClearData("divImage", txtHiddenImage));
                    FileUploaderImage.addCustomJS(FileUploaderAJAX.customJSevent.postHide,
                                                  FileHelper.ClearData("divImage", txtHiddenImage));

                    #endregion
                }

                #endregion

                #region -- File uploading --

                if (FileUploaderImage.IsPosting)
                {
                    FileHelper.ManageAjaxPost(FileUploaderImage, 0, "Image\\Tour\\", HttpPostedFileAJAX.fileType.image);
                    return;
                }

                if (FileUploaderMap.IsPosting)
                {
                    FileHelper.ManageAjaxPost(FileUploaderMap, 0, "Image\\Tour\\", HttpPostedFileAJAX.fileType.image);
                    return;
                }

                #endregion

                Title = Resources.labelTourEdit;
                if (!IsPostBack)
                {
                    BindTourTypes();
                    BindRegions();
                    BindProviders();
                    ddlPackageType.DataSource = Enum.GetNames(typeof (PackageStatus));
                    ddlPackageType.DataBind();
                    if (!string.IsNullOrEmpty(Request.QueryString["TourId"]) &&
                        Convert.ToInt32(Request.QueryString["TourId"]) > 0)
                    {
                        if (!UserIdentity.CanModify(Section))
                        {
                            ShowError(Resources.textAccessDenied);
                            panelContent.Visible = false;
                            return;
                        }
                        _tour = Module.TourGetById(Convert.ToInt32(Request.QueryString["TourId"]));
                        LoadTourInfo();
                    }
                    else
                    {
                        if (!UserIdentity.CanInsert(Section))
                        {
                            ShowError(Resources.textAccessDenied);
                            panelContent.Visible = false;
                            return;
                        }
                    }
                    BindCountries();
                    BindCountriesEnd();
                }
            }
            catch (Exception ex)
            {
                logger.Error("Page load error", ex);
                ShowError(ex.Message);
            }
        }

        #endregion

        #region -- Control Event --

        protected void ButtonSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["TourId"] != null && Convert.ToInt32(Request.QueryString["TourId"]) > 0)
                {
                    _tour = Module.TourGetById(Convert.ToInt32(Request.QueryString["TourId"]));
                }
                else
                {
                    _tour = new Tour();
                }
                // 1. Thêm thông tin khách sạn.
                GetTourInfo();
                // 2. Save / Update
                if (_tour.Id > 0)
                {
                    _tour.ModifiedBy = ((User) Page.User.Identity).Id;
                    _tour.ModifiedDate = DateTime.Now;
                    Module.Update(_tour);
                }
                else
                {
                    _tour.CreatedBy = ((User) Page.User.Identity).Id;
                    _tour.ModifiedBy = ((User) Page.User.Identity).Id;
                    _tour.ModifiedDate = DateTime.Now;
                    Module.Save(_tour);
                }

                // 3. Redirect.
                PageRedirect(string.Format("TourList.aspx?NodeId={0}&SectionId={1}&TourId={2}",
                                           Request.QueryString["NodeId"],
                                           Request.QueryString["SectionId"], _tour.Id));
            }
            catch (Exception ex)
            {
                logger.Error("Error when submit tour", ex);
                throw;
            }
        }

        #endregion

        #region -- Private Method --

        #region -- Bind Data --

        /// <summary>
        /// Load danh mục Quốc gia ra dropdownlist.
        /// </summary>
        private void BindCountries()
        {
            // Load quốc gia (quốc gia là Locations có Level=3).
            IList countries = Module.LocationGetAllByLevel(3);
            countries.Insert(0, new Location(Resources.dropdownItemChooseCountry, null, 0, "", ""));
            // Bind dữ liệu.
            dropDownCountry.DataSource = countries;
            dropDownCountry.DataTextField = "Name";
            dropDownCountry.DataValueField = "Id";
            dropDownCountry.DataBind();
            if (_tour != null && _tour.CountryStart != null)
            {
                dropDownCountry.SelectedValue = _tour.CountryStart.Id.ToString();
                BindArea(_tour.CountryStart.Id);
            }
        }

        /// <summary>
        /// Load danh mục Vùng miền thuộc một Quốc gia.
        /// </summary>
        private void BindArea(int countryId)
        {
            // Load 
            if (countryId > 0)
            {
                IList areas = Module.LocationGetByParentID(countryId);
                areas.Insert(0, new Location(Resources.dropdownItemChooseArea, null, 0, "", ""));
                if (areas.Count > 1)
                {
                    dropDownArea.Enabled = true;
                    // Bind dữ liệu.
                    dropDownArea.DataSource = areas;
                    dropDownArea.DataTextField = "Name";
                    dropDownArea.DataValueField = "Id";
                    dropDownArea.DataBind();
                    if (_tour != null && _tour.RegionStart != null)
                    {
                        dropDownArea.SelectedValue = _tour.RegionStart.Id.ToString();
                        BindCity(_tour.RegionStart.Id);
                    }
                }
                else
                {
                    dropDownArea.Enabled = false;
                }
            }
            else
            {
                dropDownCity.Enabled = false;
                BindCity(-1);
            }
        }

        /// <summary>
        /// Load danh mục Vùng miền thuộc một Quốc gia.
        /// </summary>
        private void BindCity(int areaId)
        {
            if (areaId <= 0)
            {
                dropdownListLocation.Enabled = false;
                return;
            }
            // Load 
            IList cities = Module.LocationGetByParentID(areaId);
            cities.Insert(0, new Location(Resources.dropdownItemChooseCity, null, 0, "", ""));
            if (cities.Count > 1)
            {
                // Bind dữ liệu.
                dropDownCity.Enabled = true;
                dropDownCity.DataSource = cities;
                dropDownCity.DataTextField = "Name";
                dropDownCity.DataValueField = "Id";
                dropDownCity.DataBind();
                if (_tour != null && _tour.CityStart != null)
                {
                    dropDownCity.SelectedValue = _tour.CityStart.Id.ToString();
                    if (_tour.CityStart.Children.Count > 0)
                    {
                        BindLocation(_tour.CityStart.Id);
                    }
                }
            }
            else
            {
                dropDownCity.Enabled = false;
            }
        }

        /// <summary>
        /// Load danh mục location trong một city
        /// </summary>
        private void BindLocation(int areaId)
        {
            // Load 
            IList locations = Module.LocationGetByParentID(areaId);
            locations.Insert(0, new Location(Resources.dropdownItemChooseDistrict, null, 0, "", ""));
            if (locations.Count > 1)
            {
                // Bind dữ liệu.
                dropdownListLocation.Enabled = true;
                dropdownListLocation.DataSource = locations;
                dropdownListLocation.DataTextField = "Name";
                dropdownListLocation.DataValueField = "Id";
                dropdownListLocation.DataBind();
                if (_tour != null && _tour.StartFrom != null && _tour.StartFrom.Level == 6)
                {
                    dropdownListLocation.SelectedValue = _tour.StartFrom.Id.ToString();
                }
            }
            else
            {
                dropdownListLocation.Enabled = false;
            }
        }

        /// <summary>
        /// Load danh mục Quốc gia ra dropdownlist.
        /// </summary>
        private void BindCountriesEnd()
        {
            // Load quốc gia (quốc gia là Locations có Level=3).
            IList countries = Module.LocationGetAllByLevel(3);
            countries.Insert(0, new Location(Resources.dropdownItemChooseCountry, null, 0, "", ""));
            // Bind dữ liệu.
            DropDownListCountryEnd.DataSource = countries;
            DropDownListCountryEnd.DataTextField = "Name";
            DropDownListCountryEnd.DataValueField = "Id";
            DropDownListCountryEnd.DataBind();
            if (_tour != null && _tour.CountryEnd != null)
            {
                DropDownListCountryEnd.SelectedValue = _tour.CountryEnd.Id.ToString();
                BindAreaEnd(_tour.CountryEnd.Id);
            }
        }

        /// <summary>
        /// Load danh mục Vùng miền thuộc một Quốc gia.
        /// </summary>
        private void BindAreaEnd(int countryId)
        {
            // Load 
            if (countryId > 0)
            {
                IList areas = Module.LocationGetByParentID(countryId);
                areas.Insert(0, new Location(Resources.dropdownItemChooseArea, null, 0, "", ""));
                if (areas.Count > 1)
                {
                    DropDownListRegionEnd.Enabled = true;
                    // Bind dữ liệu.
                    DropDownListRegionEnd.DataSource = areas;
                    DropDownListRegionEnd.DataTextField = "Name";
                    DropDownListRegionEnd.DataValueField = "Id";
                    DropDownListRegionEnd.DataBind();
                    if (_tour != null && _tour.RegionEnd != null)
                    {
                        DropDownListRegionEnd.SelectedValue = _tour.RegionEnd.Id.ToString();
                        BindCityEnd(_tour.RegionEnd.Id);
                    }
                }
                else
                {
                    DropDownListRegionEnd.Enabled = false;
                }
            }
            else
            {
                dropDownCity.Enabled = false;
                BindCity(-1);
            }
        }

        /// <summary>
        /// Load danh mục Vùng miền thuộc một Quốc gia.
        /// </summary>
        private void BindCityEnd(int areaId)
        {
            if (areaId <= 0)
            {
                DropDownListLocationEnd.Enabled = false;
                return;
            }
            // Load 
            IList cities = Module.LocationGetByParentID(areaId);
            cities.Insert(0, new Location(Resources.dropdownItemChooseCity, null, 0, "", ""));
            if (cities.Count > 1)
            {
                // Bind dữ liệu.
                DropDownListCityEnd.Enabled = true;
                DropDownListCityEnd.DataSource = cities;
                DropDownListCityEnd.DataTextField = "Name";
                DropDownListCityEnd.DataValueField = "Id";
                DropDownListCityEnd.DataBind();
                if (_tour != null && _tour.CityEnd != null)
                {
                    DropDownListCityEnd.SelectedValue = _tour.CityEnd.Id.ToString();
                    if (_tour.CityEnd.Children.Count > 0)
                    {
                        BindLocationEnd(_tour.CityEnd.Id);
                    }
                }
            }
            else
            {
                DropDownListCityEnd.Enabled = false;
            }
        }

        /// <summary>
        /// Load danh mục location trong một city
        /// </summary>
        private void BindLocationEnd(int areaId)
        {
            // Load 
            IList locations = Module.LocationGetByParentID(areaId);
            locations.Insert(0, new Location(Resources.dropdownItemChooseDistrict, null, 0, "", ""));
            if (locations.Count > 1)
            {
                // Bind dữ liệu.
                DropDownListLocationEnd.Enabled = true;
                DropDownListLocationEnd.DataSource = locations;
                DropDownListLocationEnd.DataTextField = "Name";
                DropDownListLocationEnd.DataValueField = "Id";
                DropDownListLocationEnd.DataBind();
                if (_tour != null && _tour.EndIn != null && _tour.EndIn.Level == 6)
                {
                    DropDownListLocationEnd.SelectedValue = _tour.EndIn.Id.ToString();
                }
            }
            else
            {
                DropDownListLocationEnd.Enabled = false;
            }
        }

        #endregion

        #region -- Cascading drop down --

        protected void dropDownCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dropDownCountry.SelectedValue != null)
            {
                if (Convert.ToInt32(dropDownCountry.SelectedValue) > 0)
                {
                    BindArea(Convert.ToInt32(dropDownCountry.SelectedValue));
                }
                else
                {
                    dropDownArea.Enabled = false;
                    BindArea(-1);
                }
                dropDownCity.Enabled = false;
                dropdownListLocation.Enabled = false;
            }
        }

        protected void dropDownArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dropDownCountry.SelectedValue != null)
            {
                if (Convert.ToInt32(dropDownArea.SelectedValue) > 0)
                {
                    BindCity(Convert.ToInt32(dropDownArea.SelectedValue));
                }
                else
                {
                    dropDownCity.Enabled = false;
                }
                dropdownListLocation.Enabled = false;
            }
        }

        protected void dropDownCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dropDownCountry.SelectedValue != null && dropDownArea.SelectedValue != null)
            {
                if (Convert.ToInt32(dropDownCity.SelectedValue) > 0)
                {
                    BindLocation(Convert.ToInt32(dropDownCity.SelectedValue));
                }
                else
                {
                    dropdownListLocation.Enabled = false;
                }
            }
        }

        #endregion

        #region -- Cascading drop down --

        protected void DropDownListCountryEnd_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListCountryEnd.SelectedValue != null)
            {
                if (Convert.ToInt32(DropDownListCountryEnd.SelectedValue) > 0)
                {
                    BindAreaEnd(Convert.ToInt32(DropDownListCountryEnd.SelectedValue));
                }
                else
                {
                    DropDownListRegionEnd.Enabled = false;
                    BindArea(-1);
                }
                DropDownListCityEnd.Enabled = false;
                DropDownListLocationEnd.Enabled = false;
            }
        }

        protected void DropDownListRegionEnd_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListCountryEnd.SelectedValue != null)
            {
                if (Convert.ToInt32(DropDownListRegionEnd.SelectedValue) > 0)
                {
                    BindCityEnd(Convert.ToInt32(DropDownListRegionEnd.SelectedValue));
                }
                else
                {
                    DropDownListCityEnd.Enabled = false;
                }
                DropDownListLocationEnd.Enabled = false;
            }
        }

        protected void DropDownListCityEnd_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListCountryEnd.SelectedValue != null && DropDownListRegionEnd.SelectedValue != null)
            {
                if (Convert.ToInt32(DropDownListCityEnd.SelectedValue) > 0)
                {
                    BindLocationEnd(Convert.ToInt32(DropDownListCityEnd.SelectedValue));
                }
                else
                {
                    DropDownListLocationEnd.Enabled = false;
                }
            }
        }

        #endregion

        private void BindTourTypes()
        {
            ddlTourType.Items.Clear();
            IList roots = Module.TourTypeGetAllRoot();
            foreach (TourType region in roots)
            {
                ddlTourType.Items.Add(new ListItem(region.Name, region.Id.ToString()));
                foreach (TourType child in region.Children)
                {
                    ddlTourType.Items.Add(new ListItem(" |-- " + child.Name, child.Id.ToString()));
                    foreach (TourType gchild in child.Children)
                    {
                        ddlTourType.Items.Add(new ListItem(Text.Padding(5) + "|-- " + gchild.Name, gchild.Id.ToString()));
                    }
                }
            }
        }

        private void BindRegions()
        {
            ddlTourRegions.Items.Clear();
            IList roots = Module.TourRegionGetAllRoot();
            foreach (TourRegion region in roots)
            {
                ddlTourRegions.Items.Add(new ListItem(region.Name, region.Id.ToString()));
                foreach (TourRegion child in region.Children)
                {
                    ddlTourRegions.Items.Add(new ListItem(" |-- " + child.Name, child.Id.ToString()));
                    foreach (TourRegion gchild in child.Children)
                    {
                        ddlTourRegions.Items.Add(new ListItem(Text.Padding(5) + "|-- " + gchild.Name,
                                                              gchild.Id.ToString()));
                    }
                }
            }
        }

        private void BindProviders()
        {
            ddlProviders.DataSource = Module.ProviderGetAll(ProviderType.Mixed);
            ddlProviders.DataTextField = "Name";
            ddlProviders.DataValueField = "Id";
            ddlProviders.DataBind();
            ddlProviders.Items.Insert(0, "-- None --");
        }

        private void LoadTourInfo()
        {
            textBoxName.Text = _tour.Name;
            textBoxNumberOfDays.Text = _tour.NumberOfDay.ToString();
            textBoxGrade.Text = _tour.Grade.ToString();
            fckEditorActivities.Value = _tour.Activities;
            fckEditorDetailsIniterary.Value = _tour.DetailsIniterary;
            fckEditorExclusion.Value = _tour.Exclusion;
            fckEditorInclusion.Value = _tour.Inclusion;
            fckEditorNoteForOperator.Value = _tour.NoteForOperator;
            fckEditorNoteForSale.Value = _tour.NoteForSale;
            fckEditorSummary.Value = _tour.Summary;
            fckEditorTourInstruction.Value = _tour.TourInstruction;
            fckEditorTripHighLight.Value = _tour.TripHighLight;
            fckEditorWhatToTake.Value = _tour.WhatToTake;
            chkHalf.Checked = _tour.IsHalf;
            ddlTourType.SelectedValue = _tour.TourType.Id.ToString();
            FileUploaderImage.addCustomJS(FileUploaderAJAX.customJSevent.preLoad,
                                          FileHelper.InsertImagePostloadJS("divImage", txtHiddenImage, _tour.Image));
            FileUploaderMap.addCustomJS(FileUploaderAJAX.customJSevent.preLoad,
                                        FileHelper.InsertImagePostloadJS("divMap", txtHiddenMap, _tour.Map));

            if (_tour.TourRegion != null)
            {
                ddlTourRegions.SelectedValue = _tour.TourRegion.Id.ToString();
            }

            if (_tour.Provider != null)
            {
                ddlProviders.SelectedValue = _tour.Provider.Id.ToString();
            }

            if (_tour.PackageStatus != null)
            {
                ddlPackageType.SelectedIndex = (int) _tour.PackageStatus;
            }
        }

        private void GetTourInfo()
        {
            #region Địa điểm.

            #region -- Start --

            if (dropDownCountry.SelectedIndex > 0 && dropDownCountry.Enabled)
            {
                _tour.CountryStart = Module.LocationGetById(Int32.Parse(dropDownCountry.SelectedValue));
                _tour.StartFrom = _tour.CountryStart;
            }

            if (dropDownArea.SelectedIndex > 0 && dropDownArea.Enabled)
            {
                _tour.RegionStart = Module.LocationGetById(Int32.Parse(dropDownArea.SelectedValue));
                _tour.StartFrom = _tour.RegionStart;
            }

            if (dropDownCity.SelectedIndex > 0 && dropDownCity.Enabled)
            {
                _tour.CityStart = Module.LocationGetById(Int32.Parse(dropDownCity.SelectedValue));
                _tour.StartFrom = _tour.CityStart;
            }

            if (dropdownListLocation.SelectedIndex > 0 && dropdownListLocation.Enabled)
            {
                _tour.StartFrom = Module.LocationGetById(Int32.Parse(dropdownListLocation.SelectedValue));
            }

            #endregion

            #region -- End --

            if (DropDownListCountryEnd.SelectedIndex > 0 && DropDownListCountryEnd.Enabled)
            {
                _tour.CountryEnd = Module.LocationGetById(Int32.Parse(DropDownListCountryEnd.SelectedValue));
                _tour.EndIn = _tour.CountryEnd;
            }

            if (DropDownListRegionEnd.SelectedIndex > 0 && DropDownListRegionEnd.Enabled)
            {
                _tour.RegionEnd = Module.LocationGetById(Int32.Parse(DropDownListRegionEnd.SelectedValue));
                _tour.EndIn = _tour.RegionEnd;
            }

            if (DropDownListCityEnd.SelectedIndex > 0 && DropDownListCityEnd.Enabled)
            {
                _tour.CityEnd = Module.LocationGetById(Int32.Parse(DropDownListCityEnd.SelectedValue));
                _tour.EndIn = _tour.CityEnd;
            }

            if (DropDownListLocationEnd.SelectedIndex > 0 && DropDownListLocationEnd.Enabled)
            {
                _tour.EndIn = Module.LocationGetById(Int32.Parse(DropDownListLocationEnd.SelectedValue));
            }

            #endregion

            #endregion

            _tour.Name = textBoxName.Text;
            _tour.NumberOfDay = Convert.ToInt32(textBoxNumberOfDays.Text);
            _tour.Grade = Convert.ToInt32(textBoxGrade.Text);
            _tour.Activities = fckEditorActivities.Value;
            _tour.DetailsIniterary = fckEditorDetailsIniterary.Value;
            _tour.Exclusion = fckEditorExclusion.Value;
            _tour.Inclusion = fckEditorInclusion.Value;
            _tour.NoteForOperator = fckEditorNoteForOperator.Value;
            _tour.NoteForSale = fckEditorNoteForSale.Value;
            _tour.Summary = fckEditorSummary.Value;
            _tour.TourInstruction = fckEditorTourInstruction.Value;
            _tour.TripHighLight = fckEditorTripHighLight.Value;
            _tour.WhatToTake = fckEditorWhatToTake.Value;
            _tour.TourType = Module.TourTypeGetById(Convert.ToInt32(ddlTourType.SelectedValue));
            _tour.IsHalf = chkHalf.Checked;
            // Bản đồ và ảnh đại diện
            if (!string.IsNullOrEmpty(txtHiddenMap.Text))
            {
                _tour.Map = txtHiddenMap.Text;
            }
            else
            {
                _tour.Map = string.Empty;
            }
            if (!string.IsNullOrEmpty(txtHiddenImage.Text))
            {
                _tour.Image = txtHiddenImage.Text;
            }
            else
            {
                _tour.Image = string.Empty;
            }

            _tour.TourRegion = Module.TourRegionGetById(Convert.ToInt32(ddlTourRegions.SelectedValue));

            if (ddlProviders.SelectedIndex > 0)
            {
                _tour.Provider = Module.ProviderGetByID(Convert.ToInt32(ddlProviders.SelectedValue));
                _tour.PackageStatus = (PackageStatus) Enum.Parse(typeof (PackageStatus), ddlPackageType.SelectedValue);
            }
            else
            {
                _tour.Provider = null;
                _tour.PackageStatus = null;
            }
        }

        #endregion
    }
}