using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Castle.Facilities.NHibernateIntegration;
using Castle.Services.Transaction;
using CMS.Core.Domain;
using log4net;
using NHibernate;
using NHibernate.Criterion;

namespace CMS.Core.DataAccess
{
    /// <summary>
    /// Functionality for common simple data access. The class uses NHibernate.
    /// </summary>
    [Transactional]
    public class CommonDao : ICommonDao
    {
        private readonly ISessionManager _sessionManager;
        private readonly ILog logger = LogManager.GetLogger(typeof (CommonDao));

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="sessionManager"></param>
        public CommonDao(ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        #region ICommonDao Members

        public object GetObjectById(Type type, int id)
        {
            ISession session = _sessionManager.OpenSession();
            return session.Load(type, id);
        }

        public object GetObjectById(Type type, int id, bool allowNull)
        {
            if (! allowNull)
            {
                return GetObjectById(type, id);
            }
            ISession session = _sessionManager.OpenSession();
            return session.Get(type, id);
        }

        public object GetObjectByDescription(Type type, string propertyName, string description)
        {
            ISession session = _sessionManager.OpenSession();
            ICriteria crit = session.CreateCriteria(type);
            crit.Add(Expression.Eq(propertyName, description));
            return crit.UniqueResult();
        }

        public IList GetObjectByProperty(Type type, string propertyName, object value)
        {
            ISession session = _sessionManager.OpenSession();
            ICriteria crit = session.CreateCriteria(type);
            crit.Add(Expression.Eq(propertyName, value));
            return crit.List();
        }

        public IList GetAll(Type type)
        {
            return GetAll(type, null);
        }

        public IList GetAll(Type type, params string[] sortProperties)
        {
            ISession session = _sessionManager.OpenSession();

            ICriteria crit = session.CreateCriteria(type);
            if (sortProperties != null)
            {
                foreach (string sortProperty in sortProperties)
                {
                    crit.AddOrder(Order.Asc(sortProperty));
                }
            }
            return crit.List();
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void SaveOrUpdateObject(object obj)
        {
            ISession session = _sessionManager.OpenSession();
            session.FlushMode = FlushMode.Commit;
            session.SaveOrUpdate(obj);
        }


        [Transaction(TransactionMode.Requires)]
        public virtual void SaveObject(object obj)
        {
            ISession session = _sessionManager.OpenSession();
            session.Save(obj);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateObject(object obj)
        {
            ISession session = _sessionManager.OpenSession();
            session.Update(obj);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteObject(object obj)
        {
            ISession session = _sessionManager.OpenSession();
            session.Delete(obj);
        }

        public void MarkForDeletion(object obj)
        {
            ISession session = _sessionManager.OpenSession();
            session.Delete(obj);
        }

        public int Count(Type type)
        {
            ISession session = _sessionManager.OpenSession();
            session.FlushMode = FlushMode.Commit;
            ICriteria criteria = session.CreateCriteria(type).SetProjection(Projections.RowCount());
            return Convert.ToInt32(criteria.List()[0]);
        }

        #endregion

        public IList GetObjectByCriterion(Type objectType, ICriterion criterion, params Order[] orders)
        {
            try
            {
                ISession session = _sessionManager.OpenSession();
                session.FlushMode = FlushMode.Commit;
                ICriteria criteria = session.CreateCriteria(objectType);
                if (criterion != null)
                {
                    criteria.Add(criterion);
                }
                if (orders.Length > 0)
                {
                    foreach (Order order in orders)
                    {
                        if (order != null)
                        {
                            criteria.AddOrder(order);
                        }
                    }
                }
                return criteria.List();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message,ex);
                return null;
            }
        }

        public IList GetObjectByCriterionPaged(Type objectType, ICriterion criterion, int pageIndex,
                                        int pageSize, params Order[] orders)
        {
            try
            {
                if (pageIndex < 0)
                {
                    pageIndex = 0;
                }

                ISession session = _sessionManager.OpenSession();
                session.FlushMode = FlushMode.Commit;
                ICriteria criteria = session.CreateCriteria(objectType);
                if (criterion != null)
                {
                    criteria.Add(criterion);
                }
                if (orders.Length > 0)
                {
                    foreach (Order order in orders)
                    {
                        if (order != null)
                        {
                            criteria.AddOrder(order);
                        }
                    }
                }

                if (pageSize > 0)
                {
                    criteria.SetMaxResults(pageSize);
                    criteria.SetFirstResult(pageSize * pageIndex);
                }

                return criteria.List();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                return null;
            }
        }

        public int CountObjectByCriterion(Type objectType, ICriterion criterion)
        {
            try
            {
                ISession session = _sessionManager.OpenSession();
                session.FlushMode = FlushMode.Commit;
                ICriteria criteria = session.CreateCriteria(objectType);
                if (criterion != null)
                {
                    criteria.Add(criterion);
                }
                criteria.SetProjection(Projections.RowCount());
                return (int) criteria.List()[0];
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                return 0;
            }
        }

        public IList PermissionsGetByRole(ModuleType module, Role role)
        {
            ICriteria criteria = _sessionManager.OpenSession().CreateCriteria(typeof (SpecialPermission));
            ICriterion criterion = Expression.And(Expression.Eq("Role", role),
                                                  Expression.Eq("ModuleType", module));
            criteria.Add(criterion);
            criteria.SetProjection(Projections.Property("Name"));
            return criteria.List();
        }

        public SpecialPermission PermissionGetByRole(string name, Role role)
        {
            ICriteria criteria = _sessionManager.OpenSession().CreateCriteria(typeof(SpecialPermission));
            ICriterion criterion = Expression.And(Expression.Eq("Role", role),
                                                  Expression.Eq("Name", name));
            criteria.Add(criterion);
            IList list = criteria.List();
            if (list.Count > 0)
            {
                return list[0] as SpecialPermission;
            }
            return null;
        }

        public SpecialPermission PermissionGetByUser(string name, User user)
        {
            ICriteria criteria = _sessionManager.OpenSession().CreateCriteria(typeof(SpecialPermission));
            ICriterion criterion = Expression.And(Expression.Eq("User", user),
                                                  Expression.Eq("Name", name));
            criteria.Add(criterion);
            IList list = criteria.List();
            if (list.Count > 0)
            {
                return list[0] as SpecialPermission;
            }
            return null;
        }

        public IList PermissionsGetByUserRole(ModuleType module, User user)
        {
            ICriteria criteria = _sessionManager.OpenSession().CreateCriteria(typeof(SpecialPermission));
            criteria.CreateAlias("Role", "role");
            criteria.CreateAlias("role.Users", "user");
            ICriterion criterion = Expression.And(Expression.Eq("user.Id", user.Id),
                                                  Expression.Eq("ModuleType", module));
            criteria.Add(criterion);
            criteria.SetProjection(Projections.Distinct(Projections.Property("Name")));
            return criteria.List();
        }

        public IList PermissionsGetByUser(ModuleType module, User user)
        {
            ICriteria criteria = _sessionManager.OpenSession().CreateCriteria(typeof(SpecialPermission));
            ICriterion criterion = Expression.And(Expression.Eq("User", user),
                                                  Expression.Eq("ModuleType", module));
            criteria.Add(criterion);
            criteria.SetProjection(Projections.Property("Name"));
            return criteria.List();
        }

        public bool PermissionCheck(string name, User user)
        {
            bool isGranted = false;

            // Check permission của role của user
            ICriteria criteria = _sessionManager.OpenSession().CreateCriteria(typeof(SpecialPermission));
            criteria.CreateAlias("Role", "role");
            criteria.CreateAlias("role.Users", "user");
            ICriterion criterion = Expression.And(Expression.Eq("user.Id", user.Id),
                                                  Expression.Eq("Name", name));
            criteria.Add(criterion);
            criteria.SetProjection(Projections.RowCount());
            int count = (int) criteria.List()[0];

            // Check permission của user
            ICriteria criteria2 = _sessionManager.OpenSession().CreateCriteria(typeof(SpecialPermission));
            ICriterion criterion2 = Expression.And(Expression.Eq("User", user),
                                                  Expression.Eq("Name", name));
            criteria2.Add(criterion2);
            criteria2.SetProjection(Projections.RowCount());
            int count2 = (int)criteria2.List()[0];
            isGranted = count + count2 > 0;

            if (isGranted)
            {
                return true;
            }

            // Check có tồn tại permission đó không, nếu không tồn tại thì luôn là granted
            ICriteria pCriteria = _sessionManager.OpenSession().CreateCriteria(typeof (ModulePermission));
            pCriteria.Add(Expression.Eq("Name", name));
            pCriteria.SetProjection(Projections.RowCount());
            int p = (int) pCriteria.List()[0];
            return p == 0; // Nếu không tồn tại thì là granted, nếu tồn tại thì đương nhiên là không granted
        }

        public ISession OpenSession()
        {
            return this._sessionManager.OpenSession();
        }

        #region -- NEW WAY TO ACCESS --

        private const int PageSize = 100;

        [Transaction(TransactionMode.Requires)]
        public IList<T> GetObject<T>(ICriterion criterion, int pageSize, int pageIndex, params Order[] orders)
        {
            ISession session = _sessionManager.OpenSession();
            ICriteria criteria = session.CreateCriteria(typeof(T));
            if (criterion != null)
                criteria.Add(criterion);

            if (pageSize > 0)
            {
                if (pageIndex < 0) pageIndex = 0;
                criteria.SetFirstResult(pageSize * pageIndex);
                criteria.SetMaxResults(pageSize);
            }

            if (orders != null)
            {
                foreach (Order order in orders)
                {
                    if (order != null)
                    {
                        criteria.AddOrder(order);
                    }
                }
            }

            return new BindingList<T>(criteria.List<T>());
        }

        [Transaction(TransactionMode.Requires)]
        public int CountObject<T>(ICriterion criterion)
        {
            ISession session = _sessionManager.OpenSession();
            ICriteria criteria = session.CreateCriteria(typeof(T));
            criteria.SetProjection(Projections.RowCount());
            if (criterion != null)
                criteria.Add(criterion);
            IList list = criteria.List();
            if (list.Count > 0)
            {
                return (int)list[0];
            }
            return 0;
        }

        /// <summary>
        /// Thực thi một thao tác đối với đối tượng get về (yêu cầu khai báo rõ loại đối tượng)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="criterion"></param>
        /// <param name="handler"></param>
        public void ExecuteObject<T>(ICriterion criterion, ExecuteHandler<T> handler)
        {
            int count = CountObject<T>(criterion); // Tổng số lượng
            int numPage = count / PageSize;

            if (count % PageSize > 0)
            {
                numPage++;
            }

            for (int ii = 0; ii < numPage; ii++)
            {
                IList<T> temp = GetObject<T>(criterion, PageSize, ii, null); // lấy về danh sách object tạm thời
                for (int jj = 0; jj < temp.Count; jj++)
                {
                    handler(temp[jj]); // thực thi công việc với đối tượng thứ jj
                }
            }

            // Kết thúc: đã xử lý hết các đối tượng
        }
        #endregion
    }
}