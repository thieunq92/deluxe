using Portal.Modules.OrientalSails.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.Domain
{
    public class Policy
    {
        public virtual int Id { get; set; }
        public virtual double Adult { get; set; }
        public virtual double Child { get; set; }
        public virtual SailsTrip Trip { get; set; }
        public virtual ICollection<GoldenDay> GoldenDays { get; set; }
        public virtual Campaign Campaign { get; set; }
        public Policy()
        {
            Trip = new SailsTrip();
            GoldenDays = new List<GoldenDay>();
        }
        public virtual ICollection<GoldenDayDTO> GetGoldenDaysDTO(){
            var goldenDaysDTO = new List<GoldenDayDTO>();
            foreach (var goldenDay in GoldenDays)
            {
                var goldenDayDTO = new GoldenDayDTO()
                {
                    Id = goldenDay.Id,
                    Date = goldenDay.Date,
                };
                goldenDaysDTO.Add(goldenDayDTO);
            }
            return goldenDaysDTO;
        }
    }
}