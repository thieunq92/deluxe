using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.UI;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class ExpensePeriod : SailsAdminBase
    {
        private SailsTrip _cruise;
        private DateTime _date;
        private int _month;
        private int _year;
        private string _header = string.Empty;

        private double _footer = 0;
        private double _total = 0;

        private DateTime _yearDate;

        protected SailsTrip ActiveCruise
        {
            get
            {
                if (_cruise == null && Request.QueryString["cruiseid"] != null)
                {
                    _cruise = Module.TripGetById(Convert.ToInt32(Request.QueryString["cruiseid"]));
                }
                return _cruise;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["cruiseid"] == null)
            {

            }

            if (Request.QueryString["month"] != null)
            {
                _month = Convert.ToInt32(Request.QueryString["month"]);
            }
            else
            {
                _month = DateTime.Today.Month;
            }

            if (Request.QueryString["year"] != null)
            {
                _year = Convert.ToInt32(Request.QueryString["year"]);
            }
            else
            {
                _year = DateTime.Today.Year;
            }

            _date = new DateTime(_year, _month, 1); // Luôn lấy ngày 1 tháng đó
            _yearDate = new DateTime(_year, 1, 1); // Luôn lấy ngày 1/1 năm đó

            if (!IsPostBack)
            {
                rptMonthlyExpenses.DataSource = Module.CostTypeGetMonthly();
                rptMonthlyExpenses.DataBind();

                rptYearlyExpenses.DataSource = Module.CostTypeGetYearly();
                rptYearlyExpenses.DataBind();

                for (int ii = 1; ii <= 12; ii++)
                {
                    ddlMonths.Items.Add(ii.ToString());
                }

                ddlMonths.SelectedIndex = _month - 1;
                txtYear.Text = _year.ToString();

                litCurrent.Text = string.Format("Tháng {0} - {1}", _month, _year);
                int nextMonth = _month + 1;
                int nextYear = _year;
                if (nextMonth > 12)
                {
                    nextMonth -= 12;
                    nextYear += 1;
                }

                int previousMonth = _month - 1;
                int previousYear = _year;
                if (previousMonth <= 0)
                {
                    previousMonth += 12;
                    previousYear -= 1;
                }
                hplNext.NavigateUrl = string.Format(
                    "ExpensePeriod.aspx?NodeId={0}&SectionId={1}&month={2}&year={3}", Node.Id, Section.Id, nextMonth,
                    nextYear);
                hplPrevious.NavigateUrl = string.Format(
                    "ExpensePeriod.aspx?NodeId={0}&SectionId={1}&month={2}&year={3}", Node.Id, Section.Id, previousMonth,
                    previousYear);

                BindCruises();
            }
        }

        protected void BindCruises()
        {
            rptCruises.DataSource = Module.CruiseGetAll();
            rptCruises.DataBind();
        }

        protected void rptMonthlyExpenses_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is CostType)
            {
                CostType type = (CostType)e.Item.DataItem;
                #region -- Xét group --
                if (!string.IsNullOrEmpty(type.GroupName) && _header.ToLower() != type.GroupName.ToLower())
                {
                    if (!string.IsNullOrEmpty(_header))
                    {
                        e.Item.FindControl("plhGroupFooter").Visible = true;
                        TextBox txtFooter = e.Item.FindControl("txtFooter") as TextBox;
                        if (txtFooter != null)
                        {
                            txtFooter.Text = _footer.ToString("#,0.#");
                        }
                    }

                    _header = type.GroupName;
                    e.Item.FindControl("plhGroupHeader").Visible = true;
                    Literal litHeader = e.Item.FindControl("litHeader") as Literal;
                    if (litHeader != null)
                    {
                        litHeader.Text = _header;
                    }
                    _footer = 0;
                }
                #endregion

                SailExpense expense = Module.ExpenseGetByDate(ActiveCruise, _date);

                #region -- Tính và hiển thị --
                if (expense.Id < 0)
                {
                    Module.SaveOrUpdate(expense);
                }
                else
                {
                    IList costs = Module.ExpenseServiceGet(ActiveCruise, _date, _date, null, null, null, null, true, null, 0, "", type);
                    if (costs.Count > 0)
                    {
                        ExpenseService cost = costs[0] as ExpenseService;
                        if (cost != null)
                        {
                            TextBox txtCost = (TextBox)e.Item.FindControl("txtCost");
                            TextBox txtDetail = (TextBox)e.Item.FindControl("txtDetail");
                            HiddenField hiddenId = (HiddenField)e.Item.FindControl("hiddenId");
                            hiddenId.Value = cost.Id.ToString();
                            txtCost.Text = cost.Cost.ToString("#,0.#");
                            _total += cost.Cost;
                            _footer += cost.Cost;
                            txtDetail.Text = cost.Name;
                        }
                    }
                    else
                    {
                        IList lastCosts = Module.ExpenseServiceGet(ActiveCruise, _date.AddMonths(-1), _date.AddMonths(-1), null, null, null, null, true, null, 0, "", type);
                        if (lastCosts.Count > 0)
                        {
                            ExpenseService cost = lastCosts[0] as ExpenseService;
                            if (cost != null)
                            {
                                TextBox txtCost = (TextBox)e.Item.FindControl("txtCost");
                                TextBox txtDetail = (TextBox)e.Item.FindControl("txtDetail");
                                Literal litName = (Literal)e.Item.FindControl("litName");
                                litName.Text = string.Format("{0} ({1})", litName.Text, Resources.textUnsaved);
                                txtCost.Text = cost.Cost.ToString("#,0.#");
                                _total += cost.Cost;
                                _footer += cost.Cost;
                                txtDetail.Text = cost.Name;
                            }
                        }
                    }
                }
                #endregion
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                TextBox txtFooter = e.Item.FindControl("txtFooter") as TextBox;
                if (txtFooter != null)
                {
                    txtFooter.Text = _footer.ToString("#,0.#");
                }
                TextBox txtTotal = e.Item.FindControl("txtTotal") as TextBox;
                if (txtTotal != null)
                {
                    txtTotal.Text = _total.ToString("#,0.#");
                }
            }

        }

        protected void rptYearlyExpenses_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is CostType)
            {
                CostType type = (CostType)e.Item.DataItem;
                SailExpense expense = Module.ExpenseGetByDate(ActiveCruise, _yearDate);

                if (expense.Id < 0)
                {
                    Module.SaveOrUpdate(expense);
                }
                else
                {
                    IList costs = Module.ExpenseServiceGet(ActiveCruise, _yearDate, _yearDate, null, null, null, null, true, null, 0, "", type);
                    if (costs.Count > 0)
                    {
                        ExpenseService cost = costs[0] as ExpenseService;
                        if (cost != null)
                        {
                            TextBox txtCost = (TextBox)e.Item.FindControl("txtCost");
                            HiddenField hiddenId = (HiddenField)e.Item.FindControl("hiddenId");
                            hiddenId.Value = cost.Id.ToString();
                            txtCost.Text = cost.Cost.ToString("#,0.#");
                        }
                    }
                    else
                    {
                        IList lastCosts = Module.ExpenseServiceGet(ActiveCruise, _yearDate.AddYears(-1), _yearDate.AddYears(-1), null, null, null, null, true,
                                                                   null, 0, "", type);
                        if (lastCosts.Count > 0)
                        {
                            ExpenseService cost = lastCosts[0] as ExpenseService;
                            if (cost != null)
                            {
                                TextBox txtCost = (TextBox)e.Item.FindControl("txtCost");
                                Literal litName = (Literal)e.Item.FindControl("litName");
                                litName.Text = litName.Text + " (chưa lưu)";
                                txtCost.Text = cost.Cost.ToString("#,0.#");
                            }
                        }
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            foreach (RepeaterItem item in rptMonthlyExpenses.Items)
            {
                HiddenField hiddenId = (HiddenField)item.FindControl("hiddenId");
                TextBox txtCost = (TextBox)item.FindControl("txtCost");
                TextBox txtDetail = (TextBox)item.FindControl("txtDetail");
                ExpenseService service;
                if (string.IsNullOrEmpty(hiddenId.Value))
                {
                    HiddenField hiddenTypeId = (HiddenField)item.FindControl("hiddenTypeId");
                    CostType type = Module.CostTypeGetById(Convert.ToInt32(hiddenTypeId.Value));
                    service = new ExpenseService();
                    service.Expense = Module.ExpenseGetByDate(ActiveCruise, _date);
                    if (string.IsNullOrEmpty(txtDetail.Text))
                    {
                        service.Name = string.Format("{0} - {1}/{2}", type.Name, _month, _year);
                    }
                    else
                    {
                        service.Name = txtDetail.Text;
                    }
                    service.Supplier = type.DefaultAgency;
                    service.Type = type;
                }
                else
                {
                    service = Module.ExpenseServiceGetById(Convert.ToInt32(hiddenId.Value));
                    service.Name = txtDetail.Text;
                }
                if (string.IsNullOrEmpty(txtCost.Text))
                {
                    service.Cost = 0;
                }
                else
                {
                    service.Cost = Convert.ToDouble(txtCost.Text);
                }
                Module.SaveOrUpdate(service);
            }

            foreach (RepeaterItem item in rptYearlyExpenses.Items)
            {
                HiddenField hiddenId = (HiddenField)item.FindControl("hiddenId");
                TextBox txtCost = (TextBox)item.FindControl("txtCost");
                ExpenseService service;
                if (string.IsNullOrEmpty(hiddenId.Value))
                {
                    HiddenField hiddenTypeId = (HiddenField)item.FindControl("hiddenTypeId");
                    CostType type = Module.CostTypeGetById(Convert.ToInt32(hiddenTypeId.Value));
                    service = new ExpenseService();
                    service.Expense = Module.ExpenseGetByDate(ActiveCruise, _yearDate);
                    service.Name = string.Format("{0} - {1}", type.Name, _year);
                    service.Supplier = type.DefaultAgency;
                    service.Type = type;
                }
                else
                {
                    service = Module.ExpenseServiceGetById(Convert.ToInt32(hiddenId.Value));
                }
                if (string.IsNullOrEmpty(txtCost.Text))
                {
                    service.Cost = 0;
                }
                else
                {
                    service.Cost = Convert.ToDouble(txtCost.Text);
                }
                Module.SaveOrUpdate(service);
            }
        }

        protected void rptCruises_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is Cruise)
            {
                Cruise cruise = (Cruise)e.Item.DataItem;
                HtmlGenericControl liMenu = e.Item.FindControl("liMenu") as HtmlGenericControl;
                if (liMenu != null)
                {
                    if (cruise.Id.ToString() == Request.QueryString["cruiseid"])
                    {
                        liMenu.Attributes.Add("class", "selected");
                    }
                }

                HyperLink hplCruises = e.Item.FindControl("hplCruises") as HyperLink;
                if (hplCruises != null)
                {
                    hplCruises.Text = cruise.Name;
                    hplCruises.NavigateUrl = string.Format(
                        "ExpensePeriod.aspx?NodeId={0}&SectionId={1}&cruiseid={2}&month={3}&year={4}", Node.Id, Section.Id,
                        cruise.Id, _month, _year);
                }
            }
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            int month = ddlMonths.SelectedIndex + 1;
            int year = Convert.ToInt32(txtYear.Text);
            PageRedirect(string.Format("ExpensePeriod.aspx?NodeId={0}&SectionId={1}&month={2}&year={3}", Node.Id, Section.Id, month, year));
        }
    }
}