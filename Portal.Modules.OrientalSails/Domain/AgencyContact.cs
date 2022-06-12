using System;

namespace Portal.Modules.OrientalSails.Domain
{
    /// <summary>
    /// Domain dùng để thao tác với bảng os_AgencyContact(lưu trữ thông tin nhân viên của Agency)
    /// </summary>
    public class AgencyContact
    {        
        protected int _id;
        protected string _name;
        protected string _phone;
        protected string _email;
        protected bool _enabled;
        protected Agency _agency;
        protected string _position;

        public AgencyContact()
        {
            _id = -1;
            _enabled = true;
        }

        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public virtual string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }

        public virtual string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        /// <summary>
        /// AgencyContact còn hoạt động không
        /// </summary>
        public virtual bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        /// <summary>
        /// AgencyContact thuộc Agency nào
        /// </summary>
        public virtual Agency Agency
        {
            get { return _agency; }
            set { _agency = value; }
        }

        public virtual int AgencyId
        {
            get
            {
                if (_agency!=null)
                {
                    return _agency.Id;
                }
                return -1;
            }
        }

        /// <summary>
        /// Vị trí công tác của AgencyContact
        /// </summary>
        public virtual string Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public virtual DateTime? Birthday { set; get; }
        /// <summary>
        /// Có phải là người book Booking không 
        /// </summary>
        public virtual bool IsBooker { get; set; }
        public virtual String Note { set; get; }
    }
}
