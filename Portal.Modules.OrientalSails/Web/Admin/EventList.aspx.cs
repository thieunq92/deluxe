using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.Web.Util;
using NHibernate.Criterion;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.UI;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class EventList : SailsAdminBasePage
    {
        #region -- PAGE EVENTS --
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "Events list";
            pagerEvents.AllowCustomPaging = true;
            if (!IsPostBack)
            {
                GetData();

                //if (!string.IsNullOrEmpty(Request.QueryString["from"]))
                //{
                //    var date = DateTime.FromOADate(Convert.ToDouble(Request.QueryString["from"]));
                //    txtFrom.Text = date.ToString("dd/MM/yyyy");
                //}

                //if (!string.IsNullOrEmpty(Request.QueryString["to"]))
                //{
                //    var date = DateTime.FromOADate(Convert.ToDouble(Request.QueryString["to"]));
                //    txtTo.Text = date.ToString("dd/MM/yyyy");
                //}
            }
        }

        #endregion

        #region -- CONTROL EVENTS --
        protected void rptEvents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is SailExpense)
            {
                var expense = (SailExpense)e.Item.DataItem;

                IList<EventCode> list = new List<EventCode>();

                int groups = expense.NumberOfGroup;
                if (groups == 0) groups = 1;

                for (int ii = 1; ii <= groups; ii++)
                {
                    var eventCode = new EventCode(Module);
                    eventCode.SailExpense = expense;
                    eventCode.Group = ii;
                    list.Add(eventCode);
                }

                var rptGroups = (Repeater)e.Item.FindControl("rptGroups");
                rptGroups.DataSource = list;
                rptGroups.DataBind();
            }
        }

        protected void rptGroups_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is EventCode)
            {
                var code = (EventCode)e.Item.DataItem;
                var hplEventCode = e.Item.FindControl("hplEventCode") as HyperLink;
                if (hplEventCode != null)
                {
                    hplEventCode.Text = string.Format("{0}{1:ddMMyy}-{2:00}", code.SailExpense.Trip.TripCode, code.SailExpense.Date, code.Group);
                    hplEventCode.NavigateUrl =
                        string.Format("EventEdit.aspx?NodeId={0}&SectionId={1}&expenseid={2}&group={3}", Node.Id,
                                      Section.Id, code.SailExpense.Id, code.Group);
                }

                var hplDate = e.Item.FindControl("hplDate") as HyperLink;
                if (hplDate != null)
                {
                    hplDate.Text = code.SailExpense.Date.ToString("dd/MM/yyyy");
                    hplDate.NavigateUrl = string.Format("BookingReport.aspx?NodeId={0}&SectionID={1}&Date={2}&tripid={3}", Node.Id,
                                                        Section.Id, code.SailExpense.Date.ToOADate(), code.SailExpense.Trip.Id);
                }

                var hplTrip = e.Item.FindControl("hplTrip") as HyperLink;
                if (hplTrip != null)
                {
                    hplTrip.Text = code.SailExpense.Trip.Name;
                }

                int pax = 0;
                double revenuePaid = 0;
                double revenueReceivable = 0;
                double revenueTotal = 0;

                double expensePaid = 0;
                double expensePayable = 0;
                double expenseTotal = 0;
                foreach (Booking booking in code.Bookings)
                {
                    pax += booking.Pax;

                    double paid = (booking.IsVND ? booking.TotalVND : booking.Total * booking.CurrencyRate) -
                                      booking.TotalReceivable;
                    revenuePaid += paid;
                    revenueReceivable += booking.TotalReceivable;
                    revenueTotal += booking.IsVND ? booking.TotalVND : booking.Total * booking.CurrencyRate;
                }

                foreach (ExpenseService service in code.SailExpense.Services)
                {
                    if (service.Group == code.Group)
                    {
                        expensePaid += service.Paid;
                        expensePayable += service.Cost - service.Paid;
                        expenseTotal += service.Cost;
                    }
                }

                if (pax == 0 && expenseTotal == 0 && revenueTotal == 0)
                {
                    e.Item.Visible = false;
                    return;
                }

                ValueBinder.BindLiteral(e.Item, "litPax", pax);
                ValueBinder.BindLiteral(e.Item, "litRevenuePaid", revenuePaid.ToString("#,0.#"));
                ValueBinder.BindLiteral(e.Item, "litReceivable", revenueReceivable.ToString("#,0.#"));
                ValueBinder.BindLiteral(e.Item, "litRevenue", revenueTotal.ToString("#,0.#"));
                ValueBinder.BindLiteral(e.Item, "litExpensePaid", expensePaid.ToString("#,0.#"));
                ValueBinder.BindLiteral(e.Item, "litPayable", expensePayable.ToString("#,0.#"));
                ValueBinder.BindLiteral(e.Item, "litExpense", expenseTotal.ToString("#,0.#"));
                ValueBinder.BindLiteral(e.Item, "litProfit", (revenuePaid - expenseTotal).ToString("#,0.#"));
            }
        }

        protected void buttonSearch_Click(object sender, EventArgs e)
        {
            string query = string.Empty;
            if (!string.IsNullOrEmpty(txtFrom.Text))
            {
                var date = DateTime.ParseExact(txtFrom.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                query += string.Format("&from={0}", date.ToOADate());
            }

            if (!string.IsNullOrEmpty(txtTo.Text))
            {
                var date = DateTime.ParseExact(txtTo.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                query += string.Format("&to={0}", date.ToOADate());
            }

            PageRedirect(string.Format("EventList.aspx?NodeId={0}&SectionId={1}{2}", Node.Id, Section.Id, query));
        }
        #endregion

        #region -- PRIVATE METHODS --
        private void GetData()
        {
            int count;

            DateTime from;
            DateTime to;

            if (Request.QueryString["from"] != null)
            {
                from = DateTime.FromOADate(Convert.ToDouble(Request.QueryString["from"]));
            }
            else
            {
                from = DateTime.Today.AddDays(-DateTime.Today.Day + 1);
            }

            txtFrom.Text = from.ToString("dd/MM/yyyy");

            if (Request.QueryString["to"] != null)
            {
                to = DateTime.FromOADate(Convert.ToDouble(Request.QueryString["from"]));
            }
            else
            {
                to = from.AddMonths(1).AddDays(-1);
            }

            txtTo.Text = to.ToString("dd/MM/yyyy");

            ICriterion criterion = Expression.And(Expression.Ge("Date", from), Expression.Le("Date", to));

            rptEvents.DataSource = Module.EventGet(criterion, pagerEvents.PageSize, pagerEvents.CurrentPageIndex, out count);
            pagerEvents.VirtualItemCount = count;
            rptEvents.DataBind();
        }
        #endregion
    }

    public class EventCode
    {

        private readonly SailsModule _module;
        public EventCode() { }
        public EventCode(SailsModule module)
        {
            _module = module;
        }

        public SailExpense SailExpense { get; set; }
        public int Group { get; set; }

        private IList _bookings;
        public IList Bookings
        {
            get
            {
                _bookings = new ArrayList();
                IList temp = _module.BookingGetByStartDate(SailExpense.Date, SailExpense.Trip, false);
                foreach (Booking booking in temp)
                {
                    if (booking.Group == Group)
                    {
                        _bookings.Add(booking);
                    }
                }
                return _bookings;
            }
        }

        private IList _services;
        public IList Services
        {
            get
            {
                if (_services == null)
                {
                    _services = new ArrayList();
                    foreach (ExpenseService service in SailExpense.Services)
                    {
                        if (service.Group == Group)
                        {
                            _services.Add(service);
                        }
                    }
                }
                return _services;
            }
        }
    }
}