using Portal.Modules.OrientalSails.BusinessLogic;
using Portal.Modules.OrientalSails.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class ExpenseHistory : System.Web.UI.Page
    {
        private ExpenseHistoryBLL expenseHistoryBLL;
        public ExpenseHistoryBLL ExpenseHistoryBLL
        {
            get
            {
                if (expenseHistoryBLL == null)
                {
                    expenseHistoryBLL = new ExpenseHistoryBLL();
                }
                return expenseHistoryBLL;
            }
        }
        public ExpenseService Expense
        {
            get
            {
                var expenseId = -1;
                try
                {
                    expenseId = Int32.Parse(Request.QueryString["ei"]);
                }
                catch { }
                ExpenseService expense = null;
                try
                {
                    expense = ExpenseHistoryBLL.ExpenseGetById(expenseId);
                }
                catch
                {
                }
                return expense;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var listExpenseHistory = new List<Domain.ExpenseHistory>();
            if (Expense != null)
            {
                listExpenseHistory = ExpenseHistoryBLL.ExpenseHistoryGetAllByCriterion(Expense).ToList();
            }
            if (!IsPostBack)
            {
                rptSupplierHistory.DataSource = listExpenseHistory.Where(x => x.ColumnName == "SupplierId").ToList();
                rptSupplierHistory.DataBind();
                rptNameHistory.DataSource = listExpenseHistory.Where(x => x.ColumnName == "Name").ToList();
                rptNameHistory.DataBind();
                rptPhoneHistory.DataSource = listExpenseHistory.Where(x => x.ColumnName == "Phone").ToList();
                rptPhoneHistory.DataBind();
                rptCostHistory.DataSource = listExpenseHistory.Where(x => x.ColumnName == "Cost").ToList();
                rptCostHistory.DataBind();
            }
        }
        protected void Page_Unload(object sender, EventArgs e)
        {
            if (expenseHistoryBLL != null)
            {
                expenseHistoryBLL.Dispose();
                expenseHistoryBLL = null;
            }
        }
        public string GetSupplier(string supplierId)
        {
            var output = "";
            var intSupplierId = -1;
            try
            {
                intSupplierId = Int32.Parse(supplierId);
            }
            catch { }
            var supplier = ExpenseHistoryBLL.SupplierGetById(intSupplierId);
            if (supplier != null && supplier.Id > 0)
            {
                output += supplier.Name;
            }
            return output;
        }
        public string GetCost(string cost)
        {
            var output = "";
            var doubleCost = 0.0;
            try
            {
                doubleCost = Double.Parse(cost);
            }
            catch { }
            output += doubleCost.ToString("#,##0.##") + "₫";
            return output;
        }
    }
}