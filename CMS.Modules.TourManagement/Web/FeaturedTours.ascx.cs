using System;
using System.Web.UI.WebControls;
using CMS.Modules.TourManagement.Domain;
using CMS.Web.UI;

namespace CMS.Modules.TourManagement.Web
{
    public partial class FeaturedTours : BaseModuleControl
    {
        #region -- PRIVATE MEMBERS --
        private FeaturedTourModule _module;
        //private bool HasToRender;
        #endregion

        #region -- PAGE EVENTS --
        protected void Page_Load(object sender, EventArgs e)
        {
            // Kiểm tra module
            #region -- Module test --
            if (Module is FeaturedTourModule)
            {
                _module = (FeaturedTourModule)Module;
            }
            else
            {
                throw new Exception("Must be Featured Tour Module!");
            }
            #endregion

            #region -- Location test --
            //if (_module.Location == null && PageEngine.GlobalVar.ContainsKey("Location"))
            //{
            //    HasToRender = true;
            //    _module.Location = PageEngine.GlobalVar["Location"] as Location;
            //}
            //else
            //{
            //    HasToRender = false;
            //}
            #endregion

            if (!IsPostBack)
            {
                #region -- Section test --
                //if (_module.Location != null)
                //{
                //    hyperLinkLocationName.Text = _module.Location.Name;
                //}
                liNeedASection.Visible = false;
                // Kiểm tra việc kết nối section
                if (_module.TourSection != null)// && _module.Location != null)
                {
                    //hyperLinkLocationName.NavigateUrl = Hotel.GetLinkFromLocation(_module.HotelListSection, _module.Location).Replace("/hotels/", "/tophotels/");
                }
                else
                {
                    //if (_module.Location != null)
                    //{
                    //    hyperLinkLocationName.NavigateUrl = Hotel.GetLinkFromLocation(_module.Section, _module.Location).Replace("/hotels/", "/tophotels/");
                    //}
                    if (_module.TourSection == null)
                    {
                        liNeedASection.Visible = true;
                    }
                }
                #endregion

                #region -- Thiết lập link --
                //if (_module.ShowLocationName)
                //{
                //    hyperLinkLocationName.Visible = true;
                //}
                //else
                //{
                //    hyperLinkLocationName.Visible = false;
                //}

                //if (_module.Location != null)
                //{
                //    if (!_module.ShowLocationName || string.IsNullOrEmpty(_module.Location.Image))
                //    {
                //        imageLocation.Visible = false;
                //    }
                //    else
                //    {
                //        imageLocation.ImageUrl = _module.Location.Image;
                //    }
                //}
                //else
                //{
                //    imageLocation.Visible = false;
                //}

                #endregion

                if (_module.TourSection == null)
                {
                    liNeedASection.Visible = true;
                }

                rptTours.DataSource = _module.GetTours();
                rptTours.DataBind();
            }

            // Register stylesheet
            //string cssfile = _module.ThemePath + "hotel.css";
            //RegisterStylesheet("hotelcss", cssfile);
        }

        //protected override void OnPreRender(EventArgs e)
        //{
        //    base.OnPreRender(e);
        //    // Gán Link trên title bằng với link trên tên địa điểm
        //    if (HasToRender)
        //    {
        //        _module.Section.Title = string.Format(@"<a href=""{0}"">{1}</a>", hyperLinkLocationName.NavigateUrl,
        //                                             "TOP HOTELS IN " + _module.Location.Name);
        //    }
        //    else
        //    {
        //        _module.Section.Title = string.Format(@"<a href=""{0}"">{1}</a>", hyperLinkLocationName.NavigateUrl,
        //                                             Module.Section.Title);
        //    }
        //}

        #endregion

        #region -- CONTROL EVENTS --
        protected void rptTours_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Tour tour = e.Item.DataItem as Tour;
            if (tour == null)
            {
                return;
            }
            using (HyperLink hplTour = e.Item.FindControl("hplTour") as HyperLink)
            {
                if (hplTour != null)
                {
                    if (_module.TourSection != null)
                    {
                        hplTour.NavigateUrl = tour.GetUrl(_module.TourSection);
                    }
                    else
                    {
                        hplTour.NavigateUrl = tour.GetUrl(_module.Section);
                    }
                }
            }
        }
        #endregion
    }
}