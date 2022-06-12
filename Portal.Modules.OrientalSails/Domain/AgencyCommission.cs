using System;
using System.Collections.Generic;
using System.Web;

namespace Portal.Modules.OrientalSails.Domain
{
    /// <summary>
    /// Domain dùng để thao tác với bảng os_AgencyCommission(lưu trữ thông tin chính sách tiền hoa hồng của đối tác)
    /// </summary>
    public class AgencyCommission
    {
        public virtual int Id { set; get; }
        public virtual SailsTrip SailsTrip { set; get; }
        public virtual AgencyLevel AgencyLevel { set; get; }
        public virtual double CommissionAdultUSD { set; get; }
        public virtual double CommissionAdultVND { set; get; }
        public virtual double CommissionChildUSD { set; get; }
        public virtual double CommissionChildVND { set; get; }
        public virtual double CommissionBabyUSD { set; get; }
        public virtual double CommissionBabyVND { set; get; }
        public virtual DateTime ValidFrom { set; get; }//Chính sách có hiệu lực từ bao giờ
    }
}