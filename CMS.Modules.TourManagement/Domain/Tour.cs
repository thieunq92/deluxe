using System;
using System.Collections;
using CMS.Core.Domain;
using CMS.Web.Util;

namespace CMS.Modules.TourManagement.Domain
{
	#region Tour

	/// <summary>
	/// Tour object for NHibernate mapped table 'tmt_Tour'.
	/// </summary>
	public class Tour
	{
		#region Static Columns Name
		public static string DELETED = "Deleted";
		public static string CREATEDBY = "CreatedBy";
		public static string CREATEDDATE = "CreatedDate";
		public static string MODIFIEDBY = "ModifiedBy";
		public static string MODIFIEDDATE = "ModifiedDate";
		public static string NAME = "Name";
		public static string DESCRIPTION = "Description";
		public static string IMAGE = "Image";
		public static string NUMBEROFDAY = "NumberOfDay";
		public static string SUMMARY = "Summary";
		public static string STARTFROM = "StartFrom";
		public static string ENDIN = "EndIn";
		public static string ACTIVITIES = "Activities";
		public static string GRADE = "Grade";
		public static string TRIPHIGHLIGHT = "TripHighLight";
		public static string DETAILSINITERARY = "DetailsIniterary";
		public static string INCLUSION = "Inclusion";
		public static string EXCLUSION = "Exclusion";
		public static string WHATTOTAKE = "WhatToTake";
		public static string TOURINSTRUCTION = "TourInstruction";
		public static string MAP = "Map";
		public static string ALBUMID = "AlbumId";
		public static string NOTEFOROPERATOR = "NoteForOperator";
		public static string NOTEFORSALE = "NoteForSale";
		public static string COMPANYID = "CompanyId";
	    public static string TOURREGION = "TourRegion";
	    public static string PACKAGESTATUS = "PackageStatus";
	    public static string FEATURED = "Featured";
		#endregion
		
		#region Member Variables
		
		protected int _id;
		protected bool _deleted;
		protected int _createdBy;
		protected DateTime _createdDate;
		protected int _modifiedBy;
		protected DateTime _modifiedDate;
		protected string _name;
		protected string _description;
		protected string _image;
		protected int _numberOfDay;
		protected string _summary;
		protected Location _startFrom;
        protected Location _endIn;
		protected string _activities;
		protected int _grade;
		protected string _tripHighLight;
		protected string _detailsIniterary;
		protected string _inclusion;
		protected string _exclusion;
		protected string _whatToTake;
		protected string _tourInstruction;
		protected string _map;
		protected int _albumId;
		protected string _noteForOperator;
		protected string _noteForSale;
		protected int _companyId;
		protected TourType _tourType;
	    protected Location _countryStart;
	    protected Location _regionStart;
	    protected Location _cityStart;
	    protected Location _countryEnd;
	    protected Location _regionEnd;
	    protected Location _cityEnd;
	    protected int _publicStatus;
	    protected TourRegion _tourRegion;
	    protected Provider _provider;
	    protected PackageStatus? _packageStatus;
	    protected int _featured;
	    protected IList _comments;
	    protected bool _isHalf;
		#endregion

		#region Constructors

		public Tour() 
		{
			_id = -1;
		    _featured = 0;
		}

        public Tour(bool deleted, int createdBy, DateTime createdDate, int modifiedBy, DateTime modifiedDate, string name, string description, string image, int numberOfDay, string summary, Location startFrom, Location endIn, string activities, int grade, string tripHighLight, string detailsIniterary, string inclusion, string exclusion, string whatToTake, string tourInstruction, string map, int albumId, string noteForOperator, string noteForSale, int companyId, TourType tourType, int publicStatus)
		{
			_deleted = deleted;
			_createdBy = createdBy;
			_createdDate = createdDate;
			_modifiedBy = modifiedBy;
			_modifiedDate = modifiedDate;
			_name = name;
			_description = description;
			_image = image;
			_numberOfDay = numberOfDay;
			_summary = summary;
			_startFrom = startFrom;
			_endIn = endIn;
			_activities = activities;
			_grade = grade;
			_tripHighLight = tripHighLight;
			_detailsIniterary = detailsIniterary;
			_inclusion = inclusion;
			_exclusion = exclusion;
			_whatToTake = whatToTake;
			_tourInstruction = tourInstruction;
			_map = map;
			_albumId = albumId;
			_noteForOperator = noteForOperator;
			_noteForSale = noteForSale;
			_companyId = companyId;
			_tourType = tourType;
            _publicStatus = publicStatus;
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

	    public virtual int PublicStatus
	    {
            get { return _publicStatus; }
            set { _publicStatus = value; }
	    }

		public virtual int CreatedBy
		{
			get { return _createdBy; }
			set { _createdBy = value; }
		}

		public virtual DateTime CreatedDate
		{
			get { return _createdDate; }
			set { _createdDate = value; }
		}

		public virtual int ModifiedBy
		{
			get { return _modifiedBy; }
			set { _modifiedBy = value; }
		}

		public virtual DateTime ModifiedDate
		{
			get { return _modifiedDate; }
			set { _modifiedDate = value; }
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

		public virtual string Description
		{
			get { return _description; }
			set
			{
				_description = value;
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

		public virtual int NumberOfDay
		{
			get { return _numberOfDay; }
			set { _numberOfDay = value; }
		}

		public virtual string Summary
		{
			get { return _summary; }
			set
			{
				_summary = value;
			}
		}

		public virtual Location StartFrom
		{
			get { return _startFrom; }
			set { _startFrom = value; }
		}

		public virtual Location EndIn
		{
			get { return _endIn; }
			set { _endIn = value; }
		}

		public virtual string Activities
		{
			get { return _activities; }
			set
			{
				_activities = value;
			}
		}

		public virtual int Grade
		{
			get { return _grade; }
			set { _grade = value; }
		}

		public virtual string TripHighLight
		{
			get { return _tripHighLight; }
			set
			{
				_tripHighLight = value;
			}
		}

		public virtual string DetailsIniterary
		{
			get { return _detailsIniterary; }
			set
			{
				_detailsIniterary = value;
			}
		}

		public virtual string Inclusion
		{
			get { return _inclusion; }
			set
			{
				_inclusion = value;
			}
		}

		public virtual string Exclusion
		{
			get { return _exclusion; }
			set
			{
				_exclusion = value;
			}
		}

		public virtual string WhatToTake
		{
			get { return _whatToTake; }
			set
			{
				_whatToTake = value;
			}
		}

		public virtual string TourInstruction
		{
			get { return _tourInstruction; }
			set
			{
				_tourInstruction = value;
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

		public virtual int AlbumId
		{
			get { return _albumId; }
			set { _albumId = value; }
		}

		public virtual string NoteForOperator
		{
			get { return _noteForOperator; }
			set
			{
				_noteForOperator = value;
			}
		}

		public virtual string NoteForSale
		{
			get { return _noteForSale; }
			set
			{
				_noteForSale = value;
			}
		}

		public virtual int CompanyId
		{
			get { return _companyId; }
			set { _companyId = value; }
		}

		public virtual TourType TourType
		{
			get { return _tourType; }
			set { _tourType = value; }
		}

	    public virtual Location CountryStart
	    {
            get { return _countryStart; }
            set { _countryStart = value; }
	    }
        public virtual Location RegionStart
        {
            get { return _regionStart; }
            set { _regionStart = value; }
        }
        public virtual Location CityStart
        {
            get { return _cityStart; }
            set { _cityStart = value; }
        }
        public virtual Location CountryEnd
        {
            get { return _countryEnd; }
            set { _countryEnd = value; }
        }
        public virtual Location RegionEnd
        {
            get { return _regionEnd; }
            set { _regionEnd = value; }
        }
        public virtual Location CityEnd
        {
            get { return _cityEnd; }
            set { _cityEnd = value; }
        }
	    public virtual TourRegion TourRegion
	    {
            get { return _tourRegion; }
            set { _tourRegion = value; }
	    }

	    public virtual Provider Provider
	    {
            get { return _provider; }
            set { _provider = value; }
	    }

	    public virtual PackageStatus? PackageStatus
	    {
            get { return _packageStatus; }
            set { _packageStatus = value; }
	    }

	    public virtual int Featured
	    {
            get { return _featured; }
            set { _featured = value; }
	    }

	    public virtual bool IsHalf
	    {
            get { return _isHalf; }
            set { _isHalf = value; }
	    }

	    public virtual IList Comments
	    {
	        get
	        {
                if (_comments == null)
                {
                    _comments = new ArrayList();
                }
                return _comments;
	        }
            set { _comments = value; }
	    }

		#endregion        

        #region -- Methods --
        public virtual string GetUrl(Section section)
        {
            string basepath = UrlHelper.GetUrlNoAspxFromSection(section);
            string type = "notype";
            if (_tourType!=null)
            {
                type = UrlHelper.ConvertToUrl(_tourType.Name);
            }
            string region = "noregion";
            if (_tourRegion!=null)
            {
                region = UrlHelper.ConvertToUrl(_tourRegion.Name);
            }
            string name = UrlHelper.ConvertToUrl(_name);

            return string.Format("{0}/tour/{1}/{2}/{3}/{4}{5}", basepath, Id, region, type, name,UrlHelper.EXTENSION);
        }

        public virtual string GetOrderUrl(Section section)
        {
            string basepath = UrlHelper.GetUrlNoAspxFromSection(section);
            string type = "notype";
            if (_tourType != null)
            {
                type = UrlHelper.ConvertToUrl(_tourType.Name);
            }
            string region = "noregion";
            if (_tourRegion != null)
            {
                region = UrlHelper.ConvertToUrl(_tourRegion.Name);
            }
            string name = UrlHelper.ConvertToUrl(_name);

            return string.Format("{0}/order/{1}/{2}/{3}/{4}{5}", basepath, Id, region, type, name, UrlHelper.EXTENSION);            
        }
        #endregion
    }

    public enum PackageStatus
    {
        FullPackage,
        PartPackage,
        Both
    }

    public class PackageStatusClass : NHibernate.Type.EnumStringType
    {
        public PackageStatusClass()
            : base(typeof(PackageStatus), 20)
        {

        }
    }

	#endregion
}
