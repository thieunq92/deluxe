using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using NHibernate.Criterion;
using Portal.Modules.OrientalSails.Domain;

namespace Portal.Modules.OrientalSails.Web.Util
{
    public class ExpenseCalculator
    {
        private readonly SailsModule _module;
        private readonly bool PartnershipManager;

        #region -- FIELDS --

        /// <summary>
        /// Biến lưu bảng chi phí tháng theo mốc thời gian
        /// </summary>
        private readonly Dictionary<DateTime, double> _monthExpense = new Dictionary<DateTime, double>();

        /// <summary>
        /// Bảng lưu chi phí tháng theo mốc thời gian và theo tàu
        /// </summary>
        private readonly Dictionary<Cruise, Dictionary<DateTime, double>> _monthExpenseCruise =
            new Dictionary<Cruise, Dictionary<DateTime, double>>();

        /// <summary>
        /// Biến lưu bảng chi phí tháng theo mốc thời gian
        /// </summary>
        private readonly Dictionary<DateTime, double> _yearExpense = new Dictionary<DateTime, double>();

        /// <summary>
        /// Bảng lưu chi phí năm theo mốc thời gian và theo tàu
        /// </summary>
        private readonly Dictionary<Cruise, Dictionary<DateTime, double>> _yearExpenseCruise =
            new Dictionary<Cruise, Dictionary<DateTime, double>>();

        private CruiseExpenseTable _cruiseTable;
        private Dictionary<CostType, double> _currentCostMap;
        private double _currentTotal;
        private DailyCostTable _dailyTable;

        /// <summary>
        /// Tổng chi phí trung bình tháng
        /// </summary>
        private double _month;

        private int _pax;
        private CostingTable _table;
        private CostingTable[,] _tableCache;

        /// <summary>
        /// Tổng chi phí trung bình năm
        /// </summary>
        private double _year;

        #endregion

        #region -- JIT FIELDS --

        private IList _allCostTypes;

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

        #endregion

        public ExpenseCalculator(SailsModule module, bool IsPartnershipManager)
        {
            _module = module;
            PartnershipManager = IsPartnershipManager;
        }

        public SailsModule Module
        {
            get { return _module; }
        }

        /// <summary>
        /// Tính toán chi phí cho repeater item (mỗi item là một expense); lưu thông tin expense
        /// </summary>
        /// <param name="e"></param>
        /// <param name="expenses"></param>
        /// <returns></returns>
        public Dictionary<CostType, double> ExpenseCalculate(RepeaterItemEventArgs e, SailExpense expenses)
        {
            #region -- Thông tin chung của expense, lấy danh sách booking theo expense và tính số khách --

            SailExpense expense;
            if (expenses == null)
            {
                expense = (SailExpense) e.Item.DataItem;
            }
            else
            {
                expense = expenses;
            }

            // Khi tính chi phí thì chỉ tính theo khách đã check-in
            ICriterion criterion = Expression.And(Expression.Eq(Booking.STARTDATE, expense.Date.Date),
                                                  Expression.Eq(Booking.STATUS, StatusType.Approved));
            // Bỏ deleted và cả transfer
            criterion = Expression.And(Expression.Eq("Deleted", false), criterion);
            criterion = Expression.And(Expression.Eq("IsTransferred", false), criterion);

            // Nếu là trang báo cáo chi phí từng tàu thì chỉ lấy theo tàu đó
            if (expense.Trip != null)
            {
                criterion = Expression.And(criterion, Expression.Eq("Trip", expense.Trip));
            }

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

            #region -- bỏ lấy bảng chi phí thuê tàu Hải Phong --
            //GetCurrentCruiseTable(expense.Date, expense.Trip);
            #endregion

            _currentTotal = 0; // Tổng cho ngày hiện tại

            #region -- bỏ đoạn tính chi phí tháng và năm --
            if (false)
            {
                if (e != null)
                {
                    #region -- Chi phí tháng --

                    // Nếu là tính chi phí cho một tàu thì chia chi phí bình thường
                    if (expense.Trip != null)
                    {
                        #region -- Một tàu --

                        //// Nếu có chạy hoặc là tháng chưa kết thúc
                        //if (bookings.Count > 0 ||
                        //    expense.Date.Month + expense.Date.Year*12 >= DateTime.Today.Month + DateTime.Today.Year*12)
                        //{
                        //    // Tính chi phí tháng
                        //    Literal litMonth = e.Item.FindControl("litMonth") as Literal;
                        //    if (litMonth != null)
                        //    {
                        //        DateTime dateMonth = new DateTime(expense.Date.Year, expense.Date.Month, 1);
                        //        if (!_monthExpense.ContainsKey(dateMonth))
                        //        {
                        //            int runcount; // Số ngày tàu chạy trong tháng, chỉ phục vụ việc tính chi phí trung bình
                        //            // Không cần tính lại trong mỗi lần lặp
                        //            // Nếu là tháng chưa kết thúc
                        //            if (dateMonth.AddMonths(1) > DateTime.Today)
                        //            {
                        //                runcount = dateMonth.AddMonths(1).Subtract(dateMonth).Days;
                        //            }
                        //            else
                        //            {
                        //                runcount = Module.RunningDayCount(expense.Cruise, expense.Date.Year,
                        //                                                  expense.Date.Month);
                        //            }

                        //            SailExpense monthExpense = Module.ExpenseGetByDate(expense.Cruise, dateMonth);
                        //            if (monthExpense.Id < 0)
                        //            {
                        //                Module.SaveOrUpdate(monthExpense);
                        //            }
                        //            double total = Module.CopyMonthlyCost(monthExpense);
                        //            _monthExpense.Add(dateMonth, total/runcount);
                        //        }

                        //        litMonth.Text = _monthExpense[dateMonth].ToString("#,0.#");
                        //        _month += _monthExpense[dateMonth];
                        //        _currentTotal += _monthExpense[dateMonth];
                        //    }
                        //}

                        #endregion
                    }
                    else // Nếu là tính chi phí tổng hợp thì tính cho tất cả các tàu rồi cộng lại
                    {
                        IList cruises = Module.CruiseGetAll();
                        double monthAll = 0; // tổng chi phí tháng trung bình
                        foreach (Cruise cruise in cruises)
                        {
                            DateTime dateMonth = new DateTime(expense.Date.Year, expense.Date.Month, 1);
                            // Nếu chưa có bảng chi phí cho tàu hiện tại
                            if (!_monthExpenseCruise.ContainsKey(cruise))
                            {
                                _monthExpenseCruise.Add(cruise, new Dictionary<DateTime, double>());
                                // Tạo một từ điển trắng để lưu dữ liệu
                            }

                            // Nếu chưa có chi phí của tàu hiện tại trong tháng hiện tại
                            if (!_monthExpenseCruise[cruise].ContainsKey(dateMonth))
                            {
                                int runcount;
                                // Nếu là tháng chưa kết thúc
                                if (dateMonth.AddMonths(1) > DateTime.Today)
                                {
                                    runcount = dateMonth.AddMonths(1).Subtract(dateMonth).Days;
                                }
                                else
                                {
                                    runcount = Module.RunningDayCount(cruise, expense.Date.Year, expense.Date.Month);
                                }

                                //SailExpense monthExpense = Module.ExpenseGetByDate(cruise, dateMonth);
                                //if (monthExpense.Id < 0)
                                //{
                                //    Module.SaveOrUpdate(monthExpense);
                                //}
                                //double total = Module.CopyMonthlyCost(monthExpense);
                                //_monthExpenseCruise[cruise].Add(dateMonth, total/runcount);
                            }

                            bool isRun = false;
                            if (dateMonth.AddMonths(1) <= DateTime.Today)
                            {
                                foreach (Booking booking in bookings)
                                {
                                    if (booking.Cruise == cruise)
                                    {
                                        isRun = true;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                isRun = true; // Nếu là tháng chưa kết thúc thì mặc định là mọi ngày tàu đều chạy
                            }


                            if (isRun)
                            {
                                monthAll += _monthExpenseCruise[cruise][dateMonth];
                                // cộng thêm chi phí cho tàu này, ngày này khi tàu có chạy
                            }
                        }

                        _month += monthAll;

                        Literal litMonth = e.Item.FindControl("litMonth") as Literal;
                        if (litMonth != null)
                        {
                            litMonth.Text = monthAll.ToString("#,0.#");
                        }
                    }

                    #endregion

                    #region -- Chi phí năm --

                    //if (expense.Cruise != null)
                    //{
                    //    // Nếu có chạy hoặc năm chưa kết thúc
                    //    if (bookings.Count > 0 || expense.Date.Year >= DateTime.Today.Year)
                    //    {
                    //        // Tính chi phí năm
                    //        Literal litYear = e.Item.FindControl("litYear") as Literal;
                    //        if (litYear != null)
                    //        {
                    //            DateTime dateYear = new DateTime(expense.Date.Year, 1, 1);
                    //            int runcount;
                    //            // Nếu là năm chưa kết thúc
                    //            if (dateYear.AddYears(1) > DateTime.Today)
                    //            {
                    //                runcount = dateYear.AddYears(1).Subtract(dateYear).Days;
                    //            }
                    //            else
                    //            {
                    //                //runcount = Module.RunningDayCount(expense.Cruise, expense.Date.Year, 0);
                    //            }
                    //            if (!_yearExpense.ContainsKey(dateYear))
                    //            {
                    //                SailExpense yearExpense = Module.ExpenseGetByDate(expense.Cruise, dateYear);
                    //                if (yearExpense.Id < 0)
                    //                {
                    //                    Module.SaveOrUpdate(yearExpense);
                    //                }
                    //                double total = Module.CopyYearlyCost(yearExpense);
                    //                _yearExpense.Add(dateYear, total/runcount);
                    //            }

                    //            litYear.Text = _yearExpense[dateYear].ToString("#,0.#");
                    //            _year += _yearExpense[dateYear];
                    //            _currentTotal += _yearExpense[dateYear];
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    IList cruises = Module.CruiseGetAll();
                    //    double yearAll = 0; // tổng chi phí tháng trung bình
                    //    foreach (Cruise cruise in cruises)
                    //    {
                    //        DateTime dateYear = new DateTime(expense.Date.Year, 1, 1);

                    //        // Nếu chưa có bảng chi phí cho tàu hiện tại
                    //        if (!_yearExpenseCruise.ContainsKey(cruise))
                    //        {
                    //            _yearExpenseCruise.Add(cruise, new Dictionary<DateTime, double>());
                    //            // Tạo một từ điển trắng để lưu dữ liệu
                    //        }

                    //        // Nếu chưa có chi phí của tàu hiện tại trong năm hiện tại
                    //        if (!_yearExpenseCruise[cruise].ContainsKey(dateYear))
                    //        {
                    //            int runcount;
                    //            // Nếu là năm chưa kết thúc
                    //            if (dateYear.AddYears(1) > DateTime.Today)
                    //            {
                    //                runcount = dateYear.AddYears(1).Subtract(dateYear).Days;
                    //            }
                    //            else
                    //            {
                    //                runcount = Module.RunningDayCount(cruise, expense.Date.Year, 0);
                    //            }

                    //            SailExpense yearExpense = Module.ExpenseGetByDate(cruise, dateYear);
                    //            if (yearExpense.Id < 0)
                    //            {
                    //                Module.SaveOrUpdate(yearExpense);
                    //            }
                    //            double total = Module.CopyYearlyCost(yearExpense);

                    //            _yearExpenseCruise[cruise].Add(dateYear, total/runcount);
                    //        }

                    //        bool isRun = false;
                    //        if (dateYear.AddYears(1) <= DateTime.Today)
                    //        {
                    //            foreach (Booking booking in bookings)
                    //            {
                    //                if (booking.Cruise == cruise)
                    //                {
                    //                    isRun = true;
                    //                    break;
                    //                }
                    //            }
                    //        }
                    //        else
                    //        {
                    //            isRun = true; // Nếu là năm chưa kết thúc thì mặc định là mọi ngày tàu đều chạy
                    //        }


                    //        if (isRun)
                    //        {
                    //            yearAll += _yearExpenseCruise[cruise][dateYear];
                    //            // cộng thêm chi phí cho tàu này, ngày này khi tàu có chạy
                    //        }
                    //    }

                    //    _year += yearAll;

                    //    Literal litYear = e.Item.FindControl("litYear") as Literal;
                    //    if (litYear != null)
                    //    {
                    //        litYear.Text = yearAll.ToString("#,0.#");
                    //    }
                    //}

                    #endregion
                }
            }
            #endregion

            #region -- bỏ chi phí thuê tàu Hải Phong --
            if (_cruiseTable == null)
            {
                //throw new Exception("Hai phong cruise price table is out of valid");
            }
            #endregion

            try
            {
                // Lấy về bảng giá đã tính
                _currentCostMap = expense.Calculate(AllCostTypes, GetCurrentTable, GetCurrentDailyTable, GetCurrentCruiseTable,
                                                    expense.Trip,
                                                    bookings, Module, PartnershipManager);
            }
            catch
            {
                
            }            
            return _currentCostMap;
        }

        protected CruiseExpenseTable GetCurrentCruiseTable(DateTime date, SailsTrip cruise)
        {
            #region -- cruise table --

            bool isNeedNewTable = false;
            if (_cruiseTable != null)
            {
                if (_cruiseTable.ValidFrom > date || _cruiseTable.ValidTo < date)
                {
                    isNeedNewTable = true;
                }
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

        public CostingTable GetCurrentTable(DateTime date, SailsTrip trip, TripOption option)
        {
            #region -- costing table --

            if (_tableCache == null)
            {
                // Lấy về mảng costing table
                int trips = Module.TripMaxId() + 1;
                const int options = 3;
                _tableCache = new CostingTable[trips,options];
            }

            // Nếu bảng giá tại vị trí này là null hoặc hết hạn
            if (_tableCache[trip.Id, (int) option] == null || _tableCache[trip.Id, (int) option].ValidTo < date)
            {
                _tableCache[trip.Id, (int) option] = Module.CostingTableGetValid(date, trip, option);
            }

            _table = _tableCache[trip.Id, (int) option];

            #endregion

            if (_table == null)
            {
                throw new Exception(string.Format("No costing table for {0:dd/MM/yyyy}, {1} {2}", date, trip.Name,
                                                  option));
            }

            return _table;
        }

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
    }
}