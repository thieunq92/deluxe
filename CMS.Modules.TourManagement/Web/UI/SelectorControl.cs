using System.Web.UI;
using CMS.Core.Domain;
using CMS.Web.UI;

namespace CMS.Modules.TourManagement.Web.UI
{
    public abstract class SelectorControl: LocalizedUserControl
    {
        public abstract ModuleBase Module { get; set; }
    }
}
