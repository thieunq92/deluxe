using System;

namespace Portal.Modules.OrientalSails.Domain
{
    #region Customer

    /// <summary>
    /// Customer object for NHibernate mapped table 'os_Customer'.
    /// </summary>
    public class USDRate
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
        protected DateTime _validFrom;
        protected DateTime? _validTo;
        protected double _rate;

        #endregion

        #region Constructors

        public USDRate()
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
            set { _validFrom = value;}
        }

        public virtual DateTime? ValidTo
        {
            get { return _validTo;}
            set { _validTo = value;}
        }

        public virtual double Rate
        {
            get { return _rate;}
            set { _rate = value;}
        }
        #endregion
    }

    #endregion
}
