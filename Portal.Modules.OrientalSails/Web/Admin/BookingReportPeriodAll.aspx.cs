using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using NHibernate.Criterion;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.Util;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class BookingReportPeriodAll : BookingReportPeriod
    {
        #region -- FIELDS --
        private IList _cruises;

        protected IList AllCruises
        {
            get
            {
                if (_cruises==null)
                {
                    _cruises = Module.TripGetAll(true, UserIdentity);
                }
                return _cruises;
            }
        }

        private RoomUtil _util;

        private SailsTrip _activeCruise;
        protected SailsTrip ActiveCruise
        {
            get { return _activeCruise; }
        }
        #endregion

        #region -- PAGE EVENTS --
        protected override void Page_Load(object sender, EventArgs e)
        {
            //if (!UserIdentity.HasPermission(AccessLevel.Administrator))
            //{
            //    ShowError("You don't have permission to view this info, please go away");
            //    return;
            //}
            if (AllCruises.Count == 1 && Request.QueryString["cruiseid"]==null)
            {
                SailsTrip cruise = (SailsTrip)AllCruises[0];
                PageRedirect(string.Format("BookingReportPeriod.aspx?NodeId={0}&SectionId={1}&tripid={2}", Node.Id, Section.Id, cruise.Id));
            }
            _util = new RoomUtil(Module);
            base.Page_Load(sender, e);
        }
        #endregion

        protected override void rptBookingList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                Repeater rptTripRow = e.Item.FindControl("rptTripRow") as Repeater;
                if (rptTripRow != null)
                {
                    rptTripRow.DataSource = Module.TripGetAll(true, UserIdentity);
                    rptTripRow.DataBind();
                }
            }

            #region -- Item --
            if (e.Item.DataItem is DateTime)
            {
                Literal litTr = e.Item.FindControl("litTr") as Literal;
                if (litTr != null)
                {
                    if (e.Item.ItemType == ListItemType.Item)
                    {
                        litTr.Text = "<tr>";
                    }
                    else
                    {
                        litTr.Text = @"<tr style=""background-color:#E9E9E9"">";
                    }
                }

                DateTime date = (DateTime)e.Item.DataItem;
                Label labelDate = (Label)e.Item.FindControl("labelDate");
                if (labelDate != null)
                {
                    labelDate.Text = date.ToString("dd/MM/yyyy");
                }

                HyperLink hplDate = (HyperLink)e.Item.FindControl("hplDate");
                if (hplDate != null)
                {
                    hplDate.Text = date.ToString("dd/MM/yyyy");
                    hplDate.NavigateUrl = string.Format("BookingReport.aspx?NodeId={0}&SectionId={1}&Date={2}", Node.Id,
                                                        Section.Id, date.ToOADate());
                }

                #region -- Counting --
                int count;
                // Điều kiện bắt buộc
                ICriterion criterion = Expression.And(Expression.Eq(Booking.DELETED, false),
                                                      Expression.Eq(Booking.STATUS, StatusType.Approved));
                // Không bao gồm booking transfer
                criterion = Expression.And(criterion, Expression.Not(Expression.Eq("IsTransferred", true)));
                criterion = Module.AddDateExpression(criterion, date);
                _util.Bookings = Module.BookingGetByCriterion(criterion, null, out count, 0, 0, false, UserIdentity);

                Repeater rptTripCustomer = e.Item.FindControl("rptTripCustomer") as Repeater;
                if (rptTripCustomer != null)
                {
                    rptTripCustomer.DataSource = Module.TripGetAll(true, UserIdentity);
                    rptTripCustomer.DataBind();
                }
                #endregion
            }
            #endregion
        }

        protected void rptTripRow_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is SailsTrip)
            {
                SailsTrip cruise = (SailsTrip)e.Item.DataItem;
                HtmlTableCell thTrip = e.Item.FindControl("thTrip") as HtmlTableCell;
                if (thTrip != null)
                {
                    thTrip.InnerText = cruise.Name;
                }
            }
        }

        protected void rptTripCustomer_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is SailsTrip)
            {
                SailsTrip trip = (SailsTrip) e.Item.DataItem;
                Literal litGroup = e.Item.FindControl("litGroup") as Literal;
                if (litGroup!=null)
                {
                    litGroup.Text = _util.BookingCount(trip).ToString();
                }
                Literal litCustomer = e.Item.FindControl("litCustomer") as Literal;
                if (litCustomer!=null)
                {
                    litCustomer.Text = _util.CustomerCount(trip).ToString();
                }
            }
        }
    }
}
