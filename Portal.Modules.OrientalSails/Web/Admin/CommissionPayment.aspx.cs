using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CMS.Core.Domain;
using CMS.Web.Util;
using GemBox.Spreadsheet;
using log4net;
using NHibernate.Criterion;
using NPOI.HSSF.Model;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.UI;
using Portal.Modules.OrientalSails.Web.Util;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class CommissionPayment : SailsAdminBasePage
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(CommissionPayment));
        private IList _agencies;
        private double _currentRate;

        private int _adult;
        private int _baby;
        private int _child;

        private double _total;
        private double _totalVND;
        private double _paid;
        private double _paidBase;
        private double _receivable;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Title = @"Công nợ commission";
                if (!IsPostBack)
                {
                    #region -- Bind sales --
                    Role role = Module.RoleGetById(Convert.ToInt32(Module.ModuleSettings("Sale")));
                    ddlSales.DataSource = role.Users;
                    ddlSales.DataTextField = "Username";
                    ddlSales.DataValueField = "Id";
                    ddlSales.DataBind();
                    ddlSales.Items.Insert(0, new ListItem("-- unbound sale --", "0"));
                    ddlSales.Items.Insert(0, "-- All --");
                    #endregion
                    LoadInfo();
                    GetDataSource();
                    rptBookingList.DataBind();
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error when Page_load in BookingList", ex);
                ShowError(ex.Message);
            }
        }

        private void LoadInfo()
        {
            if (Request.QueryString["from"] != null)
            {
                txtFrom.Text = DateTime.FromOADate(Convert.ToDouble(Request.QueryString["from"])).ToString("dd/MM/yyyy");
            }

            if (Request.QueryString["to"] != null)
            {
                txtTo.Text = DateTime.FromOADate(Convert.ToDouble(Request.QueryString["to"])).ToString("dd/MM/yyyy");
            }

            if (Request.QueryString["agencyname"] != null)
            {
                txtAgency.Text = Request.QueryString["agencyname"];
            }

            if (Request.QueryString["bookingcode"] != null)
            {
                txtBookingCode.Text = Request.QueryString["bookingcode"];
            }

            if (Request.QueryString["saleid"] != null)
            {
                ddlSales.SelectedValue = Request.QueryString["saleid"];
            }

            if (Request.QueryString["paidon"] != null)
            {
                txtPaidOn.Text = DateTime.FromOADate(Convert.ToDouble(Request.QueryString["paidon"])).ToString("dd/MM/yyyy");
            }
        }

        protected void btnDisplay_Click(object sender, EventArgs e)
        {
            string query = string.Empty;

            if (!string.IsNullOrEmpty(txtFrom.Text))
            {
                query += "&from=" +
                         DateTime.ParseExact(txtFrom.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToOADate();
            }

            if (!string.IsNullOrEmpty(txtTo.Text))
            {
                query += "&to=" + DateTime.ParseExact(txtTo.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToOADate();
            }

            if (!string.IsNullOrEmpty(txtPaidOn.Text))
            {
                query += "&paidon=" + DateTime.ParseExact(txtPaidOn.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToOADate();
            }

            if (!string.IsNullOrEmpty(txtAgency.Text))
            {
                query += "&agencyname=" + txtAgency.Text;
            }

            if (!string.IsNullOrEmpty(txtBookingCode.Text))
            {
                query += "&bookingcode=" + txtBookingCode.Text;
            }

            if (ddlSales.SelectedIndex > 0)
            {
                query += "&saleid=" + ddlSales.SelectedValue;
            }

            PageRedirect(string.Format("CommissionPayment.aspx?NodeId={0}&SectionId={1}{2}", Node.Id, Section.Id, query));
        }

        protected void GetDataSource()
        {
            int count;
            rptBookingList.DataSource = GetData(out count);
        }

        public IList GetAgencies()
        {
            if (_agencies == null)
            {
                if (Request.QueryString["agencyid"] != null)
                {
                    _agencies = new ArrayList();
                    int agencyid = Convert.ToInt32(Request.QueryString["agencyid"]);
                    if (agencyid > 0)
                    {
                        _agencies.Add(Module.AgencyGetById(agencyid));
                    }
                    else
                    {
                        _agencies.Add(new Agency());
                    }
                    return _agencies;
                }

                if (!string.IsNullOrEmpty(txtAgency.Text))
                {
                    _agencies = Module.AgencyGetByName(txtAgency.Text);
                }
                else
                {
                    _agencies = new ArrayList();
                }
            }
            return _agencies;
        }

        protected double GetCurrentRate()
        {
            if (_currentRate <= 0)
            {
                _currentRate = Module.ExchangeGetByDate(DateTime.Now).Rate;
            }
            return _currentRate;
        }

        protected IList GetData(out int count, params Order[] orders)
        {
            ICriterion criterion = SailsModule.IncomeCriterion();
            DateTime from;
            DateTime to;
            if (string.IsNullOrEmpty(txtFrom.Text) || string.IsNullOrEmpty(txtTo.Text))
            {
                from = DateTime.Today.AddDays(-DateTime.Today.Day + 1);
                to = from.AddMonths(1).AddDays(-1);
            }
            else
            {
                from = DateTime.ParseExact(txtFrom.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                to = DateTime.ParseExact(txtTo.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            //if (Request.QueryString["mode"] != "all") // Nếu không phải là mode all thì thêm điều kiện về thời gian
            //{
            txtFrom.Text = from.ToString("dd/MM/yyyy");
            txtTo.Text = to.ToString("dd/MM/yyyy");
            criterion = Expression.And(criterion,
                                       Expression.And(Expression.Ge(Booking.STARTDATE, from),
                                                      Expression.Le(Booking.STARTDATE, to)));
            //}
            //else
            //{
            //criterion = Expression.And(criterion, Expression.Not(Expression.Eq("ComPaid", true)));
            ICriterion commissionCriterion = Expression.Or(Expression.Gt("Commission", (double)0), Expression.Gt("CommissionVND", (double)0));
            criterion = Expression.And(criterion,commissionCriterion);
            //}

            if (Request.QueryString["paidon"] != null)
            {
                criterion = Expression.And(criterion, Expression.Ge("PaidDate", DateTime.FromOADate(Convert.ToDouble(Request.QueryString["paidon"]))));
                criterion = Expression.And(criterion, Expression.Lt("PaidDate", DateTime.FromOADate(Convert.ToDouble(Request.QueryString["paidon"])).AddDays(1)));
            }

            ICriterion agencyCrit = SetCriterion(GetAgencies());
            if (agencyCrit != null)
            {
                criterion = Expression.And(criterion, agencyCrit);
            }

            if (UserIdentity.HasPermission(AccessLevel.Administrator) || Module.PermissionCheck("VIEW_ALLBOOKINGRECEIVABLE", UserIdentity))
            {
                // Nếu có quyền xem hết thì mới để ý đến tham số saleid
                if (Request.QueryString["saleid"] != null)
                {
                    int saleid = Convert.ToInt32(Request.QueryString["saleid"]);
                    if (saleid > 0)
                    {
                        criterion = Expression.And(criterion, Expression.Eq("agency.Sale", Module.UserGetById(saleid)));
                    }
                    else
                    {
                        criterion = Expression.And(criterion, Expression.IsNull("agency.Sale"));
                    }
                }
            }
            else
            {
                criterion = Expression.And(criterion, Expression.Eq("agency.Sale", UserIdentity));
            }

            if (Request.QueryString["bookingcode"] != null)
            {
                string code = Request.QueryString["bookingcode"];
                criterion = SailsModule.AddBookingCodeCriterion(criterion, code);
            }

            Order order = Order.Asc(Booking.STARTDATE);
            if (orders.Length > 0)
            {
                order = orders[0];
            }

            return Module.BookingGetByCriterion(criterion, order, out count, 0, 0, false, UserIdentity);
        }

        public static ICriterion SetCriterion(IList agencies)
        {
            ICriterion criterion = null;
            foreach (Agency agency in agencies)
            {
                if (criterion == null)
                {
                    if (agency.Id > 0)
                    {
                        criterion = Expression.Eq("Agency", agency);
                    }
                    else
                    {
                        criterion = Expression.IsNull("Agency");
                    }
                }
                else
                {
                    criterion = Expression.Or(criterion, Expression.Eq("Agency", agency));
                }
            }
            return criterion;
        }

        protected void rptBookingList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is Booking)
            {
                Booking booking = (Booking)e.Item.DataItem;

                #region -- Thông tin chung --
                HyperLink hplCode = e.Item.FindControl("hplCode") as HyperLink;
                if (hplCode != null)
                {
                    hplCode.Text = string.Format(BookingFormat, booking.Id);
                    hplCode.NavigateUrl = string.Format("BookingView.aspx?NodeId={0}&SectionId={1}&BookingId={2}",
                                                        Node.Id, Section.Id, booking.Id);
                }

                HyperLink hplCruise = e.Item.FindControl("hplCruise") as HyperLink;
                if (hplCruise != null)
                {
                    if (booking.Cruise != null)
                    {
                        hplCruise.Text = booking.Cruise.Name;
                        hplCruise.NavigateUrl = string.Format("CommissionPayment.aspx?NodeId={0}&SectionId={1}&cruiseid={2}", Node.Id,
                                              Section.Id,
                                              booking.Cruise.Id);
                    }
                }

                #region -- Others --

                Literal litDate = e.Item.FindControl("litDate") as Literal;
                if (litDate != null)
                {
                    litDate.Text = booking.StartDate.ToString("dd/MM/yyyy");
                }

                HyperLink hyperLink_Partner = e.Item.FindControl("hyperLink_Partner") as HyperLink;
                if (hyperLink_Partner != null)
                {
                    if (booking.Agency != null)
                    {
                        hyperLink_Partner.Text = booking.Agency.Name;
                        DateTime from;
                        DateTime to;
                        try
                        {
                            from = DateTime.ParseExact(txtFrom.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            to = DateTime.ParseExact(txtTo.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            hyperLink_Partner.NavigateUrl =
                                string.Format(
                                    "CommissionPayment.aspx?NodeId={0}&SectionId={1}&agencyid={2}&from={3}&to={4}", Node.Id,
                                    Section.Id, booking.Agency.Id, from.ToOADate(), to.ToOADate());
                        }
                        catch
                        {
                            hyperLink_Partner.NavigateUrl =
                                string.Format("CommissionPayment.aspx?NodeId={0}&SectionId={1}&AgencyId={2}", Node.Id,
                                              Section.Id,
                                              booking.Agency.Id);
                        }
                    }
                    else
                    {
                        hyperLink_Partner.Text = SailsModule.NOAGENCY;
                    }
                }

                Literal litService = e.Item.FindControl("litService") as Literal;
                if (litService != null)
                {
                    litService.Text = booking.Trip.TripCode;
                }

                #endregion

                #region -- Number of pax --

                Label label_NoOfAdult = e.Item.FindControl("label_NoOfAdult") as Label;
                if (label_NoOfAdult != null)
                {
                    label_NoOfAdult.Text = booking.Adult.ToString();
                }

                Label label_NoOfChild = e.Item.FindControl("label_NoOfChild") as Label;
                if (label_NoOfChild != null)
                {
                    label_NoOfChild.Text = booking.Child.ToString();
                }

                Label label_NoOfBaby = e.Item.FindControl("label_NoOfBaby") as Label;
                if (label_NoOfBaby != null)
                {
                    label_NoOfBaby.Text = booking.Baby.ToString();
                }

                _adult += booking.Adult;
                _child += booking.Child;
                _baby += booking.Baby;

                #endregion
                #endregion

                #region -- Paid/Total --

                Label label_TotalPrice = e.Item.FindControl("label_TotalPrice") as Label;
                if (label_TotalPrice != null)
                {
                    if (booking.Commission > 0)
                    {
                        label_TotalPrice.Text = booking.Commission.ToString("#,###");
                    }
                    else
                    {
                        label_TotalPrice.Text = @"0";
                    }
                }

                Label lblTotalPriceVND = e.Item.FindControl("lblTotalPriceVND") as Label;
                if (lblTotalPriceVND != null)
                {
                    if (booking.CommissionVND > 0)
                    {
                        lblTotalPriceVND.Text = booking.CommissionVND.ToString("#,###");
                    }
                    else
                    {
                        lblTotalPriceVND.Text = @"0";
                    }
                }


                #endregion

                #region -- Editable --

                HtmlAnchor aPayment = e.Item.FindControl("aPayment") as HtmlAnchor;
                if (aPayment != null)
                {
                    string url = string.Format("CommissionEdit.aspx?NodeId={0}&SectionId={1}&BookingId={2}", Node.Id,
                                               Section.Id, booking.Id);
                    aPayment.Attributes.Add("onclick",
                                            CMS.ServerControls.Popup.OpenPopupScript(url, "Payment", 300, 400));
                }

                Literal litPaid = e.Item.FindControl("litPaid") as Literal;
                if (litPaid != null)
                {
                    litPaid.Text = booking.ComUSDpayed.ToString("#,0.#");
                }

                Literal litPaidBase = e.Item.FindControl("litPaidBase") as Literal;
                if (litPaidBase != null)
                {
                    litPaidBase.Text = booking.ComVNDpayed.ToString("#,0.#");
                }

                Literal litCurrentRate = e.Item.FindControl("litCurrentRate") as Literal;
                if (litCurrentRate != null)
                {
                    if (booking.ComRate > 0)
                    {
                        litCurrentRate.Text = booking.ComRate.ToString("#,0.#");
                    }
                    else
                    {
                        booking.ComRate = GetCurrentRate();
                        litCurrentRate.Text = booking.ComRate.ToString("#,0.#");
                    }
                }

                Literal litReceivable = e.Item.FindControl("litReceivable") as Literal;
                if (litReceivable != null)
                {
                    litReceivable.Text = booking.CommissionLeft.ToString("#,0.#");
                }
                _total += booking.Commission;
                _totalVND += booking.CommissionVND;
                _paid += booking.ComUSDpayed;
                _paidBase += booking.ComVNDpayed;
                _receivable += booking.CommissionLeft;

                #endregion

                #region -- Tô màu --

                HtmlTableRow trItem = e.Item.FindControl("trItem") as HtmlTableRow;
                if (trItem != null)
                {
                    string color = string.Empty;
                    if (booking.Agency != null && booking.Agency.PaymentPeriod != PaymentPeriod.Monthly)
                    {
                        color = SailsModule.WARNING;
                    }
                    if (booking.IsPaymentNeeded)
                    {
                        color = SailsModule.IMPORTANT;
                    }
                    if (booking.ComPaid)
                    {
                        color = SailsModule.GOOD;
                    }
                    if (!string.IsNullOrEmpty(color))
                    {
                        trItem.Attributes.Add("style", "background-color:" + color);
                    }
                }

                #endregion

                Literal litSaleInCharge = e.Item.FindControl("litSaleInCharge") as Literal;
                if (litSaleInCharge != null)
                {
                    if (booking.Agency != null && booking.Agency.Sale != null)
                    {
                        litSaleInCharge.Text = booking.Agency.Sale.UserName;
                    }
                }

                Literal litTACode = e.Item.FindControl("litTACode") as Literal;
                if (litTACode != null)
                {
                    if (!string.IsNullOrEmpty(booking.AgencyCode))
                    {
                        litTACode.Text = booking.AgencyCode;
                    }
                }

                Literal litBooker = e.Item.FindControl("litBooker") as Literal;
                if (litBooker != null)
                {
                    if (booking.Booker != null)
                    {
                        litBooker.Text = booking.Booker.Name;
                    }
                }

                if (booking.ComPaidDate.HasValue)
                {
                    ValueBinder.BindLiteral(e.Item, "litPaidOn", booking.ComPaidDate.Value);
                }
            }
            else
            {
                if (e.Item.ItemType == ListItemType.Footer)
                {
                    #region -- get control --

                    Label label_NoOfAdult = (Label)e.Item.FindControl("label_NoOfAdult");
                    Label label_NoOfChild = (Label)e.Item.FindControl("label_NoOfChild");
                    Label label_NoOfBaby = (Label)e.Item.FindControl("label_NoOfBaby");
                    Label label_TotalPrice = (Label)e.Item.FindControl("label_TotalPrice");
                    Label label_TotalPriceVND = (Label)e.Item.FindControl("label_TotalPriceVND");
                    Literal litPaid = (Literal)e.Item.FindControl("litPaid");
                    Literal litPaidBase = (Literal)e.Item.FindControl("litPaidBase");
                    Literal litReceivable = (Literal)e.Item.FindControl("litReceivable");

                    #endregion

                    #region -- set value --

                    label_NoOfAdult.Text = _adult.ToString();
                    label_NoOfChild.Text = _child.ToString();
                    label_NoOfBaby.Text = _baby.ToString();
                    label_TotalPrice.Text = _total.ToString("#,###");
                    label_TotalPriceVND.Text = _totalVND.ToString("#,###");
                    if (_paid > 0)
                    {
                        litPaid.Text = _paid.ToString("#,###");
                    }
                    else
                    {
                        litPaid.Text = "0";
                    }

                    litPaidBase.Text = _paidBase.ToString("#,0.#");

                    if (_receivable > 0)
                    {
                        litReceivable.Text = _receivable.ToString("#,###");
                    }
                    else
                    {
                        litReceivable.Text = "0";
                    }

                    #endregion

                    HtmlAnchor aPayment = e.Item.FindControl("aPayment") as HtmlAnchor;
                    if (aPayment != null)
                    {
                        string from = Request.QueryString["from"];
                        string to = Request.QueryString["to"];
                        if (string.IsNullOrEmpty(from))
                        {
                            var date = DateTime.Today.AddDays(-DateTime.Today.Day + 1);
                            from = date.ToOADate().ToString();
                            to = date.AddMonths(1).AddDays(-1).ToOADate().ToString();
                        }
                        string url;
                        if (Request.QueryString["agencyid"] != null)
                        {
                            url =
                                string.Format(
                                    "CommissionEdit.aspx?NodeId={0}&SectionId={1}&agencyid={4}&from={2}&to={3}",
                                    Node.Id, Section.Id, from, to, Request.QueryString["agencyid"]);
                        }
                        else if (Request.QueryString["agencyname"] != null)
                        {
                            url =
                                string.Format(
                                    "CommissionEdit.aspx?NodeId={0}&SectionId={1}&agencyname={4}&from={2}&to={3}",
                                    Node.Id, Section.Id, from, to, Request.QueryString["agencyname"]);
                        }
                        else
                        {
                            url = string.Format("CommissionEdit.aspx?NodeId={0}&SectionId={1}&from={2}&to={3}", Node.Id,
                                                Section.Id, from, to);
                        }
                        aPayment.Attributes.Add("onclick",
                                                CMS.ServerControls.Popup.OpenPopupScript(url, "Payment", 300, 400));
                        
                    }
                }
            }
        }

        #region -- Xuất công nợ theo đối tác --
        /// <summary>
        /// Xuất công nợ, lọc theo 1 đối tác = 1 sheet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExport_Click(object sender, EventArgs e)
        {
            if (!Module.PermissionCheck(Permission.ACTION_EXPORTCONGNO, UserIdentity))
            {
                ShowError("You do not have permission to use this function!");
                return;
            }
            // Bắt đầu thao tác export            
            ExcelFile excelFile = new ExcelFile();
            excelFile.LoadXls(Server.MapPath("/Modules/Sails/Admin/ExportTemplates/Commission.xls"));

            ExcelWorksheet sheet;
            ExcelWorksheet SheetTotal;

            #region -- Lấy các dữ liệu chuẩn bị --

            // Lấy danh sách agency
            IList agencyList = GetAgencies();
            int count;
            //TODO: Sort theo start date
            IList fullList = GetData(out count, Order.Asc("Booker")); // Danh sách toàn bộ booking

            try
            {
                if (agencyList.Count == 0)
                {
                    foreach (Booking booking in fullList)
                    {
                        if (booking.Agency == null)
                        {
                            continue;
                        }
                        if (!agencyList.Contains(booking.Agency))
                        {
                            agencyList.Add(booking.Agency);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot get agency list", ex);
            }

            DateTime from;
            DateTime to;
            if (string.IsNullOrEmpty(txtFrom.Text) || string.IsNullOrEmpty(txtTo.Text))
            {
                from = DateTime.Today.AddDays(-DateTime.Today.Day + 1);
                to = from.AddMonths(1).AddDays(-1);
            }
            else
            {
                from = DateTime.ParseExact(txtFrom.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                to = DateTime.ParseExact(txtTo.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }


            string time;
            if (from.AddMonths(1).AddDays(-1) == to)
            {
                time = from.ToString("MMM - yyyy");
            }
            else
            {
                time = string.Format("{0:dd/MM/yyyy} - {1:dd/MM/yyyy}", from, to);
            }

            #endregion

            USDRate currentRate = Module.ExchangeGetByDate(DateTime.Now);
            if (currentRate == null)
            {
                ShowError("Exchanged rate haven't been configurated!");
            }

            SheetTotal = excelFile.Worksheets.AddCopy("Total", excelFile.Worksheets[0]); //Để tạo sheet Total là sheet đầu tiên trong file công nợ
            ExportAgencyData(currentRate, null, fullList, SheetTotal, time);

            foreach (Agency agency in agencyList)
            {
                 
                sheet = excelFile.Worksheets.AddCopy(string.Format("{0} ({1})", agency.Name, agency.Id), excelFile.Worksheets[0]);
                sheet.Columns[3].Delete();    //Để xóa cột partner trong sheet mẫu
                // Tạo sheet mới, sao chép nguyên từ sheet cũ, số lượng sheet = số lượng agency

                IList list = new ArrayList();

                // Chỉ lấy các booking chưa trả hết nợ của agency này
                foreach (Booking booking in fullList)
                {

                    if (booking.Agency != agency)
                    {
                        continue;
                    }
                    // Chỉ loại trừ khi nợ đúng bằng 0
                    list.Add(booking);
                }

                ExportAgencyData(currentRate, agency, list, sheet, time);
            }

            #region -- Trả dữ liệu về cho người dùng --

            // Xóa sheet mẫu
            if (excelFile.Worksheets.Count > 1)
            {
                excelFile.Worksheets[0].Delete();
            }

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AppendHeader("content-disposition", "attachment; filename=" + string.Format("Congno{0}.xls", time));

            MemoryStream m = new MemoryStream();

            excelFile.SaveXls(m);

            Response.OutputStream.Write(m.GetBuffer(), 0, m.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();

            m.Close();
            Response.End();

            #endregion
        }

        /// <summary>
        /// Xuất thông tin theo một đại lý
        /// </summary>
        /// <param name="currentRate">Tỉ giá hiện tại (mặc định cho các booking chưa áp tỉ giá</param>
        /// <param name="agency">Đối tác cần xuất công nợ</param>
        /// <param name="list">Các booking của đối tác xuất công nợ</param>
        /// <param name="sheet">Sheet sẽ chứa dữ liệu xuất</param>
        /// <param name="time">Thời điểm của báo cáo</param>
        private void ExportAgencyData(USDRate currentRate, Agency agency, IList list, ExcelWorksheet sheet, string time)
        {
            #region -- Thông tin chung --

            //Agency agency = Module.AgencyGetById(Convert.ToInt32(ddlAgencies.SelectedValue));

            sheet.Cells["G1"].Value = time;
            if (agency != null)
            {
                sheet.Cells["C5"].Value = agency.Accountant + " " + agency.Name;
                sheet.Cells["C6"].Value = agency.Address;
                sheet.Cells["C7"].Value = agency.Phone;
                sheet.Cells["H7"].Value = agency.Fax;
            }

            // Tổng ban đầu = 0
            double _totalReceivables = 0;

            // Đối với mỗi booking trong danh sách export, cộng thêm giá trị còn lại
            foreach (Booking booking in list)
            {
                // Nếu chưa có tỉ giá thì phải chia cho tỉ giá mặc định
                if (booking.ComRate > 0)
                {
                    _totalReceivables += booking.CommissionLeft / booking.ComRate; // In USD
                }
                else
                {
                    _totalReceivables += booking.CommissionLeft / currentRate.Rate;
                }
            }

            //TODO: check lại phương pháp tính tổng
            //sheet.Cells["L14"].Value = _totalReceivables;
            //sheet.Cells["N14"].Value = _totalReceivables * currentRate.Rate;

            sheet.Cells["M21"].Value = UserIdentity.FullName;
            if (agency != null && agency.Sale != null)
            {
                sheet.Cells["B21"].Value = agency.Sale.FullName;
            }


            #endregion

            #region -- Các giá trị dùng chung --

            // Lấy giá theo phòng
            Room room = null;

            // Chính sách giá cho đại lý hiện tại
            IList _policies;
            // Nếu là export công nợ đại lý và đại lý có role
            if (agency != null && agency.Role != null)
            {
                _policies = Module.AgencyPolicyGetByRole(agency.Role);
            }
            // Nếu không phải công nợ đại lý hoặc không có role thì lấy giá anonymous
            else
            {
                _policies = Module.AgencyPolicyGetByRole(Module.RoleGetById(4));
            }

            #endregion

            // Sao chép dòng đầu theo số lượng booking
            // Dòng đầu tiên là dòng 11
            const int firstrow = 12;

            sheet.Rows[firstrow].InsertCopy(list.Count - 1, sheet.Rows[firstrow]);

            #region -- Thông tin từng booking --

            // Ghi vào file excel theo từng dòng
            int current = firstrow;
            double totalCommission = 0.0;
            double totalCommissionIsVND = 0.0;
            double totalComissionVND = 0.0;
            foreach (Booking booking in list)
            {
                int index = current - firstrow + 1;
                InsertBookingData(currentRate, sheet, current, index, booking, room, _policies);
                totalCommission = totalCommission + booking.Commission;
                totalCommissionIsVND = totalCommissionIsVND + booking.CommissionVND;
                totalComissionVND = totalComissionVND + (booking.Commission * booking.ComRate + booking.CommissionVND);
                current += 1;
            }
            if (sheet.Name == "Total")
            {
                sheet.Cells["K" + (current + 1)].Value = "$ " + totalCommission.ToString("#,0.#");
                sheet.Cells["L" + (current + 1)].Value = "VND " + totalCommissionIsVND.ToString("#,0.#");
                sheet.Cells["N" + (current + 1)].Value = "VND " + totalComissionVND.ToString("#,0.#");
            }
            else
            {
                sheet.Cells["J" + (current + 1)].Value = "$ " + totalCommission.ToString("#,0.#");
                sheet.Cells["K" + (current + 1)].Value = "VND " + totalCommissionIsVND.ToString("#,0.#");
                sheet.Cells["M" + (current + 1)].Value = "VND " + totalComissionVND.ToString("#,0.#");
            }

            #endregion
        }

        private void InsertBookingData(USDRate currentRate, ExcelWorksheet sheet, int current, int index,
            Booking booking, Room room, IList _policies)
        {
            sheet.Cells[current, 0].Value = index; // Cột index
            if (!string.IsNullOrEmpty(booking.AgencyCode))
            {
                sheet.Cells[current, 1].Value = booking.AgencyCode;
            }
            else
            {
                sheet.Cells[current, 1].Value = string.Format(BookingFormat, booking.Id);
            }

            if (booking.Booker != null)
            {
                sheet.Cells[current, 2].Value = booking.Booker.Name; // Cột booker
            }

            if (sheet.Name == "Total") //Để chỉnh sửa cột dữ liệu sheet total và các sheet khác cho phù hợp
            {
                if (booking.Agency != null)
                {
                    sheet.Cells[current, 3].Value = booking.Agency.Name;
                }

                sheet.Cells[current, 4].Value = booking.StartDate; // Check in
                sheet.Cells[current, 5].Value = booking.StartDate.AddDays(booking.Trip.NumberOfDay - 1);
                if (booking.Trip.NumberOfOptions <= 1)
                {
                    sheet.Cells[current, 6].Value = booking.Trip.TripCode;
                }
                else
                {
                    sheet.Cells[current, 6].Value = booking.Trip.TripCode + booking.TripOption;
                }
                sheet.Cells[current, 7].Value = booking.Adult; // Số người lớn
                sheet.Cells[current, 8].Value = booking.Child; // Số trẻ em

                if (booking.ComRate == 0)
                {
                    booking.ComRate = currentRate.Rate;
                }
                sheet.Cells[current, 10].Value = booking.Commission.ToString("#,0.#");
                sheet.Cells[current, 11].Value = booking.CommissionVND.ToString("#,0.#"); 
                sheet.Cells[current, 12].Value = booking.ComRate;
                sheet.Cells[current, 13].Value = booking.Commission * booking.ComRate + booking.CommissionVND;
                sheet.Cells[current, 14].Value = booking.Note;
                sheet.Cells[current, 15].Value = booking.ComPaid ? "Paid" : "UnPaid";
            }
            else
            {
                sheet.Cells[current, 3].Value = booking.StartDate; // Check in
                sheet.Cells[current, 4].Value = booking.StartDate.AddDays(booking.Trip.NumberOfDay - 1);
                if (booking.Trip.NumberOfOptions <= 1)
                {
                    sheet.Cells[current, 5].Value = booking.Trip.TripCode;
                }
                else
                {
                    sheet.Cells[current, 5].Value = booking.Trip.TripCode + booking.TripOption;
                }
                sheet.Cells[current, 6].Value = booking.Adult; // Số người lớn
                sheet.Cells[current, 7].Value = booking.Child; // Số trẻ em

                if (booking.ComRate == 0)
                {
                    booking.ComRate = currentRate.Rate;
                }
                sheet.Cells[current, 9].Value = booking.Commission.ToString("#,0.#");
                sheet.Cells[current, 10].Value = booking.CommissionVND.ToString("#,0.#");
                sheet.Cells[current, 11].Value = booking.ComRate;
                sheet.Cells[current, 12].Value = booking.Commission * booking.ComRate + booking.CommissionVND;
                sheet.Cells[current, 13].Value = booking.Note;
                sheet.Cells[current, 14].Value = booking.ComPaid ? "Paid" : "UnPaid";
            }
        }
        #endregion
    }
}
