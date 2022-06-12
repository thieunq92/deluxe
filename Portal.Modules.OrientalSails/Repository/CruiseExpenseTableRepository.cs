using NHibernate;
using Portal.Modules.OrientalSails.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.Repository
{
    public class CruiseExpenseTableRepository: RepositoryBase<CruiseExpenseTable>
    {
        public CruiseExpenseTableRepository() : base() { }
        public CruiseExpenseTableRepository(ISession session) : base() { }

        public CruiseExpenseTable CruiseExpenseTableGetByTripAndDate(SailsTrip trip, DateTime startDate)
        {
            var query = _session.QueryOver<CruiseExpenseTable>();
            Cruise cruiseAlias = null;
            query = query.JoinAlias(cet=>cet.Cruise, ()=>cruiseAlias);
            query = query.Where(() => cruiseAlias.Name == trip.Name);
            query = query.Where(cet => cet.ValidFrom <= startDate && cet.ValidTo >= startDate);
            return query.FutureValue().Value;
        }
    }
}