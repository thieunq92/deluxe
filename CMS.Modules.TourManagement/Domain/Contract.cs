using System;
using CMS.Core.Domain;

namespace CMS.Modules.TourManagement.Domain
{
    /// <summary>
    /// Contract object for NHibernate mapped table 'tmh_Contract'.
    /// </summary>
    public class Contract
    {
        #region Member Variables

        protected User _createdBy;
        protected DateTime _createdDate;
        protected bool _deleted;
        protected DateTime _expireDate;
        protected string _fileType;
        protected int _id;
        protected User _modifiedBy;
        protected DateTime _modifiedDate;
        protected string _path;
        protected DateTime _startDate;
        protected string _name;

        #endregion

        #region Constructors

        public Contract()
        {
            _id = -1;
            _deleted = false;
        }

        public Contract(bool deleted, DateTime createdDate, User createdBy, DateTime modifiedDate, User modifiedBy,
                        DateTime startDate, DateTime expireDate, string path, string fileType)
        {
            _deleted = deleted;
            _createdDate = createdDate;
            _createdBy = createdBy;
            _modifiedDate = modifiedDate;
            _modifiedBy = modifiedBy;
            _startDate = startDate;
            _expireDate = expireDate;
            _path = path;
            _fileType = fileType;
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

        public virtual DateTime CreatedDate
        {
            get { return _createdDate; }
            set { _createdDate = value; }
        }

        public virtual User CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; }
        }

        public virtual DateTime ModifiedDate
        {
            get { return _modifiedDate; }
            set { _modifiedDate = value; }
        }

        public virtual User ModifiedBy
        {
            get { return _modifiedBy; }
            set { _modifiedBy = value; }
        }

        public virtual DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }

        public virtual DateTime ExpireDate
        {
            get { return _expireDate; }
            set { _expireDate = value; }
        }

        public virtual string Path
        {
            get { return _path; }
            set
            {
                if (value != null && value.Length > 200)
                    throw new ArgumentOutOfRangeException("Invalid value for Path", value, value);
                _path = value;
            }
        }

        public virtual string FileType
        {
            get { return _fileType; }
            set
            {
                if (value != null && value.Length > 20)
                    throw new ArgumentOutOfRangeException("Invalid value for FileType", value, value);
                _fileType = value;
            }
        }

        public virtual string Name
        {
            get { return _name; }
            set
            {
                if (value != null && value.Length > 100)
                    throw new ArgumentOutOfRangeException("Invalid value for FileType", value, value);
                _name = value;
            }
        }

        #endregion
    }
}