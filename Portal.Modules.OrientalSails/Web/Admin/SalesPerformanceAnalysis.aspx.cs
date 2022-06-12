using CMS.Core.Domain;
using log4net;
using Portal.Modules.OrientalSails.BusinessLogic;
using Portal.Modules.OrientalSails.BusinessLogic.Share;
using Portal.Modules.OrientalSails.DataTransferObject;
using Portal.Modules.OrientalSails.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class SalesPerformanceAnalysis : System.Web.UI.Page
    {
        private SalesPerformanceAnalysisBLL _salesPerformanceAnalysisBLL = new SalesPerformanceAnalysisBLL();
        private ILog _log = LogManager.GetLogger(typeof(SalesPerformanceAnalysis));
        private List<TopPartnerAnalysisDTO> _topPartnerMonth = new List<TopPartnerAnalysisDTO>();
        private List<TopPartnerAnalysisDTO> _topPartnerAnalysis = new List<TopPartnerAnalysisDTO>();
        private UserBLL _userBLL = new UserBLL();
        public User Sales
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["salesId"]))
                {
                    return _userBLL.UserGetById(Int32.Parse(Request.QueryString["salesId"]));
                }
                return null;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Sales == null)
            {
                throw new HttpException(404, "Page not found");
            }
            if (!IsPostBack)
            {
                Enumerable.Range(1, 13).ToList().ForEach(x =>
                {
                    ddlMonth.Items.Add(new ListItem() { Text = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(x), Value = x.ToString() });
                });
                ddlMonth.SelectedValue = DateTime.Today.Month.ToString();
                Enumerable.Range(2000, 100).ToList().ForEach(x =>
                {
                    ddlYear.Items.Add(new ListItem() { Text = x.ToString(), Value = x.ToString() });
                });
                ddlYear.SelectedValue = DateTime.Today.Year.ToString();
                Enumerable.Range(1, 13).ToList().ForEach(x =>
                {
                    ddlTopPartnerMonth.Items.Add(new ListItem() { Text = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(x), Value = x.ToString() });
                });
                ddlTopPartnerMonth.SelectedValue = DateTime.Today.Month.ToString();
                Enumerable.Range(2000, 100).ToList().ForEach(x =>
                {
                    ddlTopPartnerYear.Items.Add(new ListItem() { Text = x.ToString(), Value = x.ToString() });
                });
                ddlTopPartnerYear.SelectedValue = DateTime.Today.Year.ToString();
            }
            var month = int.Parse(ddlMonth.SelectedValue);
            var year = int.Parse(ddlYear.SelectedValue);
            var salesId = Sales.Id;
            var generalInfomation = GetGeneralInformation(month, year, salesId);
            var giSameMonthLastYear = generalInfomation.Where(x => x.MONTH == month && x.YEAR == year - 1).FirstOrDefault();
            if (giSameMonthLastYear != null)
            {
                generalInfomation.RemoveAt(0);
                generalInfomation.Add(giSameMonthLastYear);
            }
            rptMonths.DataSource = generalInfomation;
            rptMonths.DataBind();
            rptNoOfPax.DataSource = generalInfomation;
            rptNoOfPax.DataBind();
            rptNoOfBooking.DataSource = generalInfomation;
            rptNoOfBooking.DataBind();
            rptMeetingReport.DataSource = generalInfomation;
            rptMeetingReport.DataBind();
            rptPercentOutOfTotal.DataSource = generalInfomation;
            rptPercentOutOfTotal.DataBind();
            rptRevenueInMonth.DataSource = generalInfomation;
            rptRevenueInMonth.DataBind();
            rptTopPercentage.DataSource = generalInfomation;
            rptTopPercentage.DataBind();
            _topPartnerAnalysis = GetTopPartnerAnalysis(month, year, salesId);
            _topPartnerMonth = _topPartnerAnalysis.GroupBy(tpa => new { tpa.MONTH, tpa.YEAR }).Select(tpa => tpa.FirstOrDefault()).ToList();
            rptTopPartnerMonth.DataSource = _topPartnerMonth;
            rptTopPartnerMonth.DataBind();
            rptTopPartnerMonthSub.DataSource = _topPartnerMonth;
            rptTopPartnerMonthSub.DataBind();
            rptTopPartnerIndex.DataSource = Enumerable.Range(0, 10);
            rptTopPartnerIndex.DataBind();
            var partnerInCharge = GetPartnerInCharge(salesId);
            rptPartnerInCharge.DataSource = partnerInCharge;
            rptPartnerInCharge.DataBind();
            var newPartner = GetNewPartner(salesId, month, year);
            rptNewPartner.DataSource = newPartner;
            rptNewPartner.DataBind();
            var topPartnerMonth = int.Parse(ddlTopPartnerMonth.SelectedValue);
            var topPartnerYear = int.Parse(ddlTopPartnerYear.SelectedValue);
            var topPartner = GetTopPartnerAnalysis(topPartnerMonth, topPartnerYear, salesId).Where(x => x.MONTH == topPartnerMonth && x.YEAR == topPartnerYear);
            rptTopPartner.DataSource = topPartner;
            rptTopPartner.DataBind();
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (_salesPerformanceAnalysisBLL != null)
            {
                _salesPerformanceAnalysisBLL.Dispose();
                _salesPerformanceAnalysisBLL = null;
            }
            if (_userBLL != null)
            {
                _userBLL.Dispose();
                _userBLL = null;
            }
        }

        public List<GeneralInformationDTO> GetGeneralInformation(int month, int year, int salesId)
        {
            try
            {
                var data = _salesPerformanceAnalysisBLL.GetGeneralInformation(month, year, salesId);
                _log.Info("SalesPerformanceAnalysis.GetGeneralInfomation : Lấy dữ liệu thành công");
                return data;
            }
            catch (Exception ex)
            {
                _log.Error("Đã có lỗi xảy ra " + ex.Message);
                throw ex;
            }
        }

        protected void rptMonths_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var dataItem = (GeneralInformationDTO)e.Item.DataItem;
            var ltrMonth = (Literal)e.Item.FindControl("ltrMonth");
            ltrMonth.Text = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(dataItem.MONTH) + "-" + dataItem.YEAR;
        }

        protected void rptNoOfPax_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var dataItem = (GeneralInformationDTO)e.Item.DataItem;
            var ltrNoOfPax = (Literal)e.Item.FindControl("ltrNoOfPax");
            ltrNoOfPax.Text = dataItem.NO_OF_PAX.ToString("#,0.##");
        }

        protected void rptPercentOutOfTotal_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var dataItem = (GeneralInformationDTO)e.Item.DataItem;
            var ltrPercentOutOfTotal = (Literal)e.Item.FindControl("ltrPercentOutOfTotal");
            ltrPercentOutOfTotal.Text = dataItem.PERCENT_OUT_OF_TOTAL.ToString("#,0.##");
        }

        protected void rptRevenueInMonth_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var dataItem = (GeneralInformationDTO)e.Item.DataItem;
            var ltrRevenueInMonth = (Literal)e.Item.FindControl("ltrRevenueInMonth");
            ltrRevenueInMonth.Text = dataItem.REVENUE_IN_MONTH.ToString("#,0.##");
        }

        protected void rptTopPercentage_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var dataItem = (GeneralInformationDTO)e.Item.DataItem;
            var ltrTopPercentage = (Literal)e.Item.FindControl("ltrTopPercentage");
            ltrTopPercentage.Text = dataItem.TOP_PERCENTAGE.ToString("#,0.##");
        }

        public int GetNumberOfPartnerInCharge(int salesId)
        {
            try
            {
                var data = _salesPerformanceAnalysisBLL.GetNumberOfPartnerInCharge(salesId);
                _log.Info("SalesPerformanceAnalysis.GetNumberOfPartnerInCharge : Lấy dữ liệu thành công");
                return data;
            }
            catch (Exception ex)
            {
                _log.Error("Đã có lỗi xảy ra " + ex.Message);
                throw ex;
            }
        }

        public int GetNumberOfMeetingInMonth(int month, int salesId)
        {
            try
            {
                var data = _salesPerformanceAnalysisBLL.GetNumberOfMeetingInMonth(month, salesId);
                _log.Info("SalesPerformanceAnalysis.GetNumberOfMeetingInMonth : Lấy dữ liệu thành công");
                return data;
            }
            catch (Exception ex)
            {
                _log.Error("Đã có lỗi xảy ra " + ex.Message);
                throw ex;
            }

        }

        public List<TopPartnerAnalysisDTO> GetTopPartnerAnalysis(int month, int year, int salesId)
        {
            try
            {
                var data = _salesPerformanceAnalysisBLL.GetTopPartnerAnalysis(month, year, salesId);
                _log.Info("SalesPerformanceAnalysis.GetTopPartnerAnalysis : Lấy dữ liệu thành công");
                return data;
            }
            catch (Exception ex)
            {
                _log.Error("Đã có lỗi xảy ra " + ex.Message);
                throw ex;
            }
        }

        protected void rptTopPartnerMonth_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var dataItem = (TopPartnerAnalysisDTO)e.Item.DataItem;
            var ltrMonth = (Literal)e.Item.FindControl("ltrMonth");
            ltrMonth.Text = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(dataItem.MONTH) + "-" + dataItem.YEAR;
        }

        private int totalNumberOfPax = 0;
        protected void rptTopPartnerData_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (((RepeaterItem)e.Item.Parent.Parent).ItemType == ListItemType.Item || ((RepeaterItem)e.Item.Parent.Parent).ItemType == ListItemType.AlternatingItem)
            {
                var dataItem = (TopPartnerAnalysisDTO)e.Item.DataItem;
                var index = (int)((RepeaterItem)e.Item.Parent.Parent).DataItem;
                var month = dataItem.MONTH;
                var year = dataItem.YEAR;
                var data = _topPartnerAnalysis.Where(tpa => tpa.YEAR == year && tpa.MONTH == month).ToList();
                if (index > data.Count - 1) { return; }
                var ltrName = (Literal)e.Item.FindControl("ltrName");
                var ltrNumberOfPax = (Literal)e.Item.FindControl("ltrNumberOfPax");
                ltrName.Text = "<a href='AgencyView.aspx?NodeId=1&SectionId=15&AgencyId=" + data.ElementAt(index).AGENCY_ID + @"'>" + data.ElementAt(index).AGENCY_NAME + "</a>";
                totalNumberOfPax += data.ElementAt(index).NUMBER_OF_PAX;
                ltrNumberOfPax.Text = data.ElementAt(index).NUMBER_OF_PAX.ToString("#,0.##");
            }
            if (((RepeaterItem)e.Item.Parent.Parent).ItemType == ListItemType.Footer)
            {
                var dataItem = (TopPartnerAnalysisDTO)e.Item.DataItem;
                var month = dataItem.MONTH;
                var year = dataItem.YEAR;
                var data = _topPartnerAnalysis.Where(tpa => tpa.YEAR == year && tpa.MONTH == month).ToList();
                var ltrNumberOfPax = (Literal)e.Item.FindControl("ltrNumberOfPax");
                ltrNumberOfPax.Text = data.Sum(x => x.NUMBER_OF_PAX).ToString("#,0.##");
            }
        }

        protected void rptTopPartnerIndex_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var rptTopPartnerData = (Repeater)e.Item.FindControl("rptTopPartnerData");
            rptTopPartnerData.DataSource = _topPartnerMonth;
            rptTopPartnerData.DataBind();
        }

        protected void rptMostRecentMeeting_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var data = (MostRecentMeetingDTO)e.Item.DataItem;
            var ltrAgencyName = (Literal)e.Item.FindControl("ltrAgencyName");
            var ltrDateMeeting = (Literal)e.Item.FindControl("ltrDateMeeting");
            var ltrAgencyContactName = (Literal)e.Item.FindControl("ltrAgencyContactName");
            var ltrNote = (Literal)e.Item.FindControl("ltrNote");
            ltrAgencyName.Text = data.AGENCY_NAME;
            ltrDateMeeting.Text = data.DATE_MEETING.ToString("dd/MM/yyyy");
            ltrAgencyContactName.Text = data.AGENCYCONTACT_NAME;
            ltrNote.Text = data.NOTE;
        }

        protected void rptTopPartner_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var dataItem = (TopPartnerAnalysisDTO)e.Item.DataItem;
            var ltrAgencyName = (Literal)e.Item.FindControl("ltrAgencyName");
            ltrAgencyName.Text = "<a href='AgencyView.aspx?NodeId=1&SectionId=15&AgencyId=" + dataItem.AGENCY_ID + @"'>" + dataItem.AGENCY_NAME + @"</a>";
            var ltrNumberOfPax = (Literal)e.Item.FindControl("ltrNumberOfPax");
            ltrNumberOfPax.Text = dataItem.NUMBER_OF_PAX.ToString("#,0.##");
        }

        protected void rptNoOfBooking_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var dataItem = (GeneralInformationDTO)e.Item.DataItem;
            var ltrNoOfBooking = (Literal)e.Item.FindControl("ltrNoOfBooking");
            ltrNoOfBooking.Text = dataItem.NO_OF_BOOKING.ToString("#,0.##");
        }

        protected void rptMeetingReport_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var dataItem = (GeneralInformationDTO)e.Item.DataItem;
            var ltrMeetingReport = (Literal)e.Item.FindControl("ltrMeetingReport");
            ltrMeetingReport.Text = dataItem.MEETING_REPORT.ToString("#,0.##");
        }

        protected void rptPartnerInCharge_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var dataItem = (PartnerInChargeDTO)e.Item.DataItem;
            var ltrName = (Literal)e.Item.FindControl("ltrName");
            ltrName.Text = "<a href='AgencyView.aspx?NodeId=1&SectionId=15&AgencyId=" + dataItem.AGENCY_ID + @"'>" + dataItem.AGENCY_NAME + @"</a>";
            var ltrContractStatus = (Literal)e.Item.FindControl("ltrContractStatus");
            switch (dataItem.CONTRACT_STATUS)
            {
                case 0:
                    ltrContractStatus.Text = "No";
                    break;
                case 1:
                    ltrContractStatus.Text = "Pending";
                    break;
                case 2:
                    ltrContractStatus.Text = "Yes";
                    break;
            }
            var ltrRole = (Literal)e.Item.FindControl("ltrRole");
            ltrRole.Text = dataItem.ROLE_NAME;
            var ltrLastBooking = (Literal)e.Item.FindControl("ltrLastBooking");
            ltrLastBooking.Text = dataItem.LAST_BOOKING.HasValue ? dataItem.LAST_BOOKING.Value.ToString("dd/MM/yyyy") : "";
            var ltrLastMeeting = (Literal)e.Item.FindControl("ltrLastMeeting");
            ltrLastMeeting.Text = dataItem.LAST_MEETING.HasValue ? dataItem.LAST_MEETING.Value.ToString("dd/MM/yyyy") : "";
            var ltrDetails = (Literal)e.Item.FindControl("ltrDetails");
            ltrDetails.Text = dataItem.DETAIL;
        }

        public List<PartnerInChargeDTO> GetPartnerInCharge(int salesId)
        {
            try
            {
                var data = _salesPerformanceAnalysisBLL.GetPartnerInCharge(salesId);
                _log.Info("SalesPerformanceAnalysis.GetPartnerInCharge : Lấy dữ liệu thành công");
                return data;
            }
            catch (Exception ex)
            {
                _log.Error("Đã có lỗi xảy ra " + ex.Message);
                throw ex;
            }
        }

        public List<NewPartnerDTO> GetNewPartner(int salesId, int month, int year)
        {
            try
            {
                var data = _salesPerformanceAnalysisBLL.GetNewPartner(salesId, month, year);
                _log.Info("SalesPerformanceAnalysis.GetNewPartner : Lấy dữ liệu thành công");
                return data;
            }
            catch (Exception ex)
            {
                _log.Error("Đã có lỗi xảy ra " + ex.Message);
                throw ex;
            }
        }

        protected void rptNewPartner_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var dataItem = (NewPartnerDTO)e.Item.DataItem;
            var ltrName = (Literal)e.Item.FindControl("ltrName");
            ltrName.Text = "<a href='AgencyView.aspx?NodeId=1&SectionId=15&AgencyId=" + dataItem.AGENCY_ID + @"'>" + dataItem.AGENCY_NAME + @"</a>";
            var ltrMostRecentMeeting = (Literal)e.Item.FindControl("ltrMostRecentMeeting");
            ltrMostRecentMeeting.Text = dataItem.MOST_RECENT_MEETING;
            var ltrLastBooking = (Literal)e.Item.FindControl("ltrLastBooking");
            ltrLastBooking.Text = dataItem.LAST_BOOKING.HasValue ? dataItem.LAST_BOOKING.Value.ToString("dd/MM/yyyy") : "";
        }
    }
}