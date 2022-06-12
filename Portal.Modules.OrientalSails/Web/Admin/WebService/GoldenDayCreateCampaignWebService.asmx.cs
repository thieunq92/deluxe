using CMS.Core.Domain;
using Newtonsoft.Json;
using Portal.Modules.OrientalSails.BusinessLogic;
using Portal.Modules.OrientalSails.BusinessLogic.Share;
using Portal.Modules.OrientalSails.DataTransferObject;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Portal.Modules.OrientalSails.Web.Admin.WebService
{
    /// <summary>
    /// Summary description for GoldenDayCreateCampaign
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class GoldenDayCreateCampaignWebService : System.Web.Services.WebService
    {
        private GoldenDayCreateCampaignBLL goldenDayCreateCampaignBLL;
        private UserBLL userBLL;
        public UserBLL UserBLL
        {
            get
            {
                if (userBLL == null)
                    userBLL = new UserBLL();
                return userBLL;
            }
        }
        public GoldenDayCreateCampaignBLL GoldenDayCreateCampaignBLL
        {
            get
            {
                if (goldenDayCreateCampaignBLL == null)
                {
                    goldenDayCreateCampaignBLL = new GoldenDayCreateCampaignBLL();
                }
                return goldenDayCreateCampaignBLL;
            }
        }
        [WebMethod(MessageName = "CreateCampaign")]
        public string CampaignSaveOrUpdate(int month, int year)
        {
            var campaign = GoldenDayCreateCampaignBLL.CampaignGetByMonthAndYear(month, year);
            if (campaign == null || campaign.Id == 0) campaign = new Campaign() { CreatedBy = UserBLL.UserGetCurrent() };
            campaign.Month = month;
            campaign.Year = year;
            GoldenDayCreateCampaignBLL.CampaignSaveOrUpdate(campaign);
            return CampaignGetById(campaign.Id);
        }
        [WebMethod]
        public string CampaignSaveOrUpdate(CampaignDTO campaignDTO)
        {
            campaignDTO.Dates = campaignDTO.DateAsStrings.Select(das => DateTime.ParseExact(das, "dd/MM/yyyy", CultureInfo.InvariantCulture)).ToList();
            var campaign = GoldenDayCreateCampaignBLL.CampaignGetById(campaignDTO.Id);
            if (campaign == null || campaign.Id == 0) return "";
            var goldenDaysDTO = new List<GoldenDay>();
            for (var i = 0; i < campaignDTO.Dates.Count; i++)
            {
                var goldenDayDTO = new GoldenDay()
                {
                    Date = campaignDTO.Dates.ElementAt(i),
                };
                goldenDaysDTO.Add(goldenDayDTO);
            }
            //Xóa policies
            foreach (var policy in campaign.Policies)
            {
                if (!campaignDTO.Policies.Any(p => p.Id == policy.Id))
                {
                    var policyDb = GoldenDayCreateCampaignBLL.PolicyGetById(policy.Id);
                    policyDb.GoldenDays.Clear();
                    GoldenDayCreateCampaignBLL.PolicySaveOrUpdate(policyDb);
                    GoldenDayCreateCampaignBLL.PolicyDelete(policyDb);
                }
            }
            foreach (var policyDTO in campaignDTO.Policies)
            {
                var policy = campaign.Policies.ToList().Find(p => p.Id == policyDTO.Id);
                if (policy == null || policy.Id == 0)
                {
                    policy = new Policy();
                    GoldenDayCreateCampaignBLL.PolicySaveOrUpdate(policy);
                }

                policy.Adult = policyDTO.Adult;
                policy.Child = policyDTO.Child;
                policy.Trip = GoldenDayCreateCampaignBLL.TripGetById(policyDTO.Trip.Id);
                policy.Campaign = campaign;
                //Xóa golden days
                for (var i = 0; i < policy.GoldenDays.Count - goldenDaysDTO.Count; i++)
                {
                    var goldenDayId = policy.GoldenDays.ElementAt(i).Id;
                    var goldenDay = GoldenDayCreateCampaignBLL.GoldenDayGetById(goldenDayId);
                    goldenDay.Policies.Clear();
                    GoldenDayCreateCampaignBLL.GoldenDaySaveOrUpdate(goldenDay);
                    GoldenDayCreateCampaignBLL.GoldenDayDelete(goldenDay);
                    policy.GoldenDays.Remove(policy.GoldenDays.ElementAt(i));
                }
                //--
                for (var i = 0; i < goldenDaysDTO.Count; i++)
                {
                    //Cập nhật lại date trong golden day của policy nếu số ngày người dùng chọn ít hơn số golden day của policy
                    if (i < policy.GoldenDays.Count)
                    {
                        var goldenDayId = policy.GoldenDays.ElementAt(i).Id;
                        var goldenDay = GoldenDayCreateCampaignBLL.GoldenDayGetById(goldenDayId);
                        goldenDay.Date = goldenDaysDTO[i].Date;
                        GoldenDayCreateCampaignBLL.GoldenDaySaveOrUpdate(goldenDay);
                        continue;
                    }
                    //Nếu golden day được chọn nhiều hơn golden day của policy, tạo mới golden day và thêm vào policy
                    //Kiểm tra golden day được chọn đã có trong db chưa
                    var gd = GoldenDayCreateCampaignBLL.GoldenDayGetByDate(goldenDaysDTO[i].Date);
                    //Kiểm tra nếu golden day chưa có thì tạo mới
                    if (gd == null || gd.Id == 0)
                    {
                        gd = new GoldenDay();
                    }
                    gd.Date = goldenDaysDTO[i].Date;
                    GoldenDayCreateCampaignBLL.GoldenDaySaveOrUpdate(gd);
                    //--
                    //Gắn golden day vào policy và lưu policy
                    var p = GoldenDayCreateCampaignBLL.PolicyGetById(policy.Id);
                    p.GoldenDays.Add(gd);
                    GoldenDayCreateCampaignBLL.PolicySaveOrUpdate(p);
                    //--
                }

                //Gắn policy vào campaign tự động lưu
                if (policy.Id == 0) { campaign.Policies.Add(policy); }
            }

            GoldenDayCreateCampaignBLL.CampaignSaveOrUpdate(campaign);
            Dispose();
            return CampaignGetById(campaign.Id);
        }

        [WebMethod]
        public string CampaignGetById(int campaignId)
        {
            var campaign = GoldenDayCreateCampaignBLL.CampaignGetById(campaignId);
            if (campaign == null || campaign.Id == 0) return "";
            var campaignDTO = new CampaignDTO()
            {
                Id = campaign.Id,
                CreatedDate = campaign.CreatedDate,
                Month = campaign.Month,
                Year = campaign.Year,
                Name = campaign.Name,
                Policies = PolicyDTO.GetPoliciesDTO(campaign.Policies).ToList(),
                Dates = campaign.Policies.SelectMany(p => p.GoldenDays.Select(gd => gd.Date)).Distinct().ToList(),
            };
            campaignDTO.DateAsStrings = campaignDTO.Dates.Select(d => d.ToString("dd/MM/yyyy")).ToList();
            return JsonConvert.SerializeObject(campaignDTO);
        }
        [WebMethod]
        public string GoldenDayGetById(int goldenDayId)
        {
            var goldenDay = GoldenDayCreateCampaignBLL.GoldenDayGetById(goldenDayId);
            if (goldenDay == null || goldenDay.Id == 0) return "";
            var goldenDayDTO = new GoldenDayDTO()
            {
                Id = goldenDay.Id,
                Date = goldenDay.Date,
            };
            var policiesDTO = new List<PolicyDTO>();
            var policies = goldenDay.Policies.GroupBy(p => p.Trip.Id).Select(grp => grp.Count() > 1 ? grp.Where(p => p.Campaign == null).First() : grp.First());
            foreach (var policy in policies)
            {
                
                var policyDTO = new PolicyDTO()
                {
                    Id = policy.Id,
                    Adult = policy.Adult,
                    Child = policy.Child,
                    Trip = new TripDTO
                    {
                        Id = policy.Trip.Id,
                        Name = policy.Trip.Name
                    },
                };
                policiesDTO.Add(policyDTO);
            }
            return JsonConvert.SerializeObject(new { goldenDayDTO = goldenDayDTO, policiesDTO = policiesDTO });
        }
        [WebMethod]
        public string GoldenDaySaveOrUpdate(GoldenDayDTO goldenDayDTO, List<PolicyDTO> policiesDTO)
        {
            var goldenDay = GoldenDayCreateCampaignBLL.GoldenDayGetById(goldenDayDTO.Id);
            foreach (var policyDTO in policiesDTO)
            {
                var policy = new Policy()
                {
                    Adult = policyDTO.Adult,
                    Child = policyDTO.Child,
                    Trip = GoldenDayCreateCampaignBLL.TripGetById(policyDTO.Trip.Id)
                };
                policy.GoldenDays.Add(goldenDay);
                GoldenDayCreateCampaignBLL.PolicySaveOrUpdate(policy);
                goldenDay.Policies.Add(policy);
            }
            GoldenDayCreateCampaignBLL.GoldenDaySaveOrUpdate(goldenDay);
            Dispose();
            return "";
        }
        public void Dispose()
        {
            if (goldenDayCreateCampaignBLL != null)
            {
                goldenDayCreateCampaignBLL.Dispose();
                goldenDayCreateCampaignBLL = null;
            }
        }
    }
}
