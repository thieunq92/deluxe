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
    public partial class ProviderList : TourAdminBasePage
    {
        #region -- Private Member --

        private readonly ILog logger = LogManager.GetLogger(typeof (ProviderList));

        #endregion

        #region -- Page Event --

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Title = Resources.titleProviderList;

                if (!UserIdentity.HasPermission(AccessLevel.Administrator) && !UserIdentity.HasFullPermission(_module.Section))
                {
                    ShowError(Resources.textAccessDenied);
                    panelContent.Visible = false;
                    return;
                }

                #region -- Page Size --

                int pageSize;
                if (Request.QueryString["pageSize"] != null &&
                    Int32.TryParse(Request.QueryString["pageSize"], out pageSize))
                {
                    pagerProvider.PageSize = pageSize;
                    if (!IsPostBack)
                    {
                        ddlPageSize.SelectedValue = pageSize.ToString();
                    }
                }
                else
                {
                    pagerProvider.PageSize = 20;
                }

                #endregion

                if (!IsPostBack)
                {
                    BindCountry();
                    LoadInfo();
                    GetDataSource();
                    rptProvider.DataBind();
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error when Page_Load in ProviderList", ex);
                throw;
            }
        }

        #endregion

        #region -- Private Method --

        /// <summary>
        /// Load info (nếu có) của Provider
        /// </summary>
        private void LoadInfo()
        {
            if (
                !(string.IsNullOrEmpty(Request.QueryString["Name"]) &&
                  string.IsNullOrEmpty(Request.QueryString["Location"])))
            {
                panelResult.Visible = true;
            }
            if (Request.QueryString["Name"] != null)
            {
                textBoxName.Text = Request.QueryString["Name"];
            }

            #region -Xử lý Location-

            if (Request.QueryString["Location"] != null && Convert.ToInt32(Request.QueryString["Location"]) > 0)
            {
                Location temp = Module.LocationGetById(Convert.ToInt32(Request.QueryString["Location"]));
                if (temp != null)
                {
                    Location tmpCountry, tmpRegion, tmpCity;
                    switch (temp.Level)
                    {
                            //Nếu Level là 3 thì Location chính là Country
                        case 3:
                            BindCountry();
                            ddlCountry.SelectedValue = temp.Id.ToString();
                            BindRegion(temp.Id);
                            break;
                            //Nếu Level là 4 thì Location chính là Region
                        case 4:
                            tmpCountry = temp.Parent;
                            BindCountry();
                            ddlCountry.SelectedValue = tmpCountry.Id.ToString();
                            BindRegion(tmpCountry.Id);
                            ddlRegion.SelectedValue = temp.Id.ToString();
                            BindCity(temp.Id);
                            break;
                            //Nếu Level là 5 thì Location chính là City
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
                            //Nếu Level là 6 thì Location chính là District
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
                            //Hiện tại Không có trường hợp Location >6 vì khi tạo TransportRoute người dùng chỉ được chọn Location Cấp thấp nhất là 6
                        default:
                            break;
                    }
                }
            }

            #endregion
        }

        /// <summary>
        /// Get DataSource Cho repeater
        /// </summary>
        protected virtual void GetDataSource()
        {
            IList dataSource = Module.ProviderSearchFromQueryString(Request.QueryString);
            labelResults.Text = string.Format(Resources.stringFormatResult, dataSource.Count);
            rptProvider.DataSource = dataSource;
        }

        /// <summary>
        /// Tạo query string
        /// </summary>
        /// <returns></returns>
        public virtual string BuildQueryString()
        {
            string query = string.Empty;
            if (!string.IsNullOrEmpty(textBoxName.Text))
            {
                query += string.Format("&Name={0}", textBoxName.Text);
            }

            int LocationID;

            if (ddlLocation.Enabled && ddlLocation.SelectedIndex > 0)
            {
                LocationID = (Module.LocationGetById(Convert.ToInt32(ddlLocation.SelectedValue))).Id;
            }
            else if (ddlCity.Enabled && ddlCity.SelectedIndex > 0)
            {
                LocationID = (Module.LocationGetById(Convert.ToInt32(ddlCity.SelectedValue))).Id;
            }
            else if (ddlRegion.Enabled && ddlRegion.SelectedIndex > 0)
            {
                LocationID = (Module.LocationGetById(Convert.ToInt32(ddlRegion.SelectedValue))).Id;
            }
            else if (ddlCountry.Enabled && ddlCountry.SelectedIndex > 0)
            {
                LocationID = (Module.LocationGetById(Convert.ToInt32(ddlCountry.SelectedValue))).Id;
            }
            else
            {
                LocationID = -1;
            }

            if (LocationID > 0)
            {
                query += string.Format("&Location={0}", LocationID);
            }

            return query;
        }

        #region - Drop Down List -
        //Các hàm xử lý cho dropdownlist Location

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

        #region -- Control Event --

        #region - Pager -

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Request.RawUrl.ToLower().Contains("pagesize="))
            {
                PageRedirect(Request.RawUrl.Replace("PageSize=" + Request.QueryString["PageSize"],
                                                         "PageSize=" + ddlPageSize.SelectedValue));
            }
            else
            {
                PageRedirect(Request.RawUrl + "&PageSize=" + ddlPageSize.SelectedValue);
            }
        }

        protected void pagerProvider_CacheEmpty(object sender, EventArgs e)
        {
            GetDataSource();
        }

        protected void pagerProvider_PageChanged(object sender, PageChangedEventArgs e)
        {
            GetDataSource();
            rptProvider.DataBind();
        }

        #endregion

        #region - Repeater -

        protected virtual void rptProvider_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is Provider)
            {
                Provider item = e.Item.DataItem as Provider;
                if (item != null)
                {
                    #region - HyperLink Name -

                    using (HyperLink hyperLinkName = e.Item.FindControl("hyperLinkName") as HyperLink)
                    {
                        if (hyperLinkName != null)
                        {
                            hyperLinkName.Text = item.Name;
                            hyperLinkName.NavigateUrl = "#";
                        }
                    }

                    #endregion

                    #region - Label Address -

                    using (Label labelProviderAddress = e.Item.FindControl("labelProviderAddress") as Label)
                    {
                        if (labelProviderAddress != null)
                        {
                            labelProviderAddress.Text = item.Address;
                        }
                    }

                    #endregion

                    #region - Location -

                    using (Label labelProviderLocation = e.Item.FindControl("labelProviderLocation") as Label)
                    {
                        if (labelProviderLocation != null)
                        {
                            string strLocation = "";
                            strLocation += item.Location.Name;
                            Location _location = item.Location;
                            while (true)
                            {
                                if (_location.Parent != null)
                                {
                                    _location = _location.Parent;
                                    strLocation += ", " + _location.Name;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            labelProviderLocation.Text = strLocation;
                        }
                    }

                    #endregion

                    #region - Featured -

                    using (CheckBox checkBoxFeatured = e.Item.FindControl("checkBoxFeatured") as CheckBox)
                    {
                        Label labelProviderFeatured = e.Item.FindControl("labelProviderFeatured") as Label;
                        if (labelProviderFeatured != null && checkBoxFeatured != null)
                        {
                            checkBoxFeatured.Checked = item.Featured;
                            checkBoxFeatured.Enabled = false;
                            labelProviderFeatured.Text = string.Format(Resources.labelProviderFeatured, item.IsFeatured);
                        }
                    }

                    #endregion

                    #region - HyperLink Edit -

                    using (HyperLink hyperLinkEdit = e.Item.FindControl("hyperLinkEdit") as HyperLink)
                    {
                        if (hyperLinkEdit != null)
                        {
                            hyperLinkEdit.NavigateUrl =
                                string.Format("/Modules/TourManagement/Admin/ProviderEdit.aspx?NodeId={0}&SectionId={1}&ProviderID={2}", Node.Id,
                                              Section.Id, item.Id);
                        }
                    }

                    #endregion
                }
            }
            if (e.Item.ItemType == ListItemType.Header)
            {
                RepeaterOrder.FILE_NAME = "ProviderList.aspx";

                RepeaterOrder.SetOrderLink(e, Provider.NAME, Request.QueryString);
                RepeaterOrder.SetOrderLink(e, Provider.ADDRESS, Request.QueryString);
                RepeaterOrder.SetOrderLink(e, Provider.LOCATION, Request.QueryString);
                RepeaterOrder.SetOrderLink(e, Provider.ISFEATURED, Request.QueryString);
            }
        }

        protected void rptProvider_OnItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                Provider item;
                switch (e.CommandName)
                {
                    case "Delete":
                        item = Module.ProviderGetByID(Convert.ToInt32(e.CommandArgument));
                        Module.Delete(item);
                        GetDataSource();
                        rptProvider.DataBind();
                        break;
                    case "FeatureUp":
                        item = Module.ProviderGetByID(Convert.ToInt32(e.CommandArgument));
                        item.IsFeatured++;
                        Module.Update(item);
                        GetDataSource();
                        rptProvider.DataBind();
                        break;
                    case "FeatureDown":
                        item = Module.ProviderGetByID(Convert.ToInt32(e.CommandArgument));
                        item.IsFeatured--;
                        Module.Update(item);
                        GetDataSource();
                        rptProvider.DataBind();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error when rptProvider_OnItemCommand in ProviderList", ex);
                throw;
            }
        }

        #endregion


        /// <summary>
        /// Click vào search tạo query string để tìm kiếm provider
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void butonSearch_OnClick(object sender, EventArgs e)
        {
            try
            {
                PageRedirect(string.Format("ProviderList.aspx?NodeId={0}&SectionId={1}{2}", Node.Id, Section.Id,
                                                BuildQueryString()));
            }
            catch (Exception ex)
            {
                logger.Error("Error when butonSearch_OnClick in ProviderList", ex);
                throw;
            }
        }

        #region - Drop Down List Location -

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
                logger.Error("Error when ddlCountry_SelectedIndexChanged in ProviderList", ex);
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
                logger.Error("Error when ddlRegion_SelectedIndexChanged in ProviderList", ex);
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
                logger.Error("Error when ddlCity_SelectedIndexChanged in ProviderList", ex);
                throw;
            }
        }

        #endregion

        #endregion
    }
}