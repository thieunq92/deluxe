using System;

namespace Portal.Modules.OrientalSails.Domain
{
    #region Customer

    /// <summary>
    /// Customer object for NHibernate mapped table 'os_Customer'.
    /// </summary>
    public class BookingChanged
    {
        #region Static Columns Name

        #endregion

        #region Member Variables

        protected int _id;
        protected BookingAction _action;
        protected string _parameter;
        protected BookingTrack _track;

        #endregion

        #region Constructors

        public BookingChanged()
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

        public virtual BookingAction Action
        {
            get { return _action;}
            set { _action = value; }
        }

        public virtual string Parameter
        {
            get { return _parameter; }
            set { _parameter = value; }
        }

        public virtual BookingTrack Track
        {
            get { return _track; }
            set { _track = value; }
        }

        #endregion
    }

    public enum BookingAction
    {
        RemoveRoom,
        Created,
        Approved,
        Cancelled,
        AddRoom,
        ChangeRoomType,
        ChangeRoomNumber,
        AddCustomer,
        RemoveCustomer,
        ChangeTrip,
        ChangeDate,
        ChangeTotal,
        Transfer,
        Untransfer,
        ChangeTransfer
    }

    public class BookingActionClass : NHibernate.Type.EnumStringType
    {
        public BookingActionClass()
            : base(typeof(BookingAction), 30)
        {

        }
    }

    #endregion
}
