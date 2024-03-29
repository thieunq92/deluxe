using System;
using System.Collections;
using CMS.Core.Domain;

namespace Portal.Modules.OrientalSails.Domain
{
    #region Booking

    /// <summary>
    /// Booking object for NHibernate mapped table 'os_Booking'.
    /// </summary>
    public class Transaction
    {
        #region -- CONST --

        public const int BOOKING = 0;
        public const int COMMISSION = 1;
        public const int GUIDECOLLECT = 2;
        public const int CALCULATED_EXPENSE = 3;
        public const int MANUALDAILY_EXPENSE = 4;
        public const int DRIVERCOLLECT = 5;

        #endregion

        #region -- PRIVATE MEMBERS --

        private string _agencyName;
        #endregion

        #region Constructors

        public Transaction()
        {
        }

        #endregion

        #region Public Properties

        public virtual int Id { get; set; }
        public virtual Booking Booking { get; set; }
        public virtual int TransactionType { get; set; }
        public virtual double USDAmount { get; set; }
        public virtual double VNDAmount { get; set; }
        public virtual Agency Agency { get; set; }
        public virtual string AgencyName
        {
            get
            {
                if (Agency!=null)
                {
                    return Agency.Name;
                }
                return _agencyName;
            }
            set { _agencyName = value; }
        }

        /// <summary>
        /// Ghi chú thanh toán
        /// </summary>
        public virtual string Note { get; set; }

        public virtual bool IsExpense { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual User CreatedBy { get; set; }



        #endregion
    }
    #endregion
}