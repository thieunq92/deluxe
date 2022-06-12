using CMS.Core.Domain;
using Portal.Modules.OrientalSails.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.Repository
{
    public class UserOrganizationRepository : RepositoryBase<UserOrganization>
    {
        public UserOrganizationRepository() : base() { }

        public IEnumerable<Organization> RegionGetAllByUser(User user)
        {
            var regions = new List<Organization>();
            if (!user.IsInRole("Administrator"))
            {
                var query = _session.QueryOver<UserOrganization>();
                query = query.Where(uo => uo.User == user);
                query = query.Select(uo => uo.Organization);
                regions = query.Future<Organization>().ToList();
            }
            else
            {
                var queryIfToBeAdministrator = _session.QueryOver<Organization>();
                regions = queryIfToBeAdministrator.Future().ToList();
            }
            return regions;
        }

        public IList<User> UserGetAllByOrganizations(IList<Organization> organizations)
        {
            var query = _session.QueryOver<UserOrganization>();
            query = query.AndRestrictionOn(uo => uo.Organization).IsIn(organizations.ToArray());
            query = query.Select(uo => uo.User);
            return query.Future<User>().ToList();
        }
    }
}