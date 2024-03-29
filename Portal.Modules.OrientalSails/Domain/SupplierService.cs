namespace Portal.Modules.OrientalSails.Domain
{
    #region Customer

    ///// <summary>
    ///// Customer object for NHibernate mapped table 'os_Customer'.
    ///// </summary>
    //public class SupplierService
    //{
    //    #region Static Columns Name

    //    public static string BIRTHDAY = "Birthday";
    //    public static string BOOKING = "Booking";
    //    public static string COUNTRY = "Country";
    //    public static string FULLNAME = "Fullname";
    //    public static string PASSPORT = "Passport";
    //    public static string BOOKINGROOM = "BookingRoom";

    //    #endregion

    //    #region Member Variables

    //    protected int _id;
    //    protected string _name;
    //    protected string _phone;
    //    protected Agency _agency;
    //    protected double _cost;
    //    protected ServiceType _serviceType;
    //    protected SailExpense _expense;
    //    protected bool _isRemoved;

    //    #endregion

    //    #region Constructors

    //    public SupplierService()
    //    {
    //        _id = -1;
    //    }

    //    #endregion

    //    #region Public Properties

    //    public virtual int Id
    //    {
    //        get { return _id; }
    //        set { _id = value; }
    //    }

    //    public virtual string Name
    //    {
    //        get { return _name; }
    //        set { _name = value; }
    //    }

    //    public virtual string Phone
    //    {
    //        get { return _phone; }
    //        set { _phone = value; }
    //    }

    //    public virtual Agency Agency
    //    {
    //        get { return _agency;}
    //        set { _agency = value;}
    //    }

    //    public virtual SailExpense Expense
    //    {
    //        get { return _expense; }
    //        set { _expense = value; }
    //    }

    //    public virtual double Cost
    //    {
    //        get { return _cost;}
    //        set { _cost = value;}
    //    }

    //    public virtual ServiceType ServiceType
    //    {
    //        get { return _serviceType; }
    //        set { _serviceType = value; }
    //    }

    //    /// <summary>
    //    /// Biến đánh dấu trạng thái xóa
    //    /// </summary>
    //    public virtual bool IsRemoved
    //    {
    //        get { return _isRemoved; }
    //        set { _isRemoved = value; }
    //    }
    //    #endregion
    //}

    public enum ServiceType
    {
        Guide,
        Transport,
        Operator,
        DayBoat,
        Others
    }

    #endregion
}
