using System;
using System.Collections;
using System.Collections.Generic;

namespace Portal.Modules.OrientalSails.Domain
{
    #region RoomClass

    /// <summary>
    /// RoomClass object for NHibernate mapped table 'os_RoomClass'.
    /// </summary>
    public class Organization
    {
        #region Member Variables

        protected int _id;
        protected string _name;
        protected Organization _parent;

        #endregion

        #region Constructors

        public Organization()
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

        public virtual string Name { get; set; }
        public virtual Organization Parent { get; set; }
        public virtual IList<Organization> Children
        {
            get
            {
                return new List<Organization>();
            }
        }

        #endregion
    }

    #endregion
}