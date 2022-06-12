
using System;
using System.Collections;
using System.Web.UI.WebControls;

namespace CMS.Modules.TourManagement.Domain
{
    /// <summary>
    /// TmCompany object for NHibernate mapped table 'tm_Company'.
    /// </summary>
    public class Company
    {
        #region Member Variables
		
        protected int _id;
        protected bool _deleted;
        protected DateTime _createdDate;
        protected int _createdBy;
        protected DateTime _modifiedDate;
        protected int _modifiedBy;
        protected string _name;
        protected string _address;
        protected int _location;
        protected string _website;
        protected string _tel;
        protected string _fax;
        protected string _mobile;
        protected string _description;
        protected string _map;
        protected string _image;
        protected int _isFeatured;
        protected IList _tmgGuides;

        #endregion

        #region Constructors

        public Company() 
        {
            _id = -1;
        }

        public Company( bool deleted, DateTime createdDate, int createdBy, DateTime modifiedDate, int modifiedBy, string name, string address, int location, string website, string tel, string fax, string mobile, string description, string map, string image, int isFeatured )
        {
            _deleted = deleted;
            _createdDate = createdDate;
            _createdBy = createdBy;
            _modifiedDate = modifiedDate;
            _modifiedBy = modifiedBy;
            _name = name;
            _address = address;
            _location = location;
            _website = website;
            _tel = tel;
            _fax = fax;
            _mobile = mobile;
            _description = description;
            _map = map;
            _image = image;
            _isFeatured = isFeatured;
        }

        #endregion

        #region Public Properties

        public virtual int Id
        {
            get {return _id;}
            set {_id = value;}
        }

        public virtual bool Deleted
        {
            get { return _deleted; }
            set { _deleted = value; }
        }

        public virtual DateTime CreatedDate
        {
            get { return _createdDate; }
            set { _createdDate = value; }
        }

        public virtual int CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; }
        }

        public virtual DateTime ModifiedDate
        {
            get { return _modifiedDate; }
            set { _modifiedDate = value; }
        }

        public virtual int ModifiedBy
        {
            get { return _modifiedBy; }
            set { _modifiedBy = value; }
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

        public virtual string Address
        {
            get { return _address; }
            set
            {
                if ( value != null && value.Length > 1000)
                    throw new ArgumentOutOfRangeException("Invalid value for Address", value, value.ToString());
                _address = value;
            }
        }

        public virtual int Location
        {
            get { return _location; }
            set { _location = value; }
        }

        public virtual string Website
        {
            get { return _website; }
            set
            {
                if ( value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for Website", value, value.ToString());
                _website = value;
            }
        }

        public virtual string Tel
        {
            get { return _tel; }
            set
            {
                if ( value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for Tel", value, value.ToString());
                _tel = value;
            }
        }

        public virtual string Fax
        {
            get { return _fax; }
            set
            {
                if ( value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for Fax", value, value.ToString());
                _fax = value;
            }
        }

        public virtual string Mobile
        {
            get { return _mobile; }
            set
            {
                if ( value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for Mobile", value, value.ToString());
                _mobile = value;
            }
        }

        public virtual string Description
        {
            get { return _description; }
            set
            {
                if ( value != null && value.Length > 1000)
                    throw new ArgumentOutOfRangeException("Invalid value for Description", value, value.ToString());
                _description = value;
            }
        }

        public virtual string Map
        {
            get { return _map; }
            set
            {
                if ( value != null && value.Length > 200)
                    throw new ArgumentOutOfRangeException("Invalid value for Map", value, value.ToString());
                _map = value;
            }
        }

        public virtual string Image
        {
            get { return _image; }
            set
            {
                if ( value != null && value.Length > 200)
                    throw new ArgumentOutOfRangeException("Invalid value for Image", value, value.ToString());
                _image = value;
            }
        }

        public virtual int IsFeatured
        {
            get { return _isFeatured; }
            set { _isFeatured = value; }
        }

        public virtual IList tmg_Guides
        {
            get
            {
                if (_tmgGuides==null)
                {
                    _tmgGuides = new ArrayList();
                }
                return _tmgGuides;
            }
            set { _tmgGuides = value; }
        }

        #endregion        
    }
}