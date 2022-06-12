using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using NHibernate.Criterion;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.UI;
using Portal.Modules.OrientalSails.Web.Util;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class ExpenseReport : SailsAdminBasePage
    {
        #region -- PRIVATE MEMBERS --

        private IList _allCostTypes;
        private IList _autoCostTypes;
        private CruiseExpenseTable _cruiseTable;
        private Dictionary<CostType, double> _currentCostMap;
        private double _currentTotal;
        private DailyCostTable _dailyTable;
        private IList _monthlyCost;
        private IList _yearlyCost;

        /// <summary>
        /// Tổng chi phí của cả thời kỳ báo cáo theo từng dịch vụ
        /// </summary>
        private Dictionary<CostType, double> _grandTotal;

        /// <summary>
        /// Tổng chi phí trung bình tháng
        /// </summary>
        private double _month;

        /// <summary>
        /// Tổng chi phí trung bình năm
        /// </summary>
        private double _year;

        private int _pax;
        private CostingTable _table;
        private CostingTable[,] _tableCache;

        /// <summary>
        /// Bảng lưu chi phí tháng theo mốc thời gian và theo tàu
        /// </summary>
        private readonly Dictionary<Cruise, Dictionary<DateTime, double>> _monthExpenseCruise =
            new Dictionary<Cruise, Dictionary<DateTime, double>>();

        /// <summary>
        /// Bảng lưu chi phí năm theo mốc thời gian và theo tàu
        /// </summary>
        private readonly Dictionary<Cruise, Dictionary<DateTime, double>> _yearExpenseCruise =
            new Dictionary<Cruise, Dictionary<DateTime, double>>();

        /// <summary>
        /// Biến lưu bảng chi phí tháng theo mốc thời gian
        /// </summary>
        private readonly Dictionary<DateTime, double> _monthExpense = new Dictionary<DateTime, double>();

        /// <summary>
        /// Biến lưu bảng chi phí tháng theo mốc thời gian
        /// </summary>
        private readonly Dictionary<DateTime, double> _yearExpense = new Dictionary<DateTime, double>();

        protected IList AutoCalTypes
        {
            get
            {
                if (_autoCostTypes == null)
                {
                    _autoCostTypes = Module.CostTypeGetNotInput();
                }
                return _autoCostTypes;
            }
        }

        protected IList AllCostTypes
        {
            get
            {
                if (_allCostTypes == null)
                {
                    _allCostTypes = Module.CostTypeGetDailyCost();
                }
                return _allCostTypes;
            }
        }

        protected IList MonthlyCost
        {
            get
            {
                if (_monthlyCost == null)
                {
                    _monthlyCost = Module.CostTypeGetMonthly();
                }
                return _monthlyCost;
            }
        }

        protected IList YearlyCost
        {
            get
            {
                if (_yearlyCost == null)
                {
                    _yearlyCost = Module.CostTypeGetYearly();
                }
                return _yearlyCost;
            }
        }

        private SailsTrip _cruise;
        protected SailsTrip ActiveTrip
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

        protected void Plus(Dictionary<CostType, double> currentCostMap)
        {
            if (_grandTotal == null)
            {
                _grandTotal = new Dictionary<CostType, double>();
                foreach (CostType type in AllCostTypes)
                {
                    _grandTotal.Add(type, currentCostMap[type]);
                }
            }
            else
            {
                foreach (CostType type in AllCostTypes)
                {
                    _grandTotal[type] += currentCostMap[type];
                }
            }
        }

        #endregion

        #region -- PAGE EVENTS --

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = @"Báo cáo chi phí";
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

                rptOrganization.DataSource = Module.OrganizationGetAllRoot();
                rptOrganization.DataBind();

                BindTrips();
            }
        }

        protected void BindTrips()
        {
            IList cruises;

            if (Request.QueryString["orgid"] != null)
            {
                var org = Module.OrganizationGetById(Convert.ToInt32(Request.QueryString["orgid"]));
                cruises = Module.TripGetByOrganization(org);
            }
            else
            {
                cruises = Module.TripGetAll(false, null);
            }

            //if (cruises.Count == 1)
            //{
            //    if (ActiveTrip == null)
            //    {
            //        SailsTrip cruise = (SailsTrip)cruises[0];
            //        PageRedirect(string.Format("ExpenseReport.aspx?NodeId={0}&SectionId={1}&cruiseid={2}", Node.Id, Section.Id, cruise.Id));
            //    }
            //    else
            //    {
            //        rptCruises.Visible = false;
            //    }
            //}
            //else
            //{
            rptCruises.DataSource = cruises;
            rptCruises.DataBind();
            //}
        }

        #endregion

        #region -- CONTROL EVENTS --

        /// <summary>
        /// Bound từng ngày
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void rptBookingList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Control ctrl = e.Item.FindControl("plhPeriodExpense");
            if (ctrl != null)
            {
                ctrl.Visible = PeriodExpenseAvg;
            }

            if (e.Item.DataItem is SailExpense)
            {
                #region -- Tính toán chi phí & hiển thị --

                SailExpense expense = ExpenseCalculate(e);

                Plus(_currentCostMap); // Cộng bảng chi phí

                #region -- Show info --
                Repeater rptServices = (Repeater)e.Item.FindControl("rptServices");
                rptServices.DataSource = AllCostTypes;
                rptServices.DataBind();

                Literal litTotal = e.Item.FindControl("litTotal") as Literal;
                if (litTotal != null)
                {
                    litTotal.Text = _currentTotal.ToString("#,0.#");
                }

                HyperLink hplDate = e.Item.FindControl("hplDate") as HyperLink;
                if (hplDate != null)
                {
                    hplDate.Text = expense.Date.ToString("dd/MM/yyyy");
                }

                #endregion

                return;

                #endregion
            }

            #region -- Header --
            if (e.Item.ItemType == ListItemType.Header)
            {
                Repeater rptServices = (Repeater)e.Item.FindControl("rptServices");
                rptServices.DataSource = AllCostTypes;
                rptServices.DataBind();
            }
            #endregion

            #region -- Footer --
            if (e.Item.ItemType == ListItemType.Footer)
            {
                Repeater rptServices = (Repeater)e.Item.FindControl("rptServices");
                rptServices.DataSource = AllCostTypes;
                rptServices.DataBind();

                double total = 0;
                foreach (CostType type in AllCostTypes)
                {
                    total += _grandTotal[type];
                }

                total += _month;
                total += _year;

                Literal litMonth = e.Item.FindControl("litMonth") as Literal;
                if (litMonth != null)
                {
                    litMonth.Text = _month.ToString("#,0.#");
                }

                Literal litYear = e.Item.FindControl("litYear") as Literal;
                if (litYear != null)
                {
                    litYear.Text = _year.ToString("#,0.#");
                }

                Literal litTotal = e.Item.FindControl("litTotal") as Literal;
                if (litTotal != null)
                {
                    litTotal.Text = total.ToString("#,0.#");
                }
            }
            #endregion
        }

        protected void rptOrganization_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is Organization)
            {
                var organization = (Organization)e.Item.DataItem;
                var hplOrganization = e.Item.FindControl("hplOrganization") as HyperLink;
                if (hplOrganization != null)
                {
                    if (organization.Id.ToString() == Request.QueryString["orgid"])
                    {
                        hplOrganization.CssClass += " selected";
                    }
                }

                if (hplOrganization != null)
                {
                    //DateTime from = DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    hplOrganization.Text = organization.Name;
                    if (Request.QueryString["from"] != null)
                    {
                        hplOrganization.NavigateUrl = string.Format(
                            "ExpenseReport.aspx?NodeId={0}&SectionId={1}&from={2}&to={4}&orgid={3}", Node.Id, Section.Id,
                            Request.QueryString["from"], organization.Id, Request.QueryString["to"]);
                    }
                    else
                    {
                        hplOrganization.NavigateUrl = string.Format(
                            "ExpenseReport.aspx?NodeId={0}&SectionId={1}&orgid={2}", Node.Id, Section.Id, organization.Id);
                    }
                }

                var rptTrips = e.Item.FindControl("rptTrips") as Repeater;
                if (rptTrips != null)
                {
                    rptTrips.DataSource = Module.TripGetByOrganization(organization);
                    rptTrips.DataBind();
                }
            }
            else
            {
                var hplOrganization = e.Item.FindControl("hplOrganization") as HyperLink;
                if (hplOrganization != null)
                {
                    if (Request.QueryString["orgid"] == null && Request.QueryString["tripid"] == null)
                    {
                        hplOrganization.CssClass += " selected";
                    }
                }

                if (hplOrganization != null)
                {
                    //DateTime date = DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    if (Request.QueryString["from"] != null)
                    {
                        hplOrganization.NavigateUrl = string.Format(
                            "ExpenseReport.aspx?NodeId={0}&SectionId={1}&from={2}&to={3}", Node.Id, Section.Id,
                            Request.QueryString["from"], Request.QueryString["to"]);
                    }
                    else
                    {
                        hplOrganization.NavigateUrl = string.Format(
                            "ExpenseReport.aspx?NodeId={0}&SectionId={1}", Node.Id, Section.Id);
                    }
                }
            }
        }

        protected virtual void rptServices_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is CostType)
            {
                CostType type = (CostType)e.Item.DataItem;
                Literal litCost = e.Item.FindControl("litCost") as Literal;
                if (litCost != null)
                {
                    litCost.Text = _currentCostMap[type].ToString("#,0.#");
                    _currentTotal += _currentCostMap[type];
                }
            }
        }

        protected virtual void rptServiceTotal_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is CostType)
            {
                CostType type = (CostType)e.Item.DataItem;
                Literal litCost = e.Item.FindControl("litCost") as Literal;
                if (litCost != null)
                {
                    litCost.Text = _grandTotal[type].ToString("#,0.#");
                }
            }
        }

        protected void rptBookingList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
        }

        protected void btnDisplay_Click(object sender, EventArgs e)
        {
            DateTime from = DateTime.ParseExact(txtFrom.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime to = DateTime.ParseExact(txtTo.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            PageRedirect(string.Format("ExpenseReport.aspx?NodeId={0}&SectionId={1}&from={2}&to={3}", Node.Id, Section.Id, from.ToOADate(), to.ToOADate()));
        }

        protected void rptCruises_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is SailsTrip)
            {
                SailsTrip cruise = (SailsTrip)e.Item.DataItem;
                HyperLink hplCruises = e.Item.FindControl("hplCruises") as HyperLink;
                if (hplCruises != null)
                {
                    if (cruise.Id.ToString() == Request.QueryString["cruiseid"])
                    {
                        hplCruises.CssClass += " selected";
                    }
                }

                if (hplCruises != null)
                {
                    DateTime from = DateTime.ParseExact(txtFrom.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime to = DateTime.ParseExact(txtTo.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    hplCruises.Text = cruise.Name;
                    hplCruises.NavigateUrl =
                        string.Format("ExpenseReport.aspx?NodeId={0}&SectionId={1}&from={2}&to={3}&cruiseid={4}", Node.Id, Section.Id, from.ToOADate(), to.ToOADate(), cruise.Id);
                }
            }
            else
            {
                HyperLink hplCruises = e.Item.FindControl("hplCruises") as HyperLink;
                if (hplCruises != null)
                {
                    if (Request.QueryString["cruiseid"] == null)
                    {
                        hplCruises.CssClass += " selected";
                    }
                }

                if (hplCruises != null)
                {
                    DateTime from = DateTime.ParseExact(txtFrom.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime to = DateTime.ParseExact(txtTo.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    hplCruises.NavigateUrl =
                        string.Format("ExpenseReport.aspx?NodeId={0}&SectionId={1}&from={2}&to={3}", Node.Id,
                                      Section.Id, from.ToOADate(), to.ToOADate());
                }
            }
        }

        #endregion

        #region -- METHODS --

        protected void GetDataSource()
        {
            try
            {
                // Ngày bắt đầu và ngày két thúc
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
                if (to.Subtract(from).Days > 100)
                {
                    throw new Exception("Kỳ hạn hiển thị quá dài, có thể gây quá tải hệ thống");
                }
                IList list = Module.ExpenseGetByDate(ActiveTrip, from, to);

                //if (Request.QueryString["orgid"] != null)
                //{
                //    int orgid = Convert.ToInt32(Request.QueryString["orgid"]);
                //    list = new ArrayList();
                //    foreach (SailExpense exp in temp)
                //    {
                //        if (exp.Trip.OrgId == orgid)
                //        {
                //            list.Add(exp);
                //        }
                //    }
                //}
                //else
                //{
                //    list = temp;
                //}

                // Bổ sung các ngày còn thiếu
                if (from <= to && (list.Count == 0 || list.Count < to.Subtract(from).Days + 1))
                {
                    IDictionary<DateTime, SailExpense> dic = new Dictionary<DateTime, SailExpense>();
                    foreach (SailExpense expense in list)
                    {
                        if (!dic.ContainsKey(expense.Date))
                        {
                            dic.Add(expense.Date, expense);
                        }
                    }
                    DateTime current;
                    current = from; // Bắt đầu từ ngày đầu tiên
                    while (current <= to)
                    {
                        if (!dic.ContainsKey(current))
                        {
                            SailExpense expense = new SailExpense();
                            expense.Date = current;
                            expense.Trip = ActiveTrip;
                            Module.SaveOrUpdate(expense);
                        }
                        current = current.AddDays(1);
                    }

                    //Cuối cùng refresh lại danh sách là xong
                    list = Module.ExpenseGetByDate(ActiveTrip, from, to);
                }
                rptBookingList.DataSource = list;
                rptBookingList.DataBind();
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                ShowError(ex.Message);
            }
        }

        protected CostingTable GetCurrentTable(DateTime date, SailsTrip trip, TripOption option)
        {
            #region -- costing table --

            if (_tableCache == null)
            {
                // Lấy về mảng costing table
                int trips = Module.TripMaxId() + 1;
                const int options = 3;
                _tableCache = new CostingTable[trips, options];
            }

            // Nếu bảng giá tại vị trí này là null hoặc hết hạn
            if (_tableCache[trip.Id, (int)option] == null || _tableCache[trip.Id, (int)option].ValidTo < date)
            {
                _tableCache[trip.Id, (int)option] = Module.CostingTableGetValid(date, trip, option);
            }

            _table = _tableCache[trip.Id, (int)option];

            #endregion

            if (_table == null)
            {
                throw new Exception(string.Format("No costing table for {0:dd/MM/yyyy}, {1} {2}", date, trip.Name,
                                                  option));
            }

            return _table;
        }

        // Lấy bảng giá tại thời điểm
        protected DailyCostTable GetCurrentDailyTable(DateTime date)
        {
            if (_dailyTable == null || _dailyTable.ValidTo < date)
            {
                _dailyTable = Module.DailyCostTableGetValid(date);
            }

            if (_dailyTable == null && Module.HasRunningCost)
            {
                throw new Exception(string.Format("Không có bảng giá chuyến cho {0:dd/MM/yyyy}", date));
            }

            return _dailyTable;
        }

        /// <summary>
        /// Lấy báng giá tàu Hải Phong tại thời điểm
        /// </summary>
        /// <param name="date"></param>
        /// <param name="cruise"></param>
        protected CruiseExpenseTable GetCurrentCruiseTable(DateTime date, SailsTrip cruise)
        {
            #region -- cruise table --

            bool isNeedNewTable = false;
            if (_cruiseTable != null)
            {
                //if (_cruiseTable.ValidFrom > date || _cruiseTable.ValidTo < date || _cruiseTable.Cruise != cruise)
                //{
                //    isNeedNewTable = true;
                //}
            }
            else
            {
                isNeedNewTable = true;
            }

            if (isNeedNewTable)
            {
                _cruiseTable = Module.CruiseTableGetValid(date, cruise);
            }

            #endregion

            return _cruiseTable;
        }

        /// <summary>
        /// Tính toán chi phí cho repeater item lưu thông tin expense
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private SailExpense ExpenseCalculate(RepeaterItemEventArgs e)
        {
            #region -- Thông tin chung của expense --
            // Lấy về expense tương ứng với ngày hôm đó và danh sách booking tương ứng để tính toán các chi phí cơ sở cần thiết

            var expense = (SailExpense)e.Item.DataItem;
            // Khi tính chi phí thì chỉ tính theo khách đã check-in
            ICriterion criterion = Expression.And(Expression.Eq(Booking.STARTDATE, expense.Date.Date),
                                                  Expression.Eq(Booking.STATUS, StatusType.Approved));
            // Bỏ deleted và cả transfer
            criterion = Expression.And(Expression.Eq("Deleted", false), criterion);
            criterion = Expression.And(Expression.Eq("IsTransferred", false), criterion);

            // Nếu là trang báo cáo chi phí từng trip thì chỉ lấy theo trip đó
            if (ActiveTrip != null)
            {
                criterion = Expression.And(criterion, Expression.Eq("Trip", ActiveTrip));
            }

            // Lấy về toàn bộ booking trong ngày để tính chi phí
            IList bookings =
                Module.BookingGetByCriterion(criterion, null, 0, 0);

            int adult = 0;
            int child = 0;
            //int baby = 0;
            foreach (Booking booking in bookings)
            {
                adult += booking.Adult;
                child += booking.Child;
            }
            _pax += adult + child;

            #endregion

            if (ActiveTrip != null)
            {
                GetCurrentCruiseTable(expense.Date, ActiveTrip);
            }

            _currentTotal = 0; // Tổng cho ngày hiện tại

            //if (_cruiseTable == null && ActiveTrip!=null)
            //{
            //    throw new Exception("Hai phong cruise price table is out of valid");
            //}

            Organization org = null;
            if (Request.QueryString["orgid"] != null)
            {
                org = Module.OrganizationGetById(Convert.ToInt32(Request.QueryString["orgid"]));
            }

            // Lấy về bảng giá đã tính
            try
            {
                _currentCostMap = expense.Calculate(AllCostTypes, GetCurrentTable, GetCurrentDailyTable,
                                                    GetCurrentCruiseTable,
                                                    expense.Trip,
                                                    bookings, Module, PartnershipManager, org);
            }
            catch
            {

            }

            return expense;
        }

        #endregion
    }
}