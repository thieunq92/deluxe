using CMS.Core.Domain;
using Portal.Modules.OrientalSails.DataTransferObject;
using Portal.Modules.OrientalSails.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.BusinessLogic
{
    public class SalesPerformanceAnalysisBLL
    {
        private UserRepository _userRepository;
        private BookingRepository _bookingRepository;

        public SalesPerformanceAnalysisBLL()
        {
            _userRepository = new UserRepository();
            _bookingRepository = new BookingRepository();
        }

        public void Dispose()
        {
            if (_userRepository != null)
            {
                _userRepository.Dispose();
                _userRepository = null;
            }
            if (_bookingRepository != null)
            {
                _bookingRepository.Dispose();
                _bookingRepository = null;
            }
        }

        public List<SalesForDropDownListDTO> SalesGetAllForDropDownList()
        {
            try
            {
                return _userRepository.SalesGetAllForDropDownList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GeneralInformationDTO> GetGeneralInformation(int month, int year, int salesId)
        {
            try
            {
                var fromDate = new DateTime(year, month, 1).AddMonths(-5);
                var toDate = new DateTime(year, month, 1).AddMonths(2).AddDays(-1);
                var fromDateSameMonthLastYear = new DateTime(year, month, 1).AddYears(-1);
                var toDateSameMonthLastYear = new DateTime(year, month, 1).AddYears(-1).AddMonths(1).AddDays(-1);
                var data = _userRepository.GetGeneralInformation(fromDate, toDate, fromDateSameMonthLastYear, toDateSameMonthLastYear, salesId);
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetNumberOfPartnerInCharge(int salesId)
        {
            try
            {
                var data = _userRepository.GetNumberOfPartnerInCharge(salesId);
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetNumberOfMeetingInMonth(int month, int salesId)
        {
            try
            {
                var fromDate = new DateTime(DateTime.Today.Year, month, 1);
                var toDate = fromDate.AddMonths(1).AddDays(-1);
                var data = _userRepository.GetNumberOfMeetingInMonth(fromDate, toDate, salesId);
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TopPartnerAnalysisDTO> GetTopPartnerAnalysis(int month, int year, int salesId)
        {
            try
            {
                var today = DateTime.Now.Date;
                var fromDate = new DateTime(year, month, 1).AddMonths(-2);
                var toDate = new DateTime(year, month, 1).AddMonths(1).AddDays(-1);
                var data = _userRepository.GetTopPartnerAnalysis(fromDate, toDate, salesId);
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PartnerInChargeDTO> GetPartnerInCharge(int salesId)
        {
            try
            {
                var data = _userRepository.GetPartnerInCharge(salesId);
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<NewPartnerDTO> GetNewPartner(int salesId, int month, int year)
        {
            try
            {
                var fromDate = new DateTime(year, month, 1);
                var toDate = new DateTime(year, month, 1).AddMonths(1).AddDays(-1);
                var data = _userRepository.GetNewPartner(salesId, fromDate, toDate);
                return data;
            }catch(Exception ex){
                throw ex;
            }
        }
    }
}