
using System;
using System.Collections;
using System.Web.UI.WebControls;

namespace Portal.Modules.OrientalSails.Domain
{
	#region ExtraOption

	/// <summary>
	/// ExtraOption object for NHibernate mapped table 'os_ExtraOption'.
	/// </summary>
	public class ExtraOption 
		{
		#region Static Columns Name
		
		public static string NAME = "Name";
		public static string PRICE = "Price";
		public static string DESCRIPTION = "Description";
		
		#endregion
			
		#region Member Variables
		
		protected int _id;
	    protected bool _deleted;
		protected string _name;
		protected double _price;
		protected string _description;
        protected IList _extraOptionosCustomerExtraOptions;
	    protected bool _isIncluded;
	    protected ServiceTarget _target;

		#endregion

		#region Constructors

		public ExtraOption() { 
				_id=-1;
			}

		public ExtraOption( string name, double price, string description )
		{
			this._name = name;
			this._price = price;
			this._description = description;
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
				if ( value != null && value.Length > 250)
					throw new ArgumentOutOfRangeException("Invalid value for Name", value, value.ToString());
				_name = value;
			}
		}

        public virtual double Price
		{
			get { return _price; }
			set { _price = value; }
		}

		public virtual string Description
		{
			get { return _description; }
			set
			{
				if ( value != null && value.Length > 500)
					throw new ArgumentOutOfRangeException("Invalid value for Description", value, value.ToString());
				_description = value;
			}
		}

	    public virtual bool Deleted
	    {
	        get{return _deleted;}
            set { _deleted = value; }
	    }

	    public virtual bool IsIncluded
	    {
            get { return _isIncluded; }
            set { _isIncluded = value; }
	    }

	    public virtual ServiceTarget Target
	    {
            get { return _target; }
            set { _target = value; }
	    }
		#endregion
		
	}
	#endregion

}
