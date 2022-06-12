namespace CMS.Modules.TourManagement.Domain
{

    #region TourRestaurantPriceConfig

    /// <summary>
    /// TourRestaurantPriceConfig object for NHibernate mapped table 'tmt_TourRestaurantPriceConfig'.
    /// </summary>
    public class TourRestaurantPriceConfig
    {
        #region Static Columns Name

        public static string CURRENCYID = "CurrencyId";
        public static string MEALTYPEID = "MealTypeId";
        public static string NETPRICE = "NetPrice";
        public static string RESTAURANTID = "RestaurantId";
        public static string TOURID = "TourId";
        public static string CUSTOMERFROM = "CustomerFrom";
        public static string CUSTOMERTO = "CustomerTo";

        #endregion

        #region Member Variables

        protected int _currencyId;
        protected int _id;
        protected int _mealTypeId;
        protected decimal _netPrice;
        protected int _numberOfMeal;
        protected int _restaurantId;
        protected int _tourId;
        protected int _customerFrom;
        protected int _customerTo;

        #endregion

        #region Constructors

        public TourRestaurantPriceConfig()
        {
            _id = -1;
        }

        public TourRestaurantPriceConfig(int tourId, int restaurantId, int mealTypeId, int currencyId, decimal netPrice)
        {
            _tourId = tourId;
            _restaurantId = restaurantId;
            _mealTypeId = mealTypeId;
            _currencyId = currencyId;
            _netPrice = netPrice;
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

        public virtual int RestaurantId
        {
            get { return _restaurantId; }
            set { _restaurantId = value; }
        }

        public virtual int MealTypeId
        {
            get { return _mealTypeId; }
            set { _mealTypeId = value; }
        }

        public virtual int CurrencyId
        {
            get { return _currencyId; }
            set { _currencyId = value; }
        }

        public virtual decimal NetPrice
        {
            get { return _netPrice; }
            set { _netPrice = value; }
        }

        public virtual int NumberOfMeal
        {
            get { return _numberOfMeal; }
            set { _numberOfMeal = value; }
        }

        public virtual int CustomerFrom
        {
            get { return _customerFrom; }
            set { _customerFrom = value; }
        }

        public virtual int CustomerTo
        {
            get { return _customerTo; }
            set { _customerTo = value; }
        }

        #endregion
    }

    #endregion
}