using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.BusinessLogic
{
    /// <summary>
    /// Bussiness logic cho trang BookingHistories
    /// </summary>
    public class BookingHistoriesBLL
    {
        public BookingHistoryRepository BookingHistoryRepository { get; set; }
        public BookingHistoriesBLL()
        {
            BookingHistoryRepository = new BookingHistoryRepository();
        }
        /// <summary>
        /// Giải phóng tất cả kết nổi đến cơ sở dữ liệu. 
        /// Phải thực hiện sau khi dùng xong Repository.
        /// </summary>
        public void Dispose()
        {
            if (BookingHistoryRepository != null)
            {
                BookingHistoryRepository.Dispose();
                BookingHistoryRepository = null;
            }
        }
        /// <summary>
        /// Lấy tất cả BookingHistory theo Booking
        /// </summary>
        /// <param name="booking">Booking cần lấy lịch sử</param>
        /// <returns></returns>
        public IEnumerable<BookingHistory> BookingHistoryGetAllByBooking(Booking booking)
        {
            return BookingHistoryRepository.BookingHistoryGetAllByBooking(booking);
        }
    }
}