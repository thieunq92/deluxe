using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.BusinessLogic
{
    /// <summary>
    /// Bussiness logic của trang ExpenseHistory
    /// </summary>
    public class ExpenseHistoryBLL
    {
        public ExpenseHistoryRepository ExpenseHistoryRepository { get; set; }
        public AgencyRepository AgencyRepository { get; set; }
        public ExpenseServiceRepository ExpenseServiceRepository { get; set; }
        public ExpenseHistoryBLL() {
            ExpenseHistoryRepository = new ExpenseHistoryRepository();
            AgencyRepository = new AgencyRepository();
            ExpenseServiceRepository = new ExpenseServiceRepository();
        }
        /// <summary>
        /// Giải phóng tất cả kết nổi đến cơ sở dữ liệu. 
        /// Phải thực hiện sau khi dùng xong Repository.
        /// </summary>
        public void Dispose()
        {
            if (ExpenseHistoryRepository != null)
            {
                ExpenseHistoryRepository.Dispose();
                ExpenseHistoryRepository = null;
            }
            if (AgencyRepository != null)
            {
                AgencyRepository.Dispose();
                AgencyRepository = null;
            }
            if (ExpenseServiceRepository != null)
            {
                ExpenseServiceRepository.Dispose();
                ExpenseServiceRepository = null;
            }
        }
        /// <summary>
        /// Lấy Supplier(Agency) theo Id.
        /// </summary>
        /// <param name="supplierId">Id của Supplier cần lấy.</param>
        /// <returns>Trả về Supplier theo Id hoặc trả về Supplier có Id = 0 nếu không tim thấy kết quả</returns>
        public Agency SupplierGetById(int supplierId)
        {
            return AgencyRepository.GetById(supplierId);
        }
        /// <summary>
        /// Lấy ExpenseService theo Id.
        /// </summary>
        /// <param name="expenseId">Id của ExpenseService cần lấy.</param>
        /// <returns>Trả về ExpenseService theo Id hoặc trả về ExpenseService có Id = 0.</returns>
        public ExpenseService ExpenseGetById(int expenseId)
        {
            return ExpenseServiceRepository.GetById(expenseId);
        }
        /// <summary>
        /// Lấy tất cả ExpenseHistory theo ExpenseServce.
        /// </summary>
        /// <param name="expenseService">ExpenseService có liên quan đến ExpenseHistory.</param>
        /// <returns>Trả về danh sách ExpenseHistory hoặc trả về danh sách rỗng nếu không tìm thấy kết quả</returns>
        public ICollection<ExpenseHistory> ExpenseHistoryGetAllByCriterion(ExpenseService expenseService)
        {
            return ExpenseHistoryRepository.ExpenseHistoryGetAllByCriterion(expenseService);
        }
    }
}