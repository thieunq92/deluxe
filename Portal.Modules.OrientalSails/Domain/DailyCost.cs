using System.Collections;
using CMS.Core.Domain;

namespace Portal.Modules.OrientalSails.Domain
{
    #region Customer

    /// <summary>
    /// Customer object for NHibernate mapped table 'os_Customer'.
    /// </summary>
    public class DailyCost
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
        protected DailyCostTable _table;
        protected double _cost;
        protected double _child;
        protected double _baby;

        #endregion

        #region Constructors

        public DailyCost()
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

        public virtual DailyCostTable Table
        {
            get { return _table; }
            set { _table = value; }
        }

        public virtual double Cost
        {
            get { return _cost; }
            set { _cost = value; }
        }


        #endregion
    }

    #endregion
}
