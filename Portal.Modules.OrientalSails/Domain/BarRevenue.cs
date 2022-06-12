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
    [Obsolete]
    public class BarRevenue
    {
        protected int _id;
        protected DateTime _date;
        protected double _revenue;
        protected Cruise _cruise;

        public BarRevenue()
        {
            _id = -1;
        }

        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        public virtual double Revenue
        {
            get { return _revenue; }
            set { _revenue = value; }
        }

        public virtual Cruise Cruise
        {
            get { return _cruise; }
            set { _cruise = value; }
        }
    }
}
