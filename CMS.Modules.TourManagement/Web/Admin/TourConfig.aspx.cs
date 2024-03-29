using System;
using System.Collections;
using System.Data;
using CMS.Core.Domain;
using CMS.Modules.TourManagement.Domain;
using CMS.Modules.TourManagement.Web.UI;
using CMS.Web.Util;

namespace CMS.Modules.TourManagement.Web.Admin
{
    public partial class TourHotelConfig : TourAdminBasePage
    {
        private int _tourId;

        #region -- PAGE EVENTS

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = Resources.labelTourConfiguration;
            if (Request.QueryString["TourId"] != null)
            {
                _tourId = Convert.ToInt32(Request.QueryString["TourId"]);
            }
            string tab = string.Empty;
            if (Request.QueryString["tab"] != null)
            {
                tab = Request.QueryString["tab"];
            }

            switch (tab.ToLower())
            {
                case "restaurant":

                    #region -- Restaurant tab --

                    if (Module.RestaurantSection != null)
                    {
                        CreateSelector(Module.RestaurantSection);
                        return;
                    }

                    #endregion

                    break;
                case "guide":

                    #region -- Guide tab --

                    if (Module.GuideSection != null)
                    {
                        CreateSelector(Module.GuideSection);
                        return;
                    }

                    #endregion

                    break;
                case "hotel":

                    #region -- Hotel tab --

                    if (Module.HotelSection != null)
                    {
                        CreateSelector(Module.HotelSection);
                    }

                    #endregion

                    break;
                case "transport":

                    #region -- Transport tab --

                    if (Module.TransportSection != null)
                    {
                        CreateSelector(Module.TransportSection);
                    }

                    #endregion

                    break;

                case "landscape":

                    #region -- Entrance Fee --

                    if (Module.LandscapeSection != null)
                    {
                        CreateSelector(Module.LandscapeSection);
                    }

                    #endregion

                    break;
                case "boat":

                    #region -- Boat tab --

                    if (Module.BoatSection != null)
                    {
                        CreateSelector(Module.BoatSection);
                    }

                    #endregion

                    break;

                case "package":
                    #region -- Package --

                    if (Module.Section != null)
                    {
                        CreateTourSelector(true);
                    }

                    #endregion
                    break;
                case "others":
                    #region -- Others --
                    if (Module.Section !=null)
                    {
                        CreateOtherSelector(true);
                    }
                    break;
                    #endregion

                case "related":
                #region -- Related --
                    if (Module.Section !=null)
                    {
                        CreateRelated(true);
                    }
                #endregion

                    break;
                default:

                    #region -- Overview tab --

                    if (Module.HotelSection != null)
                    {
                        CreateSelector(Module.HotelSection, false);
                    }

                    if (Module.RestaurantSection != null)
                    {
                        CreateSelector(Module.RestaurantSection, false);
                    }

                    if (Module.TransportSection != null)
                    {
                        CreateSelector(Module.TransportSection, false);
                    }

                    if (Module.GuideSection != null)
                    {
                        CreateSelector(Module.GuideSection, false);
                    }
                    if (Module.LandscapeSection != null)
                    {
                        CreateSelector(Module.LandscapeSection, false);
                    }
                    if (Module.BoatSection != null)
                    {
                        CreateSelector(Module.BoatSection, false);
                    }
                    panelExport.Visible = true;

                    #endregion

                    break;
            }
        }

        #endregion

        private void CreateSelector(Section section)
        {
            CreateSelector(section, true);
        }

        private void CreateSelector(Section section, bool isSelector)
        {
            TourModuleBase module = _moduleLoader.GetModuleFromSection(section) as TourModuleBase;
            if (module != null)
            {
                SelectorControl Selecter;
                if (isSelector)
                {
                    Selecter = LoadControl(module.SelecterPath) as SelectorControl;
                }
                else
                {
                    Selecter = LoadControl(module.OverviewPath) as SelectorControl;
                }
                if (Selecter == null)
                {
                    return;
                }
                if (isSelector)
                {
                    Selecter.ID = "Selector";
                }
                else
                {
                    Selecter.ID = "Selector" + section.Id;
                }
                Selecter.Module = module;
                plhConfig.Controls.Add(Selecter);
            }
        }

        private void CreateTourSelector(bool isSelector)
        {
            TourModuleBase module = Module;
            if (module != null)
            {
                SelectorControl Selecter;
                if (isSelector)
                {
                    Selecter = LoadControl("/Modules/TourManagement/Admin/TourPackageSelector.ascx") as SelectorControl;
                }
                else
                {
                    Selecter = LoadControl(module.OverviewPath) as SelectorControl;
                }
                if (Selecter == null)
                {
                    return;
                }
                if (isSelector)
                {
                    Selecter.ID = "Selector";
                }
                else
                {
                    Selecter.ID = "Selector" + module.Section.Id;
                }
                Selecter.Module = module;
                plhConfig.Controls.Add(Selecter);
            }
        }

        private void CreateOtherSelector(bool isSelector)
        {
            TourModuleBase module = Module;
            if (module != null)
            {
                SelectorControl Selecter;
                if (isSelector)
                {
                    Selecter = LoadControl("/Modules/TourManagement/Admin/OtherExpenses.ascx") as SelectorControl;
                }
                else
                {
                    Selecter = LoadControl(module.OverviewPath) as SelectorControl;
                }
                if (Selecter == null)
                {
                    return;
                }
                if (isSelector)
                {
                    Selecter.ID = "Selector";
                }
                else
                {
                    Selecter.ID = "Selector" + module.Section.Id;
                }
                Selecter.Module = module;
                plhConfig.Controls.Add(Selecter);
            }
        }

        private void CreateRelated(bool isSelector)
        {
            TourModuleBase module = Module;
            if (module != null)
            {
                SelectorControl Selecter;
                if (isSelector)
                {
                    Selecter = LoadControl("/Modules/TourManagement/Admin/RelatedTour.ascx") as SelectorControl;
                }
                else
                {
                    Selecter = LoadControl(module.OverviewPath) as SelectorControl;
                }
                if (Selecter == null)
                {
                    return;
                }
                if (isSelector)
                {
                    Selecter.ID = "Selector";
                }
                else
                {
                    Selecter.ID = "Selector" + module.Section.Id;
                }
                Selecter.Module = module;
                plhConfig.Controls.Add(Selecter);
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            Tour tour = Module.TourGetById(_tourId);
            DataTable table = new DataTable();
            #region -- CREATE TABLE --
            // Tạo 10 cột, trong đó có 2 cột tiêu đề, các cột còn lại là giá từ 1-8 người
            table.Columns.Add("Service",typeof(string));
            table.Columns.Add("Detail",typeof(string));
            table.Columns.Add("1", typeof(double));
            table.Columns.Add("2", typeof(double));
            table.Columns.Add("3", typeof(double));
            table.Columns.Add("4", typeof(double));
            table.Columns.Add("5", typeof(double));
            table.Columns.Add("6", typeof(double));
            table.Columns.Add("7", typeof(double));
            table.Columns.Add("8", typeof(double));
            #endregion

            #region -- Xuất dữ liệu từ các module sang table theo định dạng như trên --
            if (Module.HotelSection != null)
            {
                TourModuleBase hotelModule =
                    _moduleLoader.GetModuleFromSection(Module.HotelSection) as TourModuleBase;
                if (hotelModule != null)
                {
                    hotelModule.ExportTourConfigToDataTable(_tourId, table);
                }
            }

            if (Module.RestaurantSection != null)
            {
                TourModuleBase hotelModule =
                    _moduleLoader.GetModuleFromSection(Module.RestaurantSection) as TourModuleBase;
                if (hotelModule != null)
                {
                    hotelModule.ExportTourConfigToDataTable(_tourId, table);
                }
            }

            if (Module.TransportSection != null)
            {
                TourModuleBase hotelModule =
                    _moduleLoader.GetModuleFromSection(Module.TransportSection) as TourModuleBase;
                if (hotelModule != null)
                {
                    hotelModule.ExportTourConfigToDataTable(_tourId, table);
                }
            }

            if (Module.LandscapeSection != null)
            {
                TourModuleBase hotelModule =
                    _moduleLoader.GetModuleFromSection(Module.LandscapeSection) as TourModuleBase;
                if (hotelModule != null)
                {
                    hotelModule.ExportTourConfigToDataTable(_tourId, table);
                }
            }

            if (Module.GuideSection != null)
            {
                TourModuleBase hotelModule =
                    _moduleLoader.GetModuleFromSection(Module.GuideSection) as TourModuleBase;
                if (hotelModule != null)
                {
                    hotelModule.ExportTourConfigToDataTable(_tourId, table);
                }
            }

            if (Module.BoatSection != null)
            {
                TourModuleBase hotelModule =
                    _moduleLoader.GetModuleFromSection(Module.BoatSection) as TourModuleBase;
                if (hotelModule != null)
                {
                    hotelModule.ExportTourConfigToDataTable(_tourId, table);
                }
            }
            #endregion

            #region -- Cột tổng giá net--
            DataRow totalRow = table.NewRow();
            totalRow[1] = "TOTAL NET";
            IList tourPrices = Module.TourPriceGetByTour(tour);
            foreach (TourPrice price in tourPrices)
            {
                totalRow[price.NumberOfCustomers + TourManagementModule.TITLECOLUMN - 1] = price.TotalNet;
            }
            table.Rows.Add(totalRow);
            #endregion

            //int currentRow = table.Rows.Count - 1;

            // Lấy giá sale đã tính ở bảng ra rồi xuất sang table
            #region -- Giá sale --
            IList roleList = Module.RoleGetAll();
            Hashtable rolemap = new Hashtable(roleList.Count);
            foreach (Role role in roleList)
            {
                DataRow row = table.NewRow();
                row[1] = role.Name;
                table.Rows.Add(row);
                rolemap.Add(role.Id,table.Rows.Count-1);
            }

            IList tourSalePrices = Module.TourSalePriceGetByTour(tour);
            foreach (TourSalePrice saleprice in tourSalePrices)
            {
                table.Rows[Convert.ToInt32(rolemap[saleprice.RoleId])][
                    saleprice.NumberOfCustomers + TourManagementModule.TITLECOLUMN - 1] = saleprice.Total;
            }
            #endregion

            // Cuối cùng xuất ra excel
            DataExports.ExportToExcel(Context, table, tour.Name, UrlHelper.ConvertToUrl(tour.Name)+".xls");
        }

        protected static double[] Calculate(DataTable table, int rowFrom, int rowTo)
        {
            double[] total = new double[table.Columns.Count];
            for (int ii = rowFrom; ii < rowTo; ii++)
            {
                DataRow datarow = table.Rows[ii];
                for (int jj = 2; jj < table.Columns.Count; jj++)
                {
                    if (!string.IsNullOrEmpty(datarow[jj].ToString()))
                    {
                        total[jj] += Convert.ToDouble(datarow[jj]);
                    }
                }
            }
            return total;
        }

        #region -- Tab click --

        protected void buttonHotelConfig_Click(object sender, EventArgs e)
        {
            if (Request.RawUrl.ToLower().Contains("tab="))
            {
                PageRedirect(Request.RawUrl.Replace("tab=" + Request.QueryString["tab"],
                                                    "tab=" + "hotel"));
            }
            else
            {
                PageRedirect(Request.RawUrl + "&tab=" + "hotel");
            }
        }

        protected void buttonRestaurantConfig_Click(object sender, EventArgs e)
        {
            if (Request.RawUrl.ToLower().Contains("tab="))
            {
                PageRedirect(Request.RawUrl.Replace("tab=" + Request.QueryString["tab"],
                                                    "tab=" + "restaurant"));
            }
            else
            {
                PageRedirect(Request.RawUrl + "&tab=" + "restaurant");
            }
        }

        protected void buttonTransportConfig_Click(object sender, EventArgs e)
        {
            if (Request.RawUrl.ToLower().Contains("tab="))
            {
                PageRedirect(Request.RawUrl.Replace("tab=" + Request.QueryString["tab"],
                                                    "tab=" + "transport"));
            }
            else
            {
                PageRedirect(Request.RawUrl + "&tab=" + "transport");
            }
        }

        protected void buttonGuideConfig_Click(object sender, EventArgs e)
        {
            if (Request.RawUrl.ToLower().Contains("tab="))
            {
                PageRedirect(Request.RawUrl.Replace("tab=" + Request.QueryString["tab"],
                                                    "tab=" + "guide"));
            }
            else
            {
                PageRedirect(Request.RawUrl + "&tab=" + "guide");
            }
        }

        protected void buttonOverview_Click(object sender, EventArgs e)
        {
            if (Request.RawUrl.ToLower().Contains("tab="))
            {
                PageRedirect(Request.RawUrl.Replace("&tab=" + Request.QueryString["tab"],
                                                    ""));
            }
            else
            {
                PageRedirect(Request.RawUrl);
            }
        }

        protected void buttonLandscapeConfig_Click(object sender, EventArgs e)
        {
            if (Request.RawUrl.ToLower().Contains("tab="))
            {
                PageRedirect(Request.RawUrl.Replace("tab=" + Request.QueryString["tab"],
                                                    "tab=" + "landscape"));
            }
            else
            {
                PageRedirect(Request.RawUrl + "&tab=" + "landscape");
            }
        }

        protected void buttonBoatConfig_Click(object sender, EventArgs e)
        {
            if (Request.RawUrl.ToLower().Contains("tab="))
            {
                PageRedirect(Request.RawUrl.Replace("tab=" + Request.QueryString["tab"],
                                                    "tab=" + "boat"));
            }
            else
            {
                PageRedirect(Request.RawUrl + "&tab=" + "boat");
            }
        }

        protected void buttonTourPackageConfig_Click(object sender, EventArgs e)
        {
            if (Request.RawUrl.ToLower().Contains("tab="))
            {
                PageRedirect(Request.RawUrl.Replace("tab=" + Request.QueryString["tab"],
                                                    "tab=" + "package"));
            }
            else
            {
                PageRedirect(Request.RawUrl + "&tab=" + "package");
            }
        }

        #endregion

        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            Tour tour = Module.TourGetById(_tourId);

            // Danh sách các vai trò
            IList list = Module.RoleGetAll();
            for (int numberOfCustomer = 1; numberOfCustomer <= 8; numberOfCustomer++)
            {
                //Tính giá net cho từng số lượng khách hàng
                TourPrice tourPrice = Module.TourPriceGetByTourAndCustomers(tour, numberOfCustomer);

                //  Tạo một bảng giá sale, số lượng phần tử tối đa bằng số role
                Hashtable tourSalePrices = new Hashtable(list.Count);

                // Tạo một datatable
                DataTable table = new DataTable();
                table.Columns.Add("Service", typeof(string));
                table.Columns.Add("Detail", typeof(string));
                table.Columns.Add("Unit price", typeof(double));
                table.Columns.Add("Total", typeof(double));
                // Tạo một loạt các cột trong data table = role
                // Tạo dữ liệu trong hashtable theo id
                foreach (Role role in list)
                {
                    table.Columns.Add(role.Name, typeof(double));
                    // Thêm vào bảng băm các giá bán tương ứng
                    tourSalePrices.Add(role.Id, Module.TourSalePriceGet(tour, numberOfCustomer, role.Id));
                }

                int currentRow = 0;
                if (Module.HotelSection != null)
                {
                    TourModuleBase hotelModule =
                        _moduleLoader.GetModuleFromSection(Module.HotelSection) as TourModuleBase;
                    if (hotelModule != null)
                    {
                        hotelModule.ExportTourConfigToDataTable(_tourId, table, list, numberOfCustomer);
                    }
                    double[] hotel = Calculate(table, currentRow, table.Rows.Count);
                    tourPrice.TourNetHotelPrice = hotel[3];
                    int ii = 4;
                    foreach (Role role in list)
                    {
                        ((TourSalePrice)tourSalePrices[role.Id]).HotelPrice = hotel[ii];
                        ii++;
                    }
                    currentRow = table.Rows.Count - 1;
                }

                if (Module.RestaurantSection != null)
                {
                    TourModuleBase restaurantModule =
                        _moduleLoader.GetModuleFromSection(Module.RestaurantSection) as TourModuleBase;
                    if (restaurantModule != null)
                    {
                        restaurantModule.ExportTourConfigToDataTable(_tourId, table, list, numberOfCustomer);
                    }
                    double[] meal = Calculate(table, currentRow, table.Rows.Count);
                    tourPrice.TourNetMealPrice = meal[3];
                    int ii = 4;
                    foreach (Role role in list)
                    {
                        ((TourSalePrice)tourSalePrices[role.Id]).MealPrice = meal[ii];
                        ii++;
                    }
                    currentRow = table.Rows.Count - 1;
                }

                if (Module.TransportSection != null)
                {
                    TourModuleBase transportModule =
                        _moduleLoader.GetModuleFromSection(Module.TransportSection) as TourModuleBase;
                    if (transportModule != null)
                    {
                        transportModule.ExportTourConfigToDataTable(_tourId, table, list, numberOfCustomer);
                    }
                    double[] transport = Calculate(table, currentRow, table.Rows.Count);
                    tourPrice.TourNetTransportPrice = transport[3];
                    int ii = 4;
                    foreach (Role role in list)
                    {
                        ((TourSalePrice)tourSalePrices[role.Id]).TransportPrice = transport[ii];
                        ii++;
                    }
                    currentRow = table.Rows.Count - 1;
                }

                if (Module.LandscapeSection != null)
                {
                    TourModuleBase landscapeModule =
                        _moduleLoader.GetModuleFromSection(Module.LandscapeSection) as TourModuleBase;
                    if (landscapeModule != null)
                    {
                        landscapeModule.ExportTourConfigToDataTable(_tourId, table, list, numberOfCustomer);
                    }
                    double[] entrancefee = Calculate(table, currentRow, table.Rows.Count);
                    tourPrice.TourNetEntranceFeePrice = entrancefee[3];
                    int ii = 4;
                    foreach (Role role in list)
                    {
                        ((TourSalePrice)tourSalePrices[role.Id]).EntranceFeePrice = entrancefee[ii];
                        ii++;
                    }
                    currentRow = table.Rows.Count - 1;
                }

                if (Module.GuideSection != null)
                {
                    TourModuleBase guideModule =
                        _moduleLoader.GetModuleFromSection(Module.GuideSection) as TourModuleBase;
                    if (guideModule != null)
                    {
                        guideModule.ExportTourConfigToDataTable(_tourId, table, list, numberOfCustomer);
                    }

                    double[] guide = Calculate(table, currentRow, table.Rows.Count);
                    tourPrice.TourNetGuidesPrice = guide[3];
                    int ii = 4;
                    foreach (Role role in list)
                    {
                        ((TourSalePrice)tourSalePrices[role.Id]).GuidePrice = guide[ii];
                        ii++;
                    }
                    currentRow = table.Rows.Count - 1;
                }

                if (Module.BoatSection != null)
                {
                    TourModuleBase boatModule = _moduleLoader.GetModuleFromSection(Module.BoatSection) as TourModuleBase;
                    if (boatModule != null)
                    {
                        boatModule.ExportTourConfigToDataTable(_tourId, table, list, numberOfCustomer);
                    }
                    double[] boat = Calculate(table, currentRow, table.Rows.Count);
                    tourPrice.TourNetBoatPrice = boat[3];
                    int ii = 4;
                    foreach (Role role in list)
                    {
                        ((TourSalePrice)tourSalePrices[role.Id]).BoatPrice = boat[ii];
                        ((TourSalePrice)tourSalePrices[role.Id]).OtherPrice = 0;
                        ii++;
                    }
                    //currentRow = table.Rows.Count - 1;
                }

                tourPrice.TourNetOtherPrice = 0;

                DataRow row = table.NewRow();
                double[] total = new double[table.Columns.Count];
                foreach (DataRow datarow in table.Rows)
                {
                    for (int ii = 2; ii < table.Columns.Count; ii++)
                    {
                        if (!string.IsNullOrEmpty(datarow[ii].ToString()))
                        {
                            total[ii] += Convert.ToDouble(datarow[ii]);
                        }
                    }
                }
                tourPrice.TotalNet = total[3];

                int jj = 4;
                foreach (Role role in list)
                {
                    ((TourSalePrice)tourSalePrices[role.Id]).Total = total[jj];
                    ((TourSalePrice)tourSalePrices[role.Id]).NumberOfCustomers = numberOfCustomer;
                    ((TourSalePrice)tourSalePrices[role.Id]).LastCalculateDate = DateTime.Now;
                    ((TourSalePrice)tourSalePrices[role.Id]).ExtraFee = 0;
                    ((TourSalePrice)tourSalePrices[role.Id]).Tour = tour;
                    ((TourSalePrice)tourSalePrices[role.Id]).RoleId = role.Id;
                    Module.SaveOrUpdate((TourSalePrice)tourSalePrices[role.Id]);
                    jj++;
                }

                for (int ii = 2; ii < table.Columns.Count; ii++)
                {
                    row[ii] = total[ii];
                }
                table.Rows.Add(row);
                tourPrice.NumberOfCustomers = numberOfCustomer;
                tourPrice.LastCaculateDate = DateTime.Now;
                tourPrice.ExtraFee = 0;
                tourPrice.Tour = tour;
                Module.SaveOrUpdate(tourPrice);
            }
            ShowMessage("Calculating completed!");
            //DataExports.ExportToExcel(Context, table, "Report", "report.xls");
        }

        protected void buttonMiscellaneous_Click(object sender, EventArgs e)
        {
            if (Request.RawUrl.ToLower().Contains("tab="))
            {
                PageRedirect(Request.RawUrl.Replace("tab=" + Request.QueryString["tab"],
                                                    "tab=" + "others"));
            }
            else
            {
                PageRedirect(Request.RawUrl + "&tab=" + "others");
            }
        }

        protected void buttonRelated_Click(object sender, EventArgs e)
        {
            if (Request.RawUrl.ToLower().Contains("tab="))
            {
                PageRedirect(Request.RawUrl.Replace("tab=" + Request.QueryString["tab"],
                                                    "tab=" + "related"));
            }
            else
            {
                PageRedirect(Request.RawUrl + "&tab=" + "related");
            }
        }
    }
}