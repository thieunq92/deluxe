using Portal.Modules.OrientalSails.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.DataTransferObject
{
    /// <summary>
    /// Data Transfer Object của domain Policy
    /// </summary>
    public class PolicyDTO
    {
        public int Id { get; set; }
        public double Adult { get; set; }
        public double Child { get; set; }   
        public TripDTO Trip { get; set; }
        public ICollection<GoldenDayDTO> GoldenDays { get; set; }
        public PolicyDTO()
        {
            Trip = new TripDTO();
            GoldenDays = new List<GoldenDayDTO>();
        }
        /// <summary>
        /// Chuyển những object Policy sang object PolicyDTO.
        /// </summary>
        /// <param name="policies">Danh sách Policy cần chuyển.</param>
        /// <returns>Trả về danh sách Policy đã được chuyển.</returns>
        public static IEnumerable<PolicyDTO> GetPoliciesDTO(IEnumerable<Policy> policies)
        {
            var policiesDTO = new List<PolicyDTO>();
            foreach (var policy in policies)
            {
                var policyDTO = new PolicyDTO()
                {
                    Id = policy.Id,
                    Adult = policy.Adult,
                    Child = policy.Child,
                    GoldenDays = policy.GetGoldenDaysDTO(),
                    Trip = new TripDTO(),

                };
                if (policy.Trip != null)
                {
                    policyDTO.Trip.Id = policy.Trip.Id;
                    policyDTO.Trip.Name = policy.Trip.Name;
                }
                policiesDTO.Add(policyDTO);
            }
            return policiesDTO;
        }
    }
}