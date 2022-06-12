using System;
using System.Collections;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CMS.Core.Domain;

namespace Portal.Modules.OrientalSails.Domain
{
    #region Customer

    /// <summary>
    /// Customer object for NHibernate mapped table 'os_Customer'.
    /// </summary>
    public class DayNote
    {       
        #region Member Variables

        protected int _id;
        protected DateTime _date;
        protected string _note;
        protected Cruise _cruise;

        #endregion

        #region Constructors

        public DayNote()
        {
            _id = -1;
        }

        public DayNote(DateTime date)
        {
            _id = -1;
            _date = date;
        }

        #endregion

        #region Public Properties

        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual DateTime Date
        {
            get { return _date.Date; }
            set { _date = value.Date; }
        }

        public virtual string Note
        {
            get { return _note;}
            set { _note = value; }
        }

        public virtual Cruise Cruise
        {
            get { return _cruise; }
            set { _cruise = value; }
        }

        #endregion
    }

    #endregion
}
