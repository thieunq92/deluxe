using System.Collections;
using CMS.Core.DataAccess;
using CMS.Core.Domain;
using CMS.Modules.TourManagement.DataAccess;
using CMS.Modules.TourManagement.Domain;
using NHibernate.Criterion;

namespace CMS.Modules.TourManagement
{
    public class TourSearchInputModule: ModuleBase
    {
        private readonly ICommonDao _commonDao;
        private readonly ITourDao _tourDao;
        public TourSearchInputModule(ICommonDao commonDao, ITourDao tourDao)
        {
            _commonDao = commonDao;
            _tourDao = tourDao;
        }

        public IList TourTypeGetAll()
        {
            return _commonDao.GetAll(typeof (TourType));
        }

        public IList TourRegionGetAll()
        {
            return _commonDao.GetAll(typeof (TourRegion));
        }

        public IList TourRegionGetAllRoot()
        {
            return _tourDao.TourRegionSearch(Expression.IsNull(TourRegion.PARENT), Order.Asc(TourRegion.ORDER));
        }

        public IList TourTypeGetAllRoot()
        {
            return _commonDao.GetObjectByCriterion(typeof(TourType), Expression.IsNull("Parent"));
        }
    }
}
