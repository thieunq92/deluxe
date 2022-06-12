
using System;
using System.Collections;
using System.Web.UI.WebControls;

namespace Portal.Modules.OrientalSails.Domain
{
	#region CustomerExtraOption

	/// <summary>
	/// CustomerExtraOption object for NHibernate mapped table 'os_CustomerExtraOption'.
	/// </summary>
	public class BookingServicePrice 
		{
		#region Static Columns Name

	    public static string CUSTOMER = "Booking";
	    public static string EXTRAOPTION = "ExtraOption";
		#endregion
			
		#region Member Variables
		
		protected int _id;
		protected Booking _booking;
		protected ExtraOption _extraOption;
        protected double _unitPrice;

		#endregion

		#region Constructors

        public BookingServicePrice()
        { 
				_id=-1;
			}

		#endregion

		#region Public Properties

		public virtual int Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public virtual Booking Booking
		{
			get { return _booking; }
			set { _booking = value; }
		}

		public virtual ExtraOption ExtraOption
		{
			get { return _extraOption; }
			set { _extraOption = value; }
		}

        public virtual double UnitPrice
        {
            get { return _unitPrice; }
            set { _unitPrice = value; }
        }

		#endregion
		
	}

	#endregion
}
