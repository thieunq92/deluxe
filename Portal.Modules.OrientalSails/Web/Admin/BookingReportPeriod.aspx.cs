using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CMS.Web.Util;
using NHibernate.Criterion;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.UI;
using Portal.Modules.OrientalSails.Web.Util;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class BookingReportPeriod : SailsAdminBase
    {
        #region -- PRIVATE MEMBERS --
        protected Dictionary<DateTime, DayNote> notemap;
        protected Dictionary<int, int> tripmap;
        protected Dictionary<int, int> currentTripMap;
        protected Dictionary<string, int> roomMap; // Số phòng từng loại
        protected Dictionary<string, int> currentRoomMap; // Số phòng đã book

        protected IList _displayTrips;
        protected IList _rooms;

        protected IList DisplayTrips
        {
            get
            {
                if (_displayTrips == null)
                {
                    _displayTrips = Module.TripGetAll(true, UserIdentity);
                }
                return _displayTrips;
            }
        }

        protected IList Rooms // Lấy về danh sách toàn bộ phòng
        {
            get
            {
                if (_rooms == null)
                {
                    _rooms = new ArrayList();
                    IList rooms;
                    if (Cruise != null)
                    {
                        rooms = Module.RoomGetAll(Cruise);
                    }
                    else
                    {
                        rooms = Module.RoomGetAll(true);
                    }
                    roomMap = new Dictionary<string, int>();
                    foreach (Room room in rooms)
                    {
                        if (!roomMap.ContainsKey(string.Format("{0}#{1}", room.RoomClass.Id, room.RoomType.Id)))
                        {
                            roomMap.Add(string.Format("{0}#{1}", room.RoomClass.Id, room.RoomType.Id), 1);
                            _rooms.Add(room);
                        }
                        else
                        {
                            roomMap[string.Format("{0}#{1}", room.RoomClass.Id, room.RoomType.Id)] += 1;
                        }
                    }
                }
                return _rooms;
            }
        }

        private Cruise _cruise;
        protected Cruise Cruise
        {
            get
            {
                if (_cruise == null && Request.QueryString["cruiseid"] != null)
                {
                    _cruise = Module.CruiseGetById(Convert.ToInt32(Request.QueryString["cruiseid"]));
                }
                return _cruise;
            }
        }

        private int _totalAvailable; // Số phòng trống từng ngày (tổng)
        #endregion

        #region -- PAGE EVENTS --
        protected virtual void Page_Load(object sender, EventArgs e)
        {
            //if (!UserIdentity.HasPermission(AccessLevel.Administrator))
            //{
            //    ShowError("You don't have permission to view this info, please go away");
            //    return;
            //}
            Title = Resources.titleBookingPeriod;
            if (!IsPostBack)
            {
                if (Request.QueryString["from"] != null)
                {
                    DateTime from = DateTime.FromOADate(Convert.ToDouble(Request.QueryString["from"]));
                    txtFrom.Text = from.ToString("dd/MM/yyyy");
                }
                if (Request.QueryString["to"] != null)
                {
                    DateTime to = DateTime.FromOADate(Convert.ToDouble(Request.QueryString["to"]));
                    txtTo.Text = to.ToString("dd/MM/yyyy");
                }
                GetDataSource();
                BindBookings();
            }
        }
        #endregion

        public static IList GetDataSource(TextBox textFrom, TextBox textTo)
        {
            DateTime from;
            DateTime to;
            if (string.IsNullOrEmpty(textFrom.Text) || string.IsNullOrEmpty(textTo.Text))
            {
                from = DateTime.Today.AddDays(-DateTime.Today.Day + 1);
                to = from.AddMonths(1).AddDays(-1);
            }
            else
            {
                from = DateTime.ParseExact(textFrom.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                to = DateTime.ParseExact(textTo.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            textFrom.Text = from.ToString("dd/MM/yyyy");
            textTo.Text = to.ToString("dd/MM/yyyy");
            IList list = new ArrayList();
            while (from <= to)
            {
                list.Add(from);
                from = from.AddDays(1);
            }
            return list;
        }

        protected void GetDataSource()
        {
            rptBookingList.DataSource = GetDataSource(txtFrom, txtTo);
            DateTime from;
            DateTime to;
            if (string.IsNullOrEmpty(txtFrom.Text) || string.IsNullOrEmpty(txtTo.Text))
            {
                from = DateTime.Today.AddDays(-DateTime.Today.Day + 1);
                to = from.AddMonths(1).AddDays(-1);
            }
            else
            {
                from = DateTime.ParseExact(txtFrom.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                to = DateTime.ParseExact(txtTo.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            IList daynotes = Module.DayNoteGetByDay(Cruise, from, to);
            notemap = new Dictionary<DateTime, DayNote>();
            foreach (DayNote note in daynotes)
            {
                if (!notemap.ContainsKey(note.Date))
                {
                    notemap.Add(note.Date, note);
                }
            }
        }

        protected void BindBookings()
        {
            rptBookingList.DataBind();
        }

        #region -- CONTROL EVENTS --
        protected virtual void rptBookingList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                Repeater rptTrips = e.Item.FindControl("rptTrips") as Repeater;
                if (rptTrips != null)
                {
                    rptTrips.DataSource = DisplayTrips;
                    rptTrips.DataBind();
                }

                Repeater rptRooms = e.Item.FindControl("rptRooms") as Repeater;
                if (rptRooms != null)
                {
                    rptRooms.DataSource = Rooms;
                    rptRooms.DataBind();
                }

                Repeater rptRoomAvail = e.Item.FindControl("rptRoomAvail") as Repeater;
                if (rptRoomAvail != null)
                {
                    rptRoomAvail.DataSource = Rooms;
                    rptRoomAvail.DataBind();
                }
            }

            #region -- Item --
            if (e.Item.DataItem is DateTime)
            {
                Literal litTr = e.Item.FindControl("litTr") as Literal;
                if (litTr != null)
                {
                    if (e.Item.ItemType == ListItemType.Item)
                    {
                        litTr.Text = @"<tr>";
                    }
                    else
                    {
                        litTr.Text = @"<tr style=""background-color:#E9E9E9"">";
                    }
                }

                DateTime date = (DateTime)e.Item.DataItem;
                Label labelDate = (Label)e.Item.FindControl("labelDate");
                if (labelDate != null)
                {
                    labelDate.Text = date.ToString("dd/MM/yyyy");
                }

                HyperLink hplDate = (HyperLink)e.Item.FindControl("hplDate");
                if (hplDate != null)
                {
                    string cruise = string.Empty;
                    if (Cruise != null)
                    {
                        cruise = "&cruiseid=" + Cruise.Id;
                    }

                    hplDate.Text = date.ToString("dd/MM/yyyy");
                    hplDate.NavigateUrl = string.Format("BookingReport.aspx?NodeId={0}&SectionId={1}&Date={2}{3}", Node.Id,
                                                        Section.Id, date.ToOADate(), cruise);
                }

                #region -- Counting --
                int count;
                // Điều kiện bắt buộc
                ICriterion criterion = Expression.And(Expression.Eq(Booking.DELETED, false),
                                                      Expression.Eq(Booking.STATUS, StatusType.Approved));

                if (Cruise != null)
                {
                    criterion = Expression.And(criterion, Expression.Eq("Cruise", Cruise));
                }

                // Không bao gồm booking transfer
                criterion = Expression.And(criterion, Expression.Not(Expression.Eq("IsTransferred", true)));
                criterion = Module.AddDateExpression(criterion, date);
                IList bookings = Module.BookingGetByCriterion(criterion, null, out count, 0, 0, false, UserIdentity);
                int adult = 0;
                int child = 0;
                int baby = 0;

                #region -- Counting --

                currentTripMap = new Dictionary<int, int>();
                currentRoomMap = new Dictionary<string, int>();
                int avail = 0;
                foreach (Booking booking in bookings)
                {
                    int inBook = 0;
                    // Số người lớn, trẻ em, trẻ sơ sinh
                    adult += booking.Adult;
                    baby += booking.Baby;
                    child += booking.Child;

                    // Biến inbook để lấy tổng số người trong một trip (kể cả trẻ em, trẻ sơ sinh)
                    inBook += booking.Pax + booking.Baby;

                    // Tính số khách của từng trip
                    #region -- Tổng số --
                    if (tripmap == null)
                    {
                        tripmap = new Dictionary<int, int>();
                    }
                    if (!tripmap.ContainsKey(booking.Trip.Id))
                    {
                        tripmap.Add(booking.Trip.Id, inBook);
                    }
                    else
                    {
                        tripmap[booking.Trip.Id] += inBook;
                    }
                    #endregion

                    #region -- Cho ngày hiện tại --
                    if (!currentTripMap.ContainsKey(booking.Trip.Id))
                    {
                        currentTripMap.Add(booking.Trip.Id, inBook);
                    }
                    else
                    {
                        currentTripMap[booking.Trip.Id] += inBook;
                    }
                    #endregion

                    // Tính số phòng chiếm/phòng trống của từng loại                    
                    foreach (BookingRoom room in booking.BookingRooms)
                    {
                        string key = string.Format("{0}#{1}", room.RoomClass.Id, room.RoomType.Id);
                        int add;
                        if (room.RoomType.IsShared)
                        {
                            add = room.VirtualAdult;
                        }
                        else
                        {
                            add = 1;
                        }
                        if (currentRoomMap.ContainsKey(key))
                        {
                            currentRoomMap[key] += add;
                        }
                        else
                        {
                            currentRoomMap.Add(key, add);
                        }
                    }
                }

                #endregion

                #region -- display to repeater --
                Literal litAdult = (Literal)e.Item.FindControl("litAdult");
                Literal litChild = (Literal)e.Item.FindControl("litChild");
                Literal litBaby = (Literal)e.Item.FindControl("litBaby");
                Literal litTotalPax = (Literal)e.Item.FindControl("litTotalPax");
                litAdult.Text = adult.ToString("#");
                litChild.Text = child.ToString("#");
                litBaby.Text = baby.ToString("#");
                litTotalPax.Text = (adult + child + baby).ToString("#");
                #endregion

                #region -- calculate --

                Locked locked = null;
                if (Cruise != null)
                {
                    locked = Module.LockedCheckByDate(Cruise, date);
                    if (locked.Id > 0)
                    {
                        if (litTr != null)
                        {
                            litTr.Text = string.Format(@"<tr style=""background-color: {0}"">", SailsModule.IMPORTANT);
                        }
                    }
                }

                #endregion
                #endregion

                #region -- Lock & unlock --
                LinkButton lbtLock = e.Item.FindControl("lbtLock") as LinkButton;
                if (lbtLock != null)
                {
                    if (locked != null)
                    {
                        if (locked.Id > 0)
                        {
                            lbtLock.Text = "[Unlock]";
                            if (Module.LockedCheckCharter(locked))
                            {
                                lbtLock.Visible = false;
                            }
                        }
                        else
                        {
                            lbtLock.Text = "[Lock]";
                        }
                    }
                    else
                    {
                        lbtLock.Visible = false;
                    }
                }
                #endregion

                #region -- Trips/cabins --
                Repeater rptTrips = e.Item.FindControl("rptTrips") as Repeater;
                if (rptTrips != null)
                {
                    rptTrips.DataSource = DisplayTrips;
                    rptTrips.DataBind();
                }

                _totalAvailable = 0;
                Repeater rptRoomAvail = e.Item.FindControl("rptRoomAvail") as Repeater;
                if (rptRoomAvail != null)
                {
                    rptRoomAvail.DataSource = Rooms;
                    rptRoomAvail.DataBind();
                }

                Literal litTotal = e.Item.FindControl("litTotal") as Literal;
                if (litTotal!=null)
                {
                    litTotal.Text = _totalAvailable.ToString();
                }
                #endregion

                #region -- Note --

                Image imgNote = (Image)e.Item.FindControl("imgNote");
                Literal litNote = (Literal)e.Item.FindControl("litNote");
                if (notemap.ContainsKey(date))
                {
                    imgNote.Visible = true;
                    litNote.Text = notemap[date].Note;
                }
                else
                {
                    imgNote.Visible = false;
                    litNote.Visible = false;
                }
                #endregion
            }
            #endregion
        }

        protected void rptTrips_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is SailsTrip)
            {
                SailsTrip trip = (SailsTrip)e.Item.DataItem;
                Literal litPax = e.Item.FindControl("litPax") as Literal;
                if (litPax != null)
                {
                    if (currentTripMap != null && currentTripMap.ContainsKey(trip.Id))
                    {
                        litPax.Text = currentTripMap[trip.Id].ToString();
                    }
                }
            }
        }

        protected virtual void rptRooms_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is Room)
            {
                Room room = (Room)e.Item.DataItem;
                string key = string.Format("{0}#{1}", room.RoomClass.Id, room.RoomType.Id);
                Literal litRoomName = e.Item.FindControl("litRoomName") as Literal;
                if (litRoomName != null)
                {
                    litRoomName.Text = string.Format("{0} {1}", room.RoomClass.Name, room.RoomType.Name);
                }

                Literal litTotal = e.Item.FindControl("litTotal") as Literal;
                if (litTotal != null)
                {
                    if (currentRoomMap != null && currentRoomMap.ContainsKey(key))
                    {
                        if (room.RoomType.IsShared)
                        {
                            litTotal.Text = (currentRoomMap[key] / room.RoomType.Capacity).ToString();
                        }
                        else
                        {
                            litTotal.Text = (currentRoomMap[key]).ToString();
                        }
                    }
                }

                Literal litAvail = e.Item.FindControl("litAvail") as Literal;
                if (litAvail != null)
                {
                    int avail = roomMap[key];
                    // Nếu có người đặt phòng thì số avail sẽ phải nhỏ hơn
                    if (currentRoomMap != null && currentRoomMap.ContainsKey(key))
                    {
                        if (room.RoomType.IsShared)
                        {
                            avail = (roomMap[key] - currentRoomMap[key] / room.RoomType.Capacity);
                        }
                        else
                        {
                            avail = (roomMap[key] - currentRoomMap[key]);
                        }
                    }
                    if (avail < 0)
                    {
                        ShowError(Resources.errorBookingPeriod);
                        HtmlTableCell tdAvail = e.Item.FindControl("tdAvail") as HtmlTableCell;
                        if (tdAvail != null)
                        {
                            tdAvail.Attributes.CssStyle.Add("background-color", SailsModule.IMPORTANT);
                        }
                    }
                    else if (avail == 0)
                    {
                        HtmlTableCell tdAvail = e.Item.FindControl("tdAvail") as HtmlTableCell;
                        if (tdAvail != null)
                        {
                            tdAvail.Attributes.CssStyle.Add("background-color", SailsModule.WARNING);
                        }
                    }
                    _totalAvailable += avail;
                    litAvail.Text = avail.ToString();
                }
            }
        }

        protected void rptBookingList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (Cruise == null)
            {
                return;
            }

            switch (e.CommandName.ToLower())
            {
                case "lock":
                    HyperLink labelDate = e.Item.FindControl("hplDate") as HyperLink;
                    if (labelDate != null)
                    {
                        // Lấy date dòng vừa click
                        DateTime date = DateTime.ParseExact(labelDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                        // Check xem nếu có lock chưa
                        Locked locked = Module.LockedCheckByDate(Cruise, date);

                        if (locked.Id > 0)
                        {
                            Module.Delete(locked);
                        }
                        else
                        {
                            Module.SaveOrUpdate(locked);
                        }
                    }
                    GetDataSource();
                    BindBookings();
                    break;
            }
        }

        protected void btnDisplay_Click(object sender, EventArgs e)
        {
            DateTime from = DateTime.ParseExact(txtFrom.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime to = DateTime.ParseExact(txtTo.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            PageRedirect(string.Format("BookingReportPeriod.aspx?NodeId={0}&SectionId={1}&from={2}&to={3}", Node.Id, Section.Id, from.ToOADate(), to.ToOADate()));
            //GetDataSource();
            //BindBookings();
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            DataTable data = new DataTable();
            DataColumn DATE = data.Columns.Add("Date", typeof(string));
            DataColumn ADULT = data.Columns.Add("Adult", typeof(string));
            DataColumn CHILD = data.Columns.Add("Child", typeof(string));
            DataColumn BABY = data.Columns.Add("Baby", typeof(string));
            DataColumn PAX = data.Columns.Add("Total pax", typeof(string));
            DataColumn TRIP1 = data.Columns.Add("Trip 1", typeof(string));
            DataColumn TRIP2 = data.Columns.Add("Trip 2", typeof(string));
            DataColumn DOUBLE_OCCUPIED = data.Columns.Add("Double occupied", typeof(string));
            DataColumn DOUBLE_AVAILABLE = data.Columns.Add("Double available", typeof(string));
            DataColumn TWIN_OCCUPIED = data.Columns.Add("Twin occupied", typeof(string));
            DataColumn TWIN_AVAILABLE = data.Columns.Add("Twin available", typeof(string));

            foreach (RepeaterItem item in rptBookingList.Items)
            {
                Label labelDate = (Label)item.FindControl("labelDate");
                Literal litAdult = (Literal)item.FindControl("litAdult");
                Literal litChild = (Literal)item.FindControl("litChild");
                Literal litBaby = (Literal)item.FindControl("litBaby");
                Literal litTotalPax = (Literal)item.FindControl("litTotalPax");
                Literal litTrip1 = (Literal)item.FindControl("litTrip1");
                Literal litTrip2 = (Literal)item.FindControl("litTrip2");
                Literal litDouble = (Literal)item.FindControl("litDouble");
                Literal litDoubleAvaiable = (Literal)item.FindControl("litDoubleAvaiable");
                Literal litTwin = (Literal)item.FindControl("litTwin");
                Literal litTwinAvaiable = (Literal)item.FindControl("litTwinAvaiable");

                DataRow row = data.NewRow();
                row[DATE] = labelDate.Text;
                row[ADULT] = litAdult.Text;
                row[CHILD] = litChild.Text;
                row[BABY] = litBaby.Text;
                row[PAX] = litTotalPax.Text;
                row[TRIP1] = litTrip1.Text;
                row[TRIP2] = litTrip2.Text;
                row[DOUBLE_OCCUPIED] = litDouble.Text;
                row[DOUBLE_AVAILABLE] = litDoubleAvaiable.Text;
                row[TWIN_OCCUPIED] = litTwin.Text;
                row[TWIN_AVAILABLE] = litTwinAvaiable.Text;
                data.Rows.Add(row);
            }

            ExcelExport excel = new ExcelExport("Web");
            excel.ExportDetails(data, ExcelExport.ExportFormat.Excel, "booking.xls");
        }
        #endregion

        protected void rptCruises_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is Cruise)
            {
                Cruise cruise = (Cruise)e.Item.DataItem;

                HtmlGenericControl liMenu = e.Item.FindControl("liMenu") as HtmlGenericControl;
                if (liMenu != null)
                {
                    if (cruise.Id.ToString() == Request.QueryString["cruiseid"])
                    {
                        liMenu.Attributes.Add("class", "selected");
                    }
                }

                HyperLink hplCruises = e.Item.FindControl("hplCruises") as HyperLink;
                if (hplCruises != null)
                {
                    DateTime from = DateTime.ParseExact(txtFrom.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime to = DateTime.ParseExact(txtTo.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    hplCruises.Text = cruise.Name;
                    hplCruises.NavigateUrl =
                        string.Format("BookingReportPeriod.aspx?NodeId={0}&SectionId={1}&from={2}&to={3}&cruiseid={4}", Node.Id, Section.Id, from.ToOADate(), to.ToOADate(), cruise.Id);
                }
            }
            else
            {
                HtmlGenericControl liMenu = e.Item.FindControl("liMenu") as HtmlGenericControl;
                if (liMenu != null)
                {
                    if (Request.QueryString["cruiseid"] == null)
                    {
                        liMenu.Attributes.Add("class", "selected");
                    }
                }
                HyperLink hplCruises = e.Item.FindControl("hplCruises") as HyperLink;
                if (hplCruises != null)
                {
                    DateTime from = DateTime.ParseExact(txtFrom.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime to = DateTime.ParseExact(txtTo.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    hplCruises.NavigateUrl =
                        string.Format("BookingReportPeriodAll.aspx?NodeId={0}&SectionId={1}&from={2}&to={3}", Node.Id,
                                      Section.Id, from.ToOADate(), to.ToOADate());
                }
            }
        }
    }
}
