
using System;
using System.Collections;
using System.Web.UI.WebControls;

namespace CMS.Modules.TourManagement.Domain
{
	#region ServiceSalePrice

	/// <summary>
	/// ServiceSalePrice object for NHibernate mapped table 'tmt_ServiceSalePrice'.
	/// </summary>
	public class ServiceSalePrice 
		{
		#region Static Columns Name
		
		public static string SERVICEID = "ServiceId";
		public static string ROLEID = "RoleId";
		public static string SALEPRICE = "SalePrice";
		public static string CURRENCYID = "CurrencyId";
		public static string NUMBEROFCUSTOMER = "NumberOfCustomer";
		
		#endregion
			
		#region Member Variables
		
		protected int _id;
		protected int _serviceId;
		protected int _roleId;
		protected decimal _salePrice;
		protected int _currencyId;
		protected int _numberOfCustomer;

		#endregion

		#region Constructors

		public ServiceSalePrice() { 
				_id=-1;
			}

		public ServiceSalePrice( int serviceId, int roleId, decimal salePrice, int currencyId, int numberOfCustomer )
		{
			this._serviceId = serviceId;
			this._roleId = roleId;
			this._salePrice = salePrice;
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

		public virtual int RoleId
		{
			get { return _roleId; }
			set { _roleId = value; }
		}

		public virtual decimal SalePrice
		{
			get { return _salePrice; }
			set { _salePrice = value; }
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
