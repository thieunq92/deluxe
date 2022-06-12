using CMS.Core.Domain;
using Portal.Modules.OrientalSails.BusinessLogic;
using Portal.Modules.OrientalSails.BusinessLogic.Share;
using Portal.Modules.OrientalSails.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class GoldenDayCreateCampaign : System.Web.UI.Page
    {
        private GoldenDayCreateCampaignBLL goldenDayCreateCampaignBLL;
        private UserBLL userBLL;
        public GoldenDayCreateCampaignBLL GoldenDayCreateCampaignBLL
        {
            get
            {
                if (goldenDayCreateCampaignBLL == null) goldenDayCreateCampaignBLL = new GoldenDayCreateCampaignBLL();
                return goldenDayCreateCampaignBLL;
            }
        }
        public UserBLL UserBLL
        {
            get
            {
                if (userBLL == null)
                    userBLL = new UserBLL();
                return userBLL;
            }
        }
        public User CurrentUser
        {
            get
            {
                return UserBLL.UserGetCurrent();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlMonth.Items.AddRange(Enumerable.Range(1, 12).Select(x => new ListItem() { Text = DateTimeFormatInfo.CurrentInfo.GetMonthName(x), Value = x.ToString() }).ToArray());
                ddlYear.Items.AddRange(Enumerable.Range(DateTime.Now.Year, 100).Select(x => new ListItem() { Text = x.ToString(), Value = x.ToString() }).ToArray());
                ddlMonth.SelectedValue = (DateTime.Today.Month + 1).ToString();
                var regions = GoldenDayCreateCampaignBLL.RegionGetAllByUser(CurrentUser);
                var trips = new List<SailsTrip>();
                foreach (var region in regions)
                {
                    trips.AddRange(GoldenDayCreateCampaignBLL.TripGetAllByRegion(region));
                }
                ddlTrip.DataSource = trips;
                ddlTrip.DataTextField = "Name";
                ddlTrip.DataValueField = "Id";
                ddlTrip.DataBind();
                ddlTripGoldenDay.DataSource = trips;
                ddlTripGoldenDay.DataTextField = "Name";
                ddlTripGoldenDay.DataValueField = "Id";
                ddlTripGoldenDay.DataBind();
            }
        }
        protected void Page_Unload(object sender, EventArgs e)
        {
            if (goldenDayCreateCampaignBLL != null)
            {
                goldenDayCreateCampaignBLL.Dispose();
                goldenDayCreateCampaignBLL = null;
            }
        }
        public void LoadTotalPaxByDate()
        {
            var month = Int32.Parse(Request.Params["month"]);
            var year = Int32.Parse(Request.Params["year"]);
            var from = new DateTime(year, month, 1);
            var to = from.AddMonths(1).AddDays(-1);
            var dateRange = new List<DateTime>();
            var current = from;
            while (current <= to)
            {
                dateRange.Add(current);
                current = current.AddDays(1);
            }
            rptTotalPaxByDate.DataSource = dateRange;
            rptTotalPaxByDate.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            LoadTotalPaxByDate();
        }

        protected void rptTotalPaxByDate_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var date = (DateTime)e.Item.DataItem;
            var totalPax = 0;
            totalPax = GoldenDayCreateCampaignBLL.CustomerGetCountingByDate(date);
            var ltrTotalPax = e.Item.FindControl("ltrTotalPax") as Literal;
            ltrTotalPax.Text = totalPax.ToString("#,0.##");
        }
    }
}