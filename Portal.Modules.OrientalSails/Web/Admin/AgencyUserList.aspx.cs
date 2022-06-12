using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Portal.Modules.OrientalSails.Web.UI;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class AgencyUserList : SailsAdminBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            pagerUser.AllowCustomPaging = true;
            if (!IsPostBack)
            {
                LoadUserAgency();
            }
        }

        private void LoadUserAgency()
        {
            int count = 0;
            rptUsers.DataSource = Module.AgencyUserGetAll(Request.QueryString, pagerUser.PageSize,pagerUser.CurrentPageIndex,out count );
            rptUsers.DataBind();
        }

        protected void rptUsers_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}