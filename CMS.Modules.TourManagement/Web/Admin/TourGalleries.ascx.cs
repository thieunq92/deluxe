using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CMS.Core.Domain;
using CMS.Modules.TourManagement.Domain;
using CMS.Modules.TourManagement.Web.UI;
using CMS.ServerControls;

namespace CMS.Modules.TourManagement.Web.Admin
{
    public partial class TourGalleries: SelectorControl
    {
        private TourManagementModule _module;
        private Tour _tour;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request.QueryString["TourId"]))
            {
                return;
            }

            _tour = _module.TourGetById(Convert.ToInt32(Request.QueryString["TourId"]));

            if (!IsPostBack)
            {
                rptRelatedTours.DataSource = _module.RelatedTourGetByTour(_tour);
                rptRelatedTours.DataBind();
                rptTours.DataSource = _module.TourGetAll();
                rptTours.DataBind();
            }
        }

        public override ModuleBase Module
        {
            get { return _module; }
            set { _module = value as TourManagementModule; }
        }

        protected void rptTours_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is Tour)
            {
                Tour tour = (Tour) e.Item.DataItem;
                CheckBox chkTour = e.Item.FindControl("chkTour") as CheckBox;
                if (chkTour!=null)
                {
                    chkTour.Text = tour.Name;
                    chkTour.Checked = _module.CheckRelated(_tour, tour);
                }
            }
        }

        protected void pagerTours_PageChanged(object sender, PageChangedEventArgs e)
        {
            rptTours.DataSource = _module.TourGetAll();
            rptTours.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            foreach (RepeaterItem item in rptTours.Items)
            {
                HiddenField hiddenId = item.FindControl("hiddenId") as HiddenField;
                CheckBox chkTour = item.FindControl("chkTour") as CheckBox;
                if (hiddenId==null || chkTour==null)
                {
                    return;
                }

                Tour tour = _module.TourGetById(Convert.ToInt32(hiddenId.Value));
                if (chkTour.Checked)
                {
                    _module.InsertRelated(tour, _tour);
                }
                else
                {
                    _module.DeleteRelated(tour, _tour);
                }
            }
        }

        protected void rptRelatedTours_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is TourRelated)
            {
                TourRelated related = (TourRelated) e.Item.DataItem;
                Label labelTourName = e.Item.FindControl("labelTourName") as Label;
                if (labelTourName!=null)
                {
                    if (related.Related.Id == _tour.Id)
                    {
                        labelTourName.Text = related.Tour.Name;
                    }
                    else
                    {
                        labelTourName.Text = related.Related.Name;
                    }
                }
            }
        }
    }
}