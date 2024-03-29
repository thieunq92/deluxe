using System;
using System.Collections;
using System.Collections.Generic;
using CMS.Core.Domain;
using Portal.Modules.OrientalSails.Web.Util;

namespace Portal.Modules.OrientalSails.Domain
{
    public class Booking
    {

        public static string CONFIRMEDBY = "ConfirmedBy";
        public static string CREATEDBY = "CreatedBy";
        public static string CREATEDDATE = "CreatedDate";
        public static string DELETED = "Deleted";
        public static string MODIFIEDBY = "ModifiedBy";
        public static string MODIFIEDDATE = "ModifiedDate";
        public static string PARTNERID = "PảPartnerId";
        public static string SALEID = "SaleId";
        public static string STARTDATE = "StartDate";
        public static string STATUS = "Status";
        public static string TRIP = "Trip";
        public static string TRIPOPION = "TripOpion";
        public static string TOTAL = "Total";
        public static string PAID = "Paid";

        protected IList _bookosBookingRooms;
        protected User _confirmedBy;
        protected User _createdBy;
        protected DateTime _createdDate;
        protected bool _deleted;
        protected int _id;
        protected User _modifiedBy;
        protected DateTime _modifiedDate;
        protected User _partnerId;
        protected User _saleId;
        protected DateTime _startDate;
        protected StatusType _status;
        protected SailsTrip _trip;
        protected Cruise _cruise;
        protected TripOption _tripOpion;
        protected IList _customers;
        protected IList _extraServices;
        protected double _total;
        protected double _totalVND;
        protected double _paid;
        protected string _email;
        protected string _name;
        protected string _note;
        protected Agency _agency;
        protected string _pickupAddress;
        protected string _specialRequest;
        protected string _agecyCode;
        protected AgencyContact _booker;
        protected bool _isApproved;
        protected AccountingStatus _accountingStatus;
        protected int _amended;
        protected bool _isCharter;
        protected string _dropoffAddress;
        protected bool _hasInvoice;
        protected double _cancelPay;
        protected double _cancelPayVND;

        protected string _guide;
        protected string _driver;
        protected bool _guideOnboard;

        protected User _locker;

        protected IList _services;

        protected bool _isTransferred;
        protected double _transferCost;
        protected Agency _transferTo;
        protected int _transferAdult;
        protected int _transferChild;
        protected int _transferBaby;

        protected double _currencyRate;
        protected bool _isPaid;
        protected double _paidBase;
        protected bool _isPaymentNeeded;


        protected string _bookingId;
        protected string _customBookingCode;
        protected int _customBookingId;

        protected DateTime _endDate;

        protected Locked _lock;

        protected double _commission;
        protected double _commissionVND;

        protected double _comUSDpayed;
        protected double _comVNDpayed;
        protected double _comRate;
        protected bool _comPaid;

        protected int _adult;
        protected int _child;
        protected int _baby;
        protected bool _counted;

        protected int _double;
        protected int _twin;
        protected bool _roomCounted;

        public Booking()
        {
            _id = -1;
            DiscountCurrencyType = "VND";
            DiscountAmount = 0.0;
        }

        public Booking(bool deleted, User createdBy, DateTime createdDate, User modifiedBy, DateTime modifiedDate,
                       User partnerId, User saleId, DateTime startDate, StatusType status, TripOption tripOpion,
                       SailsTrip trip)
        {
            _deleted = deleted;
            _createdBy = createdBy;
            _createdDate = createdDate;
            _modifiedBy = modifiedBy;
            _modifiedDate = modifiedDate;
            _partnerId = partnerId;
            _saleId = saleId;
            _startDate = startDate;
            _status = status;
            _tripOpion = tripOpion;
            _trip = trip;
        }

        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual string BookingIdOS
        {
            get { return string.Format("ATM{0:00000} - {1}", _id, _trip.TripCode); }
        }

        public virtual string BookingCode
        {
            get { return string.Format("ATM{0:00000}", _id); }
        }
        public virtual bool Deleted
        {
            get { return _deleted; }
            set { _deleted = value; }
        }

        public virtual User CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; }
        }

        public virtual DateTime CreatedDate
        {
            get { return _createdDate; }
            set { _createdDate = value; }
        }

        public virtual User ModifiedBy
        {
            get { return _modifiedBy; }
            set { _modifiedBy = value; }
        }

        public virtual User ConfirmedBy
        {
            get { return _confirmedBy; }
            set { _confirmedBy = value; }
        }

        public virtual DateTime ModifiedDate
        {
            get { return _modifiedDate; }
            set { _modifiedDate = value; }
        }

        public virtual User Partner
        {
            get { return _partnerId; }
            set { _partnerId = value; }
        }

        public virtual User Sale
        {
            get { return _saleId; }
            set { _saleId = value; }
        }

        public virtual DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }
        /// <summary>
        /// Trạng thái của booking(Pending, Approved, Cancelled)
        /// </summary>
        public virtual StatusType Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public virtual TripOption TripOption
        {
            get { return _tripOpion; }
            set { _tripOpion = value; }
        }
        /// <summary>
        /// Trip liên quan tới Booking
        /// </summary>
        public virtual SailsTrip Trip
        {
            get { return _trip; }
            set { _trip = value; }
        }
        /// <summary>
        /// Revenue tiền đô
        /// </summary>
        public virtual double Total { set; get; }
        /// <summary>
        /// Revenue tiền việt
        /// </summary>
        public virtual double TotalVND { set; get; }
        /// <summary>
        /// Revenue bằng tiền đô hay tiền việt
        /// </summary>
        public virtual bool IsVND { get; set; }
        /// <summary>
        /// Tiền guide thu hộ bằng tiền đô hay tiền việt
        /// </summary>
        public virtual bool IsGuideCollectVND { get; set; }
        /// <summary>
        /// Tiền lái xe thu hộ bằng tiền đô hay tiền việt
        /// </summary>
        public virtual bool IsDriverCollectVND { get; set; }
        /// <summary>
        /// Commission cho đối tác trả bằng tiền đô hay tiền việt
        /// </summary>
        public virtual bool IsCommissionVND { get; set; }
        /// <summary>
        /// Tiền phạt Cancelled Booking trả bằng tiền đô hay tiền mặt 
        /// </summary>
        public virtual bool IsCancelPayVND { get; set; }

        public virtual double Paid
        {
            get { return _paid; }
            set { _paid = value; }
        }

        public virtual string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// Booking thuộc về Agency nào
        /// </summary>
        public virtual Agency Agency
        {
            get { return _agency; }
            set { _agency = value; }
        }

        public virtual string PickupAddress
        {
            get { return _pickupAddress; }
            set { _pickupAddress = value; }
        }

        public virtual string SpecialRequest
        {
            get { return _specialRequest; }
            set { _specialRequest = value; }
        }

        public virtual string AgencyCode
        {
            get { return _agecyCode; }
            set { _agecyCode = value; }
        }

        /// <summary>
        /// Giá quy đổi tiền đô sang tiền việt áp dụng cho Booking
        /// </summary>
        public virtual double CurrencyRate
        {
            get { return _currencyRate; }
            set { _currencyRate = value; }
        }

        public virtual double PaidBase
        {
            get { return _paidBase; }
            set { _paidBase = value; }
        }
        /// <summary>
        /// Booking đã được trả tiền hay chưa
        /// </summary>
        public virtual bool IsPaid
        {
            get
            {
                return _isPaid;
            }
            set { _isPaid = value; }
        }

        public virtual AccountingStatus AccountingStatus
        {
            get { return _accountingStatus; }
            set { _accountingStatus = value; }
        }
        /// <summary>
        /// Danh sách các khách hàng có trong Booking
        /// </summary>
        public virtual IList Customers
        {
            get
            {
                if (_customers == null)
                {
                    _customers = new ArrayList();
                }
                return _customers;
            }
            set { _customers = value; }
        }
        /// <summary>
        /// Lấy từ trang moorientalsails nên loại ra
        /// </summary>
        public virtual IList BookingRooms
        {
            get
            {
                if (_bookosBookingRooms == null)
                {
                    _bookosBookingRooms = new ArrayList();
                }
                return _bookosBookingRooms;
            }
            set { _bookosBookingRooms = value; }
        }
        /// <summary>
        /// Lấy từ trang moorientalsails nên loại ra
        /// </summary>
        public virtual IList ExtraServices
        {
            get
            {
                if (_extraServices == null)
                {
                    _extraServices = new ArrayList();
                }
                return _extraServices;
            }
            set { _extraServices = value; }
        }
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
            set { _services = value; }
        }

        public virtual string BookingId
        {
            get { return _bookingId; }
            set { _bookingId = value; }
        }

        public virtual int CustomBookingId
        {
            get { return _customBookingId; }
            set { _customBookingId = value; }
        }

        public virtual string CustomBookingCode
        {
            get { return _customBookingCode; }
            set { _customBookingCode = value; }
        }

        public virtual Locked Charter
        {
            get { return _lock; }
            set { _lock = value; }
        }

        #region -- Chuyển sang tàu khác --
        /// <summary>
        /// Phí khi chuyển sang cho tàu khác
        /// </summary>
        public virtual double TransferCost
        {
            get { return _transferCost; }
            set { _transferCost = value; }
        }

        /// <summary>
        /// Có đánh dấu chuyển sang tàu khác không
        /// </summary>
        public virtual bool IsTransferred
        {
            get { return _isTransferred; }
            set { _isTransferred = value; }
        }

        public virtual Agency TransferTo
        {
            get { return _transferTo; }
            set { _transferTo = value; }
        }

        public virtual int TransferAdult
        {
            get { return _transferAdult; }
            set { _transferAdult = value; }
        }

        public virtual int TransferChildren
        {
            get { return _transferChild; }
            set { _transferChild = value; }
        }

        public virtual int TransferBaby
        {
            get { return _transferBaby; }
            set { _transferBaby = value; }
        }
        #endregion

        public virtual int Amended
        {
            get { return _amended; }
            set { _amended = value; }
        }

        public virtual Cruise Cruise
        {
            get { return _cruise; }
            set { _cruise = value; }
        }

        public virtual bool IsCharter
        {
            get
            {
                if (_lock != null)
                {
                    _isCharter = true;
                }
                return _isCharter;
            }
            set { _isCharter = value; }
        }

        public virtual bool HasInvoice
        {
            get { return _hasInvoice; }
            set { _hasInvoice = value; }
        }

        public virtual double CancelPay
        {
            get { return _cancelPay; }
            set { _cancelPay = value; }
        }

        public virtual double CancelPayVND
        {
            get { return _cancelPayVND; }
            set { _cancelPayVND = value; }
        }

        public virtual string Guide
        {
            get { return _guide; }
            set { _guide = value; }
        }

        public virtual string Driver
        {
            get { return _driver; }
            set { _driver = value; }
        }

        public virtual bool GuideOnboard
        {
            get { return _guideOnboard; }
            set { _guideOnboard = value; }
        }

        public virtual User Locker
        {
            get { return _locker; }
            set { _locker = value; }
        }
        public virtual double Commission
        {
            get { return _commission; }
            set { _commission = value; }
        }

        public virtual double CommissionVND
        {
            get { return _commissionVND; }
            set { _commissionVND = value; }
        }

        public virtual double ComVNDpayed
        {
            get { return _comVNDpayed; }
            set { _comVNDpayed = value; }
        }

        public virtual double ComUSDpayed
        {
            get { return _comUSDpayed; }
            set { _comUSDpayed = value; }
        }

        public virtual double ComRate
        {
            get { return _comRate; }
            set { _comRate = value; }
        }

        public virtual bool ComPaid
        {
            get { return _comPaid; }
            set { _comPaid = value; }
        }
        private int _group;
        public virtual int Group
        {
            get
            {
                if (_group == 0)
                {
                    _group = 1;
                }
                return _group;
            }
            set { _group = value; }
        }

        public virtual double GuideCollect { get; set; }

        public virtual double DriverCollect { get; set; }

        public virtual double GuideCollectVND { get; set; }

        public virtual double DriverCollectVND { get; set; }

        public virtual bool GuideCollected { get; set; }

        public virtual bool DriverCollected { get; set; }


        public virtual double GuideCollectedUSD { get; set; }

        public virtual double DriverCollectedUSD { get; set; }

        public virtual double GuideCollectedVND { get; set; }

        public virtual double DriverCollectedVND { get; set; }

        public virtual double GuideCollectReceivable
        {
            get
            {

                if (GuideCollected)
                {
                    return 0;
                }

                if (IsGuideCollectVND)
                    return GuideCollectVND - GuideCollectedUSD * _currencyRate - GuideCollectedVND;
                else
                    return (GuideCollect - GuideCollectedUSD) * _currencyRate - GuideCollectedVND;
            }
        }

        public virtual double DriverCollectReceivable
        {
            get
            {

                if (DriverCollected)
                {
                    return 0;
                }

                if (IsDriverCollectVND)
                    return DriverCollectVND - DriverCollectedUSD * _currencyRate - DriverCollectedVND;
                else
                    return (DriverCollect - DriverCollectedUSD) * _currencyRate - DriverCollectedVND;
            }
        }

        public virtual DateTime? PaidDate { get; set; }
        public virtual DateTime? ComPaidDate { get; set; }
        public virtual bool GuideConfirmed { get; set; }

        public virtual bool AgencyConfirmed { get; set; }

        private IList<Transaction> _transactions;
        public virtual IList<Transaction> Transactions
        {
            get
            {
                if (_transactions == null)
                {
                    _transactions = new List<Transaction>();
                }
                return _transactions;
            }
            set { _transactions = value; }
        }

        public virtual double GuideCollectedRemain { get; set; }
        public virtual double AgencyRemain { get; set; }
        /// <summary>
        /// Số khách người lớn có trong Booking
        /// </summary>
        public virtual int Adult
        {
            get
            {
                if (!_counted)
                {
                    CountCustomers();
                }
                return _adult;
            }
        }
        /// <summary>
        /// Số khách có trong Booking
        /// </summary>
        public virtual int Pax
        {
            get { return _customers.Count; }
        }
        /// <summary>
        /// Số khách trẻ em có trong Booking
        /// </summary>
        public virtual int Child
        {
            get
            {
                if (!_counted)
                {
                    CountCustomers();
                }
                return _child;
            }
        }
        /// <summary>
        /// Số khách trẻ sơ sinh có trong Booking
        /// </summary>
        public virtual int Baby
        {
            get
            {
                if (!_counted)
                {
                    CountCustomers();
                }
                return _baby;
            }
        }
        /// <summary>
        /// Dùng để tính số khách Adult, Child và Baby
        /// </summary>
        protected virtual void CountCustomers()
        {
            _child = 0;
            _baby = 0;
            _adult = 0;
            foreach (Customer customer in _customers)
            {
                switch (customer.Type)
                {
                    case CustomerType.Adult:
                        _adult++;
                        break;
                    case CustomerType.Children:
                        _child++;
                        break;
                    case CustomerType.Baby:
                        _baby++;
                        break;
                }
            }
            _counted = true;
        }

        /// <summary>
        /// Lấy từ moorientalsails nên loại bỏ
        /// </summary>
        [Obsolete]
        public virtual int DoubleCabin
        {
            get
            {
                if (!_roomCounted)
                {
                    CountCabin();
                }
                return _double;
            }
        }

        /// <summary>
        /// Lấy từ moorientalsails nên loại bỏ
        /// </summary>
        [Obsolete]
        public virtual int TwinCabin
        {
            get
            {
                if (!_roomCounted)
                {
                    CountCabin();
                }
                return _twin;
            }
        }
        /// <summary>
        ///  Lấy từ moorientalsails nên loại bỏ
        /// </summary>
        [Obsolete]
        protected virtual void CountCabin()
        {
            _double = 0;
            _twin = 0;
            foreach (BookingRoom bookingRoom in BookingRooms)
            {
                if (bookingRoom.RoomType != null && bookingRoom.RoomType.Id == SailsModule.DOUBLE)
                {
                    _double++;
                }
                if (bookingRoom.RoomType != null && bookingRoom.RoomType.IsShared)
                {
                    _twin += bookingRoom.Adult;
                }
            }
            _roomCounted = true;
        }
        /// <summary>
        /// Tổng số tiền phải thu của Booking
        /// </summary>
        public virtual double TotalReceivable
        {
            get { return GuideCollectReceivable + AgencyReceivable; }
        }
        /// <summary>
        /// Tổng số tiền phải thu từ Agency
        /// </summary>
        public virtual double AgencyReceivable
        {
            get
            {
                if (_isPaid)
                {
                    return 0;
                }
                if (IsGuideCollectVND)
                {
                    if (IsVND)
                        return ValueVND - _paid + GuideCollectedUSD * _currencyRate + GuideCollectedVND - GuideCollectVND - _paidBase;
                    else
                    {
                        return Value * _currencyRate - _paid * _currencyRate + GuideCollectedUSD * _currencyRate + GuideCollectedVND - GuideCollectVND - _paidBase;
                    }
                }
                else
                {
                    if (IsVND)
                        return ValueVND - (_paid + GuideCollect) * _currencyRate - _paidBase + GuideCollectedUSD * _currencyRate + GuideCollectedVND;
                    else
                        return Value * _currencyRate - (_paid + GuideCollect) * _currencyRate - _paidBase + GuideCollectedUSD * _currencyRate + GuideCollectedVND;

                }
            }
        }

        /// <summary>
        /// Tổng số Commission chưa trả
        /// </summary>
        public virtual double CommissionLeft
        {
            get
            {

                if (_comPaid)
                {
                    return 0;
                }

                if (IsCommissionVND)
                    return CommissionVND - _comUSDpayed * _comRate - _comVNDpayed;
                else
                    return (Commission - _comUSDpayed) * _comRate - _comVNDpayed;
            }
        }

        public virtual string ContactEmail
        {
            get
            {
                if (string.IsNullOrEmpty(_email))
                {
                    if (_createdBy != null)
                    {
                        return _createdBy.Email;
                    }
                    return string.Empty;
                }
                return _email;
            }
        }

        public virtual string ContactName
        {
            get
            {
                if (string.IsNullOrEmpty(_name))
                {
                    if (_createdBy != null)
                    {
                        return _createdBy.FullName;
                    }
                    return string.Empty;
                }
                return _name;
            }
        }

        public virtual string CustomerName
        {
            get
            {
                string name = string.Empty;
                foreach (Customer customer in Customers)
                {
                    if (!string.IsNullOrEmpty(customer.Fullname))
                    {
                        name = name + customer.Fullname + "<br/>";
                    }
                }
                return name;
            }
        }

        public virtual string RoomName
        {
            get
            {
                string name = string.Empty;
                try
                {
                    Dictionary<string, int> rooms = new Dictionary<string, int>();
                    foreach (BookingRoom room in _bookosBookingRooms)
                    {
                        string key = room.RoomClass.Name + " " + room.RoomType.Name;
                        if (rooms.ContainsKey(key))
                        {
                            rooms[key] += 1;
                        }
                        else
                        {
                            rooms.Add(key, 1);
                        }
                    }

                    foreach (KeyValuePair<string, int> entry in rooms)
                    {
                        name += entry.Value + " " + entry.Key + "<br/>";
                    }
                }
                catch
                {
                }
                return name;
            }
        }

        public virtual string Confirmer
        {
            get
            {
                if (_confirmedBy != null)
                {
                    return _confirmedBy.FullName;
                }
                return "System";
            }
        }

        public virtual string Note
        {
            get { return _note; }
            set { _note = value; }
        }

        public virtual AgencyContact Booker
        {
            get { return _booker; }
            set { _booker = value; }
        }

        public virtual string BookerName
        {
            get
            {
                if (_booker != null)
                    return _booker.Name;
                return string.Empty;
            }
        }

        public virtual bool IsApproved
        {
            get { return _isApproved; }
            set { _isApproved = value; }
        }
        public virtual bool IsPaymentNeeded
        {
            get { return _isPaymentNeeded; }
            set { _isPaymentNeeded = value; }
        }
        public virtual double Value
        {
            get
            {
                switch (_status)
                {
                    case StatusType.Approved:
                        return Total;
                    case StatusType.Cancelled:
                        return CancelPay;
                    default:
                        return 0;
                }
            }
        }
        public virtual double ValueVND
        {
            get
            {
                switch (_status)
                {
                    case StatusType.Approved:
                        return TotalVND;
                    case StatusType.Cancelled:
                        return CancelPayVND;
                    default:
                        return 0;
                }
            }
        }
        /// <summary>
        /// Ngày khóa Booking
        /// </summary>
        public virtual DateTime? LockDate { get; set; }
        /// <summary>
        /// Khóa bởi ai
        /// </summary>
        public virtual User LockBy { get; set; }
        /// <summary>
        /// Khóa doanh thu hay chưa
        /// </summary>
        public virtual bool LockIncome
        {
            get
            {
                if (LockDate.HasValue && LockDate.Value < DateTime.Now)
                {
                    return true;
                }
                return false;
            }
        }
        /// <summary>
        /// Mở khóa doanh thu hay chưa
        /// </summary>
        public virtual bool UnlockIncome
        {
            get
            {
                if (LockDate.HasValue && LockDate.Value > DateTime.Now)
                {
                    return true;
                }
                return false;
            }
        }
        [Obsolete]
        public virtual DateTime? PickupTime { get; set; }
        [Obsolete]
        public virtual DateTime? SeeoffTime { get; set; }
        [Obsolete]
        public virtual String PUFlightDetails { get; set; }
        [Obsolete]
        public virtual String PUCarRequirements { get; set; }
        [Obsolete]
        public virtual String SOFlightDetails { get; set; }
        [Obsolete]
        public virtual String SOCarRequirements { get; set; }
        [Obsolete]
        public virtual String PUPickupAddress { get; set; }
        [Obsolete]
        public virtual String PUDropoffAddress { get; set; }
        [Obsolete]
        public virtual String SOPickupAddress { get; set; }
        [Obsolete]
        public virtual String SODropoffAddress { get; set; }
        /// <summary>
        /// Ngày kết thúc Trip
        /// </summary>
        public virtual DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
        }
        /// <summary>
        /// Trạng thái khóa Booking
        /// </summary>
        public virtual string LockStatus { get; set; }
        /// <summary>
        /// Sô tiền Discount
        /// </summary>
        public virtual double DiscountAmount { get; set; }
        /// <summary>
        /// Số phần trăm Discount
        /// </summary>
        public virtual double? DiscountPercent { get; set; }
        /// <summary>
        /// Loại tiền dùng ở phần Discount
        /// </summary>
        public virtual string DiscountCurrencyType { get; set; }
        public virtual Dictionary<CostType, double> Cost(CostingTable table, IList costTypes)
        {
            Dictionary<CostType, double> serviceTotal = new Dictionary<CostType, double>();
            foreach (CostType type in costTypes)
            {
                serviceTotal.Add(type, 0);
            }
            Dictionary<CostType, Costing> map = table.GetCostMap(costTypes);
            foreach (CostType type in costTypes)
            {
                if (!type.IsDailyInput && type.Service == null)
                {
                    if (map.ContainsKey(type))
                    {
                        serviceTotal[type] = map[type].Adult * Adult + map[type].Child * Child + map[type].Baby * Baby;
                    }
                }

                if (!type.IsDailyInput && type.Service != null)
                {
                    if (!map.ContainsKey(type))
                    {
                        throw new Exception("Price setting is not completed:" + type.Name);
                    }

                    if (type.Service.Target == ServiceTarget.Customer)
                    {
                        foreach (BookingRoom bkroom in BookingRooms)
                        {
                            int _index = 0;
                            foreach (Customer customer in bkroom.Customers)
                            {
                                _index++;
                                if (bkroom.IsSingle && _index == 2)
                                {
                                    continue;
                                }
                                if (customer.CustomerExtraOptions.Contains(type.Service))
                                {
                                    if (customer.IsChild)
                                    {
                                        serviceTotal[type] += map[type].Child;
                                        continue;
                                    }
                                    switch (customer.Type)
                                    {
                                        case CustomerType.Adult:
                                            serviceTotal[type] += map[type].Adult;
                                            break;
                                        case CustomerType.Children:
                                            serviceTotal[type] += map[type].Child;
                                            break;
                                        case CustomerType.Baby:
                                            serviceTotal[type] += map[type].Baby;
                                            break;
                                    }
                                }
                            }
                        }
                    }

                    if (type.Service.Target == ServiceTarget.Booking)
                    {
                        serviceTotal[type] = map[type].Adult * Adult + map[type].Child * Child + map[type].Baby * Baby;
                    }
                }
            }

            return serviceTotal;
        }
        public virtual ICollection<BookingHistory> BookingHistories { get; set; }
    }

    public enum AccountingStatus
    {
        New,
        Modified,
        Updated
    }
}