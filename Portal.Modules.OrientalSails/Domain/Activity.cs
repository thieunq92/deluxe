using System;
using System.Collections;
using CMS.Core.Domain;

namespace Portal.Modules.OrientalSails.Domain
{
    /// <summary>
    /// Domain dùng để thao tác với bảng os_Activity(lưu trữ thông tin về các meeting)
    /// </summary>
    public class Activity
    {
        public virtual int Id { get; set; }
        public virtual User User { get; set; }//Người tạo meeting 
        public virtual DateTime Time { get; set; }// Ngày tạo meeting
        [Obsolete]
        public virtual string Action { get; set; }
        public virtual string Url { get; set; }
        public virtual string Params { get; set; }//Id của Agency
        public virtual ImportantLevel Level { get; set; }
        public virtual string Note { get; set; }
        public virtual string ObjectType { get; set; }
        public virtual int ObjectId { get; set; }//Id của AgencyContact
        public virtual DateTime DateMeeting { get; set; }//Ngày sales đi gặp Agency
        public virtual DateTime? UpdateTime { get; set; }//Ngày sửa meeting

    }

    public enum ImportantLevel
    {
        Info,
        Regular,
        Important
    }

}
