using CMS.Core.Domain;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Portal.Modules.OrientalSails.Web.Admin;

namespace Portal.Modules.OrientalSails.BusinessLogic
{
    /// <summary>
    /// Bussiness logic của trang DashBoard
    /// </summary>
    public class DashBoardBLL
    {
        public BookingRepository BookingRepository { get; set; }
        public CustomerRepository CustomerRepository { get; set; }
        public ActivityRepository ActivityRepository { get; set; }
        public AgencyRepository AgencyRepository { get; set; }
        public AgencyContactRepository AgencyContactRepository { get; set; }
        public BookingHistoryRepository BookingHistoryRepository {get;set;}
        public GoldenDayRepository GoldenDayRepository { get; set; }
        public ExpenseServiceRepository ExpenseServiceRepository { get; set; }
        public TripCode_NoteRepository TripCode_NoteRepository { get; set; }
        public RoleRepository RoleRepository { get; set; }
        public TripCode_UserRepository TripCode_UserRepository { get; set; }
        public DashBoardBLL() {
            BookingRepository = new BookingRepository();
            CustomerRepository = new CustomerRepository();
            ActivityRepository = new ActivityRepository();
            AgencyRepository = new AgencyRepository();
            AgencyContactRepository = new AgencyContactRepository();
            BookingHistoryRepository = new BookingHistoryRepository();
            GoldenDayRepository = new GoldenDayRepository();
            ExpenseServiceRepository = new ExpenseServiceRepository();
            TripCode_NoteRepository = new TripCode_NoteRepository();
            RoleRepository = new RoleRepository();
            TripCode_UserRepository = new TripCode_UserRepository();
        }
        /// <summary>
        /// Giải phóng tất cả kết nổi đến cơ sở dữ liệu. 
        /// Phải thực hiện sau khi dùng xong Repository.
        /// </summary>
        public void Dispose()
        {
            if (BookingRepository != null)
            {
                BookingRepository.Dispose();
                BookingRepository = null;
            }
            if (CustomerRepository != null)
            {
                CustomerRepository.Dispose();
                CustomerRepository = null;
            }
            if (ActivityRepository != null)
            {
                ActivityRepository.Dispose();
                ActivityRepository = null;
            }
            if (AgencyRepository != null)
            {
                AgencyRepository.Dispose();
                AgencyRepository = null;
            }
            if (AgencyContactRepository != null)
            {
                AgencyContactRepository.Dispose();
                AgencyContactRepository = null;
            }
            if(BookingHistoryRepository != null){
                BookingHistoryRepository.Dispose();
                BookingHistoryRepository = null;
            }
            if (GoldenDayRepository != null)
            {
                GoldenDayRepository.Dispose();
                GoldenDayRepository = null;
            }
            if (ExpenseServiceRepository != null)
            {
                ExpenseServiceRepository.Dispose();
                ExpenseServiceRepository = null;
            }
            if (TripCode_NoteRepository != null)
            {
                TripCode_NoteRepository.Dispose();
                TripCode_NoteRepository = null;
            }
            if (RoleRepository != null)
            {
                RoleRepository.Dispose();
                RoleRepository = null;
            }

            if (TripCode_UserRepository != null)
            {
                TripCode_UserRepository.Dispose();
                TripCode_UserRepository = null;
            }
        }
        /// <summary>
        /// Lấy tất cả Booking được book ngày hôm nay theo từng sales.
        /// </summary>
        /// <param name="user">Tài khoản của sales. Nếu null trả về tất cả booking của tất cả sales.</param>
        /// <returns>Trả về tất cả booking. Trả về danh sách rỗng nếu không có kết quả.</returns>
        public IEnumerable<Booking> BookingGetAllTodayBookings(User user)
        {
            return BookingRepository.BookingGetAllTodayBookings(user);
        }
        /// <summary>
        /// Đếm số khách có trong một tháng cụ thể theo từng sales.
        /// </summary>
        /// <param name="month">Tháng nào.</param>
        /// <param name="year">Năm nào.</param>
        /// <param name="user">Tài khoản của sales. Nếu null trả về số khách của tất cả sales trong tháng.</param>
        /// <returns>Trả về số khách trong tháng.</returns>
        public int CustomerGetNumberOfCustomersInMonth(int month, int year, User user)
        {
            return CustomerRepository.CustomerGetNumberOfCustomersInMonth(month, year, user);
        }
        /// <summary>
        /// Đếm số booking có trong một tháng cụ thể theo từng sales.
        /// </summary>
        /// <param name="month">Tháng nào.</param>
        /// <param name="year">Năm nào.</param>
        /// <param name="user">Tài khoản của sales. Nếu null trả về số booking của tất cả sales trong tháng.</param>
        /// <returns>Tổng số booking trong tháng</returns>
        public int BookingGetNumberOfBookingsInMonth(int month, int year, User user)
        {
            return BookingRepository.BookingGetNumberOfBookingsInMonth(month, year, user);
        }

        /// <summary>
        /// Tính tổng Revenue(property Total) của tất cả booking trong tháng cụ thể theo từng sales.
        /// </summary>
        /// <param name="month">Tháng nào.</param>
        /// <param name="year">Năm nào.</param>
        /// <param name="user">Tài khoản của sales. Nếu null trả về số Booking của tất cả sales trong tháng.</param>
        /// <returns>Tổng Revenue của các booking trong tháng.</returns>
        public double BookingGetTotalRevenueInMonth(int month, int year, User user)
        {
            return BookingRepository.BookingGetTotalRevenueInMonth(month, year, user);
        }
        /// <summary>
        /// Lấy tất cả Activity(Meeting) trong tháng cụ thể theo sales.
        /// </summary>
        /// <param name="month">Tháng nào.</param>
        /// <param name="year">Năm nào.</param>
        /// <param name="user">Tài khoản của sales. Nếu null trả về tất cả Activity của tất cả sales trong tháng.</param>
        /// <returns>Activity trong tháng. Trả về danh sách rỗng nếu không có kết quả.</returns>
        public IEnumerable<Activity> ActivityGetAllActivityInMonth(int month, int year, User user)
        {
            return ActivityRepository.ActivityGetAllActivityInMonth(month, year, user);
        }
        /// <summary>
        /// Lấy tất cả booking được tạo mới trong ngày hôm nay theo từng sales.
        /// </summary>
        /// <param name="user">Tài khoản của sales. Nếu null trả về tất cả Booking của tất cả sales trong tháng.</param>
        /// <returns>Tất cả booking được tạo mới. Trả về danh sách rỗng nếu không có kết quả.</returns>
        public IEnumerable<Booking> BookingGetAllNewBookings(User user)
        {
            return BookingRepository.BookingGetAllNewBookings(user);
        }
        /// <summary>
        /// Lấy top10 Agency trong tháng hiện tại theo từng sales.
        /// </summary>
        /// <param name="user">Tài khoản của sales. Nếu null trả về top10 Agency của tất cả các sales.</param>
        /// <returns>Top10 Agency trong tháng hiện tại.</returns>
        public object AgencyGetTop10(User user)
        {
            return AgencyRepository.AgencyGetTop10(user);
        }
        /// <summary>
        /// Lấy tất cả Activity(Meeting) trong khoảng thời gian theo sales.
        /// </summary>
        /// <param name="user">Tài khoản của sales. Nếu null trả về Activity của tất cả các sales.</param>
        /// <param name="from">Từ ngày bao nhiêu.</param>
        /// <param name="to">Đến ngày bao nhiêu.</param>
        /// <returns>Trả về Activity trong khoảng thời gian. Trả về danh sách rỗng nếu không tìm thấy kết quả.</returns>
        public IEnumerable<Activity> ActivityGetAllRecentMeetingsInDateRange(User user, DateTime from, DateTime to)
        {
            return ActivityRepository.ActivityGetAllRecentMeetingsInDateRange(user, from, to);
        }
        /// <summary>
        /// Tìm Agency theo Id.
        /// </summary>
        /// <param name="agencyId">Id của Agency.</param>
        /// <returns>Trả về Agency. Trả về một Agency với Id = 0 nếu không tìm thấy kết quả.</returns>
        public Agency AgencyGetById(int agencyId)
        {
            return AgencyRepository.GetById(agencyId);
        }
        /// <summary>
        /// Lấy AgencyContact theo Id.
        /// </summary>
        /// <param name="agencyContactId">Id của AgencyContact cần lấy.</param>
        /// <returns>Trả về Agency theo Id. Trả về Agency có Id = 0 nếu không tìm thấy kết quả.</returns>
        public AgencyContact AgencyContactGetById(int agencyContactId)
        {
            return AgencyContactRepository.GetById(agencyContactId);
        }
        /// <summary>
        /// Lấy tất cả các Agency mà sales chưa có Meeting hoặc không có Meeting sau 2 tháng kể từ ngày hiện tại. 
        /// </summary>
        /// <param name="user">Tài khoản của sales. Nếu null trả về Agency của tất cả các sales.</param>
        /// <returns>
        /// Trả về tất cả Agency mà sales chưa có Meeting không có Meeting sau 2 tháng kể từ ngày hiện tại.
        /// Trả về danh sách rỗng nều không tìm thấy kết quả.
        /// </returns>
        public object AgencyGetAllAgenciesNotVisitedInLast2Month(User user)
        {
            return AgencyRepository.AgencyGetAllAgenciesNotVisitedInLast2Month(user);
        }
        /// <summary>
        /// Lấy tất cả các Agency không gửi Booking sau 3 tháng kể từ ngày hiện tại.
        /// </summary>
        /// <param name="user">Tài khoản của sales. Nếu null trả về Agency của tất cả các sales.</param>
        /// <returns>
        /// Trả về tất cả các Agency không gửi Booking sau 3 tháng kể từ ngày hiện tại.
        /// Trả về danh sách rỗng nều không tìm thấy kết quả.
        /// </returns>
        public object AgencyGetAllAgenciesSendNoBookingsLast3Month(User user)
        {
            return AgencyRepository.AgencyGetAllAgenciesSendNoBookingsLast3Month(user);
        }
        /// <summary>
        /// Lấy tất cả BookingHistory của Booking có trạng thái Approved hay Cancelled.
        /// </summary>
        /// <param name="user">Tài khoản của sales. Nếu null trả về Agency của tất cả các sales.</param>
        /// <returns>
        /// Trả về danh sách BookingHistory của Booking có trạng thái Approved hay Cancelled.
        /// Trả về danh sách rỗng nếu không tìm thấy kết quả.
        /// </returns>
        public IEnumerable<BookingHistory> BookingHistoryGetAllConfirmedAndPending(User user)
        {
            return BookingHistoryRepository.BookingHistoryGetAllConfirmedAndPending(user);
        }
        /// <summary>
        /// Lưu hoặc cập nhật Activity(Meeting).
        /// </summary>
        /// <param name="activity">Activity cần lưu hoặc cập nhật.</param>
        public void ActivitySaveOrUpdate(Activity activity)
        {
            ActivityRepository.SaveOrUpdate(activity);
        }
        /// <summary>
        /// Lấy Activity(Meeting) theo Id.
        /// </summary>
        /// <param name="activityId">Id của Activity cần lấy.</param>
        /// <returns>Trả về Activity theo Id hoặc trả về Activity có Id = 0 nếu không tìm thấy kết quả.</returns>
        public Activity ActivityGetById(int activityId)
        {
            return ActivityRepository.GetById(activityId);
        }
        /// <summary>
        /// Lấy tất cả AgencyContact theo Id của Agency.
        /// </summary>
        /// <param name="agencyId">Id của Agency.</param>
        /// <returns>Tất cả AgencyContact hoặc danh sách rỗng nếu không tìm thấy kết quả.</returns>
        public IEnumerable<AgencyContact> AgencyContactGetAllByAgencyId(int agencyId)
        {
            return AgencyContactRepository.AgencyContactGetAllByAgencyId(agencyId);
        }

        public IEnumerable<GoldenDay> GoldenDayGetAllByDateRange(DateTime from, DateTime to)
        {
            return GoldenDayRepository.GoldenDayGetAllByDateRange(from, to);
        }

        public IList<ExpenseService> ExpenseServiceGetAllTodayByTrip(SailsTrip trip)
        {
            return ExpenseServiceRepository.ExpenseServiceGetAllTodayByTrip(trip);
        }

        public object BookingGetAllByEventCodes(List<Web.Admin.EventCode> tripCodes)
        {
            return BookingRepository.BookingGetAllByEventCodes(tripCodes);
        }

        public object CustomerGetCountByBookings(List<Booking> bookings)
        {
            return CustomerRepository.CustomerGetCountByBookings(bookings);
        }

        public void TripCode_NoteSaveOrUpdate(TripCode_Note tripCode_Note)
        {
            TripCode_NoteRepository.SaveOrUpdate(tripCode_Note);
        }
        public List<TripCode_Note> TripCode_NoteGetAllByTripCode(string tripCode)
        {
            return TripCode_NoteRepository.TripCode_NoteGetAllByTripCode(tripCode);
        }

        public Role RoleGetById(int roleId)
        {
            return RoleRepository.GetById(roleId);
        }

        public void TripCode_UserSaveOrUpdate(TripCode_User tripCode_User)
        {
            TripCode_UserRepository.SaveOrUpdate(tripCode_User);
        }

        public TripCode_User TripCode_UserGetByTripCodeAndUser(string tripCode, User currentUser)
        {
            return TripCode_UserRepository.TripCode_UserGetByTripCodeAndUser(tripCode, currentUser);
        }

        public int NoteGetCountOfNewNotes(string tripCode, User currentUser)
        {
            var tripCode_User = TripCode_UserRepository.TripCode_UserGetByTripCodeAndUser(tripCode, currentUser);
            DateTime? lastCheck = null;
            if (tripCode_User != null && tripCode_User.Id > 0) lastCheck = tripCode_User.LastCheck;
            else return 0;
            return TripCode_NoteRepository.TripCode_NoteGetCountOfNewNotes(tripCode, lastCheck);
        }
    }
}