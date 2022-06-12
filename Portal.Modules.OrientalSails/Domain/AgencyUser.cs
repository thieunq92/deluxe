using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CMS.Core.Domain;

namespace Portal.Modules.OrientalSails.Domain
{
    /// <summary>
    /// Domain dùng để thao tác với bảng os_AgencyUser(lưu trữ thông tin User gắn với Agency )
    /// </summary>
    public class AgencyUser
    {
        protected int _id;
        protected User _user;
        protected Agency _agency;

        public AgencyUser()
        {
            _id = -1;
        }

        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual Agency Agency
        {
            get { return _agency;}
            set { _agency = value; }
        }

        public virtual User User
        {
            get { return _user; }
            set { _user = value; }
        }
    }
}
