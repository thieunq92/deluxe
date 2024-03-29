using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CMS.Web.Util;
using GemBox.Spreadsheet;
using NHibernate.Criterion;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.UI;
using Portal.Modules.OrientalSails.Web.Util;
using System.Web;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class PayableList : SailsAdminBase
    {
        #region -- PRIVATE MEMBERS --
        private double _total;
        private double _paid;
        private double _payable;
        #endregion

        #region -- PAGE EVENTS --
        protected void
            Page_Load(object sender, EventArgs e)
        {
            Title = @"Payable";
            pagerServices.AllowCustomPaging = true;
            pagerServicesHsf.AllowCustomPaging = true;
            if (!IsPostBack)
            {
                BindSuppliers(0);
                BindCostTypes();

                BindData();

                GetDataSource();

                rptOrganization.DataSource = Module.OrganizationGetAllRoot();
                rptOrganization.DataBind();

                rptServices.DataSource = Module.CostTypeGetAll();
                rptServices.DataBind();

                if (Request.QueryString["ps"] == null)
                {
                    hplAllPaid.NavigateUrl = Request.RawUrl + "&ps=all";
                    hplNotPaid.NavigateUrl = Request.RawUrl + "&ps=notpaid";
                    hplPaid.NavigateUrl = Request.RawUrl + "&ps=paid";
                    hplNotPaid.CssClass += " selected";
                }
                else
                {
                    var nvc = HttpUtility.ParseQueryString(Request.Url.Query);
                    nvc.Remove("ps");
                    string url = Request.Url.AbsolutePath + "?" + nvc.ToString();
                    hplAllPaid.NavigateUrl = url + "&ps=all";
                    hplNotPaid.NavigateUrl = url + "&ps=notpaid";
                    hplPaid.NavigateUrl = url + "&ps=paid";
                    if (Request.QueryString["ps"] == "all")
                    {
                        hplAllPaid.CssClass += " selected";
                    }
                    if (Request.QueryString["ps"] == "notpaid")
                    {
                        hplNotPaid.CssClass += " selected";
                    }
                    if (Request.QueryString["ps"] == "paid")
                    {
                        hplPaid.CssClass += " selected";
                    }
                }
            }
        }

        protected void BindSuppliers(int costtypeid)
        {
            ddlSupplier.DataSource = Module.SupplierGetAll(costtypeid);
            ddlSupplier.DataTextField = "Name";
            ddlSupplier.DataValueField = "Id";
            ddlSupplier.DataBind();
            ddlSupplier.Items.Insert(0, "-- Supplier --");
            foreach (Agency agency in Module.GuidesGetAll())
            {
                ddlSupplier.Items.Add(new ListItem(agency.Name, agency.Id.ToString()));
            }
        }

        protected void BindCostTypes()
        {
            ddlCostTypes.DataSource = Module.CostTypeGetAll();
            ddlCostTypes.DataTextField = "Name";
            ddlCostTypes.DataValueField = "Id";
            ddlCostTypes.DataBind();
            ddlCostTypes.Items.Insert(0, "-- Service --");
        }

        protected void BindData()
        {
            txtFrom.Text = DateTime.Today.AddDays(-DateTime.Today.Day + 1).ToString("dd/MM/yyyy");
            txtTo.Text = DateTime.Today.AddDays(-DateTime.Today.Day + 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy");

            if (!string.IsNullOrEmpty(Request.QueryString["from"]))
                txtFrom.Text = DateTime.FromOADate(Convert.ToDouble(Request.QueryString["from"])).ToString("dd/MM/yyyy");

            if (!string.IsNullOrEmpty(Request.QueryString["to"]))
                txtTo.Text = DateTime.FromOADate(Convert.ToDouble(Request.QueryString["to"])).ToString("dd/MM/yyyy");

            if (Request.QueryString["supplierid"] != null)
            {
                ddlSupplier.SelectedValue = Request.QueryString["supplierid"];
            }

            if (Request.QueryString["costtype"] != null)
            {
                ddlCostTypes.SelectedValue = Request.QueryString["costtype"];
            }

            if (Request.QueryString["tripcode"] != null)
            {
                txtTripCode.Text = Request.QueryString["tripcode"];
            }
        }
        #endregion

        protected void btnDisplay_Click(object sender, EventArgs e)
        {
            string query = string.Empty;
            query += string.Format("&from={0}&to={1}",
                                   DateTime.ParseExact(txtFrom.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).
                                       ToOADate(),
                                   DateTime.ParseExact(txtTo.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToOADate());
            if (ddlSupplier.SelectedIndex > 0)
            {
                query += "&supplierid=" + ddlSupplier.SelectedValue;
            }

            if (ddlCostTypes.SelectedIndex > 0)
            {
                query += "&costtype=" + ddlCostTypes.SelectedValue;
            }

            if (!String.IsNullOrEmpty(txtTripCode.Text))
            {
                query += "&tripcode=" + txtTripCode.Text;
            }
            PageRedirect(string.Format("PayableList.aspx?NodeId={0}&SectionId={1}{2}", Node.Id, Section.Id, query));
        }

        protected void GetDataSource()
        {
            var tripCodeToSearch = "";
            if (Request.QueryString["tripcode"] != null)
            {
                tripCodeToSearch = Request.QueryString["tripcode"];
            }
            var expenseServices = GetData();
            var expenseServicesHaveTripCodeToSearch = new List<ExpenseService>();
            foreach (ExpenseService expenseService in expenseServices)
            {
                var expenseServiceTripCode = string.Format("{0}{1}-{2:00}", expenseService.Expense.Trip.TripCode, expenseService.Expense.Date.ToString("ddMMyy"), expenseService.Group);
                var isExpenseServiceTripCodeContainTripCodeToSearch = expenseServiceTripCode.IndexOf(tripCodeToSearch, StringComparison.InvariantCultureIgnoreCase) >= 0;
                if (isExpenseServiceTripCodeContainTripCodeToSearch)
                {
                    expenseServicesHaveTripCodeToSearch.Add(expenseService);
                }
            }

            if (tripCodeToSearch != "")
            {
                rptExpenseServices.DataSource = expenseServicesHaveTripCodeToSearch;
                pagerServices.VirtualItemCount = expenseServicesHaveTripCodeToSearch.Count;
                rptExpenseServices.DataBind();
            }
            else
            {
                rptExpenseServices.DataSource = expenseServices;
                rptExpenseServices.DataBind();
            }

            var expenseServicesHsf = GetDataHsf();
            var expenseServicesHaveTripCodeToSearchHsf = new List<ExpenseService>();
            foreach (ExpenseService expenseService in expenseServicesHsf)
            {
                var expenseServiceTripCode = string.Format("{0}{1}-{2:00}", expenseService.Expense.Trip.TripCode, expenseService.Expense.Date.ToString("ddMMyy"), expenseService.Group);
                var isExpenseServiceTripCodeContainTripCodeToSearch = expenseServiceTripCode.IndexOf(tripCodeToSearch, StringComparison.InvariantCultureIgnoreCase) >= 0;
                if (isExpenseServiceTripCodeContainTripCodeToSearch)
                {
                    expenseServicesHaveTripCodeToSearchHsf.Add(expenseService);
                }
            }

            if (tripCodeToSearch != "")
            {
                rptExpenseServicesHsf.DataSource = expenseServicesHaveTripCodeToSearchHsf;
                pagerServicesHsf.VirtualItemCount = expenseServicesHaveTripCodeToSearchHsf.Count;
                rptExpenseServicesHsf.DataBind();
            }
            else
            {
                rptExpenseServicesHsf.DataSource = expenseServicesHsf;
                pagerServicesHsf.VirtualItemCount = expenseServicesHsf.Count;
                rptExpenseServicesHsf.DataBind();
            }

            if (Request.QueryString["expenseserviceid"] != null)
            {
                var expenseServiceId = Convert.ToInt32(Request.QueryString["expenseserviceid"]);
                var expenseService = Module.ExpenseServiceGetById(expenseServiceId);
                if (expenseService.Expense.Trip.TripCode == "HUESF")
                {
                    expenseServicesHsf = new List<ExpenseService>();
                    expenseServicesHsf.Add(expenseService);
                    rptExpenseServicesHsf.DataSource = expenseServicesHsf;
                    pagerServicesHsf.VirtualItemCount = expenseServicesHsf.Count;
                    rptExpenseServicesHsf.DataBind();
                }
                else
                {
                    expenseServices = new List<ExpenseService>();
                    expenseServices.Add(expenseService);
                    rptExpenseServices.DataSource = expenseServices;
                    pagerServices.VirtualItemCount = expenseServices.Count;
                    rptExpenseServices.DataBind();
                }
            }
        }

        protected IList GetData()
        {
            DateTime from;
            DateTime to;
            if (string.IsNullOrEmpty(Request.QueryString["from"]) && string.IsNullOrEmpty(Request.QueryString["mode"]))
            {
                from = DateTime.Today.AddDays(-DateTime.Today.Day + 1);
                to = from.AddMonths(1).AddDays(-1);
            }
            else
            {
                from = DateTime.FromOADate(Convert.ToDouble(Request.QueryString["from"]));
                to = DateTime.FromOADate(Convert.ToDouble(Request.QueryString["to"]));
            }
            Agency agency = null;
            CostType type = null;

            if (!string.IsNullOrEmpty(Request.QueryString["supplierid"]))
            {
                agency = Module.AgencyGetById(Convert.ToInt32(Request.QueryString["supplierid"]));
            }

            if (!string.IsNullOrEmpty(Request.QueryString["costtype"]))
            {
                type = Module.CostTypeGetById(Convert.ToInt32(Request.QueryString["costtype"]));
            }

            string paymentStatus = "";
            if (!string.IsNullOrEmpty(Request.QueryString["ps"]))
            {
                paymentStatus = Request.QueryString["ps"];
            }

            string tripCode = "";

            if (Request.QueryString["mode"] != "all")
            {
                if (Request.QueryString["cost"] == "bycustomer")
                {
                    IList types = Module.CostTypeGetAll();
                    var intypes = new List<CostType>();
                    foreach (CostType t in types)
                    {
                        if (!t.IsDailyInput && t.IsSupplier)
                        {
                            intypes.Add(t);
                        }
                    }
                    return Module.ExpenseServiceGet(null, from, to, agency, pagerServices, Request.QueryString["orgid"], UserIdentity, true, paymentStatus, 1, tripCode, intypes.ToArray());
                }
                return Module.ExpenseServiceGet(null, @from, to, agency, pagerServices, Request.QueryString["orgid"], UserIdentity, true, paymentStatus, 1, tripCode, type);
            }
            return Module.ExpenseServiceGet(null, null, null, agency, pagerServices, Request.QueryString["orgid"], UserIdentity, true, paymentStatus, 1, tripCode, type);
        }

        protected IList GetDataHsf()
        {
            DateTime from;
            DateTime to;
            if (string.IsNullOrEmpty(Request.QueryString["from"]) && string.IsNullOrEmpty(Request.QueryString["mode"]))
            {
                from = DateTime.Today.AddDays(-DateTime.Today.Day + 1);
                to = from.AddMonths(1).AddDays(-1);
            }
            else
            {
                from = DateTime.FromOADate(Convert.ToDouble(Request.QueryString["from"]));
                to = DateTime.FromOADate(Convert.ToDouble(Request.QueryString["to"]));
            }
            Agency agency = null;
            CostType type = null;

            if (!string.IsNullOrEmpty(Request.QueryString["supplierid"]))
            {
                agency = Module.AgencyGetById(Convert.ToInt32(Request.QueryString["supplierid"]));
            }

            if (!string.IsNullOrEmpty(Request.QueryString["costtype"]))
            {
                type = Module.CostTypeGetById(Convert.ToInt32(Request.QueryString["costtype"]));
            }

            string paymentStatus = "";
            if (!string.IsNullOrEmpty(Request.QueryString["ps"]))
            {
                paymentStatus = Request.QueryString["ps"];
            }

            string tripCode = "";

            if (Request.QueryString["mode"] != "all")
            {
                if (Request.QueryString["cost"] == "bycustomer")
                {
                    IList types = Module.CostTypeGetAll();
                    var intypes = new List<CostType>();
                    foreach (CostType t in types)
                    {
                        if (!t.IsDailyInput && t.IsSupplier)
                        {
                            intypes.Add(t);
                        }
                    }
                    return Module.ExpenseServiceGet(null, from, to, agency, pagerServicesHsf, Request.QueryString["orgid"], UserIdentity, true, paymentStatus, 2, tripCode,
                                                                             intypes.ToArray());
                }
                return Module.ExpenseServiceGet(null, @from, to, agency, pagerServicesHsf, Request.QueryString["orgid"], UserIdentity, true, paymentStatus, 2, tripCode, type);
            }
            return Module.ExpenseServiceGet(null, null, null, agency, pagerServicesHsf, Request.QueryString["orgid"], UserIdentity, true, paymentStatus, 2, tripCode, type);
        }

        protected void rptExpenseServices_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is ExpenseService)
            {
                var service = (ExpenseService)e.Item.DataItem;

                //if (service.Cost == 0 || service.Type.IsMonthly || service.Type.IsYearly)
                if (service.Type.IsMonthly || service.Type.IsYearly)
                {
                    e.Item.Visible = false;
                    return;
                }
                var hplDate = e.Item.FindControl("hplDate") as HyperLink;
                if (hplDate != null)
                {
                    hplDate.Text = service.Expense.Date.ToString("dd/MM/yyyy");
                }

                var hplTripCode = e.Item.FindControl("hplTripCode") as HyperLink;
                if (hplTripCode != null)
                {
                    hplTripCode.Text = string.Format("{0}{1}-{2:00}", service.Expense.Trip.TripCode, service.Expense.Date.ToString("ddMMyy"), service.Group);
                    hplTripCode.NavigateUrl =
                        string.Format("EventEdit.aspx?NodeId={0}&SectionId={1}&expenseid={2}&group={3}", Node.Id,
                                      Section.Id, service.Expense.Id, service.Group);
                }

                var hplPartner = e.Item.FindControl("hplPartner") as HyperLink;
                if (hplPartner != null)
                {
                    if (service.Supplier != null)
                    {
                        hplPartner.Text = service.Supplier.Name;
                        hplPartner.NavigateUrl =
                            string.Format("PayableList.aspx?NodeId={0}&SectionId={1}&from={3}&to={4}&supplierid={2}", Node.Id, Section.Id,
                                          service.Supplier.Id, Request.QueryString["from"], Request.QueryString["to"]);
                    }
                }

                var hplService = e.Item.FindControl("hplService") as HyperLink;
                if (hplService != null)
                {
                    hplService.Text = service.Type.Name;
                    hplService.NavigateUrl = string.Format("PayableList.aspx?NodeId={0}&SectionId={1}", Node.Id, Section.Id);
                }

                var litTotal = e.Item.FindControl("litTotal") as Literal;
                if (litTotal != null)
                {
                    litTotal.Text = service.Cost.ToString("#,0.#");
                    _total += service.Cost;
                }

                var litPaid = e.Item.FindControl("litPaid") as Literal;
                if (litPaid != null)
                {
                    litPaid.Text = service.Paid.ToString("#,0.#");
                    _paid += service.Paid;
                }

                var litPayable = e.Item.FindControl("litPayable") as Literal;
                if (litPayable != null)
                {
                    litPayable.Text = (service.Cost - service.Paid).ToString("#,0.#");
                    _payable += service.Cost - service.Paid;
                }

                if (service.PaidDate.HasValue)
                {
                    ValueBinder.BindLiteral(e.Item, "litPaidOn", service.PaidDate.Value);
                }
            }

            if (e.Item.ItemType == ListItemType.Footer)
            {
                var litTotal = e.Item.FindControl("litTotal") as Literal;
                if (litTotal != null)
                {
                    litTotal.Text = _total.ToString("#,0.#");
                    _total = 0;
                }

                var litPaid = e.Item.FindControl("litPaid") as Literal;
                if (litPaid != null)
                {
                    litPaid.Text = _paid.ToString("#,0.#");
                    _paid = 0;
                }

                var litPayable = e.Item.FindControl("litPayable") as Literal;
                if (litPayable != null)
                {
                    litPayable.Text = _payable.ToString("#,0.#");
                    _payable = 0;
                }
            }
        }

        protected void rptExpenseServicesHsf_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is ExpenseService)
            {
                var service = (ExpenseService)e.Item.DataItem;

                //if (service.Cost == 0 || service.Type.IsMonthly || service.Type.IsYearly)
                if (service.Type.IsMonthly || service.Type.IsYearly)
                {
                    e.Item.Visible = false;
                    return;
                }
                var hplDate = e.Item.FindControl("hplDate") as HyperLink;
                if (hplDate != null)
                {
                    hplDate.Text = service.Expense.Date.ToString("dd/MM/yyyy");
                }

                var hplTripCode = e.Item.FindControl("hplTripCode") as HyperLink;
                if (hplTripCode != null)
                {
                    hplTripCode.Text = string.Format("{0}{1}-{2:00}", service.Expense.Trip.TripCode, service.Expense.Date.ToString("ddMMyy"), service.Group);
                    hplTripCode.NavigateUrl =
                        string.Format("EventEdit.aspx?NodeId={0}&SectionId={1}&expenseid={2}&group={3}", Node.Id,
                                      Section.Id, service.Expense.Id, service.Group);
                }

                var hplPartner = e.Item.FindControl("hplPartner") as HyperLink;
                if (hplPartner != null)
                {
                    if (service.Supplier != null)
                    {
                        hplPartner.Text = service.Supplier.Name;
                        hplPartner.NavigateUrl =
                            string.Format("PayableList.aspx?NodeId={0}&SectionId={1}&from={3}&to={4}&supplierid={2}", Node.Id, Section.Id,
                                          service.Supplier.Id, Request.QueryString["from"], Request.QueryString["to"]);
                    }
                }

                var hplService = e.Item.FindControl("hplService") as HyperLink;
                if (hplService != null)
                {
                    hplService.Text = service.Type.Name;
                    hplService.NavigateUrl = string.Format("PayableList.aspx?NodeId={0}&SectionId={1}", Node.Id, Section.Id);
                }

                var litTotal = e.Item.FindControl("litTotal") as Literal;
                if (litTotal != null)
                {
                    litTotal.Text = service.Cost.ToString("#,0.#");
                    _total += service.Cost;
                }

                var litPaid = e.Item.FindControl("litPaid") as Literal;
                if (litPaid != null)
                {
                    litPaid.Text = service.Paid.ToString("#,0.#");
                    _paid += service.Paid;
                }

                var litPayable = e.Item.FindControl("litPayable") as Literal;
                if (litPayable != null)
                {
                    litPayable.Text = (service.Cost - service.Paid).ToString("#,0.#");
                    _payable += service.Cost - service.Paid;
                }

                if (service.PaidDate.HasValue)
                {
                    ValueBinder.BindLiteral(e.Item, "litPaidOn", service.PaidDate.Value);
                }
            }

            if (e.Item.ItemType == ListItemType.Footer)
            {
                var litTotal = e.Item.FindControl("litTotal") as Literal;
                if (litTotal != null)
                {
                    litTotal.Text = _total.ToString("#,0.#");
                }

                var litPaid = e.Item.FindControl("litPaid") as Literal;
                if (litPaid != null)
                {
                    litPaid.Text = _paid.ToString("#,0.#");
                }

                var litPayable = e.Item.FindControl("litPayable") as Literal;
                if (litPayable != null)
                {
                    litPayable.Text = _payable.ToString("#,0.#");
                }
            }
        }
        protected void rptExpenseServices_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Footer)
            {
                var service = Module.ExpenseServiceGetById(Convert.ToInt32(e.CommandArgument));
                var txtPay = (TextBox)e.Item.FindControl("txtPay");
                double paid;

                if (!string.IsNullOrEmpty(txtPay.Text))
                {
                    paid = Convert.ToDouble(txtPay.Text);
                }
                else
                {
                    paid = service.Cost - service.Paid;
                }

                service.Paid += paid;

                if (service.Paid == service.Cost && !service.PaidDate.HasValue)
                {
                    service.PaidDate = DateTime.Now;
                }
                var transaction = new Transaction();
                transaction.CreatedBy = UserIdentity;
                transaction.CreatedDate = DateTime.Now;
                transaction.Agency = service.Supplier;
                transaction.IsExpense = true;
                transaction.VNDAmount = paid;

                if (service.Type.IsDailyInput)
                {
                    transaction.TransactionType = Transaction.MANUALDAILY_EXPENSE;
                }
                else
                {
                    transaction.TransactionType = Transaction.CALCULATED_EXPENSE;
                }

                Module.SaveOrUpdate(transaction);
                Module.SaveOrUpdate(service);
                GetDataSource();
            }
            else
            {
                var list = GetData();
                var serviceHsfs = GetDataHsf();
                foreach (ExpenseService service in list)
                {
                    if (service.Paid != service.Cost)
                    {
                        service.Paid = service.Cost;
                        service.PaidDate = DateTime.Now;
                        Module.SaveOrUpdate(service);
                    }
                }

                foreach (ExpenseService serviceHsf in serviceHsfs)
                {
                    if (serviceHsf.Paid != serviceHsf.Cost)
                    {
                        serviceHsf.Paid = serviceHsf.Cost;
                        serviceHsf.PaidDate = DateTime.Now;
                        Module.SaveOrUpdate(serviceHsf);
                    }
                }
                rptExpenseServices.DataSource = list;
                rptExpenseServices.DataBind();
                rptExpenseServicesHsf.DataSource = serviceHsfs;
                rptExpenseServicesHsf.DataBind();
            }
        }
        protected void rptExpenseServicesHsf_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Footer)
            {
                var service = Module.ExpenseServiceGetById(Convert.ToInt32(e.CommandArgument));
                var txtPay = (TextBox)e.Item.FindControl("txtPay");
                double paid;

                if (!string.IsNullOrEmpty(txtPay.Text))
                {
                    paid = Convert.ToDouble(txtPay.Text);
                }
                else
                {
                    paid = service.Cost - service.Paid;
                }

                service.Paid += paid;

                if (service.Paid == service.Cost && !service.PaidDate.HasValue)
                {
                    service.PaidDate = DateTime.Now;
                }
                var transaction = new Transaction();
                transaction.CreatedBy = UserIdentity;
                transaction.CreatedDate = DateTime.Now;
                transaction.Agency = service.Supplier;
                transaction.IsExpense = true;
                transaction.VNDAmount = paid;

                if (service.Type.IsDailyInput)
                {
                    transaction.TransactionType = Transaction.MANUALDAILY_EXPENSE;
                }
                else
                {
                    transaction.TransactionType = Transaction.CALCULATED_EXPENSE;
                }

                Module.SaveOrUpdate(transaction);
                Module.SaveOrUpdate(service);
                GetDataSource();
            }
            else
            {
                var list = GetData();
                var serviceHsfs = GetDataHsf();
                foreach (ExpenseService service in list)
                {
                    if (service.Paid != service.Cost)
                    {
                        service.Paid = service.Cost;
                        service.PaidDate = DateTime.Now;
                        Module.SaveOrUpdate(service);
                    }
                }

                foreach (ExpenseService serviceHsf in serviceHsfs)
                {
                    if (serviceHsf.Paid != serviceHsf.Cost)
                    {
                        serviceHsf.Paid = serviceHsf.Cost;
                        serviceHsf.PaidDate = DateTime.Now;
                        Module.SaveOrUpdate(serviceHsf);
                    }
                }
                rptExpenseServices.DataSource = list;
                rptExpenseServices.DataBind();
                rptExpenseServicesHsf.DataSource = serviceHsfs;
                rptExpenseServicesHsf.DataBind();
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            var excelFile = new ExcelFile();
            excelFile.LoadXls(Server.MapPath("/Modules/Sails/Admin/ExportTemplates/MultiService.xls"));

            ExcelWorksheet sheet;

            #region -- Get data --
            DateTime from = DateTime.FromOADate(Convert.ToDouble(Request.QueryString["from"]));
            DateTime to = DateTime.FromOADate(Convert.ToDouble(Request.QueryString["to"]));

            Agency agency = null;
            CostType type = null;

            string paymentStatus = "";
            if (Request.QueryString["ps"] != null)
            {
                paymentStatus = Request.QueryString["ps"];
            }


            if (Request.QueryString["supplierid"] != null)
            {
                agency = Module.AgencyGetById(Convert.ToInt32(Request.QueryString["supplierid"]));
            }

            if (Request.QueryString["costtype"] != null)
            {
                type = Module.CostTypeGetById(Convert.ToInt32(Request.QueryString["costtype"]));
            }

            string tripCode = "";
            if (Request.QueryString["tripcode"] != null)
            {
                tripCode = Request.QueryString["tripcode"];
            }

            var data = Module.ExpenseServiceGet(null, from, to, agency, null, Request.QueryString["orgid"], UserIdentity, true, paymentStatus, 0, tripCode, type);

            if (hdnSelectedTab.Value == "0")
            {
                data = Module.ExpenseServiceGet(null, from, to, agency, null, Request.QueryString["orgid"], UserIdentity, true, paymentStatus, 1, tripCode, type);
            }

            if (hdnSelectedTab.Value == "1")
            {
                data = Module.ExpenseServiceGet(null, from, to, agency, null, Request.QueryString["orgid"], UserIdentity, true, paymentStatus, 2, tripCode, type);
            }
            #endregion

            #region -- Các thông tin chung --
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

            #region -- Get Supplier list --
            IList agencies = new ArrayList();
            if (agency != null)
            {
                sheet = excelFile.Worksheets[0];
                IList costTypes = new ArrayList();
                foreach (ExpenseService service in data)
                {
                    if (!costTypes.Contains(service.Type))
                    {
                        costTypes.Add(service.Type);
                    }
                }
                ExportAgencyData(data, sheet, time, costTypes);
            }
            else
            {
                foreach (ExpenseService service in data)
                {
                    if (!agencies.Contains(service.Supplier) && service.Supplier != null)
                    {
                        agencies.Add(service.Supplier);
                    }
                }

                foreach (Agency supplier in agencies)
                {
                    sheet = excelFile.Worksheets.AddCopy(string.Format("{0} ({1})", supplier.Name, supplier.Id), excelFile.Worksheets[0]);

                    // Tạo sheet mới, sao chép nguyên từ sheet cũ, số lượng sheet = số lượng agency

                    IList list = new ArrayList();

                    // Chỉ lấy các booking chưa trả hết nợ của agency này
                    foreach (ExpenseService service in data)
                    {
                        if (service.Supplier != supplier)
                        {
                            continue;
                        }
                        // Chỉ loại trừ khi nợ đúng bằng 0
                        //if (service.Paid != service.Cost)
                        //{
                        list.Add(service);
                        //}
                    }

                    IList costTypes = new ArrayList();
                    foreach (ExpenseService service in list)
                    {
                        if (!costTypes.Contains(service.Type) && service.Cost > 0)
                        {
                            costTypes.Add(service.Type);
                        }
                    }

                    ExportAgencyData(list, sheet, time, costTypes);
                }
            }
            #endregion

            #region -- Trả dữ liệu về cho người dùng --

            // Xóa sheet mẫu
            if (excelFile.Worksheets.Count > 2)
            {
                excelFile.Worksheets[0].Delete();
            }

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            if (hdnSelectedTab.Value == "0")
            {
                Response.AppendHeader("content-disposition", "attachment; filename=" + string.Format("Congno_DLG_{0}.xls", time));
            }
            if (hdnSelectedTab.Value == "1")
            {
                Response.AppendHeader("content-disposition", "attachment; filename=" + string.Format("Congno_HUESF_{0}.xls", time));
            }
            var m = new MemoryStream();

            excelFile.SaveXls(m);

            Response.OutputStream.Write(m.GetBuffer(), 0, m.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();

            m.Close();
            Response.End();

            #endregion
        }

        protected void btnExportOneSheet_Click(object sender, EventArgs e)
        {
            ExportOneSheet();
        }

        protected void btnExportGuide_Click(object sender, EventArgs e)
        {
            // lấy về danh sách guide
            var guides = Module.SupplierGetAll(20); // guide cost type id = 20
            // lấy về danh sách trips
            var trips = Module.TripGetAll(true, UserIdentity);

            // tổng kết trips theo guide
            DateTime from = DateTime.FromOADate(Convert.ToDouble(Request.QueryString["from"]));
            DateTime to = DateTime.FromOADate(Convert.ToDouble(Request.QueryString["to"]));

            // mở excel template
            var excelFile = new ExcelFile();
            excelFile.LoadXls(Server.MapPath("/Modules/Sails/Admin/ExportTemplates/GuideTrip.xls"));

            ExcelWorksheet sheet = excelFile.Worksheets[0];

            for (int ii = 0; ii < guides.Count; ii++)
            {
                sheet.Cells[4 + ii, 1].Value = ((Agency)guides[ii]).Name;
            }

            for (int jj = 0; jj < trips.Count; jj++)
            {
                sheet.Cells[3, 2 + jj].Value = ((SailsTrip)trips[jj]).Name;
            }

            for (int ii = 0; ii < guides.Count; ii++)
            {
                var totalCrit = Expression.And(Expression.Ge("expense.Date", from),
                                                   Expression.Le("expense.Date", to));
                totalCrit = Expression.And(totalCrit, Expression.Eq("Supplier", guides[ii]));
                var total = Module.SailsDao.ExpenseServiceCount(totalCrit);
                if (total == 0)
                {
                    sheet.Rows[4 + ii].Hidden = true;
                    continue;
                    // neu guide khong di luot nao thi bo qua
                }

                for (int jj = 0; jj < trips.Count; jj++)
                {
                    var criterion = Expression.And(Expression.Ge("expense.Date", from),
                                                   Expression.Le("expense.Date", to));
                    criterion = Expression.And(criterion,
                                               Expression.And(Expression.Eq("expense.Trip", trips[jj]),
                                                              Expression.Eq("Supplier", guides[ii])));
                    sheet.Cells[4 + ii, 2 + jj].Value = Module.SailsDao.ExpenseServiceCount(criterion);
                }
            }

            #region -- Trả dữ liệu về cho người dùng --
            string time;
            if (from.AddMonths(1).AddDays(-1) == to)
            {
                time = from.ToString("MMM_yyyy");
            }
            else
            {
                time = string.Format("{0:dd/MM/yyyy}_{1:dd/MM/yyyy}", from, to);
            }
            // Xóa sheet mẫu
            //if (excelFile.Worksheets.Count > 0)
            //{
            //    excelFile.Worksheets[0].Delete();
            //}

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AppendHeader("content-disposition", "attachment; filename=" + string.Format("Guide{0}.xls", time));

            var m = new MemoryStream();

            excelFile.SaveXls(m);

            Response.OutputStream.Write(m.GetBuffer(), 0, m.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();

            m.Close();
            Response.End();

            #endregion
            //Module.SailsDao.ExpenseServiceCount();

        }

        protected void btnExportPayables_Click(object sender, EventArgs e)
        {
            var excelFile = new ExcelFile();
            excelFile.LoadXls(Server.MapPath("/Modules/Sails/Admin/ExportTemplates/ExportAllPayablesTemplate.xls"));


            #region -- Get data --
            DateTime from = DateTime.FromOADate(Convert.ToDouble(Request.QueryString["from"]));
            DateTime to = DateTime.FromOADate(Convert.ToDouble(Request.QueryString["to"]));

            Agency agency = null;
            CostType type = null;

            string paymentStatus = "";
            if (Request.QueryString["ps"] != null)
            {
                paymentStatus = Request.QueryString["ps"];
            }


            if (Request.QueryString["supplierid"] != null)
            {
                agency = Module.AgencyGetById(Convert.ToInt32(Request.QueryString["supplierid"]));
            }

            if (Request.QueryString["costtype"] != null)
            {
                type = Module.CostTypeGetById(Convert.ToInt32(Request.QueryString["costtype"]));
            }

            string tripCode = "";
            if (Request.QueryString["tripcode"] != null)
            {
                tripCode = Request.QueryString["tripcode"];
            }

            var data = Module.ExpenseServiceGet(null, from, to, agency, null, Request.QueryString["orgid"], UserIdentity, true, paymentStatus, 0, tripCode, type);

            if (hdnSelectedTab.Value == "0")
            {
                data = Module.ExpenseServiceGet(null, from, to, agency, null, Request.QueryString["orgid"], UserIdentity, true, paymentStatus, 1, tripCode, type);
            }

            if (hdnSelectedTab.Value == "1")
            {
                data = Module.ExpenseServiceGet(null, from, to, agency, null, Request.QueryString["orgid"], UserIdentity, true, paymentStatus, 2, tripCode, type);
            }
            #endregion

            #region -- Các thông tin chung --
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

            #region -- Thông tin chung --
            const int WORKSHEET_TOTAL_PAYABLES = 0;
            const int WORKSHEET_SUB_PAYABLES = 1;
            const int COL_DATE = 0;
            const int COL_TRIPCODE = 1;
            const int COL_PARTNER = 2;
            const int COL_SERVICE = 3;
            const int COL_TOTAL = 4;
            const int FIRSTROW = 6;

            ExcelWorksheet wsTotalPayables = excelFile.Worksheets[WORKSHEET_TOTAL_PAYABLES];
            wsTotalPayables.Cells["E4"].Value = time;
            var row = FIRSTROW;
            double _total = 0.0;
            foreach (ExpenseService es in data)
            {
                wsTotalPayables.Rows[row].InsertCopy(1, wsTotalPayables.Rows[row]);
                wsTotalPayables.Cells[row, COL_DATE].Value = es.Expense.Date.ToString("dd/MM/yyyy");
                wsTotalPayables.Cells[row, COL_TRIPCODE].Value = string.Format("{0}{1}-{2:00}", es.Expense.Trip.TripCode, es.Expense.Date.ToString("ddMMyy"), es.Group);
                wsTotalPayables.Cells[row, COL_PARTNER].Value = es.Supplier != null ? es.Supplier.Name : "";
                wsTotalPayables.Cells[row, COL_SERVICE].Value = es.Type.Name;
                wsTotalPayables.Cells[row, COL_TOTAL].Value = es.Cost.ToString("#,0.#");
                row = row + 1;
                _total += es.Cost;
            }
            wsTotalPayables.Rows[row].Delete();
            var totalRow = row;
            wsTotalPayables.Cells[totalRow, COL_TOTAL].Value = _total.ToString("#,0.#");

            var subData = Module.ExpenseServiceGet(null, from, to, agency, null, Request.QueryString["orgid"], UserIdentity, true, paymentStatus, 0, tripCode, type);

            if (hdnSelectedTab.Value == "0")
            {
                subData = Module.ExpenseServiceGet(null, from, to, agency, pagerServices, Request.QueryString["orgid"], UserIdentity, true, paymentStatus, 1, tripCode, type);
            }

            if (hdnSelectedTab.Value == "1")
            {
                subData = Module.ExpenseServiceGet(null, from, to, agency, pagerServicesHsf, Request.QueryString["orgid"], UserIdentity, true, paymentStatus, 2, tripCode, type);
            }

            ExcelWorksheet wsSubPayables = excelFile.Worksheets[WORKSHEET_SUB_PAYABLES];
            wsTotalPayables.Cells["E4"].Value = time;
            row = FIRSTROW;
            _total = 0.0;
            foreach (ExpenseService es in subData)
            {
                wsSubPayables.Rows[row].InsertCopy(1, wsTotalPayables.Rows[row]);
                wsSubPayables.Cells[row, COL_DATE].Value = es.Expense.Date.ToString("dd/MM/yyyy");
                wsSubPayables.Cells[row, COL_TRIPCODE].Value = string.Format("{0}{1}-{2:00}", es.Expense.Trip.TripCode, es.Expense.Date.ToString("ddMMyy"), es.Group);
                wsSubPayables.Cells[row, COL_PARTNER].Value = es.Supplier != null ? es.Supplier.Name : "";
                wsSubPayables.Cells[row, COL_SERVICE].Value = es.Type.Name;
                wsSubPayables.Cells[row, COL_TOTAL].Value = es.Cost.ToString("#,0.#");
                row = row + 1;
                _total += es.Cost;
            }
            wsSubPayables.Rows[row].Delete();
            totalRow = row;
            wsSubPayables.Cells[totalRow, COL_TOTAL].Value = _total.ToString("#,0.#");
            #endregion

            #region -- Trả dữ liệu về cho người dùng --

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            if (hdnSelectedTab.Value == "0")
            {
                Response.AppendHeader("content-disposition", "attachment; filename=" + string.Format("TotalPayables_DLG_{0}.xls", time));
            }
            if (hdnSelectedTab.Value == "1")
            {
                Response.AppendHeader("content-disposition", "attachment; filename=" + string.Format("TotalPayables_HUESF_{0}.xls", time));
            }
            var m = new MemoryStream();

            excelFile.SaveXls(m);

            Response.OutputStream.Write(m.GetBuffer(), 0, m.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();

            m.Close();
            Response.End();

            #endregion
        }

        /// <summary>
        /// Xuất dữ liệu dưới dạng một sheet duy nhất
        /// </summary>
        protected void ExportOneSheet()
        {
            var excelFile = new ExcelFile();
            excelFile.LoadXls(Server.MapPath("/Modules/Sails/Admin/ExportTemplates/MultiService.xls"));

            ExcelWorksheet sheet = excelFile.Worksheets[0];

            DateTime from = DateTime.FromOADate(Convert.ToDouble(Request.QueryString["from"]));
            DateTime to = DateTime.FromOADate(Convert.ToDouble(Request.QueryString["to"]));

            Agency agency = null;
            CostType type = null;
            bool all = false;

            if (Request.QueryString["supplierid"] != null)
            {
                agency = Module.AgencyGetById(Convert.ToInt32(Request.QueryString["supplierid"]));
            }

            if (Request.QueryString["costtype"] != null)
            {
                type = Module.CostTypeGetById(Convert.ToInt32(Request.QueryString["costtype"]));
            }

            #region -- Generate Criterion --
            ICriterion criterion = Expression.Gt("Cost", 0d); // không lấy chi phí = 0

            IList costtypes;

            if (Request.QueryString["cost"] == "bycustomer")
            {
                IList types = Module.CostTypeGetAll();
                costtypes = new List<CostType>();
                foreach (CostType t in types)
                {
                    if (!t.IsDailyInput && t.IsSupplier)
                    {
                        costtypes.Add(t);
                    }
                }
            }
            else if (type == null)
            {
                costtypes = Module.CostTypeGetAll(); // lấy tất cả các loại chi phí vào 1 sheet
                all = true;
            }
            else
            {
                costtypes = new ArrayList();
                costtypes.Add(type);
            }

            if (to > DateTime.Today)
            {
                to = DateTime.Today;
            }

            criterion = Expression.And(criterion,
                                       Expression.And(Expression.Ge("expense.Date", from),
                                                      Expression.Le("expense.Date", to)));

            if (agency != null)
            {
                criterion = Expression.And(criterion, (Expression.Eq("Supplier", agency)));
            }

            if (costtypes.Count > 0 && !all)
            {
                // ReSharper disable SuspiciousTypeConversion.Global
                // ReSharper disable PossibleInvalidCastException
                criterion = Expression.And(criterion, Expression.In("Type", (object[])((IList<CostType>)type)));
                // ReSharper restore PossibleInvalidCastException
                // ReSharper restore SuspiciousTypeConversion.Global
            }

            //if (hideZero)
            //{
            //    criteria.Add(Expression.Gt("Cost", 0d));
            //}
            string orgid = Request.QueryString["orgid"];
            if (!string.IsNullOrEmpty(orgid))
            {
                criterion = Expression.And(criterion, Expression.Eq("trip.Organization.Id", Convert.ToInt32(orgid)));
            }
            else
            {
                // giới hạn?
            }
            #endregion

            int totalCount = Module.SailsDao.ExpenseServiceCount(criterion);

            #region -- Thông tin chung --
            string time;
            if (from.AddMonths(1).AddDays(-1) == to)
            {
                time = from.ToString("MMM_yyyy");
            }
            else
            {
                time = string.Format("{0:dd/MM/yyyy}_{1:dd/MM/yyyy}", from, to);
            }

            sheet.Cells["E4"].Value = time;
            sheet.Cells["E17"].Value = UserIdentity.FullName;
            #endregion

            #region -- Xuất chi tiết data --
            const int firstCol = 4;
            const int COL_DATE = firstCol - 4;
            const int COL_ADULT = firstCol - 3;
            const int COL_CHILD = firstCol - 2;
            const int COL_TRIP = firstCol - 1;
            const int firstRow = 7;
            if (costtypes.Count > 1)
            {
                for (int ii = 1; ii < 50; ii++)
                {
                    sheet.Columns[100].Delete();
                }
                sheet.Columns[firstCol].InsertCopy(costtypes.Count - 1, sheet.Columns[firstCol]);
            }

            for (int ii = 0; ii < costtypes.Count; ii++)
            {
                sheet.Cells[firstRow - 1, firstCol + ii].Value = ((CostType)costtypes[ii]).Name;
            }

            if (costtypes.Count > 1)
            {
                #region -- Đa dịch vụ --

                var serviceSum = new Dictionary<CostType, double>(); // Tổng chi phí cho từng dịch vụ

                foreach (CostType ct in costtypes)
                {
                    serviceSum.Add(ct, 0);
                }

                sheet.Rows[firstRow].InsertCopy(totalCount - 1, sheet.Rows[firstRow]);
                int count = 0;
                double total = 0;
                const int pageSize = 300;
                DateTime currentDate = DateTime.MinValue;

                int numPage = totalCount / pageSize;

                if (count % pageSize > 0)
                {
                    numPage++;
                }

                for (var ii = 0; ii < numPage; ii++)
                {
                    var temp = Module.SailsDao.ExpenseServiceGet(criterion, pageSize, ii);// lấy về danh sách object tạm thời
                    for (var jj = 0; jj < temp.Count; jj++)
                    {
                        var service = temp[jj];
                        var rowIndex = firstRow + count;
                        var isFirstTime = false; // có phải dòng đầu tiên trong ngày     
                        var currentGroup = -1; // group hiện tại               

                        if (currentDate != service.Expense.Date)
                        {
                            sheet.Cells[rowIndex, COL_DATE].Value = service.Expense.Date;
                            currentDate = service.Expense.Date;
                            isFirstTime = true;
                            currentGroup = -1;
                        }

                        //bool isFirstOfGroup;
                        if (currentGroup == -1)
                        {
                            rowIndex = firstRow + count;
                            count++;
                            //isFirstOfGroup = true;
                        }
                        else
                        {
                            rowIndex = firstRow + count - 1;
                            //isFirstOfGroup = false;
                        }

                        IList bookings = GetBooking(service.Expense.Date);
                        int adult = 0;
                        int child = 0;
                        foreach (Booking booking in bookings)
                        {
                            if (booking.Group == service.Group && (service.Group > 0 || isFirstTime) && (booking.Trip.Id == service.Expense.Trip.Id))
                            {
                                adult += booking.Adult;
                                child += booking.Child;
                            }
                        }

                        sheet.Cells[rowIndex, COL_TRIP].Value = service.Expense.Trip.Name;
                        sheet.Cells[rowIndex, COL_ADULT].Value = adult;
                        sheet.Cells[rowIndex, COL_CHILD].Value = child;

                        double sum = 0;
                        double paid = 0;
                        for (int kk = 0; kk < costtypes.Count; kk++)
                        {
                            if (((CostType)costtypes[kk]).Id == service.Type.Id)
                            {
                                sheet.Cells[rowIndex, firstCol + kk].Value = service.Cost;
                                sum += service.Cost;
                                total += service.Cost;
                                paid += service.Paid;
                            }
                        }
                        sheet.Cells[rowIndex, firstCol + costtypes.Count].Value = sum;
                        sheet.Cells[rowIndex, firstCol + costtypes.Count + 1].Value = paid;
                        //currentGroup = service.Group;
                    }
                }

                int totalRowIndex = firstRow + count;
                sheet.Cells[totalRowIndex, firstCol + costtypes.Count].Value = total;
                #endregion
            }

            #endregion

            #region -- Trả dữ liệu về cho người dùng --

            // Xóa sheet mẫu
            //if (excelFile.Worksheets.Count > 0)
            //{
            //    excelFile.Worksheets[0].Delete();
            //}

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
        /// 
        /// </summary>
        /// <param name="list">Danh sách các chi phí</param>
        /// <param name="sheet">Sheet chèn dữ liệu</param>
        /// <param name="time">Thời gian</param>
        /// <param name="services">Danh sách các loại dịch vụ</param>
        private void ExportAgencyData(IList list, ExcelWorksheet sheet, string time, IList services)
        {
            #region -- Thông tin chung --

            sheet.Cells["E4"].Value = time;
            sheet.Cells["E17"].Value = UserIdentity.FullName;

            const int firstCol = 4;
            const int COL_DATE = firstCol - 4;
            const int COL_ADULT = firstCol - 3;
            const int COL_CHILD = firstCol - 2;
            const int COL_TRIP = firstCol - 1;
            const int firstRow = 7;
            if (services.Count > 1)
            {
                for (int ii = 1; ii < 50; ii++)
                {
                    sheet.Columns[100].Delete();
                }
                sheet.Columns[firstCol].InsertCopy(services.Count - 1, sheet.Columns[firstCol]);
            }

            for (int ii = 0; ii < services.Count; ii++)
            {
                sheet.Cells[firstRow - 1, firstCol + ii].Value = ((CostType)services[ii]).Name;
            }


            if (services.Count > 1)
            {
                #region -- Đa dịch vụ (old) --
                //List<DateTime> dates = new List<DateTime>();
                //Dictionary<DateTime, Dictionary<CostType, double>> map =
                //    new Dictionary<DateTime, Dictionary<CostType, double>>(); // bảng các bảng chi phí theo dịch, theo ngày

                //Dictionary<CostType, double> serviceSum = new Dictionary<CostType, double>(); // Tổng chi phí cho từng dịch vụ

                //foreach (CostType type in services)
                //{
                //    serviceSum.Add(type, 0);
                //}

                //foreach (ExpenseService service in list)
                //{
                //    if (!dates.Contains(service.Expense.Date))
                //    {
                //        dates.Add(service.Expense.Date);
                //        map.Add(service.Expense.Date, new Dictionary<CostType, double>());
                //        foreach (CostType type in services)
                //        {
                //            map[service.Expense.Date].Add(type, 0);
                //        }
                //    }
                //    map[service.Expense.Date][service.Type] += service.Cost - service.Paid;
                //}

                //sheet.Rows[firstRow].InsertCopy(dates.Count - 1, sheet.Rows[firstRow]);
                //int count = 0;
                //foreach (DateTime date in dates)
                //{
                //    int rowIndex = firstRow + count;
                //    sheet.Cells[rowIndex, COL_DATE].Value = date; // Cột ngày

                //    IList bookings = GetBooking(date);
                //    int adult = 0;
                //    int child = 0;
                //    foreach (Booking booking in bookings)
                //    {
                //        adult += booking.Adult;
                //        child += booking.Child;
                //    }

                //    sheet.Cells[rowIndex, COL_ADULT].Value = adult;
                //    sheet.Cells[rowIndex, COL_CHILD].Value = child;

                //    double sum = 0;
                //    for (int ii = 0; ii < services.Count; ii++)
                //    {
                //        sheet.Cells[rowIndex, firstCol + ii].Value = map[date][(CostType) services[ii]];
                //        serviceSum[(CostType) services[ii]] += map[date][(CostType) services[ii]];
                //        sum += map[date][(CostType) services[ii]];
                //    }
                //    sheet.Cells[rowIndex, firstCol + services.Count].Value = sum;

                //    count++;
                //}

                //int totalRowIndex = firstRow + count;
                //double total = 0;
                //for (int ii = 0; ii < services.Count; ii++)
                //{
                //    sheet.Cells[totalRowIndex, firstCol + ii].Value = serviceSum[(CostType) services[ii]];
                //    total += serviceSum[(CostType) services[ii]];
                //}
                //sheet.Cells[totalRowIndex, firstCol + services.Count].Value = total;
                #endregion

                #region -- Đa dịch vụ (new) --

                Dictionary<CostType, double> serviceSum = new Dictionary<CostType, double>(); // Tổng chi phí cho từng dịch vụ

                foreach (CostType type in services)
                {
                    serviceSum.Add(type, 0);
                }

                sheet.Rows[firstRow].InsertCopy(list.Count - 1, sheet.Rows[firstRow]);
                int count = 0;
                double total = 0;
                DateTime currentDate = DateTime.MinValue;


                foreach (ExpenseService service in list)
                {
                    int rowIndex = firstRow + count;
                    bool isFirstTime = false; // có phải dòng đầu tiên trong ngày     
                    int currentGroup = -1; // group hiện tại               

                    if (currentDate != service.Expense.Date)
                    {
                        sheet.Cells[rowIndex, COL_DATE].Value = service.Expense.Date;
                        currentDate = service.Expense.Date;
                        isFirstTime = true;
                        currentGroup = -1;
                    }

                    bool isFirstOfGroup;
                    if (currentGroup == -1)
                    {
                        rowIndex = firstRow + count;
                        count++;
                        isFirstOfGroup = true;
                    }
                    else
                    {
                        rowIndex = firstRow + count - 1;
                        isFirstOfGroup = false;
                    }

                    IList bookings = GetBooking(service.Expense.Date);
                    int adult = 0;
                    int child = 0;
                    foreach (Booking booking in bookings)
                    {
                        if (booking.Group == service.Group && (service.Group > 0 || isFirstTime) && (booking.Trip.Id == service.Expense.Trip.Id))
                        {
                            adult += booking.Adult;
                            child += booking.Child;
                        }
                    }

                    sheet.Cells[rowIndex, COL_TRIP].Value = service.Expense.Trip.Name;
                    sheet.Cells[rowIndex, COL_ADULT].Value = adult;
                    sheet.Cells[rowIndex, COL_CHILD].Value = child;

                    double sum = 0;
                    for (int ii = 0; ii < services.Count; ii++)
                    {
                        if (((CostType)services[ii]).Id == service.Type.Id)
                        {
                            sheet.Cells[rowIndex, firstCol + ii].Value = service.Cost;
                            sum += service.Cost;
                            total += service.Cost;
                        }
                    }
                    sheet.Cells[rowIndex, firstCol + services.Count].Value = sum;

                    currentGroup = service.Group;

                }

                int totalRowIndex = firstRow + count;
                sheet.Cells[totalRowIndex, firstCol + services.Count].Value = total;
                #endregion
            }
            else
            {
                sheet.Rows[firstRow].InsertCopy(list.Count - 1, sheet.Rows[firstRow]);
                int count = 0;
                double total = 0;
                DateTime currentDate = DateTime.MinValue;
                foreach (ExpenseService service in list)
                {
                    int rowIndex = firstRow + count;
                    bool isFirstTime = false;
                    if (currentDate != service.Expense.Date)
                    {
                        sheet.Cells[rowIndex, COL_DATE].Value = service.Expense.Date;
                        currentDate = service.Expense.Date;
                        isFirstTime = true;
                    }

                    IList bookings = GetBooking(service.Expense.Date);
                    int adult = 0;
                    int child = 0;
                    foreach (Booking booking in bookings)
                    {
                        if (booking.Group == service.Group && (service.Group > 0 || isFirstTime) && (booking.Trip.Id == service.Expense.Trip.Id))
                        {
                            adult += booking.Adult;
                            child += booking.Child;
                        }
                    }

                    sheet.Cells[rowIndex, COL_TRIP].Value = service.Expense.Trip.Name;
                    sheet.Cells[rowIndex, COL_ADULT].Value = adult;
                    sheet.Cells[rowIndex, COL_CHILD].Value = child;

                    double sum = 0;
                    for (int ii = 0; ii < services.Count; ii++)
                    {
                        sheet.Cells[rowIndex, firstCol + ii].Value = service.Cost;
                        sum += service.Cost;
                        total += service.Cost;
                    }
                    sheet.Cells[rowIndex, firstCol + services.Count].Value = sum;

                    count++;
                }

                int totalRowIndex = firstRow + count;
                sheet.Cells[totalRowIndex, firstCol + services.Count].Value = total;
            }

            #endregion
        }

        protected IList GetBooking(DateTime date)
        {
            ICriterion criterion = Expression.Eq(Booking.DELETED, false);
            criterion = Expression.And(criterion, Expression.Eq(Booking.STATUS, StatusType.Approved));
            criterion = Module.AddDateExpression(criterion, date);
            int count;
            return Module.BookingGetByCriterion(criterion, null, out count, 0, 0, false, UserIdentity);
        }

        protected void rptServices_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is CostType)
            {
                CostType type = (CostType)e.Item.DataItem;
                if (!type.IsDailyInput && type.IsSupplier)
                {
                    e.Item.Visible = false;
                    return;
                }

                HyperLink hplCostType = e.Item.FindControl("hplCostType") as HyperLink;
                if (type.Id.ToString() == Request.QueryString["costtype"])
                {
                    hplCostType.CssClass += " selected";
                }



                if (hplCostType != null)
                {
                    hplCostType.Text = type.Name;
                    hplCostType.NavigateUrl =
                        string.Format("PayableList.aspx?NodeId={0}&SectionId={1}&from={2}&to={3}&costtype={4}", Node.Id,
                                      Section.Id, Request.QueryString["from"], Request.QueryString["to"], type.Id);
                }
            }

            if (e.Item.ItemType == ListItemType.Header)
            {
                HyperLink hplCostType = e.Item.FindControl("hplCostType") as HyperLink;
                if (Request.QueryString["costtype"] == null && Request.QueryString["cost"] == null)
                {
                    hplCostType.CssClass += " selected";
                }



                if (hplCostType != null)
                {
                    hplCostType.NavigateUrl = // link all;
                        string.Format("PayableList.aspx?NodeId={0}&SectionId={1}&from={2}&to={3}", Node.Id,
                                      Section.Id, Request.QueryString["from"], Request.QueryString["to"]);
                }
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                HyperLink hplCostType = e.Item.FindControl("hplCostType") as HyperLink;
                if (Request.QueryString["cost"] != null)
                {
                    hplCostType.CssClass += " selected";
                }

                if (hplCostType != null)
                {
                    hplCostType.NavigateUrl = // link all daily cost;
                        string.Format("PayableList.aspx?NodeId={0}&SectionId={1}&from={2}&to={3}&cost=bycustomer", Node.Id,
                                      Section.Id, Request.QueryString["from"], Request.QueryString["to"]);
                }
            }
        }

        protected void rptOrganization_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is Organization)
            {
                var organization = (Organization)e.Item.DataItem;
                var hplOrganization = e.Item.FindControl("hplOrganization") as HyperLink;

                if (organization.Id.ToString() == Request.QueryString["orgid"])
                {
                    hplOrganization.CssClass += " selected";
                }


                if (hplOrganization != null)
                {
                    //DateTime from = DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    hplOrganization.Text = organization.Name;
                    if (Request.QueryString["from"] != null)
                    {
                        hplOrganization.NavigateUrl = string.Format(
                            "PayableList.aspx?NodeId={0}&SectionId={1}&from={2}&to={4}&orgid={3}", Node.Id, Section.Id,
                            Request.QueryString["from"], organization.Id, Request.QueryString["to"]);
                    }
                    else
                    {
                        hplOrganization.NavigateUrl = string.Format(
                            "PayableList.aspx?NodeId={0}&SectionId={1}&orgid={2}", Node.Id, Section.Id, organization.Id);
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

                if (Request.QueryString["orgid"] == null && Request.QueryString["tripid"] == null)
                {
                    hplOrganization.CssClass += " selected";
                }

                if (hplOrganization != null)
                {
                    //DateTime date = DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    if (Request.QueryString["from"] != null)
                    {
                        hplOrganization.NavigateUrl = string.Format(
                            "PayableList.aspx?NodeId={0}&SectionId={1}&from={2}&to={3}", Node.Id, Section.Id,
                            Request.QueryString["from"], Request.QueryString["to"]);
                    }
                    else
                    {
                        hplOrganization.NavigateUrl = string.Format(
                            "PayableList.aspx?NodeId={0}&SectionId={1}", Node.Id, Section.Id);
                    }
                }
            }
        }
    }
}
