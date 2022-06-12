using System;
using Portal.Modules.OrientalSails.Web.Util;

namespace Portal.Modules.OrientalSails.Domain
{
	#region ExtraOption

	/// <summary>
	/// ExtraOption object for NHibernate mapped table 'os_ExtraOption'.
	/// </summary>
	public class SailsPriceTable 
		{
		#region Static Columns Name
		
		public static string TRIP = "Trip";
	    public static string OPTION = "TripOption";
	    public static string STARTDATE = "StartDate";
	    public static string ENDDATE = "EndDate";
		
		#endregion
			
		#region Member Variables
		
		protected int _id;
        protected DateTime _startDate;
        protected DateTime _endDate;
        protected string _note;
        protected SailsTrip _trip;
        protected TripOption _tripOption;
	    protected Cruise _cruise;
	    protected Agency _agency;
		#endregion

		#region Constructors

		public SailsPriceTable() { 
				_id=-1;
			}

		#endregion

		#region Public Properties

		public virtual int Id
		{
			get {return _id;}
			set {_id = value;}
		}

	    public virtual DateTime StartDate
	    {
            get { return _startDate; }
            set { _startDate = value; }
	    }

	    public virtual DateTime EndDate
	    {
            get { return _endDate; }
            set { _endDate = value; }
	    }

	    public virtual string Note
	    {
            get { return _note; }
            set { _note = value; }
	    }

        public virtual SailsTrip Trip
        {
            get { return _trip; }
            set { _trip = value; }
        }

        public virtual TripOption TripOption
        {
            get { return _tripOption; }
            set { _tripOption = value; }
        }

	    public virtual Cruise Cruise
	    {
            get { return _cruise; }
            set { _cruise = value; }
	    }

	    public virtual Agency Agency
	    {
            get { return _agency; }
            set { _agency = value; }
	    }

		#endregion
		
	}

	#endregion

}
