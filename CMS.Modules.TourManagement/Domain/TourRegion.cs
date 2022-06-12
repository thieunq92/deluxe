
using System;
using System.Collections;
using System.Web.UI.WebControls;

namespace CMS.Modules.TourManagement.Domain
{
	#region TourRegion

	/// <summary>
	/// TourRegion object for NHibernate mapped table 'tmb_TourRegion'.
	/// </summary>
	public class TourRegion 
		{
		#region Static Columns Name
		
		public static string NAME = "Name";
	    public static string ORDER = "Order";
	    public static string PARENT = "Parent";
	    public static string DESCRIPTION = "Description";
		
		#endregion
			
		#region Member Variables
		
		protected int _id;
		protected string _name;
	    protected int _order;
	    protected TourRegion _parent;
	    protected IList _children;
	    protected string _description;

		#endregion

		#region Constructors

		public TourRegion() { 
				_id=-1;
			}

		public TourRegion( string name )
		{
			this._name = name;
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

	    public virtual TourRegion Parent
	    {
            get { return _parent; }
            set { _parent = value; }
	    }

	    public virtual int Order
	    {
            get { return _order; }
            set { _order = value; }
	    }

	    public virtual string Description
	    {
            get { return _description; }
            set { _description = value; }
	    }

	    public virtual IList Children
	    {
            get
            {
                if (_children == null)
                {
                    _children = new ArrayList();
                }
                return _children;
            }
            set { _children = value; }
	    }

		#endregion
		
	}

	#endregion

}
