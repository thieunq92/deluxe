using System;
using System.Collections;

namespace Portal.Modules.OrientalSails.Domain
{

    #region RoomClass

    /// <summary>
    /// RoomClass object for NHibernate mapped table 'os_RoomClass'.
    /// </summary>
    public class RoomClass
    {
        #region Static Columns Name

        public static string DESCRIPTION = "Description";
        public static string NAME = "Name";
        public static string ORDER = "Order";

        #endregion

        #region Member Variables

        protected string _description;
        protected int _id;
        protected string _name;
        protected int _order;
        protected Cruise _cruise;
        protected IList _roomClassosBookingRooms;
        protected IList _roomClassosRooms;
        protected IList _roomClassosSailsPriceConfigs;

        #endregion

        #region Constructors

        public RoomClass()
        {
            _id = -1;
        }

        public RoomClass(string name, string description)
        {
            _name = name;
            _description = description;
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
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for Name", value, value);
                _name = value;
            }
        }

        public virtual string Description
        {
            get { return _description; }
            set
            {
                if (value != null && value.Length > 250)
                    throw new ArgumentOutOfRangeException("Invalid value for Name", value, value);
                _description = value;
            }
        }

        public virtual int Order
        {
            get { return _order; }
            set { _order = value; }
        }

        public virtual Cruise Cruise
        {
            get { return _cruise; }
            set { _cruise = value; }
        }

        public virtual IList BookingRooms
        {
            get
            {
                if (_roomClassosBookingRooms == null)
                {
                    _roomClassosBookingRooms = new ArrayList();
                }
                return _roomClassosBookingRooms;
            }
            set { _roomClassosBookingRooms = value; }
        }

        public virtual IList Rooms
        {
            get
            {
                if (_roomClassosRooms == null)
                {
                    _roomClassosRooms = new ArrayList();
                }
                return _roomClassosRooms;
            }
            set { _roomClassosRooms = value; }
        }

        public virtual IList SailsPriceConfigs
        {
            get
            {
                if (_roomClassosSailsPriceConfigs == null)
                {
                    _roomClassosSailsPriceConfigs = new ArrayList();
                }
                return _roomClassosSailsPriceConfigs;
            }
            set { _roomClassosSailsPriceConfigs = value; }
        }

        #endregion
    }

    #endregion
}