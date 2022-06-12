using System;
using System.Collections.Generic;
using System.Web;

namespace Portal.Modules.OrientalSails.Domain
{
    /// <summary>
    /// Domain dùng để thao tác với bảng os_AgencyLevel(lưu trữ thông tin level của Agency)
    /// </summary>
    public class AgencyLevel
    {
        public virtual int Id { set; get; }
        public virtual string Name { set; get;}
        [Obsolete]
        public virtual IList<Agency> AgencyList { set; get; }
        [Obsolete]
        public virtual IList<AgencyCommission> AgencyCommissionList { set; get; }
    }
}