using System;

namespace CMS.Modules.TourManagement.Domain
{

    #region TourSalePrice

    /// <summary>
    /// TourSalePrice object for NHibernate mapped table 'tmt_TourSalePrice'.
    /// </summary>
    public class TourSalePrice
    {
        #region Static Columns Name

        public static string BOATPRICE = "BoatPrice";
        public static string CURRENCYID = "CurrencyId";
        public static string ENTRANCEFEEPRICE = "EntranceFeePrice";
        public static string EXTRAFEE = "ExtraFee";
        public static string GUIDEPRICE = "GuidePrice";
        public static string HOTELPRICE = "HotelPrice";
        public static string LASTCALCULATEDATE = "LastCalculateDate";
        public static string MEALPRICE = "MealPrice";
        public static string NUMBEROFCUSTOMERS = "NumberOfCustomers";
        public static string OTHERPRICE = "OtherPrice";
        public static string ROLEID = "RoleId";
        public static string TOTAL = "Total";
        public static string TOURID = "TourId";
        public static string TRANSPORTPRICE = "TransportPrice";

        #endregion

        #region Member Variables

        protected Double _boatPrice;
        protected Currency _currency;
        protected Double _entranceFeePrice;
        protected Double _extraFee;
        protected Double _guidePrice;
        protected Double _hotelPrice;
        protected int _id;
        protected DateTime _lastCalculateDate;
        protected Double _mealPrice;
        protected int _numberOfCustomers;
        protected Double _otherPrice;
        protected int _roleId;
        protected Double _total;
        protected Tour _tour;
        protected Double _transportPrice;

        #endregion

        #region Constructors

        public TourSalePrice()
        {
            _id = -1;
        }

        public TourSalePrice(Tour tourId, Double hotelPrice, Double transportPrice, Double mealPrice,
                             Double guidePrice, Double entranceFeePrice, Double boatPrice, Double otherPrice,
                             Double total, Double extraFee, int numberOfCustomers, DateTime lastCalculateDate,
                             Currency currencyId, int roleId)
        {
            _tour = tourId;
            _hotelPrice = hotelPrice;
            _transportPrice = transportPrice;
            _mealPrice = mealPrice;
            _guidePrice = guidePrice;
            _entranceFeePrice = entranceFeePrice;
            _boatPrice = boatPrice;
            _otherPrice = otherPrice;
            _total = total;
            _extraFee = extraFee;
            _numberOfCustomers = numberOfCustomers;
            _lastCalculateDate = lastCalculateDate;
            _currency = currencyId;
            _roleId = roleId;
        }

        #endregion

        #region Public Properties

        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual Tour Tour
        {
            get { return _tour; }
            set { _tour = value; }
        }

        public virtual Double HotelPrice
        {
            get { return _hotelPrice; }
            set { _hotelPrice = value; }
        }

        public virtual Double TransportPrice
        {
            get { return _transportPrice; }
            set { _transportPrice = value; }
        }

        public virtual Double MealPrice
        {
            get { return _mealPrice; }
            set { _mealPrice = value; }
        }

        public virtual Double GuidePrice
        {
            get { return _guidePrice; }
            set { _guidePrice = value; }
        }

        public virtual Double EntranceFeePrice
        {
            get { return _entranceFeePrice; }
            set { _entranceFeePrice = value; }
        }

        public virtual Double BoatPrice
        {
            get { return _boatPrice; }
            set { _boatPrice = value; }
        }

        public virtual Double OtherPrice
        {
            get { return _otherPrice; }
            set { _otherPrice = value; }
        }

        public virtual Double Total
        {
            get { return _total; }
            set { _total = value; }
        }

        public virtual Double ExtraFee
        {
            get { return _extraFee; }
            set { _extraFee = value; }
        }

        public virtual int NumberOfCustomers
        {
            get { return _numberOfCustomers; }
            set { _numberOfCustomers = value; }
        }

        public virtual DateTime LastCalculateDate
        {
            get { return _lastCalculateDate; }
            set { _lastCalculateDate = value; }
        }

        public virtual Currency Currency
        {
            get { return _currency; }
            set { _currency = value; }
        }

        public virtual int RoleId
        {
            get { return _roleId; }
            set { _roleId = value; }
        }

        #endregion
    }

    #endregion
}