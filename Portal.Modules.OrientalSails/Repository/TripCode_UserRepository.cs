using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Portal.Modules.OrientalSails.Domain;

namespace Portal.Modules.OrientalSails.Repository
{
    public class TripCode_UserRepository : RepositoryBase<TripCode_User>
    {
        public TripCode_UserRepository() : base() { }

        public TripCode_User TripCode_UserGetByTripCodeAndUser(string tripCode, CMS.Core.Domain.User currentUser)
        {
            return _session.QueryOver<TripCode_User>()
                .Where(tu => tu.TripCode == tripCode && tu.User == currentUser).Future<TripCode_User>().ToList()
                .FirstOrDefault();
        }
    }
}