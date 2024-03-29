using System;
using System.Collections;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;
using CMS.Core.Util;
using CMS.ServerControls;
using log4net;
using NHibernate;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.UI;
using Portal.Modules.OrientalSails.Web.Util;
using System.Web.UI.HtmlControls;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class SailsPriceConfig : SailsAdminBasePage
    {
        #region -- Private Member --

        private readonly ILog _logger = LogManager.GetLogger(typeof(SailsPriceConfig));
        public SailsTrip _trip { set; get; }

        private int TripId
        {
            get
            {
                int id;
                if (Request.QueryString["TripId"] != null && Int32.TryParse(Request.QueryString["TripId"], out id))
                {
                    return id;
                }
                return -1;
            }
        }

        private int _tableId
        {
            get
            {
                if (ViewState["TableId"] != null)
                {
                    return Convert.ToInt32(ViewState["TableId"]);
                }
                return -1;
            }
            set
            {
                ViewState["TableId"] = value;
            }
        }

        private SailsPriceTable _table;

        public TripOption Option
        {
            get
            {
                if (Request.QueryString["Option"] != null)
                {
                    switch (Request.QueryString["Option"])
                    {
                        case "2":
                            return TripOption.Option2;
                        case "3":
                            return TripOption.Option3;
                        default:
                            return TripOption.Option1;
                    }
                }
                return TripOption.Option1;
            }
        }

        public SailsPriceTable Table
        {
            get
            {
                if (_table == null)
                {
                    _table = Module.PriceTableGetById(_tableId);
                }
                return _table;
            }
        }

        private Cruise _activeCruise;
        private Cruise ActiveCruise
        {
            get
            {
                if (_activeCruise == null)
                {
                    if (Request.QueryString["cruiseid"] != null)
                    {
                        _activeCruise = Module.CruiseGetById(Convert.ToInt32(Request.QueryString["cruiseid"]));
                    }
                    else
                    {
                        _activeCruise = Module.CruiseGetById(3);
                    }
                }

                return _activeCruise;
            }
        }

        private Agency _agency;
        private Agency ActiveAgency
        {
            get
            {
                if (_agency == null && Request.QueryString["agencyid"] != null)
                {
                    _agency = Module.AgencyGetById(Convert.ToInt32(Request.QueryString["agencyid"]));
                }
                return _agency;
            }
        }

        public String Url
        {
            get
            {
                var nvc = HttpUtility.ParseQueryString(Request.Url.Query);
                nvc.Remove("validfrom");
                return Request.Url.AbsolutePath + "?" + nvc.ToString();
            }
        }

        public DateTime ValidFrom
        {
            get
            {
                if (String.IsNullOrEmpty(Request.QueryString["validfrom"]))
                    return DateTime.Now.Date;
                else
                    return DateTime.ParseExact(Request.QueryString["validfrom"], "ddMMyyyy", CultureInfo.InvariantCulture);
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = Resources.titleSailsPriceConfig;
            if (TripId <= 0)
            {
                ShowError(Resources.stringAccessDenied);
                return;
            }
            _trip = Module.TripGetById(TripId);

            int option = 0;
            if (Request.QueryString["option"] != null)
            {
                option = Convert.ToInt32(Request.QueryString["option"]);
            }
            if (option == 0)
            {
                option = 1;
            }
            if (ActiveAgency != null)
            {
                titleSailsPriceConfig.Text = string.Format("Price config for {0}, option {1} on {2} of {3}", _trip.Name, option, ActiveCruise.Name, ActiveAgency.Name);
            }
            else
            {
                titleSailsPriceConfig.Text = string.Format("Price config for {0}, option {1} on {2}", _trip.Name, option, ActiveCruise.Name);
            }

            if (!IsPostBack)
            {
                Domain.SailsPriceConfig priceConfig = Module.SailsPriceConfigGet(_trip, ValidFrom.Date);
                txtPriceAdultUSD.Text = priceConfig.PriceAdultUSD.ToString("#,0.#");
                txtPriceAdultVND.Text = priceConfig.PriceAdultVND.ToString("#,0.#");
                txtPriceChildUSD.Text = priceConfig.PriceChildUSD.ToString("#,0.#");
                txtPriceChildVND.Text = priceConfig.PriceChildVND.ToString("#,0.#");
                txtPriceBabyUSD.Text = priceConfig.PriceBabyUSD.ToString("#,0.#");
                txtPriceBabyVND.Text = priceConfig.PriceBabyVND.ToString("#,0.#");
                textBoxStartDate.Text = ValidFrom.ToString("dd/MM/yyyy");
                rptCommission.DataSource = Module.AgencyLevelGetAll();
                rptCommission.DataBind();
            }
            int count;
            rptValidFrom.DataSource = Module.SailsPriceConfigGetAllBySailsTripAndOption(_trip, Option, pgValidFrom.PageSize, pgValidFrom.CurrentPageIndex, out count);
            pgValidFrom.AllowCustomPaging = true;
            pgValidFrom.VirtualItemCount = count;
            rptValidFrom.DataBind();
        }

        //protected void rptRoomClass_ItemDataBound(object sender, RepeaterItemEventArgs e)
        //{
        //    {
        //        RoomClass roomClass = (RoomClass)e.Item.DataItem;

        //        #region -- Header --

        //        // Đối với header, thêm danh sách roomType thông thường
        //        using (Repeater rpt = e.Item.FindControl("rptRoomTypeHeader") as Repeater)
        //        {
        //            if (rpt != null)
        //            {
        //                rpt.DataSource = Module.RoomTypexGetAll();
        //                rpt.DataBind();
        //            }
        //        }

        //        #endregion

        //        #region -- Item --

        //        #region RoomClass Id

        //        using (Label labelRoomClassId = e.Item.FindControl("labelRoomClassId") as Label)
        //        {
        //            if (labelRoomClassId != null)
        //            {
        //                labelRoomClassId.Text = roomClass.Id.ToString();
        //            }
        //        }

        //        #endregion

        //        //Đối với từng dòng
        //        using (Repeater rpt = e.Item.FindControl("rptRoomTypeCell") as Repeater)
        //        {
        //            if (rpt != null)
        //            {
        //                // Gán sự kiện ItemDataBound (vì control trong Repeater không tự nhận hàm này)
        //                rpt.ItemDataBound += RptRoomTypeItemDataBound;

        //                IList roomTypeList = Module.RoomTypexGetAll();

        //                rpt.DataSource = roomTypeList;
        //                rpt.DataBind();
        //            }
        //        }

        //        #endregion
        //    }
        //}

        //protected void RptRoomTypeItemDataBound(object sender, RepeaterItemEventArgs e)
        //{
        //    RoomTypex rtype = e.Item.DataItem as RoomTypex;
        //    RoomClass father = (RoomClass)(((RepeaterItem)e.Item.Parent.Parent).DataItem);
        //    TextBox txtSingle = (e.Item.Parent.Parent).FindControl("txtSingle") as TextBox;
        //    if (rtype != null)
        //    {
        //        #region RoomType Id

        //        using (Label labelRoomTypeId = e.Item.FindControl("labelRoomTypeId") as Label)
        //        {
        //            if (labelRoomTypeId != null)
        //            {
        //                labelRoomTypeId.Text = rtype.Id.ToString();
        //            }
        //        }

        //        #endregion

        //        TextBox textBoxPrice = (TextBox)e.Item.FindControl("textBoxPrice");
        //        TextBox txtPriceVND = (TextBox)e.Item.FindControl("txtPriceVND");
        //        Label labelSailsPriceConfigId = (Label)e.Item.FindControl("labelSailsPriceConfigId");

        //        //Kiểm tra xem có tồn tại room nào mà class và type là rtype và father ko?
        //        IList room = Module.RoomGetBy_ClassType(ActiveCruise, father, rtype);
        //        //Nếu có thì hiện giá
        //        if (room.Count > 0)
        //        {
        //            Domain.SailsPriceConfig priceConfig = Module.SailsPriceConfigGet(Table, rtype, father);
        //            //Module.SailsPriceConfigGetBy_RoomType_RoomClass_Trip(_trip,rtype,father,Option);
        //            //Nếu có giá thì hiện
        //            if (priceConfig != null)
        //            {
        //                labelSailsPriceConfigId.Text = priceConfig.Id.ToString();


        //                if (txtSingle != null)
        //                {
        //                    txtSingle.Text = priceConfig.SpecialPrice.ToString("#,0.#");
        //                }
        //            }
        //        }
        //        //Nếu không tồn tại room thì để N/A
        //        else
        //        {
        //            textBoxPrice.Enabled = false;
        //            textBoxPrice.Text = "N/A";
        //            txtPriceVND.Enabled = false;
        //            txtPriceVND.Text = "N/A";
        //        }
        //    }
        //}

        protected void buttonSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                bool isvalid = false;
                SailsPriceTable table;
                if (_tableId <= 0)
                {
                    table = new SailsPriceTable();
                }
                else
                {
                    table = Table;
                }
                //table.StartDate = DateTime.ParseExact(textBoxStartDate.Text, "dd/MM/yyyy",
                //                                      CultureInfo.InvariantCulture);
                //table.EndDate = DateTime.ParseExact(textBoxEndDate.Text, "dd/MM/yyyy",
                //                                      CultureInfo.InvariantCulture);
                table.Trip = _trip;
                table.TripOption = Option;
                table.Note = string.Empty;
                table.Agency = ActiveAgency;
                if (ActiveCruise != null)
                {
                    table.Cruise = ActiveCruise;
                }
                else
                {
                    //if (ddlCruises.SelectedIndex > 0)
                    //{
                    //    table.Cruise = Module.CruiseGetById(Convert.ToInt32(ddlCruises.SelectedValue));
                    //}
                    //else
                    //{
                    //    table.Cruise = null;
                    //}
                }

                //foreach (RepeaterItem rptClassItem in rptRoomClass.Items)
                //{
                //    Repeater rptRoomTypeCell = rptClassItem.FindControl("rptRoomTypeCell") as Repeater;
                //    Label labelRoomClassId = rptClassItem.FindControl("labelRoomClassId") as Label;
                //    if (labelRoomClassId != null && labelRoomClassId.Text != string.Empty && rptRoomTypeCell != null)
                //    {
                //        RoomClass roomClass = Module.RoomClassGetById(Convert.ToInt32(labelRoomClassId.Text));

                //        #region -- Kiểm tra tính hợp lệ của bảng giá --

                //        foreach (RepeaterItem priceItem in rptRoomTypeCell.Items)
                //        {
                //            TextBox txtCellPrice = priceItem.FindControl("textBoxPrice") as TextBox;
                //            //Kiểm tra xem textboxPrice có enable ko ( không nghĩa là o tồn tại giá kiểu class và type đó)
                //            if (txtCellPrice != null && txtCellPrice.Enabled)
                //            {
                //                double price;
                //                //kiểm tra xem price có hợp lệ ko
                //                isvalid = double.TryParse(txtCellPrice.Text, out price);
                //                if (!isvalid) break;
                //            }
                //        } 

                //        #endregion

                //        //Nếu bảng giá hợp lệ thì lưu
                //        if (isvalid)
                //        {
                //            Module.SaveOrUpdate(table);

                //            TextBox txtSingle = rptClassItem.FindControl("txtSingle") as TextBox;
                //            double single = 0;
                //            if (txtSingle != null && !string.IsNullOrEmpty(txtSingle.Text))
                //            {
                //                single = Convert.ToDouble(txtSingle.Text);
                //            }

                //            foreach (RepeaterItem priceItem in rptRoomTypeCell.Items)
                //            {
                //                Label labelRoomTypeId = priceItem.FindControl("labelRoomTypeId") as Label;
                //                Label labelSailsPriceConfigId =
                //                    priceItem.FindControl("labelSailsPriceConfigId") as Label;
                //                TextBox textBoxPrice = priceItem.FindControl("textBoxPrice") as TextBox;
                //                TextBox txtPriceVND = priceItem.FindControl("txtPriceVND") as TextBox;
                //                RoomTypex roomType = null;

                //                #region Lấy về RoomType tương ứng để chuẩn bị lưu

                //                if (labelRoomTypeId != null && labelRoomTypeId.Text != string.Empty)
                //                {
                //                    if (Convert.ToInt32(labelRoomTypeId.Text) > 0)
                //                    {
                //                        roomType = Module.RoomTypexGetById(Convert.ToInt32(labelRoomTypeId.Text));
                //                    }
                //                }

                //                #endregion

                //                if ((textBoxPrice != null && textBoxPrice.Enabled) && (txtPriceVND != null && txtPriceVND.Enabled))
                //                {
                //                    double price;
                //                    double priceVND;

                //                    double.TryParse(textBoxPrice.Text, out price);
                //                    double.TryParse(txtPriceVND.Text, out priceVND);

                //                    Domain.SailsPriceConfig rPrice;
                //                    if (labelSailsPriceConfigId != null &&
                //                        !string.IsNullOrEmpty(labelSailsPriceConfigId.Text) &&
                //                        Convert.ToInt32(labelSailsPriceConfigId.Text) > 0)
                //                    {
                //                        //update
                //                        rPrice =
                //                            Module.SailsPriceConfigGetById(Convert.ToInt32(labelSailsPriceConfigId.Text));
                //                    }
                //                    else
                //                    {
                //                        //insert
                //                        rPrice = new Domain.SailsPriceConfig();
                //                        rPrice.RoomType = roomType;
                //                        rPrice.RoomClass = roomClass;
                //                        rPrice.TripOption = Option;
                //                        rPrice.Trip = _trip;
                //                    }

                //                    // Giá single supplement
                //                    rPrice.SpecialPrice = single;
                //                    rPrice.NetPrice = price;
                //                    rPrice.NetPriceVND = priceVND;
                //                    rPrice.Table = table;
                //                    Module.SaveOrUpdate(rPrice);
                //                }
                //            }
                //        }
                //    }
                //}
                Domain.SailsPriceConfig sailsPriceConfig = Module.SailsPriceConfigGetBySailsTripAndOption(_trip, Option, ValidFrom.Date);
                sailsPriceConfig.PriceAdultUSD = double.Parse(txtPriceAdultUSD.Text);
                sailsPriceConfig.PriceAdultVND = double.Parse(txtPriceAdultVND.Text);
                sailsPriceConfig.PriceChildUSD = double.Parse(txtPriceChildUSD.Text);
                sailsPriceConfig.PriceChildVND = double.Parse(txtPriceChildVND.Text);
                sailsPriceConfig.PriceBabyUSD = double.Parse(txtPriceBabyUSD.Text);
                sailsPriceConfig.PriceBabyVND = double.Parse(txtPriceBabyVND.Text);
                sailsPriceConfig.Trip = _trip;
                sailsPriceConfig.TripOption = Option;
                sailsPriceConfig.ValidFrom = ValidFrom;
                Module.SaveOrUpdate(sailsPriceConfig);

                foreach (RepeaterItem item in rptCommission.Items)
                {
                    AgencyCommission agencyCommission;
                    var agencyCommissionIdLabel = item.FindControl("AgencyCommissionId") as Label;

                    if (agencyCommissionIdLabel.Text != "0")
                        agencyCommission = Module.AgencyCommissionGetById(Convert.ToInt32(agencyCommissionIdLabel.Text));
                    else
                    {
                        agencyCommission = new AgencyCommission();
                    }

                    var txtAdultCommissionUSD = item.FindControl("txtAdultCommissionUSD") as TextBox;
                    agencyCommission.CommissionAdultUSD = double.Parse(txtAdultCommissionUSD.Text);

                    var txtAdultCommissionVND = item.FindControl("txtAdultCommissionVND") as TextBox;
                    agencyCommission.CommissionAdultVND = double.Parse(txtAdultCommissionVND.Text);

                    var txtChildCommissionUSD = item.FindControl("txtChildCommissionUSD") as TextBox;
                    agencyCommission.CommissionChildUSD = double.Parse(txtChildCommissionUSD.Text);

                    var txtChildCommissionVND = item.FindControl("txtChildCommissionVND") as TextBox;
                    agencyCommission.CommissionChildVND = double.Parse(txtChildCommissionVND.Text);

                    var txtBabyCommissionUSD = item.FindControl("txtBabyCommissionUSD") as TextBox;
                    agencyCommission.CommissionBabyUSD = double.Parse(txtBabyCommissionUSD.Text);

                    var txtBabyCommissionVND = item.FindControl("txtBabyCommissionVND") as TextBox;
                    agencyCommission.CommissionBabyVND = double.Parse(txtBabyCommissionVND.Text);

                    agencyCommission.ValidFrom = ValidFrom;

                    agencyCommission.SailsTrip = _trip;

                    var agencyLevelIdLabel = item.FindControl("AgencyLevelId") as Label;
                    AgencyLevel agencyLevel = Module.AgencyLevelGetById(Convert.ToInt32(agencyLevelIdLabel.Text));
                    agencyCommission.AgencyLevel = agencyLevel;

                    Module.SaveOrUpdate(agencyCommission);
                }
                Response.Redirect(Request.RawUrl);
            }
            catch (Exception ex)
            {
                _logger.Error("Error when buttonSubmit_Click in SailsPriceConfig", ex);
                ShowError(ex.Message);
            }
        }

        protected void buttonCancel_Click(object sender, EventArgs e)
        {
            PageRedirect(string.Format("SailsTripList.aspx?NodeId={0}&SectionId={1}", Node.Id, Section.Id));
        }

        protected void rptPriceTables_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is SailsPriceTable)
            {
                SailsPriceTable table = (SailsPriceTable)e.Item.DataItem;
                Label labelCruise = e.Item.FindControl("labelCruise") as Label;
                if (labelCruise != null)
                {
                    if (table.Cruise != null)
                    {
                        labelCruise.Text = table.Cruise.Name;
                    }
                }

                Label labelValidFrom = e.Item.FindControl("labelValidFrom") as Label;
                if (labelValidFrom != null)
                {
                    labelValidFrom.Text = table.StartDate.ToString("dd/MM/yyyy");
                }

                Label labelValidTo = e.Item.FindControl("labelValidTo") as Label;
                if (labelValidTo != null)
                {
                    labelValidTo.Text = table.EndDate.ToString("dd/MM/yyyy");
                }

                HyperLink hplEdit = e.Item.FindControl("hplEdit") as HyperLink;
                if (hplEdit != null)
                {
                    hplEdit.NavigateUrl = string.Format("SailsPriceConfig.aspx?NodeId={0}&SectionId={1}&TableId={2}",
                                                        Node.Id, Section.Id, table.Id);
                }
            }
        }

        protected void rptPriceTables_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            _tableId = Convert.ToInt32(e.CommandArgument);
            #region -- Load data --

            //textBoxStartDate.Text = Table.StartDate.ToString("dd/MM/yyyy");
            //textBoxEndDate.Text = Table.EndDate.ToString("dd/MM/yyyy");
            //if (ActiveCruise == null)
            //{
            //    if (Table.Cruise != null)
            //    {
            //        ddlCruises.SelectedValue = Table.Cruise.Id.ToString();
            //    }
            //    else
            //    {
            //        ddlCruises.SelectedIndex = 0;
            //    }
            //}

            //rptRoomClass.DataSource = Module.RoomClassGetAll();
            //rptRoomClass.DataBind();
            #endregion
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            //textBoxStartDate.Text = string.Empty;
            //textBoxEndDate.Text = string.Empty;

            //rptRoomClass.DataSource = Module.RoomClassGetAll();
            //rptRoomClass.DataBind();
        }

        protected void rptCommission_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            SailsTrip sailsTrip = Module.TripGetById(_trip.Id);
            Label agencyLevelIdLabel = e.Item.FindControl("AgencyLevelId") as Label;
            AgencyLevel agencyLevel = Module.AgencyLevelGetById(Convert.ToInt32(agencyLevelIdLabel.Text));
            AgencyCommission agencyCommission = Module.GetAgencyCommissionByTripAndAgencyLevel(sailsTrip, agencyLevel, ValidFrom.Date);

            var txtAdultCommissionUSD = e.Item.FindControl("txtAdultCommissionUSD") as TextBox;
            txtAdultCommissionUSD.Text = agencyCommission.CommissionAdultUSD.ToString("#,0.#");

            var txtAdultCommissionVND = e.Item.FindControl("txtAdultCommissionVND") as TextBox;
            txtAdultCommissionVND.Text = agencyCommission.CommissionAdultVND.ToString("#,0.#");

            var txtChildCommissionUSD = e.Item.FindControl("txtChildCommissionUSD") as TextBox;
            txtChildCommissionUSD.Text = agencyCommission.CommissionChildUSD.ToString("#,0.#");

            var txtChildCommissionVND = e.Item.FindControl("txtChildCommissionVND") as TextBox;
            txtChildCommissionVND.Text = agencyCommission.CommissionChildVND.ToString("#,0.#");

            var txtBabyCommissionUSD = e.Item.FindControl("txtBabyCommissionUSD") as TextBox;
            txtBabyCommissionUSD.Text = agencyCommission.CommissionBabyUSD.ToString("#,0.#");

            var txtBabyCommissionVND = e.Item.FindControl("txtBabyCommissionVND") as TextBox;
            txtBabyCommissionVND.Text = agencyCommission.CommissionBabyVND.ToString("#,0.#");

            var agencyCommissionIdLabel = e.Item.FindControl("AgencyCommissionId") as Label;
            agencyCommissionIdLabel.Text = agencyCommission.Id.ToString();

        }

        protected void rptValidFrom_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var lbtnEdit = e.Item.FindControl("lbtnEdit") as LinkButton;
            var lbtnDelete = e.Item.FindControl("lbtnDelete") as LinkButton;
            var lblValidFrom = e.Item.FindControl("lblValidFrom") as Label;
            lbtnEdit.Text = "Edit";
            lbtnDelete.Text = "Delete";
            if (String.IsNullOrEmpty(Request.QueryString["validfrom"]))
            {
                lbtnEdit.PostBackUrl = Request.RawUrl + "&validfrom=" + lblValidFrom.Text.Replace("/", "");
            }
            else
            {
                lbtnEdit.PostBackUrl = Url + "&validfrom=" + lblValidFrom.Text.Replace("/", "");

            }

        }

        protected void rptValidFrom_OnItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                var validFromString = e.CommandArgument as String;
                DateTime validFrom = DateTime.ParseExact(validFromString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                IList agencyCommissionList = Module.GetAgencyCommissionByValidFrom(_trip, validFrom);
                foreach (object obj in agencyCommissionList)
                {
                    var agencyCommission = (AgencyCommission)obj;
                    Module.Delete(agencyCommission);
                }

                IList sailsPriceConfigList = Module.SailsPriceConfigGetByValidFrom(_trip, Option, validFrom);
                foreach (object obj in sailsPriceConfigList)
                {
                    var sailsPriceConfig = (Domain.SailsPriceConfig)obj;
                    Module.Delete(sailsPriceConfig);
                }

                Response.Redirect(Url);
            }
        }

        protected void pgValidFrom_OnPageChanged(object sender, PageChangedEventArgs e)
        {
            int count;
            rptValidFrom.DataSource = Module.SailsPriceConfigGetAllBySailsTripAndOption(_trip, Option, pgValidFrom.PageSize, pgValidFrom.CurrentPageIndex, out count);
            pgValidFrom.AllowCustomPaging = true;
            pgValidFrom.VirtualItemCount = count;
            rptValidFrom.DataBind();
        }
    }
}