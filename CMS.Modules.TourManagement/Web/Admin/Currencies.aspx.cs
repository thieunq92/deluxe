using System;
using System.Globalization;
using System.Web.UI.WebControls;
using CMS.Core.Util;
using CMS.Modules.TourManagement.Domain;
using CMS.Modules.TourManagement.Web.UI;
using CMS.ServerControls;
using CMS.Core.Domain;

namespace CMS.Modules.TourManagement.Web.Admin
{
    public partial class Currencies : TourAdminBasePage
    {
        #region -- PRIVATE MEMMBERS --
        private Currency _activeCurrency;

        /// <summary>
        /// Biến ViewState lưu Service hiện tại
        /// </summary>
        private Currency ActiveCurrency
        {
            get
            {
                if (_activeCurrency != null)
                {
                    return _activeCurrency;
                }
                if (ViewState["serviceId"] != null && Convert.ToInt32(ViewState["serviceId"]) > 0)
                {
                    return Module.CurrencyGetById(Convert.ToInt32(ViewState["serviceId"]));
                }
                _activeCurrency = new Currency();
                return _activeCurrency;
            }
            set
            {
                _activeCurrency = value;
                ViewState["serviceId"] = value.Id;
            }
        }
        #endregion

        #region -- PAGE EVENTS --
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = Resources.titleAdminCurrencies;
            if (!IsPostBack)
            {
                repeaterCurrencies.DataSource = Module.CurrencyGetAll();
                repeaterCurrencies.DataBind();
                dropDownListCulture.DataSource = Globalization.GetOrderedCultures();
                dropDownListCulture.DataValueField = "Key";
                dropDownListCulture.DataTextField = "Value";
                dropDownListCulture.DataBind();
                labelFormTitle.Text = Resources.labelNewCurrency;
                btnDelete.Visible = false;
                btnDelete.Enabled = false;
            }
        }
        #endregion

        #region -- CONTROL EVENTS --
        #region -- Pager --
        protected void pagerCurrencies_CacheEmpty(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected void pagerCurrencies_PageChanged(object sender, PageChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion
        #endregion

        protected void repeaterCurrencies_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Currency currency = e.Item.DataItem as Currency;
            if (currency != null)
            {
                #region -- ITEM --
                using (HyperLink hyperLinkCurrencyEdit = e.Item.FindControl("hyperLinkCurrencyEdit") as HyperLink)
                {
                    if (hyperLinkCurrencyEdit!=null)
                    {
                        hyperLinkCurrencyEdit.Text = currency.Name;
                    }
                }

                using (Label labelCultureName = e.Item.FindControl("labelCultureName") as Label)
                {
                    if (labelCultureName!=null)
                    {
                        CultureInfo cultureInfo = new CultureInfo(currency.CultureKey);
                        labelCultureName.Text = string.Format("{0}<br/>{1}", cultureInfo.DisplayName, cultureInfo.NativeName);
                    }
                }

                using (Label label = e.Item.FindControl("labelFormat") as Label)
                {
                    if (label!=null)
                    {
                        //labelFormat.Text = currency.CurrencyFormat.Replace("{0}", "123456");
                        //labelFormat.Text = string.Format("{0:C}", 123456);
                    }
                }
                #endregion
            }
        }

        protected void repeaterCurrencies_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName.ToLower())
            {
                case "edit":
                    #region -- Lấy thông tin dịch vụ
                    ActiveCurrency = Module.CurrencyGetById(Convert.ToInt32(e.CommandArgument));
                    textBoxName.Text = ActiveCurrency.Name;
                    if (ActiveCurrency.Id > 1)
                    {
                        textBoxConversationRate.Text = ActiveCurrency.Rate.ToString("0.#####");
                        textBoxConversationRate.ReadOnly = false;
                    }
                    else
                    {
                        textBoxConversationRate.Text = "1";
                        textBoxConversationRate.ReadOnly = true;
                    }
                    dropDownListCulture.SelectedValue = ActiveCurrency.CultureKey;
                    CultureInfo cultureInfo = new CultureInfo(ActiveCurrency.CultureKey);
                    textBoxSymbol.Text = cultureInfo.NumberFormat.CurrencySymbol;
                    textBoxFormat.Text = string.Format(cultureInfo, "{0:C}", 1234);
                    //TODO: Cho phép tự tạo format mới
                    checkBoxCustomSetting.Checked = false;                    
                    #endregion
                    if (ActiveCurrency.Id > 1)
                    {
                        btnDelete.Visible = true;
                        btnDelete.Enabled = true;
                    }
                    labelFormTitle.Text = ActiveCurrency.Name;
                    break;
                default:
                    break;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            ActiveCurrency.Name = textBoxName.Text;
            ActiveCurrency.CultureKey = dropDownListCulture.SelectedValue;
            ActiveCurrency.Symbol = textBoxSymbol.Text;
            ActiveCurrency.CurrencyFormat = "{0}";
            ActiveCurrency.ModifiedBy = ((User) Page.User.Identity).Id;
            ActiveCurrency.ModifiedDate = DateTime.Now;
            ActiveCurrency.Rate = Convert.ToDouble(textBoxConversationRate.Text);
            // Kiểm tra trong View State
            if (ActiveCurrency.Id > 0)
            {
                Module.Update(ActiveCurrency);
            }
            else
            {
                ActiveCurrency.CreatedBy = ActiveCurrency.ModifiedBy;
                ActiveCurrency.CreatedDate = DateTime.Now;
                Module.Save(ActiveCurrency);
            }
            repeaterCurrencies.DataSource = Module.CurrencyGetAll();
            repeaterCurrencies.DataBind();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Module.Delete(ActiveCurrency);
            btnAddNew_Click(sender, e);
            repeaterCurrencies.DataSource = Module.CurrencyGetAll();
            repeaterCurrencies.DataBind();
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            ActiveCurrency = new Currency();
            textBoxName.Text = string.Empty;
            textBoxSymbol.Text = string.Empty;
            textBoxFormat.Text = string.Empty;
            labelFormTitle.Text = Resources.labelNewCurrency;
        }

        protected void dropDownListCulture_SelectedIndexChanged(object sender, EventArgs e)
        {
            CultureInfo cultureInfo = new CultureInfo(dropDownListCulture.SelectedValue);
            textBoxSymbol.Text = cultureInfo.NumberFormat.CurrencySymbol;
            textBoxFormat.Text = string.Format(cultureInfo, "{0:C}", 1234);
        }
    }
}
