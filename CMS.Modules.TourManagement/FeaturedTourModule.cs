using System;
using System.Collections;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CMS.Core.Communication;
using CMS.Core.DataAccess;
using CMS.Core.Domain;
using CMS.Modules.TourManagement.Domain;
using NHibernate.Criterion;

namespace CMS.Modules.TourManagement
{
    public class FeaturedTourModule: ModuleBase, IActionProvider
    {
        private readonly ICommonDao _commonDao;
        private int _numberOfTours;

        public Section TourSection
        {
            get { return Section.Connections["TourDetail"] as Section; }
        }
        public FeaturedTourModule(ICommonDao commonDao)
        {
            _commonDao = commonDao;
        }

        public ActionCollection GetOutboundActions()
        {
            ActionCollection outBoundCollection = new ActionCollection();
            outBoundCollection.Add(new CMS.Core.Communication.Action("TourDetail",new string[0]));
            return outBoundCollection;
        }

        public IList GetTours()
        {
            ICriterion criterion = Expression.And(Expression.Eq(Tour.DELETED,false),Expression.Gt(Tour.FEATURED, 0));
            ICriterion package = Expression.Or(Expression.Eq(Tour.PACKAGESTATUS, PackageStatus.FullPackage),Expression.Eq(Tour.PACKAGESTATUS,PackageStatus.Both));
            package = Expression.Or(Expression.IsNull(Tour.PACKAGESTATUS), package);
            return _commonDao.GetObjectByCriterionPaged(typeof (Tour), Expression.And(criterion, package), 0, _numberOfTours,
                                                        Order.Desc(Tour.FEATURED));

        }

        public override void ReadSectionSettings()
        {
            base.ReadSectionSettings();
            _numberOfTours = Convert.ToInt32(Section.Settings["NUMBER_OF_TOURS"]);
        }
    }
}
