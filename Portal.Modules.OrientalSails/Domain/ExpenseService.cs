using System;
using System.Collections;
using CMS.Core.Domain;
using System.Collections.Generic;

namespace Portal.Modules.OrientalSails.Domain
{

    public class ExpenseService
    {
        public static string BIRTHDAY = "Birthday";
        public static string BOOKING = "Booking";
        public static string COUNTRY = "Country";
        public static string FULLNAME = "Fullname";
        public static string PASSPORT = "Passport";
        public static string BOOKINGROOM = "BookingRoom";

        protected int _id;
        protected CostType _type;
        protected SailExpense _expense;
        protected string name;
        protected string phone;
        private Agency supplier;
        protected double cost;
        protected double _paid;
        protected bool _isOwnService;
        protected bool _isRemoved;
        protected int _group;
        public ExpenseService()
        {
            _id = -1;
        }

        private List<ExpenseHistory> listPendingExpenseHistory;
        public virtual List<ExpenseHistory> ListPendingExpenseHistory
        {
            get
            {
                if (listPendingExpenseHistory == null)
                {
                    listPendingExpenseHistory = new List<ExpenseHistory>();
                }
                return listPendingExpenseHistory;
            }
            set
            {
                listPendingExpenseHistory = value;
            }
        }


        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual CostType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public virtual SailExpense Expense
        {
            get { return _expense; }
            set { _expense = value; }
        }

        public virtual string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name == null) name = "";
                SetProperty<string>("Name", ref name, value);
            }
        }

        public virtual string Phone
        {
            get
            {
                return phone;
            }
            set
            {
                if (phone == null) phone = "";
                SetProperty<string>("Phone", ref phone, value);
            }
        }

        public virtual Agency Supplier
        {
            get
            {
                return supplier;
            }
            set
            {
                var oldSupplierId = supplier != null ? supplier.Id : -1;
                var newSupplierId = value != null ? value.Id : -1;
                SetProperty<int>("SupplierId", ref oldSupplierId, newSupplierId);
                supplier = value;
            }
        }

        public virtual double Cost
        {
            get
            {
                return cost;
            }
            set
            {
                SetProperty<double>("Cost", ref cost, value);
            }
        }

        public virtual double Paid
        {
            get { return _paid; }
            set { _paid = value; }
        }

        public virtual bool IsOwnService
        {
            get { return _isOwnService; }
            set { _isOwnService = value; }
        }

        public virtual bool IsRemoved
        {
            get { return _isRemoved; }
            set { _isRemoved = value; }
        }

        public virtual int Group
        {
            get
            {
                if (_group == 0)
                {
                    _group = 1;
                }
                return _group;
            }
            set { _group = value; }
        }

        public virtual DateTime? PaidDate { get; set; }
        public virtual String LockStatus { get; set; }
        public virtual User CurrentUser { get; set; }
        public virtual AgencyContact Driver { get; set; }
        public virtual IList<ExpenseHistory> ListExpenseHistory { get; set; }
        protected void SetProperty<T>(string name, ref T oldValue, T newValue) where T : System.IEquatable<T>
        {
            if (oldValue == null || !oldValue.Equals(newValue))
            {
                var expenseHistory = new ExpenseHistory()
                {
                    ColumnName = name,
                    OldValue = oldValue.ToString(),
                    NewValue = newValue.ToString(),
                    CreatedDate = DateTime.Now,
                    ExpenseService = this,
                    CreatedBy = CurrentUser,
                };
                ListPendingExpenseHistory.Add(expenseHistory);
                oldValue = newValue;
            }
        }
        protected void SetProperty<T>(string name, ref Nullable<T> oldValue, Nullable<T> newValue) where T : struct, System.IEquatable<T>
        {
            if (oldValue.HasValue != newValue.HasValue || (newValue.HasValue && !oldValue.Value.Equals(newValue.Value)))
            {
                var expenseHistory = new ExpenseHistory()
                {
                    ColumnName = name,
                    OldValue = oldValue.ToString(),
                    NewValue = newValue.ToString(),
                    CreatedDate = DateTime.Now,
                    ExpenseService = this,
                    CreatedBy = CurrentUser,
                };
                ListPendingExpenseHistory.Add(expenseHistory);
                oldValue = newValue;
            }
        }
    }
}
