using System;
using System.Collections;
using System.Collections.Generic;
using CMS.Core.Domain;
using NHibernate.Criterion;
using Portal.Modules.OrientalSails.Web.Util;

namespace Portal.Modules.OrientalSails.Domain
{
    #region Booking

    /// <summary>
    /// Booking object for NHibernate mapped table 'os_Booking'.
    /// </summary>
    public class SailExpense
    {
        #region Static Columns Name

        public const string DATE = "Date";
        public const string TRANSFER = "Transfer";
        public const string TICKET = "Ticket";
        public const string GUIDE = "Guide";
        public const string MEAL = "Meal";
        public const string KAYAING = "Kayaing";
        public const string SERVIVE = "Service";
        public const string CRUISE = "Cruise";
        public const string OTHERS = "Others";
        public const string TOTAL = "Total";

        #endregion

        #region Member Variables

        protected int _id;
        protected DateTime _date;
        protected SailExpensePayment _payment;
        protected IList _guides;
        protected IList _transfers;
        protected IList _operators;
        protected IList _services;

        protected bool _lockIncome;
        protected bool _lockOutcome;

        protected string _operatorName;
        protected string _operatorPhone;
        protected User _saleInCharge;

        protected SailsTrip _trip;

        #endregion

        #region Constructors

        public SailExpense()
        {
            _id = -1;
        }

        #endregion

        #region Public Properties

        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        public virtual SailsTrip Trip
        {
            get { return _trip; }
            set { _trip = value; }
        }

        public virtual SailExpensePayment Payment
        {
            get { return _payment; }
            set { _payment = value; }
        }

        /// <summary>
        /// Toàn bộ các chi phí tính theo expense này
        /// </summary>
        public virtual IList Services
        {
            get
            {
                if (_services == null)
                {
                    _services = new ArrayList();
                }
                return _services;
            }
            set
            {
                _services = value;
            }
        }

        public virtual bool LockIncome
        {
            get { return _lockIncome; }
            set { _lockIncome = value; }
        }

        public virtual bool LockOutcome
        {
            get { return _lockIncome; }
            set { _lockIncome = value; }
        }

        public virtual string OperatorName
        {
            get { return _operatorName; }
            set { _operatorName = value; }
        }

        /// <summary>
        /// Người điều hành tour cho ngày
        /// </summary>
        public virtual User Operator { get; set; }

        public virtual string OperatorPhone
        {
            get { return _operatorPhone; }
            set { _operatorPhone = value; }
        }

        public virtual User SaleInCharge
        {
            get { return _saleInCharge; }
            set { _saleInCharge = value; }
        }

        private int _numberOfGroup;
        /// <summary>
        /// Số nhóm trong ngày, tối thiểu là 1
        /// </summary>
        public virtual int NumberOfGroup
        {
            get
            {
                if (_numberOfGroup <= 0)
                {
                    _numberOfGroup = 1;
                }
                return _numberOfGroup;
            }
            set { _numberOfGroup = value; }
        }

        /// <summary>
        /// Đã bao giờ được lưu ở booking by date chưa
        /// </summary>
        public virtual bool IsEvent { get; set; }

        #endregion

        #region -- Calculated properties --
        /// <summary>
        /// Tính toán chi phí đưa về dạng từ điển: loại chi phí - chi phí
        /// </summary>
        /// <param name="costTypes">Toàn bộ chi phí tính được theo ngày</param>
        /// <param name="costTable">Hàm lấy bảng giá theo khách theo thời điểm xác định</param>
        /// <param name="dailyTable">Hàm lấy bảng giá theo chuyến</param>
        /// <param name="getCruiseTable">Bảng giá thuê tàu</param>
        /// <param name="activecruise">Tàu hiện tại</param>
        /// <param name="bookings">Danh sách booking có trong ngày</param>
        /// <param name="module">SailsModule (dùng để tác động tới CSDL)</param>
        /// <param name="partnership">Có quản lý đối tác sử dụng chi phí không</param>
        /// <returns></returns>
        public virtual Dictionary<CostType, double> Calculate(IList costTypes, GetCurrentCostTable costTable, GetCurrentDailyCostTable dailyTable, GetCurrentCruiseExpenseTable getCruiseTable, SailsTrip activecruise, IList bookings, SailsModule module, bool partnership, Organization org = null)
        {
            _trip = activecruise;
            // Nếu là chi phí từng tàu, tính chi phí cho tàu đó
            // Nếu là chi phí tổng, tính chi phí từng tàu rồi sau đó cộng lại

            #region -- Chi phí cho một hành trình --
            if (_trip != null)
            {
                // Dựng bảng dịch vụ trắng: mỗi loại dịch vụ phát sinh một chi phí duy nhất
                var serviceMap = new Dictionary<CostType, ExpenseService>();
                var serviceTotal = new Dictionary<CostType, double>();
                foreach (CostType type in costTypes)
                {
                    serviceMap.Add(type, null);
                    serviceTotal.Add(type, 0);
                }

                #region -- Tạo bảng giá trắng và lấy giá nhập thủ công theo thuyến --
                // Kiểm tra xem đã có giá các dịch vụ nào
                foreach (ExpenseService service in Services)
                {
                    serviceMap[service.Type] = service;
                    // Nếu không thuộc diện tính chi phí cho ngày (không nằm trong danh sách chi phí)
                    //TODO: Kiểm tra lại có cần điều kiện sau không?
                    if (!serviceMap.ContainsKey(service.Type)) //|| !Trip.CostTypes.Contains(service.Type))
                    {
                        continue;
                    }


                    // Nếu là giá nhập thủ công thì cộng luôn
                    if (service.Type.IsDailyInput)
                    {
                        serviceTotal[service.Type] += service.Cost;
                    }
                }
                #endregion

                int adultHaiPhong = 0;
                int childHaiPhong = 0;

                // Tính giá từng dịch vụ với từng booking (chi phí theo số khách )
                #region -- Dịch vụ theo booking (chi phí theo số khách) - chỉ dùng để tính vào tổng chi phí không lưu vào CSDL --
                foreach (Booking booking in bookings)
                {
                    Dictionary<CostType, double> bookingCost;
                    try
                    {
                        bookingCost = booking.Cost(costTable(Date, booking.Trip, booking.TripOption), costTypes);
                    }
                    catch (Exception ex)
                    {
                        break;
                    }

                    // Sau khi có bảng giá từng booking thì cộng vào tổng
                    foreach (CostType type in costTypes)
                    {
                        serviceTotal[type] += bookingCost[type];
                    }

                    // Đồng thời tính số người để tính giá thuê tàu Hải Phong luôn
                    adultHaiPhong += booking.Adult;
                    childHaiPhong += booking.Child;
                }
                #endregion

                // Bắt đầu tính chi phí từ tháng 7, còn trước tháng 7 không sử dụng biện pháp này
                if (Date >= new DateTime(2013, 7, 1))
                //if (true) //TODO: chuyển sang điều kiện datetime, đang debug
                {
                    #region -- Dịch vụ theo booking (chi phí theo số khách) chia nhỏ theo group --

                    int numberOfGroup = NumberOfGroup;
                    for (int ii = 1; ii <= numberOfGroup; ii++)
                    {
                        var serviceGroup = new Dictionary<CostType, double>();
                        foreach (CostType type in costTypes)
                        {
                            serviceGroup.Add(type, 0);
                        }

                        foreach (Booking booking in bookings)
                        {
                            if (booking.Group == ii)
                            {
                                Dictionary<CostType, double> bookingCost;
                                try
                                {
                                    bookingCost = booking.Cost(costTable(Date, booking.Trip, booking.TripOption),
                                                               costTypes);
                                }
                                catch (Exception ex)
                                {
                                    continue;
                                }
                                // Sau khi có bảng giá từng booking thì cộng vào tổng
                                foreach (CostType type in costTypes)
                                {
                                    serviceGroup[type] += bookingCost[type];
                                }
                            }
                        }

                        // Sau khi có chi phí tổng rồi check CSDL
                        foreach (CostType type in costTypes)
                        {
                            // Bỏ qua dịch vụ theo ngày vì đã lưu theo từng dịch vụ riêng rẽ
                            if (type.IsDailyInput)
                            {
                                continue;
                            }

                            #region -- Check trong cơ sở dữ liệu --

                            bool found = false;
                            foreach (ExpenseService service in Services)
                            {
                                if (service.Type.Id == type.Id && service.Group == ii)
                                {
                                    // Đã có dịch vụ, nếu chi phí không bằng thực tính thì lưu lại
                                    if (service.Cost != serviceGroup[type])
                                    {
                                        service.Cost = serviceGroup[type];
                                        module.SaveOrUpdate(service);
                                    }
                                    // Đã thấy service thì có thể break
                                    found = true;
                                    break;
                                }
                            }

                            if (!found && serviceGroup[type] > 0) // Nếu chưa có và phát sinh chi phí thì tạo mới
                            {
                                if (type.DefaultAgency == null && partnership)
                                {
                                    throw new Exception("You must config default agency for " + type.Name);
                                }
                                var service = new ExpenseService();
                                service.Expense = this;
                                service.Cost = serviceGroup[type];
                                service.Name = string.Format("{0:dd/MM/yyyy}- {1}", Date, type.Name);
                                service.Paid = 0;
                                service.Supplier = type.DefaultAgency;
                                service.Group = ii;
                                if (service.Supplier != null)
                                {
                                    service.Phone = type.DefaultAgency.Phone;
                                }
                                service.Type = type;
                                module.SaveOrUpdate(service);
                            }

                            #endregion
                        }
                    }

                    #endregion
                }

                #region -- Chi phí theo chuyến: bỏ qua không sử dụng --
                //bool _isRun = bookings.Count > 0; // Nếu có booking thì tính chi phí theo chuyến (tàu có chạy)

                //if (_isRun)
                //{
                //    DailyCostTable table = dailyTable(Date);
                //    if (table != null)
                //    {
                //        foreach (DailyCost cost in dailyTable(Date).Costs)
                //        {
                //            if (serviceTotal.ContainsKey(cost.Type))
                //            {
                //                serviceTotal[cost.Type] += cost.Cost; // Luôn cộng luôn chi phí vào tổng
                //            }
                //        }
                //    }
                //}

                #endregion

                #region -- Giá tàu Hải Phong --

                // Chỉ tính giá tàu Hải Phong nếu đây là bảng chi phí cho một tàu
                CruiseExpenseTable cruiseTable = getCruiseTable(_date, _trip);
                CalculateCruiseExpense(costTypes, serviceTotal, adultHaiPhong, childHaiPhong, cruiseTable);

                #endregion

                #region -- Trước khi trả về kết quả, kiểm tra cơ sở dữ liệu --
                foreach (CostType type in costTypes)
                {
                    // Bỏ qua dịch vụ theo ngày vì đã lưu theo từng dịch vụ riêng rẽ
                    if (type.IsDailyInput)
                    {
                        continue;
                    }

                    #region -- Bỏ qua không lưu chi phí phát sinh theo số khách ở đây --
                    //if (serviceMap[type] != null)
                    //{
                    //    // Nếu giá dịch vụ trong CSDL không bằng thực tính
                    //    if (serviceMap[type].Cost != serviceTotal[type])
                    //    {
                    //        serviceMap[type].Cost = serviceTotal[type];
                    //        module.SaveOrUpdate(serviceMap[type]);
                    //    }
                    //    // Ngược lại thì bỏ qua
                    //}
                    //else
                    //{
                    //    // Nếu chưa có thì cập nhật mới: đối với chi phí tính theo đầu người là chủ yếu
                    //    if (type.DefaultAgency == null && partnership)
                    //    {
                    //        throw new Exception("You must config default agency for " + type.Name);
                    //    }
                    //    var service = new ExpenseService();
                    //    service.Expense = this;
                    //    service.Cost = serviceTotal[type];
                    //    service.Name = string.Format("{0:dd/MM/yyyy}- {1}", Date, type.Name);
                    //    service.Paid = 0;
                    //    service.Supplier = type.DefaultAgency;
                    //    if (service.Supplier != null)
                    //    {
                    //        service.Phone = type.DefaultAgency.Phone;
                    //    }
                    //    service.Type = type;
                    //    module.SaveOrUpdate(service);
                    //}
                    #endregion
                }
                #endregion

                return serviceTotal;
            }
            #endregion

            #region -- Chi phí cho tất cả các hành trình trong ngày --
            var total = new Dictionary<CostType, double>();

            #region -- Lấy về chi phí cho từng tàu nếu là chi phí tổng --

            //Chi phí cho từng tàu
            //Dictionary<int, SailExpense> expenseCruise = new Dictionary<int, SailExpense>();
            IList trips;
            if (org != null)
            {
                trips = module.TripGetByOrganization(org);
            }
            else
            {
                trips = module.TripGetAll(false, null);
            }

            #region -- Tạo bảng giá trắng --
            foreach (CostType type in costTypes)
            {
                total.Add(type, 0);
            }
            #endregion

            foreach (SailsTrip trip in trips)
            {
                SailExpense expense = module.ExpenseGetByDate(trip, _date);
                if (expense.Id < 0) module.SaveOrUpdate(expense);

                IList filtered = new ArrayList(); // Lọc các booking theo hành trình xác định
                foreach (Booking booking in bookings)
                {
                    if (booking.Trip != null && booking.Trip.Id == trip.Id)
                    {
                        filtered.Add(booking);
                    }
                }

                Dictionary<CostType, double> expenses = expense.Calculate(costTypes, costTable, dailyTable,
                                                                          getCruiseTable, trip, filtered, module,
                                                                          partnership);
                foreach (CostType type in costTypes)
                {
                    total[type] += expenses[type];
                }
            }

            #endregion

            return total;
            #endregion
        }

        public virtual void CalculateCruiseExpense(IList costTypes, IDictionary<CostType, double> serviceTotal, int adultHaiPhong, int childHaiPhong, CruiseExpenseTable cruiseTable)
        {
            CostType cruise = null;
            foreach (CostType type in costTypes)
            {
                if (type.Id == SailsModule.HAIPHONG)
                {
                    cruise = type;
                    break;
                }
            }

            // Nếu tồn tại loại chi phí có ID giống như cấu hình cứng tại SailsModule.HAIPHONG
            if (cruise != null)
            {
                // Tính số khách dùng để tính giá
                double haiphong = adultHaiPhong + childHaiPhong / 2;

                if (haiphong > 0)
                {
                    #region Dò tìm dòng giá tàu Hải Phong phù hợp với số khách
                    int index = -1;
                    for (int ii = 0; ii < cruiseTable.Expenses.Count; ii++)
                    {
                        CruiseExpense cExpense = (CruiseExpense)cruiseTable.Expenses[ii];
                        if (cExpense.CustomerFrom >= haiphong)
                        {
                            index = ii;
                            break;
                        }
                    }

                    if (index < 0)
                    {
                        throw new Exception("Hai phong cruise price is not valid, can not find price for " +
                                            haiphong +
                                            " persons");
                    }
                    #endregion

                    #region Dựa vào dòng giá, tính chi phí
                    // Nếu đúng bằng, lấy luôn giá này
                    if (((CruiseExpense)cruiseTable.Expenses[index]).CustomerFrom == haiphong)
                    {
                        serviceTotal[cruise] += ((CruiseExpense)cruiseTable.Expenses[index]).Price;
                    }
                    else
                    {
                        if (index < 1)
                        {
                            throw new Exception("Hai phong cruise price is not valid, can not calculate for " +
                                                haiphong +
                                                " persons");
                        }
                        CruiseExpense upperExpense = (CruiseExpense)cruiseTable.Expenses[index];
                        CruiseExpense lowerExpense = (CruiseExpense)cruiseTable.Expenses[index - 1];

                        if (lowerExpense.CustomerTo != upperExpense.CustomerFrom - 1)
                        {
                            throw new Exception("Hai phong cruise price is not valid, price table must be continity");
                        }

                        // Nếu nhỏ hơn thì có 2 trường hợp: nhỏ hơn và nằm trong khoảng nhỏ
                        if (lowerExpense.CustomerTo > haiphong)
                        {
                            // Công thức sai toét, nhưng tạm thời bỏ qua
                            serviceTotal[cruise] += lowerExpense.Price * haiphong /
                                                   (lowerExpense.CustomerTo - lowerExpense.CustomerFrom + 1);
                        }
                        else
                        {
                            serviceTotal[cruise] += lowerExpense.Price +
                                                    (upperExpense.Price - lowerExpense.Price) *
                                                    (haiphong - lowerExpense.CustomerTo);
                        }
                    }
                    #endregion
                }
            }
        }

        public virtual Dictionary<CostType, double> GetPayable(IList costTypes)
        {
            Dictionary<CostType, double> serviceTotal = new Dictionary<CostType, double>();
            foreach (CostType type in costTypes)
            {
                serviceTotal.Add(type, 0);
            }

            foreach (ExpenseService service in Services)
            {
                serviceTotal[service.Type] += service.Cost - service.Paid;
            }

            return serviceTotal;
        }
        #endregion
    }

    public delegate CostingTable GetCurrentCostTable(DateTime date, SailsTrip trip, TripOption option);
    public delegate DailyCostTable GetCurrentDailyCostTable(DateTime date);
    public delegate CruiseExpenseTable GetCurrentCruiseExpenseTable(DateTime date, SailsTrip cruise);

    #endregion
}