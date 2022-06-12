using System;
using System.Collections;
using System.Web.UI.WebControls;
using CMS.Core.Util;
using CMS.Modules.TourManagement.Domain;
using CMS.Web.UI;
using CMS.Web.Util;

namespace CMS.Modules.TourManagement.Web
{
    public partial class TourSearchInput : BaseModuleControl
    {
        private TourSearchInputModule _module;

        protected void Page_Load(object sender, EventArgs e)
        {
            _module = Module as TourSearchInputModule;
            if (!IsPostBack)
            {
                BindTourTypes();
                BindTourRegion();
            }
        }

        private void BindTourTypes()
        {
            ddlTourTypes.Items.Clear();
            ddlTourTypes.Items.Add(" -- Tour type --");
            IList roots = _module.TourTypeGetAllRoot();
            foreach (TourType region in roots)
            {
                ddlTourTypes.Items.Add(new ListItem(region.Name, region.Id.ToString()));
                foreach (TourType child in region.Children)
                {
                    ddlTourTypes.Items.Add(new ListItem(" |-- " + child.Name, child.Id.ToString()));
                }
            }
        }

        protected void BindTourRegion()
        {
            ddlTourRegions.Items.Clear();
            ddlTourRegions.Items.Add(" -- Region --");
            IList roots = _module.TourRegionGetAllRoot();
            foreach (TourRegion region in roots)
            {
                ddlTourRegions.Items.Add(new ListItem(region.Name, region.Id.ToString()));
                foreach (TourRegion child in region.Children)
                {
                    ddlTourRegions.Items.Add(new ListItem(" |-- " + child.Name, child.Id.ToString()));
                    foreach (TourRegion gchild in child.Children)
                    {
                        ddlTourRegions.Items.Add(new ListItem(Text.Padding(5) + "|-- " + gchild.Name, gchild.Id.ToString()));
                    }
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string query = string.Empty;
            if (ddlTourTypes.SelectedIndex > 0)
            {
                query += "&Type=" + ddlTourTypes.SelectedValue;
            }

            if (ddlTourRegions.SelectedIndex > 0)
            {
                query += "&Region=" + ddlTourRegions.SelectedValue;
            }

            if (!string.IsNullOrEmpty(textBoxName.Text))
            {
                query += "&Name=" + textBoxName.Text;
            }

            if (!string.IsNullOrEmpty(textBoxNumberOfDays.Text))
            {
                int numberOfDays;
                if (Int32.TryParse(textBoxNumberOfDays.Text,out numberOfDays))
                {
                    query += string.Format("&TimeLt={0}&TimeGt={1}", numberOfDays + 1, numberOfDays - 1);
                }
            }

            if (string.IsNullOrEmpty(query))
            {
                PageEngine.PageRedirect(UrlHelper.GetUrlFromSection(_module.Section));
                return;
            }

            query = query.Substring(1);
            query = "?" + query;
            PageEngine.PageRedirect(UrlHelper.GetUrlFromSection(_module.Section)+query);
        }

        protected void LoadFromQuery()
        {
            if (Request.QueryString["Region"]!=null)
            {
                ddlTourRegions.SelectedValue = Request.QueryString["Region"];
            }
            if (Request.QueryString["Type"] != null)
            {
                ddlTourTypes.SelectedValue = Request.QueryString["Type"];
            }
            if (Request.QueryString["Name"]!=null)
            {
                textBoxName.Text = Request.QueryString["Name"];
            }
        }
    }
}