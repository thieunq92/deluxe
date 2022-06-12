using System;
using System.Web.UI.WebControls;
using CMS.Modules.TourManagement.Domain;
using CMS.Modules.TourManagement.Web.UI;

namespace CMS.Modules.TourManagement.Web.Admin
{
    public partial class TourPackagePriceConfig : TourAdminBasePage
    {
        private Tour _tour;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Request.QueryString["TourId"]))
                {
                    return;
                }
                _tour = Module.TourGetById(Convert.ToInt32(Request.QueryString["TourId"]));
                if (!IsPostBack)
                {
                    ddlCurrencies.DataSource = Module.CurrencyGetAll();
                    ddlCurrencies.DataValueField = "Id";
                    ddlCurrencies.DataTextField = "Name";
                    ddlCurrencies.DataBind();

                    labelTourName.Text = "Tour name: " + _tour.Name;
                    labelProvider.Text = "Provided by local operator " + _tour.Provider.Name;
                    int[] numberOfCustomers = new int[8];
                    for (int ii = 1; ii <= 8; ii++)
                    {
                        numberOfCustomers[ii - 1] = ii;
                    }

                    rptCustomers.DataSource = numberOfCustomers;
                    rptCustomers.DataBind();
                    rptPrices.DataSource = numberOfCustomers;
                    rptPrices.DataBind();
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            foreach (RepeaterItem item in rptPrices.Items)
            {
                HiddenField hiddenCustomer = item.FindControl("hiddenCustomer") as HiddenField;
                TextBox textBoxPrice = item.FindControl("textBoxPrice") as TextBox;
                if (hiddenCustomer== null || textBoxPrice==null)
                {
                    return;
                }
                int numberOfCustomer = Convert.ToInt32(hiddenCustomer.Value);
                TourPackagePrice package = Module.TourPackerPriceGetByTourAndCustomer(_tour, numberOfCustomer);
                package.Tour = _tour;
                package.NumberOfCustomers = numberOfCustomer;
                package.NetPrice = Convert.ToInt32(textBoxPrice.Text);
                package.Currency = Module.CurrencyGetById(Convert.ToInt32(ddlCurrencies.SelectedValue));
                Module.SaveOrUpdate(package);
            }
        }

        protected void rptPrices_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            HiddenField hiddenCustomer = e.Item.FindControl("hiddenCustomer") as HiddenField;
            TextBox textBoxPrice = e.Item.FindControl("textBoxPrice") as TextBox;
            if (hiddenCustomer == null || textBoxPrice == null)
            {
                return;
            }
            int numberOfCustomer = Convert.ToInt32(hiddenCustomer.Value);
            TourPackagePrice package = Module.TourPackerPriceGetByTourAndCustomer(_tour, numberOfCustomer);
            if (package.Id > 0)
            {
                textBoxPrice.Text = package.NetPrice.ToString("####");
                ddlCurrencies.SelectedValue = package.Currency.Id.ToString();
            }
        }
    }
}
