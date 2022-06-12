using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.ServerControls.FileUpload;
using CMS.Web.Util;
using log4net;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.UI;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class ProductTripDetail : SailsAgencyAdminBase
    {
        #region -- Private Member --

        private readonly ILog _logger = LogManager.GetLogger(typeof(ProductTripDetail));
        private SailsTrip _trip;

        private int TripId
        {
            get
            {
                int id;
                if (Request.QueryString["TripId"] != null && Int32.TryParse(Request.QueryString["TripId"], out id))
                {
                    return id;
                }
                return -1;
            }
        }

        #endregion

        #region -- Page Event --

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Title = "Trip";

                #region -- Ajax Image --
                #endregion

                if (!IsPostBack)
                {
                    LoadInfo();
                    rptCostTypes.DataSource = Module.CostTypeGetByTrips();
                    rptCostTypes.DataBind();
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error when Page_Load in SailsTripEdit", ex);
                ShowError(ex.Message);
            }
        }

        #endregion

        #region -- Private Method --

        private void LoadInfo()
        {
            if (TripId > 0)
            {
                _trip = Module.TripGetById(TripId);
                hidTripName.Value = _trip.Name;
                textBoxName.Text = _trip.Name;
                textBoxNumberOfDay.Text = _trip.NumberOfDay.ToString();
                litItinerary.Text = _trip.Itinerary;
                litDescription.Text = _trip.Description;
                litExclusions.Text = _trip.Exclusions;
                litWhatToTake.Text = _trip.WhatToTake;
                litInclusions.Text = _trip.Inclusions;
                imgTripImage.ImageUrl = UrlHelper.GetSiteUrl() + _trip.Image;
                txtTripCode.Text = _trip.TripCode;
                txtNumberCustomerMin.Text = _trip.NumberCustomerMin.ToString();
                txtTimeCreateBookingMin.Text = _trip.TimeCreateBookingMin.ToString();
                litNumberOfOptions.Text = _trip.NumberOfOptions.ToString();
                if (_trip.Organization != null)
                {
                    litOrganizations.Text = _trip.Organization.Name;
                }
            }
        }

        #endregion

        #region -- Control Event --
        protected void buttonCancel_Clicl(object sender, EventArgs e)
        {
            PageRedirect(string.Format("ProductTrip.aspx?NodeId={0}&SectionId={1}", Node.Id, Section.Id));
        }

        protected void rptCostTypes_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is CostType)
            {
                var type = (CostType)e.Item.DataItem;

                var labelCostType = e.Item.FindControl("labelCostType") as Label;
                if (labelCostType != null)
                {
                    labelCostType.Text = type.Name;
                    if (_trip != null && _trip.CostTypes.Contains(type))
                    {
                        labelCostType.Visible = true;
                    }
                    else
                    {
                        labelCostType.Visible = false;
                    }
                }
            }
        }
        #endregion
    }
}