using System;
using System.Collections;
using System.Web.UI.WebControls;
using CMS.Core.Domain;
using CMS.Core.Util;
using CMS.Modules.TourManagement.Domain;
using CMS.Modules.TourManagement.Web.UI;
using CMS.ServerControls;
using log4net;

namespace CMS.Modules.TourManagement.Web.Admin
{
    public partial class TourList : TourAdminBasePage
    {
        #region -- Private Member --

        private readonly ILog logger = LogManager.GetLogger(typeof (TourList));
        private Location _cityEnd;
        private Location _cityStart;
        private Location _countryEnd;
        private Location _countryStart;
        private Location _locationEnd;
        private Location _locationStart;
        private Location _regionEnd;
        private Location _regionStart;

        #endregion

        #region -- Page Event --

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = Resources.labelTourList;
            if (!IsPostBack)
            {
                BindTourTypes();
                BindTourRegion();
                LoadFromQuery();
                BindCountries();
                BindCountriesEnd();
                rptTours.DataSource = GetDataSource();
                rptTours.DataBind();
            }
        }

        #endregion

        #region -- Private Method --

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                PageRedirect(string.Format("{3}.aspx?NodeId={0}&SectionId={1}{2}", Node.Id, Section.Id,
                                           BuildQueryString(), "TourList"));
            }
            catch (Exception ex)
            {
                logger.Error("Search failed", ex);
                ShowError(ex.Message);
            }
        }

        /// <summary>
        /// Lấy Id của địa điểm trong dropdown
        /// </summary>
        /// <returns></returns>
        private string GetLocationStart()
        {
            if (dropDownCity.SelectedIndex > 0)
            {
                return dropDownCity.SelectedValue;
            }

            if (dropDownArea.SelectedIndex > 0)
            {
                return dropDownArea.SelectedValue;
            }

            if (dropDownCountry.SelectedIndex > 0)
            {
                return dropDownCountry.SelectedValue;
            }
            return "";
        }

        /// <summary>
        /// Lấy Id của địa điểm trong dropdown
        /// </summary>
        /// <returns></returns>
        private string GetLocationEnd()
        {
            if (DropDownListCityEnd.SelectedIndex > 0)
            {
                return DropDownListCityEnd.SelectedValue;
            }

            if (DropDownListRegionEnd.SelectedIndex > 0)
            {
                return DropDownListRegionEnd.SelectedValue;
            }

            if (DropDownListCountryEnd.SelectedIndex > 0)
            {
                return DropDownListCountryEnd.SelectedValue;
            }
            return "";
        }

        private string BuildQueryString()
        {
            string query = string.Empty;
            string start = GetLocationStart();
            string end = GetLocationEnd();
            if (!string.IsNullOrEmpty(start))
            {
                query += string.Format("&StartId={0}", start);
            }

            if (!string.IsNullOrEmpty(end))
            {
                query += string.Format("&EndId={0}", start);
            }

            if (!string.IsNullOrEmpty(textBoxNumberOfDayLt.Text))
            {
                query += string.Format("&TimeLt={0}", textBoxNumberOfDayLt.Text);
            }

            if (!string.IsNullOrEmpty(textBoxNumberOfDayGt.Text))
            {
                query += string.Format("&TimeGt={0}", textBoxNumberOfDayGt.Text);
            }

            if (!string.IsNullOrEmpty(textBoxLengthLt.Text))
            {
                query += string.Format("&LengthLt={0}", textBoxLengthLt.Text);
            }

            if (!string.IsNullOrEmpty(textBoxLengthGt.Text))
            {
                query += string.Format("&LengthGt={0}", textBoxLengthGt.Text);
            }

            if (ddlTourTypes.SelectedIndex > 0)
            {
                query += string.Format("&Type={0}", ddlTourTypes.SelectedValue);
            }

            if (ddlTourRegion.SelectedIndex > 0)
            {
                query += string.Format("&Region={0}", ddlTourRegion.SelectedValue);
            }
            return query;
        }

        protected IList GetDataSource()
        {
            IList list = Module.TourSearchByQueryString(Request.QueryString);
            return list;
        }

        protected virtual void LoadFromQuery()
        {
            #region -- Location --

            if (!string.IsNullOrEmpty(Request.QueryString["StartId"]))
            {
                _locationStart = Module.LocationGetById(Convert.ToInt32(Request.QueryString["StartId"]));
                Location temp = _locationStart;
                while (temp.Level > 5)
                {
                    temp = temp.Parent;
                }
                if (temp.Level == 5) _cityStart = temp;

                while (temp.Level > 4)
                {
                    temp = temp.Parent;
                }
                if (temp.Level == 4) _regionStart = temp;

                while (temp.Level > 3)
                {
                    temp = temp.Parent;
                }
                if (temp.Level == 3) _countryStart = temp;
            }

            if (!string.IsNullOrEmpty(Request.QueryString["EndId"]))
            {
                _locationEnd = Module.LocationGetById(Convert.ToInt32(Request.QueryString["EndId"]));
                Location temp = _locationEnd;
                while (temp.Level > 5)
                {
                    temp = temp.Parent;
                }
                if (temp.Level == 5) _cityEnd = temp;

                while (temp.Level > 4)
                {
                    temp = temp.Parent;
                }
                if (temp.Level == 4) _regionEnd = temp;

                while (temp.Level > 3)
                {
                    temp = temp.Parent;
                }
                if (temp.Level == 3) _countryEnd = temp;
            }

            #endregion

            if (!string.IsNullOrEmpty(Request.QueryString["TimeLt"]))
            {
                textBoxNumberOfDayLt.Text = Request.QueryString["TimeLt"];
            }
            if (!string.IsNullOrEmpty(Request.QueryString["TimeGt"]))
            {
                textBoxNumberOfDayGt.Text = Request.QueryString["TimeGt"];
            }
            if (!string.IsNullOrEmpty(Request.QueryString["LengthLt"]))
            {
                textBoxLengthLt.Text = Request.QueryString["LengthLt"];
            }
            if (!string.IsNullOrEmpty(Request.QueryString["LengthGt"]))
            {
                textBoxLengthGt.Text = Request.QueryString["LengthGt"];
            }
            if (!string.IsNullOrEmpty(Request.QueryString["Type"]))
            {
                ddlTourTypes.SelectedValue = Request.QueryString["Type"];
            }
            if (!string.IsNullOrEmpty(Request.QueryString["Region"]))
            {
                ddlTourRegion.SelectedValue = Request.QueryString["Region"];
            }
        }

        #region -- Location --

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
            if (_countryStart != null)
            {
                dropDownCountry.SelectedValue = _countryStart.Id.ToString();
                BindArea(_countryStart.Id);
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
                    if (_regionStart != null && _regionStart != null)
                    {
                        dropDownArea.SelectedValue = _regionStart.Id.ToString();
                        BindCity(_regionStart.Id);
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
                if (_cityStart != null && _cityStart != null)
                {
                    dropDownCity.SelectedValue = _cityStart.Id.ToString();
                    if (_cityStart.Children.Count > 0)
                    {
                        BindLocation(_cityStart.Id);
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
                if (_locationStart != null && _locationStart.Level == 6)
                {
                    dropdownListLocation.SelectedValue = _locationStart.Id.ToString();
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
            if (_countryEnd != null)
            {
                DropDownListCountryEnd.SelectedValue = _countryEnd.Id.ToString();
                BindAreaEnd(_countryEnd.Id);
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
                    if (_regionEnd != null)
                    {
                        DropDownListRegionEnd.SelectedValue = _regionEnd.Id.ToString();
                        BindCityEnd(_regionEnd.Id);
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
                if (_cityEnd != null)
                {
                    DropDownListCityEnd.SelectedValue = _cityEnd.Id.ToString();
                    if (_cityEnd.Children.Count > 0)
                    {
                        BindLocationEnd(_cityEnd.Id);
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
                if (_locationEnd != null && _locationEnd.Level == 6)
                {
                    DropDownListLocationEnd.SelectedValue = _locationEnd.Id.ToString();
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

        #endregion

        private void BindTourTypes()
        {
            ddlTourTypes.Items.Clear();
            ddlTourTypes.Items.Add(" -- All --");
            IList roots = Module.TourTypeGetAllRoot();
            foreach (TourType region in roots)
            {
                ddlTourTypes.Items.Add(new ListItem(region.Name, region.Id.ToString()));
                foreach (TourType child in region.Children)
                {
                    ddlTourTypes.Items.Add(new ListItem(" |-- " + child.Name, child.Id.ToString()));
                }
            }
        }

        private void BindTourRegion()
        {
            ddlTourRegion.Items.Clear();
            ddlTourRegion.Items.Add(" -- All --");
            IList roots = Module.TourRegionGetAllRoot();
            foreach (TourRegion region in roots)
            {
                ddlTourRegion.Items.Add(new ListItem(region.Name, region.Id.ToString()));
                foreach (TourRegion child in region.Children)
                {
                    ddlTourRegion.Items.Add(new ListItem(" |-- " + child.Name, child.Id.ToString()));
                }
            }
        }

        #endregion

        #region -- Control Event --

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected void pagerTours_CacheEmpty(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected void pagerTours_PageChanged(object sender, PageChangedEventArgs e)
        {
            rptTours.DataSource = GetDataSource();
            rptTours.DataBind();
        }

        #region -Repeater-

        protected void rptTours_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                RepeaterOrder.FILE_NAME = "TourList.aspx";
                RepeaterOrder.SetOrderLink(e, "Name", Request.QueryString);
                RepeaterOrder.SetOrderLink(e, "TourType", Request.QueryString);
                RepeaterOrder.SetOrderLink(e, "NumberOfDay", Request.QueryString);
                RepeaterOrder.SetOrderLink(e, "LengthTrip", Request.QueryString);
            }

            if (e.Item.DataItem is Tour)
            {
                Tour tour = (Tour) e.Item.DataItem;

                #region Name

                HyperLink hyperLinkView = e.Item.FindControl("hyperLinkView") as HyperLink;
                if (hyperLinkView != null)
                {
                    hyperLinkView.Text = tour.Name;
                    if (tour.Provider != null)
                    {
                        hyperLinkView.NavigateUrl = string.Format("TourPackagePriceConfig.aspx?NodeId={0}&SectionId={1}&TourId={2}",
                                                                  Node.Id, Section.Id, tour.Id);
                    }
                    else
                    {
                        hyperLinkView.NavigateUrl = string.Format("TourConfig.aspx?NodeId={0}&SectionId={1}&TourId={2}",
                                                                  Node.Id, Section.Id, tour.Id);
                    }
                }

                #endregion

                #region Start

                Label labelCityStart = e.Item.FindControl("labelCityStart") as Label;
                if (labelCityStart != null)
                {
                    if (tour.CityStart != null)
                    {
                        labelCityStart.Text = tour.CityStart.Name;
                    }
                    else
                    {
                        if (tour.StartFrom != null)
                        {
                            labelCityStart.Text = tour.StartFrom.Name;
                        }
                    }
                }

                #endregion

                #region End

                Label labelCityEnd = e.Item.FindControl("labelCityEnd") as Label;
                if (labelCityEnd != null)
                {
                    if (tour.CityEnd != null)
                    {
                        labelCityEnd.Text = tour.CityEnd.Name;
                    }
                    else
                    {
                        if (tour.EndIn != null)
                        {
                            labelCityEnd.Text = tour.EndIn.Name;
                        }
                    }
                }

                Label labelTourType = e.Item.FindControl("labelTourType") as Label;
                if (labelTourType != null)
                {
                    if (tour.TourType != null)
                    {
                        labelTourType.Text = tour.TourType.Name;
                    }
                }

                #endregion

                #region Days

                Label labelNumberOfDays = e.Item.FindControl("labelNumberOfDays") as Label;
                if (labelNumberOfDays != null)
                {
                    if (tour.IsHalf)
                    {
                        labelNumberOfDays.Text = "½ day";
                    }
                    else
                    {
                        if (tour.NumberOfDay > 1)
                        {
                            labelNumberOfDays.Text = tour.NumberOfDay+ " days";
                        }
                        else
                        {
                            labelNumberOfDays.Text = "1 day";
                        }
                    }
                }

                #endregion                

                #region HyperLink Edit

                HyperLink hplEdit = e.Item.FindControl("hplEdit") as HyperLink;
                Image imageEdit = e.Item.FindControl("imageEdit") as Image;
                if (hplEdit != null && imageEdit != null)
                {
                    if (UserIdentity.CanModify(Section))
                    {
                        hplEdit.NavigateUrl = string.Format("TourEdit.aspx?NodeId={0}&SectionId={1}&TourId={2}", Node.Id,
                                                            Section.Id, tour.Id);
                    }
                    else
                    {
                        hplEdit.Visible = false;
                        imageEdit.Visible = false;
                    }
                }

                #endregion

                #region Button Delete

                ImageButton btnDelete = e.Item.FindControl("btnDelete") as ImageButton;
                if (btnDelete != null)
                {
                    if (!UserIdentity.CanDelete(Section))
                    {
                        btnDelete.Visible = false;
                    }
                }

                #endregion
            }
        }

        protected void rptTours_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            Tour tour = Module.TourGetById(Convert.ToInt32(e.CommandArgument));
            switch (e.CommandName.ToLower())
            {
                case "delete":
                    if (!UserIdentity.CanDelete(Section))
                    {
                        ShowError(Resources.textAccessDenied);
                        return;
                    }
                    tour.ModifiedBy = ((User) Page.User.Identity).Id;
                    Module.Delete(tour);
                    rptTours.DataSource = GetDataSource();
                    rptTours.DataBind();
                    break;
                case "featureup":
                    tour.Featured += 1;
                    Module.Update(tour);
                    rptTours.DataSource = GetDataSource();
                    rptTours.DataBind();
                    break;
                case "featuredown":
                    tour.Featured -= 1;
                    Module.Update(tour);
                    rptTours.DataSource = GetDataSource();
                    rptTours.DataBind();
                    break;
            }
        }

        #endregion

        #endregion
    }
}