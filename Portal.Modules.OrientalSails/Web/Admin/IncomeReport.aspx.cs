using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using NHibernate.Criterion;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.UI;
using Portal.Modules.OrientalSails.Web.Util;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class IncomeReport : SailsAdminBase
    {
        private double _total;
        private double _bar;
        private int _pax;

        private IList _trips;
        private Dictionary<SailsTrip, double> _incomes;
        private Dictionary<SailsTrip, double> _currentIncomes;

        protected IList AllTrips
        {
            get
            {
                if (_trips == null)
                {
                    _trips = Module.TripGetAll(true, UserIdentity);
                }
                return _trips;
            }
        }

        private Cruise _cruise;
        protected Cruise ActiveCruise
        {
            get
            {
                if (_cruise == null && Request.QueryString["cruiseid"] != null)
                {
                    _cruise = Module.CruiseGetById(Convert.ToInt32(Request.QueryString["cruiseid"]));
                }
                return _cruise;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = Resources.titleIncomeReport;
            if (!IsPostBack)
            {
                if (Request.QueryString["from"] != null)
                {
                    DateTime from = DateTime.FromOADate(Convert.ToDouble(Request.QueryString["from"]));
                    txtFrom.Text = from.ToString("dd/MM/yyyy");
                }
                if (Request.QueryString["to"] != null)
                {
                    DateTime to = DateTime.FromOADate(Convert.ToDouble(Request.QueryString["to"]));
                    txtTo.Text = to.ToString("dd/MM/yyyy");
                }
                GetDataSource();
                rptBookingList.DataBind();

                BindCruises();
            }
        }

        protected void BindCruises()
        {
            IList cruises = Module.CruiseGetAll();
            //if (cruises.Count == 1)
            //{
            //    if (ActiveCruise == null)
            //    {
            //        Cruise cruise = (Cruise)cruises[0];
            //        PageRedirect(string.Format("IncomeReport.aspx?NodeId={0}&SectionId={1}&cruiseid={2}", Node.Id, Section.Id, cruise.Id));
            //    }
            //    else
            //    {
            //        rptCruises.Visible = false;
            //    }
            //}
            //else
            //{
            //    rptCruises.DataSource = cruises;
            //    rptCruises.DataBind();
            //}
        }

        protected void btnDisplay_Click(object sender, EventArgs e)
        {
            DateTime from = DateTime.ParseExact(txtFrom.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime to = DateTime.ParseExact(txtTo.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            PageRedirect(string.Format("IncomeReport.aspx?NodeId={0}&SectionId={1}&from={2}&to={3}", Node.Id, Section.Id, from.ToOADate(), to.ToOADate()));
            //GetDataSource();
            //rptBookingList.DataBind();
        }

        protected void rptBookingList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Dictionary<SailsTrip, double> incomes = new Dictionary<SailsTrip, double>(AllTrips.Count);
            double total = 0;
            foreach (SailsTrip trip in AllTrips)
            {
                incomes.Add(trip, 0);
            }

            if (e.Item.DataItem is DateTime)
            {
                DateTime date = (DateTime)e.Item.DataItem;
                HyperLink hplDate = (HyperLink)e.Item.FindControl("hplDate");
                if (hplDate != null)
                {
                    hplDate.Text = date.ToString("dd/MM/yyyy");
                    hplDate.NavigateUrl = string.Format("BookingReport.aspx?NodeId={0}&SectionId={1}&Date={2}", Node.Id,
                                                        Section.Id, date.ToOADate());
                }

                #region -- Counting --
                int count;
                // Khi tính income thì chỉ tính theo khách đã check-in
                ICriterion criterion = Expression.And(Expression.Eq(Booking.STARTDATE, date),
                                                      SailsModule.IncomeCriterion());
                // Bỏ deleted và cả transfer
                criterion = Expression.And(Expression.Eq("Deleted", false), criterion);
                criterion = Expression.And(Expression.Eq("IsTransferred", false), criterion);

                if (ActiveCruise != null)
                {
                    criterion = Expression.And(Expression.Eq("Cruise", ActiveCruise), criterion);
                }

                IList bookings =
                    Module.BookingGetByCriterion(criterion, null, out count, 0, 0, false, UserIdentity);
                int pax = 0;

                foreach (Booking booking in bookings)
                {
                    if (booking.Status == StatusType.Approved)
                    {
                        // Số khách trong cả book, bao gồm cả trẻ em và trẻ sơ sinh
                        int inBook = 0;
                        inBook += booking.Adult;
                        foreach (BookingRoom room in booking.BookingRooms)
                        {
                            if (room.HasChild)
                            {
                                inBook++;
                            }
                            if (room.HasBaby)
                            {
                                inBook++;
                            }
                        }
                        pax += inBook;
                    }
                    _total += booking.Value;
                    total += booking.Value;
                    incomes[booking.Trip] += booking.Value;
                    _incomes[booking.Trip] += booking.Value;                    
                }
                _pax += pax;
                Literal litTotalPax = (Literal)e.Item.FindControl("litTotalPax");
                Literal litTotal = (Literal)e.Item.FindControl("litTotal");
                litTotalPax.Text = pax.ToString();
                litTotal.Text = (total).ToString("#,0");

                #endregion

                _currentIncomes = incomes;
                Repeater rptTrip = (Repeater)e.Item.FindControl("rptTrip");
                rptTrip.DataSource = AllTrips;
                rptTrip.DataBind();

                if (ActiveCruise != null)
                {
                    BarRevenue bar = Module.BarRevenueGetByDate(ActiveCruise, date);
                    Literal litBar = e.Item.FindControl("litBar") as Literal;
                    if (litBar != null)
                    {
                        litBar.Text = bar.Revenue.ToString("0");
                        _bar += bar.Revenue;
                    }
                }
                else
                {
                    double bar = Module.SumBarByDate(date);
                    Literal litBar = e.Item.FindControl("litBar") as Literal;
                    if (litBar != null)
                    {
                        litBar.Text = bar.ToString("0");
                        _bar += bar;
                    }
                }
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                Literal litTotalPax = (Literal)e.Item.FindControl("litTotalPax");
                Literal litTotal = (Literal)e.Item.FindControl("litTotal");
                Repeater rptTrip = (Repeater)e.Item.FindControl("rptTrip");
                rptTrip.DataSource = AllTrips;
                rptTrip.DataBind();
                litTotalPax.Text = _pax.ToString();
                litTotal.Text = _total.ToString("#,0");

                Literal litBar = e.Item.FindControl("litBar") as Literal;
                if (litBar != null)
                {
                    litBar.Text = _bar.ToString("0");
                }
            }
            else if (e.Item.ItemType == ListItemType.Header)
            {
                Repeater rptTrip = (Repeater)e.Item.FindControl("rptTrip");
                rptTrip.DataSource = AllTrips;
                rptTrip.DataBind();
            }
        }

        protected void rptItemTrip_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is SailsTrip)
            {
                Literal litTotal = e.Item.FindControl("litTotal") as Literal;
                if (litTotal != null)
                {
                    litTotal.Text = _currentIncomes[(SailsTrip)e.Item.DataItem].ToString("#,0");
                }
            }
        }

        protected void rptFooterTrip_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is SailsTrip)
            {
                Literal litTotal = e.Item.FindControl("litTotal") as Literal;
                if (litTotal != null)
                {
                    litTotal.Text = _incomes[(SailsTrip)e.Item.DataItem].ToString("#,0");
                }
            }
        }

        protected void rptBookingList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }

        private void GetDataSource()
        {
            DateTime from;
            DateTime to;
            if (string.IsNullOrEmpty(txtFrom.Text) || string.IsNullOrEmpty(txtTo.Text))
            {
                from = DateTime.Today.AddDays(-DateTime.Today.Day + 1);
                to = from.AddMonths(1).AddDays(-1);
            }
            else
            {
                from = DateTime.ParseExact(txtFrom.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                to = DateTime.ParseExact(txtTo.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            txtFrom.Text = from.ToString("dd/MM/yyyy");
            txtTo.Text = to.ToString("dd/MM/yyyy");
            IList list = new ArrayList();
            while (from <= to)
            {
                list.Add(from);
                from = from.AddDays(1);
            }
            rptBookingList.DataSource = list;

            _incomes = new Dictionary<SailsTrip, double>(AllTrips.Count);
            foreach (SailsTrip trip in AllTrips)
            {
                _incomes.Add(trip, 0);
            }
        }

        protected void rptCruises_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is Cruise)
            {
                var cruise = (Cruise)e.Item.DataItem;

                var liMenu = e.Item.FindControl("liMenu") as HtmlGenericControl;
                if (liMenu != null)
                {
                    if (cruise.Id.ToString() == Request.QueryString["cruiseid"])
                    {
                        liMenu.Attributes.Add("class", "selected");
                    }
                }

                var hplCruises = e.Item.FindControl("hplCruises") as HyperLink;
                if (hplCruises != null)
                {
                    DateTime from = DateTime.ParseExact(txtFrom.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime to = DateTime.ParseExact(txtTo.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    hplCruises.Text = cruise.Name;
                    hplCruises.NavigateUrl =
                        string.Format("IncomeReport.aspx?NodeId={0}&SectionId={1}&from={2}&to={3}&cruiseid={4}", Node.Id, Section.Id, from.ToOADate(), to.ToOADate(), cruise.Id);
                }
            }
            else
            {
                HtmlGenericControl liMenu = e.Item.FindControl("liMenu") as HtmlGenericControl;
                if (liMenu != null)
                {
                    if (Request.QueryString["cruiseid"] == null)
                    {
                        liMenu.Attributes.Add("class", "selected");
                    }
                }
                HyperLink hplCruises = e.Item.FindControl("hplCruises") as HyperLink;
                if (hplCruises != null)
                {
                    DateTime from = DateTime.ParseExact(txtFrom.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime to = DateTime.ParseExact(txtTo.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    hplCruises.NavigateUrl =
                        string.Format("IncomeReport.aspx?NodeId={0}&SectionId={1}&from={2}&to={3}", Node.Id,
                                      Section.Id, from.ToOADate(), to.ToOADate());
                }
            }
        }
    }
}
