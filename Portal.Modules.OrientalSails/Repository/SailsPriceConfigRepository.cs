using NHibernate;
using Portal.Modules.OrientalSails.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.Repository
{
    public class SailsPriceConfigRepository : RepositoryBase<SailsPriceConfig>
    {
        public SailsPriceConfigRepository() : base() { }
        public SailsPriceConfigRepository(ISession session) { }

        public SailsPriceConfig SailsPriceConfigGetByTripAndStartDate(SailsTrip trip, DateTime startDate)
        {
            var query = _session.QueryOver<SailsPriceConfig>();
            query = query.Where(spc => spc.Trip == trip);
            query = query.Where(spc => spc.ValidFrom <= startDate);
            return query.OrderBy(spc => spc.ValidFrom).Desc.FutureValue().Value;
        }
    }
}