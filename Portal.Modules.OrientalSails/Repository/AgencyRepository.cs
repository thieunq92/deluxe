using CMS.Core.Domain;
using NHibernate;
using NHibernate.Criterion;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.Repository
{
    public class AgencyRepository : RepositoryBase<Agency>
    {
        public AgencyRepository() : base() { }
        public AgencyRepository(ISession session) : base(session) { }

        public object AgencyGetTop10(CMS.Core.Domain.User user)
        {
            var firstDateOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
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

            query = query.Select(
                    Projections.Property(() => agencyAlias.Id),
                    Projections.Property(() => agencyAlias.Name),
                    Projections.RowCount(),
                    Projections.Group(() => agencyAlias.Id),
                    Projections.Group(() => agencyAlias.Name)
                );
            return query.OrderBy(Projections.RowCount()).Desc.Take(10).Future<object[]>().ToList()
                .Select(x => new { AgencyId = x[0], AgencyName = x[1], NumberOfPax = x[2] }).ToList();
        }

        public object AgencyGetAllAgenciesNotVisitedInLast2Month(User user)
        {
            var last2month = DateTime.Today.AddMonths(-2);
            Activity activityAlias = null;
            var findLastestVisitedQuery = QueryOver.Of<Activity>()
                .Where(x => x.Params == activityAlias.Params && x.DateMeeting > activityAlias.DateMeeting)
                .Select(Projections.RowCount());
            var agenciesNotVisitedInLast2MonthQuery = QueryOver.Of<Activity>(() => activityAlias)
                .Where(x => x.DateMeeting <= last2month)
                .WithSubquery.WhereValue(1).Gt(findLastestVisitedQuery).Select(x => x.Params);
            var agenciesVisitedInLast2MonthQuery = QueryOver.Of<Activity>(() => activityAlias)
                .Where(x => x.DateMeeting > last2month)
                .WithSubquery.WhereValue(1).Gt(findLastestVisitedQuery).Select(x => x.Params);
            var agenciesNotVisitedAnyTime = _session.QueryOver<Agency>()
                .Where(x => x.Sale == user)
                .WithSubquery.WhereProperty(x => x.Id).NotIn(agenciesNotVisitedInLast2MonthQuery)
                .WithSubquery.WhereProperty(x => x.Id).NotIn(agenciesVisitedInLast2MonthQuery)
                .Future().ToList();
            var activitiesOfAgenciesNotVisitedInLast2Month = _session.QueryOver<Activity>(() => activityAlias).Where(x => x.DateMeeting <= last2month && x.User == user)
                .WithSubquery.WhereValue(1).Gt(findLastestVisitedQuery)
                .OrderBy(x => x.DateMeeting).Asc
                .Future().ToList();
            var agenciesNotUpdateOrNotVisitedAnyTime = new List<object>();
            foreach (var agencyNotVisitedAnyTime in agenciesNotVisitedAnyTime)
            {
                agenciesNotUpdateOrNotVisitedAnyTime.Add(new
                {
                    AgencyId = agencyNotVisitedAnyTime.Id,
                    Name = agencyNotVisitedAnyTime.Name,
                    LastMeeting = "",
                    Note = "",
                });
            }
            foreach (var activityOfAgencyNotVisitedInLast2Month in activitiesOfAgenciesNotVisitedInLast2Month)
            {
                var agencyId = 0;
                try
                {
                    agencyId = Int32.Parse(activityOfAgencyNotVisitedInLast2Month.Params);
                }
                catch { }
                var agency = _session.QueryOver<Agency>().Where(x => x.Id == agencyId).FutureValue().Value;
                var agencyNotUpdate = new
                {
                    AgencyId = agencyId,
                    Name = agency != null ? agency.Name : "",
                    LastMeeting = activityOfAgencyNotVisitedInLast2Month.DateMeeting.ToString("dd/MM/yyyy"),
                    Note = activityOfAgencyNotVisitedInLast2Month.Note
                };
                agenciesNotUpdateOrNotVisitedAnyTime.Add(agencyNotUpdate);
            }
            return agenciesNotUpdateOrNotVisitedAnyTime;
        }

        public object AgencyGetAllAgenciesSendNoBookingsLast3Month(User user)
        {
            var last3month = DateTime.Today.AddMonths(-3);
            var agenciesSendBookingsLast3Month = QueryOver.Of<Booking>().Where(x => x.CreatedDate > last3month)
                .Select(x => x.Agency.Id)
                .Select(Projections.Distinct(Projections.Property<Booking>(x => x.Agency.Id)));
            var agenciesSendNoBookingsLast3Month = QueryOver.Of<Agency>()
                .Where(x => x.Sale == user).WithSubquery.WhereProperty(x => x.Id)
                .NotIn(agenciesSendBookingsLast3Month)
                .Select(x => x.Id);
            Agency agencyAlias = null;
            Booking bookingAlias = null;
            var findLastestSendBookingQuery = QueryOver.Of<Booking>().Where(x => x.Agency == bookingAlias.Agency && x.CreatedDate > bookingAlias.CreatedDate).Select(Projections.RowCount());
            var query = _session.QueryOver<Booking>(() => bookingAlias).JoinAlias(x => x.Agency, () => agencyAlias)
                .WithSubquery.WhereProperty(x => x.Agency.Id).In(agenciesSendNoBookingsLast3Month)
                .WithSubquery.WhereValue(1).Gt(findLastestSendBookingQuery)
                .OrderBy(x => x.CreatedDate).Desc;
            return query.Future().ToList();
        }

        public object AgencyGetTop10InMonth(int month, User salesInCharge)
        {
            var firstDateOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            if (month >= 1 && month <= 12)
            {
                firstDateOfMonth = new DateTime(DateTime.Today.Year, month, 1);
            }
            var lastDateOfMonth = firstDateOfMonth.AddMonths(1).AddDays(-1);
            var query = _session.QueryOver<Customer>();
            query = query.Where(c => c.Type == CustomerType.Adult || c.Type == CustomerType.Children || c.Type == CustomerType.Baby);
            Booking bookingAlias = null;
            query = query.JoinAlias(c => c.Booking, () => bookingAlias);
            Agency agencyAlias = null;
            query = query.JoinAlias(() => bookingAlias.Agency, () => agencyAlias);
            if (salesInCharge != null)
            {
                query = query.Where(() => agencyAlias.Sale == salesInCharge);
            }
            query = query.Where(() => bookingAlias.Deleted == false);
            query = query.Where(() => bookingAlias.StartDate >= firstDateOfMonth && bookingAlias.StartDate <= lastDateOfMonth);
            query = query.Where(() => bookingAlias.Status == StatusType.Approved);

            query = query.Select(
                    Projections.Property(() => agencyAlias.Id),
                    Projections.Property(() => agencyAlias.Name),
                    Projections.RowCount(),
                    Projections.Group(() => agencyAlias.Id),
                    Projections.Group(() => agencyAlias.Name)
                );
            return query.OrderBy(Projections.RowCount()).Desc.Take(10).Future<object[]>().ToList()
                .Select(x => new { AgencyId = x[0], AgencyName = x[1], NumberOfPax = x[2] }).ToList();
        }

        public object AgencyGetTop10InMonthAndTrip(int month, SailsTrip trip)
        {
            var firstDateOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            if (month >= 1 && month <= 12)
            {
                firstDateOfMonth = new DateTime(DateTime.Today.Year, month, 1);
            }
            var lastDateOfMonth = firstDateOfMonth.AddMonths(1).AddDays(-1);
            var query = _session.QueryOver<Customer>();
            query = query.Where(c => c.Type == CustomerType.Adult || c.Type == CustomerType.Children || c.Type == CustomerType.Baby);
            Booking bookingAlias = null;
            query = query.JoinAlias(c => c.Booking, () => bookingAlias);
            Agency agencyAlias = null;
            query = query.JoinAlias(() => bookingAlias.Agency, () => agencyAlias);
            query = query.Where(() => bookingAlias.Deleted == false);
            query = query.Where(() => bookingAlias.StartDate >= firstDateOfMonth && bookingAlias.StartDate <= lastDateOfMonth);
            query = query.Where(() => bookingAlias.Status == StatusType.Approved);
            if (trip != null)
            {
                query = query.Where(() => bookingAlias.Trip == trip);
            }
            query = query.Select(
                    Projections.Property(() => agencyAlias.Id),
                    Projections.Property(() => agencyAlias.Name),
                    Projections.RowCount(),
                    Projections.Group(() => agencyAlias.Id),
                    Projections.Group(() => agencyAlias.Name)
                );
            return query.OrderBy(Projections.RowCount()).Desc.Take(10).Future<object[]>().ToList()
                .Select(x => new { AgencyId = x[0], AgencyName = x[1], NumberOfPax = x[2] }).ToList();
        }

        public object AgencyGetTop10(List<Organization> organizations, int year, int month)
        {
            var firstDateOfMonth = new DateTime(year, month, 1);
            var lastDateOfMonth = firstDateOfMonth.AddMonths(1).AddDays(-1);
            var query = _session.QueryOver<Customer>();
            query = query.Where(c => c.Type == CustomerType.Adult || c.Type == CustomerType.Children || c.Type == CustomerType.Baby);
            Booking bookingAlias = null;
            query = query.JoinAlias(c => c.Booking, () => bookingAlias);
            Agency agencyAlias = null;
            query = query.JoinAlias(() => bookingAlias.Agency, () => agencyAlias);
            SailsTrip tripAlias = null;
            query = query.JoinAlias(() => bookingAlias.Trip, () => tripAlias);
            query = query.Where(() => tripAlias.Deleted == false);
            query = query.AndRestrictionOn(() => tripAlias.Organization).IsIn(organizations);
            query = query.Where(() => bookingAlias.Deleted == false);
            query = query.Where(() => bookingAlias.StartDate >= firstDateOfMonth && bookingAlias.StartDate <= lastDateOfMonth);
            query = query.Where(() => bookingAlias.Status == StatusType.Approved);

            query = query.Select(
                    Projections.Property(() => agencyAlias.Id),
                    Projections.Property(() => agencyAlias.Name),
                    Projections.RowCount(),
                    Projections.Group(() => agencyAlias.Id),
                    Projections.Group(() => agencyAlias.Name)
                );
            return query.OrderBy(Projections.RowCount()).Desc.Take(10).Future<object[]>().ToList()
                .Select(x => new { AgencyId = x[0], AgencyName = x[1], NumberOfPax = x[2] }).ToList();
        }
    }
}