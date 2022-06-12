
using System;
using System.Collections;
using System.Web.UI.WebControls;

namespace Portal.Modules.OrientalSails.Domain
{
	#region CustomerExtraOption

	/// <summary>
	/// CustomerExtraOption object for NHibernate mapped table 'os_CustomerExtraOption'.
	/// </summary>
	public class CruiseTrip 
		{
		#region Static Columns Name

	    public static string CUSTOMER = "Booking";
	    public static string EXTRAOPTION = "ExtraOption";
		#endregion
			
		#region Member Variables
		
		protected int _id;
		protected Cruise _cruise;
		protected SailsTrip _trip;

		#endregion

		#region Constructors

		public CruiseTrip() { 
				_id=-1;
			}

		#endregion

		#region Public Properties

		public virtual int Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public virtual Cruise Cruise
		{
			get { return _cruise; }
            set { _cruise = value; }
		}

		public virtual SailsTrip Trip
		{
			get { return _trip; }
            set { _trip = value; }
		}

		#endregion
		
	}

	#endregion

}
