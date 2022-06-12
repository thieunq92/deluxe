using CMS.Core.Domain;
using Portal.Modules.OrientalSails.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.DataTransferObject.DashBoardManager
{
    public class SalesBookingsDTO
    {
        public User Sales { get; set; }
        public IEnumerable<Booking> Bookings { get; set; }
    }
}