using CMS.ServerControls;
using NHibernate;
using NHibernate.Criterion;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class TripManager : SailsAdminBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlStatus.Items.Add(new ListItem("Unpay", "unpay"));
                ddlStatus.Items.Add(new ListItem("Unpay after one month", "unpay after one month"));
                ddlStatus.Items.Add(new ListItem("Paid", "paid"));
                if (Request.QueryString["from"] == null)
                {
                    txtFrom.Text = DateTime.Today.AddDays(-DateTime.Today.Day + 1).ToString("dd/MM/yyyy");
                }

                if (Request.QueryString["to"] == null)
                {
                    txtTo.Text = DateTime.Today.AddDays(-DateTime.Today.Day + 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy");
                }
                LoadData();
                GetDataSource();
            }

        }


        public void LoadData()
        {
            if (Request.QueryString["from"] != null)
            {
                txtFrom.Text = Request.QueryString["from"];
            }

            if (Request.QueryString["to"] != null)
            {
                txtTo.Text = Request.QueryString["to"];
            }

            if (Request.QueryString["tripcode"] != null)
            {
                txtTripCode.Text = Request.QueryString["tripcode"];
            }

            if (Request.QueryString["status"] != null)
            {
                ddlStatus.Items.FindByValue(Request.QueryString["status"]).Selected = true;
            }

        }

        protected void pagerTripManager_PageChanged(object sender, PageChangedEventArgs e)
        {
            GetDataSource();
        }

        protected void GetDataSource()
        {
            var agencyId = Convert.ToInt32(Request.QueryString["agencyid"]);
            ISession session = Module.CommonDao.OpenSession();
            session.FlushMode = FlushMode.Commit;
            ICriteria criteria = session.CreateCriteria(typeof(ExpenseService));
            criteria.CreateAlias("Supplier", "supplier");
            criteria.CreateAlias("Expense", "expense");
            criteria.Add(Expression.Eq("supplier.Id", agencyId));

            try
            {
                var from = DateTime.ParseExact(txtFrom.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var to = DateTime.ParseExact(txtTo.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                criteria.Add(Expression.And(Expression.Ge("expense.Date", from), Expression.Le("expense.Date", to)));
            }
            catch { }

            criteria.AddOrder(new Order("expense.Date", false));
            var expenseServices = criteria.List<ExpenseService>();

            var expenseServicesHaveTripCodeToSearch = new List<ExpenseService>();
            if (!String.IsNullOrEmpty(txtTripCode.Text))
            {
                var tripCodeToSearch = txtTripCode.Text;
                foreach (ExpenseService expenseService in expenseServices)
                {
                    var expenseServiceTripCode = string.Format("{0}{1}-{2:00}", expenseService.Expense.Trip.TripCode, expenseService.Expense.Date.ToString("ddMMyy"), expenseService.Group);
                    var isExpenseServiceTripCodeContainTripCodeToSearch = expenseServiceTripCode.IndexOf(tripCodeToSearch, StringComparison.InvariantCultureIgnoreCase) >= 0;
                    if (isExpenseServiceTripCodeContainTripCodeToSearch)
                    {
                        expenseServicesHaveTripCodeToSearch.Add(expenseService);
                    }
                }
            }
            if (expenseServicesHaveTripCodeToSearch.Count > 0)
            {
                expenseServices = expenseServicesHaveTripCodeToSearch;
            }

            var status = ddlStatus.SelectedValue;
            var expenseServicesStatusFilter = new List<ExpenseService>();
            foreach (ExpenseService expenseService in expenseServices)
            {
                if (status == "paid")
                {
                    if (expenseService.PaidDate != null)
                    {
                        expenseServicesStatusFilter.Add(expenseService);
                    }
                }

                if (status == "unpay")
                {
                    if (expenseService.PaidDate == null)
                    {
                        expenseServicesStatusFilter.Add(expenseService);
                    }
                }

                if (status == "unpay after one month")
                {
                    if (expenseService.PaidDate == null)
                    {
                        var unpayDate = DateTime.Now.Subtract(expenseService.Expense.Date).Days;
                        if (unpayDate > 30)
                        {
                            expenseServicesStatusFilter.Add(expenseService);
                        }
                    }
                }
            }
            expenseServices = expenseServicesStatusFilter;

            rptTripManager.DataSource = expenseServices;
            pagerTripManager.VirtualItemCount = expenseServices.Count;
            rptTripManager.DataBind();
        }

        protected void rptTripManager_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var expenseService = (ExpenseService)e.Item.DataItem;
            var ltrIndex = (Literal)e.Item.FindControl("ltrIndex");
            ltrIndex.Text = (e.Item.ItemIndex + pagerTripManager.PageSize * pagerTripManager.CurrentPageIndex + 1).ToString();

            var ltrDate = (Literal)e.Item.FindControl("ltrDate");
            ltrDate.Text = expenseService.Expense.Date.ToString("dd/MM/yyyy");

            var ltrTripCode = (Literal)e.Item.FindControl("ltrTripCode");
            ltrTripCode.Text = string.Format("{0}{1}-{2:00}", expenseService.Expense.Trip.TripCode, expenseService.Expense.Date.ToString("ddMMyy"), expenseService.Group);

            var ltrStatus = (Literal)e.Item.FindControl("ltrStatus");
            var status = "";
            var isPay = false;
            if (expenseService.PaidDate != null)
            {
                isPay = true;
                status = "Paid";
            }
            else
            {
                isPay = false;
            }

            if (!isPay)
            {
                var unpayDate = DateTime.Now.Subtract(expenseService.Expense.Date).Days;
                var isUnpayAfterOneMonth = false;

                if (unpayDate < 30)
                    isUnpayAfterOneMonth = false;
                else
                    isUnpayAfterOneMonth = true;

                if (isUnpayAfterOneMonth)
                    status = "Unpay after one month";
                else
                    status = "Unpay";
            }

            ltrStatus.Text = status;

            var hplPay = (HyperLink)e.Item.FindControl("hplPay");
            hplPay.Text = "Pay";
            hplPay.NavigateUrl = "PayableList.aspx?NodeId=1&SectionId=15&expenseserviceid=" + expenseService.Id;

            if (status == "Paid")
                hplPay.Visible = false;
        }

        protected void btnDisplay_Click(object sender, EventArgs e)
        {
            string query = string.Empty;

            query += "&status=" + ddlStatus.SelectedValue;
            query += "&from=" + txtFrom.Text;
            query += "&to=" + txtTo.Text;

            if (Request.QueryString["agencyid"] != null)
            {
                query += "&agencyid=" + Request.QueryString["agencyid"];
            }

            query += "&tripcode=" + txtTripCode.Text;

            PageRedirect(string.Format("TripManager.aspx?NodeId={0}&SectionId={1}{2}", Node.Id, Section.Id, query));
        }
    }
}