using NHibernate;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Repository;
using Portal.Modules.OrientalSails.Web.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.BusinessLogic
{
    /// <summary>
    /// Bussines logic của trang BookingReport
    /// </summary>
    public class BookingReportBLL
    {
        public BookingRepository BookingRepository { get; set; }
        public ExpenseServiceRepository ExpenseServiceRepository { get; set; }
        public ExpenseHistoryRepository ExpenseHistoryRepository { get; set; }
        public BookingReportBLL()
        {
            BookingRepository = new BookingRepository();
            ExpenseServiceRepository = new ExpenseServiceRepository();
            ExpenseHistoryRepository = new ExpenseHistoryRepository();
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
            if (ExpenseServiceRepository != null)
            {
                ExpenseServiceRepository.Dispose();
                ExpenseServiceRepository = null;
            }
            if (ExpenseHistoryRepository != null)
            {
                ExpenseHistoryRepository.Dispose();
                ExpenseHistoryRepository = null;
            }
        }
        /// <summary>
        /// Lấy tất cả Booking theo ngày khởi hành và trạng thái booking
        /// </summary>
        /// <param name="date">Ngày khởi hành của booking</param>
        /// <param name="statusType">Trạng thái của booking (Approved, Cancelled, Pending)</param>
        /// <returns></returns>
        public IQueryOver<Booking, Booking> BookingGetAllByCriterion(DateTime date, params StatusType[] statusType)
        {
            return BookingRepository.BookingGetAllByCriterion(date, statusType);
        }

        /// <summary>
        /// Lưu hoặc cập nhật Booking
        /// </summary>
        /// <param name="booking">Booking cần lưu hoặc cập nhật</param>
        public void BookingSaveOrUpdate(Booking booking)
        {
            BookingRepository.SaveOrUpdate(booking);
        }

        /// <summary>
        /// Lấy tất cả ExpenseService theo ngày tạo ExpenseService
        /// </summary>
        /// <param name="date">Ngày tạo ExpenseService </param>
        /// <returns></returns>
        public IQueryOver<ExpenseService, ExpenseService> ExpenseServiceGetAllByCriterion(DateTime date)
        {
            return ExpenseServiceRepository.ExpenseServiceGetAllByCriterion(date);
        }

        /// <summary>
        /// Lưu hoặc cập nhật ExpenseService
        /// </summary>
        /// <param name="expenseService">ExpenseService cần lưu hoặc cập nhật</param>
        public void ExpenseServiceSaveOrUpdate(ExpenseService expenseService)
        {
            ExpenseServiceRepository.SaveOrUpdate(expenseService);
        }

        /// <summary>
        /// Lấy Booking bằng mã booking
        /// </summary>
        /// <param name="code">Mã booking cần lấy ATM..... </param>
        /// <returns></returns>
        public Booking BookingGetByBookingCode(string code)
        {
            return BookingRepository.BookingGetByBookingCode(code);
        }

        /// <summary>
        /// Lấy Booking bằng TAcode
        /// </summary>
        /// <param name="code">TA code</param>
        /// <returns></returns>
        public Booking BookingGetByTACode(string code)
        {
            return BookingRepository.BookingGetByTACode(code);
        }

        /// <summary>
        /// Lưu hoặc cập nhật ExpenseHistory
        /// </summary>
        /// <param name="expenseHistory">ExpenseHistory cần lưu hoặc cập nhật</param>
        public void ExpenseHistorySaveOrUpdate(ExpenseHistory expenseHistory)
        {
            ExpenseHistoryRepository.SaveOrUpdate(expenseHistory);
        }
    }
}