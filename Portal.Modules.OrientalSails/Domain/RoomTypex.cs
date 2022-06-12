using System;
using System.Collections;

namespace Portal.Modules.OrientalSails.Domain
{

    #region RoomTypex

    /// <summary>
    /// RoomTypex object for NHibernate mapped table 'os_RoomTypex'.
    /// </summary>
    public class RoomTypex
    {
        #region Static Columns Name

        public static string CAPACITY = "Capacity";
        public static string NAME = "Name";
        public static string ORDER = "Order";

        #endregion

        #region Member Variables

        protected bool _allowSingBook;
        protected int _capacity;
        protected int _id;
        protected string _name;
        protected bool _isShared;
        protected int _order;
        protected IList _roomTypeosBookingRooms;
        protected IList _roomTypeosRooms;
        protected IList _roomTypeosSailsPriceConfigs;

        #endregion

        #region Constructors

        public RoomTypex()
        {
            _id = -1;
        }

        public RoomTypex(string name, int capacity)
        {
            _name = name;
            _capacity = capacity;
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
            set
            {
                if (value != null && value.Length > 200)
                    throw new ArgumentOutOfRangeException("Invalid value for Name", value, value);
                _name = value;
            }
        }

        public virtual int Capacity
        {
            get { return _capacity; }
            set { _capacity = value; }
        }

        public virtual int Order
        {
            get { return _order; }
            set { _order = value; }
        }

        public virtual bool IsShared
        {
            get { return _isShared; }
            set { _isShared = value; }
        }

        public virtual IList BookingRooms
        {
            get
            {
                if (_roomTypeosBookingRooms == null)
                {
                    _roomTypeosBookingRooms = new ArrayList();
                }
                return _roomTypeosBookingRooms;
            }
            set { _roomTypeosBookingRooms = value; }
        }

        public virtual IList Rooms
        {
            get
            {
                if (_roomTypeosRooms == null)
                {
                    _roomTypeosRooms = new ArrayList();
                }
                return _roomTypeosRooms;
            }
            set { _roomTypeosRooms = value; }
        }

        public virtual IList SailsPriceConfigs
        {
            get
            {
                if (_roomTypeosSailsPriceConfigs == null)
                {
                    _roomTypeosSailsPriceConfigs = new ArrayList();
                }
                return _roomTypeosSailsPriceConfigs;
            }
            set { _roomTypeosSailsPriceConfigs = value; }
        }

        public virtual bool AllowSingBook
        {
            get { return _allowSingBook; }
            set { _allowSingBook = value; }
        }

        #endregion
    }

    #endregion
}