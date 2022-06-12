using System;
using System.Collections;
using CMS.Modules.Gallery.Domain;

namespace CMS.Modules.TourManagement.Domain
{

    #region Location

    /// <summary>
    /// Location object for NHibernate mapped table 'tmh_Location'.
    /// </summary>
    public class Location
    {
        #region -Column Name-

        public static string PARENT = "Parent";
        public static string ALBUM = "Album";
        #endregion

        #region Member Variables

        protected string _description;
        protected int _id;
        protected string _languageKey;
        protected int _level;
        protected IList _locationtmhHotels;
        protected string _name;
        protected Location _parentId;
        protected IList _child;
        protected string _image;
        protected int _order;
        protected Album _album;
        protected string _hotelImage;
        protected string _restaurantImage;
        protected string _destinationImage;
        protected string _path;

        #endregion

        #region Constructors

        public Location()
        {
            _id = -1;
            _image = String.Empty;
        }

        public Location(string name, Location parentId, int level, string description, string languageKey)
        {
            _id = -1;
            _name = name;
            _parentId = parentId;
            _level = level;
            _description = description;
            _languageKey = languageKey;
        }

        #endregion

        #region Public Properties

        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual string Name
        {
            get { return _name; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for Name", value, value);
                _name = value;
            }
        }

        public virtual Location Parent
        {
            get { return _parentId; }
            set { _parentId = value; }
        }

        public virtual int Level
        {
            get { return _level; }
            set { _level = value; }
        }

        public virtual string Description
        {
            get { return _description; }
            set
            {
                _description = value;
            }
        }

        public virtual string LanguageKey
        {
            get { return _languageKey; }
            set
            {
                if (value != null && value.Length > 5)
                    throw new ArgumentOutOfRangeException("Invalid value for LanguageKey", value, value);
                _languageKey = value;
            }
        }

        public virtual IList Children
        {
            get
            {
                if (_child == null)
                {
                    _child = new ArrayList();
                }
                return _child;
            }
            set { _child = value; }
        }

        public virtual string Image
        {
            get { return _image; }
            set
            {
                if (value != null && value.Length > 250)
                    throw new ArgumentOutOfRangeException("Invalid value for Name", value, value);
                _image = value;
            }
        }

        public virtual int Order
        {
            get { return _order; }
            set { _order = value; }
        }

        public virtual Album Album
        {
            get { return _album; }
            set { _album = value; }
        }

        public virtual string HotelImage
        {
            get { return _hotelImage; }
            set { _hotelImage = value; }
        }

        public virtual string RestaurantImage
        {
            get { return _restaurantImage; }
            set { _restaurantImage = value; }
        }

        public virtual string DestinationImage
        {
            get { return _destinationImage; }
            set { _destinationImage = value; }
        }

        #endregion

        #region -- Methods --
        public virtual string GetPath()
        {
            if (string.IsNullOrEmpty(_path))
            {
                string result = _name;
                Location temp = this;
                while (temp.Parent != null)
                {
                    temp = temp.Parent;
                    result += string.Format(", {0}", temp.Name);
                }
                _path = result;                
            }
            return _path;
        }

        #endregion
    }

    #endregion
}