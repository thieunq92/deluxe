using System;
using System.Web.UI.WebControls;
using CMS.Core.Util;
using CMS.Modules.TourManagement.Domain;
using CMS.Modules.TourManagement.Web.UI;
using CMS.ServerControls;
using CMS.Web.Util;

namespace CMS.Modules.TourManagement.Web
{
    public partial class Tours : TourControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Module != null)
            {
                string cssfile = Module.ThemePath + "tour.css";
                RegisterStylesheet("tourcss", cssfile);
            }
            if (!IsPostBack)
            {
                rptTours.DataSource = Module.TourSearchByQueryString(Request.QueryString);
                rptTours.DataBind();
            }
        }

        protected void rptTours_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            #region --List Image--
            {
                using (Image imageListItem = e.Item.FindControl("imageListItem") as Image)
                {
                    if (imageListItem != null)
                    {
                        imageListItem.ImageUrl = Module.ThemePath + "Images/list.gif";
                    }
                }
            }
            #endregion

            Tour tour = e.Item.DataItem as Tour;
            if (tour==null)
            {
                return;
            }

            HyperLink hyperLinkTourName = e.Item.FindControl("hyperLinkTourName") as HyperLink;
            if (hyperLinkTourName!=null)
            {
                hyperLinkTourName.Text = tour.Name;
                hyperLinkTourName.NavigateUrl = tour.GetUrl(Module.Section);
            }

            Image imageTour = e.Item.FindControl("imageTour") as Image;
            if (imageTour!=null)
            {
                if (string.IsNullOrEmpty(tour.Image))
                {
                    imageTour.ImageUrl = GlobalConst.NO_IMAGE;
                }
                else
                {
                    imageTour.ImageUrl = tour.Image;
                }
            }

            Label labelDescription = e.Item.FindControl("labelDescription") as Label;
            if (labelDescription!=null)
            {
                labelDescription.Text = Text.TruncateText(tour.Summary,300);
            }

            Label labelLocation = e.Item.FindControl("labelLocation") as Label;
            if (labelLocation!=null)
            {
                if (tour.StartFrom!=null && tour.EndIn!=null)
                labelLocation.Text = "From " + tour.StartFrom.Name + " to " + tour.EndIn.Name;
            }

            Label labelDuration = e.Item.FindControl("labelDuration") as Label;
            if (labelDuration!=null)
            {                                
                if (tour.IsHalf)
                {
                    labelDuration.Text = "Duration: ½ day";
                }
                else
                {
                    if (tour.NumberOfDay > 0)
                    {
                        labelDuration.Text = "Duration: " + tour.NumberOfDay + " days";
                    }
                    else
                    {
                        labelDuration.Text = "Duration: " + tour.NumberOfDay + " day";
                    }
                }
            }

            Label labelTourType = e.Item.FindControl("labelTourType") as Label;
            if(labelTourType!=null)
            {
                if (tour.TourType != null)
                {
                    labelTourType.Text = tour.TourType.Name;
                }
            }

            Label labelTourGrade = e.Item.FindControl("labelTourGrade") as Label;
            if (labelTourGrade!=null)
            {
                labelTourGrade.Text = "Grade: " + tour.Grade;
            }
        }

        protected void pagerTours_PageChanged(object sender, PageChangedEventArgs e)
        {
            rptTours.DataSource = Module.TourSearchByQueryString(Request.QueryString);
            rptTours.DataBind();
        }
    }
}