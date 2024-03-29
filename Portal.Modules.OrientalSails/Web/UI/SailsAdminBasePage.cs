using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CMS.Core.Util;
using CMS.Web.UI;
using Portal.Modules.OrientalSails.ReportEngine;
using Portal.Modules.OrientalSails.Web.Admin;

namespace Portal.Modules.OrientalSails.Web.UI
{
    public class SailsAdminBasePage : KitModuleAdminBasePage
    {
        #region -- PRIVATE MEMBERS --
        protected IList _allRoomTypes;
        protected IList _allRoomClasses;
        #endregion

        #region -- PROPERTIES --
        public new SailsModule Module
        {
            get { return (SailsModule)base.Module; }
        }

        public IList AllRoomTypes
        {
            get
            {
                if (_allRoomTypes == null)
                {
                    _allRoomTypes = Module.RoomTypexGetAll();
                }
                return _allRoomTypes;
            }
        }

        public IList AllRoomClasses
        {
            get
            {
                if (_allRoomClasses == null)
                {
                    _allRoomClasses = Module.RoomClassGetAll();
                }
                return _allRoomClasses;
            }
        }

        #region -- Settings --
        public double DefaultTicket
        {
            get
            {
                if (ModuleSettings[SailsModule.TICKET] != null)
                {
                    return Convert.ToDouble(ModuleSettings[SailsModule.TICKET]);
                }
                return 0;
            }
        }

        public double DefaultMeal
        {
            get
            {
                if (ModuleSettings[SailsModule.MEAL] != null)
                {
                    return Convert.ToDouble(ModuleSettings[SailsModule.MEAL]);
                }
                return 0;
            }
        }

        public double DefaultKayaking
        {
            get
            {
                if (ModuleSettings[SailsModule.KAYAKING] != null)
                {
                    return Convert.ToDouble(ModuleSettings[SailsModule.KAYAKING]);
                }
                return 0;
            }
        }

        public double DefaultService
        {
            get
            {
                if (ModuleSettings[SailsModule.SERVICE] != null)
                {
                    return Convert.ToDouble(ModuleSettings[SailsModule.SERVICE]);
                }
                return 0;
            }
        }

        public double DefaultRent
        {
            get
            {
                if (ModuleSettings[SailsModule.RENT] != null)
                {
                    return Convert.ToDouble(ModuleSettings[SailsModule.RENT]);
                }
                return 0;
            }
        }

        public double ChildPrice
        {
            get
            {
                if (ModuleSettings["CHILD_PRICE"] != null)
                {
                    return Convert.ToDouble(ModuleSettings["CHILD_PRICE"]);
                }
                return 80;
            }
        }

        public bool UseCustomBookingId
        {
            get
            {
                if (ModuleSettings["CUSTOM_BK_ID"] != null)
                {
                    return Convert.ToBoolean(ModuleSettings["CUSTOM_BK_ID"]);
                }
                return false;
            }
        }

        public bool ShowCustomerName
        {
            get
            {
                if (ModuleSettings["SHOW_CUSTOMER"] != null)
                {
                    return Convert.ToBoolean(ModuleSettings["SHOW_CUSTOMER"]);
                }
                return true;
            }
        }

        public string BookingFormat
        {
            get
            {
                if (ModuleSettings["BOOKING_FORMAT"] != null)
                {
                    return ModuleSettings["BOOKING_FORMAT"].ToString();
                }
                return "{0:00000}";
            }
        }

        public double AgencySupplement
        {
            get
            {
                if (ModuleSettings["AgencySupplement"] != null)
                {
                    return Convert.ToDouble(ModuleSettings["AgencySupplement"]);
                }
                return 0;
            }
        }

        public double ChildCost
        {
            get
            {
                if (ModuleSettings["CHILD_COST"] != null)
                {
                    return Convert.ToDouble(ModuleSettings["CHILD_COST"]);
                }
                return 80;
            }
        }

        public bool CustomPriceAddBooking
        {
            get
            {
                if (ModuleSettings[SailsModule.ADD_BK_CUSTOMPRICE] != null)
                {
                    return Convert.ToBoolean(ModuleSettings[SailsModule.ADD_BK_CUSTOMPRICE]);
                }
                return false;
            }
        }

        public bool CustomPriceForRoom
        {
            get
            {
                if (ModuleSettings[SailsModule.ROOM_CUSTOMPRICE] != null)
                {
                    return Convert.ToBoolean(ModuleSettings[SailsModule.ROOM_CUSTOMPRICE]);
                }
                return false;
            }
        }

        public bool PartnershipManager
        {
            get
            {
                if (ModuleSettings[SailsModule.PARTNERSHIP] != null)
                {
                    return Convert.ToBoolean(ModuleSettings[SailsModule.PARTNERSHIP]);
                }
                return false;
            }
        }

        public bool CheckAccountStatus
        {
            get
            {
                if (ModuleSettings[SailsModule.ACCOUNT_STATUS] != null)
                {
                    return Convert.ToBoolean(ModuleSettings[SailsModule.ACCOUNT_STATUS]);
                }
                return false;
            }
        }

        public bool CheckCharter
        {
            get
            {
                if (ModuleSettings[SailsModule.CHECK_CHARTER] != null)
                {
                    return Convert.ToBoolean(ModuleSettings[SailsModule.CHECK_CHARTER]);
                }
                return false;
            }
        }

        public bool ShowExpenseByDate
        {
            get
            {
                if (ModuleSettings[SailsModule.SHOW_EXPENSE_BY_DATE] != null)
                {
                    return Convert.ToBoolean(ModuleSettings[SailsModule.SHOW_EXPENSE_BY_DATE]);
                }
                return false;
            }
        }

        public bool ShowBarRevenue
        {
            get
            {
                if (ModuleSettings[SailsModule.BAR_REVENUE] != null)
                {
                    return Convert.ToBoolean(ModuleSettings[SailsModule.BAR_REVENUE]);
                }
                return false;
            }
        }

        public bool AllowNoAgency
        {
            get
            {
                if (ModuleSettings[SailsModule.NO_AGENCY_BK] != null)
                {
                    return Convert.ToBoolean(ModuleSettings[SailsModule.NO_AGENCY_BK]);
                }
                return false;
            }
        }

        public bool DetailService
        {
            get
            {
                if (ModuleSettings[SailsModule.DETAIL_SERVICE] != null)
                {
                    return Convert.ToBoolean(ModuleSettings[SailsModule.DETAIL_SERVICE]);
                }
                return false;
            }
        }

        public bool ShowOverallDailyExpense
        {
            get
            {
                if (ModuleSettings[SailsModule.OVERALL_EXPENSE] != null)
                {
                    return Convert.ToBoolean(ModuleSettings[SailsModule.OVERALL_EXPENSE]);
                }
                return false;
            }
        }

        public bool ApprovedDefault
        {
            get
            {
                if (ModuleSettings[SailsModule.APPROVED_DEFAULT] != null)
                {
                    return Convert.ToBoolean(ModuleSettings[SailsModule.APPROVED_DEFAULT]);
                }
                return false;
            }
        }

        public bool PuRequired
        {
            get
            {
                if (ModuleSettings[SailsModule.PUREQUIRED] != null)
                {
                    return Convert.ToBoolean(ModuleSettings[SailsModule.PUREQUIRED]);
                }
                return false;
            }
        }

        public bool PeriodExpenseAvg
        {
            get
            {
                if (ModuleSettings[SailsModule.PERIOD_EXPENSE_AVG] != null)
                {
                    return Convert.ToBoolean(ModuleSettings[SailsModule.PERIOD_EXPENSE_AVG]);
                }
                return true;
            }
        }

        public bool ApprovedLock
        {
            get
            {
                if (ModuleSettings[SailsModule.APPROVED_LOCK] != null)
                {
                    return Convert.ToBoolean(ModuleSettings[SailsModule.APPROVED_LOCK]);
                }
                return true;
            }
        }

        public bool CustomerPrice
        {
            get
            {
                if (ModuleSettings[SailsModule.CUSTOMER_PRICE] != null)
                {
                    return Convert.ToBoolean(ModuleSettings[SailsModule.CUSTOMER_PRICE]);
                }
                return true;
            }
        }

        public bool UseVNDExpense
        {
            get
            {
                if (ModuleSettings[SailsModule.USE_VND_EXPENSE] != null)
                {
                    return Convert.ToBoolean(ModuleSettings[SailsModule.USE_VND_EXPENSE]);
                }
                return true;
            }
        }
        #endregion
        #endregion

        protected override void OnPreInit(EventArgs e)
        {
            //LoadRes(null); ;
            //if (Page.MasterPageFile.Contains("SailsMaster.Master"))
            //{
            //    Page.MasterPageFile = "OS.Master";
            //}
            base.OnPreInit(e);
        }

        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
            HideError();

            if (Master is SailsMaster)
            {
                //if (ModuleSettings["Sale"]==null)
                //{
                //    return;
                //}
                SailsMaster master = (SailsMaster)Master;
                //foreach (Role role in UserIdentity.Roles)
                //{
                //    if (role.Id.ToString() == ModuleSettings["Sale"].ToString())
                //    {
                //        master.SetRoleToSale();
                //    }
                //}

                if (Section != null && Node != null)
                {
                    string title = Node.Site.Name;
                    master.SetTitle(title);
                }
            }
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            // Check quyền
            if (Page.Master is SailsMaster && Module != null)
            {
                SailsMaster master = (SailsMaster)Page.Master;
                master.CheckPermisson(Module, UserIdentity);
            }
        }

        public void ShowMessage(string message)
        {
            HtmlGenericControl divMessage = (HtmlGenericControl)Master.FindControl("divMessage");
            if (divMessage != null)
            {
                divMessage.Visible = true;
                Label labelMessage = Master.FindControl("labelMessage") as Label;
                if (labelMessage != null)
                {
                    labelMessage.Text = message;
                    if (divMessage.Attributes["class"] != null)
                    {
                        divMessage.Attributes["class"] = "module_message";
                    }
                    else
                    {
                        divMessage.Attributes.Add("class", "module_message");
                    }
                }
            }
        }

        public void ShowError(string message)
        {
            HtmlGenericControl divMessage = (HtmlGenericControl)Master.FindControl("divMessage");
            if (divMessage != null)
            {
                divMessage.Visible = true;
                Label labelMessage = Master.FindControl("labelMessage") as Label;
                if (labelMessage != null)
                {
                    labelMessage.Text = message;
                    if (divMessage.Attributes["class"] != null)
                    {
                        divMessage.Attributes["class"] = "module_error";
                    }
                    else
                    {
                        divMessage.Attributes.Add("class", "module_error");
                    }
                }
            }
        }

        public void HideError()
        {
            if (Master == null)
            {
                return;
            }
            HtmlGenericControl divMessage = Master.FindControl("divMessage") as HtmlGenericControl;
            if (divMessage != null)
            {
                divMessage.Visible = false;
            }
        }

        private IReportEngine _reportEngine;
        public IReportEngine ReportEngine
        {
            get
            {
                if (_reportEngine == null)
                {
                    string engine;
                    if (string.IsNullOrEmpty(Config.GetConfiguration()["ReportEngine"]))
                    {
                        engine = "Emotion";
                    }
                    else
                    {
                        engine = Config.GetConfiguration()["ReportEngine"];
                    }

                    switch (engine.ToLower())
                    {
                        case "emotion":
                            _reportEngine = new Emotion();
                            break;
                        case "orientalsails":
                            _reportEngine = new ReportEngine.OrientalSails();
                            break;
                        case "indochinajunk":
                            _reportEngine = new IndochinaJunk();
                            break;
                    }
                }
                return _reportEngine;
            }
        }

        private bool _tripBased;
        public bool TripBased
        {
            get
            {
                if (string.IsNullOrEmpty(Config.GetConfiguration()["BookingMode"]))
                {
                    return true;
                }
                return Config.GetConfiguration()["BookingMode"] == "TripBased";
            }
        }
    }
}