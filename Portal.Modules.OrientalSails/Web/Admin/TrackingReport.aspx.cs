using System;
using System.Collections;
using System.Globalization;
using System.Web.UI.WebControls;
using GemBox.Spreadsheet;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.ReportEngine;
using Portal.Modules.OrientalSails.Web.UI;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class TrackingReport : SailsAdminBase
    {
        private DateTime? _date;
        private double _total;

        protected DateTime Date
        {
            get
            {
                if (!_date.HasValue)
                {
                    _date = DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                return _date.Value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = Resources.textTrackingReport;
            if (!IsPostBack)
            {
                if (Request.QueryString["date"]!=null)
                {
                    txtDate.Text =
                        DateTime.FromOADate(Convert.ToDouble(Request.QueryString["date"])).ToString("dd/MM/yyyy");
                }
                else
                {
                    txtDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                }
                GetDataSource();
            }
        }

        protected void btnDisplay_Click(object sender, EventArgs e)
        {
            DateTime date = DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            PageRedirect(string.Format("TrackingReport.aspx?NodeId={0}&SectionId={1}&date={2}", Node.Id, Section.Id, date.ToOADate()));
        }

        protected void GetDataSource()
        {
            if (Request.QueryString["bookingid"] != null)
            {
                Booking booking = Module.BookingGetById(Convert.ToInt32(Request.QueryString["bookingid"]));
                rptBookings.DataSource = Module.TrackingGetByBooking(booking);
            }
            else
            {
                rptBookings.DataSource = Module.TrackingGetByDateRange(Date, Date.AddDays(1));
            }
            rptBookings.DataBind();
        }

        protected void rptBookings_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is BookingTrack)
            {
                BookingTrack track = (BookingTrack) e.Item.DataItem;
                HyperLink hplCode = e.Item.FindControl("hplCode") as HyperLink;
                if (hplCode!=null)
                {
                    if (UseCustomBookingId)
                    {
                        if (track.Booking.CustomBookingId > 0)
                        {
                            hplCode.Text = string.Format(BookingFormat, track.Booking.CustomBookingId);
                        }
                        else
                        {
                            hplCode.Text = string.Format(BookingFormat, track.Booking.Id);
                        }
                    }
                    else
                    {
                        hplCode.Text = string.Format(BookingFormat, track.Booking.Id);
                    }
                    hplCode.NavigateUrl = string.Format("BookingView.aspx?NodeId={0}&SectionId={1}&BookingId={2}",
                                                        Node.Id, Section.Id, track.Booking.Id);
                }

                Literal litTACode = e.Item.FindControl("litTACode") as Literal;
                if (litTACode!=null)
                {
                    litTACode.Text = track.Booking.AgencyCode;
                }

                Literal litTime = e.Item.FindControl("litTime") as Literal;
                if (litTime!=null)
                {
                    litTime.Text = track.ModifiedDate.ToString("dd/MM/yyyy HH:mm");
                }

                Literal litUser = e.Item.FindControl("litUser") as Literal;
                if (litUser!=null)
                {
                    if (track.User!=null)
                    {
                        litUser.Text = track.User.UserName;
                    }
                }

                Literal litContent = e.Item.FindControl("litContent") as Literal;
                Literal litTotal = e.Item.FindControl("litTotal") as Literal;
                if (litContent != null && litTotal!=null)
                {
                    string content = string.Empty;
                    double total = 0;
                    foreach (BookingChanged change in track.Changes)
                    {
                        switch (change.Action)
                        {
                            case BookingAction.ChangeTotal:                                
                                total += Convert.ToDouble(change.Parameter);
                                break;
                            case BookingAction.Approved:
                                total += Convert.ToDouble(change.Parameter);
                                break;
                            case BookingAction.Cancelled:
                                total += -Convert.ToDouble(change.Parameter);                                
                                break;
                            case BookingAction.Transfer:
                                total += Convert.ToDouble(change.Parameter);
                                break;
                            case BookingAction.Untransfer:
                                total += Convert.ToDouble(change.Parameter);
                                break;
                            case BookingAction.ChangeTransfer:
                                total += Convert.ToDouble(change.Parameter);
                                break;
                        }
                        if (string.IsNullOrEmpty(content))
                        {
                            content += Module.GetChangeContent(change);
                        }
                        else
                        {
                            content +="<br/>" +Module.GetChangeContent(change);
                        }                        
                    }
                    litContent.Text = content;
                    litTotal.Text = total.ToString();
                    _total += total;
                }
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                Literal litTotal = e.Item.FindControl("litTotal") as Literal;
                if (litTotal!=null)
                {
                    litTotal.Text = _total.ToString();
                }
            }
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            IList data;
            if (Request.QueryString["bookingid"] != null)
            {
                Booking booking = Module.BookingGetById(Convert.ToInt32(Request.QueryString["bookingid"]));
                data = Module.TrackingGetByBooking(booking);
            }
            else
            {
                data = Module.TrackingGetByDateRange(Date, Date.AddDays(1));
            }
            BookingChanges.Emotion(data, this, Response, Server.MapPath("/Modules/Sails/Admin/ExportTemplates/BK_changes.xls"));
        }
    }
}
