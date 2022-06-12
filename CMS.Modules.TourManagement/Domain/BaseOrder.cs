using System;
using CMS.Core.Domain;

namespace CMS.Modules.TourManagement.Domain
{
    public abstract class BaseOrder
    {
        #region Static Columns Name

        public static string ADDRESS = "Address";
        public static string AUTHORIP = "IP";
        public static string CITY = "City";
        public static string COUNTRY = "Country";
        public static string EMAIL = "Email";
        public static string ESTIMATEDPRICE = "EstimatedPrice";
        public static string FULLNAME = "FullName";
        public static string MOBILE = "Mobile";
        public static string ORDERDATE = "OrderDate";
        public static string ORDERSTATUS = "OrderStatus";
        public static string READSTATUS = "ReadStatus";
        public static string SPECIALREQUEST = "SpecialRequest";
        public static string TELEPHONE = "Telephone";
        public static string USER = "User";

        #endregion

        #region -- PRIVATE MEMBERS --

        protected int _id;
        protected string _address;
        protected string _city;
        protected string _country;
        protected string _email;
        protected decimal _estimatedPrice;
        protected string _fullName;
        protected string _iP;
        protected string _mobile;
        protected DateTime _orderDate;
        protected int _orderStatus;
        protected bool _readStatus;
        protected string _specialRequest;
        protected string _telephone;
        protected User _userId;

        #endregion

        #region -- PROPERTIES --

        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual string SpecialRequest
        {
            get { return _specialRequest; }
            set
            {
                if (value != null && value.Length > 1000)
                    throw new ArgumentOutOfRangeException("Invalid value for SpecialRequest", value, value);
                _specialRequest = value;
            }
        }

        public virtual string FullName
        {
            get { return _fullName; }
            set
            {
                if (value != null && value.Length > 300)
                    throw new ArgumentOutOfRangeException("Invalid value for FullName", value, value);
                _fullName = value;
            }
        }

        public virtual string Email
        {
            get { return _email; }
            set
            {
                if (value != null && value.Length > 100)
                    throw new ArgumentOutOfRangeException("Invalid value for Email", value, value);
                _email = value;
            }
        }

        public virtual string Address
        {
            get { return _address; }
            set
            {
                if (value != null && value.Length > 500)
                    throw new ArgumentOutOfRangeException("Invalid value for Address", value, value);
                _address = value;
            }
        }

        public virtual string City
        {
            get { return _city; }
            set
            {
                if (value != null && value.Length > 100)
                    throw new ArgumentOutOfRangeException("Invalid value for City", value, value);
                _city = value;
            }
        }

        public virtual string Country
        {
            get { return _country; }
            set
            {
                if (value != null && value.Length > 100)
                    throw new ArgumentOutOfRangeException("Invalid value for Country", value, value);
                _country = value;
            }
        }

        public virtual string Telephone
        {
            get { return _telephone; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for Telephone", value, value);
                _telephone = value;
            }
        }

        public virtual string Mobile
        {
            get { return _mobile; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for Mobile", value, value);
                _mobile = value;
            }
        }

        public virtual User User
        {
            get { return _userId; }
            set { _userId = value; }
        }

        public virtual string IP
        {
            get { return _iP; }
            set
            {
                if (value != null && value.Length > 15)
                    throw new ArgumentOutOfRangeException("Invalid value for IP", value, value);
                _iP = value;
            }
        }

        public virtual bool ReadStatus
        {
            get { return _readStatus; }
            set { _readStatus = value; }
        }

        public virtual int OrderStatus
        {
            get { return _orderStatus; }
            set { _orderStatus = value; }
        }

        public virtual DateTime OrderDate
        {
            get { return _orderDate; }
            set { _orderDate = value; }
        }

        public virtual decimal EstimatedPrice
        {
            get { return _estimatedPrice; }
            set { _estimatedPrice = value; }
        }

        public abstract string OrderContent();


        #endregion
    }
}