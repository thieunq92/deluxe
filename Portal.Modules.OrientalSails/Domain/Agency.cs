using System;
using System.Collections;
using CMS.Core.Domain;

namespace Portal.Modules.OrientalSails.Domain
{
    /// <summary>
    /// Domain dùng để thao tác với bảng os_Agency(lưu trữ thông tin về các đối tác, guide, nhà xe)
    /// </summary>
    public class Agency
    {

        private IList users;
        private IList bookings;
        protected IList history;

        public Agency()
        {
 
        }

        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Phone { get; set; }
        public virtual string Address { get; set; }
        public virtual string Contract { get; set; }
        public virtual string Description { get; set; }
        public virtual int ContractStatus { get; set; }
        public virtual string TaxCode { get; set; }
        public virtual string Email { get; set; }
        public virtual string Fax { get; set; }
        public virtual Role Role { get; set; }

        public virtual IList Users
        {
            get
            {
                if (users==null)
                {
                    users = new ArrayList();
                }
                return users;
            }
            set { users = value; }
        }

        public virtual IList Bookings
        {
            get
            {
                if (bookings == null)
                {
                    bookings = new ArrayList();
                }
                return bookings;
            }
            set { bookings = value; }
        }
        public virtual User Sale { get; set; }//Sales in charge
        public virtual string Accountant { get; set; }
        public virtual PaymentPeriod PaymentPeriod { get; set; }
        public virtual DateTime LastBooking { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual DateTime? ModifiedDate { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual User ModifiedBy { get; set; }
        public virtual bool Deleted { get; set; } //Đánh dấu xóa Agency
        public virtual Organization Organization { get; set; }
        public virtual AgencyLevel AgencyLevel { set; get; }

    }

    public enum PaymentPeriod
    {
        None,
        Monthly,
        MonthlyCK
    }

    public class PaymentPeriodClass :  NHibernate.Type.EnumStringType
    {
        public PaymentPeriodClass()
            : base(typeof(PaymentPeriod), 20)
        {

        }
    }
}
