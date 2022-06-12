using System;
using System.Collections;

namespace Portal.Modules.OrientalSails.Domain
{

    #region Room

    /// <summary>
    /// Room object for NHibernate mapped table 'os_Room'.
    /// </summary>
    public class Room
    {
        #region Static Columns Name

        public static string DELETED = "Deleted";
        public static string NAME = "Name";
        public static string ORDER = "Order";
        public static string ROOMCLASS = "RoomClass";
        public static string ROOMTYPE = "RoomType";

        #endregion

        #region Member Variables

        protected bool _deleted;
        protected int _id;
        protected bool _isAvailable = true;
        protected string _name;
        protected int _order;
        protected RoomClass _roomClass;
        protected IList _roomosBookingRooms;
        protected RoomTypex _roomType;
        protected int _adult;
        protected int _child;
        protected int _baby;

        protected Cruise _cruise;

        #endregion

        #region Constructors

        public Room()
        {
            _id = -1;
        }

        public Room(string name, bool deleted, RoomClass roomClass, RoomTypex roomType)
        {
            _name = name;
            _deleted = deleted;
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

        public virtual int Order
        {
            get { return _order; }
            set { _order = value; }
        }

        public virtual bool Deleted
        {
            get { return _deleted; }
            set { _deleted = value; }
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

        public virtual IList BookingRooms
        {
            get
            {
                if (_roomosBookingRooms == null)
                {
                    _roomosBookingRooms = new ArrayList();
                }
                return _roomosBookingRooms;
            }
            set { _roomosBookingRooms = value; }
        }

        public virtual bool IsAvailable
        {
            get { return _isAvailable; }
            set { _isAvailable = value; }
        }

        public virtual int Adult
        {
            get { return _adult; }
            set { _adult = value; }
        }

        public virtual int Child
        {
            get { return _child; }
            set { _child = value; }
        }

        public virtual int Baby
        {
            get { return _baby; }
            set { _baby = value; }
        }

        public virtual Cruise Cruise
        {
            get { return _cruise; }
            set { _cruise = value; }
        }

        #endregion
    }

    #endregion
}