using System;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using CMS.Core.Domain;
using CMS.Modules.TourManagement.Domain;
using CMS.Modules.TourManagement.Web.UI;
using CMS.ServerControls;
using log4net;

namespace CMS.Modules.TourManagement.Web.Admin
{
    public partial class PositionList : TourAdminBasePage
    {
        private Location parentLocation;
        private readonly ILog logger = LogManager.GetLogger(typeof (PositionList));

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = Resources.titleAdminPositionList;
            if (!UserIdentity.HasPermission(AccessLevel.Administrator))
            {
                ShowError(Resources.textAccessDenied);
                panelContent.Visible = false;
                return;
            }
            if (!IsPostBack)
            {
                GetDataSource();
                //rptLocations.DataBind();
                reorderListLocation.DataBind();
            }
        }

        private void GetDataSource()
        {
            if (Request.QueryString["LocationId"] != null)
            {
                parentLocation = Module.LocationGetById(Convert.ToInt32(Request.QueryString["LocationId"]));
                lblParentLocation.Text = string.Format(Resources.messageLocationListParent, parentLocation.Name);
                if (parentLocation.Parent != null)
                {
                    hplParentLocation.Text = string.Format(Resources.messageGoUpOneLevel, parentLocation.Parent.Name);
                    hplParentLocation.NavigateUrl = string.Format("/Modules/TourManagement/Admin/PositionList.aspx?NodeId={0}&SectionId={1}&LocationId={2}", Request.QueryString["NodeId"], Request.QueryString["SectionId"], parentLocation.Parent.Id);
                }
                else
                {
                    hplParentLocation.Text = Resources.messageGoBackToRoot;
                    hplParentLocation.NavigateUrl = string.Format("/Modules/TourManagement/Admin/PositionList.aspx?NodeId={0}&SectionId={1}", Request.QueryString["NodeId"], Request.QueryString["SectionId"]);
                }
                //rptLocations.DataSource = parentLocation.Children;
                reorderListLocation.DataSource = parentLocation.Children;
                if (parentLocation.Children.Count == 0)
                {
                    lblNotFound.Text = Resources.messageNotFoundAnyLocation;
                }
            }
            else
            {
                lblParentLocation.Text = String.Empty;
                //rptLocations.DataSource = Module.LocationGetRoot();
                reorderListLocation.DataSource = Module.LocationGetRoot();
            }
        }

        #region -- Pager --
        protected void pagerLocation_CacheEmpty(object sender, EventArgs e)
        {
            GetDataSource();
        }

        protected void pagerLocation_PageChanged(object sender, PageChangedEventArgs e)
        {
            GetDataSource();
            //rptLocations.DataBind();
            reorderListLocation.DataBind();
        }
        #endregion

        #region -- Repeater --
        /*
        protected void rptLocations_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Location location = e.Item.DataItem as Location;
            if (location != null)
            {
                using (Label lbl = e.Item.FindControl("lblSubLocation") as Label)
                {
                    if (lbl != null && location.Children.Count > 0)
                    {
                        lbl.Text = string.Format("({0})", location.Children.Count);
                    }
                }

                using (HyperLink hpl = e.Item.FindControl("hplEdit") as HyperLink)
                {
                    if (hpl != null)
                    {
                        hpl.NavigateUrl =
                            string.Format(
                                "/Modules/TourManagement/Admin/PositionEdit.aspx?NodeId={0}&SectionId={1}&LocationId={2}",
                                Request.QueryString["NodeId"], Request.QueryString["SectionId"], location.Id);
                    }
                }

                using (HyperLink hpl = e.Item.FindControl("hplLocation") as HyperLink)
                {
                    if (hpl != null)
                    {
                        hpl.NavigateUrl =
                            string.Format(
                                "/Modules/TourManagement/Admin/PositionList.aspx?NodeId={0}&SectionId={1}&LocationId={2}",
                                Request.QueryString["NodeId"], Request.QueryString["SectionId"], location.Id);
                    }                    
                }

                using (Label labelID = e.Item.FindControl("lblID") as Label)
                {
                    if (labelID!=null)
                    {
                        labelID.Text = string.Format("ID:{0}", location.Id);
                    }
                }
            }
        }

        protected void rptLocations_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            Location location;
            switch (e.CommandName)
            {
                case "View":
                    location = Module.LocationGetById(Convert.ToInt32(e.CommandArgument));
                    imgLocationView.ImageUrl = location.Image;
                    lblLocationName.Text = location.Name;
                    ModalPopupExtender.Show();
                    break;
                case "Delete":
                    location = Module.LocationGetById(Convert.ToInt32(e.CommandArgument));
                    Module.Delete(location);
                    GetDataSource();
                    rptLocations.DataBind();
                    break;
                default:
                    break;
            }
        }
        */
        #endregion

        #region -- Reorder List --
        protected void reorderListLocation_ItemDataBound(object sender, ReorderListItemEventArgs e)
        {
            Location location = e.Item.DataItem as Location;
            if (location != null)
            {
                using (Label lbl = e.Item.FindControl("lblSubLocation") as Label)
                {
                    if (lbl != null && location.Children.Count > 0)
                    {
                        lbl.Text = string.Format("({0})", location.Children.Count);
                    }
                }

                using (HyperLink hpl = e.Item.FindControl("hplEdit") as HyperLink)
                {
                    if (hpl != null)
                    {
                        hpl.NavigateUrl =
                            string.Format(
                                "/Modules/TourManagement/Admin/PositionEdit.aspx?NodeId={0}&SectionId={1}&LocationId={2}",
                                Request.QueryString["NodeId"], Request.QueryString["SectionId"], location.Id);
                    }
                }

                using (HyperLink hpl = e.Item.FindControl("hplLocation") as HyperLink)
                {
                    if (hpl != null)
                    {
                        hpl.NavigateUrl =
                            string.Format(
                                "/Modules/TourManagement/Admin/PositionList.aspx?NodeId={0}&SectionId={1}&LocationId={2}",
                                Request.QueryString["NodeId"], Request.QueryString["SectionId"], location.Id);
                    }
                }

                using (Label labelID = e.Item.FindControl("lblID") as Label)
                {
                    if (labelID != null)
                    {
                        labelID.Text = string.Format("ID:{0}", location.Id);
                    }
                }
            }
        }

        protected void reorderListLocation_ItemCommand(object sender, ReorderListCommandEventArgs e)
        {
            Location location;
            switch (e.CommandName)
            {
                case "View":
                    location = Module.LocationGetById(Convert.ToInt32(e.CommandArgument));
                    imgLocationView.ImageUrl = location.Image;
                    lblLocationName.Text = location.Name;
                    ModalPopupExtender.Show();
                    break;
                case "Delete":
                    try
                    {
                        location = Module.LocationGetById(Convert.ToInt32(e.CommandArgument));
                        Module.Delete(location);
                        GetDataSource();
                        reorderListLocation.DataBind();
                    }
                    catch(Exception ex)
                    {
                        ShowError(ex.Message);
                        logger.Error("Delete location error",ex);
                    }
                    break;
                default:
                    break;
            }
        }

        protected void reorderListLocation_ItemReorder(object sender, ReorderListItemReorderEventArgs e)
        {
            #region -- Old  --
            //Label labelId = (Label)reorderListLocation.Items[e.NewIndex].FindControl("lblID");
            //Location newIndex = Module.LocationGetById(Convert.ToInt32(labelId.Text.Substring(3)));
            //labelId = (Label)reorderListLocation.Items[e.OldIndex].FindControl("lblID");
            //Location oldIndex = Module.LocationGetById(Convert.ToInt32(labelId.Text.Substring(3)));
            //// order của old sẽ thành bằng new
            //int newOrder = newIndex.Order;
            //// Tất cả nhỏ hơn new sẽ giảm order đi 1
            //Module.Resort(newIndex,newOrder < oldIndex.Order);
            //oldIndex = Module.LocationGetById(Convert.ToInt32(labelId.Text.Substring(3)));
            //oldIndex.Order = newOrder;
            //Module.Update(oldIndex);
            #endregion

            #region -- New  --

            ReorderListItem OldItem = reorderListLocation.Items[e.OldIndex];
            Label labelId;
            Location LocationItem;
            if (e.OldIndex > e.NewIndex)
            {
                for (int ii = e.NewIndex; ii < e.OldIndex; ii++)
                {
                    ReorderListItem item = reorderListLocation.Items[ii];
                    item.ItemIndex += 1;
                    labelId = (Label) item.FindControl("lblID");
                    LocationItem = Module.LocationGetById(Convert.ToInt32(labelId.Text.Substring(3)));
                    LocationItem.Order = item.DisplayIndex;
                    Module.Update(LocationItem);
                }
            }

            if (e.OldIndex < e.NewIndex)
            {
                for (int ii = e.NewIndex; ii > e.OldIndex; ii++)
                {
                    ReorderListItem item = reorderListLocation.Items[ii];
                    item.ItemIndex -= 1;
                    labelId = (Label)item.FindControl("lblID");
                    LocationItem = Module.LocationGetById(Convert.ToInt32(labelId.Text.Substring(3)));
                    LocationItem.Order = item.DisplayIndex;
                    Module.Update(LocationItem);
                }
            }
            
            OldItem.ItemIndex = e.NewIndex;
            labelId = (Label)OldItem.FindControl("lblID");
            LocationItem = Module.LocationGetById(Convert.ToInt32(labelId.Text.Substring(3)));
            LocationItem.Order = OldItem.DisplayIndex;
            Module.Update(LocationItem);

            GetDataSource();
            reorderListLocation.DataBind();
            #endregion
        }
        #endregion        
    }
}