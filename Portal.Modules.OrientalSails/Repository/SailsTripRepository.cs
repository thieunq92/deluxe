using NHibernate;
using Portal.Modules.OrientalSails.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.Repository
{
    public class SailsTripRepository : RepositoryBase<SailsTrip>
    {
        public SailsTripRepository() : base() { }
        public SailsTripRepository(ISession session) : base(session) { }

        public IEnumerable<SailsTrip> TripGetAllByRegion(Organization region)
        {
            var query = _session.QueryOver<SailsTrip>().Where(st => st.Deleted == false);
            query = query.Where(st => st.Organization == region);
            return query.Future().ToList();
        }
    }
}