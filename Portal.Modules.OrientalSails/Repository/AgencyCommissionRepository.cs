using Portal.Modules.OrientalSails.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.Repository
{
    public class AgencyCommissionRepository : RepositoryBase<AgencyCommission>
    {
        public AgencyCommission AgencyCommissionGetBy(SailsTrip trip, DateTime startDate)
        {
            return AgencyCommissionGetBy(trip, startDate, null);
        }

        public AgencyCommission AgencyCommissionGetBy(SailsTrip trip, DateTime startDate, AgencyLevel agencyLevel)
        {
            var query = _session.QueryOver<AgencyCommission>();
            if (trip != null) { query = query.Where(ac => ac.SailsTrip == trip); }
            if (startDate != DateTime.MinValue) { query = query.Where(ac => ac.ValidFrom <= startDate); }
            if (agencyLevel != null) { query = query.Where(ac => ac.AgencyLevel == agencyLevel); }
            return query.FutureValue().Value;
        }
    }
}