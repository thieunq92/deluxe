using System;
using System.Collections;
using System.Web.UI.WebControls;

namespace Portal.Modules.OrientalSails.Domain
{
    #region CustomerExtraOption

    /// <summary>
    /// CustomerExtraOption object for NHibernate mapped table 'os_CustomerExtraOption'.
    /// </summary>
    public class BookingService
    {
        #region Member Variables

        protected int _id;
        protected Booking _booking;
        protected string _name;
        protected double _unitPrice;
        protected int _quantity;
        protected bool _isByCustomer;
        protected bool _deleted;

        #endregion

        #region Constructors

        public BookingService()
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

        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public virtual double UnitPrice
        {
            get { return _unitPrice; }
            set { _unitPrice = value; }
        }

        public virtual int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        public virtual bool IsByCustomer
        {
            get { return _isByCustomer; }
            set { _isByCustomer = value; }
        }

        public virtual bool Deleted
        {
            get { return _deleted; }
            set { _deleted = value; }
        }

        #endregion

    }

    #endregion
}
