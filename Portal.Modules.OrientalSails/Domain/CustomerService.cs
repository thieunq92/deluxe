using System;

namespace Portal.Modules.OrientalSails.Domain
{
    #region Customer

    /// <summary>
    /// Customer object for NHibernate mapped table 'os_Customer'.
    /// </summary>
    public class CustomerService
    {
        #region Static Columns Name

        #endregion

        #region Member Variables

        protected int _id;
        protected Customer _customer;
        protected ExtraOption _service;
        protected bool _isExcluded;

        #endregion

        #region Constructors

        public CustomerService()
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

        public virtual Customer Customer
        {
            get { return _customer; }
            set { _customer = value; }
        }

        public virtual ExtraOption Service
        {
            get { return _service; }
            set { _service = value; }
        }

        public virtual bool IsExcluded
        {
            get { return _isExcluded;}
            set { _isExcluded = value; }
        }

        #endregion
    }

    public enum ServiceTarget
    {
        All,
        Customer,
        Booking
    }

    #endregion
}
