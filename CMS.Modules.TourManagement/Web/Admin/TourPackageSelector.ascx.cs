using System;
using System.Web.UI.WebControls;
using CMS.Core.Domain;
using CMS.Modules.TourManagement.Domain;
using CMS.Modules.TourManagement.Web.UI;

namespace CMS.Modules.TourManagement.Web.Admin
{
    public partial class TourPackageSelector : SelectorControl
    {
        private TourManagementModule _module;

        private Tour _tour;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["TourId"] != null && Convert.ToInt32(Request.QueryString["TourId"]) > 0)
            {
                _tour = _module.TourGetById(Convert.ToInt32(Request.QueryString["TourId"]));
                if (!IsPostBack)
                {
                    LocalizeControls();
                    GetTourConfigList();
                    GetTourList();

                    rptTourConfigs.DataBind();
                    rptTourPackages.DataBind();
                }
            }
        }

        private void GetTourList()
        {
            rptTourPackages.DataSource = _module.TourGetPartialPackage();
        }

        private void GetTourConfigList()
        {
            rptTourConfigs.DataSource = _module.TourPackageConfigGetByTour(_tour);
        }

        public override ModuleBase Module
        {
            get { return _module; }
            set { _module = value as TourManagementModule; }
        }

        protected void rptTourConfigs_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is TourPackageConfig)
            {
                TourPackageConfig config = (TourPackageConfig) e.Item.DataItem;

                Label labelName = e.Item.FindControl("labelName") as Label;
                if (labelName != null)
                {
                    labelName.Text = config.Package.Name;
                }

                Label labelProvider = e.Item.FindControl("labelProvider") as Label;
                if (labelProvider!=null)
                {
                    labelProvider.Text = config.Package.Provider.Name;
                }
            }
        }

        protected void rptTourConfigs_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName.ToLower())
            {
                case "delete":
                    _module.Delete(_module.TourPackageConfigGetById(Convert.ToInt32(e.CommandArgument)));
                    Response.Redirect(Request.RawUrl);
                    break;
            }
        }

        protected void rptTourPackages_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName.ToLower())
            {
                case "select":
                    TourPackageConfig config = new TourPackageConfig();
                    config.Tour = _tour;
                    config.Package = _module.TourGetById(Convert.ToInt32(e.CommandArgument));
                    _module.SaveOrUpdate(config);
                    GetTourConfigList();
                    rptTourConfigs.DataBind();
                    break;
            }
        }
    }
}