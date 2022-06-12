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
using CMS.Core.DataAccess;
using CMS.Core.Domain;
using CMS.Modules.TourManagement.Domain;
using NHibernate.Criterion;

namespace CMS.Modules.TourManagement
{
    public class TourMenuModule: ModuleBase
    {
        private readonly ICommonDao _commonDao;
        private TourMenuType _type;
        public TourMenuModule(ICommonDao commonDao)
        {
            _commonDao = commonDao;
        }

        public IList TourRegionGetRoot()
        {
            return _commonDao.GetObjectByCriterion(typeof (TourRegion), Expression.IsNull(TourRegion.PARENT),
                                            Order.Asc(TourRegion.ORDER));
        }

        public TourRegion TourRegionGetById(int id)
        {
            return _commonDao.GetObjectById(typeof (TourRegion), id) as TourRegion;
        }

        public override void ReadSectionSettings()
        {
            base.ReadSectionSettings();
            _type = (TourMenuType)Enum.Parse(typeof(TourMenuType), Section.Settings["MENU_TYPE"].ToString());
        }

        public override string CurrentViewControlPath
        {
            get
            {
                switch (_type)
                {
                    case TourMenuType.RegionMenu:
                        return DefaultViewControlPath;
                    default:
                        return DefaultViewControlPath;
                }
            }
        }
    }

    public enum TourMenuType
    {
        RegionMenu,
        TypeMenu
    }
}
