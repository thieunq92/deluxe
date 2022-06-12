
using System;
using System.Collections;
using System.Web.UI.WebControls;

namespace CMS.Modules.TourManagement.Domain
{
	#region TourService

	/// <summary>
	/// TourService object for NHibernate mapped table 'tmt_TourService'.
	/// </summary>
	public class TourService 
		{
		#region Static Columns Name
		
		public static string NAME = "Name";
		public static string TYPE = "Type";
		public static string PROVIDERID = "ProviderId";
		public static string SERVICETYPEID = "ServiceTypeId";
		public static string SERVICETYPE = "ServiceType";
		public static string SERVICECATEGORYID = "ServiceCategoryId";
		public static string SERVICECATEGOGY = "ServiceCategogy";
		public static string SPECIAL = "Special";
	    public static string TOUR = "Tour";
	    public static string DAY = "Day";
		
		#endregion
			
		#region Member Variables
		
		protected int _id;
		protected string _name;
		protected string _type;
		protected int _providerId;
		protected int _serviceTypeId;
		protected string _serviceType;
		protected int _serviceCategoryId;
		protected string _serviceCategogy;
		protected string _special;
	    protected Tour _tour;
	    protected int _day;

		#endregion

		#region Constructors

		public TourService() { 
				_id=-1;
			}
		#endregion

		#region Public Properties

		public virtual int Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public virtual string Name
		{
			get { return _name; }
			set
			{
				if ( value != null && value.Length > 200)
					throw new ArgumentOutOfRangeException("Invalid value for Name", value, value.ToString());
				_name = value;
			}
		}

		public virtual string Type
		{
			get { return _type; }
			set
			{
				if ( value != null && value.Length > 20)
					throw new ArgumentOutOfRangeException("Invalid value for Type", value, value.ToString());
				_type = value;
			}
		}

		public virtual int ProviderId
		{
			get { return _providerId; }
			set { _providerId = value; }
		}

		public virtual int ServiceTypeId
		{
			get { return _serviceTypeId; }
			set { _serviceTypeId = value; }
		}

		public virtual string ServiceType
		{
			get { return _serviceType; }
			set
			{
				if ( value != null && value.Length > 100)
					throw new ArgumentOutOfRangeException("Invalid value for ServiceType", value, value.ToString());
				_serviceType = value;
			}
		}

		public virtual int ServiceCategoryId
		{
			get { return _serviceCategoryId; }
			set { _serviceCategoryId = value; }
		}

		public virtual string ServiceCategogy
		{
			get { return _serviceCategogy; }
			set
			{
				if ( value != null && value.Length > 100)
					throw new ArgumentOutOfRangeException("Invalid value for ServiceCategogy", value, value.ToString());
				_serviceCategogy = value;
			}
		}

		public virtual string Special
		{
			get { return _special; }
			set
			{
				if ( value != null && value.Length > 200)
					throw new ArgumentOutOfRangeException("Invalid value for Special", value, value.ToString());
				_special = value;
			}
		}

	    public virtual Tour Tour
	    {
            get { return _tour; }
            set { _tour = value; }
	    }

	    public virtual int Day
	    {
            get { return _day; }
            set { _day = value; }
	    }

		#endregion
		
	}

	#endregion

}
