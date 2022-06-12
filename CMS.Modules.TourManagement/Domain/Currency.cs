using System;

namespace CMS.Modules.TourManagement.Domain
{

    #region Currency

    /// <summary>
    /// Currency object for NHibernate mapped table 'tm_Currency'.
    /// </summary>
    public class Currency
    {
        #region Member Variables

        protected int _createdBy;
        protected DateTime _createdDate;
        protected string _cultureKey;
        protected string _currencyFormat;
        protected bool _deleted;
        protected int _id;
        protected int _modifiedBy;
        protected DateTime _modifiedDate;
        protected string _name;
        protected double _rate;
        protected string _symbol;

        #endregion

        #region Constructors

        public Currency()
        {
            _id = -1;
        }

        public Currency(bool deleted, DateTime createdDate, int createdBy, DateTime modifiedDate, int modifiedBy,
                        string cultureKey, string currencyFormat, string symbol, double rate, string name)
        {
            _deleted = deleted;
            _createdDate = createdDate;
            _createdBy = createdBy;
            _modifiedDate = modifiedDate;
            _modifiedBy = modifiedBy;
            _cultureKey = cultureKey;
            _currencyFormat = currencyFormat;
            _symbol = symbol;
            _rate = rate;
            _name = name;
        }

        #endregion

        #region Public Properties

        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual bool Deleted
        {
            get { return _deleted; }
            set { _deleted = value; }
        }

        public virtual DateTime CreatedDate
        {
            get { return _createdDate; }
            set { _createdDate = value; }
        }

        public virtual int CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; }
        }

        public virtual DateTime ModifiedDate
        {
            get { return _modifiedDate; }
            set { _modifiedDate = value; }
        }

        public virtual int ModifiedBy
        {
            get { return _modifiedBy; }
            set { _modifiedBy = value; }
        }

        public virtual string CultureKey
        {
            get { return _cultureKey; }
            set
            {
                if (value != null && value.Length > 5)
                    throw new ArgumentOutOfRangeException("Invalid value for CultureKey", value, value);
                _cultureKey = value;
            }
        }

        public virtual string CurrencyFormat
        {
            get { return _currencyFormat; }
            set
            {
                if (value != null && value.Length > 20)
                    throw new ArgumentOutOfRangeException("Invalid value for CurrencyFormat", value, value);
                _currencyFormat = value;
            }
        }

        public virtual string Symbol
        {
            get { return _symbol; }
            set
            {
                if (value != null && value.Length > 5)
                    throw new ArgumentOutOfRangeException("Invalid value for Symbol", value, value);
                _symbol = value;
            }
        }

        public virtual double Rate
        {
            get { return _rate; }
            set { _rate = value; }
        }

        public virtual string Name
        {
            get { return _name; }
            set
            {
                if (value != null && value.Length > 100)
                    throw new ArgumentOutOfRangeException("Invalid value for Name", value, value);
                _name = value;
            }
        }

        #endregion
    }

    #endregion
}