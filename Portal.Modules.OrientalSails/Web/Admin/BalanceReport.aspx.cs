using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.UI;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class BalanceReport : SailsAdminBase
    {
        private readonly Dictionary<DateTime, double> _monthExpense = new Dictionary<DateTime, double>();
        private readonly Dictionary<DateTime, double> _yearExpense = new Dictionary<DateTime, double>();

        private SailsTrip _cruise;

        private IList _cruises;
        private USDRate _currentRate;
        private double _income;
        private double _outcome;
        private double _payable;
        private double _receivable;

        protected SailsTrip ActiveCruise
        {
            get
            {
                if (_cruise == null && Request.QueryString["cruiseid"] != null)
                {
                    _cruise = Module.TripGetById(Convert.ToInt32(Request.QueryString["cruiseid"]));
                }
                return _cruise;
            }
        }

        protected IList AllCruises
        {
            get
            {
                if (_cruises == null)
                {
                    _cruises = Module.CruiseGetAll();
                }
                return _cruises;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = @"Bảng cân đối";
            if (!IsPostBack)
            {
                if (Request.QueryString["month"] != null)
                {
                    ddlMonths.SelectedIndex = Convert.ToInt32(Request.QueryString["month"]);
                }

                if (Request.QueryString["year"] != null)
                {
                    txtYear.Text = Request.QueryString["year"];
                }
                GetDataSource();

                #region -- Danh sách các tàu --

                if (AllCruises.Count == 1)
                {
                    rptCruises.Visible = false;
                }
                else
                {
                    rptCruises.DataSource = Module.TripGetAll(true, UserIdentity);
                    rptCruises.DataBind();
                }

                #endregion
            }
        }

        protected void rptBalance_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                return;
            }

            #region -- Lấy control --

            Literal litIncome = (Literal) e.Item.FindControl("litIncome");
            Literal litReceivable = (Literal) e.Item.FindControl("litReceivable");
            Literal litExpense = (Literal) e.Item.FindControl("litExpense");
            Literal litPayable = (Literal) e.Item.FindControl("litPayable");
            Literal litBalance = (Literal) e.Item.FindControl("litBalance");

            #endregion

            #region -- Footer --

            if (e.Item.ItemType == ListItemType.Footer)
            {
                litIncome.Text = _income.ToString("#,0.#");
                litReceivable.Text = _income.ToString("#,0.#");
                litExpense.Text = _outcome.ToString("#,0.#");
                litPayable.Text = _payable.ToString("#,0.#");
                litBalance.Text = (_income - _outcome).ToString("#,0.#");
                return;
            }

            #endregion

            Literal litDate = (Literal) e.Item.FindControl("litDate");

            if (e.Item.DataItem is SailExpense)
            {
                SailExpense expense = (SailExpense) e.Item.DataItem;
                litDate.Text = expense.Date.ToString("dd/MM/yyyy");

                #region -- Income --

                IList bookings = Module.BookingGetByStartDate(expense.Date, ActiveCruise, true);
                double income = 0;
                double receivable = 0;
                foreach (Booking booking in bookings)
                {
                    if (UseVNDExpense)
                    {
                        if (booking.IsPaid)
                        {
                            if (booking.CurrencyRate > 0)
                            {
                                income += booking.PaidBase + booking.Paid*booking.CurrencyRate;
                            }
                            else
                            {
                                income += booking.PaidBase + booking.Paid*_currentRate.Rate;
                            }
                        }
                        else
                        {
                            if (booking.CurrencyRate > 0)
                            {
                                income += booking.Value;
                            }
                            else
                            {
                                income += booking.Value;
                            }
                        }
                    }
                    else
                    {
                        income += booking.Value;
                    }

                    receivable += booking.TotalReceivable;
                }

                litIncome.Text = income.ToString("#,0.#");
                litReceivable.Text = receivable.ToString("#,0.#");

                #endregion

                double outcome = 0;

                if (ActiveCruise != null)
                {
                    outcome = GetOutcome(expense, bookings);
                }
                else
                {
                    foreach (SailsTrip cruise in Module.TripGetAll(true, UserIdentity))
                    {
                        SailExpense ex = Module.ExpenseGetByDate(cruise, expense.Date);
                        outcome += GetOutcome(ex, bookings);
                    }
                }

                litExpense.Text = outcome.ToString("#,0.#");

                litBalance.Text = (income - outcome).ToString("#,0.#");
                _income += income;
                _outcome += outcome;
                _receivable += receivable;
            }
            else if (e.Item.DataItem is int)
            {
                int month = (int) e.Item.DataItem;
                int year = Convert.ToInt32(txtYear.Text);
                DateTime dateFrom = new DateTime(year, month, 1);
                DateTime dateTo = dateFrom.AddMonths(1).AddDays(-1);

                litDate.Text = string.Format("{0:00}/{1:0000}", month, year);

                double income;
                double receivable;
                Module.IncomeSum(dateFrom, dateTo, out income, out receivable);

                litIncome.Text = income.ToString("#,0.#");
                litReceivable.Text = receivable.ToString("#,0.#");

                double outcome;
                double payable;
                Module.OutcomeSum(dateFrom, dateTo, out outcome, out payable);

                litExpense.Text = outcome.ToString("#,0.#");
                litPayable.Text = payable.ToString("#,0.#");

                litBalance.Text = (income - outcome).ToString("#,0.#");

                _income += income;
                _outcome += outcome;
                _receivable += receivable;
                _payable += payable;
            }
        }

        private double GetOutcome(SailExpense expense, IList bookings)
        {
            double outcome = 0;

            #region -- Cộng tổng từng service --

            foreach (ExpenseService service in expense.Services)
            {
                try
                {
                    if (service.Type.IsMonthly || service.Type.IsYearly)
                    {
                        continue;
                    }
                }
                catch
                {
                    continue;
                }
                outcome += service.Cost;
            }

            #endregion

            if (PeriodExpenseAvg)
            {
                #region -- Chi phí theo nam/thang --

                // Nếu có chạy hoặc là tháng chưa kết thúc
                if (bookings.Count > 0 &&
                    expense.Date.Month + expense.Date.Year*12 >= DateTime.Today.Month + DateTime.Today.Year*12)
                {
                    // Tính chi phí tháng
                    DateTime dateMonth = new DateTime(expense.Date.Year, expense.Date.Month, 1);
                    int runcount;
                    // Nếu là tháng chưa kết thúc
                    if (dateMonth.AddMonths(1) > DateTime.Today)
                    {
                        runcount = dateMonth.AddMonths(1).Subtract(dateMonth).Days;
                    }
                    else
                    {
                        //runcount = Module.RunningDayCount(ActiveCruise, expense.Date.Year, expense.Date.Month);
                    }
                    if (!_monthExpense.ContainsKey(dateMonth))
                    {
                        //SailExpense monthExpense = Module.ExpenseGetByDate(ActiveCruise, dateMonth);
                        //if (monthExpense.Id < 0)
                        //{
                        //    Module.SaveOrUpdate(monthExpense);
                        //}
                        //double total = Module.CopyMonthlyCost(monthExpense);
                        //_monthExpense.Add(dateMonth, total/runcount);
                    }
                    outcome += _monthExpense[dateMonth];
                }

                // Nếu có chạy hoặc năm chưa kết thúc
                if (bookings.Count > 0 && expense.Date.Year >= DateTime.Today.Year)
                {
                    // Tính chi phí năm
                    DateTime dateYear = new DateTime(expense.Date.Year, 1, 1);
                    int runcount;
                    // Nếu là năm chưa kết thúc
                    if (dateYear.AddYears(1) > DateTime.Today)
                    {
                        runcount = dateYear.AddYears(1).Subtract(dateYear).Days;
                    }
                    else
                    {
                        //runcount = Module.RunningDayCount(ActiveCruise, expense.Date.Year, 0);
                    }
                    if (!_yearExpense.ContainsKey(dateYear))
                    {
                        //SailExpense yearExpense = Module.ExpenseGetByDate(ActiveCruise, dateYear);
                        //if (yearExpense.Id < 0)
                        //{
                        //    Module.SaveOrUpdate(yearExpense);
                        //}
                        //double total = Module.CopyYearlyCost(yearExpense);
                        //_yearExpense.Add(dateYear, total/runcount);
                    }

                    outcome += _yearExpense[dateYear];
                }

                #endregion
            }
            return outcome;
        }

        protected void btnDisplay_Click(object sender, EventArgs e)
        {
            int month = Convert.ToInt32(ddlMonths.SelectedIndex);
            int year = Convert.ToInt32(txtYear.Text);
            string url = string.Format("BalanceReport.aspx?NodeId={0}&SectionId={1}&month={2}&year={3}", Node.Id,
                                       Section.Id, month, year);
            PageRedirect(url);
        }

        protected void GetDataSource()
        {
            DateTime from;
            DateTime to;

            bool isDaily = true;
            if (ddlMonths.SelectedIndex > 0)
            {
                int year;
                if (string.IsNullOrEmpty(txtYear.Text))
                {
                    year = DateTime.Now.Year;
                }
                else
                {
                    year = Convert.ToInt32(txtYear.Text);
                }
                from = new DateTime(year, Convert.ToInt32(ddlMonths.SelectedValue), 1);
                to = from.AddMonths(1).AddDays(-1);
            }
            else
            {
                if (string.IsNullOrEmpty(txtYear.Text))
                {
                    from = DateTime.Today.AddDays(-DateTime.Today.Day + 1);
                    to = from.AddMonths(1).AddDays(-1);
                }
                else
                {
                    from = new DateTime(Convert.ToInt32(txtYear.Text), 1, 1); // Lấy ngày from là ngày 1/1/xxxx
                    to = from.AddYears(1).AddDays(-1);
                    isDaily = false;
                }
            }

            if (isDaily)
            {
                ddlMonths.SelectedValue = from.Month.ToString();
            }
            else
            {
                ddlMonths.SelectedIndex = 0;
            }
            txtYear.Text = to.Year.ToString();

            IList list = new ArrayList();
            if (isDaily)
            {
                list = Module.ExpenseGetByDate(ActiveCruise, from, to);
            }
            else
            {
                while (from <= to)
                {
                    list.Add(from.Month);
                    from = from.AddMonths(1);
                }
            }

            _currentRate = Module.ExchangeGetByDate(DateTime.Now);
            rptBalance.DataSource = list;
            rptBalance.DataBind();
        }

        protected void rptCruises_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is SailsTrip)
            {
                SailsTrip trip = (SailsTrip)e.Item.DataItem;
                HtmlGenericControl liMenu = e.Item.FindControl("liMenu") as HtmlGenericControl;
                if (liMenu != null)
                {
                    if (trip.Id.ToString() == Request.QueryString["cruiseid"])
                    {
                        liMenu.Attributes.Add("class", "selected");
                    }
                }

                HyperLink hplCruises = e.Item.FindControl("hplCruises") as HyperLink;
                if (hplCruises != null)
                {
                    hplCruises.Text = trip.Name;
                    hplCruises.NavigateUrl = string.Format(
                        "BalanceReport.aspx?NodeId={0}&SectionId={1}&cruiseid={2}&month={3}&year={4}", Node.Id,
                        Section.Id,
                        trip.Id, ddlMonths.SelectedIndex, txtYear.Text);
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
                    hplCruises.NavigateUrl = string.Format(
                        "BalanceReport.aspx?NodeId={0}&SectionId={1}&month={2}&year={3}", Node.Id, Section.Id,
                        ddlMonths.SelectedIndex, txtYear.Text);
                }
            }
        }
    }
}