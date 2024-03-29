namespace Portal.Modules.OrientalSails.Domain
{

    #region Booking

    /// <summary>
    /// Booking object for NHibernate mapped table 'os_Booking'.
    /// </summary>
    public class SailExpensePayment
    {
        #region Static Columns Name

        public const string CRUISE = "Cruise";
        public const string DATE = "Date";
        public const string GUIDE = "Guide";
        public const string KAYAING = "Kayaing";
        public const string MEAL = "Meal";
        public const string OTHERS = "Others";
        public const string SERVIVE = "Service";
        public const string TICKET = "Ticket";
        public const string TOTAL = "Total";
        public const string TRANSFER = "Transfer";

        #endregion

        #region Member Variables

        protected double _cruise;
        protected SailExpense _expense;
        protected double _guide;
        protected int _id;
        protected double _kayaing;
        protected double _meal;
        protected double _others;
        protected double _service;
        protected double _ticket;
        protected double _transfer;

        #endregion

        #region Constructors

        public SailExpensePayment()
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

        public virtual double Transfer
        {
            get { return _transfer; }
            set { _transfer = value; }
        }

        public virtual SailExpense Expense
        {
            get { return _expense; }
            set { _expense = value; }
        }

        public virtual double Ticket
        {
            get { return _ticket; }
            set { _ticket = value; }
        }

        public virtual double Guide
        {
            get { return _guide; }
            set { _guide = value; }
        }

        public virtual double Meal
        {
            get { return _meal; }
            set { _meal = value; }
        }

        public virtual double Kayaing
        {
            get { return _kayaing; }
            set { _kayaing = value; }
        }

        public virtual double Service
        {
            get { return _service; }
            set { _service = value; }
        }

        public virtual double Cruise
        {
            get { return _cruise; }
            set { _cruise = value; }
        }

        public virtual double Others
        {
            get { return _others; }
            set { _others = value; }
        }

        #endregion

        #region -- Calculated properties --

        public virtual double TotalPayment
        {
            get { return _transfer + _ticket + _guide + _meal + _kayaing + _service + _cruise + _others; }
        }

        //public virtual double TransferLeft
        //{
        //    get { return _expense.Transfer - _transfer; }
        //    set { _transfer = _expense.Transfer - value; }
        //}

        //public virtual double TicketLeft
        //{
        //    get { return _expense.Ticket - _ticket; }
        //    set { _ticket = _expense.Ticket - value; }
        //}

        //public virtual double GuideLeft
        //{
        //    get { return _expense.Guide - _guide; }
        //    set { _guide = _expense.Guide - value; }
        //}

        //public virtual double MealLeft
        //{
        //    get { return _expense.Meal - _meal; }
        //    set { _meal = _expense.Meal - value; }
        //}

        //public virtual double KayaingLeft
        //{
        //    get { return _expense.Kayaing - _kayaing; }
        //    set { _kayaing = _expense.Kayaing - value; }
        //}

        //public virtual double ServiceLeft
        //{
        //    get { return _expense.Service - _service; }
        //    set { _service = _expense.Service - value; }
        //}

        //public virtual double CruiseLeft
        //{
        //    get { return _expense.Cruise - _cruise; }
        //    set { _cruise = _expense.Cruise - value; }
        //}

        //public virtual double OthersLeft
        //{
        //    get { return _expense.Others - _others; }
        //    set { _others = _expense.Others - value; }
        //}

        //public virtual double TotalDebt
        //{
        //    get { return _expense.TotalCost - TotalPayment; }
        //}

        #endregion
    }

    #endregion
}