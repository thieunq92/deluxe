using NHibernate;
using Portal.Modules.OrientalSails.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.Repository
{
    public class ExpenseHistoryRepository : RepositoryBase<ExpenseHistory>
    {
        public ExpenseHistoryRepository() : base() { }
        public ExpenseHistoryRepository(ISession session) : base(session) { }

        public ICollection<ExpenseHistory> ExpenseHistoryGetAllByCriterion(ExpenseService expenseService)
        {
            var query = _session.QueryOver<ExpenseHistory>().Where(x => x.ExpenseService == expenseService);
            return query.Future().ToList();
        }
    }
}