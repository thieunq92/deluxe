
using System;
using System.Collections;
using System.Web.UI.WebControls;

namespace CMS.Modules.TourManagement.Domain
{
	#region Email

	/// <summary>
	/// Email object for NHibernate mapped table 'tmh_Email'.
	/// </summary>
	public class Email
		{
		#region Member Variables
		
		protected int _id;
		protected string _name;
		protected string _address;
		protected int _ownerId;
	    protected string _ownerType;

	    #endregion

		#region Constructors

		public Email()
		{
			_id = -1;
		}

		public Email( string name, string address, int ownerId )
		{
			this._name = name;
			this._address = address;
			this._ownerId = ownerId;
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
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Name", value, value.ToString());
				_name = value;
			}
		}

		public virtual string Address
		{
			get { return _address; }
			set
			{
				if ( value != null && value.Length > 200)
					throw new ArgumentOutOfRangeException("Invalid value for Address", value, value.ToString());
				_address = value;
			}
		}

		public virtual int OwnerId
		{
			get { return _ownerId; }
			set { _ownerId = value; }
		}

        public virtual string OwnerType
        {
            get { return _ownerType; }
            set { _ownerType = value; }
        }

		#endregion

        #region -- Methods --
        public static Email GetEmailFromString(string value, int ownerid)
        {
            IList splited = value.Split('\\');
            if (splited.Count == 3)
            {
                Email email = new Email();
                if ((string)splited[2] != "-1")
                {
                    email.Id = Convert.ToInt32(splited[2]);
                }
                email.Name = (string)splited[0];
                email.Address = (string)splited[1];
                email.OwnerId = ownerid;
                email.OwnerType = "Hotel";
                return email;
            }
            return null;
        }

        public static Email GetEmailFromString(string value, int ownerid, Type ownerType)
        {
            IList splited = value.Split('\\');
            if (splited.Count == 3)
            {
                Email email = new Email();
                if ((string)splited[2] != "-1")
                {
                    email.Id = Convert.ToInt32(splited[2]);
                }
                email.Name = (string)splited[0];
                email.Address = (string)splited[1];
                email.OwnerId = ownerid;
                email.OwnerType = ownerType.Name;
                return email;
            }
            return null;
        }
        #endregion        
    }

	#endregion
}
