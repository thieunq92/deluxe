using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.DataTransferObject
{
    /// <summary>
    /// Data Transfer Object của domain GoldenDay
    /// </summary>
    public class GoldenDayDTO
    {
        public virtual int Id { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual ICollection<PolicyDTO> Policies { get; set; }
        public string DateAsString
        {
            get
            {
                return Date.ToString("dd/MM/yyyy");
            }
        }
        public string Policy
        {
            get;
            set;
        }
    }
 
}