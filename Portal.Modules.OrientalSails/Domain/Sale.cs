using System;
using System.Collections;
using CMS.Core.Domain;

namespace Portal.Modules.OrientalSails.Domain
{

    #region Customer

    /// <summary>
    /// Customer object for NHibernate mapped table 'os_Customer'.
    /// </summary>
    public class Sale
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

        protected User _user;
        protected IList _users;
        protected int _id;

        #endregion

        #region Constructors

        public Sale()
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

        public virtual User User
        {
            get { return _user; }
            set { _user = value; }
        }

        public virtual IList Users
        {
            get
            {
                if (_users == null)
                {
                    _users = new ArrayList();
                }
                return _users;
            }
            set { _users = value; }
        }

        #endregion
    }

    #endregion
}