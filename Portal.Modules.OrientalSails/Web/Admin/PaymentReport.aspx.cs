using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CMS.ServerControls;
using CMS.Web.Util;
using GemBox.Spreadsheet;
using log4net;
using NHibernate.Criterion;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.UI;
using Portal.Modules.OrientalSails.Web.Util;
using CMS.Core.Domain;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class PaymentReport : SailsAdminBase
    {
        #region -- PRIVATE MEMBERS --

        private readonly ILog _logger = LogManager.GetLogger(typeof(PaymentReport));
        private int _adult;
        private IList _agencies;
        private int _baby;
        private int _child;
        private double _currentRate;
        private double _paid;
        private double _paidBase;
        private double _receivable;
        private double _total;
        private double _totalVND;
        private double _totalGuideCollect;
        private double _totalGuideCollectVND;

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

        #endregion

        #region -- PAGE EVENTS --

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Title = Resources.titleBookingList;
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

                    #region -- Bind trips --

                    ddlTrips.DataSource = Module.TripGetAll(true, UserIdentity);
                    ddlTrips.DataTextField = "Name";
                    ddlTrips.DataValueField = "Id";
                    ddlTrips.DataBind();
                    ddlTrips.Items.Insert(0, "-- All --");

                    if (Request.QueryString["tripid"] != null)
                    {
                        ddlTrips.SelectedValue = Request.QueryString["tripid"];
                    }
                    #endregion

                    rptOrganization.DataSource = Module.OrganizationGetAllRoot();
                    rptOrganization.DataBind();

                    LoadInfo();

                    #region -- Bind paid status --

                    string url = Request.RawUrl;
                    if (Request.QueryString["paidstatus"] != null)
                    {
                        url = url.Replace("&paidstatus=" + Request.QueryString["paidstatus"], "");
                    }

                    hplAllPaid.NavigateUrl = url + "&paidstatus=all";
                    hplNotPaid.NavigateUrl = url + "&paidstatus=notpaid";
                    hplPaid.NavigateUrl = url + "&paidstatus=paid";
                    #endregion

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

        protected void LoadInfo()
        {
            if (Request.QueryString["from"] != null)
            {
                txtFrom.Text = DateTime.FromOADate(Convert.ToDouble(Request.QueryString["from"])).ToString("dd/MM/yyyy");
            }

            if (Request.QueryString["to"] != null)
            {
                txtTo.Text = DateTime.FromOADate(Convert.ToDouble(Request.QueryString["to"])).ToString("dd/MM/yyyy");
            }

            if (Request.QueryString["paidon"] != null)
            {
                txtPaidOn.Text = DateTime.FromOADate(Convert.ToDouble(Request.QueryString["paidon"])).ToString("dd/MM/yyyy");
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

            // Hiển thị trạng thái hiện tại
            if (Request.QueryString["paidstatus"] != null)
            {
                switch (Request.QueryString["paidstatus"].ToLower())
                {
                    case "paid":
                        hplPaid.CssClass += " selected";
                        break;
                    case "notpaid":
                        hplNotPaid.CssClass += " selected";
                        break;
                    case "all":
                        hplAllPaid.CssClass += " selected";
                        break;
                    default:
                        hplNotPaid.CssClass += " selected"; // Mặc định của trang này là not paid
                        break;
                }
            }
            else
            {
                hplNotPaid.CssClass += " selected";// Mặc định của trang này là not paid
            }
        }

        #endregion

        #region -- CONTROL EVENTS --

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

            if (!string.IsNullOrEmpty(txtAgency.Text))
            {
                query += "&agencyname=" + txtAgency.Text;
            }

            if (!string.IsNullOrEmpty(txtBookingCode.Text))
            {
                query += "&bookingcode=" + txtBookingCode.Text;
            }

            if (!string.IsNullOrEmpty(txtPaidOn.Text))
            {
                query += "&paidon=" + DateTime.ParseExact(txtPaidOn.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToOADate();
            }

            if (ddlSales.SelectedIndex > 0)
            {
                query += "&saleid=" + ddlSales.SelectedValue;
            }

            if (ddlTrips.SelectedIndex > 0)
            {
                query += "&tripid=" + ddlTrips.SelectedValue;
            }
            PageRedirect(string.Format("PaymentReport.aspx?NodeId={0}&SectionId={1}{2}", Node.Id, Section.Id, query));
        }

        protected void rptBookingList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is Booking)
            {
                var booking = (Booking)e.Item.DataItem;

                var hplCode = e.Item.FindControl("hplCode") as HyperLink;
                if (hplCode != null)
                {
                    hplCode.Text = string.Format(BookingFormat, booking.Id);
                    hplCode.NavigateUrl = string.Format("BookingView.aspx?NodeId={0}&SectionId={1}&BookingId={2}",
                                                        Node.Id, Section.Id, booking.Id);
                }

                var hplCruise = e.Item.FindControl("hplCruise") as HyperLink;
                if (hplCruise != null)
                {
                    if (booking.Cruise != null)
                    {
                        hplCruise.Text = booking.Cruise.Name;
                        hplCruise.NavigateUrl = string.Format("PaymentReport.aspx?NodeId={0}&SectionId={1}&cruiseid={2}", Node.Id,
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
                                    "PaymentReport.aspx?NodeId={0}&SectionId={1}&agencyid={2}&from={3}&to={4}", Node.Id,
                                    Section.Id, booking.Agency.Id, from.ToOADate(), to.ToOADate());
                        }
                        catch
                        {
                            hyperLink_Partner.NavigateUrl =
                                string.Format("PaymentReport.aspx?NodeId={0}&SectionId={1}&AgencyId={2}", Node.Id,
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

                ValueBinder.BindLiteral(e.Item, "litGuideCollect", booking.GuideCollect.ToString("#,0.#"));
                ValueBinder.BindLiteral(e.Item, "litGuideCollectVND", booking.GuideCollectVND.ToString("#,0.#"));

                #region -- Paid/Total --

                Label label_TotalPrice = e.Item.FindControl("label_TotalPrice") as Label;
                Label label_TotalPriceVND = e.Item.FindControl("label_TotalPriceVND") as Label;
                if (label_TotalPrice != null)
                {
                    if (booking.Value > 0)
                    {
                        label_TotalPrice.Text = booking.Value.ToString("#,###");
                    }
                    else
                    {
                        label_TotalPrice.Text = "0";
                    }
                }

                if (label_TotalPriceVND != null)
                {
                    if (booking.ValueVND > 0)
                    {
                        label_TotalPriceVND.Text = booking.ValueVND.ToString("#,###");
                    }
                    else
                    {
                        label_TotalPriceVND.Text = "0";
                    }
                }

                #endregion

                #region -- Editable --

                var aPayment = e.Item.FindControl("aPayment") as HtmlAnchor;
                if (aPayment != null)
                {
                    string url = string.Format("BookingPayment.aspx?NodeId={0}&SectionId={1}&BookingId={2}", Node.Id,
                                               Section.Id, booking.Id);
                    aPayment.Attributes.Add("onclick",
                                            CMS.ServerControls.Popup.OpenPopupScript(url, "Payment", 600, 500));
                }

                var litPaid = e.Item.FindControl("litPaid") as Literal;
                if (litPaid != null)
                {
                    litPaid.Text = (booking.Paid + booking.GuideCollectedUSD).ToString("#,0.#");
                }

                Literal litPaidBase = e.Item.FindControl("litPaidBase") as Literal;
                if (litPaidBase != null)
                {
                    litPaidBase.Text = (booking.PaidBase + booking.GuideCollectedVND).ToString("#,0.#");
                }

                Literal litCurrentRate = e.Item.FindControl("litCurrentRate") as Literal;
                if (litCurrentRate != null)
                {
                    if (booking.CurrencyRate > 0)
                    {
                        litCurrentRate.Text = booking.CurrencyRate.ToString("#,0.#");
                    }
                    else
                    {
                        booking.CurrencyRate = GetCurrentRate();
                        litCurrentRate.Text = booking.CurrencyRate.ToString("#,0.#");
                    }
                }

                Literal litReceivable = e.Item.FindControl("litReceivable") as Literal;
                if (litReceivable != null)
                {
                    litReceivable.Text = booking.TotalReceivable.ToString("#,0.#");
                }

                if (booking.AgencyReceivable == 0)
                {
                    booking.IsPaid = true;
                    // vá lỗi khi đã thanh toán đủ mà vẫn để not paid
                    Module.SaveOrUpdate(booking);
                }

                _total += booking.Value;
                _totalVND += booking.ValueVND;
                _paid += booking.Paid;
                _paidBase += booking.PaidBase;
                _receivable += booking.TotalReceivable;
                _totalGuideCollect += booking.GuideCollect;
                _totalGuideCollectVND += booking.GuideCollectVND;
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
                    if (booking.IsPaid)
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

                if (booking.PaidDate.HasValue)
                {
                    ValueBinder.BindLiteral(e.Item, "litPaidOn", booking.PaidDate.Value);
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
                    Label label_GuideCollect = (Label)e.Item.FindControl("label_GuideCollect");
                    Label label_GuideCollectVND = (Label)e.Item.FindControl("label_GuideCollectVND");
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
                    label_GuideCollect.Text = _totalGuideCollect.ToString("#,###");
                    label_GuideCollectVND.Text = _totalGuideCollectVND.ToString("#,###");
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
                        string url;
                        if (Request.QueryString["agencyid"] != null)
                        {
                            url =
                                string.Format(
                                    "BookingPayment.aspx?NodeId={0}&SectionId={1}&agencyid={4}&from={2}&to={3}",
                                    Node.Id, Section.Id, Request.QueryString["from"],
                                    Request.QueryString["to"], Request.QueryString["agencyid"]);
                        }
                        else if (Request.QueryString["agencyname"] != null)
                        {
                            url =
                                string.Format(
                                    "BookingPayment.aspx?NodeId={0}&SectionId={1}&agencyname={4}&from={2}&to={3}",
                                    Node.Id, Section.Id, Request.QueryString["from"],
                                    Request.QueryString["to"], Request.QueryString["agencyname"]);
                        }
                        else
                        {
                            url = string.Format("BookingPayment.aspx?NodeId={0}&SectionId={1}&from={2}&to={3}", Node.Id,
                                                Section.Id, Request.QueryString["from"],
                                                Request.QueryString["to"]);
                            aPayment.Visible = false;
                        }
                        aPayment.Attributes.Add("onclick",
                                                CMS.ServerControls.Popup.OpenPopupScript(url, "Payment", 300, 400));
                    }
                }
            }
        }

        protected void GetDataSource()
        {
            int count;
            rptBookingList.DataSource = GetData(out count);
        }

        protected IList GetData(out int count)
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
            if (Request.QueryString["mode"] != "all") // Nếu không phải là mode all thì thêm điều kiện về thời gian
            {
                txtFrom.Text = from.ToString("dd/MM/yyyy");
                txtTo.Text = to.ToString("dd/MM/yyyy");
                criterion = Expression.And(criterion,
                                           Expression.And(Expression.Ge(Booking.STARTDATE, from),
                                                          Expression.Le(Booking.STARTDATE, to)));

                //Mặc định luôn lấy not paid
                if (Request.QueryString["paidstatus"] != null)
                {
                    switch (Request.QueryString["paidstatus"].ToLower())
                    {
                        case "paid":
                            criterion = Expression.And(criterion, Expression.And(Expression.Eq("IsPaid", true), Expression.Eq("GuideCollected", true)));
                            break;
                        case "notpaid":
                            criterion = Expression.And(criterion, Expression.Or(Expression.Eq("IsPaid", false), Expression.Eq("GuideCollected", false)));
                            break;
                        default: // Nếu không phải một trong hai trạng thái thì không cần đặt điều kiện lọc (all)

                            break;
                    }
                }
                else
                {
                    criterion = Expression.And(criterion, Expression.Not(Expression.Eq("IsPaid", true)));
                }
            }
            else
            {
                criterion = Expression.And(criterion, Expression.Eq("IsPaid", false));
            }

            // Điều kiện paid-on
            if (Request.QueryString["paidon"] != null)
            {
                criterion = Expression.And(criterion, Expression.Ge("PaidDate", DateTime.FromOADate(Convert.ToDouble(Request.QueryString["paidon"]))));
                criterion = Expression.And(criterion, Expression.Lt("PaidDate", DateTime.FromOADate(Convert.ToDouble(Request.QueryString["paidon"])).AddDays(1)));
            }

            // Điều kiện agency
            ICriterion agencyCrit = SetCriterion(GetAgencies());
            if (agencyCrit != null)
            {
                criterion = Expression.And(criterion, agencyCrit);
            }

            // Kiểm tra quyền
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

            bool tripped = false;
            if (Request.QueryString["tripid"] != null)
            {
                criterion = Expression.And(criterion,
                                           Expression.Eq("Trip.Id", Convert.ToInt32(Request.QueryString["tripid"])));
                tripped = true;
            }

            if (Request.QueryString["orgid"] != null)
            {
                criterion = Expression.And(criterion,
                                           Expression.Eq("trip.Organization", Module.OrganizationGetById(Convert.ToInt32(Request.QueryString["orgid"]))));
                tripped = true;
            }
            return Module.BookingGetByCriterion(criterion, Order.Asc(Booking.STARTDATE), out count, 0, 0, tripped, UserIdentity);
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

        protected void pagerBookings_PageChanged(object sender, PageChangedEventArgs e)
        {
            GetDataSource();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            RepeaterItem item = ((Control)sender).Parent.Parent.Parent as RepeaterItem;
            if (item != null)
            {
                TextBox txtPaid = (TextBox)item.FindControl("txtPaid");
                Literal litReceivable = (Literal)item.FindControl("litReceivable");
                Label lblPaid = (Label)item.FindControl("lblPaid");
                HiddenField hiddenId = (HiddenField)item.FindControl("hiddenId");
                Booking booking = Module.BookingGetById(Convert.ToInt32(hiddenId.Value));
                booking.Paid = Convert.ToDouble(txtPaid.Text);
                if (booking.Paid > 0)
                {
                    lblPaid.Text = booking.Paid.ToString("#,###");
                }
                else
                {
                    lblPaid.Text = "0";
                }
                Module.Update(booking, null);
                if (booking.TotalReceivable > 0)
                {
                    litReceivable.Text = booking.TotalReceivable.ToString("#,###");
                }
                else
                {
                    litReceivable.Text = "0";
                }
            }
        }

        protected void rptOrganization_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is Organization)
            {
                var organization = (Organization)e.Item.DataItem;
                var hplOrganization = e.Item.FindControl("hplOrganization") as HyperLink;           
                if (hplOrganization != null)
                {
                    if (organization.Id.ToString() == Request.QueryString["orgid"])
                    {
                        hplOrganization.CssClass += " selected";
                    }
                }

                if (hplOrganization != null)
                {
                    //DateTime from = DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    hplOrganization.Text = organization.Name;
                    if (Request.QueryString["from"] != null)
                    {
                        hplOrganization.NavigateUrl = string.Format(
                            "PaymentReport.aspx?NodeId={0}&SectionId={1}&from={2}&to={4}&orgid={3}", Node.Id, Section.Id,
                            Request.QueryString["from"], organization.Id, Request.QueryString["to"]);
                    }
                    else
                    {
                        hplOrganization.NavigateUrl = string.Format(
                            "PaymentReport.aspx?NodeId={0}&SectionId={1}&orgid={2}", Node.Id, Section.Id, organization.Id);
                    }
                }

                var rptTrips = e.Item.FindControl("rptTrips") as Repeater;
                if (rptTrips != null)
                {
                    rptTrips.DataSource = Module.TripGetByOrganization(organization);
                    rptTrips.DataBind();
                }
            }
            else
            {
                var hplOrganization = e.Item.FindControl("hplOrganization") as HyperLink;
                if (hplOrganization != null)
                {
                    if (Request.QueryString["orgid"] == null && Request.QueryString["tripid"] == null)
                    {
                        hplOrganization.CssClass += " selected";
                    }
                }

                if (hplOrganization != null)
                {
                    //DateTime date = DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    if (Request.QueryString["from"] != null)
                    {
                        hplOrganization.NavigateUrl = string.Format(
                            "PaymentReport.aspx?NodeId={0}&SectionId={1}&from={2}&to={3}", Node.Id, Section.Id,
                            Request.QueryString["from"], Request.QueryString["to"]);
                    }
                    else
                    {
                        hplOrganization.NavigateUrl = string.Format(
                            "PaymentReport.aspx?NodeId={0}&SectionId={1}", Node.Id, Section.Id);
                    }
                }
            }
        }

        #endregion

        #region -- Export data --

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
            excelFile.LoadXls(Server.MapPath("/Modules/Sails/Admin/ExportTemplates/CongNo.xls"));

            ExcelWorksheet sheet;

            #region -- Lấy các dữ liệu chuẩn bị --

            // Lấy danh sách agency
            IList agencyList = GetAgencies();
            int count;
            //TODO: Sort theo start date
            IList fullList = GetData(out count); // Danh sách toàn bộ booking

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
                time = from.ToString("MMM_yyyy");
            }
            else
            {
                time = string.Format("{0:dd_MM_yyyy}_{1:dd_MM_yyyy}", from, to);
            }

            #endregion

            USDRate currentRate = Module.ExchangeGetByDate(DateTime.Now);
            if (currentRate == null)
            {
                ShowError("Exchanged rate haven't been configurated!");
            }
            foreach (Agency agency in agencyList)
            {
                sheet = excelFile.Worksheets.AddCopy(string.Format("{0} ({1})", agency.Name, agency.Id), excelFile.Worksheets[0]);

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
                    if (!booking.IsPaid)
                    {
                        list.Add(booking);
                    }
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

            sheet.Cells["F1"].Value = time;
            if (agency != null)
            {
                sheet.Cells["C5"].Value = agency.Accountant + " " + agency.Name;
                sheet.Cells["C6"].Value = agency.Address;
                sheet.Cells["C7"].Value = agency.Phone;
                sheet.Cells["G7"].Value = agency.Fax;
            }

            // Tổng ban đầu = 0
            double _totalReceivables = 0;

            // Đối với mỗi booking trong danh sách export, cộng thêm giá trị còn lại
            foreach (Booking booking in list)
            {
                // Nếu chưa có tỉ giá thì phải chia cho tỉ giá mặc định
                if (booking.CurrencyRate > 0)
                {
                    _totalReceivables += booking.TotalReceivable / booking.CurrencyRate; // In USD
                }
                else
                {
                    _totalReceivables += booking.TotalReceivable / currentRate.Rate;
                }
            }

            //TODO: check lại phương pháp tính tổng
            //sheet.Cells["L14"].Value = _totalReceivables;
            //sheet.Cells["N14"].Value = _totalReceivables * currentRate.Rate;

            sheet.Cells["M21"].Value = UserIdentity.FullName;

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
            foreach (Booking booking in list)
            {
                int index = current - firstrow + 1;
                InsertBookingData(currentRate, sheet, current, index, booking, room, _policies);
                current += 1;
            }

            #endregion
        }
        #endregion

        #region -- Xuất doanh thu --
        private void InsertBookingData(USDRate currentRate, ExcelWorksheet sheet, int current, int index,
                                       Booking booking, Room room, IList _policies)
        {
            sheet.Cells[current, 0].Value = index; // Cột index

            sheet.Cells[current, 1].Value = string.Format(BookingFormat, booking.Id);

            sheet.Cells[current, 2].Value = booking.AgencyCode;
            if (booking.Booker != null)
            {
                sheet.Cells[current, 4].Value = booking.Booker.Name; // Cột booker
            }
            sheet.Cells[current, 3].Value = booking.CustomerName.Replace("<br/>", "\n");
            sheet.Cells[current, 5].Value = booking.StartDate; // Check in           
            sheet.Cells[current, 6].Value = booking.StartDate.AddDays(booking.Trip.NumberOfDay - 1);
            if (booking.Trip.NumberOfOptions <= 1)
            {
                sheet.Cells[current, 7].Value = booking.Trip.TripCode;
            }
            else
            {
                sheet.Cells[current, 7].Value = booking.Trip.TripCode + booking.TripOption;
            }
            sheet.Cells[current, 8].Value = booking.Adult; // Số người lớn
            sheet.Cells[current, 9].Value = booking.Child; // Số trẻ em

            // Lấy giá của 1 vị trí twin theo hợp đồng
            // Sẽ lấy giá theo giá của 1 người lớn trong một phòng Twin.
            double unitprice;
            try
            {
                unitprice = booking.Total / booking.Pax;
            }
            catch (PriceException)
            {
                unitprice = 0;
            }
            foreach (ExtraOption service in booking.ExtraServices)
            {
                if (service.Deleted)
                {
                    continue;
                }
                // In cluded trong giá phòng, nhưng lại không sử dụng dịch vụ
                if (service.IsIncluded && !booking.ExtraServices.Contains(service))
                {
                    unitprice = unitprice - service.Price;
                }

                // Không included trong giá phòng, nhưng lại sử dụng
                if (!service.IsIncluded && booking.ExtraServices.Contains(service))
                {
                    unitprice = unitprice + service.Price;
                }
            }

            //unitprice = Module.ApplyPriceFor(unitprice, _policies);

            sheet.Cells[current, 10].Value = unitprice;

            if (booking.CurrencyRate == 0)
            {
                booking.CurrencyRate = currentRate.Rate;
            }
            sheet.Cells[current, 11].Value = Math.Round(booking.TotalReceivable / booking.CurrencyRate); // Số tiền dư
            sheet.Cells[current, 12].Value = booking.CurrencyRate;
            sheet.Cells[current, 13].Value = booking.TotalReceivable;
            sheet.Cells[current, 14].Value = booking.SpecialRequest;
            sheet.Cells[current, 15].Value = booking.Note;
        }

        private void InsertRevenueBookingData(USDRate currentRate, ExcelWorksheet sheet, int current, int index,
                                              Booking booking, Room room, IList _policies)
        {
            sheet.Cells[current, 0].Value = index; // Cột index

            sheet.Cells[current, 1].Value = string.Format(BookingFormat, booking.Id);
            sheet.Cells[current, 2].Value = booking.AgencyCode;

            if (booking.Agency != null)
            {
                sheet.Cells[current, 3].Value = booking.Agency.Name; // Cột Agency
            }
            else
            {
                sheet.Cells[current, 3].Value = SailsModule.NOAGENCY; // Cột Agency
            }

            if (booking.Agency != null && booking.Agency.Sale != null)
            {
                sheet.Cells[current, 4].Value = booking.Agency.Sale.UserName;
            }



            sheet.Cells[current, 5].Value = booking.StartDate; // Check in
            sheet.Cells[current, 6].Value = booking.StartDate.AddDays(booking.Trip.NumberOfDay - 1);
            if (booking.Trip.NumberOfOptions <= 1)
            {
                sheet.Cells[current, 7].Value = booking.Trip.TripCode;
            }
            else
            {
                sheet.Cells[current, 7].Value = booking.Trip.TripCode + booking.TripOption;
            }

            sheet.Cells[current, 8].Value = booking.Adult; // Số người lớn
            sheet.Cells[current, 9].Value = booking.Child; // Số trẻ em

            // Lấy giá của 1 vị trí twin theo hợp đồng
            // Sẽ lấy giá theo giá của 1 người lớn trong một phòng Twin.
            double unitprice = 0.0;
            double unitpriceVND = 0.0;
            try
            {
                if (booking.IsVND)
                    unitpriceVND = booking.TotalVND / booking.Pax;
                else
                {
                    unitprice = booking.Total / booking.Pax;
                }
            }
            catch (PriceException)
            {
                unitprice = 0;
                unitpriceVND = 0;
            }
            foreach (ExtraOption service in booking.ExtraServices)
            {
                if (service.Deleted)
                {
                    continue;
                }
                // In cluded trong giá phòng, nhưng lại không sử dụng dịch vụ
                if (service.IsIncluded && !booking.ExtraServices.Contains(service))
                {
                    if (booking.IsVND)
                        unitpriceVND = unitpriceVND - service.Price * booking.CurrencyRate;
                    else
                    {
                        unitprice = unitprice - service.Price;
                    }
                }

                // Không included trong giá phòng, nhưng lại sử dụng
                if (!service.IsIncluded && booking.ExtraServices.Contains(service))
                {
                    if (booking.IsVND)
                        unitpriceVND = unitprice + service.Price * booking.CurrencyRate;
                    else
                        unitprice = unitprice + service.Price;

                }
            }

            //unitprice = Module.ApplyPriceFor(unitprice, _policies);
            sheet.Cells[current, 10].Style.NumberFormat = "#,#;#,0.#";
            sheet.Cells[current, 10].Value = unitprice;
            sheet.Cells[current, 11].Style.NumberFormat = "#,#;#,0.#";
            sheet.Cells[current, 11].Value = unitpriceVND;

            if (booking.CurrencyRate == 0)
            {
                booking.CurrencyRate = currentRate.Rate;
            }

            sheet.Cells[current, 12].Style.NumberFormat = "#,#;#,0.#";
            sheet.Cells[current, 12].Value = booking.Value; // Tổng tiền
            sheet.Cells[current, 13].Style.NumberFormat = "#,#;#,0.#";
            sheet.Cells[current, 13].Value = booking.ValueVND; // Tổng tiền
            sheet.Cells[current, 14].Style.NumberFormat = "#,#;#,0.#";
            sheet.Cells[current, 14].Value = booking.CurrencyRate;
            sheet.Cells[current, 15].Style.NumberFormat = "#,#;#,0.#";
            sheet.Cells[current, 15].Value = booking.Value * booking.CurrencyRate + booking.ValueVND;

            if (booking.TotalReceivable != 0)
            {
                // Nếu tiền trả của cả hai loại tiền đều = 0
                if (booking.Paid + booking.GuideCollectedUSD == 0 && booking.PaidBase + booking.GuideCollectedVND == 0)
                {
                    sheet.Cells[current, 16].Value = "Unpaid";
                }
                else
                {
                    sheet.Cells[current, 16].Value = "Partly paid";
                }
            }
            else
            {
                if (booking.PaidDate.HasValue)
                {
                    sheet.Cells[current, 16].Value = string.Format("Paid {0}", booking.PaidDate.Value.ToString("dd/MM/yyyy"));
                }
                else
                {
                    sheet.Cells[current, 16].Value = "Paid";
                }
            }
            sheet.Cells[current, 17].Value = booking.SpecialRequest;
            sheet.Cells[current, 18].Value = booking.Note;

        }

        protected void btnExportRevenue_Click(object sender, EventArgs e)
        {
            if (!Module.PermissionCheck(Permission.ACTION_EXPORTREVENUE, UserIdentity))
            {
                ShowError("You do not have permission to use this function!");
                return;
            }

            // Bắt đầu thao tác export            
            ExcelFile excelFile = new ExcelFile();
            excelFile.LoadXls(Server.MapPath("/Modules/Sails/Admin/ExportTemplates/BaoCaoDoanhThu.xls"));

            ExcelWorksheet sheet = excelFile.Worksheets[0];

            #region -- Lấy các dữ liệu chuẩn bị --

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

            int count;
            IList fullList = GetData(out count); // Danh sách toàn bộ booking

            string time;
            if (from.AddMonths(1).AddDays(-1) == to)
            {
                time = from.ToString("MMM_yyyy");
            }
            else
            {
                time = string.Format("{0:dd/MM/yyyy}_{1:dd/MM/yyyy}", from, to);
            }

            #endregion

            // Lọc dữ liệu vào theo tàu để tách sheet
            Dictionary<int, IList> bkByCruise = new Dictionary<int, IList>();
            foreach (Booking booking in fullList)
            {
                if (booking.Cruise == null)
                {
                    continue;
                }
                if (!bkByCruise.ContainsKey(booking.Cruise.Id))
                {
                    bkByCruise.Add(booking.Cruise.Id, new ArrayList());
                }
                bkByCruise[booking.Cruise.Id].Add(booking);
            }

            USDRate currentRate = Module.ExchangeGetByDate(DateTime.Now);
            if (currentRate == null)
            {
                ShowError("Exchanged rate haven't been configurated!");
                return;
            }

            //Room room = Module.RoomGetById(SailsModule.DOUBLE);

            foreach (KeyValuePair<int, IList> pair in bkByCruise)
            {
                Booking booking = (Booking)pair.Value[0];
                ExcelWorksheet current = excelFile.Worksheets.AddCopy(booking.Cruise.Name, sheet);
                ExportCruiseRevenueToSheet(current, pair.Value, time, null, currentRate);
            }

            ExportCruiseRevenueToSheet(sheet, fullList, time, null, currentRate);

            #region -- Trả dữ liệu về cho người dùng --

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AppendHeader("content-disposition", "attachment; filename=" + string.Format("Doanhthu{0}.xls", time));

            MemoryStream m = new MemoryStream();

            excelFile.SaveXls(m);

            Response.OutputStream.Write(m.GetBuffer(), 0, m.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();

            m.Close();
            Response.End();

            #endregion
        }
        #endregion

        #region -- Xuất doanh thu không đối tác & theo sale --
        /// <summary>
        /// Xuất doanh thu tự bán
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExportNoAgency_Click(object sender, EventArgs e)
        {
            if (!Module.PermissionCheck(Permission.ACTION_EXPORTSELFSALES, UserIdentity))
            {
                ShowError("You do not have permission to use this function!");
                return;
            }

            // Bắt đầu thao tác export            
            ExcelFile excelFile = new ExcelFile();
            excelFile.LoadXls(Server.MapPath("/Modules/Sails/Admin/ExportTemplates/CongNo.xls"));

            ExcelWorksheet sheet = excelFile.Worksheets[0];

            #region -- Lấy các dữ liệu chuẩn bị --

            // Lấy danh sách agency
            IList agencyList = GetAgencies();
            int count;
            //TODO: Sort theo start date
            IList fullList = GetData(out count); // Danh sách toàn bộ booking

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
                time = from.ToString("MMM_yyyy");
            }
            else
            {
                time = string.Format("{0:dd/MM/yyyy}_{1:dd/MM/yyyy}", from, to);
            }

            #endregion

            USDRate currentRate = Module.ExchangeGetByDate(DateTime.Now);
            if (currentRate == null)
            {
                ShowError("Exchanged rate haven't been configurated!");
            }

            IList list = new ArrayList();

            // Chỉ lấy các booking của chính OS
            foreach (Booking booking in fullList)
            {
                if (booking.Agency != null)
                {
                    continue;
                }
                list.Add(booking);
            }

            ExportAgencyData(currentRate, null, list, sheet, time);

            #region -- Trả dữ liệu về cho người dùng --

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

        protected void ExportCruiseRevenueToSheet(ExcelWorksheet sheet, IList bookings, string time, Room room, USDRate rate)
        {
            #region -- Thông tin chung --

            sheet.Cells["G1"].Value = time;

            // Tổng ban đầu = 0
            double _totalUSD = 0;
            double _totalVND = 0;
            double _totalBookingCalculated = 0.0;

            // Đối với mỗi booking trong danh sách export, cộng thêm giá trị còn lại
            foreach (Booking booking in bookings)
            {
                _totalUSD += booking.Value;
                if (booking.CurrencyRate > 0)
                {
                    _totalVND += booking.ValueVND;
                }
                else
                {
                    _totalVND += booking.ValueVND;
                }
                _totalBookingCalculated += booking.Value * booking.CurrencyRate + booking.ValueVND;
            }

            //TODO: check lại phương pháp tính tổng

            sheet.Cells["K9"].Style.NumberFormat = "#,# \"USD\";#,0.# \"USD\"";
            sheet.Cells["K9"].Value = _totalUSD;
            sheet.Cells["L9"].Style.NumberFormat = "#,# \"VND\";#,0.# \"VND\"";
            sheet.Cells["L9"].Value = _totalVND;
            sheet.Cells["N9"].Style.NumberFormat = "#,# \"VND\";#,0.# \"VND\"";
            sheet.Cells["N9"].Value = _totalBookingCalculated;

            sheet.Cells["M16"].Value = UserIdentity.FullName;

            #endregion

            // Sao chép dòng đầu theo số lượng booking
            // Dòng đầu tiên là dòng 11
            const int firstrow = 7;

            sheet.Rows[firstrow].InsertCopy(bookings.Count - 1, sheet.Rows[firstrow]);

            #region -- Thông tin từng booking --

            // Ghi vào file excel theo từng dòng
            int current = firstrow;
            foreach (Booking booking in bookings)
            {
                // Chính sách giá cho đại lý hiện tại
                IList _policies;

                // Nếu là export công nợ đại lý và đại lý có role
                if (booking.Agency != null && booking.Agency.Role != null)
                {
                    _policies = Module.AgencyPolicyGetByRole(booking.Agency.Role);
                }
                // Nếu không phải công nợ đại lý hoặc không có role thì lấy giá anonymous
                else
                {
                    _policies = Module.AgencyPolicyGetByRole(Module.RoleGetById(4));
                }

                int index = current - firstrow + 1;
                InsertRevenueBookingData(rate, sheet, current, index, booking, room, _policies);
                current += 1;
            }

            #endregion
        }

        protected void btnExportRevenueBySale_Click(object sender, EventArgs e)
        {
            if (!Module.PermissionCheck(Permission.ACTION_EXPORTREVENUEBYSALE, UserIdentity))
            {
                ShowError("You do not have permission to use this function!");
                return;
            }
            // Bắt đầu thao tác export            
            ExcelFile excelFile = new ExcelFile();
            excelFile.LoadXls(Server.MapPath("/Modules/Sails/Admin/ExportTemplates/BaoCaoDoanhThuTheoSales.xls"));

            #region -- Lấy các dữ liệu chuẩn bị --

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

            int count;
            IList fullList = GetData(out count); // Danh sách toàn bộ booking

            string time;
            if (from.AddMonths(1).AddDays(-1) == to)
            {
                time = from.ToString("MMM_yyyy");
            }
            else
            {
                time = string.Format("{0:dd/MM/yyyy}_{1:dd/MM/yyyy}", from, to);
            }

            #endregion

            #region -- Tạo danh sách nhân viên sale --

            IList<User> salesList = new List<User>();
            Dictionary<User, IList> bookingSales = new Dictionary<User, IList>();
            foreach (Booking booking in fullList)
            {
                if (booking.Agency != null && booking.Agency.Sale != null)
                {
                    // Nếu trong danh sách sale chưa có khởi tạo trong map
                    if (!salesList.Contains(booking.Agency.Sale))
                    {
                        salesList.Add(booking.Agency.Sale);
                        bookingSales.Add(booking.Agency.Sale, new ArrayList());
                        bookingSales[booking.Agency.Sale].Add(booking);
                    }
                    // Nếu có rồi thì không add vào danh sách chỉ add vào map
                    else
                    {
                        bookingSales[booking.Agency.Sale].Add(booking);
                    }
                }
            }

            //Sau khi có danh sách sale, add thêm worksheet
            foreach (User user in salesList)
            {
                ExcelWorksheet currentSheet = excelFile.Worksheets.AddCopy(user.FullName, excelFile.Worksheets[0]);
                ExportBySale(user, currentSheet, bookingSales[user], time);
            }

            #endregion

            #region -- Trả dữ liệu về cho người dùng --
            // Xóa sheet mẫu
            if (excelFile.Worksheets.Count > 1)
            {
                excelFile.Worksheets[0].Delete();
            }

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AppendHeader("content-disposition", "attachment; filename=" + string.Format("Doanhthu{0}.xls", time));

            MemoryStream m = new MemoryStream();

            excelFile.SaveXls(m);

            Response.OutputStream.Write(m.GetBuffer(), 0, m.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();

            m.Close();
            Response.End();

            #endregion
        }

        protected void ExportBySale(User sale, ExcelWorksheet sheet, IList list, string time)
        {
            #region -- Thông tin chung --

            //Agency agency = Module.AgencyGetById(Convert.ToInt32(ddlAgencies.SelectedValue));

            sheet.Cells["G1"].Value = time;
            sheet.Cells["C4"].Value = sale.FullName;

            // Tổng ban đầu = 0
            double _totalUSD = 0;
            int _adult = 0;
            int _child = 0;

            // Đối với mỗi booking trong danh sách export, cộng thêm giá trị còn lại
            foreach (Booking booking in list)
            {
                _totalUSD += booking.Value;
                _adult += booking.Adult;
                _child += booking.Child;
            }
            sheet.Cells["G9"].Value = _adult;
            sheet.Cells["H9"].Value = _child;
            sheet.Cells["J9"].Value = _totalUSD;

            sheet.Cells["G16"].Value = UserIdentity.FullName;

            #endregion

            #region -- Các giá trị dùng chung --

            // Lấy giá theo phòng
            Room room = Module.RoomGetById(SailsModule.DOUBLE);

            #endregion

            // Sao chép dòng đầu theo số lượng booking
            // Dòng đầu tiên là dòng 11
            const int firstrow = 7;

            sheet.Rows[firstrow].InsertCopy(list.Count - 1, sheet.Rows[firstrow]);

            #region -- Thông tin từng booking --

            // Ghi vào file excel theo từng dòng
            int current = firstrow;
            foreach (Booking booking in list)
            {
                // Chính sách giá cho đại lý hiện tại
                IList _policies;

                // Nếu là export công nợ đại lý và đại lý có role
                if (booking.Agency != null && booking.Agency.Role != null)
                {
                    _policies = Module.AgencyPolicyGetByRole(booking.Agency.Role);
                }
                // Nếu không phải công nợ đại lý hoặc không có role thì lấy giá anonymous
                else
                {
                    _policies = Module.AgencyPolicyGetByRole(Module.RoleGetById(4));
                }

                int index = current - firstrow + 1;
                InsertSaleBookingData(sheet, current, index, booking, room, _policies);
                current += 1;
            }

            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sheet">Worksheet hiện tại</param>
        /// <param name="current"></param>
        /// <param name="index"></param>
        /// <param name="booking"></param>
        /// <param name="room"></param>
        /// <param name="policies"></param>
        protected void InsertSaleBookingData(ExcelWorksheet sheet, int current, int index, Booking booking, Room room, IList policies)
        {
            sheet.Cells[current, 0].Value = index; // Cột index
            if (!string.IsNullOrEmpty(booking.AgencyCode))
            {
                sheet.Cells[current, 1].Value = booking.AgencyCode;
            }
            else
            {
                sheet.Cells[current, 1].Value = string.Format("OS{0:00000}", booking.Id);
            }
            sheet.Cells[current, 2].Value = booking.CustomerName.Replace("<br/>", "\n");

            if (booking.Agency != null)
            {
                sheet.Cells[current, 3].Value = booking.Agency.Name; // Cột Agency
            }
            else
            {
                sheet.Cells[current, 3].Value = SailsModule.NOAGENCY; // Cột Agency
            }

            if (booking.Trip.NumberOfOptions <= 1)
            {
                sheet.Cells[current, 4].Value = booking.Trip.TripCode;
            }
            else
            {
                sheet.Cells[current, 4].Value = booking.Trip.TripCode + booking.TripOption;
            }

            sheet.Cells[current, 5].Value = booking.StartDate; // Check in
            sheet.Cells[current, 6].Value = booking.StartDate.AddDays(booking.Trip.NumberOfDay - 1);

            sheet.Cells[current, 7].Value = booking.Adult; // Số người lớn
            sheet.Cells[current, 8].Value = booking.Child; // Số trẻ em

            // Lấy giá của 1 vị trí twin theo hợp đồng
            // Sẽ lấy giá theo giá của 1 người lớn trong một phòng Twin.
            double unitprice;
            try
            {
                unitprice = BookingRoom.Calculate(room.RoomClass, room.RoomType, 1, 0, false,
                                                  booking.Trip, booking.Cruise, booking.TripOption,
                                                  booking.StartDate, Module, policies, ChildPrice,
                                                  AgencySupplement, booking.Agency);
            }
            catch
            {
                unitprice = 0;
            }

            foreach (ExtraOption service in booking.ExtraServices)
            {
                if (service.Deleted)
                {
                    continue;
                }
                // In cluded trong giá phòng, nhưng lại không sử dụng dịch vụ
                if (service.IsIncluded && !booking.ExtraServices.Contains(service))
                {
                    unitprice = unitprice - service.Price;
                }

                // Không included trong giá phòng, nhưng lại sử dụng
                if (!service.IsIncluded && booking.ExtraServices.Contains(service))
                {
                    unitprice = unitprice + service.Price;
                }
            }

            //unitprice = Module.ApplyPriceFor(unitprice, _policies);

            sheet.Cells[current, 9].Value = unitprice;

            sheet.Cells[current, 10].Value = booking.Value; // Tổng tiền
        }
        #endregion

        #endregion
    }
}