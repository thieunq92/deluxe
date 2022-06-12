using CMS.Core.Domain;
using Portal.Modules.OrientalSails.BusinessLogic;
using Portal.Modules.OrientalSails.BusinessLogic.Share;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.Admin.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class ProductAnalysis : System.Web.UI.Page
    {
        private ProductAnalysisBLL productAnalysisBLL;
        private UserBLL userBLL;
        public ProductAnalysisBLL ProductAnalysisBLL
        {
            get
            {
                if (productAnalysisBLL == null)
                {
                    productAnalysisBLL = new ProductAnalysisBLL();
                }
                return productAnalysisBLL;
            }
        }
        public UserBLL UserBLL
        {
            get
            {
                if (userBLL == null)
                    userBLL = new UserBLL();
                return userBLL;
            }
        }
        public User CurrentUser
        {
            get
            {
                return UserBLL.UserGetCurrent();
            }
        }
        public int SelectedMonth
        {
            get
            {
                int selectedMonth = DateTime.Today.Month;
                try
                {
                    selectedMonth = Int32.Parse(ddlMonth.SelectedValue);
                }
                catch { }
                return selectedMonth;
            }
        }
        public SailsTrip SelectedTrip
        {
            get
            {
                int selectedTripId = 0;
                try
                {
                    selectedTripId = Int32.Parse(ddlTrip.SelectedValue);
                }
                catch { }
                var selectedTrip = ProductAnalysisBLL.TripGetById(selectedTripId);
                if (selectedTrip.Id == 0) { selectedTrip = null; }
                return selectedTrip;
            }
        }
        public double TotalRevenue { get; set; }
        public double TotalExpense { get; set; }
        public double TotalProfitAndLoss { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlMonth.Items.AddRange(Enumerable.Range(1, 12).Select(x => new ListItem() { Text = DateTimeFormatInfo.CurrentInfo.GetMonthName(x), Value = x.ToString() }).ToArray());
                ddlMonth.SelectedValue = DateTime.Today.Month.ToString();
                var regions = ProductAnalysisBLL.RegionGetAllByUser(CurrentUser);
                var trips = new List<SailsTrip>();
                foreach (var region in regions)
                {
                    trips.AddRange(ProductAnalysisBLL.TripGetAllByRegion(region));
                }
                ddlTrip.DataSource = trips;
                ddlTrip.DataTextField = "Name";
                ddlTrip.DataValueField = "Id";
                ddlTrip.DataBind();
            }
            var months = new List<DateTime>();
            var currentMonth = new DateTime(DateTime.Today.Year, SelectedMonth, 1);
            var previousMonth = currentMonth.AddMonths(-1);
            var previousTwoMonth = currentMonth.AddMonths(-2);
            months.Add(previousTwoMonth);
            months.Add(previousMonth);
            months.Add(currentMonth);
            rptMonths.DataSource = months;
            rptMonths.DataBind();
            ltrMonth.Text = ltrMonth1.Text = DateTimeFormatInfo.CurrentInfo.GetMonthName(SelectedMonth);
            var top10Agencies = ProductAnalysisBLL.AgencyGetTop10InMonthAndTrip(SelectedMonth, SelectedTrip);
            rptTop10Partner.DataSource = top10Agencies;
            rptTop10Partner.DataBind();
            rptTotalPax.DataSource = months;
            rptTotalPax.DataBind();
            rptTotalRevenue.DataSource = months;
            rptTotalRevenue.DataBind();
            rptExpense.DataSource = months;
            rptExpense.DataBind();
            rptProfitAndLoss.DataSource = months;
            rptProfitAndLoss.DataBind();
            var tripCodes = ProductAnalysisBLL.ExpenseServiceGetAllInMonth(SelectedMonth, DateTime.Today.Year, SelectedTrip)
                .GroupBy(es => new
                {
                    es.Expense,
                    es.Group
                }).Select(ges => new EventCode(SailsModule.GetInstance())
                {
                    SailExpense = ges.Key.Expense,
                    Group = ges.Key.Group
                }).OrderBy(ges => ges.SailExpense.Trip.TripCode).ThenBy(ges => ges.SailExpense.Date).ThenBy(ges => ges.Group);
            rptTrips.DataSource = tripCodes;
            rptTrips.DataBind();
            if (SelectedTrip == null)
            {
                hplPriceConfig.Visible = false;
            }
            if (SelectedTrip != null)
            {
                hplPriceConfig.Visible = true;
                hplPriceConfig.NavigateUrl = String.Format("SailsPriceConfig.aspx?NodeId=1&SectionId=15&TripId={0}&Option=1&cruiseid=3", SelectedTrip.Id);
            }
        }
        protected void Page_Unload(object sender, EventArgs e)
        {
            if (userBLL != null)
            {
                userBLL.Dispose();
                userBLL = null;
            }
            if (productAnalysisBLL != null)
            {
                productAnalysisBLL.Dispose();
                productAnalysisBLL = null;
            }
        }
        public string ConvertToMonthAndYearString(DateTime date)
        {
            return date.ToString("MM/yyyy");
        }
        public int GetNumberOfPaxInMonth(int month, int year, SailsTrip trip)
        {
            return ProductAnalysisBLL.CustomerGetNumberOfCustomersInMonth(month, year, trip);
        }
        public double GetTotalOfBookingsInMonth(int month, int year, SailsTrip trip)
        {
            var totalRevenue = ProductAnalysisBLL.BookingGetTotalRevenueInMonth(month, year, trip);
            return Math.Round(totalRevenue);
        }
        public double GetExpenseInMonth(int month, int year, SailsTrip trip)
        {
            var bookings = ProductAnalysisBLL.BookingGetAllInMonth(month, year, trip);
            var totalCommission = bookings.Select(b => b.IsCommissionVND ? b.CommissionVND : b.Commission * b.CurrencyRate).Sum();
            var expenses = ProductAnalysisBLL.ExpenseServiceGetAllInMonth(month, year, trip);
            var totalExpense = expenses.Select(e => e.Cost).Sum();
            totalExpense += totalCommission;
            return Math.Round(totalExpense);
        }

        protected void rptTotalPax_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var ltrTotalPax = e.Item.FindControl("ltrTotalPax") as Literal;
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var date = (DateTime)e.Item.DataItem;
                var month = date.Month;
                var year = date.Year;
                ltrTotalPax.Text = GetNumberOfPaxInMonth(month, year, SelectedTrip).ToString();
            }
        }

        protected void rptTotalRevenue_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var ltrTotalRevenue = e.Item.FindControl("ltrTotalRevenue") as Literal;
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var date = (DateTime)e.Item.DataItem;
                var month = date.Month;
                var year = date.Year;
                ltrTotalRevenue.Text = NumberUtil.FormatMoney(GetTotalOfBookingsInMonth(month, year, SelectedTrip)) + "<strong>₫</strong>";
            }
        }

        protected void rptExpense_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var ltrExpense = e.Item.FindControl("ltrExpense") as Literal;
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var date = (DateTime)e.Item.DataItem;
                var month = date.Month;
                var year = date.Year;
                ltrExpense.Text = NumberUtil.FormatMoney(GetExpenseInMonth(month, year, SelectedTrip)) + "<strong>₫</strong>";
            }
        }

        protected void rptProfitAndLoss_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var ltrProfitAndLoss = e.Item.FindControl("ltrProfitAndLoss") as Literal;
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var date = (DateTime)e.Item.DataItem;
                var month = date.Month;
                var year = date.Year;
                var totalRevenue = GetTotalOfBookingsInMonth(month, year, SelectedTrip);
                var totalExpense = GetExpenseInMonth(month, year, SelectedTrip);
                ltrProfitAndLoss.Text = NumberUtil.FormatMoney(totalRevenue - totalExpense) + "<strong>₫</strong>";
            }
        }

        protected void rptTrips_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var ltrTripCode = e.Item.FindControl("ltrTripCode") as Literal;
                var ltrRevenue = e.Item.FindControl("ltrRevenue") as Literal;
                var ltrExpense = e.Item.FindControl("ltrExpense") as Literal;
                var ltrProfitAndLoss = e.Item.FindControl("ltrProfitAndLoss") as Literal;
                var tdRevenue = e.Item.FindControl("tdRevenue") as HtmlTableCell;
                var tdExpense = e.Item.FindControl("tdExpense") as HtmlTableCell;

                var code = (EventCode)e.Item.DataItem;
                ltrTripCode.Text = "<a href='EventEdit.aspx?NodeId=1&SectionId=15&expenseid=" + code.SailExpense.Id + "&group=" + code.Group + "'>" + string.Format("{0}{1}-{2:00}", code.SailExpense.Trip.TripCode, code.SailExpense.Date.ToString("ddMMyy"), code.Group + "</a>");
                double revenuePaid = 0;
                double revenueReceivable = 0;
                double revenueTotal = 0;

                double expensePaid = 0;
                double expensePayable = 0;
                double expenseTotal = 0;
                foreach (Booking booking in code.Bookings)
                {
                    double paid = booking.Total * booking.CurrencyRate - booking.TotalReceivable;
                    revenuePaid += paid;
                    revenueReceivable += booking.TotalReceivable;
                    if (booking.IsVND)
                    {
                        revenueTotal += booking.TotalVND;
                    }
                    else
                    {
                        revenueTotal += booking.Total * booking.CurrencyRate;
                    }
                    if (booking.IsCommissionVND)
                    {
                        expenseTotal += booking.CommissionVND;
                    }
                    else
                    {
                        expenseTotal += booking.Commission * booking.CurrencyRate;
                    }
                }
                foreach (ExpenseService service in code.Services)
                {
                    expensePaid += service.Paid;
                    expensePayable += service.Cost - service.Paid;
                    expenseTotal += service.Cost;
                }
                ltrRevenue.Text = NumberUtil.FormatMoney(revenueTotal) + "<strong>₫</strong>";
                ltrExpense.Text = NumberUtil.FormatMoney(expenseTotal) + "<strong>₫</strong>";
                ltrProfitAndLoss.Text = NumberUtil.FormatMoney(revenueTotal - expenseTotal) + "<strong>₫</strong>";
                if (revenueReceivable > 0)
                {
                    tdRevenue.Attributes["class"] += " --cancelled";
                }

                if (expensePayable > 0)
                {
                    tdExpense.Attributes["class"] += " --cancelled";
                }
                TotalRevenue += revenueTotal;
                TotalExpense += expenseTotal;
                TotalProfitAndLoss += (revenueTotal - expenseTotal);
            }
            if (e.Item.ItemType == ListItemType.Footer)
            {
                var ltrTotalRevenue = e.Item.FindControl("ltrTotalRevenue") as Literal;
                var ltrTotalExpense = e.Item.FindControl("ltrTotalExpense") as Literal;
                var ltrTotalProfitAndLoss = e.Item.FindControl("ltrTotalProfitAndLoss") as Literal;
                ltrTotalRevenue.Text = NumberUtil.FormatMoney(TotalRevenue) + "₫";
                ltrTotalExpense.Text = NumberUtil.FormatMoney(TotalExpense) + "₫";
                ltrTotalProfitAndLoss.Text = NumberUtil.FormatMoney(TotalProfitAndLoss) + "₫";
            }
        }
    }
}