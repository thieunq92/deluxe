
using System;
using System.Collections;
using System.Web.UI.WebControls;

namespace CMS.Modules.TourManagement.Domain
{
	#region TourTransportRent

	/// <summary>
	/// TourTransportRent object for NHibernate mapped table 'tmt_TourTransportRent'.
	/// </summary>
	public class TourTransportRent 
		{
		#region Static Columns Name
		
		public static string TRANSPORTID = "TransportId";
		public static string TOURID = "TourId";
		public static string DURATION = "Duration";
		public static string PROVIDERID = "ProviderId";
		public static string NETPRICE = "NetPrice";
		public static string CURRENCYID = "CurrencyId";
		
		#endregion
			
		#region Member Variables
		
		protected int _id;
		protected int _transportId;
		protected int _tourId;
		protected int _duration;
		protected int _providerId;
		protected decimal _netPrice;
		protected int _currencyId;

		#endregion

		#region Constructors

		public TourTransportRent() { 
				_id=-1;
			}

		public TourTransportRent( int transportId, int tourId, int duration, int providerId, decimal netPrice, int currencyId )
		{
		    _id = -1;
			this._transportId = transportId;
			this._tourId = tourId;
			this._duration = duration;
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

		public virtual int TransportId
		{
			get { return _transportId; }
			set { _transportId = value; }
		}

		public virtual int TourId
		{
			get { return _tourId; }
			set { _tourId = value; }
		}

		public virtual int Duration
		{
			get { return _duration; }
			set { _duration = value; }
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
