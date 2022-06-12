using System;
using System.Collections;
using Portal.Modules.OrientalSails.Web.Util;

namespace Portal.Modules.OrientalSails.Domain
{

    #region Customer

    /// <summary>
    /// Customer object for NHibernate mapped table 'os_Customer'.
    /// </summary>
    public class Customer
    {
        #region Static Columns Name

        public static string BIRTHDAY = "Birthday";
        public static string BOOKING = "Booking";
        public static string COUNTRY = "Country";
        public static string FULLNAME = "Fullname";
        public static string PASSPORT = "Passport";
        public static string BOOKINGROOM = "BookingRoom";

        #endregion

        #region Member Variables

        protected DateTime? _birthday;
        protected Booking _booking;
        protected string _country;
        protected IList _customerosCustomerExtraOptions;
        protected string _fullname;
        protected int _id;
        protected string _passport;
        protected BookingRoom _bookingRoom;
        protected CustomerType _type;
        protected bool? _isMale;
        protected string _visaNo;
        protected DateTime? _visaExpired;
        protected bool _isChild;
        protected bool _isVietKieu;
        protected string _purpose;
        protected DateTime? _stayFrom;
        protected DateTime? _stayTo;
        protected string _stayTerm;
        protected string _stayIn;
        protected string _code;
        protected double _total;

        protected Nationality _nationality;
        protected Purpose _stayPurpose;

        #endregion

        #region Constructors

        public Customer()
        {
            _id = -1;
        }

        public Customer(string fullname, DateTime birthday, string passport, string country, Booking booking)
        {
            _fullname = fullname;
            _birthday = birthday;
            _passport = passport;
            _country = country;
            _booking = booking;
        }

        #endregion

        #region Public Properties

        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual string Fullname
        {
            get { return _fullname; }
            set
            {
                if (value != null && value.Length > 100)
                    throw new ArgumentOutOfRangeException("Invalid value for Fullname", value, value);
                _fullname = value;
            }
        }

        public virtual DateTime? Birthday
        {
            get { return _birthday; }
            set { _birthday = value; }
        }

        public virtual string Passport
        {
            get { return _passport; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for Passport", value, value);
                _passport = value;
            }
        }

        public virtual string Country
        {
            get { return _country; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for Country", value, value);
                _country = value;
            }
        }

        public virtual Booking Booking
        {
            get { return _booking; }
            set { _booking = value; }
        }

        public virtual BookingRoom BookingRoom
        {
            get { return _bookingRoom; }
            set { _bookingRoom = value; }
        }

        public virtual IList CustomerExtraOptions
        {
            get
            {
                if (_customerosCustomerExtraOptions == null)
                {
                    _customerosCustomerExtraOptions = new ArrayList();
                }
                return _customerosCustomerExtraOptions;
            }
            set { _customerosCustomerExtraOptions = value; }
        }

        public virtual CustomerType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public virtual bool? IsMale
        {
            get { return _isMale; }
            set { _isMale = value; }
        }

        public virtual string VisaNo
        {
            get { return _visaNo;}
            set { _visaNo = value; }
        }

        public virtual DateTime? VisaExpired
        {
            get { return _visaExpired;}
            set { _visaExpired = value; }
        }

        public virtual bool IsChild
        {
            get { return _isChild;}
            set { _isChild = value; }
        }

        public virtual bool IsVietKieu
        {
            get { return _isVietKieu;}
            set { _isVietKieu = value; }
        }

        public virtual string Purpose
        {
            get { return _purpose; }
            set { _purpose = value; }
        }

        public virtual DateTime? StayFrom
        {
            get { return _stayFrom;}
            set { _stayFrom = value; }
        }

        public virtual DateTime? StayTo
        {
            get { return _stayTo; }
            set { _stayTo = value; }
        }

        public virtual string StayTerm
        {
            get { return _stayTerm; }
            set { _stayTerm = value; }
        }

        public virtual string StayIn
        {
            get { return _stayIn; }
            set { _stayIn = value; }
        }

        public virtual string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        public virtual Nationality Nationality
        {
            get { return _nationality; }
            set { _nationality = value; }
        }

        public virtual Purpose StayPurpose
        {
            get { return _stayPurpose;}
            set { _stayPurpose = value; }
        }

        public virtual double Total
        {
            get { return _total; }
            set { _total = value; }
        }
        #endregion
    }

    #endregion
}