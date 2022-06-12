using CMS.Core.Domain;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.BusinessLogic.Share
{
    //Bussiness logic cho quản lý User
    public class UserBLL
    {
        public UserRepository UserRepository { get; set; }
        public UserOrganizationRepository UserOrganizationRepository { get; set; }

        public UserBLL()
        {
            UserRepository = new UserRepository();
            UserOrganizationRepository = new UserOrganizationRepository();
        }
        /// <summary>
        /// Giải phóng tất cả kết nổi đến cơ sở dữ liệu. 
        /// Phải thực hiện sau khi dùng xong Repository.
        /// </summary>
        public void Dispose()
        {
            if (UserRepository != null)
            {
                UserRepository.Dispose();
                UserRepository = null;
            }
            if (UserOrganizationRepository != null)
            {
                UserOrganizationRepository.Dispose();
                UserOrganizationRepository = null;
            }
        }
        /// <summary>
        /// Lấy User đang đăng nhập
        /// </summary>
        /// <returns>Trả về User đang đăng nhập</returns>
        public User UserGetCurrent()
        {
            return UserRepository.UserGetById(Convert.ToInt32(HttpContext.Current.User.Identity.Name));
        }

        /// <summary>
        /// Lấy FullName của User đang đăng nhập
        /// </summary>
        /// <returns>Trả về FullName của User đang đăng nhập</returns>
        public string UserCurrentGetName()
        {
            return UserGetCurrent().FullName;
        }

        /// <summary>
        /// Lấy về danh sách các User có Role cần tìm
        /// </summary>
        /// <param name="roleId">Id của Role cần lấy User</param>
        /// <returns>Trả về danh sách User có Role cần tìm</returns>
        public IList<User> UserGetByRole(int roleId)
        {
            return UserRepository.UserGetByRole(roleId);
        }

        public IList<User> UserGetByOrganization(IList<Organization> organizations)
        {
            return UserOrganizationRepository.UserGetAllByOrganizations(organizations);
        }

        public User UserGetById(int salesId)
        {
            return UserRepository.GetById(salesId);
        }
    }
}