using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.BusinessLogic
{
    /// <summary>
    /// Bussiness logic của trang BookingView
    /// </summary>
    public class BookingViewBLL
    {
        public BookingRepository BookingRepository { get; set; }
        public CruiseExpenseTableRepository CruiseExpenseTableRepository { get; set; }
        public SailsTripRepository SailsTripRepository { get; set; }
        public SailsPriceConfigRepository SailsPriceConfigRepository { get; set; }
        public AgencyCommissionRepository AgencyCommissionRepository { get; set; }
        public AgencyRepository AgencyRepository { get; set; }
        public BookingViewBLL()
        {
            BookingRepository = new BookingRepository();
            CruiseExpenseTableRepository = new CruiseExpenseTableRepository();
            SailsTripRepository = new SailsTripRepository();
            SailsPriceConfigRepository = new SailsPriceConfigRepository();
            AgencyCommissionRepository = new AgencyCommissionRepository();
            AgencyRepository = new AgencyRepository();
        }
        /// <summary>
        /// Giải phóng tất cả kết nổi đến cơ sở dữ liệu. 
        /// Phải thực hiện sau khi dùng xong Repository.
        /// </summary>
        public void Dispose()
        {
            if (BookingRepository != null)
            {
                BookingRepository.Dispose();
                BookingRepository = null;
            }
            if (CruiseExpenseTableRepository != null)
            {
                CruiseExpenseTableRepository.Dispose();
                CruiseExpenseTableRepository = null;
            }
            if (SailsTripRepository != null)
            {
                SailsTripRepository.Dispose();
                SailsTripRepository = null;
            }
            if (SailsPriceConfigRepository != null)
            {
                SailsPriceConfigRepository.Dispose();
                SailsPriceConfigRepository = null;
            }
            if (AgencyCommissionRepository != null)
            {
                AgencyCommissionRepository.Dispose();
                AgencyCommissionRepository = null;
            }
            if (AgencyRepository != null)
            {
                AgencyRepository.Dispose();
                AgencyRepository = null;
            }
        }

        /// <summary>
        /// Lấy Booking bằng Id
        /// </summary>
        /// <param name="bookingId">Id của Booking</param>
        /// <returns></returns>
        public Booking BookingGetById(int bookingId)
        {
            return BookingRepository.BookingGetById(bookingId);
        }

        [Obsolete]
        public CruiseExpenseTable CruiseExpenseTableGetByTripAndDate(SailsTrip trip, DateTime startDate)
        {
            return CruiseExpenseTableRepository.CruiseExpenseTableGetByTripAndDate(trip, startDate);
        }

        /// <summary>
        /// Lấy Trip bằng Id
        /// </summary>
        /// <param name="tripId">Id của Trip</param>
        /// <returns></returns>
        public SailsTrip SailsTripGetById(int tripId)
        {
            return SailsTripRepository.GetById(tripId);
        }
        /// <summary>
        /// Lấy SailsPriceConfig theo Trip và ValidFrom
        /// </summary>
        /// <param name="trip">Theo Trip gắn với SailsPriceConfig</param>
        /// <param name="startDate">Theo ValidFrom gắn với SailsPriceConfig </param>
        /// <returns></returns>
        public SailsPriceConfig SailsPriceConfigGetByTripAndStartDate(SailsTrip trip, DateTime startDate)
        {
            return SailsPriceConfigRepository.SailsPriceConfigGetByTripAndStartDate(trip, startDate);
        }

        [Obsolete]
        public AgencyCommission AgencyCommissionGetBy(SailsTrip trip, DateTime startDate)
        {
            return AgencyCommissionRepository.AgencyCommissionGetBy(trip, startDate);
        }
        
        /// <summary>
        /// Lấy AgencyComission theo Trip, ValidFrom và AgencyLevel
        /// </summary>
        /// <param name="trip">theo Trip gắn với AgencyCommission</param>
        /// <param name="startDate">theo ValidFrom gắn với AgencyCommission</param>
        /// <param name="agencyLevel">theo AgencyLevel gắn với AgencyCommission</param>
        /// <returns></returns>
        public AgencyCommission AgencyCommissionGetBy(SailsTrip trip, DateTime startDate, AgencyLevel agencyLevel)
        {
            return AgencyCommissionRepository.AgencyCommissionGetBy(trip, startDate, agencyLevel);
        }
        /// <summary>
        /// Lấy Agency theo Id
        /// </summary>
        /// <param name="agencyId">Id của Agency</param>
        /// <returns></returns>
        public Agency AgencyGetById(int agencyId)
        {
            return AgencyRepository.GetById(agencyId);
        }
    }
}