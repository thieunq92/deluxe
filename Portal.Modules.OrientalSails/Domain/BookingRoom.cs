using System;
using System.Collections;
using Portal.Modules.OrientalSails.Web.Util;

namespace Portal.Modules.OrientalSails.Domain
{

    #region BookingRoom

    /// <summary>
    /// BookingRoom object for NHibernate mapped table 'os_BookingRoom'.
    /// </summary>
    public class BookingRoom
    {
        #region Static Columns Name

        public static string BOOKING = "Book";
        public static string ROOM = "Room";

        #endregion

        #region Member Variables

        protected Booking _book;
        protected int _id;
        protected Room _room;
        protected RoomClass _roomClass;
        protected RoomTypex _roomType;
        protected BookingType _bookingType;
        protected bool _hasChild;
        protected bool _hasBaby;
        protected bool _isSingle;
        protected IList _customers;

        protected int _booked;
        protected int _adult;
        protected int _virtualAdult;
        protected int _child;
        protected int _baby;

        protected double _total;

        protected IList _realCustomers;

        protected string _tourComment;
        protected string _roomComment;
        protected string _staffComment;
        protected string _foodComment;
        protected string _guideComment;
        protected string _customerIdea;

        #endregion

        #region Constructors

        public BookingRoom()
        {
            _id = -1;
        }

        public BookingRoom(Booking book, Room room, RoomClass roomClass, RoomTypex roomType)
        {
            _book = book;
            _room = room;
            _roomClass = roomClass;
            _roomType = roomType;
        }

        #endregion

        #region Public Properties

        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual Booking Book
        {
            get { return _book; }
            set { _book = value; }
        }

        public virtual Room Room
        {
            get { return _room; }
            set { _room = value; }
        }

        public virtual RoomClass RoomClass
        {
            get { return _roomClass; }
            set { _roomClass = value; }
        }

        public virtual RoomTypex RoomType
        {
            get { return _roomType; }
            set { _roomType = value; }
        }

        public virtual BookingType BookingType
        {
            get { return _bookingType; }
            set { _bookingType = value; }
        }

        public virtual bool HasChild
        {
            get { return _hasChild; }
            set { _hasChild = value; }
        }

        public virtual bool HasBaby
        {
            get { return _hasBaby; }
            set { _hasBaby = value; }
        }

        public virtual bool IsSingle
        {
            get { return _isSingle; }
            set { _isSingle = value; }
        }

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

        public virtual int Booked
        {
            get { return _booked; }
            set { _booked = value; }
        }

        public virtual string TourComment
        {
            get { return _tourComment; }
            set { _tourComment = value; }
        }

        public virtual string RoomComment
        {
            get { return _roomComment; }
            set { _roomComment = value; }
        }

        public virtual string FoodComment
        {
            get { return _foodComment; }
            set { _foodComment = value; }
        }

        public virtual string StaffComment
        {
            get { return _staffComment; }
            set { _staffComment = value; }
        }

        public virtual string GuideComment
        {
            get { return _guideComment; }
            set { _guideComment = value; }
        }

        public virtual string CustomerIdea
        {
            get { return _customerIdea; }
            set { _customerIdea = value; }
        }

        /// <summary>
        /// Số khách người lớn (thực tế)
        /// </summary>
        public virtual int Adult
        {
            get
            {
                if (_adult == 0)
                {
                    if (_isSingle)
                    {
                        // Neu la book single thi kiem tra xem co phai la child single khong thong qua client 1 (vi single luon chi co client 1)
                        if (Customers.Count > 0)
                        {
                            if (((Customer)Customers[0]).IsChild)
                            {
                                _adult = 0;
                            }
                            _adult = 1;
                        }
                    }
                    else
                    {
                        // Nếu là book bình thường thì phải đếm
                        foreach (Customer customer in RealCustomers)
                        {
                            if (customer.Type == CustomerType.Adult && !customer.IsChild)
                            {
                                _adult += 1;
                            }
                        }
                    }
                }
                return _adult;
            }
        }

        public virtual double Total
        {
            get { return _total; }
            set { _total = value; }
        }

        // Số khách ảo (dùng cho việc tính chỗ trống)
        public virtual int VirtualAdult
        {
            get
            {
                if (_virtualAdult == 0)
                {
                    foreach (Customer customer in Customers)
                    {
                        if (customer.Type == CustomerType.Adult)
                        {
                            _virtualAdult += 1;
                        }
                    }
                }
                return _virtualAdult;
            }
        }

        public virtual int Child
        {
            get
            {
                int child;
                if (IsSingle)
                {
                    child = 1 - Adult;
                }
                else
                {
                    child = 2 - Adult;
                }

                if (_hasChild)
                {
                    return child + 1;
                }
                return child;
            }
        }

        public virtual int Baby
        {
            get
            {
                if (_hasBaby)
                {
                    return 1;
                }
                return 0;
            }
        }
        #endregion

        // Danh sách khách thực
        public virtual IList RealCustomers
        {
            get
            {
                if (_realCustomers == null)
                {
                    _realCustomers = new ArrayList();
                    int space = 0;
                    int maxSpace = 2;
                    if (_isSingle) maxSpace = 1;
                    foreach (Customer customer in Customers)
                    {
                        switch (customer.Type)
                        {
                            case CustomerType.Adult:
                                if (space < maxSpace)
                                {
                                    _realCustomers.Add(customer);
                                    space += 1;
                                }
                                break;
                            case CustomerType.Children:
                                if (_hasChild)
                                {
                                    _realCustomers.Add(customer);
                                }
                                break;
                            case CustomerType.Baby:
                                if (_hasChild)
                                {
                                    _realCustomers.Add(customer);
                                }
                                break;
                        }
                    }
                }
                return _realCustomers;
            }
        }

        public virtual string CustomerName
        {
            get
            {
                string name = string.Empty;
                foreach (Customer customer in RealCustomers)
                {
                    if (!string.IsNullOrEmpty(customer.Fullname))
                    {
                        name = name + customer.Fullname + "<br/>";
                    }
                }
                return name;
            }
        }

        public virtual double Calculate(SailsModule Module, IList policies, double childPrice, double agencySup, Agency agency)
        {
            // Lấy về kiểu phòng
            RoomClass rclass = RoomClass;
            RoomTypex rtype = RoomType;

            int adult = Adult;
            int child = Child;

            double price = Calculate(rclass, rtype, adult, child, IsSingle, _book.Trip, _book.Cruise, _book.TripOption, _book.StartDate, Module,
                      policies, childPrice, agencySup, agency);

            return price;
        }

        /// <summary>
        /// Tính giá cho một loại phòng
        /// </summary>
        /// <returns></returns>
        public static double Calculate(RoomClass rclass, RoomTypex rtype, int adult, int child, bool isSingle, SailsTrip trip, Cruise cruise, TripOption option, DateTime startDate, SailsModule Module, IList policies, double childPrice, double agencySup, Agency agency)
        {
            // Lấy về bảng giá áp dụng cho thời điểm xuất phát
            if (trip == null) return 0;
            SailsPriceConfig priceConfig = Module.SailsPriceConfigGet(rclass, rtype, trip, cruise, option, startDate,
                                                                      BookingType.Double, agency);

            if (priceConfig == null)
            {
                throw new PriceException(string.Format("There isn't any price for {0} {1} room in trip {2} on {3}", rclass.Name, rtype.Name, trip.Name, startDate));
            }
            #region -- Giá phòng --
            double price;
            // Biến để lưu giá trị single supplement nếu là booking single
            double singlesup = 0;
            if (isSingle)
            {
                if (agencySup > 0)
                {
                    singlesup = agencySup;
                }
                else
                {
                    singlesup = priceConfig.SpecialPrice; //Module.ApplyPriceFor(priceConfig.SpecialPrice, policies);
                }
            }

            // Tính giá phòng theo người lớn

            // Đơn giá của phòng (đã áp dụng chính sách)
            double unitPrice;
            if (priceConfig.Table.Agency==null)
            {
                //unitPrice = Module.ApplyPriceFor(priceConfig.NetPrice, policies);
            }
            else
            {
                //unitPrice = priceConfig.NetPrice;
            }
            if (rtype.IsShared)
            {
                // Giá phòng twin phòng đơn giá x số lượng người lớn / 2 + đơn giá x tỉ lệ dành cho child x số child / 2
                // (Thực ra adult = 1/2, child =0/1)
                //price = unitPrice * adult / 2 + unitPrice * child * childPrice / 100 / 2;
                // Cộng thêm singlesup (nếu không phải single thì là + 0)
                //price += singlesup;
            }
            else
            {
                // Giá phòng double phòng đơn giá x số lượng người lớn / 2 + đơn giá x tỉ lệ dành cho child x số child / 2
                //price = unitPrice * adult / 2 + unitPrice * child * childPrice / 100 / 2;
                // Cộng thêm singlesup (nếu không phải single thì là + 0)
                //price += singlesup;
            }
            #endregion

            //return price;

            return 0.0;
        }
    }

    public class BookingTypeClass : NHibernate.Type.EnumStringType
    {
        public BookingTypeClass()
            : base(typeof(BookingType), 50)
        {
        }
    }

    #endregion
}