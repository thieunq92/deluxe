using System;
using System.Collections;
using CMS.Core.Domain;

namespace Portal.Modules.OrientalSails.Domain
{
    public class vAgency
    {
        private IList users;
        private IList bookings;
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
        public virtual User Sale { get; set; }
        public virtual string Accountant { get; set; }
        public virtual PaymentPeriod PaymentPeriod { get; set; }
        public virtual DateTime? LastBooking { get; set; }
        public virtual double Total { get; set; }
        public virtual double Paid { get; set; }
        public virtual double PaidBase { get; set; }
        public virtual double Payable { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual DateTime? ModifiedDate { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual User ModifiedBy { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual IList Users
        {
            get
            {
                if (users == null)
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

    }
}
