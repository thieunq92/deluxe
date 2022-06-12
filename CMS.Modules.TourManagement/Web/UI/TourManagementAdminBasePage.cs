using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CMS.Core.Domain;
using CMS.Web.UI;
using AccessLevel=CMS.Core.Domain.AccessLevel;

namespace CMS.Modules.TourManagement.Web.UI
{
    public class TourManagementAdminBasePage : KitModuleAdminBasePage
    {
        private MasterPage master;
        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
            if (Module != null)
            {
                master = Master as MasterPage;
                string moduleName;
                if (Section.ModuleType.Name != "TourManagement")
                {
                    moduleName = Section.ModuleType.Name;
                }
                else
                {
                    moduleName = " ";
                }
                SetVisible(moduleName);
            }
            if (!IsPostBack)
            {
                ViewState["refererUrl"] = Request.UrlReferrer;
            }
            HideError();
        }

        private void SetVisible(string moduleName)
        {
            #region -- HOTEL --
            if (moduleName == "TourHotel" || moduleName== string.Empty)
            {
                Panel panelHotel = master.FindControl("panelHotel") as Panel;
                if (panelHotel != null)
                {
                    panelHotel.Visible = true;
                    Control logList = Master.FindControl("panelHotelLogList");
                    if (logList != null && ((User)(Page.User.Identity)).HasPermission(AccessLevel.Administrator))
                    {
                        logList.Visible = true;
                    }
                    
                }

                HyperLink hyperLinkAgentList = master.FindControl("hyperLinkAgentList") as HyperLink;
                if (hyperLinkAgentList!=null)
                {
                    hyperLinkAgentList.NavigateUrl =
                        string.Format("/Modules/TourManagement/Admin/AgentList.aspx?NodeId={0}&SectionId={1}&ModuleType={2}",
                                      Request.QueryString["NodeId"], Request.QueryString["SectionId"],"HOTEL");
                }

                HyperLink HyperLinkHomePage = master.FindControl("HyperLinkHomePage") as HyperLink;
                if (HyperLinkHomePage!=null)
                {
                    HyperLinkHomePage.NavigateUrl = string.Format("/Modules/TourHotel/Admin/Home.aspx?NodeId={0}&SectionId={1}",
                                      Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
                }

                HtmlControl tabHotel = Master.FindControl("tabHotel") as HtmlControl;
                if (tabHotel!=null)
                {
                    tabHotel.Attributes.Add("class", "selected");
                }
            }
            #endregion

            #region -- LANDSCAPE --
            if (moduleName == "Landscape" || moduleName == string.Empty)
            {
                Panel panelHotel = master.FindControl("panelLandscape") as Panel;
                if (panelHotel != null)
                {
                    panelHotel.Visible = true;
                    Control divLandscapeAdmin = Master.FindControl("divLandscapeAdmin");
                    if (divLandscapeAdmin != null && UserIdentity.HasPermission(AccessLevel.Administrator))
                    {
                        divLandscapeAdmin.Visible = true;
                    }
                    HyperLink hyperLinkAgentList = master.FindControl("hyperLinkAgentList") as HyperLink;
                    if (hyperLinkAgentList != null)
                    {
                        hyperLinkAgentList.NavigateUrl =
                            string.Format("/Modules/TourManagement/Admin/AgentList.aspx?NodeId={0}&SectionId={1}&ModuleType={2}",
                                          Request.QueryString["NodeId"], Request.QueryString["SectionId"], "LANDSCAPE");
                    }
                }

                HyperLink HyperLinkHomePage = master.FindControl("HyperLinkHomePage") as HyperLink;
                if (HyperLinkHomePage != null)
                {
                    HyperLinkHomePage.NavigateUrl = string.Format("/Modules/Landscape/Admin/Home.aspx?NodeId={0}&SectionId={1}",
                                      Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
                }

                HtmlControl tabHotel = Master.FindControl("tabDestination") as HtmlControl;
                if (tabHotel != null)
                {
                    tabHotel.Attributes.Add("class", "selected");
                }
            }
            #endregion

            #region -- RESTAURANT --
            if (moduleName == "Restaurant" || moduleName == string.Empty)
            {
                Panel panelHotel = master.FindControl("panelRestaurant") as Panel;
                if (panelHotel != null)
                {
                    panelHotel.Visible = true;
                    Control divRestaurantAdmin = Master.FindControl("divRestaurantAdmin");
                    if (divRestaurantAdmin!=null && UserIdentity.HasPermission(AccessLevel.Administrator))
                    {
                        divRestaurantAdmin.Visible = true;
                    }
                    HyperLink hyperLinkAgentList = master.FindControl("hyperLinkAgentList") as HyperLink;
                    if (hyperLinkAgentList != null)
                    {
                        hyperLinkAgentList.NavigateUrl =
                            string.Format("/Modules/TourManagement/Admin/AgentList.aspx?NodeId={0}&SectionId={1}&ModuleType={2}",
                                          Request.QueryString["NodeId"], Request.QueryString["SectionId"], "RESTAURANT");
                    }
                }

                HtmlControl tabHotel = Master.FindControl("tabRestaurant") as HtmlControl;
                if (tabHotel != null)
                {
                    tabHotel.Attributes.Add("class", "selected");
                }
            }
            #endregion

            #region -- TRANSPORTS --
            if (moduleName == "Transport" || moduleName == string.Empty)
            {
                Panel panelHotel = master.FindControl("panelTransport") as Panel;
                Panel panelProvider = master.FindControl("panelProvider") as Panel;
                if (panelHotel != null)
                {
                    panelHotel.Visible = true;
                    Control logList = Master.FindControl("panelTransportLogList");
                    if (logList != null && ((User)(Page.User.Identity)).HasPermission(AccessLevel.Administrator))
                    {
                        logList.Visible = true;
                    }
                }
                if (panelProvider!=null)
                {
                    panelProvider.Visible = true;

                    HyperLink hyperLinkProviderList = master.FindControl("hyperLinkProviderList") as HyperLink;
                    if (hyperLinkProviderList!=null)
                    {
                        hyperLinkProviderList.NavigateUrl =
                            string.Format("/Modules/Transport/Admin/ProviderList.aspx?NodeId={0}&SectionId={1}",
                                          Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
                    }
                }

                HyperLink hyperLinkAgentList = master.FindControl("hyperLinkAgentList") as HyperLink;
                if (hyperLinkAgentList != null)
                {
                    hyperLinkAgentList.NavigateUrl =
                        string.Format("/Modules/Transport/Admin/TransportAgencyPolicy.aspx?NodeId={0}&SectionId={1}&ModuleType={2}",
                                      Request.QueryString["NodeId"], Request.QueryString["SectionId"], "TRANSPORT");
                }

                HtmlControl tabHotel = Master.FindControl("tabTransport") as HtmlControl;
                if (tabHotel != null)
                {
                    tabHotel.Attributes.Add("class", "selected");
                }
            }
            #endregion

            #region -- GUIDE --
            if (moduleName == "TourGuide" || moduleName == string.Empty)
            {
                Panel panelHotel = master.FindControl("panelTourGuide") as Panel;
                if (panelHotel != null)
                {
                    panelHotel.Visible = true;
                    Control logList = Master.FindControl("panelGuideAdmin");
                    if (logList != null && ((User)(Page.User.Identity)).HasPermission(AccessLevel.Administrator))
                    {
                        logList.Visible = true;
                    }
                    HyperLink hyperLinkAgentList = master.FindControl("hyperLinkAgentList") as HyperLink;
                    if (hyperLinkAgentList != null)
                    {
                        hyperLinkAgentList.NavigateUrl =
                            string.Format("/Modules/TourGuide/Admin/GuideRatePolicy.aspx?NodeId={0}&SectionId={1}",
                                          Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
                    }
                }

                Panel panelProvider = master.FindControl("panelProvider") as Panel;
                if (panelProvider != null)
                {
                    panelProvider.Visible = true;

                    HyperLink hyperLinkProviderList = master.FindControl("hyperLinkProviderList") as HyperLink;
                    if (hyperLinkProviderList != null)
                    {
                        hyperLinkProviderList.NavigateUrl =
                            string.Format("/Modules/TourManagement/Admin/ProviderList.aspx?NodeId={0}&SectionId={1}",
                                          Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
                    }
                }

                HtmlControl tabHotel = Master.FindControl("tabGuide") as HtmlControl;
                if (tabHotel != null)
                {
                    tabHotel.Attributes.Add("class", "selected");
                }
            }
            #endregion

            #region -- BOAT --
            if (moduleName == "TourBoat" || moduleName == string.Empty)
            {
                Panel panelHotel = master.FindControl("panelTourBoat") as Panel;
                Panel panelProvider = master.FindControl("panelProvider") as Panel;
                if (panelHotel != null)
                {
                    panelHotel.Visible = true;
                    Control logList = Master.FindControl("panelBoatAdmin");
                    if (logList != null && ((User)(Page.User.Identity)).HasPermission(AccessLevel.Administrator))
                    {
                        logList.Visible = true;
                    }
                    HyperLink hyperLinkAgentList = master.FindControl("hyperLinkAgentList") as HyperLink;
                    if (hyperLinkAgentList != null)
                    {
                        hyperLinkAgentList.NavigateUrl =
                            string.Format("/Modules/TourManagement/Admin/AgentList.aspx?NodeId={0}&SectionId={1}&ModuleType={2}",
                                          Request.QueryString["NodeId"], Request.QueryString["SectionId"], "TOURBOAT");
                    }
                }
                if (panelProvider != null)
                {
                    panelProvider.Visible = true;
                    HyperLink hyperLinkProviderList = master.FindControl("hyperLinkProviderList") as HyperLink;
                    if (hyperLinkProviderList != null)
                    {
                        hyperLinkProviderList.NavigateUrl =
                            string.Format("/Modules/TourManagement/Admin/ProviderList.aspx?NodeId={0}&SectionId={1}",
                                          Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
                    }
                }

                HtmlControl tabHotel = Master.FindControl("tabCruise") as HtmlControl;
                if (tabHotel != null)
                {
                    tabHotel.Attributes.Add("class", "selected");
                }
            }
            #endregion

            #region -- TOUR --
            if (moduleName==" ")
            {
                Panel panelTour = master.FindControl("panelTour") as Panel;
                Panel panelProvider = master.FindControl("panelProvider") as Panel;
                if (panelTour!=null)
                {
                    panelTour.Visible = true;
                }

                if (panelProvider != null)
                {
                    panelProvider.Visible = true;
                    HyperLink hyperLinkProviderList = master.FindControl("hyperLinkProviderList") as HyperLink;
                    if (hyperLinkProviderList != null)
                    {
                        hyperLinkProviderList.NavigateUrl =
                            string.Format("/Modules/TourManagement/Admin/ProviderList.aspx?NodeId={0}&SectionId={1}",
                                          Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
                    }
                }

                HtmlControl tabHotel = Master.FindControl("tabTour") as HtmlControl;
                if (tabHotel != null)
                {
                    tabHotel.Attributes.Add("class", "selected");
                }
            }
            #endregion

            #region -- Others --
            if (moduleName == "TourOthers")
            {
                Panel panelTour = master.FindControl("panelOthers") as Panel;
                if (panelTour != null)
                {
                    panelTour.Visible = true;
                }
            }
            #endregion
        }

        public void ShowMessage(string message)
        {
            HtmlGenericControl divMessage = (HtmlGenericControl)master.FindControl("divMessage");
            divMessage.Visible = true;
            Label labelMessage = master.FindControl("labelMessage") as Label;
            if (labelMessage != null)
            {
                labelMessage.Text = message;
                if (divMessage.Attributes["class"] != null)
                {
                    divMessage.Attributes["class"] = "module_message";
                }
                else
                {
                    divMessage.Attributes.Add("class", "module_message");
                }
            }
        }

        public void ShowError(string message)
        {
            HtmlGenericControl divMessage = (HtmlGenericControl)master.FindControl("divMessage");
            divMessage.Visible = true;
            Label labelMessage = master.FindControl("labelMessage") as Label;
            if (labelMessage != null)
            {
                labelMessage.Text = message;
                if (divMessage.Attributes["class"] != null)
                {
                    divMessage.Attributes["class"] = "module_error";
                }
                else
                {
                    divMessage.Attributes.Add("class", "module_error");
                }
            }
        }

        public void HideError()
        {
            if (master == null)
            {
                return;
            }
            HtmlGenericControl divMessage = master.FindControl("divMessage") as HtmlGenericControl;
            if (divMessage != null)
            {
                divMessage.Visible = false;
            }
        }

        public bool RedirectBack()
        {
            if (ViewState["refererUrl"]!=null)
            {
                PageRedirect(ViewState["refererUrl"].ToString());
                return true;
            }
            return false;
        }
    }
}