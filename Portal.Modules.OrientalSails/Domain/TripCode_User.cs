using CMS.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.Domain
{
    public class TripCode_User
    {
        public virtual int Id { get; set; }
        public virtual string TripCode { get; set; }
        public virtual User User { get; set; }
        public virtual DateTime LastCheck { get; set; }
    }
}