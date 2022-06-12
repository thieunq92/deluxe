using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.DataTransferObject
{
    public class BookingDTO
    {
        public int Id { get; set; }
        public int NumberOfPax { get; set; }
        public int TripId { get; set; }
        public DateTime StartDate { get; set; }
        public string SpecialRequest { get; set; }
    }
}