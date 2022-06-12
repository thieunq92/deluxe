
using System;
using System.Collections;
using System.Web.UI.WebControls;

namespace CMS.Modules.TourManagement.Domain
{
	#region TourBoatPriceConfig

	/// <summary>
	/// TourBoatPriceConfig object for NHibernate mapped table 'tmt_TourBoatPriceConfig'.
	/// </summary>
	[Serializable]
	public class TourBoatPriceConfig 
		{
		#region Static Columns Name
		
		public static string TOURID = "TourId";
		public static string PROVIDERID = "ProviderId";
		public static string BOATID = "BoatId";
		public static string TRIPID = "TripId";
		public static string ROOMTYPEID = "RoomTypeId";
		public static string ROOMCLASSID = "RoomClassId";
		public static string ROUTEID = "RouteId";
		public static string NETPRICE = "NetPrice";
		public static string CURRENCYID = "CurrencyId";
        public static string CAPACITY = "Capacity";
		
		#endregion
			
		#region Member Variables
		
		protected int _id;
		protected int _tourId;
		protected int _providerId;
		protected int _boatId;
		protected int _tripId;
		protected int _roomTypeId;
		protected int _roomClassId;
		protected int _routeId;
		protected decimal _netPrice;
		protected int _currencyId;
	    protected int _capacity;

		#endregion

		#region Constructors

		public TourBoatPriceConfig() { 
				_id=-1;
			}

        public TourBoatPriceConfig(int tourId, int providerId, int boatId, int tripId, int roomTypeId, int roomClassId, int routeId, decimal netPrice, int currencyId, int capacity)
		{
			this._tourId = tourId;
			this._providerId = providerId;
			this._boatId = boatId;
			this._tripId = tripId;
			this._roomTypeId = roomTypeId;
			this._roomClassId = roomClassId;
			this._routeId = routeId;
			this._netPrice = netPrice;
			this._currencyId = currencyId;
            _capacity = capacity;
		}

		#endregion

		#region Public Properties

		public virtual int Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public virtual int TourId
		{
			get { return _tourId; }
			set { _tourId = value; }
		}

		public virtual int ProviderId
		{
			get { return _providerId; }
			set { _providerId = value; }
		}

		public virtual int BoatId
		{
			get { return _boatId; }
			set { _boatId = value; }
		}

		public virtual int TripId
		{
			get { return _tripId; }
			set { _tripId = value; }
		}

		public virtual int RoomTypeId
		{
			get { return _roomTypeId; }
			set { _roomTypeId = value; }
		}

		public virtual int RoomClassId
		{
			get { return _roomClassId; }
			set { _roomClassId = value; }
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

	    public virtual int Capacity
	    {
            get { return _capacity; }
            set{ _capacity = value;}
	    }

	    public virtual int NumberOfDays
	    {
            get { return 1; }
	    }

		#endregion
		
	}

	#endregion

}
