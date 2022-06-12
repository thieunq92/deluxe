using System;
using CMS.Modules.TourManagement.Domain;

namespace CMS.Modules.TourManagement.Domain
{

    #region TourPrice

    /// <summary>
    /// TourPrice object for NHibernate mapped table 'tmt_TourPrice'.
    /// </summary>
    public class TourPrice
    {
        #region Static Columns Name

        public static string CURRENCYID = "CurrencyId";
        public static string EXTRAFEE = "ExtraFee";
        public static string LASTCACULATEDATE = "LastCaculateDate";
        public static string TOTALNET = "TotalNet";
        public static string TOURNETBOATPRICE = "TourNetBoatPrice";
        public static string TOURNETENTRANCEFEEPRICE = "TourNetEntranceFeePrice";
        public static string TOURNETGUIDESPRICE = "TourNetGuidesPrice";
        public static string TOURNETHOTELPRICE = "TourNetHotelPrice";
        public static string TOURNETMEALPRICE = "TourNetMealPrice";
        public static string TOURNETOTHERPRICE = "TourNetOtherPrice";
        public static string TOURNETTRANSPORTPRICE = "TourNetTransportPrice";

        #endregion

        #region Member Variables

        protected Currency _currency;
        protected double _extraFee;
        protected int _id;
        protected DateTime _lastCaculateDate;
        protected double _totalNet;
        protected Tour _tour;
        protected double _tourNetBoatPrice;
        protected double _tourNetEntranceFeePrice;
        protected double _tourNetGuidesPrice;
        protected double _tourNetHotelPrice;
        protected double _tourNetMealPrice;
        protected double _tourNetOtherPrice;
        protected double _tourNetTransportPrice;
        protected int _numberOfCustomers;

        #endregion

        #region Constructors

        public TourPrice()
        {
            _id = -1;
        }

        public TourPrice(double tourNetHotelPrice, double tourNetTransportPrice, double tourNetMealPrice,
                         double tourNetGuidesPrice, double tourNetEntranceFeePrice, double tourNetOtherPrice,
                         double tourNetBoatPrice, Currency currencyId, DateTime lastCaculateDate, double totalNet,
                         double extraFee, Tour tour)
        {
            _tourNetHotelPrice = tourNetHotelPrice;
            _tourNetTransportPrice = tourNetTransportPrice;
            _tourNetMealPrice = tourNetMealPrice;
            _tourNetGuidesPrice = tourNetGuidesPrice;
            _tourNetEntranceFeePrice = tourNetEntranceFeePrice;
            _tourNetOtherPrice = tourNetOtherPrice;
            _tourNetBoatPrice = tourNetBoatPrice;
            _currency = currencyId;
            _lastCaculateDate = lastCaculateDate;
            _totalNet = totalNet;
            _extraFee = extraFee;
            _tour = tour;
        }

        #endregion

        #region Public Properties

        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual double TourNetHotelPrice
        {
            get { return _tourNetHotelPrice; }
            set { _tourNetHotelPrice = value; }
        }

        public virtual double TourNetTransportPrice
        {
            get { return _tourNetTransportPrice; }
            set { _tourNetTransportPrice = value; }
        }

        public virtual double TourNetMealPrice
        {
            get { return _tourNetMealPrice; }
            set { _tourNetMealPrice = value; }
        }

        public virtual double TourNetGuidesPrice
        {
            get { return _tourNetGuidesPrice; }
            set { _tourNetGuidesPrice = value; }
        }

        public virtual double TourNetEntranceFeePrice
        {
            get { return _tourNetEntranceFeePrice; }
            set { _tourNetEntranceFeePrice = value; }
        }

        public virtual double TourNetOtherPrice
        {
            get { return _tourNetOtherPrice; }
            set { _tourNetOtherPrice = value; }
        }

        public virtual double TourNetBoatPrice
        {
            get { return _tourNetBoatPrice; }
            set { _tourNetBoatPrice = value; }
        }

        public virtual Currency Currency
        {
            get { return _currency; }
            set { _currency = value; }
        }

        public virtual DateTime LastCaculateDate
        {
            get { return _lastCaculateDate; }
            set { _lastCaculateDate = value; }
        }

        public virtual double TotalNet
        {
            get { return _totalNet; }
            set { _totalNet = value; }
        }

        public virtual double ExtraFee
        {
            get { return _extraFee; }
            set { _extraFee = value; }
        }

        public virtual Tour Tour
        {
            get { return _tour; }
            set { _tour = value; }
        }

        public virtual int NumberOfCustomers
        {
            get { return _numberOfCustomers; }
            set { _numberOfCustomers = value; }
        }

        #endregion
    }

    #endregion
}