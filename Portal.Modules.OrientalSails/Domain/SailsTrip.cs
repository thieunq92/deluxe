using System;
using System.Collections;
using System.Collections.Generic;
using CMS.Core.Domain;

namespace Portal.Modules.OrientalSails.Domain
{
    #region SailsTrip

    /// <summary>
    /// SailsTrip object for NHibernate mapped table 'os_SailsTrip'.
    /// </summary>
    public class SailsTrip
    {
        #region Static Columns Name

        public static string ACTIVITIES = "Activities";
        public static string CREATEDBY = "CreatedBy";
        public static string CREATEDDATE = "CreatedDate";
        public static string DELETED = "Deleted";
        public static string DESCRIPTION = "Description";
        public static string EXCLUSIONS = "Exclusions";
        public static string IMAGE = "Image";
        public static string INCLUSIONS = "Inclusions";
        public static string MODIFIEDBY = "ModifiedBy";
        public static string MODIFIEDDATE = "ModifiedDate";
        public static string NAME = "Name";
        public static string NUMBEROFDAY = "NumberOfDay";
        public static string OPTIONS = "Options";
        public static string WHATTOTAKE = "WhatToTake";

        #endregion

        #region Member Variables

        protected string _activities;
        protected User _createdBy;
        protected DateTime _createdDate;
        protected bool _deleted;
        protected string _description;
        protected string _exclusions;
        protected int _id;
        protected string _image;
        protected string _inclusions;
        protected User _modifiedBy;
        protected DateTime _modifiedDate;
        protected string _name;
        protected int _numberOfDay;
        protected int _options;
        protected IList _triposBookings;
        protected IList _triposSailsPriceConfigs;
        protected string _whatToTake;
        protected string _tripCode;
        protected IList _costTypes;

        //TODO: bỏ
        private bool _hasCruise;
        public virtual bool HasCruise
        {
            get { return _hasCruise; }
            set { _hasCruise = value; }
        }

        #endregion

        #region Constructors

        public SailsTrip()
        {
            _id = -1;
        }

        public SailsTrip(bool deleted, User createdBy, DateTime createdDate, User modifiedBy, DateTime modifiedDate,
                         string name, int numberOfDay, string whatToTake, string activities, string inclusions,
                         string exclusions, string description, string image, int options)
        {
            _deleted = deleted;
            _createdBy = createdBy;
            _createdDate = createdDate;
            _modifiedBy = modifiedBy;
            _modifiedDate = modifiedDate;
            _name = name;
            _numberOfDay = numberOfDay;
            _whatToTake = whatToTake;
            _activities = activities;
            _inclusions = inclusions;
            _exclusions = exclusions;
            _description = description;
            _image = image;
            _options = options;
        }

        #endregion

        #region Public Properties

        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual bool Deleted
        {
            get { return _deleted; }
            set { _deleted = value; }
        }

        public virtual User CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; }
        }

        public virtual DateTime CreatedDate
        {
            get { return _createdDate; }
            set { _createdDate = value; }
        }

        public virtual User ModifiedBy
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
                if (value != null && value.Length > 200)
                    throw new ArgumentOutOfRangeException("Invalid value for Name", value, value.ToString());
                _name = value;
            }
        }

        public virtual int NumberOfDay
        {
            get { return _numberOfDay; }
            set { _numberOfDay = value; }
        }

        public virtual string WhatToTake
        {
            get { return _whatToTake; }
            set { _whatToTake = value; }
        }

        public virtual string Itinerary
        {
            get { return _activities; }
            set { _activities = value; }
        }

        public virtual string Inclusions
        {
            get { return _inclusions; }
            set { _inclusions = value; }
        }

        public virtual string Exclusions
        {
            get { return _exclusions; }
            set { _exclusions = value; }
        }

        public virtual string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public virtual string Image
        {
            get { return _image; }
            set
            {
                if (value != null && value.Length > 200)
                    throw new ArgumentOutOfRangeException("Invalid value for Image", value, value.ToString());
                _image = value;
            }
        }

        public virtual int NumberOfOptions
        {
            get { return _options; }
            set { _options = value; }
        }

        public virtual string TripCode
        {
            get { return _tripCode; }
            set { _tripCode = value; }
        }
        public virtual int NumberCustomerMin
        {
            get;
            set;
        }
        public virtual int TimeCreateBookingMin
        {
            get;
            set;
        }
        public virtual IList Bookings
        {
            get
            {
                if (_triposBookings == null)
                {
                    _triposBookings = new ArrayList();
                }
                return _triposBookings;
            }
            set { _triposBookings = value; }
        }

        public virtual IList SailsPriceConfigs
        {
            get
            {
                if (_triposSailsPriceConfigs == null)
                {
                    _triposSailsPriceConfigs = new ArrayList();
                }
                return _triposSailsPriceConfigs;
            }
            set { _triposSailsPriceConfigs = value; }
        }

        public virtual IList CostTypes
        {
            get
            {
                if (_costTypes == null)
                {
                    _costTypes = new ArrayList();
                }
                return _costTypes;
            }
            set { _costTypes = value; }
        }

        public virtual Organization Organization { get; set; }

        public virtual IList<AgencyCommission> AgencyCommissionList { set; get; }

        /// <summary>
        /// Lấy về OrgId thông qua Organization (không map)
        /// </summary>
        public virtual int OrgId
        {
            get
            {
                if (Organization!=null)
                {
                    return Organization.Id;
                }
                return 0;
            }
        }

        #endregion
    }

    #endregion
}