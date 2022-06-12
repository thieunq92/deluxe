using CMS.Web.UI;

namespace CMS.Modules.TourManagement.Web.UI
{
    public class TourControlBase : BaseModuleControl
    {
        public new TourManagementModule Module
        {
            get { return base.Module as TourManagementModule; }
            set { base.Module = value; }
        }
    }
}