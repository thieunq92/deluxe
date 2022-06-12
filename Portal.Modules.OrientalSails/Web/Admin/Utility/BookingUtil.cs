using Portal.Modules.OrientalSails.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.Web.Admin.Utility
{
    public class BookingUtil
    {
        public static double GetTotal(Booking booking)
        {
            var total = 0.0;
            if (!booking.IsVND) { total = booking.Total * 23000; }
            if (booking.IsVND) { total = booking.Total; } 
            return total;
        }
        public static string GetTotalAsString(Booking booking)
        {
            return NumberUtil.FormatMoney(GetTotal(booking));
        }
        public static string GetTotalOfBookings(IEnumerable<Booking> bookings)
        {
            var total = 0.0;
            foreach (var booking in bookings)
            {
                total += GetTotal(booking);
            }
            return NumberUtil.FormatMoney(total);
        }
        public static string GetBookingCode(int bookingId) {
            return String.Format("ATM{0:D5}", bookingId);
        }
    }
}