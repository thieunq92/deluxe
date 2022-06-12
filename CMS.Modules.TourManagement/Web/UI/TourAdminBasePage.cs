using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.Core.Domain;
using CMS.Web.UI;

namespace CMS.Modules.TourManagement.Web.UI
{
    public class TourAdminBasePage : TourManagementAdminBasePage
    {
        protected override void OnPreLoad(System.EventArgs e)
        {
            base.OnPreLoad(e);
            if (Section != null)
            {
                if (Section.ModuleType.Name == "TourManagement")
                {
                    MasterPage master = Master;
                    Panel panelHotel = master.FindControl("panelHotel") as Panel;
                    if (panelHotel != null)
                    {
                        panelHotel.Visible = false;
                    }
                }

                if (Section.ModuleType.Name == "TourHotel")
                {
                    MasterPage master = Master;
                    Panel panelHotel = master.FindControl("panelHotel") as Panel;
                    if (panelHotel != null)
                    {
                        panelHotel.Visible = true;
                    }
                }
            }
        }

        public new TourManagementModule Module
        {
            get
            {
                if (base.Module is TourManagementModule)
                {
                    return (TourManagementModule) base.Module;
                }
                if (((User) Page.User.Identity).HasPermission(AccessLevel.Editor))
                {
                    _module = _moduleLoader.GetModuleFromClassName("CMS.Modules.TourManagement.TourManagementModule");
                    _module.Section = Section;
                    return (TourManagementModule) base.Module;
                }
                return null;
            }
        }
    }
}