using System.Collections;
using CMS.Core.Domain;

namespace Portal.Modules.OrientalSails.Domain
{
    #region Customer

    /// <summary>
    /// Customer object for NHibernate mapped table 'os_Customer'.
    /// </summary>
    public class Costing
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
        protected CostType _type;
        protected CostingTable _table;
        protected double _adult;
        protected double _child;
        protected double _baby;

        #endregion

        #region Constructors

        public Costing()
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

        public virtual CostType Type
        {
            get { return _type;}
            set { _type = value; }
        }

        public virtual CostingTable Table
        {
            get { return _table; }
            set { _table = value; }
        }

        public virtual double Adult
        {
            get { return _adult; }
            set { _adult = value; }
        }

        public virtual double Child
        {
            get { return _child; }
            set { _child = value; }
        }

        public virtual double Baby
        {
            get { return _baby; }
            set { _baby = value; }
        }

        #endregion
    }

    public enum CostingType
    {
        Ticket,
        Meal,
        Kayaing,
        Service,
        Rockclimbing
    }

    #endregion
}
