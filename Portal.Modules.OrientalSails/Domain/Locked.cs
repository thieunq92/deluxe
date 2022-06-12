using System;

namespace Portal.Modules.OrientalSails.Domain
{

    #region Customer

    /// <summary>
    /// Customer object for NHibernate mapped table 'os_Customer'.
    /// </summary>
    public class Locked
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

        protected Booking _charter;
        protected Cruise _cruise;
        protected string _description;
        protected DateTime _end;
        protected int _id;
        protected DateTime _start;

        #endregion

        #region Constructors

        public Locked()
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

        public virtual DateTime Start
        {
            get { return _start; }
            set { _start = value; }
        }

        public virtual DateTime End
        {
            get { return _end; }
            set { _end = value; }
        }

        public virtual string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public virtual Booking Charter
        {
            get { return _charter; }
            set { _charter = value; }
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