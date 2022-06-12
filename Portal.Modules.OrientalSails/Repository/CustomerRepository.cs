using CMS.Core.Domain;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using Portal.Modules.OrientalSails.DataTransferObject;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.Repository
{
    public class CustomerRepository : RepositoryBase<Customer>
    {
        public CustomerRepository() : base() { }

        public int CustomerGetNumberOfCustomersInMonth(int month, int year, User user)
        {
            var firstDateOfMonth = new DateTime(year, month, 1);
            var lastDateOfMonth = firstDateOfMonth.AddMonths(1).AddDays(-1);
            var query = _session.QueryOver<Customer>();
            query = query.Where(c => c.Type == CustomerType.Adult || c.Type == CustomerType.Children || c.Type == CustomerType.Baby);
            Booking bookingAlias = null;
            query = query.JoinAlias(c => c.Booking, () => bookingAlias);
            Agency agencyAlias = null;
            query = query.JoinAlias(() => bookingAlias.Agency, () => agencyAlias);
            if (user != null)
            {
                query = query.Where(() => agencyAlias.Sale == user);
            }
            query = query.Where(() => bookingAlias.Deleted == false);
            query = query.Where(() => bookingAlias.StartDate >= firstDateOfMonth && bookingAlias.StartDate <= lastDateOfMonth);
            query = query.Where(() => bookingAlias.Status == StatusType.Approved);
            query = query.Select(Projections.RowCount());
            return query.FutureValue<int>().Value;
        }

        public int CustomerGetCountingByDate(DateTime date)
        {
            var query = _session.QueryOver<Customer>();
            query = query.Where(c => c.Type == CustomerType.Adult || c.Type == CustomerType.Children || c.Type == CustomerType.Baby);
            Booking bookingAlias = null;
            query = query.JoinAlias(c => c.Booking, () => bookingAlias);
            query = query.Where(() => bookingAlias.Deleted == false);
            query = query.Where(() => bookingAlias.StartDate == date);
            query = query.Where(() => bookingAlias.Status == StatusType.Approved || bookingAlias.Status == StatusType.Pending);
            query = query.Select(Projections.RowCount());
            return query.FutureValue<int>().Value;
        }

        public int CustomerGetNumberOfCustomersInMonth(int month, int year, SailsTrip trip)
        {
            var firstDateOfMonth = new DateTime(year, month, 1);
            var lastDateOfMonth = firstDateOfMonth.AddMonths(1).AddDays(-1);
            var query = _session.QueryOver<Customer>();
            query = query.Where(c => c.Type == CustomerType.Adult || c.Type == CustomerType.Children || c.Type == CustomerType.Baby);
            Booking bookingAlias = null;
            query = query.JoinAlias(c => c.Booking, () => bookingAlias);
            Agency agencyAlias = null;
            query = query.JoinAlias(() => bookingAlias.Agency, () => agencyAlias);
            if (trip != null)
            {
                query = query.Where(() => bookingAlias.Trip == trip);
            }
            query = query.Where(() => bookingAlias.Deleted == false);
            query = query.Where(() => bookingAlias.StartDate >= firstDateOfMonth && bookingAlias.StartDate <= lastDateOfMonth);
            query = query.Where(() => bookingAlias.Status == StatusType.Approved);
            query = query.Select(Projections.RowCount());
            return query.FutureValue<int>().Value;
        }

        public object CustomerGetCountByTripsAndDate(List<SailsTrip> trips, List<DateTime> dates)
        {
            var query = @"SELECT COUNT(Customer.Id) AS NumberOfPax, Booking.TripId, Booking.StartDate FROM os_Customer Customer JOIN os_Booking Booking ON Customer.BookingId = Booking.Id
WHERE Booking.TripId IN (:tripsId) AND Booking.StartDate IN (:startDates)
GROUP BY Booking.StartDate, Booking.TripId";
           var dataAll = _session.CreateSQLQuery(query)
                .SetParameterList("tripsId",trips.Select(t=>t.Id).ToList())
                .SetParameterList("startDates", dates)
                .SetResultTransformer(Transformers.AliasToBean<BookingDTO>()).List<BookingDTO>();
            return dataAll;
        }

        public object CustomerGetCountByBookings(List<Booking> bookings)
        {
            if (bookings.Count == 0) return new List<BookingDTO>();
            var query = @"SELECT COUNT(Customer.Id) AS NumberOfPax, Booking.Id, Booking.SpecialRequest FROM os_Customer Customer JOIN os_Booking Booking ON Customer.BookingId = Booking.Id
WHERE Booking.Id IN (:bookingIds)
GROUP BY Booking.Id, Booking.SpecialRequest";
            var dataAll = _session.CreateSQLQuery(query)
                .SetParameterList("bookingIds", bookings.Select(t => t.Id).ToList())
                .SetResultTransformer(Transformers.AliasToBean<BookingDTO>()).List<BookingDTO>();
            return dataAll;
        }
    }
}