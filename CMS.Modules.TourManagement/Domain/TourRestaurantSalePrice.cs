using System;

namespace CMS.Modules.TourManagement.Domain
{
	#region TourRestaurantSalePrice

	/// <summary>
	/// TourRestaurantSalePrice object for NHibernate mapped table 'tmt_TourRestaurantSalePrice'.
	/// </summary>
	public class TourRestaurantSalePrice 
		{
		#region Static Columns Name
		
		public static string ROLEID = "RoleId";
		public static string NUMBEROFCUSTOMERS = "NumberOfCustomers";
		public static string TOURRESTAURANTID = "TourRestaurantId";
		public static string TOTALNETPRICE = "TotalNetPrice";
		public static string TOTALSALEPRICE = "TotalSalePrice";
		public static string LASTCALCULATED = "LastCalculated";
		
		#endregion
			
		#region Member Variables
		
		protected int _id;
		protected int _roleId;
		protected int _numberOfCustomers;
		protected int _tourRestaurantId;
		protected decimal _totalNetPrice;
		protected decimal _totalSalePrice;
		protected DateTime _lastCalculated;

		#endregion

		#region Constructors

		public TourRestaurantSalePrice() { 
				_id=-1;
			}

		public TourRestaurantSalePrice( int roleId, int numberOfCustomers, int tourRestaurantId, decimal totalNetPrice, decimal totalSalePrice, DateTime lastCalculated )
		{
			this._roleId = roleId;
			this._numberOfCustomers = numberOfCustomers;
			this._tourRestaurantId = tourRestaurantId;
			this._totalNetPrice = totalNetPrice;
			this._totalSalePrice = totalSalePrice;
			this._lastCalculated = lastCalculated;
		}

		#endregion

		#region Public Properties

		public virtual int Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public virtual int RoleId
		{
			get { return _roleId; }
			set { _roleId = value; }
		}

		public virtual int NumberOfCustomers
		{
			get { return _numberOfCustomers; }
			set { _numberOfCustomers = value; }
		}

		public virtual int TourRestaurantId
		{
			get { return _tourRestaurantId; }
			set { _tourRestaurantId = value; }
		}

		public virtual decimal TotalNetPrice
		{
			get { return _totalNetPrice; }
			set { _totalNetPrice = value; }
		}

		public virtual decimal TotalSalePrice
		{
			get { return _totalSalePrice; }
			set { _totalSalePrice = value; }
		}

		public virtual DateTime LastCalculated
		{
			get { return _lastCalculated; }
			set { _lastCalculated = value; }
		}


		#endregion
		
	}

	#endregion
}
