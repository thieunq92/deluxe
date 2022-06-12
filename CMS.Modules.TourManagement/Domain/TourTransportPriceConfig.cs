
using System;
using System.Collections;
using System.Web.UI.WebControls;

namespace CMS.Modules.TourManagement.Domain
{
    #region TourTransportPriceConfig

    /// <summary>
    /// TourTransportPriceConfig object for NHibernate mapped table 'tmt_TourTransportPriceConfig'.
    /// </summary>
    public class TourTransportPriceConfig
    {
        #region Static Columns Name

        public static string TRANSPORTID = "TransportId";
        public static string TRANSPORTTYPEID = "TransportTypeId";
        public static string PROVIDERID = "ProviderId";
        public static string ROUTEID = "RouteId";
        public static string NETPRICE = "NetPrice";
        public static string CURRENCYID = "CurrencyId";
        public static string TOURID = "TourId";
        public static string CUSTOMERFROM = "CustomerFrom";
        public static string CUSTOMERTO = "CustomerTo";
        public static string SIZE = "Size";

        #endregion

        #region Member Variables

        protected int _id;
        protected int _transportId;
        protected int _transportTypeId;
        protected int _providerId;
        protected int _routeId;
        protected decimal _netPrice;
        protected int _currencyId;
        protected int _tourId;
        protected int _customerFrom;
        protected int _customerTo;
        protected int _size;

        #endregion

        #region Constructors

        public TourTransportPriceConfig()
        {
            _id = -1;
        }

        public TourTransportPriceConfig(int transportId, int transportTypeId, int providerId, int routeId, decimal netPrice, int currencyId,int tourId)
        {
            this._transportId = transportId;
            this._transportTypeId = transportTypeId;
            this._providerId = providerId;
            this._routeId = routeId;
            this._netPrice = netPrice;
            this._currencyId = currencyId;
            _tourId = tourId;
        }

        #endregion

        #region Public Properties

        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual int TransportId
        {
            get { return _transportId; }
            set { _transportId = value; }
        }

        public virtual int TransportTypeId
        {
            get { return _transportTypeId; }
            set { _transportTypeId = value; }
        }

        public virtual int ProviderId
        {
            get { return _providerId; }
            set { _providerId = value; }
        }

        public virtual int TourId
        {
            get { return _tourId; }
            set{ _tourId = value;}
        }

        public virtual int RouteId
        {
            get { return _routeId; }
            set { _routeId = value; }
        }

        public virtual decimal NetPrice
        {
            get { return _netPrice; }
            set { _netPrice = value; }
        }

        public virtual int CurrencyId
        {
            get { return _currencyId; }
            set { _currencyId = value; }
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

        public virtual int Size
        {
            get
            {
                if (_size > 0)
                {
                    return _size;
                }
                return 1;
            }
            set { _size = value; }
        }
        #endregion

    }

    #endregion

}
