using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.Web.Util;
using log4net;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.ReportEngine;
using Portal.Modules.OrientalSails.Web.Controls;
using Portal.Modules.OrientalSails.Web.UI;
using Portal.Modules.OrientalSails.Web.Util;
using CMS.Core.Domain;
using Portal.Modules.OrientalSails.Repository;
using Portal.Modules.OrientalSails.BusinessLogic;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class BookingHistories : Page
    {
        private BookingHistoriesBLL bookingHistoriesBLL;
        private BookingHistory _prev;
        public SailsModule Module
        {
            get { return SailsModule.GetInstance(); }
        }
        public BookingHistoriesBLL BookingHistoriesBLL
        {
            get
            {
                if (bookingHistoriesBLL == null)
                    bookingHistoriesBLL = new BookingHistoriesBLL();
                return bookingHistoriesBLL;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            var booking = Module.BookingGetById(Convert.ToInt32(Request.QueryString["bookingid"]));
            var histories = BookingHistoriesBLL.BookingHistoryGetAllByBooking(booking);
            _prev = null;
            rptAgencies.DataSource = histories;
            rptAgencies.DataBind();
            _prev = null;
            rptDates.DataSource = histories;
            rptDates.DataBind();
            _prev = null;
            rptStatus.DataSource = histories;
            rptStatus.DataBind();
            _prev = null;
            rptTrips.DataSource = histories;
            rptTrips.DataBind();
            _prev = null;
            rptSpecialRequest.DataSource = histories;
            rptSpecialRequest.DataBind();
            _prev = null;
            rptNumberOfPax.DataSource = histories;
            rptNumberOfPax.DataBind();
            _prev = null;
            rptCustomerInfo.DataSource = histories;
            rptCustomerInfo.DataBind();
        }
        protected void Page_Unload(object sender, EventArgs e)
        {
            if (bookingHistoriesBLL != null)
            {
                bookingHistoriesBLL.Dispose();
                bookingHistoriesBLL = null;
            }
        }
        protected void rptDates_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is BookingHistory)
            {
                var history = (BookingHistory)e.Item.DataItem;

                if (_prev != null)
                {
                    if (_prev.StartDate == history.StartDate)
                    {
                        e.Item.Visible = false;
                        return;
                    }
                }

                try
                {
                    ValueBinder.BindLiteral(e.Item, "litTime", history.Date.ToString("dd-MMM-yyyy HH:mm"));
                    ValueBinder.BindLiteral(e.Item, "litUser", history.User.FullName);
                    ValueBinder.BindLiteral(e.Item, "litTo", history.StartDate.ToString("dd/MM/yyyy"));
                }
                catch (Exception) { }

                if (_prev != null)
                {
                    try
                    {
                        ValueBinder.BindLiteral(e.Item, "litFrom", _prev.StartDate.ToString("dd/MM/yyyy"));
                    }
                    catch (Exception) { }
                }
                _prev = history;
            }
        }

        protected void rptStatus_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is BookingHistory)
            {
                var history = (BookingHistory)e.Item.DataItem;

                if (_prev != null)
                {
                    if (_prev.Status == history.Status)
                    {
                        e.Item.Visible = false;
                        return;
                    }
                }
                try
                {
                    ValueBinder.BindLiteral(e.Item, "litTime", history.Date.ToString("dd-MMM-yyyy HH:mm"));
                    ValueBinder.BindLiteral(e.Item, "litUser", history.User.FullName);
                    ValueBinder.BindLiteral(e.Item, "litTo", history.Status.ToString());
                }
                catch (Exception) { }

                if (_prev != null)
                {
                    try
                    {
                        ValueBinder.BindLiteral(e.Item, "litFrom", _prev.Status.ToString());
                    }
                    catch (Exception) { }
                }
                _prev = history;
            }
        }

        protected void rptTrips_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is BookingHistory)
            {
                var history = (BookingHistory)e.Item.DataItem;

                if (_prev != null)
                {
                    if (_prev.Trip.Id == history.Trip.Id)
                    {
                        e.Item.Visible = false;
                        return;
                    }
                }

                try
                {
                    ValueBinder.BindLiteral(e.Item, "litTime", history.Date.ToString("dd-MMM-yyyy HH:mm"));
                    ValueBinder.BindLiteral(e.Item, "litUser", history.User.FullName);
                    ValueBinder.BindLiteral(e.Item, "litTo", history.Trip.Name);
                }
                catch (Exception) { }

                if (_prev != null)
                {
                    try
                    {
                        ValueBinder.BindLiteral(e.Item, "litFrom", _prev.Trip.Name);
                    }
                    catch (Exception) { }
                }
                _prev = history;
            }
        }

        protected void rptAgencies_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is BookingHistory)
            {
                var history = (BookingHistory)e.Item.DataItem;

                if (_prev != null)
                {
                    try
                    {
                        if (_prev.Agency.Id == history.Agency.Id)
                        {
                            e.Item.Visible = false;
                            return;
                        }
                    }
                    catch (Exception) { }
                }

                try
                {
                    ValueBinder.BindLiteral(e.Item, "litTime", history.Date.ToString("dd-MMM-yyyy HH:mm"));
                    ValueBinder.BindLiteral(e.Item, "litUser", history.User.FullName);
                    ValueBinder.BindLiteral(e.Item, "litTo", history.Agency.Name);
                }
                catch (Exception) { }

                if (_prev != null)
                {
                    try
                    {
                        ValueBinder.BindLiteral(e.Item, "litFrom", _prev.Agency.Name);
                    }
                    catch (Exception) { }
                }
                _prev = history;
            }
        }

        protected void rptTotals_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is BookingHistory)
            {
                var history = (BookingHistory)e.Item.DataItem;

                if (_prev != null)
                {
                    if (_prev.Total == history.Total && _prev.TotalCurrency == history.TotalCurrency)
                    {
                        e.Item.Visible = false;
                        return;
                    }
                }

                try
                {
                    ValueBinder.BindLiteral(e.Item, "litTime", history.Date.ToString("dd-MMM-yyyy HH:mm"));
                    ValueBinder.BindLiteral(e.Item, "litUser", history.User.FullName);
                }
                catch (Exception) { }

                ValueBinder.BindLiteral(e.Item, "litTo", history.Total + history.TotalCurrency);

                if (_prev != null)
                {
                    ValueBinder.BindLiteral(e.Item, "litFrom", _prev.Total + _prev.TotalCurrency);
                }
                _prev = history;
            }
        }

        protected void rptSpecialRequest_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is BookingHistory)
            {
                var history = (BookingHistory)e.Item.DataItem;

                if (_prev != null)
                {
                    if (_prev.SpecialRequest == history.SpecialRequest)
                    {
                        e.Item.Visible = false;
                        return;
                    }
                }

                try
                {
                    ValueBinder.BindLiteral(e.Item, "litTime", history.Date.ToString("dd-MMM-yyyy HH:mm"));
                    ValueBinder.BindLiteral(e.Item, "litUser", history.User.FullName);
                }
                catch (Exception) { }
                ValueBinder.BindLiteral(e.Item, "litTo", history.SpecialRequest);

                if (_prev != null)
                {
                    ValueBinder.BindLiteral(e.Item, "litFrom", _prev.SpecialRequest);
                }
                _prev = history;
            }
        }

        protected void rptNumberOfPax_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is BookingHistory)
            {
                var history = (BookingHistory)e.Item.DataItem;

                if (_prev != null)
                {
                    if (_prev.NumberOfPax == history.NumberOfPax)
                    {
                        e.Item.Visible = false;
                        return;
                    }
                }

                try
                {
                    ValueBinder.BindLiteral(e.Item, "litTime", history.Date.ToString("dd-MMM-yyyy HH:mm"));
                    ValueBinder.BindLiteral(e.Item, "litUser", history.User.FullName);
                    ValueBinder.BindLiteral(e.Item, "litTo", history.NumberOfPax);
                }
                catch (Exception) { }

                if (_prev != null)
                {
                    try
                    {
                        ValueBinder.BindLiteral(e.Item, "litFrom", _prev.NumberOfPax);
                    }
                    catch (Exception) { }
                }
                _prev = history;
            }
        }

        protected void rptCustomerInfo_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is BookingHistory)
            {
                var history = (BookingHistory)e.Item.DataItem;

                if (_prev != null)
                {
                    if (_prev.CustomerInfo == history.CustomerInfo)
                    {
                        e.Item.Visible = false;
                        return;
                    }
                }

                try
                {
                    ValueBinder.BindLiteral(e.Item, "litTime", history.Date.ToString("dd-MMM-yyyy HH:mm"));
                    ValueBinder.BindLiteral(e.Item, "litUser", history.User.FullName);
                    ValueBinder.BindLiteral(e.Item, "litTo", history.CustomerInfo);
                }
                catch (Exception) { }

                if (_prev != null)
                {
                    try
                    {
                        ValueBinder.BindLiteral(e.Item, "litFrom", _prev.CustomerInfo);
                    }
                    catch (Exception) { }
                }
                _prev = history;
            }
        }
    }
}