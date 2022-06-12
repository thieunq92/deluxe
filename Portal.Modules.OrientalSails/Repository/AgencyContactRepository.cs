using Portal.Modules.OrientalSails.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.Repository
{
    public class AgencyContactRepository : RepositoryBase<AgencyContact>
    {
        public AgencyContactRepository() : base() { }

        public IEnumerable<AgencyContact> AgencyContactGetAllByAgencyId(int agencyId)
        {
            return _session.QueryOver<AgencyContact>().Where(x => x.Agency.Id == agencyId).Future().ToList();
        }
    }
}