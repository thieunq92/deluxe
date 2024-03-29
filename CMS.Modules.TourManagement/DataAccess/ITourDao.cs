using System.Collections;
using CMS.Core.Domain;
using CMS.Modules.TourManagement.Domain;
using NHibernate.Criterion;

namespace CMS.Modules.TourManagement.DataAccess
{
    public interface ITourDao
    {
        #region -- Location --

        /// <summary>
        /// Lấy danh sách toàn bộ các địa điểm
        /// </summary>
        /// <returns></returns>
        IList LocationGetAll();

        /// <summary>
        /// Lấy tất cả các location mà không có cha.
        /// </summary>
        /// <returns></returns>
        IList LocationGetRoot();

        /// <summary>
        /// Lấy danh sách Location con của 1 Location
        /// </summary>
        /// <param name="parentID">ID của Location Cha</param>
        /// <returns></returns>
        IList LocationGetByParentID(int parentID);

        void Resort(Location NewlyLocation, bool up);

        #endregion

        #region -- Agency --

        /// <summary>
        /// Lấy danh sách toàn bộ các chính sách đại lý
        /// </summary>
        /// <returns></returns>
        IList AgencyPolicyGetAll(string moduleType);

        /// <summary>
        /// Lấy về đại lý theo role cho trước
        /// </summary>
        /// <param name="role">Role đại lý lấy về</param>
        /// <param name="moduleType"></param>
        /// <returns>AgencyPolicy</returns>
        IList AgencyPolicyGetByRole(Role role, string moduleType);

        /// <summary>
        /// Lấy tất cả các Role có AgencyPolicy;
        /// </summary>
        /// <returns></returns>
        IList RoleAgencyPolicyGetAll(string moduleType);

        double RoleGetPercentage(Role role, string moduleType);

        #endregion

        #region -- Currency --

        IList CurrencyGetAll();
        void Delete(Currency currency);

        #endregion

        #region -- Gallery Services --

        IList AlbumGetByName(string name);

        #endregion

        #region -- Provoder --

        /// <summary>
        /// Lấy về tất cả các Provider
        /// </summary>
        /// <returns></returns>
        IList ProviderGetAll();

        IList ProviderGetAll(ProviderType type);

        /// <summary>
        /// Tìm kiếm Provider
        /// </summary>
        /// <param name="criterion">Điều kiện</param>
        /// <param name="order">Cách xắp xếp</param>
        /// <returns></returns>
        IList ProviderSearch(ICriterion criterion, Order order);

        #endregion

        #region -- Tour --

        IList TourSearch(ICriterion criterion, Order order, int count);
        IList TourSearch(ICriterion criterion, Order order, TourRegion region, int count);
        #endregion

        int UserCount();

        void SetLocationLevelChangedEventHandler(LocationLevelChangedEvent eventHandler);
        IList LocationGetByRootAndName(Location root, string name);

        IList TourRegionSearch(ICriterion criterion, Order order);
    }
}