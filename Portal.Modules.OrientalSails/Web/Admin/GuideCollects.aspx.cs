using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CMS.Core.Domain;
using CMS.ServerControls;
using CMS.Web.Util;
using NHibernate.Criterion;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.UI;
using log4net;
using GemBox.Spreadsheet;
using System.IO;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class GuideCollects : SailsAdminBasePage
    {
        #region -- PRIVATE MEMBERS --

        private readonly ILog _logger = LogManager.GetLogger(typeof(GuideCollects));
        private int _adult;
        private int _adultTransfer;
        private IList _agencies;
        private int _baby;
        private int _child;
        private int _childTransfer;
        private double _currentRate;
        private int _double;
        private double _paid;
        private double _paidBase;
        private double _receivable;
        private double _total;
        private double _totalVND;
        private int _twin;

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

                //if (!string.IsNullOrEmpty(txtAgency.Text))
                //{
                //    _agencies = Module.AgencyGetByName(txtAgency.Text);
                //}
                //else
                //{
                _agencies = new ArrayList();
                //}
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
                //Title = Resources.titleBookingList;
                Title = @"Guide collects";
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

                    #region -- Bind sales --

                    ddlTrips.DataSource = Module.TripGetAll(true, UserIdentity);
                    ddlTrips.DataTextField = "Name";
                    ddlTrips.DataValueField = "Id";
                    ddlTrips.DataBind();
                    ddlTrips.Items.Insert(0, "-- All --");

                    if (Request.QueryString["tripid"] != null)
                    {
                        ddlTrips.SelectedValue = Request.QueryString["tripid"];
                    }
                    //Role role = Module.RoleGetById(Convert.ToInt32(Module.ModuleSettings("Sale")));
                    //ddlSales.DataSource = role.Users;
                    //ddlSales.DataTextField = "Username";
                    //ddlSales.DataValueField = "Id";
                    //ddlSales.DataBind();
                    //ddlSales.Items.Insert(0, new ListItem("-- unbound sale --", "0"));
                    //ddlSales.Items.Insert(0, "-- All --");
                    #endregion

                    rptOrganization.DataSource = Module.OrganizationGetAllRoot();
                    rptOrganization.DataBind();

                    LoadInfo();
                    GetDataSource();
                    rptBookingList.DataBind();

                    if (Request.QueryString["ps"] == null)
                    {
                        hplAllPaid.NavigateUrl = Request.RawUrl + "&ps=all";
                        hplNotPaid.NavigateUrl = Request.RawUrl + "&ps=notpaid";
                        hplPaid.NavigateUrl = Request.RawUrl + "&ps=paid";
                    }
                    else
                    {
                        var nvc = HttpUtility.ParseQueryString(Request.Url.Query);
                        nvc.Remove("ps");
                        string url = Request.Url.AbsolutePath + "?" + nvc.ToString();
                        hplAllPaid.NavigateUrl = url + "&ps=all";
                        hplNotPaid.NavigateUrl = url + "&ps=notpaid";
                        hplPaid.NavigateUrl = url + "&ps=paid";
                    }
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

            //if (Request.QueryString["agencyname"] != null)
            //{
            //    txtAgency.Text = Request.QueryString["agencyname"];
            //}
            if (Request.QueryString["guidename"] != null)
            {
                txtGuide.Text = Request.QueryString["guidename"];
            }

            if (Request.QueryString["bookingcode"] != null)
            {
                txtBookingCode.Text = Request.QueryString["bookingcode"];
            }

            if (Request.QueryString["saleid"] != null)
            {
                ddlSales.SelectedValue = Request.QueryString["saleid"];
            }
            if (Request.QueryString["ps"] != null)
            {
                switch (Request.QueryString["ps"].ToLower())
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

            if (!string.IsNullOrEmpty(txtGuide.Text))
            {
                query += "&guidename=" + txtGuide.Text;
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

            if (String.IsNullOrEmpty(Request.QueryString["ps"]))
                query += "&ps=notpaid";
            else
                query += "&ps=" + Request.QueryString["ps"];

            if (!String.IsNullOrEmpty(Request.QueryString["supplierid"]))
            {
                query += "&supplierid=" + Request.QueryString["supplierid"];
            }

            if (!String.IsNullOrEmpty(Request.QueryString["orgid"]))
                query += "&ps=" + Request.QueryString["orgid"];

            PageRedirect(string.Format("GuideCollects.aspx?NodeId={0}&SectionId={1}{2}", Node.Id, Section.Id, query));
        }

        protected void rptBookingList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is Booking)
            {
                var booking = (Booking)e.Item.DataItem;

                Literal litTACode = e.Item.FindControl("litTACode") as Literal;
                if (litTACode != null)
                {
                    var sailDate = Module.ExpenseGetByDate(booking.Trip, booking.StartDate);
                    ExpenseService guide = null;
                    foreach (ExpenseService service in sailDate.Services)
                    {
                        if (service.Type.Name.ToUpper() == "GUIDE" && service.Group == booking.Group)
                        {
                            guide = service;
                        }
                    }

                    if (guide != null && guide.Supplier != null)
                    {
                        litTACode.Text = guide.Supplier.Name;
                    }
                }


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
                        hplCruise.NavigateUrl = string.Format("GuideCollects.aspx?NodeId={0}&SectionId={1}&cruiseid={2}", Node.Id,
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

                var hyperLink_Partner = e.Item.FindControl("hyperLink_Partner") as HyperLink;
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
                                    "GuideCollects.aspx?NodeId={0}&SectionId={1}&agencyid={2}&from={3}&to={4}", Node.Id,
                                    Section.Id, booking.Agency.Id, from.ToOADate(), to.ToOADate());
                        }
                        catch
                        {
                            hyperLink_Partner.NavigateUrl =
                                string.Format("GuideCollects.aspx?NodeId={0}&SectionId={1}&AgencyId={2}", Node.Id,
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

                ValueBinder.BindLiteral(e.Item, "litGuideCollect", booking.GuideCollect);
                ValueBinder.BindLiteral(e.Item, "litGuideCollectVND", booking.GuideCollectVND.ToString("#,0.#"));

                #region -- Paid/Total --

                Label label_TotalPrice = e.Item.FindControl("label_TotalPrice") as Label;
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

                #endregion

                #region -- Editable --

                HtmlAnchor aPayment = e.Item.FindControl("aPayment") as HtmlAnchor;
                if (aPayment != null)
                {
                    string url = string.Format("BookingPayment.aspx?NodeId={0}&SectionId={1}&BookingId={2}&mode=guide", Node.Id,
                                               Section.Id, booking.Id);
                    aPayment.Attributes.Add("onclick",
                                            CMS.ServerControls.Popup.OpenPopupScript(url, "Payment", 300, 400));
                }

                Literal litPaid = e.Item.FindControl("litPaid") as Literal;
                if (litPaid != null)
                {
                    litPaid.Text = booking.GuideCollectedUSD.ToString("#,0.#");
                }

                Literal litPaidBase = e.Item.FindControl("litPaidBase") as Literal;
                if (litPaidBase != null)
                {
                    litPaidBase.Text = booking.GuideCollectedVND.ToString("#,0.#");
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
                    litReceivable.Text = booking.GuideCollectReceivable.ToString("#,0.#");
                }
                _total += booking.GuideCollect;
                _totalVND += booking.GuideCollectVND;
                _paid += booking.GuideCollectedUSD;
                _paidBase += booking.GuideCollectedVND;
                _receivable += booking.GuideCollectReceivable;

                #endregion

                #region -- Tô màu --

                HtmlTableRow trItem = e.Item.FindControl("trItem") as HtmlTableRow;
                if (trItem != null)
                {
                    string color = string.Empty;
                    if (booking.GuideCollected)
                    {
                        color = SailsModule.GOOD;
                    }
                    else
                    {
                        color = SailsModule.WARNING;
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

            if (String.IsNullOrEmpty(Request.QueryString["ps"]))
                criterion = Expression.And(criterion, Expression.Eq("GuideCollected", false));

            criterion = Expression.And(criterion, Expression.Or(Expression.Gt("GuideCollect", 0d), Expression.Gt("GuideCollectVND", 0d)));
            DateTime from = DateTime.Today.AddDays(-DateTime.Today.Day + 1);
            DateTime to = from.AddMonths(1).AddDays(-1);
            if (string.IsNullOrEmpty(txtFrom.Text) || string.IsNullOrEmpty(txtTo.Text))
            {

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
            }
            else
            {
                criterion = Expression.And(criterion, Expression.Eq("IsPaid", false));
            }

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

            if (UserIdentity.HasPermission(AccessLevel.Administrator) || Module.PermissionCheck("VIEW_ALLBOOKINGGUIDECOLLECT", UserIdentity))
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

            if (Request.QueryString["ps"] != null)
            {
                switch (Request.QueryString["ps"])
                {
                    case "notpaid":
                        criterion = Expression.And(criterion, Expression.Eq("GuideCollected", false));
                        break;
                    case "paid":
                        criterion = Expression.And(criterion, Expression.Eq("GuideCollected", true));
                        break;
                }
            }

            var bookings = Module.BookingGetByCriterion(criterion, Order.Asc(Booking.STARTDATE), out count, 0, 0, tripped, UserIdentity);
            var bookingFilterList = new List<Booking>();
            if (Request.QueryString["guidename"] != null)
            {
                foreach (Booking booking in bookings)
                {
                    var sailDate = Module.ExpenseGetByDate(booking.Trip, booking.StartDate);
                    ExpenseService guide = null;
                    foreach (ExpenseService service in sailDate.Services)
                    {
                        if (service.Type.Name.ToUpper() == "GUIDE" && service.Group == booking.Group)
                        {
                            guide = service;
                            if (guide != null && guide.Supplier != null)
                            {
                                if (guide.Supplier.Name.Contains(Request.QueryString["guidename"]))
                                {
                                    bookingFilterList.Add(booking);
                                }
                            }
                        }
                    }
                }
                bookings = bookingFilterList;
            }

            var bookingGuideFilterList = new List<Booking>();
            if (Request.QueryString["supplierid"] != null)
            {
                var agencyId = Convert.ToInt32(Request.QueryString["supplierid"]);
                foreach (Booking booking in bookings)
                {
                    var sailDate = Module.ExpenseGetByDate(booking.Trip, booking.StartDate);
                    ExpenseService guide = null;
                    foreach (ExpenseService service in sailDate.Services)
                    {
                        if (service.Type.Name.ToUpper() == "GUIDE" && service.Group == booking.Group)
                        {
                            guide = service;
                            if (guide != null && guide.Supplier != null)
                            {
                                if (guide.Supplier.Id == agencyId)
                                {
                                    bookingGuideFilterList.Add(booking);
                                }
                            }
                        }
                    }
                }
                bookings = bookingGuideFilterList;
            }

            return bookings;
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

        protected void btnExportGuideCollect_Click(object sender, EventArgs e)
        {
            ExcelFile excelFile = new ExcelFile();
            excelFile.LoadXls(Server.MapPath("/Modules/Sails/Admin/ExportTemplates/GuideCollectCommission.xls"));

            ExcelWorksheet sheet;

            int count = 0;
            IList guideList = GetData(out count);

            IList<ExpenseService> expenseServicesList = new List<ExpenseService>();
            IList<ExpenseService> distinctExpenseServicesList = new List<ExpenseService>();

            //Để tạo sheet đầu tiên tổng các guid thu
            sheet = excelFile.Worksheets.AddCopy(string.Format("Cong no GuideCollect "), excelFile.Worksheets[0]);

            ExportGuidCollect(guideList, expenseServicesList, sheet);

            if (expenseServicesList.Count != 0)
            {
                //Để tạo một list các guide không trùng 
                foreach (ExpenseService es in expenseServicesList)
                {
                    bool duplicatefound = false;
                    foreach (ExpenseService es2 in distinctExpenseServicesList)
                    {
                        if (es.Supplier.Name.Equals(es2.Supplier.Name))
                            duplicatefound = true;

                    }
                    if (!duplicatefound)
                        distinctExpenseServicesList.Add(es);

                }

                //Để tạo các sheet con theo từng guide
                foreach (ExpenseService es in distinctExpenseServicesList)
                {
                    sheet = excelFile.Worksheets.AddCopy(string.Format("{0}", es.Supplier.Name), excelFile.Worksheets[0]);
                    IList<Booking> listBookingOfGuid = new List<Booking>();
                    foreach (Booking booking in guideList)
                    {
                        var sailDate = Module.ExpenseGetByDate(booking.Trip, booking.StartDate);
                        foreach (ExpenseService service in sailDate.Services)
                        {
                            if (service.Type.Name.ToUpper() == "GUIDE" && service.Group == booking.Group)
                            {
                                if (service.Supplier.Name.Equals(es.Supplier.Name))
                                {
                                    listBookingOfGuid.Add(booking);
                                }
                            }
                        }
                    }
                    ExportSingleGuidCollect(listBookingOfGuid, sheet);
                }
            }

            if (excelFile.Worksheets.Count > 1)
            {
                excelFile.Worksheets[0].Delete();
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

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AppendHeader("content-disposition", "attachment; filename=" + string.Format("CongnoGuidecollect{0}.xls", time));

            MemoryStream m = new MemoryStream();

            excelFile.SaveXls(m);

            Response.OutputStream.Write(m.GetBuffer(), 0, m.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();

            m.Close();
            Response.End();
        }

        private void ExportGuidCollect(IList guideList, IList<ExpenseService> expenseServicesList, ExcelWorksheet sheet)
        {
            sheet.Cells["C5"].Value = txtFrom.Text;
            sheet.Cells["F5"].Value = txtTo.Text;

            int no = 0;
            int startRow = 9;
            double totalGuideCollect = 0.0;
            double totalGuideCollectVND = 0.0;
            double totalGuideCollectReceivable = 0.0;

            sheet.Rows[startRow - 1].InsertCopy(guideList.Count - 1, sheet.Rows[startRow - 1]);
            foreach (Booking booking in guideList)
            {
                sheet.Cells["A" + startRow].Value = no;
                sheet.Cells["B" + startRow].Value = string.Format(BookingFormat, booking.Id);
                sheet.Cells["C" + startRow].Value = booking.Agency != null ? booking.Agency.Sale != null ? booking.Agency.Sale.UserName : "" : "";

                var sailDate = Module.ExpenseGetByDate(booking.Trip, booking.StartDate);
                ExpenseService guide = null;
                foreach (ExpenseService service in sailDate.Services)
                {
                    if (service.Type.Name.ToUpper() == "GUIDE" && service.Group == booking.Group)
                    {
                        guide = service;
                    }
                }


                if (guide != null && guide.Supplier != null)
                {
                    sheet.Cells["D" + startRow].Value = guide.Supplier != null ? guide.Supplier.Name : "";
                    expenseServicesList.Add(guide);
                }

                sheet.Cells["E" + startRow].Value = booking.StartDate.ToString("dd/MM/yyyy");
                sheet.Cells["F" + startRow].Value = booking.Trip != null ? booking.Trip.TripCode : "";
                sheet.Cells["G" + startRow].Value = booking.Adult.ToString();
                sheet.Cells["H" + startRow].Value = booking.Child.ToString();
                sheet.Cells["I" + startRow].Value = booking.Baby.ToString();
                sheet.Cells["J" + startRow].Value = booking.GuideCollect.ToString("#,0.#");
                sheet.Cells["K" + startRow].Value = booking.GuideCollectVND.ToString("#,0.#");

                if (booking.CurrencyRate > 0)
                {
                    sheet.Cells["L" + startRow].Value = booking.CurrencyRate.ToString("#,0.#");
                }
                else
                {
                    booking.CurrencyRate = GetCurrentRate();
                    sheet.Cells["L" + startRow].Value = booking.CurrencyRate.ToString("#,0.#");
                }

                sheet.Cells["M" + startRow].Value = (booking.GuideCollect * booking.CurrencyRate + booking.GuideCollectVND).ToString("#,0.#");
                sheet.Cells["N" + startRow].Value = booking.ComPaid ? "Paid" : "UnPaid";

                no += 1;
                startRow += 1;
                totalGuideCollect += booking.GuideCollect;
                totalGuideCollectVND += booking.GuideCollectVND;
                totalGuideCollectReceivable += booking.GuideCollect * booking.CurrencyRate + booking.GuideCollectVND;
            }
            sheet.Cells["J" + (startRow)].Value = "$" + " " + totalGuideCollect.ToString("#,0.#");
            sheet.Cells["K" + (startRow)].Value = "VND" + " " + totalGuideCollectVND.ToString("#,0.#");
            sheet.Cells["M" + (startRow)].Value = "VND" + " " + totalGuideCollectReceivable.ToString("#,0.#");

        }

        private void ExportSingleGuidCollect(IList<Booking> listBookingOfGuid, ExcelWorksheet sheet)
        {
            sheet.Columns["D"].Delete();
            sheet.Cells["C5"].Value = txtFrom.Text;
            sheet.Cells["F5"].Value = txtTo.Text;

            int no = 0;
            int startRow = 9;
            double totalGuideCollect = 0.0;
            double totalGuideCollectVND = 0.0;
            double totalGuideCollectReceivable = 0.0;

            sheet.Rows[startRow - 1].InsertCopy(listBookingOfGuid.Count - 1, sheet.Rows[startRow - 1]);
            foreach (Booking booking in listBookingOfGuid)
            {
                sheet.Cells["A" + startRow].Value = no;
                sheet.Cells["B" + startRow].Value = string.Format(BookingFormat, booking.Id);
                sheet.Cells["C" + startRow].Value = booking.Agency != null ? booking.Agency.Sale != null ? booking.Agency.Sale.UserName : "" : "";
                sheet.Cells["D" + startRow].Value = booking.StartDate.ToString("dd/MM/yyyy");
                sheet.Cells["E" + startRow].Value = booking.Trip != null ? booking.Trip.TripCode : "";
                sheet.Cells["F" + startRow].Value = booking.Adult.ToString();
                sheet.Cells["G" + startRow].Value = booking.Child.ToString();
                sheet.Cells["H" + startRow].Value = booking.Baby.ToString();
                sheet.Cells["I" + startRow].Value = booking.GuideCollect.ToString("#,0.#");
                sheet.Cells["J" + startRow].Value = booking.GuideCollectVND.ToString("#,0.#");
                if (booking.CurrencyRate > 0)
                {
                    sheet.Cells["K" + startRow].Value = booking.CurrencyRate.ToString("#,0.#");
                }
                else
                {
                    booking.CurrencyRate = GetCurrentRate();
                    sheet.Cells["K" + startRow].Value = booking.CurrencyRate.ToString("#,0.#");
                }

                sheet.Cells["L" + startRow].Value = (booking.GuideCollect * booking.CurrencyRate + booking.GuideCollectVND).ToString("#,0.#");
                sheet.Cells["M" + startRow].Value = booking.GuideCollected ? "Paid" : "UnPaid";

                no += 1;
                startRow += 1;
                totalGuideCollect += booking.GuideCollect;
                totalGuideCollectVND += booking.GuideCollectVND;
                totalGuideCollectReceivable += booking.GuideCollect * booking.CurrencyRate + booking.GuideCollectVND;
            }
            sheet.Cells["I" + (startRow)].Value = "$" + " " + totalGuideCollect.ToString("#,0.#");
            sheet.Cells["J" + (startRow)].Value = "VND" + " " + totalGuideCollectVND.ToString("#,0.#");
            sheet.Cells["L" + (startRow)].Value = "VND" + " " + totalGuideCollectReceivable.ToString("#,0.#");
        }

        #endregion
    }
}