using System;
using System.Collections;
using System.Web.UI.WebControls;

namespace CMS.Modules.TourManagement.Domain
{
	#region TourHotelSalePrice

	/// <summary>
	/// TourHotelSalePrice object for NHibernate mapped table 'tmt_TourHotelSalePrice'.
	/// </summary>
	public class TourBoatSalePrice
		{
		#region Member Variables
		
		protected int _id;
		protected int _roleId;
		protected int _numberOfCustomer;
		protected decimal _totalNetPrice;
		protected decimal _totalSalePrice;
	    protected DateTime _lastCalculated;
	    protected int _tourHotelId;

		#endregion

		#region Constructors

		public TourBoatSalePrice() 
		{
			_id = -1;
		}

        public TourBoatSalePrice(int roleId, int numberOfCustomer, decimal totalNetPrice, decimal totalSalePrice)
		{
			this._roleId = roleId;
			this._numberOfCustomer = numberOfCustomer;
			this._totalNetPrice = totalNetPrice;
			this._totalSalePrice = totalSalePrice;
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

		public virtual int NumberOfCustomer
		{
			get { return _numberOfCustomer; }
			set { _numberOfCustomer = value; }
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
        
	    public virtual int TourBoatId
	    {
            get { return _tourHotelId; }
            set { _tourHotelId = value; }
	    }

		#endregion
	}

	#endregion
}
