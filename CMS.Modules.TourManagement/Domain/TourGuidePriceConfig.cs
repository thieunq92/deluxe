namespace CMS.Modules.TourManagement.Domain
{
	#region TourGuidePriceConfig

	/// <summary>
	/// TourGuidePriceConfig object for NHibernate mapped table 'tmt_TourGuidePriceConfig'.
	/// </summary>
	public class TourGuidePriceConfig 
		{
		#region Static Columns Name
		
		public static string TOURID = "TourId";
		public static string SERVICEID = "ServiceId";
		public static string LANGUAGEID = "LanguageId";
		public static string LOCATIONID = "LocationId";
		public static string PROVIDERID = "ProviderId";
		public static string NETPRICE = "NetPrice";
		public static string CURRENCYID = "CurrencyId";
		
		#endregion
			
		#region Member Variables
		
		protected int _id;
		protected int _tourId;
		protected int _serviceId;
		protected int _languageId;
		protected int _locationId;
		protected int _providerId;
		protected decimal _netPrice;
		protected int _currencyId;

		#endregion

		#region Constructors

		public TourGuidePriceConfig() { 
				_id=-1;
			}

		public TourGuidePriceConfig( int tourId, int serviceId, int languageId, int locationId, int providerId, decimal netPrice, int currencyId )
		{
			this._tourId = tourId;
			this._serviceId = serviceId;
			this._languageId = languageId;
			this._locationId = locationId;
			this._providerId = providerId;
			this._netPrice = netPrice;
			this._currencyId = currencyId;
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

		public virtual int ServiceId
		{
			get { return _serviceId; }
			set { _serviceId = value; }
		}

		public virtual int LanguageId
		{
			get { return _languageId; }
			set { _languageId = value; }
		}

		public virtual int LocationId
		{
			get { return _locationId; }
			set { _locationId = value; }
		}

		public virtual int ProviderId
		{
			get { return _providerId; }
			set { _providerId = value; }
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


		#endregion
		
	}

	#endregion

}
