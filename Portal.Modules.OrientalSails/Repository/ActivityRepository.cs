using CMS.Core.Domain;
using NHibernate;
using NHibernate.Criterion;
using Portal.Modules.OrientalSails.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.Repository
{
    public class ActivityRepository : RepositoryBase<Activity>
    {
        public ActivityRepository() : base() { }

        public IEnumerable<Activity> ActivityGetAllActivityInMonth(int month, int year, User user)
        {
            var firstDateOfMonth = new DateTime(year, month, 1);
            var lastDateOfMonth = firstDateOfMonth.AddMonths(1).AddDays(-1).Add(new TimeSpan(23, 59, 59));
            var query = _session.QueryOver<Activity>();
            query = query.Where(x => x.DateMeeting >= firstDateOfMonth && x.DateMeeting <= lastDateOfMonth)
                .Where(x => x.User == user);
            return query.Future().ToList();
        }

        public IEnumerable<Activity> ActivityGetAllRecentMeetingsInDateRange(User user, DateTime from, DateTime to)
        {
            var query = _session.QueryOver<Activity>();
            if (user != null)
            {
                query = query.Where(x => x.User == user);
            }
            query = query.Where(a => a.DateMeeting >= from && a.DateMeeting <= to);
            return query.Future().ToList();
        }

        public Dictionary<User, IFutureValue<int>> ActivityGetCountByMonthYearAndSales(int month, int year, List<User> saleses)
        {
            var firstDateOfMonth = new DateTime(year, month, 1);
            var lastDateOfMonth = firstDateOfMonth.AddMonths(1).AddDays(-1).Add(new TimeSpan(23, 59, 59));
            var salesActivitiesCounts = new Dictionary<User, IFutureValue<int>>();
            foreach (var sales in saleses)
            {
                var query = _session.QueryOver<Activity>();
                query = query.Where(x => x.DateMeeting >= firstDateOfMonth && x.DateMeeting <= lastDateOfMonth);
                query.Where(x => x.User == sales);
                salesActivitiesCounts.Add(sales, query.Select(Projections.RowCount()).FutureValue<int>());
            }
            return salesActivitiesCounts;
        }
    }
}