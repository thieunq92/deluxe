using CMS.Core.Domain;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.BusinessLogic
{
    //Bussiness logic của trang ProductAnalysis
    public class ProductAnalysisBLL
    {
        public UserOrganizationRepository UserOrganizationRepository { get; set; }
        public SailsTripRepository SailsTripRepository { get; set; }
        public AgencyRepository AgencyRepository { get; set; }
        public CustomerRepository CustomerRepository { get; set; }
        public BookingRepository BookingRepository { get; set; }
        public ExpenseServiceRepository ExpenseServiceRepository { get; set; }
        public SailExpenseRepository SailExpenseRepository { get; set; }

        public ProductAnalysisBLL()
        {
            UserOrganizationRepository = new UserOrganizationRepository();
            SailsTripRepository = new SailsTripRepository();
            AgencyRepository = new AgencyRepository();
            CustomerRepository = new CustomerRepository();
            BookingRepository = new BookingRepository();
            ExpenseServiceRepository = new ExpenseServiceRepository();
            SailExpenseRepository = new SailExpenseRepository();
        }
        /// <summary>
        /// Giải phóng tất cả kết nổi đến cơ sở dữ liệu. 
        /// Phải thực hiện sau khi dùng xong Repository.
        /// </summary>
        public void Dispose()
        {
            if (UserOrganizationRepository != null)
            {
                UserOrganizationRepository.Dispose();
                UserOrganizationRepository = null;
            }
            if (SailsTripRepository != null)
            {
                SailsTripRepository.Dispose();
                SailsTripRepository = null;
            }
            if (AgencyRepository != null)
            {
                AgencyRepository.Dispose();
                AgencyRepository = null;
            }
            if (ExpenseServiceRepository != null)
            {
                ExpenseServiceRepository.Dispose();
                ExpenseServiceRepository = null;
            }
            if (SailExpenseRepository != null)
            {
                SailExpenseRepository.Dispose();
                SailExpenseRepository = null;
            }
        }
        /// <summary>
        /// Lấy tất cả Region(Organization) mà User có liên quan
        /// </summary>
        /// <param name="user">User mà cần lấy Region(Organization)</param>
        /// <returns></returns>
        public IEnumerable<Organization> RegionGetAllByUser(User user)
        {
            return UserOrganizationRepository.RegionGetAllByUser(user);
        }
        /// <summary>
        /// Lấy tất cả các Trip theo Region(Organization(Bắc, Trung, Nam))
        /// </summary>
        /// <param name="region">Region cần lấy. Nếu null trả về tất cả các trip của tất cả các region. </param>
        /// <returns>Trả về danh sách các trip theo Region. Trả về danh sách rỗng nếu không tìm thấy kết quả.</returns>
        public IEnumerable<SailsTrip> TripGetAllByRegion(Organization region)
        {
            return SailsTripRepository.TripGetAllByRegion(region);
        }
        /// <summary>
        /// Lấy Trip theo Id.
        /// </summary>
        /// <param name="tripId">Id của Trip cần lấy.</param>
        /// <returns>Trả về Trip theo Id. Hoặc Trip có Id = 0 nếu không tìm thấy kết quả.</returns>
        public SailsTrip TripGetById(int tripId)
        {
            return SailsTripRepository.GetById(tripId);
        }
        [Obsolete]
        public int CustomerGetNumberOfCustomersInMonth(int month, int year, User user)
        {
            return CustomerRepository.CustomerGetNumberOfCustomersInMonth(month, year, user);
        }
        [Obsolete]
        public int BookingGetNumberOfBookingsInMonth(int month, int year, User user)
        {
            return BookingRepository.BookingGetNumberOfBookingsInMonth(month, year, user);
        }
        [Obsolete]
        public double BookingGetTotalRevenueInMonth(int month, int year, User user)
        {
            return BookingRepository.BookingGetTotalRevenueInMonth(month, year, user);
        }
        [Obsolete]
        public IList<Booking> BookingGetAllInMonth(int month, int year, User user)
        {
            return BookingRepository.BookingGetAllInMonth(month, year, user);
        }
        [Obsolete]
        public IList<ExpenseService> ExpenseServiceGetAllInMonth(int month, int year)
        {
            return ExpenseServiceRepository.ExpenseServiceGetAllInMonth(month, year);
        }
        [Obsolete]
        public object AgencyGetTop10InMonth(int selectedMonth)
        {
            return AgencyRepository.AgencyGetTop10InMonth(selectedMonth, null);
        }
        /// <summary>
        /// Tính tổng tất cả các ExpenseService của Booking trong tháng cụ thể và tìm theo Trip
        /// </summary>
        /// <param name="month">Tháng nào.</param>
        /// <param name="year">Năm nào.</param>
        /// <param name="trip">Trip gì.</param>
        /// <returns>Trả về tổng tất cả chi phí trong tháng và tìm theo Trip</returns>
        public IList<ExpenseService> ExpenseServiceGetAllInMonth(int month, int year, SailsTrip trip)
        {
            return ExpenseServiceRepository.ExpenseServiceGetAllInMonth(month, year, trip);
        }
        /// <summary>
        /// Đếm số Customer của những Booking trong tháng cụ thể và tìm theo Trip
        /// </summary>
        /// <param name="month">Tháng nào.</param>
        /// <param name="year">Năm nào.</param>
        /// <param name="trip">Trip gì.</param>
        /// <returns>Trả về số khách có trong tháng cụ thể và theo từng Trip</returns>
        public int CustomerGetNumberOfCustomersInMonth(int month, int year, SailsTrip trip)
        {
            return CustomerRepository.CustomerGetNumberOfCustomersInMonth(month, year, trip);
        }
        /// <summary>
        /// Tính tổng Revenue(Total) của các Booking có StartDate nằm trong tháng và tìm theo Trip.
        /// </summary>
        /// <param name="month">Tháng nào.</param>
        /// <param name="year">Năm nào.</param>
        /// <param name="trip">Trip gì.</param>
        /// <returns>Trả về tổng Revenue của các Booking.</returns>
        public double BookingGetTotalRevenueInMonth(int month, int year, SailsTrip trip)
        {
            return BookingRepository.BookingGetTotalRevenueInMonth(month, year, trip);
        }
        /// <summary>
        /// Lấy danh sách tất cả Booking theo năm, tháng cụ thể và tìm theo Trip.
        /// </summary>
        /// <param name="month">Tháng nào.</param>
        /// <param name="year">Năm nào.</param>
        /// <param name="trip">Trip gì.</param>
        /// <returns>Trả về danh sách tất cả Booking theo năm, tháng và theo Trip hoặc danh sách rỗng nếu không tìm thấy kết quả.</returns>
        public IList<Booking> BookingGetAllInMonth(int month, int year, SailsTrip trip)
        {
            return BookingRepository.BookingGetAllInMonth(month, year, trip);
        }
        /// <summary>
        /// Lấy danh sách top 10 các Agency theo tháng cụ thể và tìm theo Trip. 
        /// </summary>
        /// <param name="month">Tháng nào.</param>
        /// <param name="trip">Trip gì</param>
        /// <returns>Trả về danh sách tất cả Booking theo tháng và theo Trip hoặc danh sách rỗng nếu không tìm thấy kết quả.</returns>
        public object AgencyGetTop10InMonthAndTrip(int month, SailsTrip trip)
        {
            return AgencyRepository.AgencyGetTop10InMonthAndTrip(month, trip);
        }
    }
}