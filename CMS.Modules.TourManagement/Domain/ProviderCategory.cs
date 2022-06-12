using System;
using System.Collections;

namespace CMS.Modules.TourManagement.Domain
{
    /// <summary>
    /// TmProvider object for NHibernate mapped table 'tm_Provider'.
    /// </summary>
    public class ProviderCategory
    {
        #region Static Columns Name
        public static string NAME = "Name";
        public static string ORDER = "Order";
        #endregion
		
        #region Member Variables
		
        protected int _id;
        protected string _name;
        protected int _order;

        #endregion

        #region Constructors

        public ProviderCategory() 
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

        public virtual int Order
        {
            get { return _order; }
            set { _order = value; }
        }
        #endregion        
    }
}