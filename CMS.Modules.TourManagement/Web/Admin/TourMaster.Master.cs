using System;
using System.Web.UI;

namespace CMS.Modules.TourManagement.Web.Admin
{
    public partial class TourMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HyperLinkHomePage.Text = Resources.HyperLinkHomePage;

            #region -- Location --

            HyperLinkLocationManagement.Text = Resources.HyperLinkLocationManagement;
            hyperLinkAddNewLocation.NavigateUrl = string.Format("/Modules/TourManagement/Admin/PositionEdit.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            hyperLinkAddNewLocation.Text = Resources.hyperLinkAddNewLocation;
            hyperLinkLocationList.NavigateUrl = string.Format("/Modules/TourManagement/Admin/PositionList.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            hyperLinkLocationList.Text = Resources.hyperLinkLocationList;

            hyperLinkProviderEdit.NavigateUrl = string.Format("/Modules/TourManagement/Admin/ProviderEdit.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            hyperLinkProviderEdit.Text = Resources.hyperLinkProviderEdit;
            if (string.IsNullOrEmpty(hyperLinkProviderList.NavigateUrl))
            {
                hyperLinkProviderList.NavigateUrl =
                    string.Format("/Modules/TourManagement/Admin/ProviderList.aspx?NodeId={0}&SectionId={1}",
                                  Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            }
            hyperLinkProviderList.Text = Resources.hyperLinkProviderList;
            hplCurrencies.NavigateUrl = string.Format("/Modules/TourManagement/Admin/Currencies.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            hplCurrencies.Text = Resources.hplCurrencies;

            #endregion

            #region -- Hotel --

            HyperLinkHotelManagement.Text = Resources.HyperLinkHotelManagement;
            hyperLinkAddNewHotel.NavigateUrl = string.Format("/Modules/TourHotel/Admin/HotelEdit.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            hyperLinkAddNewHotel.Text = Resources.hyperLinkAddNewHotel;
            hyperLinkHotelList.NavigateUrl = string.Format("/Modules/TourHotel/Admin/HotelList.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            hyperLinkHotelList.Text = Resources.hyperLinkHotelList;
            hyperLinkHotelLogList.NavigateUrl = string.Format("/Modules/TourHotel/Admin/HotelLogList.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            hyperLinkHotelLogList.Text = Resources.hyperLinkHotelLogList;
            hyperLinkRoomTypes.NavigateUrl = string.Format("/Modules/TourHotel/Admin/RoomTypes.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            hyperLinkRoomTypes.Text = Resources.hyperLinkRoomTypes;
            hyperLinkAdminComments.NavigateUrl = string.Format("/Modules/TourHotel/Admin/AdminComments.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            hyperLinkAdminComments.Text = Resources.hyperLinkAdminComments;
            hyperLinkPriceList.NavigateUrl = string.Format("/Modules/TourHotel/Admin/HotelPriceList.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            hyperLinkPriceList.Text = Resources.hyperLinkPriceList;
            hyperLinkAdminOrders.NavigateUrl = string.Format("/Modules/TourHotel/Admin/AdminOrders.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            hyperLinkAdminOrders.Text = Resources.hyperLinkAdminOrders;

            #endregion

            #region -- Landscape --

            hyperLinkLandscapeManagement.Text = Resources.hyperLinkLandscapeManagement;
            hyperLinkLandscapeEdit.NavigateUrl = string.Format("/Modules/Landscape/Admin/LandscapeEdit.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            hyperLinkLandscapeEdit.Text = Resources.hyperLinkLandscapeEdit;
            hyperLinkLandscapeList.NavigateUrl = string.Format("/Modules/Landscape/Admin/LandscapeList.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            hyperLinkLandscapeList.Text = Resources.hyperLinkLandscapeList;
            hyperLinkEntranceFeeTypes.NavigateUrl = string.Format("/Modules/Landscape/Admin/EntranceFeeTypes.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            hyperLinkEntranceFeeTypes.Text = Resources.hyperLinkEntranceFeeTypes;
            hplLandscapeComment.NavigateUrl = string.Format("/Modules/Landscape/Admin/AdminComments.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            hplLandscapeComment.Text = Resources.hplLandscapeComment;

            #endregion

            #region -- Restaurant --

            hyperLinkRestaurantManagement.Text = Resources.hyperLinkRestaurantManagement;
            hyperLinkRestaurantEdit.NavigateUrl = string.Format("/Modules/Restaurant/Admin/RestaurantEdit.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            hyperLinkRestaurantEdit.Text = Resources.hyperLinkRestaurantEdit;
            hyperLinkRestaurantList.NavigateUrl = string.Format("/Modules/Restaurant/Admin/RestaurantList.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            hyperLinkRestaurantList.Text = Resources.hyperLinkRestaurantList;
            hyperLinkMealEdit.NavigateUrl = string.Format("/Modules/Restaurant/Admin/MealEdit.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            hyperLinkMealEdit.Text = Resources.hyperLinkMealEdit;
            hyperLinkMealList.NavigateUrl = string.Format("/Modules/Restaurant/Admin/MealList.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            hyperLinkMealList.Text = Resources.hyperLinkMealList;

            hyperLinkCuisineEdit.Text = Resources.hyperLinkCuisineEdit;
            hyperLinkCuisineEdit.NavigateUrl = string.Format("/Modules/Restaurant/Admin/CuisineEdit.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            hyperLinkRestaurantComments.Text = Resources.hyperLinkRestaurantComments;
            hyperLinkRestaurantComments.NavigateUrl = string.Format("/Modules/Restaurant/Admin/AdminComments.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            hplRestaurantOrders.Text = Resources.hplRestaurantOrders;
            hplRestaurantOrders.NavigateUrl = string.Format("/Modules/Restaurant/Admin/AdminOrders.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);

            #endregion

            #region -- Transport --
            hyperLinkTransportEdit.NavigateUrl = string.Format("/Modules/Transport/Admin/TransportEdit.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            hyperLinkTransportEdit.Text = Resources.hyperLinkTransportEdit;
            hyperLinkTransportList.NavigateUrl = string.Format("/Modules/Transport/Admin/TransportList.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            hyperLinkTransportList.Text = Resources.hyperLinkTransportList;
            hyperLinkRouteEdit.NavigateUrl = string.Format("/Modules/Transport/Admin/TransportRouteEdit.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            hyperLinkRouteEdit.Text = Resources.hyperLinkRouteEdit;
            hyperLinkRouteList.NavigateUrl = string.Format("/Modules/Transport/Admin/TransportRouteList.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            hyperLinkRouteList.Text = Resources.hyperLinkRouteList;
            hyperLinkTransportTypeSetting.NavigateUrl = string.Format("/Modules/Transport/Admin/TransportTypeSetting.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            hyperLinkTransportTypeSetting.Text = Resources.hyperLinkTransportTypeSetting;
            hyperLinkTransportLogList.NavigateUrl = string.Format("/Modules/Transport/Admin/TransportLogList.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            hyperLinkTransportLogList.Text = Resources.hyperLinkTransportLogList;
            hyperLinkRouteCategories.NavigateUrl = string.Format("/Modules/Transport/Admin/RouteCategories.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            #endregion

            #region -- Guide --

            hyperLinkGuideManagement.Text = Resources.hyperLinkGuideManagement;
            hyperLinkGuideEdit.NavigateUrl = string.Format("/Modules/TourGuide/Admin/GuideEdit.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            hyperLinkGuideEdit.Text = Resources.hyperLinkGuideEdit;
            hyperLinkGuideList.NavigateUrl = string.Format("/Modules/TourGuide/Admin/GuideList.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            hyperLinkGuideList.Text = Resources.hyperLinkGuideList;
            hyperLinkGuideServices.NavigateUrl = string.Format("/Modules/TourGuide/Admin/Services.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            hyperLinkGuideServices.Text = Resources.hyperLinkGuideServices;
            hyperLinkGuideRate.NavigateUrl = string.Format("/Modules/TourGuide/Admin/GuideRate.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            hyperLinkGuideRate.Text = Resources.hyperLinkGuideRate;
            hyperLinkGuideAdministrator.NavigateUrl = string.Format("/Modules/TourGuide/Admin/GuideAdministrator.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            hyperLinkGuideAdministrator.Text = Resources.hyperLinkGuideAdministrator;
            hyperLinkGuideLanguagesSetting.NavigateUrl = string.Format("/Modules/TourGuide/Admin/Languages.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            hyperLinkGuideLanguagesSetting.Text = Resources.hyperLinkGuideLanguagesSetting;
            hyperLinkAddGuideRate.NavigateUrl = string.Format("/Modules/TourGuide/Admin/AddGuideRates.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            hyperLinkAddGuideRate.Text = Resources.hyperLinkAddGuideRate;

            #endregion

            #region -- Boat --
            hyperLinkBoatManagement.Text = Resources.hyperLinkBoatManagement;
            hyperLinkBoatEdit.NavigateUrl = string.Format("/Modules/TourBoat/Admin/BoatEdit.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            hyperLinkBoatEdit.Text = Resources.hyperLinkBoatEdit;
            hyperLinkBoatList.NavigateUrl = string.Format("/Modules/TourBoat/Admin/BoatList.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            hyperLinkBoatList.Text = Resources.hyperLinkBoatList;
            hyperLinkBoatRoomTypes.NavigateUrl = string.Format("/Modules/TourBoat/Admin/RoomTypes.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            hyperLinkBoatRoomTypes.Text = Resources.hyperLinkBoatRoomTypes; string.Format("/Modules/TourBoat/Admin/RoomTypes.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            hyperLinkBoatAdministrator.NavigateUrl = string.Format("/Modules/TourBoat/Admin/BoatAdministrator.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            hyperLinkBoatAdministrator.Text = Resources.hyperLinkBoatAdministrator;
            //hyperLinkBoatTripTourEdit.NavigateUrl = string.Format("/Modules/TourBoat/Admin/BoatTripTourEdit.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            //hyperLinkBoatTripTourEdit.Text = Resources.hyperLinkBoatTripTourEdit;
            //hyperLinkBoatTripTourList.NavigateUrl = string.Format("/Modules/TourBoat/Admin/BoatTripList.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            //hyperLinkBoatTripTourList.Text = Resources.hyperLinkBoatTripTourList;
            hyperLinkBoatTypes.NavigateUrl = string.Format("/Modules/TourBoat/Admin/BoatTypes.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            hyperLinkBoatTypes.Text = Resources.hyperLinkBoatTypes;
            hyperLinkBoatCategories.NavigateUrl = string.Format("/Modules/TourBoat/Admin/BoatCategories.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            hyperLinkBoatCategories.Text = Resources.hyperLinkBoatCategories;
            #endregion

            #region -- Agency --

            HyperLinkPricingManagement.Text = Resources.HyperLinkPricingManagement;
            hyperLinkAgentList.Text = Resources.hyperLinkAgentList;

            #endregion

            #region -- Tour --
            hyperLinkTourEdit.NavigateUrl = string.Format("/Modules/TourManagement/Admin/TourEdit.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            hyperLinkTourList.NavigateUrl = string.Format("/Modules/TourManagement/Admin/TourList.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);

            hyperLinkTourTypes.NavigateUrl = string.Format("/Modules/TourManagement/Admin/TourTypes.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            hyperLinkTourTypes.Text = Resources.hyperLinkTourTypes;

            hyperLinkTourRegions.NavigateUrl = string.Format("/Modules/TourManagement/Admin/TourRegions.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            hyperLinkTourRegions.Text = Resources.hyperLinkTourRegions;
            #endregion

            hyperLinkAddArticles.NavigateUrl = string.Format("/Modules/TourOthers/Admin/ArticlesEdit.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            hyperLinkArticles.NavigateUrl = string.Format("/Modules/TourOthers/Admin/ArticlesList.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
            hyperLinkCategory.NavigateUrl = string.Format("/Modules/TourOthers/Admin/Categories.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
        }
    }
}