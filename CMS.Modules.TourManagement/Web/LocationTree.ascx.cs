using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using CMS.Core.Domain;
using CMS.Modules.TourManagement.Domain;
using CMS.Modules.TourManagement.Web.UI;
using CMS.Web.Util;

namespace CMS.Modules.TourManagement.Web
{
    public partial class LocationTree : TourControlBase
    {
        #region -- PAGE EVENTS --

        private string _path;
        private Section section;

        private TourModuleBase _listModule;
        protected void Page_Load(object sender, EventArgs e)
        {
            section = Module.Section.Connections["Listing"] as Section;
            if (section!=null)
            {
                _path = UrlHelper.GetUrlFromSection(section);
            }
            else
            {
                _path = UrlHelper.GetUrlFromNode(Module.Section.Node);
                return;
            }
            _listModule = ModuleHelper.GetModuleFromSection(section, PageEngine.Container) as TourModuleBase;
            if (_listModule == null)
            {
                return;
            }
            if (!IsPostBack)
            {
                if (!HasCachedOutput)
                {
                    #region -- Xử lý cây --

                    // Lấy toàn bộ Root Location
                    IList listLocations = Module.LocationGetRoot();

                    // Đối với mỗi root Location, thêm vào tree (nếu có data)
                    // Nếu root có con, cho phép populate on demand
                    foreach (Location rootLocation in listLocations)
                    {
                        if (!_listModule.CheckDataExists(rootLocation))
                        {
                            continue;
                        }
                        TreeNode rootNode = new TreeNode(rootLocation.Name, rootLocation.Id.ToString());
                        //if (rootLocation.Id == 1)
                        //{
                            // Đoạn này xử lý cứng để kịp thời gian
                            //TODO: Fix triệt để
                        //    rootNode.NavigateUrl = "/10/view.aspx?locationId=1";
                        //}
                        //else
                        //{
                        if (section!=null)
                        {
                            rootNode.NavigateUrl = _listModule.GetLinkFromLocation(section, rootLocation);
                        }
                        else
                        {
                            rootNode.NavigateUrl = string.Format("{0}/list?locationId={1}", _path, rootLocation.Id);
                        }
                        //}
                        if (rootLocation.Children.Count > 0)
                        {
                            rootNode.PopulateOnDemand = false;
                        }                        
                        treeViewLocation.Nodes.Add(rootNode);

                        // Expand root
                        BuildChildNode(rootLocation, rootNode);
                        rootNode.Expanded = true;
                    }

                    #endregion

                    #region -- Xử lý khi có tham số locationId truyền vào --

                    if (Request.QueryString["LocationId"] != null)
                    {
                        try
                        {
                            // Lấy Location truyền vào
                            Location location =
                                Module.LocationGetById(Convert.ToInt32(Request.QueryString["LocationId"]));

                            List<Location> locationList = new List<Location>();
                            // Khởi tạo cây phả hệ của địa điểm truyền vào
                            while (location != null)
                            {
                                locationList.Add(location);
                                location = location.Parent;
                            }

                            TreeNode currentNode = null;
                            // Xử lý toàn bộ cây phả hệ
                            while (locationList.Count > 0)
                            {
                                // Xử lý từ tổ tiên trở xuống
                                Location current = locationList[locationList.Count - 1];
                                // Nếu CurrentNode không phải null, tức là đây không phải là ông tổ
                                if (currentNode != null)
                                {
                                    // Xử lý từng con của CurrentNode để xác định node ứng với current Location
                                    foreach (TreeNode node in currentNode.ChildNodes)
                                    {
                                        if (node.Value == current.Id.ToString())
                                        {
                                            node.Expand();
                                            currentNode = node;
                                        }
                                    }
                                }
                                else
                                {
                                    // Nếu currentLocation thuộc dạng ông tổ, tìm ngay trên các node gốc
                                    foreach (TreeNode node in treeViewLocation.Nodes)
                                    {
                                        if (node.Value == current.Id.ToString())
                                        {
                                            node.Expand();
                                            currentNode = node;
                                        }
                                    }
                                }
                                locationList.Remove(current);
                            }
                        }
                        catch
                        {
                            throw new Exception("Bad Request");
                        }
                        return;
                    }

                    #endregion

                    #region -- Xử lý khi có tham số locationId truyền vào --

                    if (PageEngine.GlobalVar.ContainsKey("Location"))
                    {
                        try
                        {
                            // Lấy Location truyền vào
                            Location location = (Location)PageEngine.GlobalVar["Location"];

                            List<Location> locationList = new List<Location>();
                            // Khởi tạo cây phả hệ của địa điểm truyền vào
                            while (location != null)
                            {
                                locationList.Add(location);
                                location = location.Parent;
                            }

                            TreeNode currentNode = null;
                            // Xử lý toàn bộ cây phả hệ
                            while (locationList.Count > 0)
                            {
                                // Xử lý từ tổ tiên trở xuống
                                Location current = locationList[locationList.Count - 1];
                                // Nếu CurrentNode không phải null, tức là đây không phải là ông tổ
                                if (currentNode != null)
                                {
                                    // Xử lý từng con của CurrentNode để xác định node ứng với current Location
                                    foreach (TreeNode node in currentNode.ChildNodes)
                                    {
                                        if (node.Value == current.Id.ToString())
                                        {
                                            node.Expand();
                                            currentNode = node;
                                        }
                                    }
                                }
                                else
                                {
                                    // Nếu currentLocation thuộc dạng ông tổ, tìm ngay trên các node gốc
                                    foreach (TreeNode node in treeViewLocation.Nodes)
                                    {
                                        if (node.Value == current.Id.ToString())
                                        {
                                            node.Expand();
                                            currentNode = node;
                                        }
                                    }
                                }
                                locationList.Remove(current);
                            }
                        }
                        catch
                        {
                            throw new Exception("Bad Request");
                        }
                    }

                    #endregion
                }
            }
        }
        #endregion

        #region -- PRIVATE METHODS --
        /// <summary>
        /// Sinh tree node cho tất cả các node nằm trong một tree node cho trước bằng con của
        /// địa điểm cho trước
        /// </summary>
        /// <param name="parentLocation">Địa điểm ứng với node</param>
        /// <param name="parentNode">Node cần sinh con</param>
        private void BuildChildNode(Location parentLocation, TreeNode parentNode)
        {
            foreach (Location location in parentLocation.Children)
            {
                if (location.Id > 0)
                {
                    if (!_listModule.CheckDataExists(location))
                    {
                        continue;
                    }
                    TreeNode node = new TreeNode(location.Name, location.Id.ToString());

                    if (section!=null)
                    {
                        node.NavigateUrl = _listModule.GetLinkFromLocation(section, location);
                    }
                    else
                    {
                        node.NavigateUrl = string.Format("{0}/list?locationId={1}", _path, location.Id);
                    }

                    if (location.Children.Count > 0)
                    {
                        node.PopulateOnDemand = true;
                    }
                    parentNode.ChildNodes.Add(node);
                }
            }
        }
        #endregion

        #region -- CONTROL EVENTS --
        /// <summary>
        /// Xử lý khi một node được expand
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// 1. Nếu node hiện tại có cha, expand nốt cha (và tổ tiên)
        /// 2. Lấy các node con của node hiện tại
        /// </remarks>
        protected void treeViewLocation_TreeNodePopulate(object sender, TreeNodeEventArgs e)
        {
            if (e.Node.Parent!=null)
            {
                TreeNode node = e.Node.Parent;
                while (node != null)
                {
                    node.Expanded = true;
                    node = node.Parent;
                }
            }
            Location location = Module.LocationGetById(Convert.ToInt32(e.Node.Value));
            BuildChildNode(location, e.Node);
        }
        #endregion
    }
}