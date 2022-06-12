using System;
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
using CMS.Modules.TourManagement.Web.UI;

namespace CMS.Modules.TourManagement.Web.Admin
{
    public partial class OtherExpenses : SelectorControl
    {
        private TourManagementModule _module;
        private Tour _tour;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["TourId"] != null && Convert.ToInt32(Request.QueryString["TourId"]) > 0)
            {
                _tour = _module.TourGetById(Convert.ToInt32(Request.QueryString["TourId"]));
                if (!IsPostBack)
                {
                    LocalizeControls();
                    GetAddExpenseList();
                    IList numberOfCustomers = new ArrayList();
                    for (int ii = 1; ii <= 8; ii++ )
                    {
                        numberOfCustomers.Add(ii);
                    }

                    rptExpensePrice.DataSource = numberOfCustomers;

                    IList dayRange = new ArrayList();
                    for (int ii = 1; ii <= _tour.NumberOfDay; ii++ )
                    {
                        dayRange.Add(ii);
                    }

                    ddlDayFrom.DataSource = dayRange;
                    ddlDayFrom.DataBind();
                    ddlDayTo.DataSource = dayRange;
                    ddlDayTo.DataBind();

                    rptAddedExpenses.DataBind();
                    rptExpensePrice.DataBind();
                }
            }
        }

        private void GetAddExpenseList()
        {
            rptAddedExpenses.DataSource = _module.ExpenseGetByTour(_tour);
        }

        public override ModuleBase Module
        {
            get { return _module; }
            set { _module = value as TourManagementModule; }
        }

        protected void rptAddedExpenses_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is TourOtherExpense)
            {
                
            }
        }

        protected void rptAddedExpenses_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected void buttonAdd_Click(object sender, EventArgs e)
        {
            TourOtherExpense expense = new TourOtherExpense();
            expense.Tour = _tour;
            expense.Name = txtBoxName.Text;
            _module.SaveOrUpdate(expense);

            foreach (RepeaterItem item in rptExpensePrice.Items)
            {
                TourOtherExpensePrice price = new TourOtherExpensePrice();
                price.Expense = expense;
                HiddenField hiddenNumberOfCustomer = item.FindControl("hiddenNumberOfCustomer") as HiddenField;
                TextBox textBoxPrice = item.FindControl("txtPrice") as TextBox;
                if (hiddenNumberOfCustomer==null || textBoxPrice == null)
                {
                    continue;
                }
                price.NumberOfCustomers = Convert.ToInt32(hiddenNumberOfCustomer.Value);
                price.NetPrice = Convert.ToDecimal(textBoxPrice.Text);
                price.Currency = _module.CurrencyGetById(1);
                _module.SaveOrUpdate(price);
            }

            GetAddExpenseList();
            rptAddedExpenses.DataBind();
        }
    }
}