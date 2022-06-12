using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.DataTransferObject
{
    public class UserDTO
    {
        public int SalesId { get; set; }
        public int NumberOfBooking { get; set; }
        public decimal RevenueInUSD { get; set; }
        public int NumberOfPax { get; set; }
        public int NumberOfReport { get; set; }
    }

    public class TopPartnerAnalysisDTO
    {
        public int NUMBER_OF_PAX { get; set; }
        public int AGENCY_ID { get; set; }
        public string AGENCY_NAME { get; set; }
        public int MONTH { get; set; }
        public int YEAR { get; set; }
    }

    public class AgencySendNoBookingLast3MonthDTO
    {
        public DateTime CREATED_DATE { get; set; }
        public string AGENCY_NAME { get; set; }
        public int AGENCY_ID { get; set; }
    }

    public class AgencyNotVisitedUpdatedLast2MonthDTO
    {
        public int AGENCY_ID { get; set; }
        public string AGENCY_NAME { get; set; }
        public int MEETING_ID { get; set; }
        public DateTime DATE_MEETING { get; set; }
    }

    public class MostRecentMeetingDTO
    {
        public DateTime DATE_MEETING { get; set; }
        public int AGENCY_ID { get; set; }
        public string AGENCY_NAME { get; set; }
        public string AGENCYCONTACT_NAME { get; set; }
        public string NOTE { get; set; }
    }

    public class GeneralInformationDTO
    {
        public int NO_OF_PAX { get; set; }
        public int PERCENT_OUT_OF_TOTAL { get; set; }
        public decimal REVENUE_IN_MONTH { get; set; }
        public int TOP_PERCENTAGE { get; set; }
        public int NO_OF_BOOKING { get; set; }
        public int MEETING_REPORT { get; set; }
        public int MONTH { get; set; }
        public int YEAR { get; set; }
    }

    public class PartnerInChargeDTO
    {
        public int AGENCY_ID { get; set; }
        public string AGENCY_NAME { get; set; }
        public int CONTRACT_STATUS { get; set; }
        public string ROLE_NAME { get; set; }
        public DateTime? LAST_BOOKING { get; set; }
        public DateTime? LAST_MEETING { get; set; }
        public string DETAIL { get; set; }
    }

    public class NewPartnerDTO
    {
        public int AGENCY_ID { get; set; }
        public string AGENCY_NAME { get; set; }
        public string MOST_RECENT_MEETING { get; set; }
        public DateTime? LAST_BOOKING { get; set; }
    }
}