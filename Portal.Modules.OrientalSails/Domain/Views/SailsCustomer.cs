using System;
using System.Collections;
using CMS.Core.Domain;

namespace Portal.Modules.OrientalSails.Domain.Views
{
    #region Customer

    /// <summary>
    /// Customer object for NHibernate mapped table 'os_Customer'.
    /// </summary>
    public class SailsCustomer
    {        
        #region Member Variables

        protected int _id;
        protected string _name;
        protected string _phone;
        protected string _address;
        protected Role _role;
        protected IList _users;
        protected string _fax;
        protected string _email;
        protected string _taxCode;
        protected int _contractStatus;
        protected string _description;
        protected string _contract;
        protected User _sale;
        protected string _accountant;
        protected PaymentPeriod _paymentMethod;
        protected DateTime _lastBooking;
        protected IList _bookings;

        protected DateTime? _createdDate;
        protected User _createdBy;
        protected DateTime? _modifiedDate;
        protected User _modifiedBy;
        protected bool _isDeleted;

        #endregion

        #region Constructors

        public SailsCustomer()
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

        public virtual int Pax { get; set; }
        public virtual SailExpense Expense { get; set; }

        #endregion
    }

    #endregion
}
