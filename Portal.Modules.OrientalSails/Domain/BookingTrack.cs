using System;
using System.Collections;
using CMS.Core.Domain;

namespace Portal.Modules.OrientalSails.Domain
{
    #region Customer

    /// <summary>
    /// Customer object for NHibernate mapped table 'os_Customer'.
    /// </summary>
    public class BookingTrack
    {
        #region Static Columns Name

        #endregion

        #region Member Variables

        protected int _id;
        protected Booking _booking;
        protected DateTime _modifiedDate;
        protected User _user;

        protected IList _changes;

        #endregion

        #region Constructors

        public BookingTrack()
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

        public virtual Booking Booking
        {
            get { return _booking; }
            set { _booking = value; }
        }

        public virtual DateTime ModifiedDate
        {
            get { return _modifiedDate; }
            set { _modifiedDate = value; }
        }

        public virtual User User
        {
            get { return _user;}
            set { _user = value; }
        }

        public virtual IList Changes
        {
            get
            {
                if (_changes == null)
                {
                    _changes = new ArrayList();
                }
                return _changes;
            }
            set { _changes = value; }
        }

        #endregion
    }

    #endregion
}
