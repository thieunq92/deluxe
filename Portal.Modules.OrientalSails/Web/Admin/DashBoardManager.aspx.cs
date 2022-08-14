using CMS.Core.Domain;
using log4net;
using NHibernate;
using Portal.Modules.OrientalSails.BusinessLogic;
using Portal.Modules.OrientalSails.BusinessLogic.Share;
using Portal.Modules.OrientalSails.DataTransferObject;
using Portal.Modules.OrientalSails.DataTransferObject.DashBoardManager;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Enums;
using Portal.Modules.OrientalSails.Web.Admin.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NHibernate.Criterion;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class DashBoardManager : Page
    {
        private UserBLL userBLL;
        private User currentUser;
        private PermissionBLL permissionBLL;
        private DashBoardManagerBLL dashBoardManagerBLL;
        private SailsModule module;
        private List<User> saleses;
        private List<User> monthSummarySaleses;
        private List<Booking> bookingsLastOneYear = new List<Booking>();
        private int monthSearching;
        private int yearSearching;
        private int totalPax;    
        private List<Organization> organizations = new List<Organization>();
        private List<Organization> monthSummaryOrganizations = new List<Organization>();
        private OrganizationBLL organizationBLL;
        private List<SailsTrip> trips = new List<SailsTrip>();
        private List<BookingDTO> bookingDTOs = new List<BookingDTO>();
        private List<UserDTO> userDTOs = new List<UserDTO>();
        private List<UserDTO> userDTOsLastYear = new List<UserDTO>();
        private List<Booking> codeBookings = new List<Booking>();
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

        public List<User> Saleses
        {
            get
            {
                var users = UserBLL.UserGetByOrganization(this.organizations);
                var sales = UserBLL.UserGetByRole((int)Roles.Sales);
                var salesHaveSameOrganizations = sales.Where(s => users.Select(u => u.Id).Contains(s.Id)).ToList();
                return salesHaveSameOrganizations;
            }
        }

        public int MonthSearching
        {
            get
            {
                var month = DateTime.Today.Month;
                try
                {
                    month = Int32.Parse(ddlMonthSearching.SelectedValue);
                }
                catch (Exception) { }
                return month;
            }
        }
        public int YearSearching
        {
            get
            {
               var year = DateTime.Today.Year;
                try
                {
                    year = Int32.Parse(ddlYearSearching.SelectedValue);
                }
                catch (Exception) { }
                return year;
            }
        }
        public int MonthTopPartner
        {
            get
            {
                var month = DateTime.Today.Month;
                try
                {
                    month = Int32.Parse(ddlMonthTopPartner.SelectedValue);
                }
                catch (Exception) { }
                return month;
            }
        }
        public int YearTopPartner
        {
            get
            {
                var year = DateTime.Today.Year;
                try
                {
                    year = Int32.Parse(ddlYearTopPartner.SelectedValue);
                }
                catch (Exception) { }
                return year;
            }
        }
        public void Initialize()
        {
            this.monthSearching = MonthSearching;
            this.yearSearching = YearSearching;
            //Khởi tạo organizationBLL
            this.organizationBLL = new OrganizationBLL();
            //Khởi tạo DashBoardManagerBLL
            this.dashBoardManagerBLL = new DashBoardManagerBLL();
            //Khởi tạo module
            this.module = SailsModule.GetInstance();
            //Khởi tạo danh sách Origanization theo User
            this.organizations = organizationBLL.OrganizationGetAllByUser(CurrentUser);
            //Khởi tạo danh sách Trip theo theo danh sách Origanization
            foreach (var organization in this.organizations)
            {
                this.trips.AddRange(this.module.TripGetByOrganization(organization).Cast<SailsTrip>());
            }
            this.saleses = this.Saleses;
            this.userDTOs = (List<UserDTO>)this.dashBoardManagerBLL.GetMonthSummary(monthSearching, yearSearching, this.saleses);
            this.userDTOsLastYear = (List<UserDTO>)this.dashBoardManagerBLL.GetMonthSummary(monthSearching, yearSearching - 1, this.saleses);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Redirect();
            Initialize();
            if (!IsPostBack)
            {
                ddlMonthSearching.Items.AddRange(Enumerable.Range(1, 12).Select(x => new ListItem() { Text = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(x), Value = x.ToString() }).ToArray());
                ddlYearSearching.Items.AddRange(Enumerable.Range(2008, (DateTime.Today.Year - 2008) + 3).Select(x => new ListItem() { Text = x.ToString(), Value = x.ToString() }).ToArray());
                ddlMonthSearching.SelectedValue = DateTime.Today.Month.ToString();
                ddlYearSearching.SelectedValue = DateTime.Today.Year.ToString();
                ddlSales.DataSource = this.saleses;
                ddlSales.DataTextField = "UserName";
                ddlSales.DataValueField = "Id";
                ddlSales.DataBind();
                ddlMonthTopPartner.Items.AddRange(Enumerable.Range(1, 12).Select(x => new ListItem() { Text = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(x), Value = x.ToString() }).ToArray());
                ddlMonthTopPartner.SelectedValue = DateTime.Today.Month.ToString();
                ddlYearTopPartner.Items.AddRange(Enumerable.Range(2008, (DateTime.Today.Year - 2008) + 3).Select(x => new ListItem() { Text = x.ToString(), Value = x.ToString() }).ToArray());
                ddlYearTopPartner.SelectedValue = DateTime.Today.Year.ToString();
                ddlCampaign.Items.Add(new ListItem(){Value = DateTime.Now.ToString("dd/MM/yyyy"), Text="-- Campaign --"});
                ddlCampaign.DataSource = this.dashBoardManagerBLL.CampaignGetAll();
                ddlCampaign.DataTextField = "Name";
                ddlCampaign.DataValueField = "DateCampaignAsString";
                ddlCampaign.DataBind();
                if (this.organizations.Count > 0) 
                    ddlRegion.Items.Add(new ListItem() { Text = "-- Region --", Value = "0" });
                ddlRegion.DataSource = this.organizations;
                ddlRegion.DataTextField = "Name";
                ddlRegion.DataValueField = "Id";
                ddlRegion.DataBind();
            }
            LoadMonthSummary();

            var tripCodes = new List<EventCode>();
            foreach (var trip in this.trips)
            {
                tripCodes.AddRange(this.dashBoardManagerBLL.ExpenseServiceGetAllTodayByTrip(trip)
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
            this.codeBookings = (List<Booking>)this.dashBoardManagerBLL.BookingGetAllByEventCodes(tripCodes);
            var bookings = tripCodes.SelectMany(tc => tc.Bookings.Cast<Booking>()).Cast<Booking>().ToList();
            this.bookingDTOs = (List<BookingDTO>)this.dashBoardManagerBLL.CustomerGetCountByBookings(bookings);
            rptTodayRunningGroups.DataSource = tripCodes.OrderBy(ges => ges.SailExpense.Trip.TripCode).ThenBy(ges => ges.SailExpense.Date).ThenBy(ges => ges.Group);
            rptTodayRunningGroups.DataBind();

            LoadBookingReport();

            var top10Agencies = this.dashBoardManagerBLL.AgencyGetTop10(organizations, YearTopPartner, MonthTopPartner);
            rptTop10Partner.DataSource = top10Agencies;
            rptTop10Partner.DataBind();
            rptTrips.DataSource = trips;
            rptTrips.DataBind();

            var from = DateTime.Today;
            try
            {
                from = DateTime.ParseExact(txtAvaibilityDateSearching.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch { }
            from = from.AddDays(1);
            var to = from.AddDays(int.Parse(ddlView.SelectedValue));
            var dateRange = new List<DateTime>();
            var current = from;
            while (current < to)
            {
                dateRange.Add(current);
                current = current.AddDays(1);
            }
            this.bookingDTOs = (List<BookingDTO>)this.dashBoardManagerBLL.CustomerGetCountByTripAndStartDate(trips, dateRange);
            rptDates.DataSource = dateRange;
            rptDates.DataBind();
        }
        public void Redirect()
        {
            var canAccessDashBoardManagerPage = this.PermissionBLL.UserCheckPermission(CurrentUser, PermissionEnum.DASHBOARDMANAGER_ACCESS);
            var canAccessDashBoardPage = this.PermissionBLL.UserCheckPermission(CurrentUser, PermissionEnum.DASHBOARD_ACCESS);
            var canAccessDashBoardOperationPage = this.PermissionBLL.UserCheckPermission(CurrentUser, PermissionEnum.AllowAccessDashBoardOperationPage);
            if (!canAccessDashBoardManagerPage)
            { 
                if (canAccessDashBoardPage)
                {
                    Response.Redirect("DashBoard.aspx");
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
        private void LoadMonthSummary()
        {
            var organizationId = 0;
            try
            {
               organizationId = Int32.Parse(ddlRegion.SelectedValue);
            }
            catch { }
            if (organizationId == 0)
                this.monthSummaryOrganizations = this.organizations;
            else
                this.monthSummaryOrganizations = this.organizations.Where(o => o.Id == organizationId).ToList();
            var users = UserBLL.UserGetByOrganization(this.monthSummaryOrganizations);
            var sales = UserBLL.UserGetByRole((int)Roles.Sales);
            var salesHaveSameOrganizations = sales.Where(s => users.Select(u => u.Id).Contains(s.Id)).ToList();
            this.monthSummarySaleses = salesHaveSameOrganizations;
            //Sales header
            rptSales.DataSource = this.monthSummarySaleses;
            rptSales.DataBind();
            //Bind month summary
            rptSalesNoOfPax.DataSource = this.monthSummarySaleses;
            rptSalesNoOfPax.DataBind();
            rptSalesNoOfBookings.DataSource = this.monthSummarySaleses;
            rptSalesNoOfBookings.DataBind();
            rptSalesRevenueInUSD.DataSource = this.monthSummarySaleses;
            rptSalesRevenueInUSD.DataBind();
            rptSalesMeetingReports.DataSource = this.monthSummarySaleses;
            rptSalesMeetingReports.DataBind();
            #region Total column
            //Total pax của tất cả sales
            ltrTotalPax.Text = this.userDTOs.Where(u=>this.monthSummarySaleses.Select(s => s.Id).Contains(u.SalesId)).Select(u => u.NumberOfPax).Sum().ToString();
            //Total booking của tất cả sales
            ltrTotalBooking.Text = this.userDTOs.Where(u=>this.monthSummarySaleses.Select(s => s.Id).Contains(u.SalesId)).Select(u => u.NumberOfBooking).Sum().ToString();
            ltrTotalRevenue.Text = NumberUtil.FormatMoney(Math.Ceiling(this.userDTOs.Where(u=>this.monthSummarySaleses.Select(s => s.Id).Contains(u.SalesId)).Select(u => u.RevenueInUSD).Sum()));
            ltrTotalReport.Text = this.userDTOs.Where(u=>this.monthSummarySaleses.Select(s => s.Id).Contains(u.SalesId)).Select(u => u.NumberOfReport).Sum().ToString();
            #endregion
            #region Last one year column
            ltrPaxLastOneYear.Text = this.userDTOsLastYear.Where(u => this.monthSummarySaleses.Select(s => s.Id).Contains(u.SalesId)).Select(u => u.NumberOfPax).Sum().ToString("#,0.##");
            ltrBookingLastOneYear.Text = this.userDTOsLastYear.Where(u => this.monthSummarySaleses.Select(s => s.Id).Contains(u.SalesId)).Select(u => u.NumberOfBooking).Sum().ToString("#,0.##");
            ltrRevenueLastOneYear.Text = NumberUtil.FormatMoney(this.userDTOsLastYear.Where(u => this.monthSummarySaleses.Select(s => s.Id).Contains(u.SalesId)).Select(u => u.RevenueInUSD).Sum());
            ltrReportLastOneYear.Text = this.userDTOsLastYear.Where(u => this.monthSummarySaleses.Select(s => s.Id).Contains(u.SalesId)).Select(u => u.NumberOfReport).Sum().ToString("#,0.##");
            #endregion
        }



        protected void Page_Unload(object sender, EventArgs e)
        {
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
            if (dashBoardManagerBLL != null)
            {
                dashBoardManagerBLL.Dispose();
                dashBoardManagerBLL = null;
            }
            if (organizationBLL != null)
            {
                organizationBLL.Dispose();
                organizationBLL = null;
            }
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
                if (transport != null && transport.Supplier != null) ltrTransport.Text = transport.Supplier.Name + @"<br/>" + (transport.Driver != null ? (transport.Driver.Name + @"<br/>") : "") + NumberUtil.FormatPhoneNumber(transport.Phone);
                totalPax += noOfPax;
            }
            if (e.Item.ItemType == ListItemType.Footer)
            {
                var ltrTotalPax = e.Item.FindControl("ltrTotalPax") as Literal;
                ltrTotalPax.Text = totalPax.ToString("#,0.##");
            }
        }
        public void LoadBookingReport()
        {
            var date = DateTime.Today;
            var salesIdSelected = 0;
            try
            {
                salesIdSelected = int.Parse(ddlSales.SelectedValue);
            }
            catch { }
            var sales = this.module.UserGetById(salesIdSelected);
            var newBookings = GetNewBookings(date, organizations, sales);
            var cancelledBookings = GetCancelledBookings(date, organizations, sales);
            var bookingsReported = new List<Booking>();
            if (newBookings != null) bookingsReported.AddRange(newBookings);
            if (cancelledBookings != null) bookingsReported.AddRange(cancelledBookings);
            rptNewBookings.DataSource = bookingsReported;
            rptNewBookings.DataBind();
        }
        public IEnumerable<Booking> GetNewBookings(DateTime date, List<Organization> organizations, User sales)
        {
            return this.dashBoardManagerBLL.BookingGetAllNewBookings(date, organizations, sales);
        }
        public IEnumerable<Booking> GetCancelledBookings(DateTime date, List<Organization> organizations, User sales)
        {
            return this.dashBoardManagerBLL.BookingGetAllCancelledBookingOnDate(date, organizations, sales);
        }
        public double GetTotal(Booking booking)
        {
            var total = 0.0;
            if (booking.IsVND)
                total = booking.TotalVND;
            else
                total = booking.Total * 23000;
            return total;
        }
        public string GetTotalAsString(Booking booking)
        {
            return NumberUtil.FormatMoney(GetTotal(booking));
        }

        protected void rptDates_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var rptDateTrips = e.Item.FindControl("rptDateTrips") as Repeater;
            rptDateTrips.DataSource = trips;
            rptDateTrips.DataBind();
        }

        protected void rptDateTrips_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var date = (DateTime)((RepeaterItem)e.Item.Parent.Parent).DataItem;
            var trip = (SailsTrip)e.Item.DataItem;
            var ltrNumberOfPax = (Literal)e.Item.FindControl("ltrNumberOfPax");
            var bookingDTO = bookingDTOs.Where(b => b.StartDate == date && b.TripId == trip.Id).FirstOrDefault();
            if (bookingDTO == null) ltrNumberOfPax.Text = "0";
            else ltrNumberOfPax.Text = bookingDTO.NumberOfPax.ToString();
        }

        protected void rptSalesRevenueInUSD_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var sales = (User)e.Item.DataItem;
            var ltrRevenue = e.Item.FindControl("ltrRevenue") as Literal;
            var userDTO = userDTOs.Where(u => u.SalesId == sales.Id).FirstOrDefault();
            if (userDTO == null) ltrRevenue.Text = "0";
            else
            {
                userDTO.RevenueInUSD = Math.Ceiling(userDTO.RevenueInUSD);
                ltrRevenue.Text = NumberUtil.FormatMoney(userDTO.RevenueInUSD);
            }
        }

        protected void rptSalesNoOfBookings_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var sales = (User)e.Item.DataItem;
            var ltrNumberOfBooking = e.Item.FindControl("ltrNumberOfBooking") as Literal;
            var userDTO = userDTOs.Where(u => u.SalesId == sales.Id).FirstOrDefault();
            if (userDTO == null) ltrNumberOfBooking.Text = "0";
            else ltrNumberOfBooking.Text = userDTO.NumberOfBooking.ToString("#,0.##");
        }

        protected void rptSalesNoOfPax_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var sales = (User)e.Item.DataItem;
            var ltrNumberOfPax = e.Item.FindControl("ltrNumberOfPax") as Literal;
            var userDTO = userDTOs.Where(u => u.SalesId == sales.Id).FirstOrDefault();
            if (userDTO == null) ltrNumberOfPax.Text = "0";
            else ltrNumberOfPax.Text = userDTO.NumberOfPax.ToString("#,0.##");
        }

        protected void rptSalesMeetingReports_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var sales = (User)e.Item.DataItem;
            var ltrNumberOfReport = e.Item.FindControl("ltrNumberOfReport") as Literal;
            var userDTO = userDTOs.Where(u => u.SalesId == sales.Id).FirstOrDefault();
            if (userDTO == null) ltrNumberOfReport.Text = "0";
            else ltrNumberOfReport.Text = userDTO.NumberOfReport.ToString("#,0.##");

        }
    }
}