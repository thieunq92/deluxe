using System;
using System.Collections;

namespace CMS.Modules.TourManagement.Domain
{

    #region Currency

    /// <summary>
    /// Currency object for NHibernate mapped table 'tm_Currency'.
    /// </summary>
    public class TourType
    {
        #region Member Variables

        protected int _id;
        protected string _name;
        protected string _description;
        protected int _companyId;
        protected TourType _parent;
        protected IList _chidren;
        //protected int _orderPr

        #endregion

        #region Constructors

        public TourType()
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

        public virtual string Name
        {
            get { return _name; }
            set
            {
                if (value != null && value.Length > 100)
                    throw new ArgumentOutOfRangeException("Invalid value for Name", value, value);
                _name = value;
            }
        }

        public virtual string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public virtual int CompanyId
        {
            get { return _companyId; }
            set { _companyId = value; }
        }

        public virtual TourType Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        public virtual IList Children
        {
            get
            {
                if (_chidren!=null)
                {
                    return _chidren;
                }
                return new ArrayList();
            }
            set
            { _chidren = value; }
        }

        #endregion
    }

    #endregion
}