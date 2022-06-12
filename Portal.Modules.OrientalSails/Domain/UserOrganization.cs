using System;
using System.Collections.Generic;
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
    public class UserOrganization
    {        
        #region Constructors

        public UserOrganization()
        {
            //_id = -1;
        }

        #endregion

        #region Public Properties

        public virtual int Id { get; set; }
        public virtual User User { get; set; }
        public virtual Organization Organization { get; set; }

        #endregion
    }

    #endregion
}
