using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.Core.Domain;
using CMS.Core.Util;
using CMS.ServerControls;
using CMS.Web.Admin.Controls;
using CMS.Web.Util;
using iTextSharp.text;
using iTextSharp.text.pdf;
using log4net;
using NHibernate.Criterion;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.UI;
using Portal.Modules.OrientalSails.Web.Util;
using ListItem=System.Web.UI.WebControls.ListItem;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class OrderReport : SailsAdminBase
    {
    }
}