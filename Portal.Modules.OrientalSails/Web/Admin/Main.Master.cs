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
    public partial class Main : System.Web.UI.MasterPage
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
            var fromLogin = false;
            try
            {
                fromLogin = Convert.ToBoolean(Int32.Parse(Request.QueryString["fromLogin"]));
            }
            catch { }
            if (fromLogin)
            {
                if(PermissionBLL.UserCheckPermission(CurrentUser,PermissionEnum.DASHBOARDMANAGER_ACCESS)
                    || PermissionBLL.UserCheckPermission(CurrentUser, PermissionEnum.DASHBOARD_ACCESS)
                    || permissionBLL.UserCheckPermission(CurrentUser,PermissionEnum.AllowAccessDashBoardOperationPage))
                {
                    Response.Redirect("DashBoardManager.aspx?NodeId=1&SectionId=15");
                }
            }
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
            hlBookingList.NavigateUrl = "BookingList.aspx?NodeId=1&SectionId=15";
            hlAddBooking.NavigateUrl = "AddBooking.aspx?NodeId=1&SectionId=15";
            hlOrders.NavigateUrl = "OrderReport.aspx?NodeId=1&SectionId=15"; 
            hlBookingDate.NavigateUrl = "BookingReport.aspx?NodeId=1&SectionId=15";
            hlBookingPeriod.NavigateUrl = "BookingReportPeriodAll.aspx?NodeId=1&SectionId=15";
            hlIncomeReport.NavigateUrl = "IncomeReport.aspx?NodeId=1&SectionId=15";
            hlIncomeOwn.NavigateUrl = "PaymentReport.aspx?NodeId=1&SectionId=15";
            hlExpenseReport.NavigateUrl = "ExpenseReport.aspx?NodeId=1&SectionId=15";
            hlExpenseDebt.NavigateUrl = "PayableList.aspx?NodeId=1&SectionId=15";
            hlBalance.NavigateUrl = "BalanceReport.aspx?NodeId=1&SectionId=15";
            hlAgencyEdit.NavigateUrl = "AgencyEdit.aspx?NodeId=1&SectionId=15";
            hlAgencyList.NavigateUrl = "AgencyList.aspx?NodeId=1&SectionId=15";
            hlAgentList.NavigateUrl = "AgentList.aspx?NodeId=1s&SectionId=15";
            hlTripList.NavigateUrl = "SailsTripList.aspx?NodeId=1&SectionId=15";
            hlTripEdit.NavigateUrl = "SailsTripEdit.aspx?NodeId=1&SectionId=15";      
            hlExtraOption.NavigateUrl = "ExtraOptionEdit.aspx?NodeId=1&SectionId=15";
            hlUSDRate.NavigateUrl = "ExchangeRate.aspx?NodeId=1&SectionId=15";
            hlCosting.NavigateUrl = "Costing.aspx?NodeId=1&SectionId=15";
            hlHaiPhong.NavigateUrl = "CruiseConfig.aspx?NodeId=1&SectionId=15";
            hlCostTypes.NavigateUrl = "CostTypes.aspx?NodeId=1&SectionId=15";
            hlExpensePeriod.NavigateUrl = "ExpensePeriod.aspx?NodeId=1&SectionId=15";
            hlExpenseDate.NavigateUrl = hlBookingDate.NavigateUrl;
            hlPermissions.NavigateUrl = "SetPermission.aspx?NodeId=1&SectionId=15";
            hlReceiablePayable.NavigateUrl = "ReceivableTotal.aspx?NodeId=1&SectionId=15";
            hlAddQuestion.NavigateUrl = "QuestionGroupEdit.aspx?NodeId=1&SectionId=15";
            hlQuestionList.NavigateUrl = "QuestionView.aspx?NodeId=1&SectionId=15";
            hlFeedbackReport.NavigateUrl = "FeedbackReport.aspx?NodeId=1&SectionId=15";
            hlDocumentManage.NavigateUrl = "DocumentManage.aspx?NodeId=1&SectionId=15";
            hlViewDocument.NavigateUrl = "DocumentView.aspx?NodeId=1&SectionId=15";
            hlViewMeetings.NavigateUrl = "ViewMeetings.aspx?NodeId=1&SectionId=15";
            hlUserPanel.NavigateUrl = "User.aspx?NodeId=1&SectionId=15";
            hlBookingByOperator.NavigateUrl = "BookingByOperator.aspx?NodeId=1&SectionId=15";
            hlEventList.NavigateUrl = "EventList.aspx?NodeId=1&SectionId=15";
            hlGuideCollect.NavigateUrl = "GuideCollects.aspx?NodeId=1&SectionId=15";
            hlDriverCollect.NavigateUrl = "DriverCollects.aspx?NodeId=1&SectionId=15";
            hlCommission.NavigateUrl = "CommissionPayment.aspx?NodeId=1&SectionId=15";
            hlPaymentChecking.NavigateUrl = "PaymentChecking.aspx?NodeId=1&SectionId=15";
            hlOrgs.NavigateUrl = "Organizations.aspx?NodeId=1&SectionId=15";
            hplAgencyUser.NavigateUrl = "AgencyUserList.aspx?NodeId=1&SectionId=15";
        }

        public void NavigateVisibleByPermission()
        {
            if (CurrentUser == null)
                return;

            if (PermissionBLL.UserCheckRole(CurrentUser.Id, (int)Roles.Administrator))
            {
                return;
            }

            pAddBooking.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_ADDBOOKING);
            pBookingList.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_BOOKINGLIST);
            pOrders.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_ORDERREPORT);
            pBookingReport.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_BOOKINGREPORTPERIOD);
            pIncomeReport.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_INCOMEREPORT);
            pReceivable.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_PAYMENTREPORT);
            pExpenseReport.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_EXPENSEREPORT);
            pPayable.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_PAYABLELIST);
            pBalance.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_BALANCEREPORT);
            pSummary.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_RECEIVABLETOTAL);
            pAgencyEdit.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_AGENCYEDIT);
            pAgencyList.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_AGENCYLIST);
            pAgencyPolicies.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_AGENTLIST);
            pTripEdit.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_SAILSTRIPEDIT);
            pTripList.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_SAILSTRIPLIST);
            pExtraService.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_EXTRAOPTIONEDIT);
            pCostingConfig.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_COSTING);
            pHaiPhong.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_CRUISECONFIG);
            pExpensePeriod.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_EXPENSEPERIOD);
            pCostTypes.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_COSTTYPES);
            pUSDRate.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_EXCHANGERATE);

            if (pAddBooking.Visible || pBookingList.Visible || pOrders.Visible || pBookingDate.Visible || pBookingReport.Visible)
            {
                tabBooking.Visible = true;
            }
            else
            {
                tabBooking.Visible = false;
            }

            if (pIncomeReport.Visible || pReceivable.Visible || pExpenseReport.Visible || pPayable.Visible
                || pBalance.Visible || pSummary.Visible)
            {
                tabReports.Visible = true;
            }
            else
            {
                tabReports.Visible = false;
            }

            if (pAgencyEdit.Visible || pAgencyList.Visible
                || pAgencyPolicies.Visible || pAgencyViewMeetings.Visible )
            {
                tabConfiguration.Visible = true;
            }
            else
            {
                tabConfiguration.Visible = false;
            }

            if (pTripEdit.Visible || pTripList.Visible)
            {
                tabTrips.Visible = true;
            }
            else
            {
                tabTrips.Visible = false;
            }

            if (pExtraService.Visible || pCostingConfig.Visible || pDailyManualCost.Visible || pExpensePeriod.Visible || pHaiPhong.Visible
                || pCostTypes.Visible || pUSDRate.Visible)
            {
                tabCost.Visible = true;
            }
            else
            {
                tabCost.Visible = false;
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