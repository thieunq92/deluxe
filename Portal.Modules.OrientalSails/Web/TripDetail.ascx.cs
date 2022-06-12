using System;
using System.Collections;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.Core.Domain;
using CMS.Web.UI;
using CMS.Web.Util;
using log4net;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.Resources;
using Portal.Modules.OrientalSails.Web.Util;

namespace Portal.Modules.OrientalSails.Web
{
    public partial class TripDetail : BaseModuleControl
    {
        #region -- Private Member --

        private readonly ILog _logger = LogManager.GetLogger(typeof (TripDetail));
        private RoomClass _currentClass;
        private SailsPriceTable _table;
        private IList _policies;

        private IList CurrentPolicies
        {
            get
            {
                if (_policies==null)
                {
                    if (PageEngine.CuyahogaUser!=null)
                    {
                        _policies = Module.AgencyPolicyGetByRole(PageEngine.CuyahogaUser.Roles[0] as Role);
                    }
                    else
                    {
                        _policies = Module.AgencyPolicyGetByRole(Module.RoleGetById(4));
                    }
                }
                return _policies;
            }
        }

        private SailsTrip _trip;

        private new SailsModule Module
        {
            get { return base.Module as SailsModule; }
        }

        #endregion

        #region -- Page Event --

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    LocalizeControls();

                    _trip = Module.TripGetById(Module.TripId);
                    image_Trip.ImageUrl = _trip.Image;
                    label_Name.Text = _trip.Name;
                    label_NumberOfDays.Text = string.Format("{0}: {1}", Strings.labelNumberOfDays, _trip.NumberOfDay);

                    Description.InnerHtml = _trip.Description;
                    Activities.InnerHtml = _trip.Itinerary;
                    Exclusions.InnerHtml = _trip.Exclusions;
                    Inclusions.InnerHtml = _trip.Inclusions;
                    WhatToTake.InnerHtml = _trip.WhatToTake;
                    IList options = new ArrayList();
                    switch (_trip.NumberOfOptions)
                    {
                        case 2:
                            ddlOption.Items.Add(new ListItem(Enum.GetName(typeof (TripOption), TripOption.Option1), "1"));
                            ddlOption.Items.Add(new ListItem(Enum.GetName(typeof (TripOption), TripOption.Option2), "2"));
                            options.Add("Option 1");
                            options.Add("Option 2");
                            break;
                        case 3:
                            ddlOption.Items.Add(new ListItem(Enum.GetName(typeof (TripOption), TripOption.Option1), "1"));
                            ddlOption.Items.Add(new ListItem(Enum.GetName(typeof (TripOption), TripOption.Option2), "2"));
                            ddlOption.Items.Add(new ListItem(Enum.GetName(typeof (TripOption), TripOption.Option3), "3"));
                            options.Add("Option 1");
                            options.Add("Option 2");
                            options.Add("Option 3");
                            break;
                        default:
                            options.Add("");
                            ddlOption.Enabled = false;
                            ddlOption.Visible = false;
                            break;
                    }
                    ddlOption.DataBind();

                    rptTripOption.DataSource = options;
                    rptTripOption.DataBind();                    
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error when Page_Load in TripDetail control", ex);
                throw;
            }
        }

        #endregion

        #region -- Control Event --

        protected void imageButtonBook_Click(object sender, ImageClickEventArgs e)
        {
            if (PageEngine.IsValid)
            {
                DateTime startDate;
                try
                {
                    startDate = DateTime.ParseExact(textBoxStartDate.Text, "dd/MM/yyyy",
                                                    CultureInfo.InvariantCulture);
                }
                catch
                {
                    startDate = DateTime.Now.AddDays(1);
                }
                if (startDate < DateTime.Today.AddDays(1))
                {
                    labelError.Text = "You can not book in the past!";
                    return;
                }

                _trip = Module.TripGetById(Module.TripId);
                Session.Add("StartDate", textBoxStartDate.Text);
                Session.Add("TripId", _trip.Id);
                if (_trip.NumberOfOptions > 1)
                {
                    Session.Add("TripOption",ddlOption.SelectedValue);
                    PageEngine.PageRedirect(string.Format("{0}/{1}/{2}/{3}/{4}{5}",
                                                          UrlHelper.GetUrlFromSection(Module.Section),
                                                          SailsModule.ACTION_ORDER_PARAM, _trip.Id,
                                                          ddlOption.SelectedValue,
                                                          _trip.Name, UrlHelper.EXTENSION));
                }
                else
                {
                    Session.Add("TripOption", Enum.GetName(typeof(TripOption),TripOption.Option1));
                    PageEngine.PageRedirect(string.Format("{0}/{1}/{2}/{3}/{4}{5}",
                                                          UrlHelper.GetUrlFromSection(Module.Section),
                                                          SailsModule.ACTION_ORDER_PARAM, _trip.Id, 0,
                                                          _trip.Name,
                                                          UrlHelper.EXTENSION));
                }
            }
        }

        #endregion

        protected void rptRoomClasses_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is RoomClass)
            {
                _currentClass = (RoomClass) e.Item.DataItem;
                Repeater rptRoomTypes = (Repeater) e.Item.FindControl("rptRoomTypes");
                rptRoomTypes.DataSource = Module.RoomTypexGetAll();
                rptRoomTypes.DataBind();
            }
        }

        protected void rptRoomTypes_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is RoomTypex)
            {
                RoomTypex type = (RoomTypex) e.Item.DataItem;
                Literal litRoomName = (Literal) e.Item.FindControl("litRoomName");
                litRoomName.Text = _currentClass.Name + " " + type.Name;
                //TODO: CRUISE INFO
                SailsPriceConfig config = Module.SailsPriceConfigGet(_currentClass, type, _trip, null, TripOption.Option1,
                                                                     DateTime.Today, BookingType.Double, null);
                Literal litPrice = (Literal) e.Item.FindControl("litPrice");
                //litPrice.Text = Module.ApplyPriceFor(config.NetPrice,CurrentPolicies).ToString("$#,0");
            }
        }

        protected void rptTripOption_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rptRoomClasses = (Repeater) e.Item.FindControl("rptRoomClasses");
            rptRoomClasses.DataSource = Module.RoomClassGetAll();
            rptRoomClasses.DataBind();

            Literal litOption = (Literal) e.Item.FindControl("litOption");
            if (!string.IsNullOrEmpty(e.Item.DataItem.ToString()))
            {
                litOption.Text = e.Item.DataItem.ToString();
            }
        }
    }
}