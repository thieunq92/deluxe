using System;
using System.Collections;

namespace Portal.Modules.OrientalSails.Domain
{
    public class CruiseExpense : IComparable
    {
        protected int _id;
        protected int _customerFrom;
        protected int _custeromTo;
        protected double _price;
        protected int _currency;
        protected CruiseExpenseTable _table;
        public CruiseExpense()
        {
            _id = -1;
        }
        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual int CustomerFrom
        {
            get { return _customerFrom; }
            set { _customerFrom = value; }
        }

        public virtual int CustomerTo
        {
            get { return _custeromTo; }
            set { _custeromTo = value; }
        }

        public virtual double Price
        {
            get { return _price; }
            set { _price = value; }
        }

        public virtual int Currency
        {
            get { return _currency; }
            set { _currency = value; }
        }

        public virtual CruiseExpenseTable Table
        {
            get { return _table;}
            set { _table = value; }
        }
        public virtual int CompareTo(object obj)
        {
            return CustomerFrom - ((CruiseExpense)obj).CustomerFrom;
        }
    }
}
