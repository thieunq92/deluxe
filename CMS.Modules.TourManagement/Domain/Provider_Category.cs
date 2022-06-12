using System;
using System.Collections;

namespace CMS.Modules.TourManagement.Domain
{
    /// <summary>
    /// TmProvider object for NHibernate mapped table 'tm_Provider'.
    /// </summary>
    public class Provider_Category
    {
        #region Static Columns Name
        public static string NAME = "Name";
        public static string ORDER = "Order";
        #endregion
		
        #region Member Variables
		
        protected int _id;
        protected Provider _provider;
        protected ProviderCategory _category;

        #endregion

        #region Constructors

        public Provider_Category() 
        {
            _id = -1;
        }        

        #endregion

        #region Public Properties

        public virtual int Id
        {
            get {return _id;}
            set {_id = value;}
        }

        public virtual Provider Provider
        {
            get { return _provider; }
            set
            {
                _provider = value;
            }
        }

        public virtual ProviderCategory Category
        {
            get { return _category; }
            set { _category = value; }
        }
        #endregion        
    }
}