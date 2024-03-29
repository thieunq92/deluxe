using System;
using System.Collections.Generic;
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
using CMS.Web.UI;
using CMS.Web.Util;

namespace CMS.Modules.TourManagement.Web
{
    public partial class TourRegionMenu : BaseModuleControl
    {
        #region -- PAGE EVENTS --

        private string _path;
        private Section section;

        public new TourMenuModule Module
        {
            get { return base.Module as TourMenuModule; }
        }
        //private TourModuleBase _listModule;
        protected void Page_Load(object sender, EventArgs e)
        {
            section = Module.Section.Connections["TourListing"] as Section;
            if (section != null)
            {
                _path = UrlHelper.GetUrlFromSection(section);
            }
            else
            {
                _path = UrlHelper.GetUrlFromNode(Module.Section.Node);
            }

            //_listModule = ModuleHelper.GetModuleFromSection(section, PageEngine.Container) as TourModuleBase;
            //if (_listModule == null)
            //{
            //    return;
            //}
            if (!IsPostBack)
            {
                if (!HasCachedOutput)
                {
                    #region -- Xử lý cây --

                    // Lấy toàn bộ Root BoatCategory
                    IList listBoatCategorys = Module.TourRegionGetRoot();

                    // Đối với mỗi root BoatCategory, thêm vào tree (nếu có data)
                    // Nếu root có con, cho phép populate on demand
                    foreach (TourRegion rootBoatCategory in listBoatCategorys)
                    {
                        TreeNode rootNode = new TreeNode(rootBoatCategory.Name, rootBoatCategory.Id.ToString());
                        rootNode.NavigateUrl = string.Format("{0}/list?Region={1}", _path, rootBoatCategory.Id);
                        if (rootBoatCategory.Children.Count > 0)
                        {
                            rootNode.PopulateOnDemand = false;
                        }
                        treeViewTourRegion.Nodes.Add(rootNode);

                        // Expand root
                        BuildChildNode(rootBoatCategory, rootNode);
                        rootNode.Expanded = true;
                    }

                    #endregion

                    #region -- Xử lý khi có tham số BoatCategoryId truyền vào --

                    if (Request.QueryString["BoatCategoryId"] != null)
                    {
                        try
                        {
                            // Lấy BoatCategory truyền vào
                            TourRegion BoatCategory =
                                Module.TourRegionGetById(Convert.ToInt32(Request.QueryString["BoatCategoryId"]));

                            List<TourRegion> BoatCategoryList = new List<TourRegion>();
                            // Khởi tạo cây phả hệ của địa điểm truyền vào
                            while (BoatCategory != null)
                            {
                                BoatCategoryList.Add(BoatCategory);
                                BoatCategory = BoatCategory.Parent;
                            }

                            TreeNode currentNode = null;
                            // Xử lý toàn bộ cây phả hệ
                            while (BoatCategoryList.Count > 0)
                            {
                                // Xử lý từ tổ tiên trở xuống
                                TourRegion current = BoatCategoryList[BoatCategoryList.Count - 1];
                                // Nếu CurrentNode không phải null, tức là đây không phải là ông tổ
                                if (currentNode != null)
                                {
                                    // Xử lý từng con của CurrentNode để xác định node ứng với current BoatCategory
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
                                    // Nếu currentBoatCategory thuộc dạng ông tổ, tìm ngay trên các node gốc
                                    foreach (TreeNode node in treeViewTourRegion.Nodes)
                                    {
                                        if (node.Value == current.Id.ToString())
                                        {
                                            node.Expand();
                                            currentNode = node;
                                        }
                                    }
                                }
                                BoatCategoryList.Remove(current);
                            }
                        }
                        catch
                        {
                            throw new Exception("Bad Request");
                        }
                        return;
                    }

                    #endregion

                    #region -- Xử lý khi có tham số BoatCategoryId truyền vào --

                    if (PageEngine.GlobalVar.ContainsKey("BoatCategory"))
                    {
                        try
                        {
                            // Lấy BoatCategory truyền vào
                            TourRegion BoatCategory = (TourRegion)PageEngine.GlobalVar["BoatCategory"];

                            List<TourRegion> BoatCategoryList = new List<TourRegion>();
                            // Khởi tạo cây phả hệ của địa điểm truyền vào
                            while (BoatCategory != null)
                            {
                                BoatCategoryList.Add(BoatCategory);
                                BoatCategory = BoatCategory.Parent;
                            }

                            TreeNode currentNode = null;
                            // Xử lý toàn bộ cây phả hệ
                            while (BoatCategoryList.Count > 0)
                            {
                                // Xử lý từ tổ tiên trở xuống
                                TourRegion current = BoatCategoryList[BoatCategoryList.Count - 1];
                                // Nếu CurrentNode không phải null, tức là đây không phải là ông tổ
                                if (currentNode != null)
                                {
                                    // Xử lý từng con của CurrentNode để xác định node ứng với current BoatCategory
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
                                    // Nếu currentBoatCategory thuộc dạng ông tổ, tìm ngay trên các node gốc
                                    foreach (TreeNode node in treeViewTourRegion.Nodes)
                                    {
                                        if (node.Value == current.Id.ToString())
                                        {
                                            node.Expand();
                                            currentNode = node;
                                        }
                                    }
                                }
                                BoatCategoryList.Remove(current);
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
        /// <param name="parentBoatCategory">Địa điểm ứng với node</param>
        /// <param name="parentNode">Node cần sinh con</param>
        private void BuildChildNode(TourRegion parentBoatCategory, TreeNode parentNode)
        {
            foreach (TourRegion BoatCategory in parentBoatCategory.Children)
            {
                if (BoatCategory.Id > 0)
                {
                    //if (!_listModule.CheckDataExists(BoatCategory))
                    //{
                    //    continue;
                    //}
                    TreeNode node = new TreeNode(BoatCategory.Name, BoatCategory.Id.ToString());

                    //if (section != null && _listModule.SelecterPath.Contains("Hotel"))
                    //{
                    //    node.NavigateUrl = _listModule.GetLinkFromBoatCategory(section, BoatCategory);
                    //}
                    //else
                    //{
                    node.NavigateUrl = string.Format("{0}/list?Region={1}", _path, BoatCategory.Id);
                    //}

                    if (BoatCategory.Children.Count > 0)
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
        protected void treeViewTourRegion_TreeNodePopulate(object sender, TreeNodeEventArgs e)
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
            TourRegion BoatCategory = Module.TourRegionGetById(Convert.ToInt32(e.Node.Value));
            BuildChildNode(BoatCategory, e.Node);
        }
        #endregion
    }
}