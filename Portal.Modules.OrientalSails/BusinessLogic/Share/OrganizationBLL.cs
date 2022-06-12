using CMS.Core.Domain;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Enums;
using Portal.Modules.OrientalSails.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.BusinessLogic.Share
{
    public class OrganizationBLL
    {
        public UserRepository UserRepository { get; set; }

        public RoleRepository RoleRepository { get; set; }

        public PermissionRepository PermissionRepository { get; set; }

        public SpecialPermissionRepository SpecialPermissionRepository { get; set; }

        public SailsModule Module { get; set; }
        public OrganizationBLL()
        {
            UserRepository = new UserRepository();
            RoleRepository = new RoleRepository();
            PermissionRepository = new PermissionRepository();
            SpecialPermissionRepository = new SpecialPermissionRepository();
            Module = SailsModule.GetInstance();
        }
        public void Dispose()
        {
            if (UserRepository != null)
            {
                UserRepository.Dispose();
                UserRepository = null;
            }

            if (RoleRepository != null)
            {
                RoleRepository.Dispose();
                RoleRepository = null;
            }

            if (PermissionRepository != null)
            {
                PermissionRepository.Dispose();
                PermissionRepository = null;
            }

            if (SpecialPermissionRepository != null)
            {
                SpecialPermissionRepository.Dispose();
                SpecialPermissionRepository = null;
            }
        }
        public bool UserCheckRole(int userId, int roleId)
        {
            var user = UserRepository.UserGetById(userId);
            var role = RoleRepository.RoleGetById(roleId);

            if (user == null || role == null)
                return false;

            foreach (Role userRole in user.Roles)
            {
                if (role.Id == userRole.Id)
                {
                    return true;
                }
            }
            return false;
        }
        public List<Organization> OrganizationGetAllByUser(User user)
        {
            var organizations = Module.OrganizationGetAllRoot();
            if (!UserCheckRole(user.Id, (int)Roles.Administrator))
                return Module.OrganizationGetByUser(user).Cast<UserOrganization>().Select(uo=>uo.Organization).ToList();
            return organizations.Cast<Organization>().ToList();
        }
    }
}