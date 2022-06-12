using CMS.Core.Domain;
using CMS.Web.HttpModules;
using Portal.Modules.OrientalSails.BusinessLogic.Share;
using Portal.Modules.OrientalSails.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class MainAgency : System.Web.UI.MasterPage
    {
        private PermissionBLL permissionBLL;
        private UserBLL userBLL;
        private User currentUser;

        public PermissionBLL PermissionBLL
        {
            get
            {
                if (permissionBLL == null)
                {
                    permissionBLL = new PermissionBLL();
                }
                return permissionBLL;
            }
        }
        public UserBLL UserBLL
        {
            get
            {
                if (userBLL == null)
                {
                    userBLL = new UserBLL();
                }
                return userBLL;
            }
        }
        public User CurrentUser
        {
            get
            {
                if (currentUser == null)
                {
                    currentUser = UserBLL.UserGetCurrent();
                }
                return currentUser;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            FillNavigateUrl();
            NavigateVisibleByPermission();
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (permissionBLL != null)
            {
                permissionBLL.Dispose();
                permissionBLL = null;
            }

            if (userBLL != null)
            {
                userBLL.Dispose();
                userBLL = null;
            }

            if (!Page.IsPostBack)
            {
                Session["Redirect"] = false;
            }

            if (!(bool)Session["Redirect"])
            {
                ClearMessage();
            }
        }

        protected void lbLogOut_Click(object sender, EventArgs e)
        {
            var am = (AuthenticationModule)Context.ApplicationInstance.Modules["AuthenticationModule"];
            am.Logout();

            Context.Response.Redirect(Context.Request.RawUrl);
        }

        public void FillNavigateUrl()
        {
            hlBookingList.NavigateUrl = "AgencyBookingList.aspx?NodeId=1&SectionId=15";
            hlAddBooking.NavigateUrl = "AgencyAddBooking.aspx?NodeId=1&SectionId=15";
            hlUserPanel.NavigateUrl = "AgencyUserDetail.aspx?NodeId=1&SectionId=15";
            hplProduct.NavigateUrl = "ProductTrip.aspx?NodeId=1&SectionId=15";
        }

        public void NavigateVisibleByPermission()
        {
            if (CurrentUser == null)
                return;

            if (PermissionBLL.UserCheckRole(CurrentUser.Id, (int)Roles.Administrator))
            {
                return;
            }

            
        }

        public string UserCurrentGetName()
        {
            return UserBLL.UserCurrentGetName();
        }

        public void ClearMessage()
        {
            Session["SuccessMessage"] = null;
            Session["InfoMessage"] = null;
            Session["WarningMessage"] = null;
            Session["ErrorMessage"] = null;
        }
    }
}