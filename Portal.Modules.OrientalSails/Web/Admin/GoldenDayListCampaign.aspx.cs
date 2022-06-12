using Aspose.Words;
using Aspose.Words.Tables;
using Portal.Modules.OrientalSails.BusinessLogic;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.Admin.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class GoldenDayListCampaign : System.Web.UI.Page
    {
        private GoldenDayListCampaignBLL goldenDayListCampaignBLL;
        public GoldenDayListCampaignBLL GoldenDayListCampaignBLL
        {
            get
            {
                if (goldenDayListCampaignBLL == null)
                {
                    goldenDayListCampaignBLL = new GoldenDayListCampaignBLL();
                }
                return goldenDayListCampaignBLL;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int count = 0;
                rptCampaign.DataSource = GoldenDayListCampaignBLL.CampaignGetAllPaged(pagerCampaign.PageSize,
                    pagerCampaign.CurrentPageIndex, out count);
                pagerCampaign.AllowCustomPaging = true;
                pagerCampaign.VirtualItemCount = count;
                rptCampaign.DataBind();

            }
        }
        protected void Page_Unload(object sender, EventArgs e)
        {
            if (goldenDayListCampaignBLL != null)
            {
                goldenDayListCampaignBLL.Dispose();
                goldenDayListCampaignBLL = null;
            }
        }
        public string GetDateApplied(Campaign campaign)
        {
            var dates = campaign.Policies.SelectMany(
                p => p.GoldenDays.Select(gd =>
                    "<a style='" + (gd.Policies.Where(p1 => p1.Campaign == null).Count() > 0 ? "color:#6a0dad" : "color:#333333") + "' href='' data-toggle='tooltip' title ='"
                    + string.Join("<br/>", gd.Policies.GroupBy(p1 => p1.Trip.Id).Select(grp => grp.Count() > 1 ? grp.Where(p1 => p1.Campaign == null).First() : grp.First())
                    .Select(x => "Trip: " + x.Trip.Name + " | Adult: " + x.Adult.ToString("#,0.##") + "₫ | Child: " + x.Child.ToString("#,0.##") + "₫").ToArray())
                    + "'>" + gd.Date.ToString("dd/MM/yyyy")
                    + "</a>" + "&nbsp&nbsp<a href='GoldenDayCreateCampaign.aspx?gdi=" + gd.Id + "&ci=" + campaign.Id + "'>Edit</a>"))
                    .Distinct().ToArray();
            return String.Join("<br/>", dates);
        }
        public IEnumerable<Booking> GetNewBookings(Campaign campaign)
        {
            return GoldenDayListCampaignBLL.BookingGetAllNewBookingsByCampaign(campaign);
        }
        public string GetNoOfNewBooking(Campaign campaign)
        {
            var bookings = GoldenDayListCampaignBLL.BookingGetAllNewBookingsByCampaign(campaign);
            return bookings.Count().ToString();
        }

        protected void rptCampaign_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                var campaignId = 0;
                try
                {
                    campaignId = Convert.ToInt32(e.CommandArgument);
                }
                catch { }
                var campaign = GoldenDayListCampaignBLL.CampaginGetById(campaignId);
                GoldenDayListCampaignBLL.CampaignDelete(campaign);
                Response.Redirect(Request.RawUrl);
            }
        }

        protected void rptBooking_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }

        protected void rptCampaign_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var campaign = (Campaign)e.Item.DataItem;
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var lbtView = (LinkButton)e.Item.FindControl("lbtView");
                lbtView.OnClientClick = "$('#" + hidCampaignId.ClientID + "').val(" + campaign.Id + ");$get('" + btnTrigger.ClientID + "').click();return false;";
            }
        }

        protected void btnTrigger_Click(object sender, EventArgs e)
        {
            plhTableBooking.Visible = true;
            var campaignId = Int32.Parse(hidCampaignId.Value);
            var campaign = GoldenDayListCampaignBLL.CampaginGetById(campaignId);
            var bookings = new List<Booking>();
            bookings = GetNewBookings(campaign).ToList();
            rptBooking.DataSource = bookings;
            rptBooking.DataBind();
        }
    }
}