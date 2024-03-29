using System;
using System.Collections;
using Castle.Facilities.NHibernateIntegration;
using Castle.Services.Transaction;
using CMS.Core.Domain;
using CMS.Modules.Gallery.Domain;
using CMS.Modules.TourManagement.Domain;
using NHibernate;
using NHibernate.Criterion;

namespace CMS.Modules.TourManagement.DataAccess
{
    public class TourDao : ITourDao
    {
        private readonly ISessionManager _session;

        public TourDao(ISessionManager session)
        {
            _session = session;
        }

        #region -- Event catcher --

        private event LocationLevelChangedEvent LocationLevelChanged;

        public void OnLocationLevelChanged(Location location)
        {
            if (LocationLevelChanged != null)
            {
                LocationLevelChanged(location);
            }
        }

        #endregion

        #region -- Location --

        public IList LocationGetByParentID(int parentID)
        {
            ICriteria criteria =
                _session.OpenSession().CreateCriteria(typeof(Location)).Add(Expression.Eq("Parent.Id", parentID));
            return criteria.List();
        }

        /// <summary>
        /// Lấy danh sách toàn bộ các địa điểm
        /// </summary>
        /// <returns></returns>
        public IList LocationGetAll()
        {
            ICriteria criteria =
                _session.OpenSession().CreateCriteria(typeof (Location))
                    .AddOrder(Order.Asc("Order"));
            return criteria.List();
        }

        /// <summary>
        /// Lấy tất cả các location mà không có cha.
        /// </summary>
        /// <returns></returns>
        public IList LocationGetRoot()
        {
            ICriteria criteria =
                _session.OpenSession().CreateCriteria(typeof (Location))
                    .Add(Expression.IsNull("Parent"))
                    .AddOrder(Order.Asc("Order"));
            return criteria.List();
        }

        [Transaction(TransactionMode.RequiresNew)]
        public void Resort(Location NewlyLocation, bool up)
        {
            IList reordered;
            if (up)
            {
                reordered = _session.OpenSession().CreateCriteria(typeof (Location))
                    .Add(Expression.Ge("Order", NewlyLocation.Order)).List();
            }
            else
            {
                reordered = _session.OpenSession().CreateCriteria(typeof (Location))
                    .Add(Expression.Le("Order", NewlyLocation.Order)).List();
            }
            foreach (Location location in reordered)
            {
                if (up)
                {
                    location.Order += 1;
                }
                else
                {
                    location.Order -= 1;
                }
                _session.OpenSession().Update(location);
            }
            _session.OpenSession().Flush();
        }

        public IList LocationGetByRootAndName(Location root,string name)
        {
            ICriteria criteria = _session.OpenSession().CreateCriteria(typeof (Location)).CreateAlias(Location.PARENT,"parent");
            ICriterion criterion = Expression.Eq(Location.PARENT, root);
            criterion = Expression.Or(criterion, Expression.Eq("parent." + Location.PARENT, root));
            if (!string.IsNullOrEmpty(name))
            {
                criterion = Expression.And(criterion, Expression.Like("Name", name, MatchMode.Anywhere));
            }
            criteria.Add(criterion);
            return criteria.List();
        }

        #endregion

        #region -- Agency --

        /// <summary>
        /// Lấy danh sách toàn bộ các kiểu đại lý
        /// </summary>
        /// <returns></returns>
        public IList AgencyPolicyGetAll(string moduleType)
        {
            ICriteria criteria =
                _session.OpenSession().CreateCriteria(typeof (AgencyPolicy))
                    .Add(Expression.Eq("ModuleType", moduleType))
                    .AddOrder(Order.Asc("Name"));
            return criteria.List();
        }

        /// <summary>
        /// Lấy về đại lý theo role cho trước
        /// </summary>
        /// <param name="role">Role đại lý lấy về</param>
        /// <param name="moduleType"></param>
        /// <returns>AgencyPolicy</returns>
        public IList AgencyPolicyGetByRole(Role role, string moduleType)
        {
            ICriteria criteria =
                _session.OpenSession().CreateCriteria(typeof (AgencyPolicy))
                    .Add(Expression.Eq("Role", role))
                    .Add(Expression.Eq("ModuleType", moduleType))
                    .AddOrder(Order.Asc("CostFrom"));
            return criteria.List();
        }

        /// <summary>
        /// Lấy tất cả các Role có AgencyPolicy;
        /// </summary>
        /// <returns></returns>
        public IList RoleAgencyPolicyGetAll(string moduleType)
        {
            ICriteria criteria = _session.OpenSession().CreateCriteria(typeof (AgencyPolicy))
                .Add(Expression.Eq("ModuleType", moduleType))
                .SetProjection(Projections.Distinct(Projections.Property("Role")));
            return criteria.List();
        }

        public double RoleGetPercentage(Role role, string moduleType)
        {
            ICriteria criteria = _session.OpenSession().CreateCriteria(typeof (AgencyPolicy))
                .Add(Expression.Eq("ModuleType", moduleType))
                .SetProjection(Projections.Property("Percentage"))
                .Add(Expression.Eq("Role", role))
                .AddOrder(Order.Desc("Percentage"))
                .SetMaxResults(1);
            IList list = criteria.List();
            if (list.Count > 0)
            {
                return (double) list[0];
            }
            if (role.Id == 4)
            {
                return 100;
            }
            return RoleGetPercentage((Role) _session.OpenSession().Load(typeof (Role), 4), moduleType);
        }

        public IList AlbumGetByName(string name)
        {
            return _session.OpenSession().CreateCriteria(typeof (Album))
                .Add(Expression.Like("Title", name, MatchMode.Anywhere))
                .List();
        }

        #endregion

        #region -- Currency --

        public IList CurrencyGetAll()
        {
            return _session.OpenSession().CreateCriteria(typeof (Currency)).Add(Expression.Eq("Deleted", false)).List();
        }

        [Transaction(TransactionMode.Requires)]
        public void Delete(Currency currency)
        {
            currency.Deleted = true;
            _session.OpenSession().Update(currency);
        }

        #endregion

        #region -- Provider --

        /// <summary>
        /// Lấy về tất cả các provider
        /// </summary>
        /// <returns></returns>
        public IList ProviderGetAll()
        {
            ICriteria criteria =
                _session.OpenSession().CreateCriteria(typeof (Provider)).Add(Expression.Eq(Provider.DELETED, false));
            return criteria.List();
        }

        public IList ProviderGetAll(ProviderType type)
        {
            ICriteria criteria =
                _session.OpenSession().CreateCriteria(typeof (Provider)).Add(Expression.Eq(Provider.DELETED, false));
            if (type!=ProviderType.Mixed)
            {
                criteria.Add(Expression.Or(Expression.Eq(Provider.PROVIDERTYPE, type),
                                           Expression.Eq(Provider.PROVIDERTYPE, ProviderType.Mixed)));
            }
            else
            {
                criteria.Add(Expression.Eq(Provider.PROVIDERTYPE, type));
            }
            return criteria.List();
        }

        /// <summary>
        /// Tìm kiếm provider
        /// </summary>
        /// <param name="criterion">Điều kiện</param>
        /// <param name="order">Cách xắp xếp</param>
        /// <returns></returns>
        public IList ProviderSearch(ICriterion criterion, Order order)
        {
            ICriteria criteria = _session.OpenSession().CreateCriteria(typeof (Provider));
            if (criterion != null)
            {
                criteria.Add(criterion);
            }

            if (order != null)
            {
                criteria.AddOrder(order);
            }
            return criteria.List();
        }

        #endregion

        #region -- Tour --

        public IList TourSearch(ICriterion criterion, Order order, int count)
        {
            ICriteria criteria = _session.OpenSession().CreateCriteria(typeof (Tour));
            if (criterion!=null)
            {
                criteria.Add(criterion);
            }

            if (order!=null)
            {
                criteria.AddOrder(order);
            }

            if (count > 0)
            {
                criteria.SetMaxResults(count);
            }

            return criteria.List();
        }

        public IList TourSearch(ICriterion criterion, Order order, TourRegion region, int count)
        {
            ICriteria criteria = _session.OpenSession().CreateCriteria(typeof(Tour));

            if (region!=null)
            {
                if (criterion != null)
                {
                    criterion = Expression.And(Expression.Eq(Tour.TOURREGION, region),criterion);
                }
                else
                {
                    criterion = Expression.Eq(Tour.TOURREGION, region);
                }

                if (region.Children.Count > 0)
                {
                    criteria.CreateAlias(Tour.TOURREGION, "region");
                    criterion = Expression.Or(criterion, Expression.Eq("region." + TourRegion.PARENT,
                                                                       region));
                }
            }

            if (criterion != null)
            {
                criteria.Add(criterion);
            }


            if (order != null)
            {
                criteria.AddOrder(order);
            }

            if (count > 0)
            {
                criteria.SetMaxResults(count);
            }

            return criteria.List();
        }

        public IList TourRegionSearch(ICriterion criterion, Order order)
        {
            ICriteria criteria = _session.OpenSession().CreateCriteria(typeof(TourRegion));
            if (criterion != null)
            {
                criteria.Add(criterion);
            }

            if (order != null)
            {
                criteria.AddOrder(order);
            }

            return criteria.List();
        }

        #endregion

        #region ITourDao Members

        public int UserCount()
        {
            ICriteria criteria = _session.OpenSession().CreateCriteria(typeof (User))
                .SetProjection(Projections.RowCount());
            try
            {
                return Convert.ToInt32(criteria.List()[0]);
            }
            catch
            {
                return 0;
            }
        }

        public void SetLocationLevelChangedEventHandler(LocationLevelChangedEvent eventHandler)
        {
            if (eventHandler != null)
            {
                LocationLevelChanged = eventHandler;
            }
        }

        #endregion
    }
}