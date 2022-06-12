using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.BusinessLogic
{
    /// <summary>
    /// Bussiness logic của trang EventEdit
    /// </summary>
    public class EventEditBLL
    {
        ExpenseServiceRepository ExpenseServiceRepository { get; set; }
        SailExpenseRepository SailExpenseRepository { get; set; }
        ExpenseHistoryRepository ExpenseHistoryRepository { get; set; }
        public EventEditBLL()
        {
            ExpenseServiceRepository = new ExpenseServiceRepository();
            SailExpenseRepository = new SailExpenseRepository();
            ExpenseHistoryRepository = new ExpenseHistoryRepository();
        }
        /// <summary>
        /// Giải phóng tất cả kết nổi đến cơ sở dữ liệu. 
        /// Phải thực hiện sau khi dùng xong Repository.
        /// </summary>
        public void Dispose()
        {
            if (ExpenseServiceRepository != null)
            {
                ExpenseServiceRepository.Dispose();
                ExpenseServiceRepository = null;
            }
            if (SailExpenseRepository != null)
            {
                SailExpenseRepository.Dispose();
                SailExpenseRepository = null;
            }
            if (ExpenseHistoryRepository != null)
            {
                ExpenseHistoryRepository.Dispose();
                ExpenseHistoryRepository = null;
            }
        }
        /// <summary>
        /// Lấy ExpenseService theo Id.
        /// </summary>
        /// <param name="expenseServiceId">Id của ExpenseService cần lấy.</param>
        /// <returns>Trả về ExpenseService theo Id hoặc trả về ExpenseService có Id = 0.</returns>
        public ExpenseService ExpenseServiceGetById(int expenseServiceId)
        {
            return ExpenseServiceRepository.GetById(expenseServiceId);
        }
        /// <summary>
        /// Lưu và cập nhật ExpenseService.
        /// </summary>
        /// <param name="expenseService">ExpenseService cần lưu hoặc cập nhật.</param>
        public void ExpenseServiceSaveOrUpdate(ExpenseService expenseService)
        {
           ExpenseServiceRepository.SaveOrUpdate(expenseService);
        }
        /// <summary>
        /// Lấy Expense theo Id 
        /// </summary>
        /// <param name="sailExpenseId">Id của Expense cần lấy.</param>
        /// <returns>Trả về Expense theo Id hoặc trả về Expense có Id = 0 nếu không tìm thấy kết quả.</returns>
        public SailExpense SailExpenseGetById(int sailExpenseId)
        {
            return SailExpenseRepository.GetById(sailExpenseId);
        }
        /// <summary>
        /// Lưu hoặc cập nhật ExpenseHistory.
        /// </summary>
        /// <param name="expenseHistory">ExpenseHistory cần lưu hoặc cập nhật.</param>
        public void ExpenseHistorySaveOrUpdate(ExpenseHistory expenseHistory)
        {
            ExpenseHistoryRepository.SaveOrUpdate(expenseHistory);
        }

        public void ExpenseServiceDelete(ExpenseService expenseService)
        {   
            ExpenseServiceRepository.Delete(expenseService);
        }
    }
}