using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.BusinessLogic
{
    public class GoldenDayListCampaignBLL
    {
        //Bussiness logic cho trang GoldenDayListCampaign
        public CampaignRepository CampaignRepository { get; set; }
        public BookingRepository BookingRepository { get; set; }
        public GoldenDayListCampaignBLL()
        {
            CampaignRepository = new CampaignRepository();
            BookingRepository = new BookingRepository();
        }
        /// <summary>
        /// Giải phóng tất cả kết nổi đến cơ sở dữ liệu. 
        /// Phải thực hiện sau khi dùng xong Repository.
        /// </summary>
        public void Dispose()
        {
            if (CampaignRepository != null)
            {
                CampaignRepository.Dispose();
                CampaignRepository = null;
            }
            if (BookingRepository != null)
            {
                BookingRepository.Dispose();
                BookingRepository = null;
            }
        }
        /// <summary>
        /// Lấy tất cả Campaign.
        /// </summary>
        /// <returns>Trả về tất cả Campaign hoặc trả về danh sách rỗng nếu không tìm thấy kết quả.</returns>
        public IEnumerable<Campaign> CampaignGetAll()
        {
            return CampaignRepository.CampaignGetAll();
        }
        /// <summary>
        /// Lấy tất cả Booking được tạo mới sau khi Campaign được tạo.
        /// Booking phải có Status là Approved và phải có StartDate trùng với một trong những GoldenDay của Campaign.
        /// </summary>
        /// <param name="campaign">Campaign để tìm kiếm Booking.</param>
        /// <returns>Trả về tất cả Booking theo điều kiện của Campaign hoặc trả về danh sách rỗng nếu không tìm thấy kết quả.</returns>
        public IEnumerable<Booking> BookingGetAllNewBookingsByCampaign(Campaign campaign)
        {
            return BookingRepository.BookingGetAllNewBookingsByCampaign(campaign);
        }
        /// <summary>
        /// Lấy Campaign theo Id.
        /// </summary>
        /// <param name="campaignId">Id của Campaign cần lấy.</param>
        /// <returns>Trả về Campaign theo Id hoặc trả về Campaign với Id = 0 nếu không tìm thấy kết quả.</returns>
        public Campaign CampaginGetById(int campaignId)
        {
            return CampaignRepository.GetById(campaignId);
        }
        /// <summary>
        /// Lấy tất cả Campaign theo phân trang.
        /// </summary>
        /// <param name="pageSize">Số item hiển thị mỗi trang.</param>
        /// <param name="pageIndex">Chỉ số của trang hiện tại.</param>
        /// <param name="count">Bắt đầu hiển thị từ item thứ mấy.</param>
        /// <returns>Trả về danh sách Campaign tương ứng theo trang. Nếu không có kết quả trả về danh sách rỗng.</returns>
        public IEnumerable<Campaign> CampaignGetAllPaged(int pageSize, int pageIndex, out int count)
        {
            return CampaignRepository.CampaignGetAllPaged(pageSize, pageIndex, out count);
        }
        /// <summary>
        /// Xóa Campaign.
        /// </summary>
        /// <param name="campaign">Campaign cần xóa.</param>
        public void CampaignDelete(Campaign campaign)
        {
            CampaignRepository.Delete(campaign);
        }
    }
}