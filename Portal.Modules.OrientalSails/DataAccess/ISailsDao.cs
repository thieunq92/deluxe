using System;
using System.Collections;
using System.Collections.Generic;
using CMS.Core.Domain;
using NHibernate.Criterion;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.Util;

namespace Portal.Modules.OrientalSails.DataAccess
{
    public interface ISailsDao
    {
        #region -- Room --

        /// <summary>
        /// Lấy về danh sách các room
        /// </summary>
        /// <param name="exceptDeleted">Có loại trừ deleted không</param>
        /// <returns></returns>
        IList RoomGetAll(bool exceptDeleted);

        /// <summary>
        /// Lấy về danh sach room theo class và type
        /// </summary>
        /// <param name="cruise"></param>
        /// <param name="roomClass"></param>
        /// <param name="roomType"></param>
        /// <returns></returns>
        IList RoomGetBy_ClassType(Cruise cruise, RoomClass roomClass, RoomTypex roomType);

        /// <summary>
        /// Kiểm tra xem có tồn tại phòng tương ứng với type và class hay ko?
        /// </summary>
        /// <param name="roomType"></param>
        /// <param name="roomClass"></param>
        /// <returns></returns>
        bool RoomCheckExist(RoomTypex roomType, RoomClass roomClass);

        /// <summary>
        /// List tất cả các phòng đã book tại một thời điểm
        /// </summary>
        /// <param name="cruise"></param>
        /// <param name="date"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        IList RoomGetNotAvailable(Cruise cruise, DateTime date, int duration);

        IList RoomGetNotAvailable(Cruise cruise, DateTime date, int duration, Booking exception);

        /// <summary>
        /// Lấy thông tin book các phòng trên tàu tại một thời điểm
        /// </summary>
        /// <param name="cruise">Các booking trên tàu này</param>
        /// <param name="date">Thời điểm check</param>
        /// <param name="duration">Thời gian cần kiểm tra (số ngày liên tiếp)</param>
        /// <returns></returns>
        IList BookingRoomNotAvaiable(Cruise cruise, DateTime date, int duration);

        IList BookingRoomNotAvaiable(Cruise cruise, DateTime date, int duration, Booking exception);
        #endregion

        #region -- Room Class --

        IList RoomClassGetAll();
        #endregion

        #region -- Room Type --

        IList RoomTypexGetAll();
         #endregion

        #region -- Booking --

        /// <summary>
        /// Lấy về danh sách các booking 
        /// </summary>
        /// <param name="exceptDeleted">Có lọai trừ deleted ko?</param>
        /// <returns></returns>
        IList BookingGetAll(bool exceptDeleted);

        /// <summary>
        /// Tìm kiếm Booking
        /// </summary>
        /// <param name="criterion"></param>
        /// <param name="customer"></param>
        /// <param name="blocked"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        IList BookingSearch(ICriterion criterion,string customer, int blocked, Order order);

        IList BookingSearch(ICriterion criterion, string customer, int blocked,int pageSize, int pageIndex, Order order);

        int BookingCount(ICriterion criterion, string customer, int blocked);

        int BookingGetMaxCustomId();
        #endregion

        #region -- Sail Price Config --
        /// <summary>
        /// Lấy về price config theo trip, room type và room class
        /// </summary>
        /// <param name="trip"></param>
        /// <param name="type"></param>
        /// <param name="roomClass"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        SailsPriceConfig SailsPriceConfigGetBy_RoomType_RoomClass_Trip(SailsTrip trip, RoomTypex type, RoomClass roomClass, TripOption option);
        #endregion

        #region -- Agency --

        /// <summary>
        /// Lấy về đại lý theo role cho trước
        /// </summary>
        /// <param name="role">Role đại lý lấy về</param>
        /// <returns>AgencyPolicy</returns>
        IList AgencyPolicyGetByRole(Role role);

        int UserCount();

        IList AgencyGetWithLastBooking(ICriterion criterion, params Order[] orders);
        #endregion

        #region -- Customer --

        /// <summary>
        /// Lấy về các Customer của 1 booking
        /// </summary>
        /// <param name="booking"></param>
        /// <returns></returns>
        IList CustomerGetByBooking(Booking booking);

        void IncomeSum(DateTime from, DateTime to, out double income, out double receivable);
        void OutcomeSum(DateTime from, DateTime to, out double outcome, out double payable);

        IList CustomerGetAllDistinct();
        IList CustomerGetByCriterionPaged(ICriterion criterion, int pageSize, int pageIndex,out int count, params Order[] orders);
        //int RoomBookedCount(RoomClass roomClass, RoomTypex roomType, DateTime date);
        #endregion        

        #region -- Expense --

        IList ExpenseServiceGet(SailsTrip trip, DateTime? from, DateTime? to, Agency agency, int pageSize, int pageIndex, out int count, string orgid, User user, bool hideZero, string paymentStatus, int tripType , string tripcode, params CostType[] type);
        #endregion

        #region -- Bar --

        double SumBarByDate(DateTime date);
        #endregion

        int CustomerServiceCountByBooking(ExtraOption service,Booking booking);
        int RunningDayCount(Cruise cruise, int year, int month);

        IList SailCustomerGet(ICriterion criterion);

        //double TotalCost(DateTime from, DateTime to);
        IList FeedbackReport(ICriterion criterion);
        int AnswerReport(ICriterion criterion);
        IList<Booking> GetBookingShadow(DateTime date);

        #region -- NEW WAY TO ACCESS --

        IList<ExpenseService> ExpenseServiceGet(ICriterion criterion, int pageSize = 0, int pageIndex = 0);
        int ExpenseServiceCount(ICriterion criterion);

        #endregion
    }
}