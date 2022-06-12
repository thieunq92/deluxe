using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.Core.Domain;
using Portal.Modules.OrientalSails.BusinessLogic;
using Portal.Modules.OrientalSails.BusinessLogic.Share;
using Portal.Modules.OrientalSails.DataTransferObject;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Enums;
using Portal.Modules.OrientalSails.Web.Admin.Utility;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class DashBoardOperation : Page
    {
        private readonly List<SailsTrip> _trips = new List<SailsTrip>();
        private List<BookingDTO> _bookingDtos = new List<BookingDTO>();
        private List<Booking> _codeBookings = new List<Booking>();
        private DashBoardOperationBLL _dashBoardOperationBll = new DashBoardOperationBLL();
        private DateTime _dateSearching;
        private SailsModule _module;
        private OrganizationBLL _organizationBll;
        private List<Organization> _organizations = new List<Organization>();
        private PermissionBLL _permissionBll;
        private double _totalBoatExpense;
        private int _totalBooking;
        private double _totalGuideExpense;
        private double _totalOfTotalExpense;
        private double _totalOtherExpense;
        private int _totalPax;
        private double _totalTransportExpense;
        private UserBLL _userBll;

        public UserBLL UserBll
        {
            get { return _userBll ?? (_userBll = new UserBLL()); }
        }

        public User CurrentUser
        {
            get { return UserBll.UserGetCurrent(); }
        }

        public PermissionBLL PermissionBll
        {
            get { return _permissionBll ?? (_permissionBll = new PermissionBLL()); }
        }

        protected void btnAddNoteSave_Click(object sender, EventArgs e)
        {
            var tripCodeNote = new TripCode_Note
            {
                TripCode = hidTripCode.Value,
                Note = txtNote.Text,
                CreatedDate = DateTime.Now,
                CreatedUser = CurrentUser,
                ToRole =
                    _dashBoardOperationBll.RoleGetById(int.Parse(ddlForRole.SelectedValue)).Id > 0
                    ? _dashBoardOperationBll.RoleGetById(int.Parse(ddlForRole.SelectedValue)) : null
            };
            _dashBoardOperationBll.TripCode_NoteSaveOrUpdate(tripCodeNote);
        }

        public string GetTripCode(EventCode code)
        {
            return string.Format("{0}{1:ddMMyy}-{2:00}", code.SailExpense.Trip.TripCode, code.SailExpense.Date,
                code.Group);
        }

        public void Initialize()
        {
            _userBll = new UserBLL();
            _organizationBll = new OrganizationBLL();
            _dashBoardOperationBll = new DashBoardOperationBLL();
            _module = SailsModule.GetInstance();
            _organizations = _organizationBll.OrganizationGetAllByUser(CurrentUser);
            _dateSearching = DateTime.Today;
            try
            {
                _dateSearching =
                    DateTime.ParseExact(txtDateSearching.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Redirect();
            Initialize();
        }


        protected void Page_PreRender(object sender, EventArgs e)
        {

            if (!IsPostBack) txtDateSearching.Text = DateTime.Today.ToString("dd/MM/yyyy");

            foreach (var organization in _organizations)
                _trips.AddRange(_module.TripGetByOrganization(organization).Cast<SailsTrip>());

            var tripCodes = new List<EventCode>();
            foreach (var trip in _trips)
                tripCodes.AddRange(_dashBoardOperationBll
                    .ExpeseServiceGetAllTodayByTripAndDate(trip, _dateSearching)
                    .GroupBy(es => new
                    {
                        es.Expense,
                        es.Group
                    }).Select(ges => new EventCode(_module)
                    {
                        SailExpense = ges.Key.Expense,
                        Group = ges.Key.Group
                    }));

            _codeBookings = (List<Booking>)_dashBoardOperationBll.BookingGetAllByEventCodes(tripCodes);
            var bookings = tripCodes.SelectMany(tc => tc.Bookings.Cast<Booking>()).ToList();
            _bookingDtos = (List<BookingDTO>)_dashBoardOperationBll.CustomerGetCountByBookings(bookings);
            rptTodayRunningGroups.DataSource = tripCodes.OrderBy(ges => ges.SailExpense.Trip.TripCode)
                .ThenBy(ges => ges.SailExpense.Date).ThenBy(ges => ges.Group).ToList();
            rptTodayRunningGroups.DataBind();
            ddlForRole.DataSource = _module.RoleGetAll().Cast<Role>().ToList().Where(
                r => r.Name != "Administrator"
                     && r.Name != "Editor"
                     && r.Name != "Authenticated user"
                     && r.Name != "Anonymous user"
                     && r.Name != "Guide supplier"
                     && r.Name != "Supplier"
                     && r.Name != "Sales Agency"
                     && r.Name != "Hotels"
                     && r.Name != "Guides"
                     && r.Name != "Land Transport"
                     && r.Name != "Boats");
            ddlForRole.DataTextField = "Name";
            ddlForRole.DataValueField = "Id";
            ddlForRole.DataBind();
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (_userBll != null)
            {
                _userBll.Dispose();
                _userBll = null;
            }

            if (_dashBoardOperationBll != null)
            {
                _dashBoardOperationBll.Dispose();
                _dashBoardOperationBll = null;
            }

            if (_organizationBll != null)
            {
                _organizationBll.Dispose();
                _organizationBll = null;
            }

            if (_permissionBll == null) return;
            _permissionBll.Dispose();
            _permissionBll = null;
        }

        public void Redirect()
        {
            var canAccessDashBoardManagerPage =
                PermissionBll.UserCheckPermission(CurrentUser, PermissionEnum.DASHBOARDMANAGER_ACCESS);
            var canAccessDashBoardPage =
                PermissionBll.UserCheckPermission(CurrentUser, PermissionEnum.DASHBOARD_ACCESS);
            var canAccessDashBoardOperationPage =
                PermissionBll.UserCheckPermission(CurrentUser, PermissionEnum.AllowAccessDashBoardOperationPage);
            if (canAccessDashBoardOperationPage) return;
            if (canAccessDashBoardManagerPage)
                Response.Redirect("DashBoardManager.aspx");
            else if (canAccessDashBoardPage)
                Response.Redirect("DashBoard.aspx");
            else
                Response.Redirect("AccessDenied.aspx");
        }

        protected void rptNotes_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            var ltrNote = e.Item.FindControl("ltrNote") as Literal;
            var tripCodeNote = (TripCode_Note)e.Item.DataItem;
            var color = tripCodeNote.ToRole.Name == "Operator" ? "color:red" : "color:blue";
            if (ltrNote != null) ltrNote.Text = @"<p style='word-break: break-all; text-align:left'>" + tripCodeNote.CreatedUser.FullName + @" - "
                                                + @"<span style='" + color + @"'>"
                                                + @"to" + @" " + tripCodeNote.ToRole.Name
                                                + @"</span>" + @":" + @" "
                                                + tripCodeNote.Note + @"</p>";
        }

        protected void rptTodayRunningGroups_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var ltrTripCode = e.Item.FindControl("ltrTripCode") as Literal;
                var ltrNoOfPax = e.Item.FindControl("ltrNoOfPax") as Literal;
                var ltrGuide = e.Item.FindControl("ltrGuide") as Literal;
                var ltrTransport = e.Item.FindControl("ltrTransport") as Literal;
                var ltrNoOfBooking = e.Item.FindControl("ltrNoOfBooking") as Literal;
                var ltrGuideExpense = e.Item.FindControl("ltrGuideExpense") as Literal;
                var ltrTransportExpense = e.Item.FindControl("ltrTransportExpense") as Literal;
                var ltrBoatExpense = e.Item.FindControl("ltrBoatExpense") as Literal;
                var ltrOtherExpense = e.Item.FindControl("ltrOtherExpense") as Literal;
                var ltrTotalExpense = e.Item.FindControl("ltrTotalExpense") as Literal;
                var ltrSpecialRequest = e.Item.FindControl("ltrSpecialRequest") as Literal;
                var rptNotes = e.Item.FindControl("rptNotes") as Repeater;
                var code = (EventCode)e.Item.DataItem;
                ltrTripCode.Text = @"<a href='EventEdit.aspx?NodeId=1&SectionId=15&expenseid=" + code.SailExpense.Id
                                                                                               + @"&group=" +
                                                                                               code.Group + @"'>" +
                                                                                               string.Format(
                                                                                                   "{0}{1:ddMMyy}-{2:00}",
                                                                                                   code.SailExpense.Trip
                                                                                                       .TripCode,
                                                                                                   code.SailExpense
                                                                                                       .Date,
                                                                                                   code.Group) +
                                                                                               @"</a>";
                var codeBooking1 = _codeBookings.Where(cb =>
                    cb.Trip.Id == code.SailExpense.Trip.Id && cb.StartDate == code.SailExpense.Date &&
                    cb.Group == code.Group);
                var noOfPax = _bookingDtos.Where(bd => codeBooking1.Select(b => b.Id).Contains(bd.Id))
                    .Select(bd => bd.NumberOfPax).Sum();
                ltrNoOfPax.Text = noOfPax.ToString("#,0.##");
                var noOfBooking = _bookingDtos.Count(bd => codeBooking1.Select(b => b.Id).Contains(bd.Id));
                ltrNoOfBooking.Text = noOfBooking.ToString("#,0.##");
                var guide = code.SailExpense.Services.Cast<ExpenseService>()
                    .FirstOrDefault(es => es.Type.Name == "Guide" && es.Group == code.Group);
                if (guide != null) ltrGuide.Text = guide.Name + @"<br/>" + NumberUtil.FormatPhoneNumber(guide.Phone);
                var transport = code.SailExpense.Services.Cast<ExpenseService>()
                    .FirstOrDefault(es => es.Type.Name == "Transport" && es.Group == code.Group);
                if (transport != null && transport.Supplier != null)
                    ltrTransport.Text = transport.Supplier.Name + @"<br/>" + (transport.Driver != null ? (transport.Driver.Name + @"<br/>") : "")
                                        + NumberUtil.FormatPhoneNumber(transport.Phone);
                var boat = code.SailExpense.Services.Cast<ExpenseService>()
                    .FirstOrDefault(es => es.Type.Name == "Boat" && es.Group == code.Group);
                var guideExpense = guide != null ? guide.Cost : 0;
                ltrGuideExpense.Text = guideExpense.ToString("#,0.##");
                var transportExpense = transport != null ? transport.Cost : 0;
                ltrTransportExpense.Text = transportExpense.ToString("#,0.##");
                var boatExpense = boat != null ? boat.Cost : 0;
                ltrBoatExpense.Text = boatExpense.ToString("#,0.##");
                var otherExpense = code.SailExpense.Services.Cast<ExpenseService>().Where(es =>
                    es.Type.Name != "Guide" && es.Type.Name != "Transport" && es.Type.Name != "Boat" &&
                    es.Group == code.Group).Sum(es => es.Cost);
                ltrOtherExpense.Text = otherExpense.ToString("#,0.##");
                var totalExpense = code.SailExpense.Services.Cast<ExpenseService>().Where(es => es.Group == code.Group)
                    .Sum(es => es.Cost);
                ltrTotalExpense.Text = totalExpense.ToString("#,0.##");
                _totalPax += noOfPax;
                _totalBooking += noOfBooking;
                _totalGuideExpense += guideExpense;
                _totalTransportExpense += transportExpense;
                _totalBoatExpense += boatExpense;
                _totalOtherExpense += otherExpense;
                _totalOfTotalExpense += totalExpense;
                ltrSpecialRequest.Text = string.Join("<br/>",
                    _bookingDtos.Where(bd => codeBooking1.Select(b => b.Id).Contains(bd.Id))
                        .Select(b => string.Format("ATM{0:00000}", b.Id) + " - " + b.SpecialRequest).ToArray());
                var notes = _dashBoardOperationBll.TripCode_NoteGetAllByTripCode(string.Format("{0}{1:ddMMyy}-{2:00}",
                    code.SailExpense.Trip.TripCode, code.SailExpense.Date, code.Group));
                rptNotes.DataSource = notes;
                rptNotes.DataBind();
            }

            if (e.Item.ItemType != ListItemType.Footer) return;
            var ltrTotalPax = e.Item.FindControl("ltrTotalPax") as Literal;
            var ltrTotalBooking = e.Item.FindControl("ltrTotalBooking") as Literal;
            var ltrTotalGuideExpense = e.Item.FindControl("ltrTotalGuideExpense") as Literal;
            var ltrTotalTransportExpense = e.Item.FindControl("ltrTotalTransportExpense") as Literal;
            var ltrTotalBoatExpense = e.Item.FindControl("ltrTotalBoatExpense") as Literal;
            var ltrTotalOtherExpense = e.Item.FindControl("ltrTotalOtherExpense") as Literal;
            var ltrTotalOfTotalExpense = e.Item.FindControl("ltrTotalOfTotalExpense") as Literal;
            ltrTotalPax.Text = _totalPax.ToString("#,0.##");
            ltrTotalBooking.Text = _totalBooking.ToString("#,0.##");
            ltrTotalGuideExpense.Text = _totalGuideExpense.ToString("#,0.##");
            ltrTotalTransportExpense.Text = _totalTransportExpense.ToString("#,0.##");
            ltrTotalBoatExpense.Text = _totalBoatExpense.ToString("#,0.##");
            ltrTotalOtherExpense.Text = _totalOtherExpense.ToString("#,0.##");
            ltrTotalOfTotalExpense.Text = _totalOfTotalExpense.ToString("#,0.##");
        }
    }
}