using System;
using Portal.Modules.OrientalSails.Web.Util;

namespace Portal.Modules.OrientalSails.Domain
{

    #region SailsPriceConfig

    /// <summary>
    /// SailsPriceConfig object for NHibernate mapped table 'os_SailsPriceConfig'.
    /// </summary>
    public class  SailsPriceConfig
    {
        #region Static Columns Name

        public static string NETPRICE = "NetPrice";
        public static string SPECIALPRICE = "SpecialPrice";
        public static string TRIPOPTION = "TripOption";
        public static string ROOMTYPE = "RoomType";
        public static string ROOMCLASS = "RoomClass";
        public static string TRIP = "Trip";
        public static string TABLE = "Table";
        #endregion

        #region Member Variables

        protected int _id;
        //protected double _netPrice;
        //protected double _netPriceVND;
        protected double _priceForChildren;
        protected RoomClass _roomClass;
        protected RoomTypex _roomType;
        protected SailsTrip _trip;
        protected TripOption _tripOption;
        protected SailsPriceTable _table;

        #endregion

        #region Constructors

        public SailsPriceConfig()
        {
            _id = -1;
        }

        public SailsPriceConfig(TripOption tripOption, double netPrice, double priceForChildren, RoomClass roomClass,
                                RoomTypex roomType, SailsTrip trip, double netPriceVND)
        {
            _tripOption = tripOption;
            //_netPrice = netPrice;
            _priceForChildren = priceForChildren;
            _roomClass = roomClass;
            _roomType = roomType;
            _trip = trip;
            //_netPriceVND = netPriceVND;
        }

        #endregion

        #region Public Properties

        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual TripOption TripOption
        {
            get { return _tripOption; }
            set { _tripOption = value; }
        }

        //public virtual double NetPrice
        //{
        //    get { return _netPrice; }
        //    set { _netPrice = value; }
        //}

        //public virtual double NetPriceVND
        //{
        //    get { return _netPriceVND; }
        //    set { _netPriceVND = value; }
        //}

        public virtual double SpecialPrice
        {
            get { return _priceForChildren; }
            set { _priceForChildren = value; }
        }

        public virtual RoomClass RoomClass
        {
            get { return _roomClass; }
            set { _roomClass = value; }
        }

        public virtual RoomTypex RoomType
        {
            get { return _roomType; }
            set { _roomType = value; }
        }

        public virtual SailsTrip Trip
        {
            get { return _trip; }
            set { _trip = value; }
        }

        public virtual SailsPriceTable Table
        {
            get { return _table; }
            set { _table = value; }
        }

        public virtual double PriceAdultUSD { set; get; }
        public virtual double PriceAdultVND { set; get; }
        public virtual double PriceChildUSD { set; get; }
        public virtual double PriceChildVND { set; get; }
        public virtual double PriceBabyUSD { set; get; }
        public virtual double PriceBabyVND { set; get; }
        public virtual DateTime ValidFrom { set; get; }

        #endregion
    }

    #endregion
}