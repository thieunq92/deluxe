using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Core.Domain;

namespace Portal.Modules.OrientalSails.Domain
{
    public class TripCode_Note
    {
        public virtual int Id { get; set; }
        public virtual  string TripCode { get; set; }
        public virtual string Note { get; set; }
        public virtual Role ToRole { get; set; }
        public virtual User CreatedUser { get; set; }
        public virtual DateTime CreatedDate { get; set; }
    }
}