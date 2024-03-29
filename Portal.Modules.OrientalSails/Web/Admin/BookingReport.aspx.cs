using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CMS.Core.Domain;
using CMS.ServerControls;
using GemBox.Spreadsheet;
using log4net;
using NHibernate.Criterion;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.ReportEngine;
using Portal.Modules.OrientalSails.Web.UI;
using Portal.Modules.OrientalSails.Web.Util;
using Portal.Modules.OrientalSails.BusinessLogic;
using NHibernate;
using NHibernate.Linq;
using System.Linq;
using System.Text.RegularExpressions;
using Portal.Modules.OrientalSails.BusinessLogic.Share;
using Portal.Modules.OrientalSails.Enums;
using System.Linq;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class BookingReport : SailsAdminBase
    {
        private BookingReportBLL bookingReportBLL;
        private int _adult;
        private int _baby;
        private int _child;
        private SailsTrip _trip;
        private IList _cruises;
        private double _customerCost;
        private IList _dailyCost;
        private int _doubleCabin;
        private IList _guides;
        private double _runningCost;
        private int _numberOfGroups;
        private int _currentGroup;
        private IList _services;
        private double _total;
        private double _totalCost;
        private int _transferAdult;
        private int _transferChild;
        private int _twin;
        private bool isExpenseLocked = false;
        private UserBLL userBLL;
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
        public DateTime Date
        {
            get
            {
                DateTime date = DateTime.Today;
                try
                {
                    if (!String.IsNullOrEmpty(Request.QueryString["Date"]))
                    {
                        date = DateTime.FromOADate(Convert.ToDouble(Request.QueryString["Date"]));
                    }
                }
                catch { }
                return date;
            }
        }
        public String SearchCode
        {
            get
            {
                var searchCode = "";
                if (!String.IsNullOrEmpty(Request.QueryString["searchcode"]))
                {
                    searchCode = Request.QueryString["searchcode"];
                }
                return searchCode;
            }
        }
        public String Type
        {
            get
            {
                var type = "";
                if (!String.IsNullOrEmpty(Request.QueryString["type"]))
                {
                    type = Request.QueryString["type"];
                }
                return type;
            }
        }
        public int FoundBookingId
        {
            get
            {
                var foundBookingId = 0;
                if (!String.IsNullOrEmpty(Request.QueryString["foundbookingid"]))
                {
                    try
                    {
                        foundBookingId = Int32.Parse(Request.QueryString["foundbookingid"]);
                    }
                    catch { }
                }
                return foundBookingId;
            }
        }
        public IList Suppliers(int costypeid)
        {
            if (Request.QueryString["tripid"] != null)
            {
                SailsTrip trip = Module.TripGetById(Convert.ToInt32(Request.QueryString["tripid"]));
                return Module.SupplierGetAll(costypeid, trip.Organization);
            }

            if (Request.QueryString["orgid"] != null)
            {
                Organization organization =
                    Module.OrganizationGetById(Convert.ToInt32(Request.QueryString["orgid"]));
                return Module.SupplierGetAll(costypeid, organization);
            }
            return Module.SupplierGetAll(costypeid);
        }
        public IList Guides
        {
            get
            {
                if (_guides == null)
                {
                    if (Request.QueryString["tripid"] != null)
                    {
                        SailsTrip trip = Module.TripGetById(Convert.ToInt32(Request.QueryString["tripoid"]));
                        _guides = Module.GuidesGetAll(trip.Organization);
                    }
                    else if (Request.QueryString["orgid"] != null)
                    {
                        Organization organization =
                            Module.OrganizationGetById(Convert.ToInt32(Request.QueryString["orgid"]));
                        _guides = Module.GuidesGetAll(organization);
                    }
                    else
                    {
                        _guides = Module.GuidesGetAll();
                    }
                }
                return _guides;
            }
        }
        public IList DailyCost
        {
            get
            {
                if (_dailyCost == null)
                {
                    _dailyCost = Module.CostTypeGetDailyInput();
                }
                return _dailyCost;
            }
        }

        public SailsTrip ActiveTrip
        {
            get
            {
                if (_trip == null && Request.QueryString["tripid"] != null)
                {
                    _trip = Module.TripGetById(Convert.ToInt32(Request.QueryString["tripid"]));
                }
                return _trip;
            }
        }

        private readonly Dictionary<int, int> _customers = new Dictionary<int, int>();
        private readonly Dictionary<int, int> _customerByOrg = new Dictionary<int, int>();
        private IList _operators;
        public IList Operators
        {
            get
            {
                if (_operators == null)
                    _operators = Module.OperatorGetAll();
                return _operators;
            }
            set { _operators = value; }
        }
        public BookingReportBLL BookingReportBLL
        {
            get
            {
                if (bookingReportBLL == null)
                {
                    bookingReportBLL = new BookingReportBLL();
                }
                return bookingReportBLL;
            }
        }
        public IList<Booking> ListBooking
        {
            get
            {
                var regionOfUser = Module.OrganizationGetByUser(UserIdentity);
                var listBooking = BookingReportBLL.BookingGetAllByCriterion(Date, StatusType.Approved).Future().ToList();
                foreach (UserOrganization userRegion in regionOfUser)
                {
                    listBooking = listBooking.Where(x => x.Trip.Organization.Id == userRegion.Organization.Id).ToList();
                }
                return listBooking;
            }
        }
        public IList<ExpenseService> ListExpenseService
        {
            get
            {
                var regionOfUser = Module.OrganizationGetByUser(UserIdentity);
                var listExpenseService = BookingReportBLL.ExpenseServiceGetAllByCriterion(Date).Future().ToList();
                foreach (UserOrganization userRegion in regionOfUser)
                {
                    listExpenseService = listExpenseService.Where(x => x.Expense.Trip.Organization.Id == userRegion.Organization.Id).ToList();
                }
                return listExpenseService;
            }
        }
        public string LockOrUnlock
        {
            get
            {
                var status = "Unlock";
                if (!ListBooking.Select(x => x.LockStatus).Contains("Unlocked") &&
                    !(ListBooking.Select(x => x.LockStatus).Contains("") || ListBooking.Select(x => x.LockStatus).Contains(null)))
                {
                    status = "Lock";
                }
                else
                    if (!ListExpenseService.Select(x => x.LockStatus).Contains("Unlocked") &&
                        !(ListExpenseService.Select(x => x.LockStatus).Contains("") || ListExpenseService.Select(x => x.LockStatus).Contains(null)))
                    {
                        status = "Lock";
                    }
                return status;
            }
        }
        public bool CanAddExpense
        {
            get
            {
                return PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowAddDailyExpense);
            }
        }
        public bool CanEditExpense
        {
            get
            {
                return PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowEditDailyExpense);
            }
        }
        public bool CanDeleteExpense
        {
            get
            {
                return PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowDeleteDailyExpense);
            }
        }
        public bool CanLockBooking
        {
            get
            {
                return PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowLockBooking);
            }
        }
        public void Page_Load(object sender, EventArgs e)
        {
            Title = "Booking by date";
            if (!IsPostBack)
            {
                txtSearchCode.Text = SearchCode;
                _services = new ArrayList();
                foreach (CostType type in DailyCost)
                {
                    if (type.IsSupplier)
                    {
                        _services.Add(type);
                    }
                }

                if (ActiveTrip == null)
                {
                    plhOperator.Visible = false;
                }

                var list = Module.OrganizationGetByUser(UserIdentity);
                if (list.Count == 1)
                {
                    string date = string.Empty;
                    if (Request.QueryString["date"] == null)
                    {
                        date = DateTime.Today.ToOADate().ToString();
                        date = "&date=" + date;
                        PageRedirect(string.Format("BookingReport.aspx?NodeId={0}&SectionId={1}{3}&orgid={2}",
                                               Node.Id, Section.Id, ((UserOrganization)list[0]).Organization.Id, date));
                        return;
                    }

                }

                if (Request.QueryString["Date"] != null)
                {
                    txtDate.Text =
                        DateTime.FromOADate(Convert.ToDouble(Request.QueryString["Date"])).ToString("dd/MM/yyyy");
                }
                else
                {
                    txtDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                }

                if (ActiveTrip != null)
                {
                    DateTime adate = DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    string script =
                        string.Format(
                            "window.location='CustomerComment.aspx?NodeId={0}&SectionId={1}&cruiseid={2}&date={3}'",
                            Node.Id, Section.Id, ActiveTrip.Id, adate.ToOADate());
                    btnComment.Attributes.Add("onclick", script);

                    var expense = Module.ExpenseGetByDate(ActiveTrip, adate);
                    if (expense.NumberOfGroup > 0)
                    {
                        _numberOfGroups = expense.NumberOfGroup;
                    }
                }
                else
                {
                    btnComment.Visible = false;
                }

                GetDataSource();
                rptBookingList.DataBind();

                rptOrganization.DataSource = Module.OrganizationGetAllRoot();
                rptOrganization.DataBind();

                if (Request.QueryString["orgid"] == null)
                {
                    if (ActiveTrip == null)
                    {
                        rptTrips.DataSource = Module.TripGetAll(false, UserIdentity).Cast<SailsTrip>().OrderBy(x => x.Name);
                        rptTrips.DataBind();
                    }
                    else
                    {
                        rptTrips.DataSource = Module.TripGetByOrganization(ActiveTrip.Organization).Cast<SailsTrip>().OrderBy(x => x.Name);
                        rptTrips.DataBind();
                    }
                }
                else
                {
                    rptTrips.DataSource =
                        Module.TripGetByOrganization(
                            Module.OrganizationGetById(Convert.ToInt32(Request.QueryString["orgid"]))).Cast<SailsTrip>().OrderBy(x => x.Name);
                    rptTrips.DataBind();
                }

                DateTime today = Request.QueryString["Date"] != null ? DateTime.FromOADate(Convert.ToDouble(Request.QueryString["Date"])) : DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var shadows = Module.GetBookingShadow(today);
                var listHistories = new ArrayList();
                foreach (Booking booking in shadows)
                {
                    var histories = Module.BookingGetHistory(booking);
                    if (histories == null) continue;
                    for (int ii = histories.Count - 1; ii >= 0; ii--)
                    {
                        if (((BookingHistory)(histories[ii])).StartDate == today || ((BookingHistory)(histories[ii])).Status == StatusType.Cancelled)
                        {
                            var bh = (BookingHistory)(histories[ii]);
                            if (bh.Date > today.AddDays(-2) && bh.Date < today.AddDays(2))
                            {
                                listHistories.Add(booking);
                            }

                            break;
                        }
                    }
                }

                rptShadows.DataSource = listHistories;
                rptShadows.DataBind();

            }

            if (ActiveTrip == null)
            {
                btnExport.Visible = false;
            }

            foreach (RepeaterItem rptItem in rptBookingList.Items)
            {
                var ddlGroup = (DropDownList)rptItem.FindControl("ddlGroup");
                if (ddlGroup.SelectedIndex >= 0)
                {
                    var hiddenId = (HiddenField)rptItem.FindControl("hiddenId");
                    var bk = Module.BookingGetById(Convert.ToInt32(hiddenId.Value));
                    int group = Convert.ToInt32(ddlGroup.SelectedValue);
                    if (bk.Group != group)
                    {
                        bk.Group = group;
                        Module.SaveOrUpdate(bk);
                    }
                }
            }

        }
        public void Page_Unload(object sender, EventArgs e)
        {
            if (bookingReportBLL != null)
            {
                bookingReportBLL.Dispose();
                bookingReportBLL = null;
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

        protected void btnSaveExpenses_Click(object sender, EventArgs e)
        {
            IList list;
            int count = GetData(out list, false);
            DateTime date = Request.QueryString["Date"] != null ? DateTime.FromOADate(Convert.ToDouble(Request.QueryString["Date"])) : DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            foreach (RepeaterItem rptItem in rptCruiseExpense.Items)
            {
                var hiddenId = (HiddenField)rptItem.FindControl("hiddenId");
                var cruise = Module.TripGetById(Convert.ToInt32(hiddenId.Value));
                var expense = Module.ExpenseGetByDate(cruise, date);

                if (ActiveTrip != null)
                {
                    if (ddlOperators.SelectedIndex > 0)
                    {
                        expense.Operator = Module.UserGetById(Convert.ToInt32(ddlOperators.SelectedValue));
                        expense.OperatorName = expense.Operator.FullName;
                        expense.OperatorPhone = expense.Operator.Website;
                    }
                    else
                    {
                        expense.OperatorName = txtOperator.Text;
                        expense.OperatorPhone = txtPhone.Text;
                    }
                    if (ddlSaleInCharge.SelectedIndex > 0)
                    {
                        expense.SaleInCharge = Module.UserGetById(Convert.ToInt32(ddlSaleInCharge.SelectedValue));
                    }
                }

                var rptServices = (Repeater)rptItem.FindControl("rptServices");
                IList expenseList = rptServicesToIList(rptServices);
                int numberOfGroup = 1;
                foreach (ExpenseService service in expenseList)
                {
                    numberOfGroup = Math.Max(service.Group, numberOfGroup);
                }
                expense.NumberOfGroup = numberOfGroup;
                expense.IsEvent = true;

                if (expense.Id < 0)
                {
                    Module.SaveOrUpdate(expense);
                }

                foreach (ExpenseService service in expenseList)
                {
                    service.Expense = expense;
                    if (service.IsRemoved)
                    {
                        if (service.Id > 0)
                        {
                            expense.Services.Remove(service);
                        }
                    }
                    else
                    {
                        Module.SaveOrUpdate(service);
                    }
                    foreach (var expenseHistory in service.ListPendingExpenseHistory)
                    {
                        BookingReportBLL.ExpenseHistorySaveOrUpdate(expenseHistory);
                    }
                }

                Module.SaveOrUpdate(expense);
            }

            foreach (RepeaterItem rptItem in rptBookingList.Items)
            {
                var ddlGroup = (DropDownList)rptItem.FindControl("ddlGroup");
                if (ddlGroup.SelectedIndex >= 0)
                {
                    var hiddenId = (HiddenField)rptItem.FindControl("hiddenId");
                    var bk = Module.BookingGetById(Convert.ToInt32(hiddenId.Value));
                    int group = Convert.ToInt32(ddlGroup.SelectedValue);
                    if (bk.Group != group)
                    {
                        bk.Group = group;
                        Module.SaveOrUpdate(bk);
                    }
                }
            }
            LoadService(date);
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            IList list;
            int count = GetData(out list, false);
            if (count == 0)
            {
                ShowError(Resources.errorNoBookingSave);
                return;
            }
            DateTime date = Request.QueryString["Date"] != null ? DateTime.FromOADate(Convert.ToDouble(Request.QueryString["Date"])) : DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            IList expenseList = null;
            foreach (RepeaterItem rptItem in rptCruiseExpense.Items)
            {
                HiddenField hiddenId = (HiddenField)rptItem.FindControl("hiddenId");
                SailsTrip cruise = Module.TripGetById(Convert.ToInt32(hiddenId.Value));
                SailExpense expense = Module.ExpenseGetByDate(cruise, date);

                if (ActiveTrip != null)
                {
                    if (ddlOperators.SelectedIndex > 0)
                    {
                        expense.Operator = Module.UserGetById(Convert.ToInt32(ddlOperators.SelectedValue));
                        expense.OperatorName = expense.Operator.FullName;
                        expense.OperatorPhone = expense.Operator.Website;
                    }
                    else
                    {
                        expense.OperatorName = txtOperator.Text;
                        expense.OperatorPhone = txtPhone.Text;
                    }
                    if (ddlSaleInCharge.SelectedIndex > 0)
                    {
                        expense.SaleInCharge = Module.UserGetById(Convert.ToInt32(ddlSaleInCharge.SelectedValue));
                    }
                }

                if (expense.Id < 0)
                {
                    Module.SaveOrUpdate(expense);
                }

                Repeater rptServices = (Repeater)rptItem.FindControl("rptServices");
                expenseList = rptServicesToIList(rptServices);
                int numberOfGroup = 1;
                foreach (ExpenseService service in expenseList)
                {
                    numberOfGroup = Math.Max(service.Group, numberOfGroup);
                }
                expense.NumberOfGroup = numberOfGroup;
                expense.IsEvent = true;

                foreach (ExpenseService service in expenseList)
                {
                    service.Expense = expense;
                    if (service.IsRemoved)
                    {
                        if (service.Id > 0)
                        {
                            expense.Services.Remove(service);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(service.Name) || service.Cost > 0)
                        {
                            Module.SaveOrUpdate(service);
                        }
                    }
                }
                Module.SaveOrUpdate(expense);
            }
            LoadService(date);
            string tplPath = Server.MapPath("/Modules/Sails/Admin/ExportTemplates/Lenh_dieu_tour.xls");
            TourCommand.Export(list, count, expenseList, Date, BookingFormat, Response, tplPath, ActiveTrip, this);
        }
        protected void btnExportRoom_Click(object sender, EventArgs e)
        {
            IList list;
            GetData(out list, false);
            int totalRows = 0;
            foreach (Booking booking in list)
            {
                totalRows += booking.BookingRooms.Count;
            }
            ExcelFile excelFile = new ExcelFile();
            excelFile.LoadXls(Server.MapPath("/Modules/Sails/Admin/ExportTemplates/Rooming_list.xls"));
            ExcelWorksheet sheet = excelFile.Worksheets[0];
            const int firstrow = 3;
            sheet.Rows[firstrow].InsertCopy(totalRows - 1, sheet.Rows[firstrow]);
            int curr = firstrow;
            foreach (Booking booking in list)
            {
                foreach (BookingRoom room in booking.BookingRooms)
                {
                    sheet.Cells[curr, 0].Value = curr - firstrow + 1;
                    string name = string.Empty;
                    foreach (Customer customer in room.Customers)
                    {
                        if (!string.IsNullOrEmpty(customer.Fullname))
                        {
                            name += customer.Fullname + "\n";
                        }
                    }
                    if (name.Length > 0)
                    {
                        name = name.Remove(name.Length - 1);
                    }
                    sheet.Cells[curr, 1].Value = name;
                    sheet.Cells[curr, 2].Value = room.Adult + room.Child;
                    sheet.Cells[curr, 3].Value = room.RoomType.Name;
                    if (room.Room != null)
                    {
                        sheet.Cells[curr, 4].Value = room.Room.Name;
                    }
                    curr++;
                }
            }
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AppendHeader("content-disposition",
                                  "attachment; filename=" + string.Format("Roomlist{0:dd_MMM}.xls", Date));

            MemoryStream m = new MemoryStream();

            excelFile.SaveXls(m);

            Response.OutputStream.Write(m.GetBuffer(), 0, m.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();

            m.Close();
            Response.End();
        }

        protected void btnExportWelcome_Click(object sender, EventArgs e)
        {
            IList bklist;
            GetData(out bklist, false);
            IList list = new ArrayList();
            foreach (Booking booking in bklist)
            {
                foreach (Customer customer in booking.Customers)
                {
                    list.Add(customer);
                }
            }
            int count = list.Count;
            ExcelFile excelFile = new ExcelFile();
            excelFile.LoadXls(Server.MapPath("/Modules/Sails/Admin/ExportTemplates/welcome_board.xls"));
            ExcelWorksheet sheet = excelFile.Worksheets[0];
            CellRange range = sheet.Cells.GetSubrangeRelative(0, 0, 12, 6);

            const int firstrow = 0;
            for (int ii = 1; ii < count; ii++)
            {
                range.CopyTo(ii * 6, 0);
                for (int jj = 0; jj < 6; jj++)
                {
                    sheet.Rows[ii * 6 + jj].Height = sheet.Rows[jj].Height;
                }
            }
            int curr = firstrow;
            foreach (Customer customer in list)
            {
                string name = customer.Fullname;
                sheet.Cells[curr + 2, 0].Value = name;
                sheet.Cells[curr + 4, 4].Value = customer.Booking.PickupAddress;
                sheet.Cells[curr + 4, 9].Value = UserIdentity.FullName;
                curr += 6;
            }

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AppendHeader("content-disposition",
                                  "attachment; filename=" + string.Format("WelcomeBoard{0:dd_MMM}", Date));

            MemoryStream m = new MemoryStream();

            excelFile.SaveXls(m);

            Response.OutputStream.Write(m.GetBuffer(), 0, m.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();

            m.Close();
            Response.End();
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            IList dataSource;
            GetData(out dataSource, false);

            IList customers = new ArrayList();
            DateTime startDate = DateTime.MinValue;

            foreach (Booking book in dataSource)
            {
                if (startDate < book.StartDate)
                {
                    startDate = book.StartDate;
                }
                foreach (BookingRoom room in book.BookingRooms)
                {
                    foreach (Customer customer in room.RealCustomers)
                    {
                        customers.Add(customer);
                    }
                }
            }

            if (startDate == DateTime.MinValue)
            {
                ShowError("Không có thông tin");
                return;
            }
            string tpl = Server.MapPath("/Modules/Sails/Admin/ExportTemplates/Clients Details.xls");
            ReportEngine.CustomerDetails(customers, startDate, tpl, Response);
        }

        protected void btnProvisional_Click(object sender, EventArgs e)
        {
            IList list;
            GetData(out list, false);
            Purpose defaultPurpose = Module.PurposeGetById(3);
            ReportEngine.Provisional(list, Date, defaultPurpose, this, Response,
                                     Server.MapPath("/Modules/Sails/Admin/ExportTemplates/KhaiBaoTamTru.xls"));
        }

        protected void btnIncomeDate_Click(object sender, EventArgs e)
        {


            IList data = Module.BookingGetByStartDate(Date, null, false);

            ReportEngine.IncomeByDate(data, this, Response,
                                      Server.MapPath("/Modules/Sails/Admin/ExportTemplates/IncomeDate.xls"));
        }

        protected void rptBookingList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                var thTrip = (HtmlTableCell)e.Item.FindControl("thTrip");
                var thSpecialRequest = (HtmlTableCell)e.Item.FindControl("thSpecialRequest");
                var thTime = (HtmlTableCell)e.Item.FindControl("thTime");
                var thFligthDetails = (HtmlTableCell)e.Item.FindControl("thFlightDetails");
                var thCarRequirements = (HtmlTableCell)e.Item.FindControl("thCarRequirements");
                var thDropoffAddress = (HtmlTableCell)e.Item.FindControl("thDropoffAddress");

                if (ActiveTrip != null)
                {
                    if (ActiveTrip.Name.ToLower() == "airport transfer")
                    {
                        if (thTrip != null)
                            thTrip.Visible = false;
                        if (thTime != null)
                            thTime.Visible = true;
                        if (thFligthDetails != null)
                            thFligthDetails.Visible = true;
                        if (thCarRequirements != null)
                            thCarRequirements.Visible = true;
                        if (thDropoffAddress != null)
                            thDropoffAddress.Visible = true;
                    }
                    else
                    {
                        if (thTrip != null)
                            thTrip.Visible = true;
                        if (thTime != null)
                            thTime.Visible = false;
                        if (thFligthDetails != null)
                            thFligthDetails.Visible = false;
                        if (thCarRequirements != null)
                            thCarRequirements.Visible = false;
                        if (thDropoffAddress != null)
                            thDropoffAddress.Visible = false;
                    }
                }
                else
                {
                    if (thTrip != null)
                        thTrip.Visible = true;
                    if (thTime != null)
                        thTime.Visible = false;
                    if (thFligthDetails != null)
                        thFligthDetails.Visible = false;
                    if (thCarRequirements != null)
                        thCarRequirements.Visible = false;
                    if (thDropoffAddress != null)
                        thDropoffAddress.Visible = false;
                }
            }

            if (e.Item.DataItem is Booking)
            {
                Booking booking = e.Item.DataItem as Booking;
                HtmlTableRow trItem = (HtmlTableRow)e.Item.FindControl("trItem");
                if (booking.StartDate < Date)
                {
                    trItem.Attributes.Add("style", "background-color :" + SailsModule.WARNING);
                }
                Label label_NameOfPax = (Label)e.Item.FindControl("label_NameOfPax");
                Label label_NoOfAdult = (Label)e.Item.FindControl("label_NoOfAdult");
                Label label_NoOfChild = (Label)e.Item.FindControl("label_NoOfChild");
                Label label_NoOfBaby = (Label)e.Item.FindControl("label_NoOfBaby");
                Label label_NoOfDoubleCabin = (Label)e.Item.FindControl("label_NoOfDoubleCabin");
                Label label_NoOfTwinCabin = (Label)e.Item.FindControl("label_NoOfTwinCabin");
                Label label_NoOfTransferAdult = (Label)e.Item.FindControl("label_NoOfTransferAdult");
                Label label_NoOfTransferChild = (Label)e.Item.FindControl("label_NoOfTransferChild");
                Label label_TotalPrice = (Label)e.Item.FindControl("label_TotalPrice");
                HyperLink hyperLink_Partner = (HyperLink)e.Item.FindControl("hyperLink_Partner");
                Literal ltrTime = (Literal)e.Item.FindControl("ltrTime");
                Literal ltrFlightDetails = (Literal)e.Item.FindControl("ltrFlightDetails");
                Literal ltrCarRequirements = (Literal)e.Item.FindControl("ltrCarRequirements");
                var ltrDropoffAddress = (Literal)e.Item.FindControl("ltrDropoffAddress");
                var tdTrip = (HtmlTableCell)e.Item.FindControl("tdTrip");
                var tdSpecialRequest = (HtmlTableCell)e.Item.FindControl("tdSpecialRequest");
                var tdTime = (HtmlTableCell)e.Item.FindControl("tdTime");
                var tdFligthDetails = (HtmlTableCell)e.Item.FindControl("tdFlightDetails");
                var tdCarRequirements = (HtmlTableCell)e.Item.FindControl("tdCarRequirements");
                var tdFeedback = (HtmlTableCell)e.Item.FindControl("tdFeedback");
                var tdDropoffAddress = (HtmlTableCell)e.Item.FindControl("tdDropoffAddress");


                if (booking.SeeoffTime >= Date && booking.SeeoffTime <= Date.Add(new TimeSpan(23, 59, 59)))
                {
                    if (ltrTime != null)
                    {
                        try
                        {

                            ltrTime.Text = booking.SeeoffTime.Value.ToString("HH:mm");

                        }
                        catch (Exception)
                        {
                            ltrTime.Text = "";
                        }
                    }
                }

                if (booking.PickupTime >= Date && booking.PickupTime <= Date.Add(new TimeSpan(23, 59, 59)))
                {
                    if (ltrTime != null)
                    {
                        try
                        {
                            ltrTime.Text = booking.PickupTime.Value.ToString("HH:mm");
                        }
                        catch (Exception)
                        {
                            ltrTime.Text = "";
                        }
                    }
                }



                Literal litIndex = (Literal)e.Item.FindControl("litIndex");
                litIndex.Text =
                    (e.Item.ItemIndex + 1).ToString();

                label_NameOfPax.Text = booking.CustomerName;
                if (booking.Agency != null)
                {
                    hyperLink_Partner.Text = booking.Agency.Name;
                }
                else
                {
                    hyperLink_Partner.Text = SailsModule.NOAGENCY;
                }
                label_NoOfAdult.Text = booking.Adult.ToString();
                _adult += booking.Adult;
                label_NoOfChild.Text = booking.Child.ToString();
                label_NoOfBaby.Text = booking.Baby.ToString();
                _child += booking.Child;
                _baby += booking.Baby;
                label_NoOfDoubleCabin.Text = booking.DoubleCabin.ToString();
                label_NoOfTwinCabin.Text = string.Format("{1}({0} adults)", booking.TwinCabin,
                                                         booking.TwinCabin / 2 + booking.TwinCabin % 2);
                _doubleCabin += booking.DoubleCabin;
                _twin += booking.TwinCabin;
                bool transfer = false;
                foreach (ExtraOption service in booking.ExtraServices)
                {
                    if (service.Id == SailsModule.TRANSFER)
                    {
                        transfer = true;
                        break;
                    }
                }
                if (transfer)
                {
                    label_NoOfTransferAdult.Text = label_NoOfAdult.Text;
                    label_NoOfTransferChild.Text = label_NoOfChild.Text;
                    _transferChild += booking.Child;
                    _transferAdult += booking.Adult;
                }
                else
                {
                    label_NoOfTransferAdult.Text = @"0";
                    label_NoOfTransferChild.Text = @"0";
                }

                label_TotalPrice.Text = booking.TotalVND.ToString("#,0");
                _total += booking.TotalVND;

                Label labelItinerary = e.Item.FindControl("labelItinerary") as Label;
                if (labelItinerary != null)
                {
                    labelItinerary.Text = booking.Trip.TripCode;
                }

                Label labelPuAddress = e.Item.FindControl("labelPuAddress") as Label;
                if (labelPuAddress != null)
                {
                    labelPuAddress.Text = booking.PickupAddress;
                }


                if (booking.Trip.Name.ToLower() == "airport transfer")
                {
                    if (booking.SeeoffTime >= Date && booking.SeeoffTime <= Date.Add(new TimeSpan(23, 59, 59)))
                    {

                        if (labelPuAddress != null)
                        {
                            labelPuAddress.Text = booking.SOPickupAddress;
                        }


                        if (ltrDropoffAddress != null)
                        {
                            ltrDropoffAddress.Text = booking.SODropoffAddress;
                        }

                    }

                    if (booking.PickupTime >= Date && booking.PickupTime <= Date.Add(new TimeSpan(23, 59, 59)))
                    {
                        if (labelPuAddress != null)
                        {
                            labelPuAddress.Text = booking.PUPickupAddress;
                        }

                        if (ltrDropoffAddress != null)
                        {
                            ltrDropoffAddress.Text = booking.PUDropoffAddress;
                        }

                    }
                }

                Label labelSpecialRequest = e.Item.FindControl("labelSpecialRequest") as Label;
                if (labelSpecialRequest != null)
                {
                    labelSpecialRequest.Text = booking.SpecialRequest;
                }

                HyperLink hplCode = e.Item.FindControl("hplCode") as HyperLink;
                if (hplCode != null)
                {
                    if (true)
                    {
                        if (booking.CustomBookingId > 0)
                        {
                            hplCode.Text = string.Format(BookingFormat, booking.CustomBookingId);
                        }
                        else
                        {
                            hplCode.Text = string.Format(BookingFormat, booking.Id);
                        }
                    }
                    hplCode.NavigateUrl = string.Format("BookingView.aspx?NodeId={0}&SectionId={1}&BookingId={2}",
                                                        Node.Id, Section.Id, booking.Id);
                }

                HtmlAnchor anchorFeedback = e.Item.FindControl("anchorFeedback") as HtmlAnchor;
                if (anchorFeedback != null)
                {
                    string url = string.Format("SurveyInput.aspx?NodeId={0}&SectionId={1}&BookingId={2}", Node.Id,
                                               Section.Id, booking.Id);
                    anchorFeedback.Attributes.Add("onclick",
                                            CMS.ServerControls.Popup.OpenPopupScript(url, "Survey input", 600, 800));
                }

                if (_numberOfGroups < int.MaxValue)
                {
                    DropDownList ddlGroup = (DropDownList)e.Item.FindControl("ddlGroup");
                    for (int ii = 1; ii <= _numberOfGroups; ii++)
                    {
                        ddlGroup.Items.Add(ii.ToString());
                    }
                    ddlGroup.SelectedValue = booking.Group.ToString();
                }
                else if (Request.QueryString["tripid"] == null)
                {
                    DropDownList ddlGroup = (DropDownList)e.Item.FindControl("ddlGroup");
                    ddlGroup.Visible = false;

                    Literal litGroup = (Literal)e.Item.FindControl("litGroup");
                    litGroup.Text = booking.Group.ToString();
                }

                if (ActiveTrip != null)
                {
                    if (ActiveTrip.Name.ToLower() == "airport transfer")
                    {
                        if (tdTrip != null)
                            tdTrip.Visible = false;
                        if (tdTime != null)
                            tdTime.Visible = true;
                        if (tdFligthDetails != null)
                            tdFligthDetails.Visible = true;
                        if (tdCarRequirements != null)
                            tdCarRequirements.Visible = true;
                        if (tdFeedback != null)
                            tdFeedback.Visible = false;
                        if (tdDropoffAddress != null)
                            tdDropoffAddress.Visible = true;
                        if (booking.SeeoffTime >= Date && booking.SeeoffTime <= Date.Add(new TimeSpan(23, 59, 59)))
                        {
                            trItem.Attributes.Add("style", "background-color:violet");
                            if (booking.SOCarRequirements != null)
                            {
                                ltrCarRequirements.Text = booking.SOCarRequirements;
                            }

                            if (booking.SOFlightDetails != null)
                            {
                                ltrFlightDetails.Text = booking.SOFlightDetails;
                            }

                        }
                        if (booking.PickupTime >= Date && booking.PickupTime <= Date.Add(new TimeSpan(23, 59, 59)))
                        {
                            trItem.Attributes.Add("style", "background-color:yellowgreen");

                            if (booking.PUCarRequirements != null)
                            {
                                ltrCarRequirements.Text = booking.PUCarRequirements;
                            }

                            if (booking.PUFlightDetails != null)
                            {
                                ltrFlightDetails.Text = booking.PUFlightDetails;
                            }

                        }
                    }
                    else
                    {
                        if (tdTrip != null)
                            tdTrip.Visible = true;
                        if (tdTime != null)
                            tdTime.Visible = false;
                        if (tdFligthDetails != null)
                            tdFligthDetails.Visible = false;
                        if (tdCarRequirements != null)
                            tdCarRequirements.Visible = false;
                        if (tdFeedback != null)
                            tdFeedback.Visible = true;
                        if (tdDropoffAddress != null)
                            tdDropoffAddress.Visible = false;
                    }
                }
                else
                {
                    if (tdTrip != null)
                        tdTrip.Visible = true;
                    if (tdTime != null)
                        tdTime.Visible = false;
                    if (tdFligthDetails != null)
                        tdFligthDetails.Visible = false;
                    if (tdCarRequirements != null)
                        tdCarRequirements.Visible = false;
                    if (tdFeedback != null)
                        tdFeedback.Visible = true;
                    if (tdDropoffAddress != null)
                        tdDropoffAddress.Visible = false;
                }

                if (booking.Id == FoundBookingId)
                {
                    trItem.Attributes.Add("style", "background-color:#F4F900");
                }
            }

            if (e.Item.ItemType == ListItemType.Footer)
            {
                Label label_NoOfAdult = (Label)e.Item.FindControl("label_NoOfAdult");
                label_NoOfAdult.Text = _adult.ToString();
                Label label_NoOfChild = (Label)e.Item.FindControl("label_NoOfChild");
                label_NoOfChild.Text = _child.ToString();
                Label label_NoOfBaby = (Label)e.Item.FindControl("label_NoOfBaby");
                label_NoOfBaby.Text = _baby.ToString();
                Label label_NoOfDoubleCabin = (Label)e.Item.FindControl("label_NoOfDoubleCabin");
                label_NoOfDoubleCabin.Text = _doubleCabin.ToString();
                Label label_NoOfTwinCabin = (Label)e.Item.FindControl("label_NoOfTwinCabin");
                label_NoOfTwinCabin.Text = string.Format("{1}({0} adults)", _twin, _twin / 2 + _twin % 2);
                Label label_NoOfTransferAdult = (Label)e.Item.FindControl("label_NoOfTransferAdult");
                label_NoOfTransferAdult.Text = _transferAdult.ToString();
                Label label_NoOfTransferChild = (Label)e.Item.FindControl("label_NoOfTransferChild");
                label_NoOfTransferChild.Text = _transferChild.ToString();
                Label label_TotalPrice = (Label)e.Item.FindControl("label_TotalPrice");
                label_TotalPrice.Text = _total.ToString("#,####");
            }
        }

        protected void rptBookingList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
        }

        protected void rptServices_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is ExpenseService)
            {
                ExpenseService service = (ExpenseService)e.Item.DataItem;

                if (service.Group != _currentGroup && service.Group > 0)
                {
                    _currentGroup = service.Group;
                    HtmlTableRow seperator = (HtmlTableRow)e.Item.FindControl("seperator");
                    seperator.Visible = true;
                    if (service.Group == 1) { seperator.Visible = false; }
                }

                var hiddenId = (HiddenField)e.Item.FindControl("hiddenId");
                var hiddenType = (HiddenField)e.Item.FindControl("hiddenType");
                var txtName = (TextBox)e.Item.FindControl("txtName");
                var txtPhone = (TextBox)e.Item.FindControl("txtPhone");
                var ddlSuppliers = (DropDownList)e.Item.FindControl("ddlSuppliers");
                var ddlGuides = (DropDownList)e.Item.FindControl("ddlGuides");
                var ddlGroups = (DropDownList)e.Item.FindControl("ddlGroups");
                var txtCost = (TextBox)e.Item.FindControl("txtCost");
                var litType = (Literal)e.Item.FindControl("litType");
                var btnRemove = (Button)e.Item.FindControl("btnRemove");
                var cddlDriver = (CascadingDropDown)e.Item.FindControl("cddlDriver");
                ddlGroups.Items.Add(new ListItem(service.Group.ToString(), service.Group.ToString()));
                if (_numberOfGroups < int.MaxValue)
                {
                    for (int ii = 1; ii <= _numberOfGroups + 1; ii++)
                    {
                        ddlGroups.Items.Add(ii.ToString());
                    }
                }

                hiddenId.Value = service.Id.ToString();
                hiddenType.Value = (service.Type.Id).ToString();
                if (service.Group >= 0)
                {
                    ddlGroups.SelectedValue = service.Group.ToString();
                }
                if (service.Type.IsCustomType)
                {
                    txtName.Text = service.Name;
                    txtPhone.Text = service.Phone;
                }
                else
                {
                    txtName.Enabled = false;
                    txtPhone.Enabled = false;
                    if (service.Supplier != null)
                    {
                        txtName.Text = service.Supplier.Name;
                        txtPhone.Text = service.Supplier.Phone;
                    }
                }
                txtCost.Text = service.Cost.ToString("#,#,0.#");

                txtCost.Attributes.Add("onchange", "this.value = addCommas(this.value);");
                ddlGuides.Visible = false;

                if (service.Type.Id < 0)
                {
                    litType.Text = @"Guide";
                    ddlGuides.DataSource = Guides;
                    ddlGuides.DataTextField = "Name";
                    ddlGuides.DataValueField = "Id";
                    ddlGuides.DataBind();
                    txtName.Visible = false;
                    ddlGuides.Visible = true;
                    ddlSuppliers.Visible = false;
                    if (string.IsNullOrEmpty(txtPhone.Text))
                    {
                        txtPhone.Text = @"AUTO FROM DATABASE";
                    }
                    txtPhone.Enabled = false;
                }
                else
                {
                    litType.Text = service.Type.Name;
                    if (service.Type.IsSupplier)
                    {
                        ddlSuppliers.DataSource = Suppliers(service.Type.Id);
                    }
                    else
                    {
                        ddlSuppliers.Visible = false;
                        e.Item.FindControl("btnRemove").Visible = false;
                        e.Item.FindControl("txtCost").Visible = false;
                    }
                }

                if (ddlSuppliers.Visible)
                {
                    ddlSuppliers.DataTextField = "Name";
                    ddlSuppliers.DataValueField = "Id";
                    ddlSuppliers.DataBind();
                    ddlSuppliers.Items.Insert(0, new ListItem("-- Supplier --", "0"));
                }

                if (service.Type.IsSupplier)
                {
                    if (service.Supplier != null)
                    {
                        ddlSuppliers.SelectedValue = service.Supplier.Id.ToString();
                    }
                }

                if (Request.QueryString["tripid"] == null)
                {
                    if (service.Cost == 0)
                    {
                        e.Item.Visible = false;
                    }
                }

                if (service.IsRemoved)
                {
                    e.Item.Visible = false;
                }
                else if (service.Type.IsSupplier)
                {
                    _totalCost += service.Cost;
                }
                if (service.LockStatus == "Locked")
                {
                    isExpenseLocked = true;
                    txtName.Attributes.Add("readonly", "readonly");
                    txtPhone.Attributes.Add("readonly", "readonly");
                    ddlSuppliers.Attributes.Add("disabled", "disabled");
                    ddlGuides.Attributes.Add("disabled", "disabled");
                    ddlGroups.Attributes.Add("disabled", "disabled");
                    txtCost.Attributes.Add("readonly", "readonly");
                    btnRemove.Attributes.Add("disabled", "disabled");
                }

                if (service.Type.Name == "Transport")
                {
                    cddlDriver.Visible = true;
                    cddlDriver.Items.Add(new ListItem("-- Driver --", "0"));
                    cddlDriver.DataSource = Module.ContactGetAllEnabled();
                    cddlDriver.DataTextField = "Name";
                    cddlDriver.DataValueField = "Id";
                    cddlDriver.DataParentField = "AgencyId";
                    cddlDriver.ParentClientID = ddlSuppliers.ClientID;
                    cddlDriver.DataBind();
                    cddlDriver.SelectedValue = service.Driver != null ? service.Driver.Id.ToString() : "0";
                    txtName.Visible = false;
                }
            }
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            Control control = ((Control)sender).Parent;
            control.Visible = false;
        }

        protected void btnAddServiceBlock_Click(object sender, EventArgs e)
        {
            Button btnAddServiceBlock = (Button)sender;
            Repeater rptServices = btnAddServiceBlock.Parent.FindControl("rptServices") as Repeater;
            IList list = rptServicesToIList(rptServices);
            SailsTrip trip = Module.TripGetById(Convert.ToInt32(btnAddServiceBlock.CommandArgument));

            int maxGroup = 1;
            foreach (ExpenseService temp in list)
            {
                maxGroup = Math.Max(temp.Group, maxGroup);
            }

            foreach (CostType costType in trip.CostTypes)
            {
                ExpenseService service = new ExpenseService();
                service.Type = costType;
                service.IsRemoved = !costType.IsSupplier;
                service.Group = maxGroup + 1;

                int index = 0;
                foreach (ExpenseService temp in list)
                {
                    index += 1;
                }

                list.Insert(index, service);
            }
            _numberOfGroups = maxGroup + 1;
            rptServices.DataSource = list;
            rptServices.DataBind();
        }

        protected void btnAddService_Click(object sender, EventArgs e)
        {
            var btnAddService = (Button)sender;
            var rptServices = btnAddService.Parent.Parent.Parent.FindControl("rptServices") as Repeater;
            IList list = rptServicesToIList(rptServices);
            var service = new ExpenseService();
            CostType costType = Module.CostTypeGetById(Convert.ToInt32(btnAddService.CommandArgument));
            service.Type = costType;
            service.IsRemoved = !costType.IsSupplier;

            int index = 0;
            int maxGroup = 1;
            foreach (ExpenseService temp in list)
            {
                index += 1;
                maxGroup = Math.Max(temp.Group, maxGroup);
            }

            _numberOfGroups = maxGroup;
            list.Insert(index, service);
            rptServices.DataSource = list;
            rptServices.DataBind();
        }

        protected void rtpAddServices_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is CostType)
            {
                Button btnAddService = (Button)e.Item.FindControl("btnAddService");
                btnAddService.Text = @"Add " + ((CostType)e.Item.DataItem).Name;
            }
        }

        protected void rptTrips_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var hplTrip = e.Item.FindControl("hplTrip") as HyperLink;
                hplTrip.CssClass = "btn btn-default";
                var trip = (SailsTrip)e.Item.DataItem;

                if (trip.Id.ToString() == Request.QueryString["tripid"])
                {
                    hplTrip.CssClass = hplTrip.CssClass + " " + "active";
                }

                if (_customers.ContainsKey(trip.Id))
                {
                    hplTrip.Text = string.Format("{0}({1})", trip.TripCode, _customers[trip.Id]);
                    hplTrip.CssClass = hplTrip.CssClass + " " + "has-customer";
                }
                else
                {
                    hplTrip.Text = trip.TripCode;
                    hplTrip.CssClass = hplTrip.CssClass + " " + "hidden";
                }
                DateTime date = Request.QueryString["Date"] != null ? DateTime.FromOADate(Convert.ToDouble(Request.QueryString["Date"])) : DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                hplTrip.NavigateUrl = string.Format(
                    "BookingReport.aspx?NodeId={0}&SectionId={1}&Date={2}&tripid={3}", Node.Id, Section.Id,
                    date.ToOADate(), trip.Id);
            }
        }

        protected void btnDisplay_Click(object sender, EventArgs e)
        {
            BookingSearch();
        }

        protected void rptCruiseExpense_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is SailsTrip)
            {
                var cruise = (SailsTrip)e.Item.DataItem;
                var rptServices = e.Item.FindControl("rptServices") as Repeater;
                LoadService(rptServices, cruise, Date);

                var rptAddServices = e.Item.FindControl("rptAddServices") as Repeater;
                if (rptAddServices == null) return;
                if (Request.QueryString["tripid"] != null)
                {
                    rptAddServices.DataSource = _services;
                    rptAddServices.DataBind();
                }

                var btnAddServiceBlock = (Button)e.Item.FindControl("btnAddServiceBlock");
                btnAddServiceBlock.CommandArgument = cruise.Id.ToString();
                if (isExpenseLocked)
                {
                    btnAddServiceBlock.Attributes.Add("disabled", "disabled");
                    isExpenseLocked = false;
                }
            }
            else
            {
                Literal litSTotal = e.Item.FindControl("litTotal") as Literal;
                if (litSTotal != null)
                {
                    litSTotal.Text = _totalCost.ToString("#,##0.##");
                }
            }
        }

        protected void GetDataSource()
        {
            IList list;
            GetData(out list, true);
            rptBookingList.DataSource = list;
        }

        protected int GetData(out IList list, bool loadService)
        {
            ICriterion criterion = Expression.Eq(Booking.DELETED, false);
            criterion = Expression.And(criterion, Expression.Eq(Booking.STATUS, StatusType.Approved));

            criterion = Expression.And(criterion, Expression.Not(Expression.Eq("IsTransferred", true)));

            criterion = Module.AddDateExpression(criterion, Date);
            ICriterion airportTransferCriterion = null;
            ICriterion pickupTimeCriterion = null;
            pickupTimeCriterion = Expression.And(Expression.Ge("PickupTime", Date), Expression.Le("PickupTime", Date.Add(new TimeSpan(23, 59, 59))));
            ICriterion seeOffTimeCriterion = null;
            seeOffTimeCriterion = Expression.And(Expression.Ge("SeeoffTime", Date), Expression.Le("SeeoffTime", Date.Add(new TimeSpan(23, 59, 59))));
            ICriterion pickUpTimeSeeOffTimeCriterion = null;
            pickUpTimeSeeOffTimeCriterion = Expression.Or(pickupTimeCriterion, seeOffTimeCriterion);
            airportTransferCriterion = pickUpTimeSeeOffTimeCriterion;
            criterion = Expression.Or(criterion, airportTransferCriterion);
            int count;

            IList bookings = Module.BookingGetByCriterion(criterion, null, out count, 0, 0, false, UserIdentity);
            var filteredBookings1 = new List<Booking>();
            foreach (Booking booking in bookings)
            {
                if (booking.Trip.Name.ToLower() != "airport transfer")
                {
                    if (booking.StartDate <= Date && booking.EndDate > Date)
                    {
                        filteredBookings1.Add(booking);
                        continue;
                    }

                    if (booking.StartDate == Date && booking.StartDate >= booking.EndDate)
                    {
                        filteredBookings1.Add(booking);
                        continue;
                    }
                }

                if (booking.Trip.Name.ToLower() == "airport transfer")
                {
                    if (booking.PickupTime >= Date && booking.PickupTime <= Date.Add(new TimeSpan(23, 59, 59)))
                    {
                        filteredBookings1.Add(booking);
                        continue;
                    }

                    if (booking.SeeoffTime >= Date && booking.SeeoffTime <= Date.Add(new TimeSpan(23, 59, 59)))
                    {
                        filteredBookings1.Add(booking);
                        continue;
                    }

                }
            }
            bookings = filteredBookings1;
            foreach (Booking booking in bookings)
            {
                if (!_customers.ContainsKey(booking.Trip.Id))
                {
                    _customers.Add(booking.Trip.Id, 0);
                }
                _customers[booking.Trip.Id] += booking.Pax;

                if (booking.Trip.Organization != null)
                {
                    if (!_customerByOrg.ContainsKey(booking.Trip.Organization.Id))
                    {
                        _customerByOrg.Add(booking.Trip.Organization.Id, 0);
                    }
                    _customerByOrg[booking.Trip.Organization.Id] += booking.Pax;
                }
            }

            bool tripped = false;
            if (Request.QueryString["tripid"] != null)
            {
                SailsTrip cruise = Module.TripGetById(Convert.ToInt32(Request.QueryString["tripid"]));
                criterion = Expression.And(criterion, Expression.Eq("Trip", cruise));
                tripped = true;
            }

            if (!tripped && Request.QueryString["orgid"] != null)
            {
                Organization org = Module.OrganizationGetById(Convert.ToInt32(Request.QueryString["orgid"]));
                criterion = Expression.And(criterion, Expression.Eq("trip.Organization", org));
                tripped = true;
            }

            list = Module.BookingGetByCriterion(criterion, Order.Asc("Group"), out count, 0, 0, tripped, UserIdentity);
            var filteredBookings = new List<Booking>();
            foreach (Booking booking in list)
            {
                if (booking.Trip.Name.ToLower() != "airport transfer")
                {
                    if (booking.StartDate <= Date && booking.EndDate > Date)
                    {
                        filteredBookings.Add(booking);
                        continue;
                    }

                    if (booking.StartDate == Date && booking.StartDate >= booking.EndDate)
                    {
                        filteredBookings.Add(booking);
                        continue;
                    }
                }

                if (booking.Trip.Name.ToLower() == "airport transfer")
                {
                    if (booking.PickupTime >= Date && booking.PickupTime <= Date.Add(new TimeSpan(23, 59, 59)))
                    {
                        filteredBookings.Add(booking);
                        continue;
                    }

                    if (booking.SeeoffTime >= Date && booking.SeeoffTime <= Date.Add(new TimeSpan(23, 59, 59)))
                    {
                        filteredBookings.Add(booking);
                        continue;
                    }

                }
            }

            list = filteredBookings;

            if (Request.QueryString["tripid"] != null)
            {
                SailsTrip trip = Module.TripGetById(Convert.ToInt32(Request.QueryString["tripid"]));
            }

            if (loadService)
            {
                LoadService(Date);
            }

            return count;
        }

        private void LoadService(DateTime date)
        {
            _numberOfGroups = 0;
            IList trips;

            if (Request.QueryString["orgid"] == null)
            {
                if (ActiveTrip == null)
                {
                    trips = Module.TripGetAll(true, UserIdentity);
                    _numberOfGroups = int.MaxValue;
                }
                else
                {
                    trips = new ArrayList();
                    trips.Add(ActiveTrip);
                }
            }
            else
            {
                trips = Module.TripGetByOrganization(
                        Module.OrganizationGetById(Convert.ToInt32(Request.QueryString["orgid"])));
            }
            SailExpense expense = Module.ExpenseGetByDate(null, date);
            if (expense.Id < 0)
            {
                Module.SaveOrUpdate(expense);
            }

            var calculator = new ExpenseCalculator(Module, PartnershipManager);

            _customerCost = 0;
            _runningCost = 0;
            Dictionary<CostType, double> cost = calculator.ExpenseCalculate(null, expense); // Luôn tính toán chi phí với trip = null --> tính mọi chi phí phát sinh
            if (cost != null)
            {
                foreach (KeyValuePair<CostType, double> pair in cost)
                {
                    if (pair.Key.IsSupplier && !pair.Key.IsDailyInput && !pair.Key.IsDaily && !pair.Key.IsMonthly &&
                        !pair.Key.IsYearly)
                    {
                        _customerCost += pair.Value;
                    }
                    else if (pair.Key.IsSupplier && !pair.Key.IsDailyInput && pair.Key.IsDaily)
                    {
                        _runningCost += pair.Value;
                    }
                }
            }

            if (DailyCost.Count > 0)
            {
                rptCruiseExpense.DataSource = trips;
                rptCruiseExpense.DataBind();
            }
            else
            {
                plhDailyExpenses.Visible = false;
                rptCruiseExpense.Visible = false;
            }
        }

        private void LoadService(Repeater rptServices, SailsTrip cruise, DateTime date)
        {
            SailExpense expense = Module.ExpenseGetByDate(cruise, date);

            ddlOperators.DataSource = Operators;
            ddlOperators.DataTextField = "UserName";
            ddlOperators.DataValueField = "Id";
            ddlOperators.DataBind();
            ddlOperators.Items.Insert(0, "-- Please choose one --");

            if (expense.Operator != null)
            {
                var listitem = ddlOperators.Items.FindByValue(expense.Operator.Id.ToString());
                if (listitem != null)
                {
                    ddlOperators.SelectedValue = expense.Operator.Id.ToString();
                    txtOperator.ReadOnly = true;
                    txtPhone.ReadOnly = true;
                }
            }
            txtOperator.Text = expense.OperatorName;
            txtPhone.Text = expense.OperatorPhone;
            Role role = Module.RoleGetById(Convert.ToInt32(Module.ModuleSettings("Sale")));
            ddlSaleInCharge.DataSource = role.Users;

            ddlSaleInCharge.DataTextField = "UserName";
            ddlSaleInCharge.DataValueField = "Id";
            ddlSaleInCharge.DataBind();
            ddlSaleInCharge.Items.Insert(0, "-- Not selected --");

            if (expense.SaleInCharge != null)
            {
                ddlSaleInCharge.SelectedValue = expense.SaleInCharge.Id.ToString();
                txtSalePhone.Text = expense.SaleInCharge.Website;
            }

            var serviceMap = new Dictionary<CostType, bool>();

            foreach (CostType type in DailyCost)
            {
                serviceMap.Add(type, false);
            }

            IList services = new ArrayList();
            foreach (ExpenseService service in expense.Services)
            {
                try
                {
                    if (service.Type.IsDailyInput && !service.Type.IsMonthly && !service.Type.IsYearly)
                    {
                        serviceMap[service.Type] = true;
                        services.Add(service);
                        _numberOfGroups = Math.Max(_numberOfGroups, service.Group);
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }

            foreach (CostType type in DailyCost)
            {
                if (!serviceMap[type] && cruise.CostTypes.Contains(type))
                {
                    ExpenseService service = new ExpenseService();
                    service.Type = type;
                    service.IsOwnService = !type.IsSupplier;
                    services.Add(service);
                }
            }

            int count = 0;
            foreach (ExpenseService service in services)
            {
                if (service.Cost > 0 && service.Type.IsDailyInput)
                {
                    count++;
                    break;
                }
            }
            if (count > 0 || Request.QueryString["tripid"] != null)
            {
                rptServices.DataSource = services;
                rptServices.DataBind();
            }
            else
            {
                rptServices.Parent.Visible = false;
            }

        }

        protected IList rptServicesToIList(Repeater rptServices)
        {
            IList list = new ArrayList();
            foreach (RepeaterItem item in rptServices.Items)
            {
                HiddenField hiddenId = (HiddenField)item.FindControl("hiddenId");
                HiddenField hiddenType = (HiddenField)item.FindControl("hiddenType");
                TextBox txtName = (TextBox)item.FindControl("txtName");
                TextBox txtPhone = (TextBox)item.FindControl("txtPhone");
                DropDownList ddlSuppliers = (DropDownList)item.FindControl("ddlSuppliers");
                DropDownList ddlGuides = (DropDownList)item.FindControl("ddlGuides");
                DropDownList ddlGroups = (DropDownList)item.FindControl("ddlGroups");
                TextBox txtCost = (TextBox)item.FindControl("txtCost");
                CascadingDropDown cddlDriver = (CascadingDropDown)item.FindControl("cddlDriver");
                int serviceId = Convert.ToInt32(hiddenId.Value);

                ExpenseService service;
                if (serviceId <= 0)
                {
                    service = new ExpenseService();
                }
                else
                {
                    service = Module.ExpenseServiceGetById(serviceId);
                }
                service.CurrentUser = CurrentUser;
                service.Type = Module.CostTypeGetById(Convert.ToInt32(hiddenType.Value));
                service.Name = txtName.Text;
                service.Phone = txtPhone.Text;
                var supplier = Module.AgencyGetById(Convert.ToInt32(ddlSuppliers.SelectedValue));
                if (ddlSuppliers.SelectedIndex > 0)
                {
                    service.Supplier = supplier;
                }
                else
                {
                    service.Supplier = null;
                }

                if (service.Type.Name == "Transport")
                {
                    if (cddlDriver.SelectedIndex > 0)
                    {
                        var driver = Module.ContactGetById(Convert.ToInt32(cddlDriver.SelectedValue));
                        service.Driver = driver;
                        service.Phone = (driver != null && driver.Id > 0) ? driver.Phone : "";
                    }
                    else
                    {
                        service.Driver = null;
                        service.Phone = (supplier != null && supplier.Id > 0) ? supplier.Phone : "";
                    }
                }
                else
                {
                    service.Driver = null;
                }

                service.IsOwnService = !service.Type.IsSupplier;
                service.Cost = Convert.ToDouble(txtCost.Text);
                service.IsRemoved = !item.Visible;
                if (ddlGroups.SelectedIndex >= 0)
                {
                    service.Group = Convert.ToInt32(ddlGroups.SelectedValue);
                }
                list.Add(service);
            }
            return list;
        }
        protected void rptOrganization_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is Organization)
            {
                var organization = (Organization)e.Item.DataItem;
                var liMenu = e.Item.FindControl("liMenu") as HtmlGenericControl;
                if (liMenu != null)
                {
                    if (organization.Id.ToString() == Request.QueryString["orgid"])
                    {
                        liMenu.Attributes.Add("class", "selected");
                    }
                }

                var hplOrganization = e.Item.FindControl("hplOrganization") as HyperLink;
                if (hplOrganization != null)
                {
                    DateTime date = Request.QueryString["Date"] != null ? DateTime.FromOADate(Convert.ToDouble(Request.QueryString["Date"])) : DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    hplOrganization.Text = organization.Name;
                    if (_customerByOrg.ContainsKey(organization.Id))
                    {
                        hplOrganization.Text += string.Format(" ({0})", _customerByOrg[organization.Id]);
                    }
                    hplOrganization.NavigateUrl = string.Format(
                        "BookingReport.aspx?NodeId={0}&SectionId={1}&Date={2}&orgid={3}", Node.Id, Section.Id,
                        date.ToOADate(), organization.Id);
                }

                var rptTrips = e.Item.FindControl("rptTrips") as Repeater;
                if (rptTrips != null)
                {
                    rptTrips.DataSource = Module.TripGetByOrganization(organization);
                    rptTrips.DataBind();
                }
            }
            else
            {
                var liMenu = e.Item.FindControl("liMenu") as HtmlGenericControl;
                if (liMenu != null)
                {
                    if (Request.QueryString["orgid"] == null && Request.QueryString["tripid"] == null)
                    {
                        liMenu.Attributes.Add("class", "selected");
                    }
                }

                var hplOrganization = e.Item.FindControl("hplOrganization") as HyperLink;
                if (hplOrganization != null)
                {
                    DateTime date = Request.QueryString["Date"] != null ? DateTime.FromOADate(Convert.ToDouble(Request.QueryString["Date"])) : DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    hplOrganization.NavigateUrl = string.Format(
                        "BookingReport.aspx?NodeId={0}&SectionId={1}&Date={2}", Node.Id, Section.Id, date.ToOADate());
                }
            }
        }

        protected void btnLockDate_Click(object sender, EventArgs e)
        {
            if (CanLockBooking)
                ListBooking.ForEach(x => { x.LockStatus = "Locked"; BookingReportBLL.BookingSaveOrUpdate(x); });
            ListExpenseService.ForEach(x => { x.LockStatus = "Locked"; BookingReportBLL.ExpenseServiceSaveOrUpdate(x); });
            Response.Redirect(Request.RawUrl);
        }

        protected void btnUnlockDate_Click(object sender, EventArgs e)
        {
            if (CanLockBooking)
                ListBooking.ForEach(x => { x.LockStatus = "Unlocked"; BookingReportBLL.BookingSaveOrUpdate(x); });
            ListExpenseService.ForEach(x => { x.LockStatus = "Unlocked"; BookingReportBLL.ExpenseServiceSaveOrUpdate(x); });
            Response.Redirect(Request.RawUrl);
        }

        protected void txtSearchCode_TextChanged(object sender, EventArgs e)
        {
            BookingSearch();
        }
        private void BookingSearch()
        {
            BookingSearchByCodeOrTACode();
            BookingSearchByDate();
        }
        private void BookingSearchByDate()
        {
            DateTime date = DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string url = string.Format("BookingReport.aspx?NodeId={0}&SectionId={1}&Date={2}", Node.Id, Section.Id,
                                       date.ToOADate());
            PageRedirect(url);
        }
        private void BookingSearchByCodeOrTACode()
        {
            if (String.IsNullOrEmpty(txtSearchCode.Text.Trim())) return;
            var code = txtSearchCode.Text;
            //Phân loại code
            var bookingCodePartern = @"(ATM)[\d]{1,}|^[\d]{1,}";
            var isMatch = Regex.IsMatch(code, bookingCodePartern, RegexOptions.IgnoreCase);
            Booking booking = null;
            string type = "";
            if (isMatch)
            {
                //Nếu đúng mẫu tìm theo booking code
                booking = BookingReportBLL.BookingGetByBookingCode(code);
                type = "bc";
            }
            else
            {
                //Nếu sai thì tìm theo ta code
                booking = BookingReportBLL.BookingGetByTACode(code);
                type = "tac";
            }
            var startDate = new DateTime();
            var foundBookingId = 0;
            if (booking != null)
            {
                startDate = booking.StartDate;
                foundBookingId = booking.Id;
            }
            Response.Redirect(String.Format("BookingReport.aspx?NodeId=1&SectionId=15&Date={0}&searchcode={1}&type={2}&foundbookingid={3}", startDate.ToOADate(), code, type, foundBookingId));
        }
    }
}