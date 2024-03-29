using System;
using System.Collections;
using CMS.Core.Domain;

namespace Portal.Modules.OrientalSails.Domain
{
    #region Booking

    /// <summary>
    /// Booking object for NHibernate mapped table 'os_Booking'.
    /// </summary>
    public class Cruise
    {
        #region Static Columns Name

        public static string CREATEDBY = "CreatedBy";
        public static string CREATEDDATE = "CreatedDate";
        public static string DELETED = "Deleted";
        public static string MODIFIEDBY = "ModifiedBy";
        public static string MODIFIEDDATE = "ModifiedDate";

        #endregion

        #region Member Variables

        protected User _createdBy;
        protected DateTime _createdDate;
        protected bool _deleted;
        protected int _id;
        protected User _modifiedBy;
        protected DateTime _modifiedDate;
        protected IList _trips;

        protected string _name;
        protected string _description;
        protected string _image;
        protected string _code;

        protected string _roomPlan;
        
        #endregion

        #region Constructors

        public Cruise()
        {
            _id = -1;
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
            set { _name = value; }
        }

        public virtual string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public virtual string Image
        {
            get { return _image; }
            set { _image = value; }
        }

        public virtual string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        public virtual IList Trips
        {
            get
            {
                if (_trips == null)
                {
                    _trips = new ArrayList();
                }
                return _trips;
            }
            set { _trips = value; }
        }

        public virtual string RoomPlan
        {
            get { return _roomPlan; }
            set { _roomPlan = value; }
        }
        #endregion
    }
    #endregion
}