using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using CMS.Modules.TourManagement.Domain;
using CMS.Web.Admin.UI;
using CMS.Web.UI;

namespace CMS.Modules.TourManagement.Web.Controls
{
    [Obsolete("No use any more")]
    public partial class LocationTree : CustomTypeSettingControl
    {
        public const string PATH = "/Modules/TourManagement/Controls/LocationTree.ascx";

        #region -- PRIVATE MEMBERS --

        private int _selectedId = 0;
        public int SelectedId
        {
            set { _selectedId = value; }
        }
        #endregion

        #region

        public override string SelectedValue
        {
            get
            {
                return treeViewLocation.SelectedValue;
            }
            set
            {
                return;
            }
        }

        public PageEngine PageEngine
        {
            get { return Page as PageEngine; }
        }

        #endregion

        #region

        #region Delegates

        public delegate Location GetById(int id);

        public delegate IList GetRoot();

        #endregion

        public GetById LocationGetById;
        public GetRoot LocationGetRoot;

        #endregion

        #region -- PAGE EVENTS --

        protected void Page_Load(object sender, EventArgs e)
        {
            if (treeViewLocation.Nodes.Count == 0)
            {
                // Lấy toàn bộ Root Location
                IList listLocations = LocationGetRoot();

                // Đối với mỗi root Location, thêm vào tree
                // Nếu root có con, cho phép populate on demand
                foreach (Location rootLocation in listLocations)
                {
                    TreeNode rootNode = new TreeNode(rootLocation.Name, rootLocation.Id.ToString());
                    // Cho phép populate on demand
                    if (rootLocation.Children.Count > 0)
                    {
                        rootNode.PopulateOnDemand = true;
                    }
                    treeViewLocation.Nodes.Add(rootNode);

                    // Expand root
                    BuildChildNode(rootLocation, rootNode);
                    rootNode.Expand();
                }

                #region -- Xử lý khi có tham số locationId truyền vào --

                if (_selectedId > 0)
                {
                    try
                    {
                        // Lấy Location truyền vào
                        Location location = LocationGetById(_selectedId);

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

                //treeViewLocation.Nodes[0].Selected = true;
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
                TreeNode node = new TreeNode(location.Name, location.Id.ToString());
                if (location.Children.Count > 0)
                {
                    node.PopulateOnDemand = true;
                }
                parentNode.ChildNodes.Add(node);
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
            if (e.Node.Parent != null)
            {
                TreeNode node = e.Node.Parent;
                while (node != null)
                {
                    node.Expanded = true;
                    node = node.Parent;
                }
            }
            Location location = LocationGetById(Convert.ToInt32(e.Node.Value));
            BuildChildNode(location, e.Node);
        }

        #endregion
    }
}