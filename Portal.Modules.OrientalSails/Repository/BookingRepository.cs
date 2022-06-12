using CMS.Core.Domain;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Dialect.Function;
using NHibernate.Transform;
using Portal.Modules.OrientalSails.DataTransferObject;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.Admin;
using Portal.Modules.OrientalSails.Web.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.Repository
{
    public class BookingRepository : RepositoryBase<Booking>
    {
        public BookingRepository() : base() { }
        public BookingRepository(ISession session) : base(session) { }

        public IQueryOver<Booking, Booking> BookingGetAllByCriterion(DateTime? date, params StatusType[] statusType)
        {
            var query = _session.QueryOver<Booking>().Where(x => x.Deleted == false);
            if (date != null)
            {
                query = query.Where(x => x.StartDate == date);
            }
            if (statusType.Count() > 0)
            {
                foreach (var eachStatusType in statusType)
                {
                    query = query.Where(x => x.Status == eachStatusType);
                }
            }
            return query;
        }

        public Booking BookingGetById(int bookingId)
        {
            return _session.QueryOver<Booking>()
                .Where(x => x.Deleted == false)
                .Where(x => x.Id == bookingId).FutureValue().Value;
        }

        public Booking BookingGetByBookingCode(string code)
        {
            code = code.Replace("ATM", "");
            int codeAsInt = 0;
            try
            {
                codeAsInt = Int32.Parse(code);
            }
            catch { }
            var query = _session.QueryOver<Booking>().Where(x => x.Deleted == false);
            query = query.Where(x => x.Id == codeAsInt);
            return query.FutureValue().Value;
        }

        public Booking BookingGetByTACode(string code)
        {
            var query = _session.QueryOver<Booking>().Where(x => x.Deleted == false);
            query = query.Where(x => x.AgencyCode == code);
            return query.FutureValue().Value;
        }

        public IEnumerable<Booking> BookingGetAllTodayBookings(User user)
        {
            var query = _session.QueryOver<Booking>().Where(x => x.Deleted == false);
            Agency agencyAlias = null;
            query = query.JoinAlias(x => x.Agency, () => agencyAlias);
            if (user != null)
            {
                query = query.Where(() => agencyAlias.Sale == user);
            }
            query = query.Where(x => x.StartDate == DateTime.Today);
            return query.Future().ToList();
        }

        public int BookingGetNumberOfBookingsInMonth(int month, int year, User user)
        {
            var firstDateOfMonth = new DateTime(year, month, 1);
            var lastDateOfMonth = firstDateOfMonth.AddMonths(1).AddDays(-1);
            var query = _session.QueryOver<Booking>().Where(b => b.Deleted == false);
            Agency agencyAlias = null;
            query = query.JoinAlias(b => b.Agency, () => agencyAlias);
            if (user != null)
            {
                query = query.Where(() => agencyAlias.Sale == user);
            }
            query = query.Where(x => x.StartDate >= firstDateOfMonth && x.StartDate <= lastDateOfMonth);
            query = query.Where(x => x.Status == StatusType.Approved);
            query = query.Select(Projections.RowCount());
            return query.FutureValue<int>().Value;
        }

        public double BookingGetTotalRevenueInMonth(int month, int year, User user)
        {
            var firstDateOfMonth = new DateTime(year, month, 1);
            var lastDateOfMonth = firstDateOfMonth.AddMonths(1).AddDays(-1);
            var query = _session.QueryOver<Booking>().Where(b => b.Deleted == false);
            Agency agencyAlias = null;
            query = query.JoinAlias(b => b.Agency, () => agencyAlias);
            if (user != null)
            {
                query = query.Where(() => agencyAlias.Sale == user);
            }
            query = query.Where(x => x.StartDate >= firstDateOfMonth && x.StartDate <= lastDateOfMonth);
            query = query.Where(x => x.Status == StatusType.Approved);
            query = query.Select(
                Projections.Sum(
                    Projections.Conditional(
                        Restrictions.Where<Booking>(x => !x.IsVND),
                        Projections.SqlFunction(
                            new VarArgsSQLFunction("(", "*", ")"),
                            NHibernateUtil.Double,
                            Projections.Property<Booking>(x => x.Total),
                             Projections.Property<Booking>(x => x.CurrencyRate)
                        ),
                        Projections.Property<Booking>(x => x.TotalVND)
                    )
                ));
            return query.Take(1).SingleOrDefault<double>();
        }

        public IEnumerable<Booking> BookingGetAllNewBookings(User user)
        {
            var query = _session.QueryOver<Booking>().Where(b => b.Deleted == false);
            Agency agencyAlias = null;
            query = query.JoinAlias(b => b.Agency, () => agencyAlias);
            if (user != null)
            {
                query = query.Where(() => agencyAlias.Sale == user);
            }
            //Kiểm tra điều kiện booking được tạo trong ngày hôm nay
            query = query.Where(x => x.CreatedDate >= DateTime.Today && x.CreatedDate <= DateTime.Today.Add(new TimeSpan(23, 59, 59)));
            //--
            return query.Future().ToList();
        }

        public IEnumerable<Booking> BookingGetAllNewBookingsByCampaign(Campaign campaign)
        {
            var query = _session.QueryOver<Booking>().Where(b => b.CreatedDate > campaign.CreatedDate);
            query = query.AndRestrictionOn(b => b.StartDate).IsIn(campaign.Policies.SelectMany(p => p.GoldenDays.Select(gd => new DateTime(gd.Date.Year, gd.Date.Month, gd.Date.Day))).ToList());
            return query.Future().ToList();
        }

        public IList<Booking> BookingGetAllInMonth(int month, int year, User user)
        {
            var firstDateOfMonth = new DateTime(year, month, 1);
            var lastDateOfMonth = firstDateOfMonth.AddMonths(1).AddDays(-1);
            var query = _session.QueryOver<Booking>().Where(b => b.Deleted == false);
            Agency agencyAlias = null;
            query = query.JoinAlias(b => b.Agency, () => agencyAlias);
            if (user != null)
            {
                query = query.Where(() => agencyAlias.Sale == user);
            }
            query = query.Where(x => x.StartDate >= firstDateOfMonth && x.StartDate <= lastDateOfMonth);
            query = query.Where(x => x.Status == StatusType.Approved);
            return query.Future().ToList();
        }

        public double BookingGetTotalRevenueInMonth(int month, int year, SailsTrip trip)
        {
            var firstDateOfMonth = new DateTime(year, month, 1);
            var lastDateOfMonth = firstDateOfMonth.AddMonths(1).AddDays(-1);
            var query = _session.QueryOver<Booking>().Where(b => b.Deleted == false);
            if (trip != null)
            {
                query = query.Where(b => b.Trip == trip);
            }
            Agency agencyAlias = null;
            query = query.JoinAlias(b => b.Agency, () => agencyAlias);
            query = query.Where(x => x.StartDate >= firstDateOfMonth && x.StartDate <= lastDateOfMonth);
            query = query.Where(x => x.Status == StatusType.Approved);
            query = query.Select(
                Projections.Sum(
                    Projections.Conditional(
                        Restrictions.Where<Booking>(x => !x.IsVND),
                        Projections.SqlFunction(
                            new VarArgsSQLFunction("(", "*", ")"),
                            NHibernateUtil.Double,
                            Projections.Property<Booking>(x => x.Total),
                            Projections.Property<Booking>(x => x.CurrencyRate)
                        ),
                        Projections.Property<Booking>(x => x.TotalVND)
                    )
                ));
            return query.Take(1).SingleOrDefault<double>();
        }

        public IList<Booking> BookingGetAllInMonth(int month, int year, SailsTrip trip)
        {
            var firstDateOfMonth = new DateTime(year, month, 1);
            var lastDateOfMonth = firstDateOfMonth.AddMonths(1).AddDays(-1);
            var query = _session.QueryOver<Booking>().Where(b => b.Deleted == false);
            if (trip != null)
            {
                query = query.Where(b => b.Trip == trip);
            }
            Agency agencyAlias = null;
            query = query.JoinAlias(b => b.Agency, () => agencyAlias);
            query = query.Where(x => x.StartDate >= firstDateOfMonth && x.StartDate <= lastDateOfMonth);
            query = query.Where(x => x.Status == StatusType.Approved);
            return query.Future().ToList();
        }

        public IEnumerable<Booking> BookingGetByMonthYearAndSales(int month, int year, User sales)
        {
            var firstDateOfMonth = new DateTime(year, month, 1);
            var lastDateOfMonth = firstDateOfMonth.AddMonths(1).AddDays(-1);
            var query = _session.QueryOver<Booking>().Where(b => b.Deleted == false);
            Agency agencyAlias = null;
            query = query.JoinAlias(b => b.Agency, () => agencyAlias);
            query = query.Where(() => agencyAlias.Sale == sales);
            query = query.Where(x => x.StartDate >= firstDateOfMonth && x.StartDate <= lastDateOfMonth);
            query = query.Where(x => x.Status == StatusType.Approved);
            return query.Future();
        }

        public IEnumerable<Booking> BookingGetAllNewBookings(DateTime date, List<Organization> organizations, User sales)
        {
            var query = _session.QueryOver<Booking>().Where(x => x.Deleted == false);
            //Kiểm tra điều kiện booking được tạo trong ngày hôm nay
            query = query.Where(x => x.CreatedDate >= date && x.CreatedDate <= date.Add(new TimeSpan(23, 59, 59)));
            //--
            SailsTrip tripAlias = null;
            query = query.JoinAlias(x => x.Trip, () => tripAlias);
            query = query.AndRestrictionOn(() => tripAlias.Organization).IsIn(organizations);
            Agency agencyAlias = null;
            if (sales != null && sales.Id > 0)
            {
                query = query.JoinAlias(x => x.Agency, () => agencyAlias);
                query = query.Where(x => agencyAlias.Sale == sales);
            }
            query = query.Where(x => x.Status == StatusType.Pending || x.Status == StatusType.Approved);
            return query.Future().ToList();
        }

        public IEnumerable<Booking> BookingGetAllCancelledBookingOnDate(DateTime date, List<Organization> organizations, User sales)
        {
            var query = _session.QueryOver<Booking>().Where(x => x.Deleted == false);
            SailsTrip tripAlias = null;
            query = query.JoinAlias(x => x.Trip, () => tripAlias);
            query = query.AndRestrictionOn(x => tripAlias.Organization).IsIn(organizations);
            BookingHistory bookingHistory = null;
            query = query.JoinAlias(b => b.BookingHistories, () => bookingHistory);
            query = query.Where(() => bookingHistory.Date >= date && bookingHistory.Date <= date.Add(new TimeSpan(23, 59, 59)));
            query = query.Where(() => bookingHistory.Status == StatusType.Cancelled);
            Agency agencyAlias = null;
            if (sales != null && sales.Id > 0)
            {
                query = query.JoinAlias(x => x.Agency, () => agencyAlias);
                query = query.Where(x => agencyAlias.Sale == sales);
            }
            return query.TransformUsing(new DistinctRootEntityResultTransformer()).Future().ToList();
        }

        public object BookingGetAllByEventCodes(List<EventCode> tripCodes)
        {
            var dates = tripCodes.Select(tc => tc.SailExpense.Date).ToList();
            var trips = tripCodes.Select(tc => tc.SailExpense.Trip).ToList();
            var query = _session.QueryOver<Booking>()
                .Where(b => b.Deleted == false)
                .Where(b => b.Status == StatusType.Approved)
                .WhereRestrictionOn(b => b.Trip).IsIn(trips)
                .WhereRestrictionOn(b => b.StartDate).IsIn(dates);
            return query.List().ToList();
        }
    }
}