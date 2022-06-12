namespace CMS.Modules.TourManagement.Domain
{

    #region TourEntranceFeePriceConfig

    /// <summary>
    /// TourEntranceFeePriceConfig object for NHibernate mapped table 'tmt_TourEntranceFeePriceConfig'.
    /// </summary>
    public class TourEntranceFeePriceConfig
    {
        #region Static Columns Name

        public static string CURRENCYID = "CurrencyId";
        public static string ENTRANCEFEETYPE = "EntranceFeeType";
        public static string LANSCAPEID = "LanscapeId";
        public static string NETPRICE = "NetPrice";
        public static string NUMBEROFCUSTOMER = "NumberOfCustomer";
        public static string SALEPRICE = "SalePrice";
        public static string TOURID = "TourId";

        #endregion

        #region Member Variables

        protected int _currencyId;
        protected int _entranceFeeType;
        protected int _id;
        protected int _lanscapeId;
        protected double _netPrice;
        protected int _numberOfCustomer;
        protected double _salePrice;
        protected int _tourId;

        #endregion

        #region Constructors

        public TourEntranceFeePriceConfig()
        {
            _id = -1;
        }

        public TourEntranceFeePriceConfig(int tourId, int lanscapeId, int entranceFeeType, double netPrice,
                                          double salePrice, int numberOfCustomer, int currencyId)
        {
            _tourId = tourId;
            _lanscapeId = lanscapeId;
            _entranceFeeType = entranceFeeType;
            _netPrice = netPrice;
            _salePrice = salePrice;
            _numberOfCustomer = numberOfCustomer;
            _currencyId = currencyId;
        }

        #endregion

        #region Public Properties

        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual int TourId
        {
            get { return _tourId; }
            set { _tourId = value; }
        }

        public virtual int LanscapeId
        {
            get { return _lanscapeId; }
            set { _lanscapeId = value; }
        }

        public virtual int EntranceFeeType
        {
            get { return _entranceFeeType; }
            set { _entranceFeeType = value; }
        }

        public virtual double NetPrice
        {
            get { return _netPrice; }
            set { _netPrice = value; }
        }

        public virtual double SalePrice
        {
            get { return _salePrice; }
            set { _salePrice = value; }
        }

        public virtual int NumberOfCustomer
        {
            get { return _numberOfCustomer; }
            set { _numberOfCustomer = value; }
        }

        public virtual int CurrencyId
        {
            get { return _currencyId; }
            set { _currencyId = value; }
        }

        #endregion
    }

    #endregion
}