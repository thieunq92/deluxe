using System;
using CMS.Core.Domain;

namespace Portal.Modules.OrientalSails.Domain
{
    #region Booking

    /// <summary>
    /// Booking object for NHibernate mapped table 'os_Booking'.
    /// </summary>
    public class Nationality
    {
        #region Static Columns Name

        #endregion

        #region Member Variables

        protected int _id;
        protected string _name;
        protected string _code;
        protected bool _deleted;
        
        #endregion

        #region Constructors

        public Nationality()
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
            set { _name = value; }
        }

        public virtual string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        public virtual bool Deleted
        {
            get { return _deleted; }
            set { _deleted = value; }
        }
        #endregion
    }
    #endregion
}