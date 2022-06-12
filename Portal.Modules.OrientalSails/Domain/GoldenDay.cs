using Portal.Modules.OrientalSails.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.Domain
{
    public class GoldenDay
    {
        public virtual int Id { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual ICollection<Policy> Policies { get; set; }
        public GoldenDay()
        {
            Policies = new List<Policy>();
        }
        public virtual ICollection<PolicyDTO> GetPoliciesDTO()
        {
            var policiesDTO = new List<PolicyDTO>();
            var policies = Policies.GroupBy(p => p.Trip.Id).Select(grp => grp.Count() > 1 ? grp.Where(p => p.Campaign == null).First() : grp.First());
            foreach (var policy in policies)
            {
                var policyDTO = new PolicyDTO()
                {
                    Id = policy.Id,
                    Adult = policy.Adult,
                    Child = policy.Child,
                    Trip = new TripDTO()
                    {
                        Id = policy.Trip.Id,
                        Name = policy.Trip.Name
                    },
                    GoldenDays = policy.GetGoldenDaysDTO(),
                };
                policiesDTO.Add(policyDTO);
            }
            return policiesDTO;
        }
    }
}