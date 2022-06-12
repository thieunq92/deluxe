using System;
using System.Collections;

namespace CMS.Modules.TourManagement.Domain
{
	#region Phone

	/// <summary>
	/// Phone object for NHibernate mapped table 'tmh_Phone'.
	/// </summary>
	public class Phone
		{
		#region Member Variables
		
		protected int _id;
		protected string _name;
		protected string _type;
		protected string _number;
		protected int _owner;
	    protected string _ownerType;

	    #endregion

		#region Constructors

		public Phone()
		{
			_id = -1;
		}

		public Phone( string name, string type, string number, int owner )
		{
			this._name = name;
			this._type = type;
			this._number = number;
			this._owner = owner;
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
				if ( value != null && value.Length > 10)
					throw new ArgumentOutOfRangeException("Invalid value for Type", value, value.ToString());
				_type = value;
			}
		}

		public virtual string Number
		{
			get { return _number; }
			set
			{
				if ( value != null && value.Length > 20)
					throw new ArgumentOutOfRangeException("Invalid value for Number", value, value.ToString());
				_number = value;
			}
		}

		public virtual int Owner
		{
			get { return _owner; }
			set { _owner = value; }
		}

	    public virtual string OwnerType
	    {
            get { return _ownerType; }
            set { _ownerType = value; }
	    }

		#endregion

        #region -- Methods --
        public static Phone GetPhoneFromString(string value,int ownerid)
        {
            IList splited = value.Split('\\');
            if (splited.Count == 3)
            {
                Phone phone = new Phone();
                if ((string) splited[2] != "-1")
                {
                    phone.Id = Convert.ToInt32(splited[2]);
                }
                phone.Name = (string) splited[0];
                phone.Number = (string) splited[1];
                phone.Owner = ownerid;
                phone.Type = "Phone";
                phone.OwnerType = "Hotel";
                return phone;
            }
            return null;
        }

        public static Phone GetPhoneFromString(string value, int ownerid, Type ownerType)
        {
            IList splited = value.Split('\\');
            if (splited.Count == 3)
            {
                Phone phone = new Phone();
                if ((string)splited[2] != "-1")
                {
                    phone.Id = Convert.ToInt32(splited[2]);
                }
                phone.Name = (string)splited[0];
                phone.Number = (string)splited[1];
                phone.Owner = ownerid;
                phone.Type = "Phone";
                phone.OwnerType = ownerType.Name;
                return phone;
            }
            return null;
        }
        #endregion
    }

	#endregion
}
