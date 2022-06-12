using CMS.Core.Domain;
using NHibernate.Criterion;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.Repository
{
    public class BookingHistoryRepository : RepositoryBase<BookingHistory>
    {
        public IEnumerable<BookingHistory> BookingHistoryGetAllByBooking(Booking booking)
        {
            var query = _session.QueryOver<BookingHistory>();
            query = query.Where(bh => bh.Booking == booking);
            return query.Future();
        }

        public IEnumerable<BookingHistory> BookingHistoryGetAllConfirmedAndPending(User user)
        {
            var query = QueryOver.Of<BookingHistory>();
            query = query.Where(bh => bh.Date >= DateTime.Today && bh.Date <= DateTime.Today.Add(new TimeSpan(23, 59, 59)));
            Booking bookingAlias = null;
            query = query.JoinAlias(bh => bh.Booking, () => bookingAlias);
            query = query.Where(() => bookingAlias.Status == Web.Util.StatusType.Pending || bookingAlias.Status == StatusType.Approved);
            Agency agencyAlias = null;
            query = query.JoinAlias(() => bookingAlias.Agency, () => agencyAlias);
            if (user != null)
            {
                query = query.Where(() => agencyAlias.Sale == user);
            }
            query = query.Select(Projections.Distinct(Projections.Property(() => bookingAlias.Id)));

            var mainQuery = _session.QueryOver<BookingHistory>();
            mainQuery = mainQuery.WithSubquery.WhereProperty(bh => bh.Booking.Id).In(query);
            return mainQuery.Future().ToList();
        }
    }
}