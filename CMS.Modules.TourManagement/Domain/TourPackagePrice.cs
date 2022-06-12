using System;

namespace CMS.Modules.TourManagement.Domain
{
	#region TourPackagePrice

	/// <summary>
	/// TourBoatPriceConfig object for NHibernate mapped table 'tmt_TourBoatPriceConfig'.
	/// </summary>
	[Serializable]
	public class TourPackagePrice 
		{
		#region Static Columns Name
		
        //public static string TOURID = "TourId";
        //public static string PROVIDERID = "ProviderId";
        //public static string BOATID = "BoatId";
        //public static string TRIPID = "TripId";
        //public static string ROOMTYPEID = "RoomTypeId";
        //public static string ROOMCLASSID = "RoomClassId";
        //public static string ROUTEID = "RouteId";
        //public static string NETPRICE = "NetPrice";
        //public static string CURRENCYID = "CurrencyId";
        //public static string CAPACITY = "Capacity";
		
		#endregion
			
		#region Member Variables
		
		protected int _id;
		protected Tour _tour;
		protected int _numberOfCustomers;
		protected decimal _netPrice;
		protected Currency _currency;

		#endregion

		#region Constructors

		public TourPackagePrice() { 
				_id=-1;
		}
		#endregion

		#region Public Properties

		public virtual int Id
		{
			get {return _id;}
			set {_id = value;}
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

		public virtual decimal NetPrice
		{
			get { return _netPrice; }
			set { _netPrice = value; }
		}

		public virtual Currency Currency
		{
			get { return _currency; }
			set { _currency = value; }
		}

		#endregion
		
	}

	#endregion

}
