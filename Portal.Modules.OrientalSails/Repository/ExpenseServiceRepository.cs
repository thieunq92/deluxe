using NHibernate;
using Portal.Modules.OrientalSails.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.Repository
{
    public class ExpenseServiceRepository : RepositoryBase<ExpenseService>
    {
        public ExpenseServiceRepository() : base() { }
        public ExpenseServiceRepository(ISession session) : base(session) { }

        public IQueryOver<ExpenseService, ExpenseService> ExpenseServiceGetAllByCriterion(DateTime? date)
        {
            var query = _session.QueryOver<ExpenseService>();
            SailExpense sailExpense = null;
            query = query.JoinAlias(x => x.Expense, () => sailExpense);
            if (date != null)
            {
                query = query.Where(() => sailExpense.Date == date);
            }
            return query;
        }

        public IList<ExpenseService> ExpenseServiceGetAllInMonth(int month, int year)
        {
            var firstDateOfMonth = new DateTime(year, month, 1);
            var lastDateOfMonth = firstDateOfMonth.AddMonths(1).AddDays(-1);
            var query = _session.QueryOver<ExpenseService>();
            SailExpense sailExpense = null;
            query = query.JoinAlias(x => x.Expense, () => sailExpense);
            query = query.Where(() => sailExpense.Date >= firstDateOfMonth && sailExpense.Date <= lastDateOfMonth);
            return query.Future().ToList();
        }

        public IList<ExpenseService> ExpenseServiceGetAllInMonth(int month, int year, SailsTrip trip)
        {
            var firstDateOfMonth = new DateTime(year, month, 1);
            var lastDateOfMonth = firstDateOfMonth.AddMonths(1).AddDays(-1);
            var query = _session.QueryOver<ExpenseService>();
            SailExpense sailExpense = null;
            query = query.JoinAlias(x => x.Expense, () => sailExpense);
            query = query.Where(() => sailExpense.Date >= firstDateOfMonth && sailExpense.Date <= lastDateOfMonth);
            if (trip != null)
                query = query.Where(() => sailExpense.Trip == trip);
            return query.Future().ToList();
        }

        public IList<ExpenseService> ExpenseServiceGetAllTodayByTrip(SailsTrip trip)
        {
            var today = DateTime.Today;
            var query = _session.QueryOver<ExpenseService>();
            SailExpense sailExpense = null;
            query = query.JoinAlias(x => x.Expense, () => sailExpense);
            query = query.Where(() => sailExpense.Date == today);
            if (trip != null)
                query = query.Where(() => sailExpense.Trip == trip);
            return query.Future().ToList();
        }
        public IList<ExpenseService> ExpenseServiceGetAllTodayByTripAndDate(SailsTrip trip, DateTime date)
        {
            var today = date;
            var query = _session.QueryOver<ExpenseService>();
            SailExpense sailExpense = null;
            query = query.JoinAlias(x => x.Expense, () => sailExpense);
            query = query.Where(() => sailExpense.Date == today);
            if (trip != null)
                query = query.Where(() => sailExpense.Trip == trip);
            return query.Future().ToList();
        }
    }
}