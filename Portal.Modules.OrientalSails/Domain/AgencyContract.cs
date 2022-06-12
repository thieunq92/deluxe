using System;

namespace Portal.Modules.OrientalSails.Domain
{
    /// <summary>
    /// Domain dùng để thao tác với bảng os_AgencyContract(lưu trữ thông tin hợp đồng với Agency)
    /// </summary>
    public class AgencyContract
    {
        protected int _id;
        protected string _contractName;
        protected byte[] _contractFile;
        protected DateTime _expiredDate;
        protected Agency _agency;
        protected string _fileName;

        public AgencyContract()
        {
            _id = -1;
        }

        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual string ContractName
        {
            get { return _contractName; }
            set { _contractName = value; }
        }

        public virtual byte[] ContractFile
        {
            get { return _contractFile; }
            set { _contractFile = value; }
        }

        public virtual DateTime ExpiredDate
        {
            get { return _expiredDate; }
            set { _expiredDate = value; }
        }

        public virtual Agency Agency
        {
            get { return _agency; }
            set { _agency = value; }
        }

        public virtual string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        public virtual string FilePath { get; set; }

        public virtual DateTime? CreateDate { set; get; }

        /// <summary>
        /// Agency đã nhận được hợp đồng chưa
        /// </summary>
        public virtual Boolean Received { set; get; }
    }
}
