using System.Collections;
using System.Data;
using CMS.Core.Domain;
using CMS.Modules.TourManagement.Domain;
using CMS.Web.Util;

namespace CMS.Modules.TourManagement
{
    public abstract class TourModuleBase: ModuleBase
    {
        protected string _selecterPath;
        public string SelecterPath
        {
            get { return _selecterPath; }
        }

        protected string _overviewPath;
        public string OverviewPath
        {
            get { return _overviewPath; }
        }

        public abstract void ExportTourConfigToDataTable(int tourid, DataTable table, IList roles, int numberOfCustomer);

        public virtual void ExportTourConfigToDataTable(int tourid, DataTable table)
        {
            return;
        }

        /// <summary>
        /// Kiểm tra trong location có dữ liệu hay không để hiển thị
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public virtual bool CheckDataExists(Location location)
        {
            return true;
        }

        public virtual string GetLinkFromLocation(Section section, Location location)
        {
            return string.Format("{0}/list?locationId={1}", UrlHelper.GetUrlFromSection(section), location.Id);
        }
    }
}
