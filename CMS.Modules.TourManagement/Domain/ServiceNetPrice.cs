
using System;
using System.Collections;
using System.Web.UI.WebControls;

namespace CMS.Modules.TourManagement.Domain
{
	#region ServiceNetPrice

	/// <summary>
	/// ServiceNetPrice object for NHibernate mapped table 'tmt_ServiceNetPrice'.
	/// </summary>
	public class ServiceNetPrice 
		{
		#region Static Columns Name
		
		public static string SERVICEID = "ServiceId";
		public static string NETPRICE = "NetPrice";
		public static string CURRENCYID = "CurrencyId";
		public static string NUMBEROFCUSTOMER = "NumberOfCustomer";
		
		#endregion
			
		#region Member Variables
		
		protected int _id;
		protected int _serviceId;
		protected int _netPrice;
		protected int _currencyId;
		protected int _numberOfCustomer;

		#endregion

		#region Constructors

		public ServiceNetPrice() { 
				_id=-1;
			}

		public ServiceNetPrice( int serviceId, int netPrice, int currencyId, int numberOfCustomer )
		{
			this._serviceId = serviceId;
			this._netPrice = netPrice;
			this._currencyId = currencyId;
			this._numberOfCustomer = numberOfCustomer;
		}

		#endregion

		#region Public Properties

		public virtual int Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public virtual int ServiceId
		{
			get { return _serviceId; }
			set { _serviceId = value; }
		}

		public virtual int NetPrice
		{
			get { return _netPrice; }
			set { _netPrice = value; }
		}

		public virtual int CurrencyId
		{
			get { return _currencyId; }
			set { _currencyId = value; }
		}

		public virtual int NumberOfCustomer
		{
			get { return _numberOfCustomer; }
			set { _numberOfCustomer = value; }
		}


		#endregion
		
	}

	#endregion

}
