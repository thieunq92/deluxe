using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.Core.Domain;
using CMS.Core.Util;
using GemBox.Spreadsheet;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Portal.Modules.OrientalSails.BusinessLogic;
using Portal.Modules.OrientalSails.BusinessLogic.Share;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Enums;
using Portal.Modules.OrientalSails.ReportEngine;
using Portal.Modules.OrientalSails.Web.Controls;
using Portal.Modules.OrientalSails.Web.UI;
using Portal.Modules.OrientalSails.Web.Util;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class AgencyBookingView : SailsAgencyAdminBase
    {
        private string ByCustomerId = string.Empty;
        private Booking _booking;
        private IList _extraServices;
        private UserBLL userBLL;
        private EmailService emailService = null;
        public EmailService EmailService
        {
            get
            {
                if (emailService == null)
                    emailService = new EmailService();
                return emailService;
            }
        }
        public UserBLL UserBLL
        {
            get
            {
                if (userBLL == null)
                    userBLL = new UserBLL();
                return userBLL;
            }
        }
        public User CurrentUser
        {
            get
            {
                return UserBLL.UserGetCurrent();
            }
        }
        private PermissionBLL permissionBLL;
        public PermissionBLL PermissionBLL
        {
            get
            {
                if (permissionBLL == null)
                    permissionBLL = new PermissionBLL();
                return permissionBLL;
            }
        }
        private BookingViewBLL bookingViewBLL;
        public BookingViewBLL BookingViewBLL
        {
            get
            {
                if (bookingViewBLL == null)
                {
                    bookingViewBLL = new BookingViewBLL();
                }
                return bookingViewBLL;
            }
        }
        private int BookingId
        {
            get
            {
                int id;
                if (Request.QueryString["BookingId"] != null && Int32.TryParse(Request.QueryString["BookingId"], out id))
                {
                    return id;
                }
                return -1;
            }
        }

        public Booking Booking
        {
            get
            {
                Booking booking = null;
                try
                {
                    booking = BookingViewBLL.BookingGetById(Int32.Parse(Request.QueryString["BookingId"]));
                }
                catch { }
                return booking;
            }
        }
        protected IList ExtraServices
        {
            get
            {
                if (_extraServices == null)
                {
                    _extraServices = Module.ExtraOptionGetAll();
                }
                return _extraServices;
            }
        }

        public bool CanEditTotal
        {
            get
            {
                return PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowEditTotalBooking);
            }
        }
        public bool CanLockIncome
        {
            get
            {
                return PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowLockIncomeBooking);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            cddlBooker.Attributes.Add("onchange", "Validate();");
            cddlTrips.Visible = TripBased;
            plhAccountStatus.Visible = CheckAccountStatus;
            plhServices.Visible = !DetailService;
            aServices.Visible = !DetailService;
            plhEmo.Visible = !DetailService;
            plhDetailService.Visible = DetailService;


            IList list = Module.ExtraOptionGetCustomer();
            foreach (ExtraOption option in list)
            {
                ByCustomerId += "#" + option.Id + "#";
            }

            string vids = string.Empty;
            foreach (SailsTrip trip in Module.TripGetAll(false, UserIdentity))
            {
                if (trip.NumberOfOptions == 2)
                {
                    vids += "#" + trip.Id + "#";
                }
            }
            if (BookingId <= 0)
            {
                PageRedirect(string.Format("AgencyBookingList.aspx?NodeId={0}&SectionId={1}", Node.Id, Section.Id));
                return;
            }

            string stayUrl = string.Format("StayDetail.aspx?NodeId={0}&SectionId={1}&bookingid={2}", Node.Id,
                Section.Id,
                BookingId);
            btnProvisionalDetail.Attributes.Add("onclick", string.Format("window.location='{0}';", stayUrl));
            btnProvisionalDetail.Value = Resources.textProvisionalDetail;
            Title = Resources.titleBookingView;

            if (UseCustomBookingId)
            {
                label_BookingId.Visible = false;
                txtBookingId.Visible = true;
            }
            else
            {
                txtBookingId.Visible = false;
                label_BookingId.Visible = true;
            }

            if (!IsPostBack)
            {
                ddlStatusType.DataSource = Enum.GetNames(typeof(StatusType));
                ddlStatusType.DataBind();

                #region -- Bind agency --
                var listaAgencyUserGetByUser = Module.AgencyUserGetByUser(UserIdentity);
                var agencys = new List<Agency>();
                foreach (AgencyUser agencyUser in listaAgencyUserGetByUser)
                {
                    agencys.Add(agencyUser.Agency);
                }
                ddlAgencies.DataSource = agencys;
                ddlAgencies.DataTextField = "Name";
                ddlAgencies.DataValueField = "Id";
                ddlAgencies.DataBind();

                cddlBooker.DataSource = Module.ContactGetAllEnabled();
                cddlBooker.DataTextField = "Name";
                cddlBooker.DataValueField = "Id";
                cddlBooker.DataParentField = "AgencyId";
                cddlBooker.ParentClientID = ddlAgencies.ClientID;
                cddlBooker.DataBind();
                cddlBooker.Items.Insert(0, "-- Contact --");

                #endregion

                BindTrips();

                LoadInfo();

                // Nếu không còn bất kỳ phòng trống nào thì ẩn khung add phòng
                if (ddlRoomTypes.Items.Count <= 0)
                {
                    //plhAddRoom.Visible = false;
                }

                #region -- Email popup --

                const string centerPopup =
                    @"function PopupCenter(pageURL, title,h,w) {
var left = (screen.width/2)-(w/2);
var top = (screen.height/2)-(h/2);
var targetWin = window.open (pageURL, title, 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width='+w+', height='+h+', top='+top+', left='+left);
targetWin.focus();
return targetWin;
}";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(BookingView), "centerPopup", centerPopup, true);

                string url = string.Format("SendEmail.aspx?NodeId={0}&SectionId={1}&BookId={2}",
                    Node.Id, Section.Id, BookingId);
                btnEmail.Attributes.Add("onclick", string.Format("PopupCenter('{0}','Send email',800,600);", url));
                #endregion
            }

            string servicesUrl = string.Format("BookingServicePrices.aspx?NodeId={0}&SectionId={1}&bookingid={2}",
                Node.Id, Section.Id, BookingId);
            aServices.Attributes.Add("onclick", string.Format("PopupCenter('{0}','',300,400)", servicesUrl));

            string bhurl = string.Format("BookingHistories.aspx?NodeId={0}&SectionId={1}&BookingId={2}",
                Node.Id, Section.Id, BookingId);
            btnViewHistory.Attributes.Add("onclick", string.Format("window.location = '{0}'; return false;", bhurl));

        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (bookingViewBLL != null)
            {
                bookingViewBLL.Dispose();
                bookingViewBLL = null;
            }
            if (userBLL != null)
            {
                userBLL.Dispose();
                userBLL = null;
            }
            if (permissionBLL != null)
            {
                permissionBLL.Dispose();
                permissionBLL = null;
            }
        }
        public string PaxGetDetails()
        {
            return string.Format("Adults : {0}</br> Childs : {1}<br/> Baby : {2}", Booking.Adult, Booking.Child, Booking.Baby);
        }
        protected void BindTrips()
        {
            ddlOrgs.DataSource = Module.OrganizationGetAllRoot();
            ddlOrgs.DataTextField = "Name";
            ddlOrgs.DataValueField = "Id";
            ddlOrgs.DataBind();

            cddlTrips.DataSource = Module.TripGetAll(false);
            cddlTrips.DataTextField = "Name";
            cddlTrips.DataValueField = "Id";
            cddlTrips.DataParentField = "OrgId";
            cddlTrips.ParentClientID = ddlOrgs.ClientID;
            cddlTrips.DataBind();
        }

        private void LoadInfo()
        {
            if (BookingId > 0)
            {
                _booking = Module.BookingGetById(BookingId);
                if (_booking.SeeoffTime != null)
                    txtSeeoffTime.Text = _booking.SeeoffTime.Value.ToString("dd/MM/yyyy HH:mm");
                if (_booking.PickupTime != null)
                    txtPickupTime.Text = _booking.PickupTime.Value.ToString("dd/MM/yyyy HH:mm");
                txtPUFlightDetails.Text = _booking.PUFlightDetails;
                txtPUCarRequirements.Text = _booking.PUCarRequirements;

                txtSOFlightDetails.Text = _booking.SOFlightDetails;
                txtSOCarRequirements.Text = _booking.SOCarRequirements;
                txtPUPickupAddress.Text = _booking.PUPickupAddress;
                txtPUDropoffAddress.Text = _booking.PUDropoffAddress;
                txtSOPickupAddress.Text = _booking.SOPickupAddress;
                txtSODropoffAddress.Text = _booking.SODropoffAddress;
                if (_booking.LockIncome)
                {
                    txtTotal.CssClass += " locked";
                    if (!Module.PermissionCheck(Permission.ACTION_EDIT_AFTER_LOCK, UserIdentity))
                    {
                        txtTotal.ReadOnly = true;
                    }

                    btnLockIncome.Visible = false;
                    litLockIncome.Visible = true;
                    imgLockIncome.Visible = true;

                    litLockIncome.Text =
                        string.Format(
                            "Locked (individual booking) by {0} at {1:dd/MM/yyyy HH:mm}", _booking.LockBy.UserName,
                            _booking.LockDate.Value);

                    if (Module.PermissionCheck(Permission.ACTION_LOCKINCOME, UserIdentity))
                    {
                        btnUnlockIncome.Visible = true;
                    }
                }
                else
                {
                    var expense = Module.ExpenseGetByDate(_booking.Trip, _booking.StartDate);
                    if (expense.LockIncome && !_booking.UnlockIncome)
                    {
                        txtTotal.CssClass += " locked";
                        if (!Module.PermissionCheck(Permission.ACTION_EDIT_AFTER_LOCK, UserIdentity))
                        {
                            txtTotal.ReadOnly = true;
                        }

                        btnLockIncome.Visible = false;
                        litLockIncome.Visible = true;
                        imgLockIncome.Visible = true;

                        if (Module.PermissionCheck(Permission.ACTION_LOCKINCOME, UserIdentity))
                        {
                            btnUnlockIncome.Visible = true;
                        }
                    }
                    else
                    {
                        btnLockIncome.Visible = Module.PermissionCheck(Permission.ACTION_LOCKINCOME, UserIdentity);

                        if (!Module.PermissionCheck(Permission.ACTION_EDIT_TOTAL, UserIdentity))
                        {
                            txtTotal.ReadOnly = true;
                        }
                    }
                }
                if (!Module.PermissionCheck(Permission.INPUT_DRIVER_COLLECT, UserIdentity))
                {
                    txtDriverCollect.ReadOnly = true;
                }

                label_BookingId.Text = string.Format(BookingFormat, _booking.Id);
                if (_booking.Trip != null)
                {
                    if (_booking.Trip.Organization != null)
                    {
                        ddlOrgs.SelectedValue = _booking.Trip.Organization.Id.ToString();
                    }
                    cddlTrips.SelectedValue = _booking.Trip.Id.ToString();
                    if (_booking.Trip.NumberOfDay <= 2)
                    {
                        ddlOptions.Attributes.CssStyle["display"] = "none";
                    }
                }

                if (_booking.Agency == null)
                {
                    txtAgencyCode.Attributes.CssStyle["display"] = "none";
                }
                else
                {
                    txtAgencyCode.Text = _booking.AgencyCode;
                }
                hplBookingDate.Text = _booking.CreatedDate.ToString("dd/MM/yyyy");

                if (_booking.CreatedBy != null)
                {
                    litCreated.Text = _booking.CreatedBy.FullName;
                }
                if (_booking.ConfirmedBy != null)
                {
                    litApprovedBy.Text = _booking.ConfirmedBy.FullName;
                }

                txtStartDate.Text = _booking.StartDate.ToString("dd/MM/yyyy");
                ddlStatusType.SelectedValue = Enum.GetName(typeof(StatusType), _booking.Status);

                if (_booking.Booker != null)
                {
                    cddlBooker.SelectedValue = _booking.Booker.Id.ToString();
                    labelTelephone.Text = _booking.Booker.Phone;
                }

                chkIsPaymentNeeded.Checked = _booking.IsPaymentNeeded;
                txtBookingId.Text = _booking.CustomBookingId.ToString();
                txtCustomerInfo.Text = _booking.Note;
                ddlRoomTypes.Items.Clear();
                foreach (RoomClass rclass in AllRoomClasses)
                {
                    foreach (RoomTypex rtype in AllRoomTypes)
                    {
                        int avaiable;
                        if (TripBased)
                        {
                            avaiable = Module.RoomCount(rclass, rtype, _booking.Cruise, _booking.StartDate,
                                _booking.Trip.NumberOfDay);
                        }
                        else
                        {
                            avaiable = Module.RoomCount(rclass, rtype, _booking.Cruise, _booking.StartDate,
                                (_booking.EndDate - _booking.StartDate).Days);
                        }
                        if (avaiable > 0)
                        {
                            ddlRoomTypes.Items.Add(new ListItem(rtype.Name + " " + rclass.Name,
                                rclass.Id + "|" + rtype.Id));
                        }
                    }
                }
                rptCustomers.DataSource = _booking.Customers;
                rptCustomers.DataBind();
                litPax.Text = _booking.Pax.ToString();


                if (_booking.Agency != null)
                {
                    ddlAgencies.SelectedValue = _booking.Agency.Id.ToString();

                    if (string.IsNullOrEmpty(_booking.PickupAddress))
                    {
                        string defAdd = string.Format("Address: {0}, Tel: {1}", _booking.Agency.Address,
                            _booking.Agency.Phone);
                        _booking.PickupAddress = defAdd;
                    }
                }

                ddlOptions.SelectedIndex = (int)_booking.TripOption;

                if (_booking.IsVND)
                {
                    ddlCurrency.SelectedValue = "true";
                }
                else
                {
                    txtTotal.Text = _booking.Total.ToString("#,0.#");
                    ddlCurrency.SelectedValue = "false";
                }

                if (_booking.IsCommissionVND)
                {
                    txtCommission.Text = _booking.CommissionVND.ToString("#,0.#");
                    ddlCurrencyComission.SelectedValue = "true";
                }
                else
                {
                    txtCommission.Text = _booking.Commission.ToString("#,0.#");
                    ddlCurrencyComission.SelectedValue = "false";
                }

                if (_booking.IsGuideCollectVND)
                {
                    txtGuideCollect.Text = _booking.GuideCollectVND.ToString("#,0.#");
                    ddlCurrencyGuideCollect.SelectedValue = "true";
                }
                else
                {
                    txtGuideCollect.Text = _booking.GuideCollect.ToString("#,0.#");
                    ddlCurrencyGuideCollect.SelectedValue = "false";
                }

                if (_booking.IsDriverCollectVND)
                {
                    txtDriverCollect.Text = _booking.DriverCollectVND.ToString("#,0.#");
                    ddlCurrencyDriverCollect.SelectedValue = "true";
                }
                else
                {
                    txtDriverCollect.Text = _booking.DriverCollect.ToString("#,0.#");
                    ddlCurrencyDriverCollect.SelectedValue = "false";
                }

                if (_booking.IsCancelPayVND)
                {
                    txtPenalty.Text = _booking.CancelPayVND.ToString("#,0.#");
                    ddlCurrencyCPenalty.SelectedValue = "true";
                }
                else
                {
                    txtPenalty.Text = _booking.CancelPay.ToString("#,0.#");
                    ddlCurrencyCPenalty.SelectedValue = "false";
                }

                txtPickup.Text = _booking.PickupAddress;
                txtSpecialRequest.Text = _booking.SpecialRequest;
                if (_booking.AccountingStatus == AccountingStatus.Updated)
                {
                    btnAccountingUpdate.Text = Resources.textSetAsNotUpdated;
                }
                else
                {
                    btnAccountingUpdate.Text = Resources.textSetAsUpdated;
                }

                rptExtraServices.DataSource = Module.ExtraOptionGetBooking();
                rptExtraServices.DataBind();

                rptServices.DataSource = _booking.Services;
                rptServices.DataBind();

                if (_booking.Status == StatusType.Approved && ApprovedLock)
                {
                    foreach (ListItem item in cddlTrips.Items)
                    {
                        if (!item.Selected) item.Enabled = false;
                    }
                    txtStartDate.ReadOnly = true;
                    foreach (ListItem item in ddlAgencies.Items)
                    {
                        if (!item.Selected) item.Enabled = false;
                    }
                    foreach (ListItem item in cddlBooker.Items)
                    {
                        if (!item.Selected) item.Enabled = false;
                    }
                    txtTotal.ReadOnly = true;
                    chkIsPaymentNeeded.Enabled = false;
                    txtPenalty.ReadOnly = true;
                    btnAddService.Visible = false;
                    txtPickup.ReadOnly = true;
                    txtSpecialRequest.ReadOnly = true;
                    txtCustomerInfo.ReadOnly = true;
                }

                if (_booking.CreatedBy != null)
                {
                    litCreatedTime.Text = string.Format("Created by {0} at {1:dd/MM/yyyy HH:mm}", _booking.CreatedBy.UserName, _booking.CreatedDate);
                }

                if (_booking.ModifiedBy != null)
                {
                    litLastEdited.Text = string.Format("Last edited by {0} at {1:dd/MM/yyyy HH:mm}",
                        _booking.ModifiedBy.UserName, _booking.ModifiedDate);
                }
            }

            if (Request.QueryString["Notify"] != null)
            {
                if (Convert.ToInt32(Request.QueryString["Notify"]) == 0)
                {
                    chkSendMail.Checked = false;
                }
            }
        }
        protected void rptRoomList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (CustomPriceForRoom)
            {
                IList policies;
                if (_booking.Agency != null && _booking.Agency.Role != null)
                {
                    policies = Module.AgencyPolicyGetByRole(_booking.Agency.Role);
                }
                else
                {
                    policies = Module.AgencyPolicyGetByRole(Module.RoleGetById(4));
                }
                CustomersInfo.rptRoomList_itemDataBound(sender, e, Module, CustomPriceForRoom, policies, this,
                    ddlRoomTypes.Items);
            }
            else
            {
                CustomersInfo.rptRoomList_itemDataBound(sender, e, Module, false, null, this, ddlRoomTypes.Items);
            }
        }

        protected void buttonCancel_Click(object sender, EventArgs e)
        {
            PageRedirect(string.Format("AgencyBookingList.aspx?NodeId={0}&SectionId={1}", Node.Id, Section.Id));
        }

        protected void buttonSubmit_Click(object sender, EventArgs e)
        {
            if (!Module.PermissionCheck(Permission.ACTION_EDITBOOKING, UserIdentity))
            {
                ShowError(Resources.textDeniedFunction);
                return;
            }

            _booking = Module.BookingGetById(BookingId);
            try
            {
                _booking.SeeoffTime = DateTime.ParseExact(Request.Form[txtSeeoffTime.UniqueID], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                _booking.SeeoffTime = null;
            }

            try
            {
                _booking.PickupTime = DateTime.ParseExact(Request.Form[txtPickupTime.UniqueID], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                _booking.PickupTime = null;
            }

            _booking.PUFlightDetails = Request.Form[txtPUFlightDetails.UniqueID];

            _booking.PUCarRequirements = Request.Form[txtPUCarRequirements.UniqueID];

            _booking.SOFlightDetails = Request.Form[txtSOFlightDetails.UniqueID];

            _booking.SOCarRequirements = Request.Form[txtSOCarRequirements.UniqueID];

            _booking.PUPickupAddress = Request.Form[txtPUPickupAddress.UniqueID];

            _booking.PUDropoffAddress = Request.Form[txtPUDropoffAddress.UniqueID];

            _booking.SOPickupAddress = Request.Form[txtSOPickupAddress.UniqueID];

            _booking.SODropoffAddress = Request.Form[txtSODropoffAddress.UniqueID];
            var discountAmount = 0.0;
            try
            {
                discountAmount = Double.Parse(txtDiscountAmount.Text);
            }
            catch { }
            _booking.DiscountAmount = discountAmount;
            var discountPercent = 0.0;
            try
            {
                discountPercent = double.Parse(txtDiscountPercent.Text);
            }
            catch { }
            if (String.IsNullOrEmpty(txtDiscountPercent.Text)) { _booking.DiscountPercent = null; }
            if (!String.IsNullOrEmpty(txtDiscountPercent.Text)) { _booking.DiscountPercent = discountPercent; }
            _booking.DiscountCurrencyType = ddlCurrency.SelectedValue;
            BookingTrack track = new BookingTrack();
            track.ModifiedDate = DateTime.Now;
            track.User = UserIdentity;
            track.Booking = _booking;

            IList changes = new ArrayList();

            foreach (RepeaterItem item in rptCustomers.Items)
            {
                CustomerInfoRowInput customerInfo1 = item.FindControl("customerData") as CustomerInfoRowInput;
                if (customerInfo1 != null)
                {
                    Customer customer1 = customerInfo1.NewCustomer(Module);
                    customer1.Booking = _booking;
                    Module.SaveOrUpdate(customer1);

                    Repeater rptService1 = item.FindControl("rptServices1") as Repeater;
                    if (rptService1 != null)
                    {
                        if (DetailService)
                        {
                            CustomerServiceRepeaterHandler.Save(rptService1, Module, customer1);
                        }
                    }
                }
            }

            bool isApproved = false;
            StatusType statusType = (StatusType)Enum.Parse(typeof(StatusType), ddlStatusType.SelectedValue);
            if (_booking.Status != statusType)
            {
                BookingChanged change = new BookingChanged();
                change.Parameter = string.Empty;
                if (_booking.Status == StatusType.Rejected || _booking.Status == StatusType.Cancelled)
                {
                    if (statusType == StatusType.Approved || statusType == StatusType.Pending)
                    {
                        change.Action = BookingAction.Approved;
                        change.Parameter = _booking.Total.ToString();
                        changes.Add(change);
                    }
                }
                if (_booking.Status == StatusType.Approved || _booking.Status == StatusType.Pending)
                {
                    if (statusType == StatusType.Cancelled || statusType == StatusType.Rejected)
                    {
                        change.Action = BookingAction.Cancelled;
                        change.Parameter = _booking.Total.ToString();
                        changes.Add(change);
                    }
                }

                if (_booking.ConfirmedBy == null)
                {
                    _booking.ConfirmedBy = UserIdentity;
                }
                _booking.Status = statusType;
                if (_booking.Status == StatusType.Approved)
                {
                    isApproved = true;
                }
            }

            if (!string.IsNullOrEmpty(ddlAgencies.SelectedValue))
            {
                if (_booking.Agency == null || _booking.Agency.Id.ToString() != ddlAgencies.SelectedValue)
                {
                    _booking.Agency = Module.AgencyGetById(Convert.ToInt32(ddlAgencies.SelectedValue));
                }
            }
            else
            {
                _booking.Agency = null;
            }
            DateTime date = DateTime.ParseExact(txtStartDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            SailsTrip trip = null;
            if (_booking.Trip != null && _booking.Trip.Id.ToString() != cddlTrips.SelectedValue ||
                _booking.StartDate != date || isApproved)
            {
                trip = Module.TripGetById(Convert.ToInt32(cddlTrips.SelectedValue));
            }
            if (TripBased)
            {
                if (trip != null)
                {
                    _booking.Trip = trip;
                }
            }
            else
            {
                _booking.Trip = null;
            }
            _booking.StartDate = date;
            _booking.PickupAddress = txtPickup.Text;
            string defAdd = string.Empty;
            if (_booking.Agency != null)
            {
                defAdd = string.Format("Address: {0}, Tel: {1}", _booking.Agency.Address,
                    _booking.Agency.Phone);

                if (string.IsNullOrEmpty(_booking.PickupAddress) || _booking.PickupAddress == defAdd)
                {
                    if (string.IsNullOrEmpty(_booking.BookerName))
                    {
                        _booking.PickupAddress = string.Format("{0}, Tel: {1}", _booking.Agency.Address,
                            _booking.Agency.Phone);
                    }
                    else
                    {
                        _booking.PickupAddress = string.Format("{0}, Tel: {1}, Booker: {2}",
                            _booking.Agency.Address, _booking.Agency.Phone,
                            _booking.BookerName);
                    }
                }
            }

            _booking.SpecialRequest = txtSpecialRequest.Text;
            if (cddlBooker.SelectedIndex > 0)
            {
                _booking.Booker = Module.ContactGetById(Convert.ToInt32(cddlBooker.SelectedValue));
            }

            if (TripBased)
            {
                if (_booking.Trip.NumberOfDay == 3)
                {
                    _booking.TripOption = (TripOption)ddlOptions.SelectedIndex;
                }
            }

            _booking.AgencyCode = txtAgencyCode.Text;
            _booking.IsPaymentNeeded = chkIsPaymentNeeded.Checked;
            _booking.CustomBookingId = Convert.ToInt32(txtBookingId.Text.Replace(" ", ""));
            _booking.Note = txtCustomerInfo.Text;

            double total = !string.IsNullOrEmpty(txtTotal.Text) ? Convert.ToDouble(txtTotal.Text) : 0.0;
            double finalTotal = total;

            if (_booking.CurrencyRate == 0)
            {
                var rate = Module.ExchangeGetByDate(_booking.CreatedDate);
                _booking.CurrencyRate = rate.Rate;
            }

            if (ddlCurrency.SelectedValue == "true")
            {
                _booking.IsVND = true;
                if (_booking.TotalVND != finalTotal)
                {
                    _booking.TotalVND = finalTotal;
                    _booking.Total = 0.0;
                    _booking.GuideConfirmed = false;
                    _booking.AgencyConfirmed = false;
                    _booking.IsPaid = false;
                }
            }
            else
            {
                _booking.IsVND = false;
                if (_booking.Total != finalTotal)
                {
                    _booking.Total = finalTotal;
                    _booking.TotalVND = 0.0;
                    _booking.GuideConfirmed = false;
                    _booking.AgencyConfirmed = false;
                    _booking.IsPaid = false;
                }
            }

            if (ddlCurrencyComission.SelectedValue == "true")
            {
                _booking.IsCommissionVND = true;
                _booking.CommissionVND = !string.IsNullOrEmpty(txtCommission.Text) ? Convert.ToDouble(txtCommission.Text) : 0.0;
                _booking.Commission = 0.0;
            }
            else
            {
                _booking.IsCommissionVND = false;
                _booking.Commission = !string.IsNullOrEmpty(txtCommission.Text) ? Convert.ToDouble(txtCommission.Text) : 0.0;
                _booking.CommissionVND = 0.0;
            }

            if (ddlCurrencyCPenalty.SelectedValue == "true")
            {
                _booking.IsCancelPayVND = true;
                _booking.CancelPayVND = !string.IsNullOrEmpty(txtPenalty.Text) ? Convert.ToDouble(txtPenalty.Text) : 0.0;
                _booking.CancelPay = 0.0;
            }
            else
            {
                _booking.IsCancelPayVND = false;
                _booking.CancelPay = !string.IsNullOrEmpty(txtPenalty.Text) ? Convert.ToDouble(txtPenalty.Text) : 0.0;
                _booking.CancelPayVND = 0.0;
            }

            var guideCollect = !string.IsNullOrEmpty(txtGuideCollect.Text) ? Convert.ToDouble(txtGuideCollect.Text) : 0.0;
            if (ddlCurrencyGuideCollect.SelectedValue == "true")
            {
                _booking.IsGuideCollectVND = true;
                if (_booking.GuideCollectVND != guideCollect)
                {
                    _booking.IsPaid = false;
                    _booking.GuideCollectVND = guideCollect;
                    _booking.GuideCollect = 0.0;
                    _booking.GuideConfirmed = false;
                    _booking.AgencyConfirmed = false;
                }
            }
            else
            {
                _booking.IsGuideCollectVND = false;
                if (_booking.GuideCollect != guideCollect)
                {
                    _booking.IsPaid = false;
                    _booking.GuideCollect = guideCollect;
                    _booking.GuideCollectVND = 0.0;
                    _booking.GuideConfirmed = false;
                    _booking.AgencyConfirmed = false;
                }

            }

            var driverCollect = !string.IsNullOrEmpty(txtDriverCollect.Text) ? Convert.ToDouble(txtDriverCollect.Text) : 0.0;
            if (ddlCurrencyDriverCollect.SelectedValue == "true")
            {
                _booking.IsDriverCollectVND = true;
                if (_booking.DriverCollectVND != driverCollect)
                {
                    _booking.IsPaid = false;
                    _booking.DriverCollectVND = driverCollect;
                    _booking.DriverCollect = 0.0;
                    _booking.GuideConfirmed = false;
                    _booking.AgencyConfirmed = false;
                }
            }
            else
            {
                _booking.IsGuideCollectVND = false;
                if (_booking.DriverCollect != driverCollect)
                {
                    _booking.IsPaid = false;
                    _booking.DriverCollect = driverCollect;
                    _booking.DriverCollectVND = 0.0;
                    _booking.GuideConfirmed = false;
                    _booking.AgencyConfirmed = false;
                }

            }

            if (UseCustomBookingId && !Module.CheckCustomBookingCode(_booking.CustomBookingId, _booking))
            {
                ShowError(Resources.errorDuplicateBookingCode);
                return;
            }
            if (changes.Count > 0 && !isApproved && _booking.Status == StatusType.Approved)
            {
                _booking.Amended++;
            }

            if (!ApprovedLock || _booking.Status != StatusType.Approved || isApproved)
            {
                _booking.ModifiedBy = UserIdentity;
                _booking.ModifiedDate = DateTime.Now;
                Module.Update(_booking, UserIdentity);
            }

            if (changes.Count > 0)
            {
                Module.SaveOrUpdate(track);
                foreach (BookingChanged change in changes)
                {
                    change.Track = track;
                    Module.SaveOrUpdate(change);
                }
            }

            foreach (RepeaterItem item in rptExtraServices.Items)
            {
                var hiddenId = (HiddenField)item.FindControl("hiddenId");
                var chkService = (CheckBox)item.FindControl("chkService");
                var option = Module.ExtraOptionGetById(Convert.ToInt32(hiddenId.Value));
                var extra = Module.BookingExtraGet(_booking, option);
                if (extra.Id <= 0 && chkService.Checked)
                {
                    Module.SaveOrUpdate(extra);
                }
                if (extra.Id > 0 && !chkService.Checked)
                {
                    Module.Delete(extra);
                }
            }

            IList bkservice = rptServicesToIList(_booking);
            foreach (BookingService service in bkservice)
            {
                if (!service.Deleted)
                {
                    Module.SaveOrUpdate(service);
                }
                else
                {
                    if (service.Id > 0)
                    {
                        Module.Delete(service);
                    }
                }
            }
            SendEmailNotif(_booking);
            PageRedirect(string.Format("BookingReport.aspx?NodeId={0}&SectionId={1}&Date={2}", Node.Id, Section.Id, _booking.StartDate.ToOADate()));

        }

        protected void rptRoomList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName.ToLower())
            {
                case "delete":
                    BookingRoom booking = Module.BookingRoomGetById(Convert.ToInt32(e.CommandArgument));
                    Module.Delete(booking, UserIdentity);
                    PageRedirect(Request.RawUrl);
                    break;
            }
        }

        protected void btnAddRoom_Click(object sender, EventArgs e)
        {
            _booking = Module.BookingGetById(BookingId);

            Customer customer1 = new Customer();
            customer1.Booking = _booking;
            customer1.Type = CustomerType.Adult;
            Module.SaveOrUpdate(customer1);
            var history = new BookingHistory();
            history.Booking = Booking;
            history.Date = DateTime.Now;
            history.Agency = Booking.Agency;
            history.StartDate = Booking.StartDate;
            history.Status = Booking.Status;
            history.Trip = Booking.Trip;
            history.User = UserIdentity;
            history.SpecialRequest = Booking.SpecialRequest;
            history.NumberOfPax = (Booking.Adult + Booking.Child + Booking.Baby);
            history.CustomerInfo = Booking.Note;
            Module.SaveOrUpdate(history);
            PageRedirect(Request.RawUrl);
        }

        protected void lbtCalculate_Click(object sender, EventArgs e)
        {
            _booking = Module.BookingGetById(BookingId);
            Agency _oldAgency = null;
            if (ddlAgencies.SelectedIndex > 0)
            {
                _oldAgency = _booking.Agency;
                _booking.Agency = Module.AgencyGetById(Convert.ToInt32(ddlAgencies.SelectedValue));
            }

            double total;
            Domain.SailsTrip sailsTrip = Module.TripGetById(Int32.Parse(cddlTrips.SelectedValue));
            Domain.SailsPriceConfig sailsPriceConfig = Module.SailsPriceConfigGetByNearestValidFrom(sailsTrip, _booking.TripOption, _booking.CreatedDate);

            int adult = 0;
            int child = 0;
            int baby = 0;

            foreach (RepeaterItem item in rptCustomers.Items)
            {
                var customerData = item.FindControl("customerData") as CustomerInfoRowInput;
                var ddlCustomer = customerData.FindControl("ddlCustomerType") as DropDownList;

                switch (ddlCustomer.SelectedItem.Text)
                {
                    case "Adult":
                        adult += 1;
                        break;
                    case "Child":
                        child += 1;
                        break;
                    case "Baby":
                        baby += 1;
                        break;
                    default:
                        break;
                }

            }

            if (ddlCurrency.SelectedValue == "false")
                total = adult * sailsPriceConfig.PriceAdultUSD + child * sailsPriceConfig.PriceChildUSD + baby * sailsPriceConfig.PriceBabyUSD;
            else
                total = adult * sailsPriceConfig.PriceAdultVND + child * sailsPriceConfig.PriceChildVND + baby * sailsPriceConfig.PriceBabyVND;
            txtTotal.Text = total.ToString("#,0.#");

            Agency agency = Module.AgencyGetById(Convert.ToInt32(ddlAgencies.SelectedValue));
            AgencyLevel agencyLevel = agency.AgencyLevel;
            SailsTrip trip = Module.TripGetById(Convert.ToInt32(cddlTrips.SelectedValue));

            double totalCommission = 0.0;
            Domain.AgencyCommission agencyCommission = Module.AgencyCommissionGetByNearestValidFrom(sailsTrip, agencyLevel, _booking.CreatedDate);
            if (ddlCurrency.SelectedValue == "false")
                totalCommission = adult * agencyCommission.CommissionAdultUSD + child * agencyCommission.CommissionChildUSD + baby * agencyCommission.CommissionChildUSD;
            else
                totalCommission = adult * agencyCommission.CommissionAdultVND + child * agencyCommission.CommissionChildVND + baby * agencyCommission.CommissionBabyVND;
            txtCommission.Text = totalCommission.ToString("#,0.#");
            if (ddlAgencies.SelectedIndex > 0)
            {
                _booking.Agency = _oldAgency;
            }
        }

        protected void rptExtraServices_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            CheckBox chkService = (CheckBox)e.Item.FindControl("chkService");
            ExtraOption option = (ExtraOption)e.Item.DataItem;
            chkService.Checked = _booking.ExtraServices.Contains(option);
            chkService.Text = option.Name;
        }

        protected void btnAccountingUpdate_Click(object sender, EventArgs e)
        {
            _booking = Module.BookingGetById(BookingId);
            if (btnAccountingUpdate.Text == Resources.textSetAsUpdated)
            {
                _booking.AccountingStatus = AccountingStatus.Updated;
                btnAccountingUpdate.Text = Resources.textSetAsNotUpdated;
            }
            else
            {
                btnAccountingUpdate.Text = Resources.textSetAsUpdated;
                _booking.AccountingStatus = AccountingStatus.Modified;
            }
            _booking.ModifiedBy = UserIdentity;
            _booking.ModifiedDate = DateTime.Now;
            Module.SaveOrUpdate(_booking);
        }
        protected void btnConfirmation_Click(object sender, EventArgs e)
        {
            _booking = Module.BookingGetById(Convert.ToInt32(Request.QueryString["bookingid"]));
            BookingConfirmation.Emotion(_booking, this, Response,
                Server.MapPath("/Modules/Sails/Admin/ExportTemplates/BkConfirm.xls"));
        }

        protected void rptCustomers_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is Customer)
            {
                Customer customer = (Customer)e.Item.DataItem;
                CustomerInfoRowInput customerData = e.Item.FindControl("customerData") as CustomerInfoRowInput;
                if (customerData != null)
                {
                    customerData.GetCustomer(customer, Module);
                }
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Button btnDelete = (Button)sender;
            RepeaterItem item = (RepeaterItem)btnDelete.Parent;
            CustomerInfoRowInput customerData = item.FindControl("customerData") as CustomerInfoRowInput;
            if (customerData != null)
            {
                Customer customer = customerData.NewCustomer(Module);
                Module.Delete(customer);
                var history = new BookingHistory();
                history.Booking = Booking;
                history.Date = DateTime.Now;
                history.Agency = Booking.Agency;
                history.StartDate = Booking.StartDate;
                history.Status = Booking.Status;
                history.Trip = Booking.Trip;
                history.User = UserIdentity;
                history.SpecialRequest = Booking.SpecialRequest;
                history.NumberOfPax = (Booking.Adult + Booking.Child + Booking.Baby);
                history.CustomerInfo = Booking.Note;
                Module.SaveOrUpdate(history);
            }
            PageRedirect(Request.RawUrl);
        }

        protected void btnAddService_Click(object sender, EventArgs e)
        {
            _booking = Module.BookingGetById(Convert.ToInt32(Request.QueryString["bookingid"]));
            IList list = rptServicesToIList(_booking);
            list.Add(new BookingService());
            rptServices.DataSource = list;
            rptServices.DataBind();
        }

        protected void rptServices_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is BookingService)
            {
                BookingService service = (BookingService)e.Item.DataItem;
                HiddenField hiddenId = (HiddenField)e.Item.FindControl("hiddenId");
                DropDownList ddlService = (DropDownList)e.Item.FindControl("ddlService");
                TextBox txtUnitPrice = (TextBox)e.Item.FindControl("txtUnitPrice");
                TextBox txtQuantity = (TextBox)e.Item.FindControl("txtQuantity");
                CheckBox chkByCustomer = (CheckBox)e.Item.FindControl("chkByCustomer");

                chkByCustomer.Attributes.Add("onchange",
                    string.Format("setDefault('{0}','{1}','{2}');", chkByCustomer.ClientID,
                        txtQuantity.ClientID, litPax.Text));
                ddlService.Attributes.Add("onchange",
                    string.Format("serviceChanged('{0}','{1}','{2}','{3}','{4}');",
                        ddlService.ClientID, chkByCustomer.ClientID,
                        txtQuantity.ClientID, litPax.Text, ByCustomerId));

                ddlService.DataSource = ExtraServices;
                ddlService.DataTextField = "Name";
                ddlService.DataValueField = "Id";
                ddlService.DataBind();

                ListItem item = ddlService.Items.FindByText(service.Name);
                if (item == null && !string.IsNullOrEmpty(service.Name))
                {
                    ddlService.Items.Add(service.Name);
                    ddlService.Items[ddlService.Items.Count - 1].Selected = true;
                }
                else if (item != null)
                {
                    item.Selected = true;
                }

                hiddenId.Value = service.Id.ToString();
                chkByCustomer.Checked = service.IsByCustomer;
                txtUnitPrice.Text = service.UnitPrice.ToString();
                txtQuantity.Text = service.Quantity.ToString();
            }
        }

        protected IList rptServicesToIList(Booking booking)
        {
            IList result = new ArrayList();
            foreach (RepeaterItem item in rptServices.Items)
            {
                HiddenField hiddenId = (HiddenField)item.FindControl("hiddenId");
                DropDownList ddlService = (DropDownList)item.FindControl("ddlService");
                TextBox txtUnitPrice = (TextBox)item.FindControl("txtUnitPrice");
                TextBox txtQuantity = (TextBox)item.FindControl("txtQuantity");
                CheckBox chkByCustomer = (CheckBox)item.FindControl("chkByCustomer");
                BookingService service = new BookingService();
                service.Id = Convert.ToInt32(hiddenId.Value);
                service.Name = ddlService.SelectedItem.Text;
                service.UnitPrice = Convert.ToDouble(txtUnitPrice.Text);
                service.Quantity = Convert.ToInt32(txtQuantity.Text);
                service.Booking = booking;
                service.IsByCustomer = chkByCustomer.Checked;
                service.Deleted = !item.Visible;
                result.Add(service);
            }
            return result;
        }

        protected void rptServices_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.ToLower() == "remove")
            {
                e.Item.Visible = false;
            }
        }

        private string _callbackResult;
        public string GetCallbackResult()
        {
            return _callbackResult;
        }

        protected void btnInvoice_Click(object sender, EventArgs e)
        {
            _booking = Module.BookingGetById(Convert.ToInt32(Request.QueryString["bookingid"]));
            Invoice(_booking, this, Response,
                Server.MapPath("/Modules/Sails/Admin/ExportTemplates/invoice.xls"));

        }

        protected void btnLockIncome_Click(object sender, EventArgs e)
        {
            if (!Module.PermissionCheck(Permission.ACTION_LOCKINCOME, UserIdentity))
            {
                ShowError("You do not have permission");
                return;
            }

            _booking = Module.BookingGetById(BookingId);
            _booking.LockDate = DateTime.Now;
            _booking.LockBy = UserIdentity;
            Module.Update(_booking, null);

            PageRedirect(Request.RawUrl);
        }

        protected void btnUnlockIncome_Click(object sender, EventArgs e)
        {
            if (!Module.PermissionCheck(Permission.ACTION_LOCKINCOME, UserIdentity))
            {
                ShowError("You do not have permission");
                return;
            }



            _booking = Module.BookingGetById(BookingId);
            _booking.LockDate = DateTime.Today.AddDays(2);
            _booking.LockBy = UserIdentity;
            Module.Update(_booking, null);

            PageRedirect(Request.RawUrl);
        }

        public void Invoice(Booking booking, SailsAdminBasePage Page, HttpResponse Response, string templatePath)
        {
            var excelFile = new ExcelFile();
            excelFile.LoadXls(templatePath);
            var sheet = excelFile.Worksheets[0];

            if (booking.Agency != null)
            {
                sheet.Cells["B6"].Value = booking.Agency.Name;
                sheet.Cells["B7"].Value = booking.Agency.Phone;
                sheet.Cells["B8"].Value = booking.Agency.Fax;
            }
            sheet.Cells["E4"].Value = "CODE: " + string.Format(BookingFormat, booking.Id);
            sheet.Cells["F5"].Value = booking.StartDate;

            if (!string.IsNullOrEmpty(booking.BookerName))
            {
                sheet.Cells["A11"].Value = booking.BookerName;
            }
            sheet.Cells["B11"].Value = booking.Pax;
            sheet.Cells["F11"].Value = booking.CreatedDate;
            sheet.Cells["A15"].Value = booking.StartDate;
            sheet.Cells["B15"].Value = booking.Trip.Name;
            sheet.Cells["D15"].Value = booking.Total / booking.Pax;
            sheet.Cells["E15"].Value = booking.Pax;
            sheet.Cells["F15"].Value = booking.Total;
            if (booking.CreatedBy != null)
            {
                sheet.Cells["A28"].Value = booking.CreatedBy.FullName;
                sheet.Cells["B29"].Value = booking.CreatedBy.Website;
                sheet.Cells["B30"].Value = booking.CreatedBy.Email;
            }

            foreach (Customer c in booking.Customers)
            {
                if (!string.IsNullOrEmpty(c.Fullname))
                {
                    sheet.Cells["B4"].Value = c.Fullname;
                    break;
                }
            }

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AppendHeader("content-disposition",
                "attachment; filename=" + string.Format("invoice-{0}.xls", booking.Id));

            MemoryStream m = new MemoryStream();

            excelFile.SaveXls(m);

            HSSFWorkbook workbook = new HSSFWorkbook(m);
            Sheet wsheet = workbook.GetSheetAt(0);
            HSSFClientAnchor anchor;
            anchor = new HSSFClientAnchor(0, 0, 0, 255, 0, 0, 4, 7);
            anchor.AnchorType = 2;
            HSSFPatriarch patriarch = (HSSFPatriarch)wsheet.CreateDrawingPatriarch();
            HSSFPicture picture = (HSSFPicture)patriarch.CreatePicture(anchor, LoadImage(templatePath.Replace("invoice.xls", "logo.jpg"), workbook));
            picture.Resize();

            MemoryStream output = new MemoryStream();
            workbook.Write(output);

            Response.OutputStream.Write(output.GetBuffer(), 0, output.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();

            m.Close();
            output.Close();
            Response.End();
        }

        public static int LoadImage(string path, HSSFWorkbook wb)
        {
            FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[file.Length];
            file.Read(buffer, 0, (int)file.Length);
            return wb.AddPicture(buffer, PictureType.JPEG);

        }

        public double GetPublicPrice()
        {
            SailsTrip trip = null;
            DateTime startDate = DateTime.MinValue;
            var currency = "VND";
            var publicPrice = 0.0;

            if (!IsPostBack)
            {
                trip = Booking.Trip;
                startDate = Booking.StartDate;
                currency = Booking.IsVND ? "VND" : "USD";
            }
            else
            {
                var tripId = 0;
                try
                {
                    tripId = Int32.Parse(cddlTrips.SelectedValue);
                }
                catch { }
                trip = BookingViewBLL.SailsTripGetById(tripId);
                try
                {
                    startDate = DateTime.ParseExact(txtStartDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                catch { }
                currency = bool.Parse(ddlCurrency.SelectedValue) ? "VND" : "USD";
            }
            var sailsPriceConfig = BookingViewBLL.SailsPriceConfigGetByTripAndStartDate(trip, startDate);
            if (sailsPriceConfig == null || sailsPriceConfig.Id == 0) { return publicPrice; }
            if (currency == "VND")
            {
                publicPrice = (sailsPriceConfig.PriceAdultVND * Booking.Adult)
                              + (sailsPriceConfig.PriceBabyVND * Booking.Baby)
                              + (sailsPriceConfig.PriceChildVND * Booking.Child);
            }
            if (currency == "USD")
            {
                publicPrice = (sailsPriceConfig.PriceAdultUSD * Booking.Adult)
                              + (sailsPriceConfig.PriceBabyUSD * Booking.Baby)
                              + (sailsPriceConfig.PriceChildUSD * Booking.Child);
            }
            return publicPrice;
        }

        public double GetTACommission()
        {
            SailsTrip trip = null;
            DateTime startDate = DateTime.MinValue;
            var currency = "VND";
            AgencyLevel agencyLevel = null;
            var TAcommission = 0.0;
            if (!IsPostBack)
            {
                trip = Booking.Trip;
                startDate = Booking.StartDate;
                currency = Booking.IsVND ? "VND" : "USD";
                try
                {
                    agencyLevel = Booking.Agency.AgencyLevel;
                }
                catch { }
            }
            else
            {
                var tripId = 0;
                try
                {
                    tripId = Int32.Parse(cddlTrips.SelectedValue);
                }
                catch { }
                trip = BookingViewBLL.SailsTripGetById(tripId);
                try
                {
                    startDate = DateTime.ParseExact(txtStartDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                catch { }
                currency = bool.Parse(ddlCurrency.SelectedValue) ? "VND" : "USD";
                var agencyId = 0;
                try
                {
                    agencyId = Int32.Parse(ddlAgencies.SelectedValue);
                }
                catch { }
                var agency = BookingViewBLL.AgencyGetById(agencyId);
                if (agency == null || agency.Id == 0) { agencyLevel = null; }
                agencyLevel = agency.AgencyLevel;
            }
            var agencyCommission = BookingViewBLL.AgencyCommissionGetBy(trip, startDate, agencyLevel);
            if (agencyCommission == null || agencyCommission.Id == 0) { return TAcommission; }
            if (currency == "VND")
            {
                TAcommission = (agencyCommission.CommissionAdultVND * Booking.Adult)
                               + (agencyCommission.CommissionBabyVND * Booking.Baby)
                               + (agencyCommission.CommissionChildVND * Booking.Child);
            }
            if (currency == "USD")
            {
                TAcommission = (agencyCommission.CommissionAdultUSD * Booking.Adult)
                               + (agencyCommission.CommissionBabyUSD * Booking.Baby)
                               + (agencyCommission.CommissionChildUSD * Booking.Child);
            }
            return TAcommission;

        }

        public double GetReceivable()
        {
            return GetPublicPrice() - GetTACommission();
        }

        protected void txtTotal_Load(object sender, EventArgs e)
        {
            var total = 0.0;
            total = Booking.IsVND ? Booking.TotalVND : Booking.Total;
            if (!IsPostBack)
            {
                if (Booking.IsVND)
                {
                    total = Booking.TotalVND == 0 ? GetPublicPrice() - GetTACommission() : Booking.TotalVND;
                }
            }
            else
            {
                try
                {
                    total = Double.Parse(txtTotal.Text);
                }
                catch { }
                if (total == 0.0)
                {
                    if (Booking.IsVND)
                    {
                        total = GetPublicPrice() - GetTACommission();
                    }
                }
            }
            txtTotal.Text = total.ToString("#,0.##");
        }

        protected void txtDiscountAmount_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtDiscountAmount.Text = Booking.DiscountAmount.ToString("#,0.##");
            }
        }

        protected void txtPercentDiscount_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtDiscountPercent.Text = Booking.DiscountPercent.HasValue ? Booking.DiscountPercent.Value.ToString("#,0.##") : "";
            }
        }

        public double GetActualReceivable()
        {
            var total = 0.0;
            var discountAmount = 0.0;
            discountAmount = GetDiscountAmount();
            total = GetTotal();
            return total - discountAmount;
        }
        public double GetTotal()
        {
            var total = 0.0;
            if (!IsPostBack) { total = Booking.IsVND ? Booking.TotalVND : Booking.Total; }
            if (IsPostBack)
            {
                try
                {
                    total = Double.Parse(txtTotal.Text);
                }
                catch { }
            }
            return total;
        }
        public double GetDiscountAmount()
        {
            var discountAmount = 0.0;
            var publicPrice = 0.0;
            var discountPercent = 0.0;
            publicPrice = GetPublicPrice();
            discountPercent = GetDiscountPercent();
            if (!IsPostBack)
            {
                if (Booking.DiscountPercent == null) { discountAmount = Booking.DiscountAmount; }
                if (Booking.DiscountPercent != null) { discountAmount = (publicPrice * discountPercent / 100); }
            }
            if (IsPostBack)
            {
                if (String.IsNullOrEmpty(txtDiscountPercent.Text))
                {
                    try
                    {
                        discountAmount = Double.Parse(txtDiscountAmount.Text);
                    }
                    catch { }
                }
                if (!String.IsNullOrEmpty(txtDiscountPercent.Text)) { discountAmount = (publicPrice * discountPercent / 100); }
            }
            return discountAmount;
        }

        public double GetDiscountPercent()
        {
            var discountPercent = 0.0;
            if (!IsPostBack) { discountPercent = Booking.DiscountPercent.HasValue ? Booking.DiscountPercent.Value : 0.0; }
            if (IsPostBack)
            {
                try
                {
                    discountPercent = Double.Parse(txtDiscountPercent.Text);
                }
                catch { }
            }
            return discountPercent;
        }
        public void SendEmailNotif(Booking booking)
        {
            try
            {
                string content = "";
                using (StreamReader streamReader = new StreamReader(HostingEnvironment.MapPath("/Modules/Sails/Admin/EmailTemplate/CreateBookingNotify.txt")))
                {
                    content = streamReader.ReadToEnd();
                };
                var appPath = string.Format("{0}://{1}{2}{3}",
                    Request.Url.Scheme,
                    Request.Url.Host,
                    Request.Url.Port == 80
                        ? string.Empty
                        : ":" + Request.Url.Port,
                    Request.ApplicationPath);
                content = content.Replace("{link}",
                    appPath + "Modules/Sails/Admin/AgencyBookingView.aspx?NodeId=1&SectionId=15&BookingId=" + booking.Id);
                content = content.Replace("{bookingcode}", booking.BookingIdOS);
                content = content.Replace("{agency}", booking.Agency.Name);
                content = content.Replace("{startdate}", booking.StartDate.ToString("dd/MM/yyyy"));
                content = content.Replace("{trip}", booking.Trip.Name);
                content = content.Replace("{customernumber}", (Booking.Adult + Booking.Child + Booking.Baby).ToString());
                content = content.Replace("{submiter}", UserIdentity.FullName);
                
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(ConfigurationManager.AppSettings["EmailId"], "Vietnam Deluxe Group Tours");
                if (booking.CreatedBy != null)
                {
                    if (!string.IsNullOrEmpty(booking.CreatedBy.Email))
                    {
                        if (booking.CreatedBy.Email != "reservation@atravelmate.com")
                        {
                            mail.To.Add(booking.CreatedBy.Email);
                        }
                    }
                }

                if (booking.ModifiedBy != null)
                {
                    if (!string.IsNullOrEmpty(booking.ModifiedBy.Email))
                    {
                        if (booking.CreatedBy != null && booking.ModifiedBy.Email != booking.CreatedBy.Email)
                        {
                            if (booking.ModifiedBy.Email != "reservation@atravelmate.com")
                            {
                                mail.To.Add(booking.ModifiedBy.Email);
                            }
                        }
                    }
                }
                mail.CC.Add("reservation@atravelmate.com");
                mail.Subject = "Thông báo xác nhận booking";

                mail.Body = content;

                mail.IsBodyHtml = true;

                mail.BodyEncoding = Encoding.UTF8;
                SmtpClient smtp = new SmtpClient();

                smtp.Host = ConfigurationManager.AppSettings["HostMail"];

                smtp.Port = 25;

                smtp.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["EmailId"], ConfigurationManager.AppSettings["EmailPassword"]);
                smtp.EnableSsl = false;

                smtp.Send(mail);
                //EmailService.SendMessage(message);
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
                return;
            }
        }
    }
}