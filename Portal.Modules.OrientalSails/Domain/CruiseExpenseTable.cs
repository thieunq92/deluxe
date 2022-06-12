using System;
using System.Collections;

namespace Portal.Modules.OrientalSails.Domain
{

    #region Customer

    /// <summary>
    /// Customer object for NHibernate mapped table 'os_Customer'.
    /// </summary>
    public class CruiseExpenseTable
    {
        #region Static Columns Name

        public static string BIRTHDAY = "Birthday";
        public static string BOOKING = "Booking";
        public static string BOOKINGROOM = "BookingRoom";
        public static string COUNTRY = "Country";
        public static string FULLNAME = "Fullname";
        public static string PASSPORT = "Passport";

        #endregion

        #region Member Variables

        protected int _id;
        protected DateTime _validFrom;
        protected DateTime? _validTo;
        protected IList _expenses;
        protected Cruise _cruise;

        #endregion

        #region Constructors

        public CruiseExpenseTable()
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

        public virtual DateTime ValidFrom
        {
            get { return _validFrom;}
            set { _validFrom = value; }
        }

        public virtual DateTime? ValidTo
        {
            get { return _validTo; }
            set { _validTo = value; }
        }

        public virtual IList Expenses
        {
            get
            {
                if (_expenses==null)
                {
                    _expenses = new ArrayList();
                }
                return _expenses;
            }
            set
            {
                _expenses = value;
            }
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