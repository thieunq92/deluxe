using CMS.Core.Domain;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.BusinessLogic
{
    //Bussiness logic của trang GoldenDayCreateCampaign
    public class GoldenDayCreateCampaignBLL
    {
        public CampaignRepository CampaignRepository { get; set; }
        public CustomerRepository CustomerRepository { get; set; }
        public SailsTripRepository SailsTripRepository { get; set; }
        public UserOrganizationRepository UserOrganizationRepository { get; set; }
        public PolicyRepository PolicyRepository { get; set; }
        public GoldenDayRepository GoldenDayRepository { get; set; }
        public GoldenDayCreateCampaignBLL()
        {
            CampaignRepository = new CampaignRepository();
            CustomerRepository = new CustomerRepository();
            SailsTripRepository = new SailsTripRepository();
            UserOrganizationRepository = new UserOrganizationRepository();
            PolicyRepository = new PolicyRepository();
            GoldenDayRepository = new GoldenDayRepository();
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
            if (CustomerRepository != null)
            {
                CustomerRepository.Dispose();
                CustomerRepository = null;
            }
            if (SailsTripRepository != null)
            {
                SailsTripRepository.Dispose();
                SailsTripRepository = null;
            }
            if (UserOrganizationRepository != null)
            {
                UserOrganizationRepository.Dispose();
                UserOrganizationRepository = null;
            }
            if (PolicyRepository != null)
            {
                PolicyRepository.Dispose();
                PolicyRepository = null;
            }
            if (GoldenDayRepository != null)
            {
                GoldenDayRepository.Dispose();
                GoldenDayRepository = null;
            }
        }
        /// <summary>
        /// Lấy Campaign theo tháng và năm.
        /// </summary>
        /// <param name="month">Tháng bao nhiêu.</param>
        /// <param name="year">Năm bao nhiêu.</param>
        /// <returns>Trả về Campaign theo năm và tháng.Trả về Campaign có Id = 0 nếu không tìm thấy kết quả</returns>
        public Campaign CampaignGetByMonthAndYear(int month, int year)
        {
            return CampaignRepository.CampaignGetByMonthAndYear(month, year);
        }
        /// <summary>
        /// Lưu và cập nhật Campaign.
        /// </summary>
        /// <param name="campaign">Campaign cần lưu và cập nhật.</param>
        public void CampaignSaveOrUpdate(Campaign campaign)
        {
            CampaignRepository.SaveOrUpdate(campaign);
        }
        /// <summary>
        /// Lấy Campaign theo Id.
        /// </summary>
        /// <param name="campaignId">Id của Campaign cần lấy.</param>
        /// <returns>Trả về Campaign theo Id hoặc trả về Campaign có Id = 0 nếu không tìm thấy kết quả.</returns>
        public Campaign CampaignGetById(int campaignId)
        {
            return CampaignRepository.GetById(campaignId);
        }
        /// <summary>
        /// Đếm số khách theo ngày khởi hành của Booking có trạng thái Approved hoặc Pending.
        /// </summary>
        /// <param name="date">Ngày khởi hành của Booking.</param>
        /// <returns>Trả về số lượng khách trong ngày. </returns>
        public int CustomerGetCountingByDate(DateTime date)
        {
            return CustomerRepository.CustomerGetCountingByDate(date);
        }
        /// <summary>
        /// Lấy tất cả các Trip theo Region(Organization(Bắc, Trung, Nam))
        /// </summary>
        /// <param name="region">Region cần lấy. Nếu null trả về tất cả các trip của tất cả các region. </param>
        /// <returns>Trả về danh sách các trip theo Region. Trả về danh sách rỗng nếu không tìm thấy kết quả.</returns>
        public IEnumerable<SailsTrip> TripGetAllByRegion(Organization region)
        {
            return SailsTripRepository.TripGetAllByRegion(region);
        }
        /// <summary>
        /// Lấy tất cả Region(Organization) mà User có liên quan
        /// </summary>
        /// <param name="user">User mà cần lấy Region(Organization)</param>
        /// <returns></returns>
        public IEnumerable<Organization> RegionGetAllByUser(User user)
        {
            return UserOrganizationRepository.RegionGetAllByUser(user);
        }
        /// <summary>
        /// Lấy Policy theo Id.
        /// </summary>
        /// <param name="tripId">Id của Policy cần lấy.</param>
        /// <returns>Trả về Policy theo Id. Hoặc Policy có Id = 0 nếu không tìm thấy kết quả.</returns>
        public Policy PolicyGetById(int policyId)
        {
            return PolicyRepository.GetById(policyId);
        }
        /// <summary>
        /// Lấy Trip theo Id.
        /// </summary>
        /// <param name="tripId">Id của Trip cần lấy.</param>
        /// <returns>Trả về Trip theo Id. Hoặc Trip có Id = 0 nếu không tìm thấy kết quả.</returns>
        public SailsTrip TripGetById(int tripId)
        {
            var trip = SailsTripRepository.GetById(tripId);
            if (trip.Id == 0)
                return null;
            return SailsTripRepository.GetById(tripId);
        }
        /// <summary>
        /// Lưu hoặc cập nhật Policy.
        /// </summary>
        /// <param name="policy">Policy cần lưu hoặc cập nhật.</param>
        public void PolicySaveOrUpdate(Policy policy)
        {
            PolicyRepository.SaveOrUpdate(policy);
        }
        /// <summary>
        /// Xóa Policy.
        /// </summary>
        /// <param name="policy">Policy cần xóa.</param>
        public void PolicyDelete(Policy policy)
        {
            PolicyRepository.Delete(policy);
        }
        /// <summary>
        /// Xóa GoldenDay.
        /// </summary>
        /// <param name="goldenDay">GoldenDay cần xóa.</param>
        public void GoldenDayDelete(GoldenDay goldenDay)
        {
            GoldenDayRepository.Delete(goldenDay);
        }
        /// <summary>
        /// Lấy GoldenDay theo Id.
        /// </summary>
        /// <param name="goldenDayId">Id của GoldenDay cần lấy.</param>
        /// <returns>Trả về GoldenDay theo Id. Trả về GoldenDay có Id = 0 nếu không tìm thấy kết quả.</returns>
        public GoldenDay GoldenDayGetById(int goldenDayId)
        {
            return GoldenDayRepository.GetById(goldenDayId);
        }
        /// <summary>
        /// Lưu hoặc cập nhật GoldenDay.
        /// </summary>
        /// <param name="goldenDay">GoldenDay cần lưu hoặc cập nhật.</param>
        public void GoldenDaySaveOrUpdate(GoldenDay goldenDay)
        {
            GoldenDayRepository.SaveOrUpdate(goldenDay);
        }
        /// <summary>
        /// Lấy GoldenDay theo Date.
        /// </summary>
        /// <param name="date">Date của GoldenDay.</param>
        /// <returns>Trả về GoldenDay có Date như thế hoặc GoldenDay có Id = 0 nếu không có kết quả.</returns>
        public GoldenDay GoldenDayGetByDate(DateTime date)
        {
            return GoldenDayRepository.GoldenDayGetByDate(date);
        }
        [Obsolete]
        public void GoldenDayUpdate(GoldenDay goldenDay)
        {
            GoldenDayRepository.GoldenDayUpdate(goldenDay);
        }
    }
}