
using System;
using System.Collections;
using System.Web.UI.WebControls;

namespace Portal.Modules.OrientalSails.Domain
{
	#region CustomerExtraOption

	/// <summary>
	/// CustomerExtraOption object for NHibernate mapped table 'os_CustomerExtraOption'.
	/// </summary>
	public class TripCostType 
		{
		#region Static Columns Name

	    public static string CUSTOMER = "Booking";
	    public static string EXTRAOPTION = "ExtraOption";
		#endregion
			
		#region Member Variables
		
		protected int _id;
		protected SailsTrip _trip;
		protected CostType _costType;

		#endregion

		#region Constructors

		public TripCostType() { 
				_id=-1;
		}

		#endregion

		#region Public Properties

		public virtual int Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public virtual SailsTrip Trip
		{
			get { return _trip; }
			set { _trip = value; }
		}

		public virtual CostType CostType
		{
			get { return _costType; }
			set { _costType = value; }
		}


		#endregion
		
	}

	#endregion

}
