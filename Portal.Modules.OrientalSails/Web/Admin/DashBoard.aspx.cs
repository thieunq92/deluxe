using Aspose.Words;
using Aspose.Words.Tables;
using CMS.Core.Domain;
using Portal.Modules.OrientalSails.BusinessLogic;
using Portal.Modules.OrientalSails.BusinessLogic.Share;
using Portal.Modules.OrientalSails.DataTransferObject;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Enums;
using Portal.Modules.OrientalSails.Web.Admin.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class DashBoard : Page
    {
        private DashBoardBLL dashBoardBLL;
        private PermissionBLL permissionBLL;
        private UserBLL userBLL;
        private User currentUser;
        private SailsModule module;
        private List<SailsTrip> trips = new List<SailsTrip>();
        private int totalPax;
        private List<Booking> codeBookings = new List<Booking>();
        private List<BookingDTO> bookingDTOs = new List<BookingDTO>();
        private List<Organization> organizations = new List<Organization>();
        private OrganizationBLL organizationBLL;
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
        public DashBoardBLL DashBoardBLL
        {
            get
            {
                if (dashBoardBLL == null) { dashBoardBLL = new DashBoardBLL(); }
                return dashBoardBLL;
            }
        }

        public void Initialize()
        {
            module = SailsModule.GetInstance();
            //Khởi tạo organizationBLL
            this.organizationBLL = new OrganizationBLL();
            dashBoardBLL = new DashBoardBLL();
            this.organizations = organizationBLL.OrganizationGetAllByUser(CurrentUser);
            //Khởi tạo danh sách Trip theo theo danh sách Origanization
            foreach (var organization in this.organizations)
            {
                this.trips.AddRange(this.module.TripGetByOrganization(organization).Cast<SailsTrip>());
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {

            var todayBookings = DashBoardBLL.BookingGetAllTodayBookings(CurrentUser);
            rptTodayBookings.DataSource = todayBookings;
            rptTodayBookings.DataBind();
            ddlMonthSearching.Items.AddRange(Enumerable.Range(1, 12).Select(x => new ListItem() { Text = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(x), Value = x.ToString() }).ToArray());
            ddlYearSearching.Items.AddRange(Enumerable.Range(2008, 50).Select(x => new ListItem() { Text = x.ToString(), Value = x.ToString() }).ToArray());
            ddlMonthSearching.SelectedValue = DateTime.Today.Month.ToString();
            ddlYearSearching.SelectedValue = DateTime.Today.Year.ToString();
            LoadYourMonthArchivement();
            LoadNewBookings();
            LoadChangedState();
            var top10Agencies = DashBoardBLL.AgencyGetTop10(CurrentUser);
            rptTop10Partner.DataSource = top10Agencies;
            rptTop10Partner.DataBind();
            LoadRecentMeetings();
            var agenciesNotVisited = DashBoardBLL.AgencyGetAllAgenciesNotVisitedInLast2Month(CurrentUser);
            rptAgencyNotVisited.DataSource = agenciesNotVisited;
            rptAgencyNotVisited.DataBind();
            var agenciesSendNoBookings = DashBoardBLL.AgencyGetAllAgenciesSendNoBookingsLast3Month(CurrentUser);
            rptAgenciesSendNoBookings.DataSource = agenciesSendNoBookings;
            rptAgenciesSendNoBookings.DataBind();
            ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(btnExport);
            var tripCodes = new List<EventCode>();
            foreach (var trip in this.trips)
            {
                tripCodes.AddRange(this.dashBoardBLL.ExpenseServiceGetAllTodayByTrip(trip)
                    .GroupBy(es => new
                    {
                        es.Expense,
                        es.Group
                    }).Select(ges => new EventCode(this.module)
                    {
                        SailExpense = ges.Key.Expense,
                        Group = ges.Key.Group
                    }));
            }
            this.codeBookings = (List<Booking>)this.dashBoardBLL.BookingGetAllByEventCodes(tripCodes);
            var bookings = tripCodes.SelectMany(tc => tc.Bookings.Cast<Booking>()).Cast<Booking>().ToList();
            this.bookingDTOs = (List<BookingDTO>)this.dashBoardBLL.CustomerGetCountByBookings(bookings);
            rptTodayRunningGroups.DataSource = tripCodes.OrderBy(ges => ges.SailExpense.Trip.TripCode).ThenBy(ges => ges.SailExpense.Date).ThenBy(ges => ges.Group);
            rptTodayRunningGroups.DataBind();
            ddlForRole.DataSource = module.RoleGetAll().Cast<Role>().ToList().Where(
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

        protected void Page_Load(object sender, EventArgs e)
        {
            Redirect();
            Initialize();
        }
        public void Redirect()
        {
            var canAccessDashBoardManagerPage = this.PermissionBLL.UserCheckPermission(CurrentUser, PermissionEnum.DASHBOARDMANAGER_ACCESS);
            var canAccessDashBoardPage = this.PermissionBLL.UserCheckPermission(CurrentUser, PermissionEnum.DASHBOARD_ACCESS);
            var canAccessDashBoardOperationPage = this.PermissionBLL.UserCheckPermission(CurrentUser, PermissionEnum.AllowAccessDashBoardOperationPage);
            if (!canAccessDashBoardPage)
            {
                if (canAccessDashBoardManagerPage)
                {
                    Response.Redirect("DashBoardManager.aspx");
                }
                else if (canAccessDashBoardOperationPage)
                {
                    Response.Redirect("DashBoardOperation.aspx");
                }
                else
                {
                    Response.Redirect("AccessDenied.aspx");
                }
            }
        }
        protected void Page_Unload(object sender, EventArgs e)
        {
            if (userBLL != null)
            {
                userBLL.Dispose();
                userBLL = null;
            }
            if (dashBoardBLL != null)
            {
                dashBoardBLL.Dispose();
                dashBoardBLL = null;
            }
            if (permissionBLL != null)
            {
                permissionBLL.Dispose();
                permissionBLL = null;
            }
        }
        public void LoadNewBookings()
        {
            var newBookings = DashBoardBLL.BookingGetAllNewBookings(CurrentUser);
            rptNewBookings.DataSource = newBookings;
            rptNewBookings.DataBind();
        }
        public void LoadChangedState()
        {
            var confirmedAndPendingBookingHistories = DashBoardBLL.BookingHistoryGetAllConfirmedAndPending(CurrentUser);
            var changedStateBookings = new List<Booking>();
            var confirmedAndPendingBookingHistoriesBookingGroups = confirmedAndPendingBookingHistories.GroupBy(bh => bh.Booking);
            foreach (var confirmedAndPendingBookingHistoriesBookingGroup in confirmedAndPendingBookingHistoriesBookingGroups)
            {
                var booking = confirmedAndPendingBookingHistoriesBookingGroup.Key;
                var isChangedBooking = false;
                var todayBookingHistoriesBookingGroup = confirmedAndPendingBookingHistoriesBookingGroup.Where(bh => bh.Date >= DateTime.Today && bh.Date <= DateTime.Today.Add(new TimeSpan(23, 59, 59)));
                foreach (var todayBookingHistory in todayBookingHistoriesBookingGroup)
                {
                    var previousBookingHistories = confirmedAndPendingBookingHistoriesBookingGroup.Where(bh => bh.Date < todayBookingHistory.Date);
                    foreach (var previousBookingHistory in previousBookingHistories)
                    {
                        if (previousBookingHistory.Status != todayBookingHistory.Status)
                        {
                            isChangedBooking = true;
                        }
                    }
                }
                if (isChangedBooking) changedStateBookings.Add(booking);
            }
            rptChangedState.DataSource = changedStateBookings;
            rptChangedState.DataBind();
        }
        public void LoadYourMonthArchivement()
        {
            var month = DateTime.Today.Month;
            var year = DateTime.Today.Year;
            try
            {
                month = Int32.Parse(ddlMonthSearching.SelectedValue);
            }
            catch { }
            try
            {
                year = Int32.Parse(ddlYearSearching.SelectedValue);
            }
            catch { }
            lblNumberOfPax.Text = GetNumberOfPaxInMonth(month, year).ToString();
            lblNumberOfBookings.Text = GetNumberOfBookingsInMonth(month, year).ToString();
            lblTotalRevenue.Text = GetTotalOfBookingsInMonth(month, year).ToString();
            lblAgenciesVisited.Text = GetNumberOfAgenciesVisited(month, year).ToString();
            lblMeetingReports.Text = GetNumberOfMeetingsInMonth(month, year).ToString();
        }
        public int GetNumberOfPaxInMonth(int month, int year)
        {
            return DashBoardBLL.CustomerGetNumberOfCustomersInMonth(month, year, CurrentUser);
        }
        public int GetNumberOfBookingsInMonth(int month, int year)
        {
            return DashBoardBLL.BookingGetNumberOfBookingsInMonth(month, year, CurrentUser);
        }
        public string GetTotalOfBookingsInMonth(int month, int year)
        {
            var totalRevenue = DashBoardBLL.BookingGetTotalRevenueInMonth(month, year, CurrentUser);
            return NumberUtil.FormatMoney(totalRevenue);
        }
        public IEnumerable<Activity> ActivityGetAllActivityInMonth(int month, int year)
        {
            return DashBoardBLL.ActivityGetAllActivityInMonth(month, year, CurrentUser);
        }
        public int GetNumberOfMeetingsInMonth(int month, int year)
        {
            return ActivityGetAllActivityInMonth(month, year).Count();
        }
        public int GetNumberOfAgenciesVisited(int month, int year)
        {
            return ActivityGetAllActivityInMonth(month, year).Select(x => x.Params).Distinct().Count();
        }
        protected void ddlMonthSearching_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadYourMonthArchivement();
        }
        protected void ddlYearSearching_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadYourMonthArchivement();
        }
        protected void rptTodayRunningGroups_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var ltrTripCode = e.Item.FindControl("ltrTripCode") as Literal;
                var ltrNoOfPax = e.Item.FindControl("ltrNoOfPax") as Literal;
                var ltrGuide = e.Item.FindControl("ltrGuide") as Literal;
                var ltrTransport = e.Item.FindControl("ltrTransport") as Literal;
                var code = (EventCode)e.Item.DataItem;
                ltrTripCode.Text = "<a href='EventEdit.aspx?NodeId=1&SectionId=15&expenseid=" + code.SailExpense.Id
                    + "&group=" + code.Group + "'>" + string.Format("{0}{1}-{2:00}", code.SailExpense.Trip.TripCode, code.SailExpense.Date.ToString("ddMMyy"), code.Group + "</a>");
                var codeBooking1 = this.codeBookings.Where(cb => cb.Trip.Id == code.SailExpense.Trip.Id && cb.StartDate == code.SailExpense.Date && cb.Group == code.Group);
                var noOfPax = this.bookingDTOs.Where(bd => codeBooking1.Select(b => b.Id).Contains(bd.Id)).Select(bd => bd.NumberOfPax).Sum();
                ltrNoOfPax.Text = noOfPax.ToString("#,0.##");
                var guide = code.SailExpense.Services.Cast<ExpenseService>().Where(es => es.Type.Name == "Guide" && es.Group == code.Group).FirstOrDefault();
                if (guide != null) ltrGuide.Text = guide.Name + "<br/>" + NumberUtil.FormatPhoneNumber(guide.Phone);
                var transport = code.SailExpense.Services.Cast<ExpenseService>().Where(es => es.Type.Name == "Transport" && es.Group == code.Group).FirstOrDefault();
                if (transport != null && transport.Supplier != null) ltrTransport.Text = transport.Supplier.Name + "<br/>" + transport.Name + "<br/>" + NumberUtil.FormatPhoneNumber(transport.Phone);
                totalPax += noOfPax;
            }
            if (e.Item.ItemType == ListItemType.Footer)
            {
                var ltrTotalPax = e.Item.FindControl("ltrTotalPax") as Literal;
                ltrTotalPax.Text = totalPax.ToString("#,0.##");
            }
        }
        public void LoadRecentMeetings()
        {
            var recentMeetings = GetRecentMeetings().OrderByDescending(a => a.DateMeeting).ToList();
            rptRecentMeetings.DataSource = recentMeetings;
            rptRecentMeetings.DataBind();
        }
        public IEnumerable<Activity> GetRecentMeetings()
        {
            var from = DateTime.Today.AddDays(-10);
            try
            {
                from = DateTime.ParseExact(txtFromRecentMeetingSearch.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch { }
            var to = DateTime.Today;
            try
            {
                to = DateTime.ParseExact(txtToRecentMeetingSearch.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch { }
            return DashBoardBLL.ActivityGetAllRecentMeetingsInDateRange(CurrentUser, from, to);
        }
        public Agency AgencyGetById(string agencyId)
        {
            var agencyIdAsInt = 0;
            try
            {
                agencyIdAsInt = Int32.Parse(agencyId);
            }
            catch { }
            return DashBoardBLL.AgencyGetById(agencyIdAsInt);
        }

        public AgencyContact AgencyContactGetById(int agencyContactId)
        {
            return DashBoardBLL.AgencyContactGetById(agencyContactId);
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadRecentMeetings();
        }
        public string GetTripCode(EventCode code)
        {
            return string.Format("{0}{1:ddMMyy}-{2:00}", code.SailExpense.Trip.TripCode, code.SailExpense.Date,
                code.Group);
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            var document = new Document();
            var builder = new DocumentBuilder(document);
            var activities = GetRecentMeetings().OrderByDescending(a => a.DateMeeting).ToList();
            var sales = activities.Select(x => x.User).Distinct().ToList();
            for (int i = 0; i < sales.Count(); i++)
            {
                var needInsertSalesHeader = true;
                for (int j = 0; j < activities.Count(); j++)
                {
                    var activity = activities[j] as Activity;
                    var uniqueSales = sales[i];
                    var salesInActivity = activity.User;
                    if (uniqueSales.Id != salesInActivity.Id)
                        continue;
                    var contact = DashBoardBLL.AgencyContactGetById(activity.ObjectId);
                    var contactName = contact != null ? contact.Name : "";
                    var contactPosition = contact != null ? contact.Position : "";
                    var dateMeeting = activity.DateMeeting.ToString("dd/MM/yyyy");
                    var agencyId = 0;
                    try
                    {
                        agencyId = Int32.Parse(activity.Params);
                    }
                    catch { }
                    var agency = DashBoardBLL.AgencyGetById(agencyId);
                    var agencyName = agency != null ? agency.Name : "";
                    var note = activity.Note;
                    var salesName = uniqueSales.FullName;
                    InsertTableActivityToDocument(builder, needInsertSalesHeader, dateMeeting, salesName, contactName, contactPosition, agencyName, note);
                    needInsertSalesHeader = false;
                }
            }
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/ms-word";
            Response.AppendHeader("content-disposition", "attachment; filename=" + string.Format("Meetings.doc"));
            MemoryStream m = new MemoryStream();
            document.Save(m, SaveFormat.Doc);
            Response.OutputStream.Write(m.GetBuffer(), 0, m.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();
            m.Close();
            Response.End();
        }
        private void InsertTableActivityToDocument(DocumentBuilder builder, bool needInsertSalesHeader,
           string dateMeeting, string sales, string meetingWith, string position, string belongToAgency, string note)
        {
            InsertHeader(builder, needInsertSalesHeader, sales);
            builder.StartTable();
            InsertRow(builder, dateMeeting, meetingWith, position, belongToAgency, note);
            builder.EndTable();
        }
        public void InsertHeader(DocumentBuilder builder, bool needInsertSalesHeader, string sales)
        {
            var font = builder.Font;
            font.Bold = true;
            font.Size = 16;
            var paragraph = builder.ParagraphFormat;
            paragraph.Alignment = ParagraphAlignment.Center;
            var from = DateTime.Today.AddDays(-10);
            try
            {
                from = DateTime.ParseExact(txtFromRecentMeetingSearch.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch { }
            var to = DateTime.Today;
            try
            {
                to = DateTime.ParseExact(txtToRecentMeetingSearch.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch { }
            if (needInsertSalesHeader)
            {
                builder.Writeln(string.Format("Sales {0} meetings from {1} to {2}", sales, from.ToString("dd/MM/yyyy"), to.ToString("dd/MM/yyyy")));
                builder.Writeln("");
            }
        }

        public void InsertRow(DocumentBuilder builder, string dateMeeting, string meetingWith, string position,
            string belongToAgency, string note)
        {
            var font = builder.Font;
            var paragraph = builder.ParagraphFormat;
            font.Size = 12;
            paragraph = builder.ParagraphFormat;
            paragraph.Alignment = ParagraphAlignment.Left;
            builder.CellFormat.Width = 80;
            builder.InsertCell().CellFormat.HorizontalMerge = CellMerge.None;
            if (dateMeeting != null)
            {
                builder.Writeln(dateMeeting);
            }

            builder.InsertCell();
            if (meetingWith != null)
            {
                builder.Writeln(meetingWith);
            }

            builder.InsertCell();
            if (position != null)
            {
                builder.Writeln(position);
            }

            font.Size = 10;
            builder.InsertCell().CellFormat.FitText = true;

            if (belongToAgency != null)
            {
                builder.Writeln(belongToAgency);
            }
            builder.CellFormat.Width = 200;
            builder.EndRow();

            font.Bold = false;
            font.Size = 12;

            builder.InsertCell().CellFormat.HorizontalMerge = CellMerge.First;
            if (note != null)
            {
                builder.Writeln(note);
            }
            builder.InsertCell().CellFormat.HorizontalMerge = CellMerge.Previous;
            builder.Writeln("");

            builder.InsertCell().CellFormat.HorizontalMerge = CellMerge.Previous;
            builder.Writeln("");

            builder.InsertCell().CellFormat.HorizontalMerge = CellMerge.Previous;
            builder.Writeln("");
            builder.EndRow();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            var agencyContactId = 0;
            try
            {
                agencyContactId = Int32.Parse(Request.Params["ddlContact"]);
            }
            catch { }
            var agencyContact = DashBoardBLL.AgencyContactGetById(agencyContactId);
            var dateMeeting = new DateTime();
            try
            {
                dateMeeting = DateTime.ParseExact(txtDateMeeting.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch { }
            var activityId = 0;
            try
            {
                activityId = Int32.Parse(hidActivityId.Value);
            }
            catch { }
            var activity = DashBoardBLL.ActivityGetById(activityId);
            if (activity == null || activity.Id == 0) activity = new Activity();
            activity.UpdateTime = DateTime.Now;
            activity.Time = DateTime.Now;
            activity.Params = agencyContact != null && agencyContact.Agency != null ? agencyContact.Agency.Id.ToString() : 0.ToString();
            activity.DateMeeting = dateMeeting;
            activity.Note = txtNote.Text;
            activity.ObjectType = "MEETING";
            activity.ObjectId = agencyContact != null ? agencyContact.Id : 0;
            activity.Url = "AgencyView.aspx?NodeId=1&SectionId=15&agencyid=" + agencyContact != null && agencyContact.Agency != null ? agencyContact.Agency.Id.ToString() : 0.ToString();
            activity.User = CurrentUser;
            activity.Level = ImportantLevel.Important;
            DashBoardBLL.ActivitySaveOrUpdate(activity);
            Response.Redirect(Request.RawUrl);
        }

        protected void btnAddNoteSave_Click(object sender, EventArgs e)
        {
            var tripCodeNote = new TripCode_Note
            {
                TripCode = hidTripCode.Value,
                Note = txtNoteTripCode.Text,
                CreatedDate = DateTime.Now,
                CreatedUser = CurrentUser,
                ToRole =
                    dashBoardBLL.RoleGetById(int.Parse(ddlForRole.SelectedValue)).Id > 0
                        ? dashBoardBLL.RoleGetById(int.Parse(ddlForRole.SelectedValue)) : null
            };
            DashBoardBLL.TripCode_NoteSaveOrUpdate(tripCodeNote);
        }

        protected void rptNotes_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            var ltrNote = e.Item.FindControl("ltrNote") as Literal;
            var tripCodeNote = (TripCode_Note)e.Item.DataItem;
            var color = tripCodeNote.ToRole.Name == "Operator" ? "color:red" : "color:blue";
            if (ltrNote != null)
                ltrNote.Text =
                    tripCodeNote.CreatedUser.FullName + @" - "
                    + @"<span style='" + color + @"'>"
                    + @"to" + @" " + tripCodeNote.ToRole.Name
                    + @"</span>" + @":" + @" "
                    + tripCodeNote.Note;

        }

        protected void rptTodayRunningGroups_OnItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "View")
            {
                var tripCode = e.CommandArgument.ToString();
                var notes = dashBoardBLL.TripCode_NoteGetAllByTripCode(e.CommandArgument.ToString());
                rptNotes.DataSource = notes;
                rptNotes.DataBind();
                var tripCode_User = dashBoardBLL.TripCode_UserGetByTripCodeAndUser(tripCode, currentUser);
                if (tripCode_User == null || tripCode_User.Id <= 0)
                {
                    tripCode_User = new TripCode_User()
                    {
                        TripCode = tripCode,
                        User = CurrentUser,
                        LastCheck = DateTime.Now,
                    };
                }
                else
                {
                    tripCode_User.LastCheck = DateTime.Now;
                }
                DashBoardBLL.TripCode_UserSaveOrUpdate(tripCode_User);
            }
        }

        protected int NoteGetCountOfNewNotes(EventCode eventCode)
        {
            var tripCode = GetTripCode(eventCode);
            return DashBoardBLL.NoteGetCountOfNewNotes(tripCode, currentUser);
        }
    }
}