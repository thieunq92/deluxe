using Portal.Modules.OrientalSails.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.DataTransferObject
{
    /// <summary>
    /// Data Tranfer Object của domain Campaign
    /// </summary>
    public class CampaignDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public DateTime CreatedDate { get; set; }
        public ICollection<PolicyDTO> Policies { get; set; }
        public ICollection<DateTime> Dates { get; set; }
        public ICollection<String> DateAsStrings { get; set; }
        public CampaignDTO()
        {
            Policies = new List<PolicyDTO>();
            Dates = new List<DateTime>();
            DateAsStrings = new List<String>();
        }
    }
}