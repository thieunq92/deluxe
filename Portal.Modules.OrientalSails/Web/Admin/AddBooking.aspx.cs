using System;
using System.Collections;
using System.Globalization;
using System.Web.UI.WebControls;
using CMS.Core.Domain;
using log4net;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.UI;
using Portal.Modules.OrientalSails.Web.Util;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class AddBooking : SailsAdminBase
    {
        #region -- PRIVATE MEMBERS --
        /// <summary>
        /// Ngày khởi hành của booking lấy từ dữ liệu vào
        /// </summary>
        private DateTime? _date;

        /// <summary>
        /// Hành trình sẽ book
        /// </summary>
        private SailsTrip _trip;

        /// <summary>
        /// Danh sách chính sách giá áp dụng cho đại lý hiện tại
        /// </summary>
        private IList _policies;

        private readonly ILog _logger = LogManager.GetLogger(typeof(AddBooking));

        /// <summary>
        /// Lấy hành trình từ hộp chọn của người dùng
        /// </summary>
        protected SailsTrip Trip
        {
            get
            {
                if (_trip == null)
                {
                    _trip = Module.TripGetById(Convert.ToInt32(cddlTrips.SelectedValue));
                }
                return _trip;
            }
        }
        /// <summary>
        /// Lấy ngày khởi hành từ hộp thoại của người dùng
        /// </summary>
        protected DateTime? Date
        {
            get
            {
                if (_date == null)
                {
                    try
                    {
                        _date = DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        return null;
                    }
                }
                return _date;
            }
        }

        private DateTime? _endDate;
        /// <summary>
        /// Lấy ngày khởi hành từ hộp thoại của người dùng
        /// </summary>
        protected DateTime? EndDate
        {
            get
            {
                if (_endDate == null)
                {
                    try
                    {
                        _endDate = DateTime.ParseExact(txtEndDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        return null;
                    }
                }
                return _endDate;
            }
        }
        /// <summary>
        /// Tất cả các hành trình
        /// </summary>
        private IList _trips;
        #endregion

        #region -- PAGE EVENTS --
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = Resources.textAddBooking;
            plhTrip.Visible = TripBased;
            plhEndDate.Visible = !plhTrip.Visible;
            // Lấy tất cả các hành trình để lọc ra các hành trình có nhiều option, phục vụ cho việc ẩn/hiện hộp chọn option
            _trips = Module.TripGetAll(false, UserIdentity);
            string visibleIds = string.Empty;
            foreach (SailsTrip trip in _trips)
            {
                if (trip.NumberOfOptions == 2)
                {
                    visibleIds += "#" + trip.Id + "#";
                }
            }
            cddlTrips.Attributes.Add("onChange", string.Format("ddltype_changed('{0}','{1}','{2}')", cddlTrips.ClientID, ddlOptions.ClientID, visibleIds));
            if (!IsPostBack)
            {
                for (int ii = 0; ii <= 15; ii++)
                {
                    if (ii == 0)
                    {
                        ddlAdult.Items.Add(new ListItem("--- Adult ----", 0.ToString()));
                        ddlChild.Items.Add(new ListItem("--- Child ----", 0.ToString()));
                        ddlBaby.Items.Add(new ListItem("--- Baby ----", 0.ToString()));
                        continue;
                    }
                    ddlAdult.Items.Add(ii.ToString());
                    ddlChild.Items.Add(ii.ToString());
                    ddlBaby.Items.Add(ii.ToString());
                }
                BindTrips();
                if (DetailService)
                {
                    BindServices();
                }
                else
                {
                    rptExtraServices.Visible = false;
                }
            }
        }
        #endregion

        #region -- PRIVATE METHODS --
        /// <summary>
        /// Load dữ liệu hành trình vào hộp chọn
        /// </summary>
        protected void BindTrips()
        {
            ddlOrgs.DataSource = Module.OrganizationGetAllRoot();
            ddlOrgs.DataTextField = "Name";
            ddlOrgs.DataValueField = "Id";
            ddlOrgs.DataBind();

            var list = Module.OrganizationGetByUser(UserIdentity);
            if (list.Count == 1)
            {
                ddlOrgs.SelectedValue = ((UserOrganization)list[0]).Organization.Id.ToString();
            }

            cddlTrips.DataSource = Module.TripGetAll(false, UserIdentity);
            cddlTrips.DataTextField = "Name";
            cddlTrips.DataValueField = "Id";
            cddlTrips.DataParentField = "OrgId";
            cddlTrips.ParentClientID = ddlOrgs.ClientID;
            cddlTrips.DataBind();

            //ddlTrips.DataSource = _trips;// Danh sách trip luôn được get về trước khi gọi tới hàm bind trips
            //ddlTrips.DataTextField = "Name";
            //ddlTrips.DataValueField = "Id";
            //ddlTrips.DataBind();
        }

        /// <summary>
        /// Load các dịch vụ gia tăng vào danh sách hộp đánh dấu
        /// </summary>
        protected void BindServices()
        {
            rptExtraServices.DataSource = Module.ExtraOptionGetBooking();
            rptExtraServices.DataBind();
        }

        #endregion

        #region -- CONTROL EVENTS --
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra thêm về phòng và thông tin đối tác
                if (!AllowNoAgency && agencySelector.SelectedAgency == null)
                {
                    ShowError("Chưa có thông tin đối tác");
                    return;
                }

                if (!Date.HasValue)
                {
                    ShowError("Phải có ngày khởi hành");
                    return;
                }

                //2. Lưu thông tin phòng như thế nào
                // Dùng vòng lặp lưu thông tin đơn thuần, không có giá trị đi kèm nào cả

                // Phải lưu thông tin booking trước
                #region -- Booking --
                Booking booking = new Booking();
                if (agencySelector.SelectedAgency != null)
                {
                    booking.Agency = agencySelector.SelectedAgency;

                    booking.PickupAddress = string.Format("Address: {0}, Tel: {1}", booking.Agency.Address, booking.Agency.Phone);
                }
                booking.CreatedBy = Page.User.Identity as User;
                booking.CreatedDate = DateTime.Now;
                booking.ModifiedDate = DateTime.Now;
                booking.Partner = null;
                booking.Sale = booking.CreatedBy;
                booking.StartDate = Date.Value;
                booking.IsVND = true;
                booking.IsCommissionVND = true;
                booking.IsGuideCollectVND = true;
                booking.IsDriverCollectVND = true;
                booking.IsCancelPayVND = true;
                if (ApprovedDefault)
                {
                    booking.Status = StatusType.Approved;
                }
                else
                {
                    booking.Status = StatusType.Pending;
                }
                if (TripBased)
                {
                    booking.Trip = Module.TripGetById(Convert.ToInt32(cddlTrips.SelectedValue));
                }
                else
                {
                    booking.Trip = null;
                }
                booking.Total = 0;

                // Xác định custom booking id
                if (UseCustomBookingId)
                {
                    int maxId = Module.BookingGenerateCustomId(booking.StartDate);
                    booking.CustomBookingId = maxId;
                }
                Module.Save(booking, UserIdentity);

                #region -- Track thêm mới --
                BookingTrack track = new BookingTrack();
                track.Booking = booking;
                track.ModifiedDate = DateTime.Now;
                track.User = UserIdentity;
                Module.SaveOrUpdate(track);

                BookingChanged change = new BookingChanged();
                change.Action = BookingAction.Created;
                change.Parameter = string.Format("{0}", booking.Total);
                change.Track = track;
                Module.SaveOrUpdate(change);
                #endregion

                #region -- Thông tin dịch vụ đi kèm --
                foreach (RepeaterItem serviceItem in rptExtraServices.Items)
                {
                    //HiddenField hiddenValue = (HiddenField)serviceItem.FindControl("hiddenValue");
                    HiddenField hiddenId = (HiddenField)serviceItem.FindControl("hiddenId");
                    CheckBox chkService = (CheckBox)serviceItem.FindControl("chkService");
                    if (chkService.Checked)
                    {
                        ExtraOption service = Module.ExtraOptionGetById(Convert.ToInt32(hiddenId.Value));
                        BookingExtra extra = new BookingExtra();
                        extra.Booking = booking;
                        extra.ExtraOption = service;
                        Module.Save(extra);
                    }
                }
                #endregion

                #endregion

                // Tạo danh sách khách theo booking
                int adult = 0;
                int child = 0;
                int baby = 0;

                int totalAdult = ddlAdult.SelectedIndex;
                int totalChild = ddlChild.SelectedIndex;
                int totalBaby = ddlBaby.SelectedIndex;

                while (adult < totalAdult)
                {
                    Customer customer = new Customer();
                    customer.Type = CustomerType.Adult;
                    customer.Booking = booking;
                    Module.SaveOrUpdate(customer);
                    adult++;
                }

                while (child < totalChild)
                {
                    Customer customer = new Customer();
                    customer.Type = CustomerType.Children;
                    customer.Booking = booking;
                    Module.SaveOrUpdate(customer);
                    child++;
                }

                while (baby < totalBaby)
                {
                    Customer customer = new Customer();
                    customer.Type = CustomerType.Baby;
                    customer.Booking = booking;
                    Module.SaveOrUpdate(customer);
                    baby++;
                }

                PageRedirect(string.Format("BookingView.aspx?NodeId={0}&SectionId={1}&BookingId={2}&Notify=0", Node.Id, Section.Id, booking.Id));
            }
            catch (Exception ex)
            {
                ShowError("Error :" + ex.Message);
                _logger.Error(ex.Message, ex);
            }
        }

        protected void rptExtraServices_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is ExtraOption)
            {
                CheckBox chkService = (CheckBox)e.Item.FindControl("chkService");
                chkService.Checked = ((ExtraOption)e.Item.DataItem).IsIncluded; // Mặc định là chọn dịch vụ nếu đã được bao gồm trong giá phòng
            }
        }
        #endregion
    }
}