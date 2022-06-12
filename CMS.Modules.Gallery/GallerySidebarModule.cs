using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CMS.Core.Domain;
using CMS.Core.Service.SiteStructure;

namespace CMS.Modules.Gallery
{
    public class GallerySidebarModule : ModuleBase
    {
        private ISectionService _sectionService;
        private int _appendandGallerySectionId;

        public ISectionService ISectionService { get { return _sectionService; } }
        public int AppendandGallerySectionId{ get { return _appendandGallerySectionId; } }

        public int AlbumId;
        
        public GallerySidebarModule(ISectionService sectionService)
        {
            _sectionService = sectionService;
        }

        protected override void ParsePathInfo()
        {
            base.ParsePathInfo();
            if (ModuleParams.Length > 0)
            {
                AlbumId = Convert.ToInt32(ModuleParams[1]);
            }
            else
            {
                AlbumId = 1;
            }
        }

        public override void ReadSectionSettings()
        {
            base.ReadSectionSettings();
            //_appendandGallerySectionId = Convert.ToInt32(base.Section.Settings["APPENDAND_GALLERY"]);
        }

    }
}
