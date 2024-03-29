namespace Portal.Modules.OrientalSails.Domain
{
    #region Customer

    /// <summary>
    /// Customer object for NHibernate mapped table 'os_Customer'.
    /// </summary>
    public class CostType
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

        protected int _id;
        protected string _name;
        protected bool _isDailyInput;
        protected bool _isCustomType;
        protected bool _isSupplier;
        protected Agency _defaultAgency;
        protected ExtraOption _service;
        protected bool _isMonthly;
        protected bool _isYearly;
        protected bool _isDaily;
        protected string _groupName;
        protected bool _isPayNow;

        #endregion

        #region Constructors

        public CostType()
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

        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Đánh dấu chi phí nhập thủ công
        /// </summary>
        public virtual bool IsDailyInput
        {
            get { return _isDailyInput; }
            set { _isDailyInput = value; }
        }

        public virtual bool IsCustomType
        {
            get { return _isCustomType;}
            set { _isCustomType = value; }
        }

        public virtual bool IsSupplier
        {
            get { return _isSupplier;}
            set { _isSupplier = value; }
        }

        public virtual Agency DefaultAgency
        {
            get { return _defaultAgency; }
            set { _defaultAgency = value; }
        }

        public virtual ExtraOption Service
        {
            get { return _service;}
            set { _service = value; }
        }

        public virtual bool IsMonthly
        {
            get { return _isMonthly;}
            set { _isMonthly = value; }
        }

        public virtual bool IsYearly
        {
            get { return _isYearly; }
            set { _isYearly = value; }
        }

        public virtual bool IsDaily
        {
            get { return _isDaily;}
            set { _isDaily = value; }
        }

        public virtual string GroupName
        {
            get { return _groupName; }
            set { _groupName = value; }
        }

        public virtual bool IsPayNow
        {
            get { return _isPayNow; }
            set { _isPayNow = value; }
        }

        #endregion
    }

    public enum CostBase
    {
        Both,
        Customer,
        Booking
    }

    #endregion
}
