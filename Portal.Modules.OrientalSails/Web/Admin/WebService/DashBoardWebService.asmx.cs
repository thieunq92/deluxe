using CMS.Core.Domain;
using Newtonsoft.Json;
using Portal.Modules.OrientalSails.BusinessLogic;
using Portal.Modules.OrientalSails.DataTransferObject;
using Portal.Modules.OrientalSails.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;

namespace Portal.Modules.OrientalSails.Web.Admin.WebService
{
    /// <summary>
    /// Summary description for DashBoardWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class DashBoardWebService : System.Web.Services.WebService
    {
        private DashBoardBLL dashBoardBLL;
        public DashBoardBLL DashBoardBLL
        {
            get
            {
                if (dashBoardBLL == null)
                    dashBoardBLL = new DashBoardBLL();
                return dashBoardBLL;
            }
        }
        public new void Dispose()
        {
            if (dashBoardBLL != null)
            {
                dashBoardBLL.Dispose();
                dashBoardBLL = null;
            }
        }
        [WebMethod]
        public string AgencyContactGetAllByAgencyId(string ai)
        {
            var agencyId = 0;
            try
            {
                agencyId = Int32.Parse(ai);
            }
            catch { }
            var agencyContacts = DashBoardBLL.AgencyContactGetAllByAgencyId(agencyId).Select(x => new { Id = x.Id, Name = x.Name, Position = x.Position });
            Dispose();
            return JsonConvert.SerializeObject(agencyContacts);
        }

        [WebMethod]
        public string GoldenDayGetAllInMonthByDate(DateTime date)
        {
            var firstDateOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDateOfMonth = firstDateOfMonth.AddMonths(1).AddDays(-1);
            var goldenDays = DashBoardBLL.GoldenDayGetAllByDateRange(firstDateOfMonth, lastDateOfMonth);
            var goldenDayDTOs = new List<GoldenDayDTO>();
            foreach (var goldenDay in goldenDays)
            {
                var goldenDayDTO = new GoldenDayDTO()
                {
                   Id = goldenDay.Id,
                   Date = goldenDay.Date,
                   Policies = goldenDay.GetPoliciesDTO(),
                };
                goldenDayDTO.Policy = string.Join("<br/>", goldenDayDTO.Policies.Select(x => "Trip: " + x.Trip.Name + " | Adult: " + x.Adult.ToString("#,0.##") + "₫ | Child: " + x.Child.ToString("#,0.##") + "₫").ToArray());
                goldenDayDTOs.Add(goldenDayDTO);
            }       
            Dispose();
            return JsonConvert.SerializeObject(goldenDayDTOs);
        }
    }
}
