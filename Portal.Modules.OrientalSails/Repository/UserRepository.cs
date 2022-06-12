using CMS.Core.Domain;
using NHibernate;
using NHibernate.Transform;
using Portal.Modules.OrientalSails.DataTransferObject;
using Portal.Modules.OrientalSails.Web.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.Repository
{
    public class UserRepository : RepositoryBase<User>
    {

        public UserRepository() : base() { }

        public UserRepository(ISession session) : base(session) { }


        public User UserGetById(int userId)
        {
            return _session.QueryOver<User>()
                .Where(x => x.IsActive == true)
                .Where(x => x.Id == userId).Take(1).FutureValue<User>().Value;
        }

        public string UserGetName(int userId)
        {
            return UserGetById(userId).FullName;
        }

        public IList<User> UserGetByRole(int roleId)
        {
            Role roleAlias = null;
            return _session.QueryOver<User>()
                .Where(x => x.IsActive == true)
                .JoinAlias(x => x.Roles, () => roleAlias)
                .Where(x => roleAlias.Id == roleId).Future<User>().ToList();
        }

        public object GetMonthSummary(int month, int year, List<User> saleses)
        {
            var firstDateOfMonth = new DateTime(year, month, 1);
            var lastDateOfMonth = firstDateOfMonth.AddMonths(1).AddDays(-1);

            var query = @"SELECT NumberOfBooking,
       RevenueInUSD,
       NumberOfPax,
       NumberOfReport,
       b1.userid AS SalesId
FROM (
        (SELECT SUM(NumberOfBooking) AS NumberOfBooking,
                SUM(RevenueInUSD) AS RevenueInUSD,
                userid
         FROM
           (SELECT COUNT(Booking.Id) AS NumberOfBooking,
                   CASE
                       WHEN Booking.IsVND = 0 THEN Booking.Total * CASE
                                                                          WHEN Booking.CurrencyRate = 0 THEN 23000
                                                                          ELSE Booking.CurrencyRate
                                                                      END
                       WHEN Booking.IsVND = 1 THEN Booking.TotalVND
                   END AS RevenueInUSD,
                   _User.userid
            FROM os_Booking Booking
            JOIN os_Agency Agency ON Booking.AgencyId = Agency.Id
            JOIN bitportal_user _User ON _User.userid = Agency.SaleId
            WHERE Booking.StartDate >= :firstDateOfMonth
              AND Booking.StartDate <= :lastDateOfMonth
              AND Booking.Deleted = 0
              AND Booking.Status = " + (int)StatusType.Approved + @"
              AND _User.userid IN(:salesId)
            GROUP BY _User.userid,
                     Booking.IsVND,
                     Booking.TotalVND,
                     Booking.CurrencyRate,
                     Booking.IsVND,
                     Booking.Total) AS b
         GROUP BY b.userid) AS b1
      FULL JOIN
        (SELECT COUNT(Customer.Id) AS NumberOfPax,
                _User.userid
         FROM os_Customer Customer
         JOIN os_Booking Booking ON Customer.BookingId = Booking.Id
         JOIN os_Agency Agency ON Booking.AgencyId = Agency.Id
         JOIN bitportal_user _User ON _User.userid = Agency.SaleId
         WHERE Booking.StartDate >= :firstDateOfMonth
           AND Booking.StartDate <= :lastDateOfMonth
           AND Booking.Deleted = 0
           AND Booking.Status = " + (int)StatusType.Approved + @"
           AND _User.userid IN(:salesId)
         GROUP BY _User.userid) AS c ON b1.userid = c.userid
      FULL JOIN
        (SELECT COUNT(Activity.Id) AS NumberOfReport,
                _User.userid
         FROM os_Activity Activity
         JOIN bitportal_user _User ON Activity.UserId = _User.userid
         WHERE Activity.DateMeeting >= :firstDateOfMonth
           AND Activity.DateMeeting <= :lastDateOfMonth
           AND _User.userid IN(:salesId)
         GROUP BY _User.userid) AS a ON c.userid = a.userid)";
            var dataAll = _session.CreateSQLQuery(query)
             .SetParameter("firstDateOfMonth", firstDateOfMonth)
             .SetParameter("lastDateOfMonth", lastDateOfMonth)
             .SetParameterList("salesId", saleses.Select(s => s.Id).ToList())
             .SetResultTransformer(Transformers.AliasToBean<UserDTO>()).List<UserDTO>();
            return dataAll;
        }

        public List<SalesForDropDownListDTO> SalesGetAllForDropDownList()
        {
            var sql = @"
                        SELECT 
                          U.userid AS VALUE, 
                          U.username AS TEXT 
                        FROM 
                          bitportal_role AS R 
                          INNER JOIN bitportal_userrole AS UR ON R.roleid = UR.roleid 
                          INNER JOIN bitportal_user AS U ON UR.userid = U.userid 
                        WHERE 
                          R.name = 'Sales'";
            var dataAll = _session.CreateSQLQuery(sql).SetResultTransformer(
                Transformers.AliasToBean<SalesForDropDownListDTO>())
                .List<SalesForDropDownListDTO>();
            return dataAll.ToList();
        }

        public List<GeneralInformationDTO> GetGeneralInformation(DateTime fromDate, DateTime toDate, DateTime fromDateSameMonthLastYear, DateTime toDateSameMonthLastYear, int salesId)
        {
            var sql = @"
                        SELECT 
                          COALESCE(TABLEA.NO_OF_PAX, 0) AS NO_OF_PAX, 
                          COALESCE(
                            TABLEA.NO_OF_PAX * 100 / TABLEB.NUMBER_OF_PAX_TOTAL, 
                            0
                          ) AS PERCENT_OUT_OF_TOTAL, 
                          COALESCE(TABLEC.REVENUE_IN_MONTH, 0) AS REVENUE_IN_MONTH, 
                          COALESCE(
                            TABLED.TOP_NUMBER_OF_PAX * 100 / TABLEB.NUMBER_OF_PAX_TOTAL, 
                            0
                          ) AS TOP_PERCENTAGE, 
                          COALESCE(TABLEE.NO_OF_BOOKING, 0) AS NO_OF_BOOKING, 
                          COALESCE(TABLEF.MEETING_REPORT, 0) AS MEETING_REPORT, 
                          COALESCE(TABLEA.MONTH,TABLEF.MONTH) AS MONTH, 
                          COALESCE(TABLEA.YEAR,TABLEF.YEAR) AS YEAR 
                        FROM 
                          (
                            SELECT 
                              COUNT(1) AS NO_OF_PAX, 
                              DATEPART(MONTH, StartDate) AS MONTH, 
                              DATEPART(YEAR, StartDate) AS YEAR 
                            FROM 
                              os_Booking AS B 
                              INNER JOIN os_Agency AS A ON B.AgencyId = A.Id 
                              INNER JOIN os_Customer AS C ON C.BookingId = B.Id 
                            WHERE 
                              B.Status = :P_STATUS 
                              AND A.SaleId = :P_SALES_ID 
                              AND (
                                (
                                  B.StartDate >= :P_FROM_DATE 
                                  AND B.StartDate <= :P_TO_DATE
                                ) 
                                OR (
                                  B.StartDate >= :P_FROM_DATE_SAME_MONTH_LAST_YEAR 
                                  AND B.StartDate <= :P_TO_DATE_SAME_MONTH_LAST_YEAR
                                )
                              ) 
                            GROUP BY 
                              DATEPART(MONTH, B.StartDate), 
                              DATEPART(YEAR, B.StartDate)
                          ) AS TABLEA 
                          INNER JOIN (
                            SELECT 
                              COUNT(1) AS NUMBER_OF_PAX_TOTAL, 
                              DATEPART(MONTH, StartDate) AS MONTH, 
                              DATEPART(YEAR, B.StartDate) AS YEAR 
                            FROM 
                              os_Booking AS B 
                              INNER JOIN os_Agency AS A ON B.AgencyId = A.Id 
                              INNER JOIN os_Customer AS C ON C.BookingId = B.Id 
                            WHERE 
                              B.Status = :P_STATUS 
                              AND (
                                (
                                  B.StartDate >= :P_FROM_DATE 
                                  AND B.StartDate <= :P_TO_DATE
                                ) 
                                OR (
                                  B.StartDate >= :P_FROM_DATE_SAME_MONTH_LAST_YEAR 
                                  AND B.StartDate <= :P_TO_DATE_SAME_MONTH_LAST_YEAR
                                )
                              ) 
                            GROUP BY 
                              DATEPART(MONTH, B.StartDate), 
                              DATEPART(YEAR, B.StartDate)
                          ) AS TABLEB ON TABLEA.MONTH = TABLEB.MONTH 
                          AND TABLEA.YEAR = TABLEB.YEAR 
                          INNER JOIN (
                            SELECT 
                              SUM(
                                CASE WHEN IsVND = 1 THEN TotalVND / CurrencyRate WHEN IsVND = 0 THEN Total END
                              ) AS REVENUE_IN_MONTH, 
                              DATEPART(MONTH, StartDate) AS MONTH, 
                              DATEPART(YEAR, StartDate) AS YEAR 
                            FROM 
                              os_Booking AS B 
                              INNER JOIN os_Agency AS A ON B.AgencyId = A.Id 
                            WHERE 
                              B.Status = :P_STATUS 
                              AND A.SaleId = :P_SALES_ID 
                              AND (
                                (
                                  B.StartDate >= :P_FROM_DATE 
                                  AND B.StartDate <= :P_TO_DATE
                                ) 
                                OR (
                                  B.StartDate >= :P_FROM_DATE_SAME_MONTH_LAST_YEAR 
                                  AND B.StartDate <= :P_TO_DATE_SAME_MONTH_LAST_YEAR
                                )
                              ) 
                            GROUP BY 
                              DATEPART(MONTH, StartDate), 
                              DATEPART(YEAR, StartDate)
                          ) AS TABLEC ON TABLEB.MONTH = TABLEC.MONTH 
                          AND TABLEB.YEAR = TABLEC.YEAR 
                          INNER JOIN (
                            SELECT 
                              SUM(TABLEB.NUMBER_OF_PAX) AS TOP_NUMBER_OF_PAX, 
                              TABLEB.MONTH, 
                              TABLEB.YEAR 
                            FROM 
                              (
                                SELECT 
                                  ROW_NUMBER() OVER(
                                    PARTITION BY MONTH, 
                                    YEAR 
                                    ORDER BY 
                                      TABLEA.NUMBER_OF_PAX DESC
                                  ) AS _ROW, 
                                  TABLEA.NUMBER_OF_PAX, 
                                  TABLEA.MONTH, 
                                  TABLEA.YEAR 
                                FROM 
                                  (
                                    SELECT 
                                      COUNT(1) AS NUMBER_OF_PAX, 
                                      DATEPART(MONTH, StartDate) AS MONTH, 
                                      DATEPART(YEAR, StartDate) AS YEAR 
                                    FROM 
                                      os_Booking AS B 
                                      INNER JOIN os_Agency AS A ON B.AgencyId = A.Id 
                                      INNER JOIN os_Customer AS C ON C.BookingId = B.Id 
                                    WHERE 
                                      B.Status = :P_STATUS 
                                      AND A.SaleId = :P_SALES_ID 
                                      AND (
                                        (
                                          B.StartDate >= :P_FROM_DATE 
                                          AND B.StartDate <= :P_TO_DATE
                                        ) 
                                        OR (
                                          B.StartDate >= :P_FROM_DATE_SAME_MONTH_LAST_YEAR  
                                          AND B.StartDate <= :P_TO_DATE_SAME_MONTH_LAST_YEAR
                                        )
                                      ) 
                                    GROUP BY 
                                      DATEPART(MONTH, B.StartDate), 
                                      DATEPART(YEAR, B.StartDate), 
                                      A.Id
                                  ) AS TABLEA
                              ) AS TABLEB 
                            WHERE 
                              TABLEB._ROW <= 10 
                            GROUP BY 
                              TABLEB.MONTH, 
                              TABLEB.YEAR
                          ) AS TABLED ON TABLEC.MONTH = TABLED.MONTH 
                          AND TABLEC.YEAR = TABLED.YEAR 
                          INNER JOIN (
                            SELECT 
                              COUNT(1) AS NO_OF_BOOKING, 
                              DATEPART(MONTH, B.StartDate) AS MONTH, 
                              DATEPART(YEAR, B.StartDate) AS YEAR 
                            FROM 
                              os_Booking AS B 
                              INNER JOIN os_Agency AS A ON B.AgencyId = A.Id 
                            WHERE 
                              B.Status = :P_STATUS 
                              AND (
                                (
                                  B.StartDate >= :P_FROM_DATE 
                                  AND B.StartDate <= :P_TO_DATE
                                ) 
                                OR (
                                  B.StartDate >= :P_FROM_DATE_SAME_MONTH_LAST_YEAR 
                                  AND B.StartDate <= :P_TO_DATE_SAME_MONTH_LAST_YEAR
                                )
                              ) 
                              AND A.SaleId = :P_SALES_ID 
                            GROUP BY 
                              DATEPART(MONTH, B.StartDate), 
                              DATEPART(YEAR, B.StartDate)
                          ) AS TABLEE ON TABLED.MONTH = TABLEE.MONTH 
                          AND TABLED.YEAR = TABLEE.YEAR FULL 
                          JOIN (
                            SELECT 
                              COUNT(1) AS MEETING_REPORT, 
                              DATEPART(MONTH, A.DateMeeting) AS MONTH, 
                              DATEPART(YEAR, A.DateMeeting) AS YEAR 
                            FROM 
                              os_Activity AS A 
                              INNER JOIN os_Agency AS AGC ON A.Params = AGC.Id 
                            WHERE 
                              (
                                (
                                  A.DateMeeting >= :P_FROM_DATE 
                                  AND A.DateMeeting <= :P_TO_DATE
                                ) 
                                OR (
                                  A.DateMeeting >= :P_FROM_DATE_SAME_MONTH_LAST_YEAR
                                  AND A.DateMeeting <= :P_TO_DATE_SAME_MONTH_LAST_YEAR
                                )
                              ) 
                              AND A.UserId = :P_SALES_ID 
                              AND AGC.SaleId = :P_SALES_ID 
                            GROUP BY 
                              DATEPART(MONTH, A.DateMeeting), 
                              DATEPART(YEAR, A.DateMeeting)
                          ) AS TABLEF ON TABLEE.MONTH = TABLEF.MONTH 
                          AND TABLEE.YEAR = TABLEF.YEAR 
                        ORDER BY 
                          YEAR, 
                          MONTH
                        ";
            var data = _session.CreateSQLQuery(sql)
                .SetParameter("P_SALES_ID", salesId)
                .SetParameter("P_STATUS", StatusType.Approved)
                .SetParameter("P_FROM_DATE", fromDate)
                .SetParameter("P_TO_DATE", toDate)
                .SetParameter("P_FROM_DATE_SAME_MONTH_LAST_YEAR", fromDateSameMonthLastYear)
                .SetParameter("P_TO_DATE_SAME_MONTH_LAST_YEAR", toDateSameMonthLastYear)
                .SetResultTransformer(Transformers.AliasToBean<GeneralInformationDTO>())
                .List<GeneralInformationDTO>().ToList();
            return data;
        }

        public int GetNumberOfPartnerInCharge(int salesId)
        {
            var sql = @"
                        SELECT 
                          COUNT(1) AS NUMBER_OF_PARTNER_IN_CHARGE 
                        FROM 
                          os_Agency 
                        WHERE 
                          SaleId = :P_SALES_ID 
                          AND Deleted = 0
                        ";
            var data = _session.CreateSQLQuery(sql)
                .SetParameter("P_SALES_ID", salesId)
                .List<int>().FirstOrDefault();
            return data;
        }

        public int GetNumberOfMeetingInMonth(DateTime fromDate, DateTime toDate, int salesId)
        {
            var sql = @"
                        SELECT 
                          COUNT(1) AS NUMBER_OF_MEETING_IN_MONTH 
                        FROM 
                          os_Activity 
                        WHERE 
                          UserId = :P_SALES_ID 
                          AND DateMeeting >= :P_FROM_DATE 
                          AND DateMeeting <= :P_TO_DATE";
            var data = _session.CreateSQLQuery(sql)
                .SetParameter("P_SALES_ID", salesId)
                .SetParameter("P_FROM_DATE", fromDate)
                .SetParameter("P_TO_DATE", toDate)
                .List<int>().FirstOrDefault();
            return data;
        }

        public List<TopPartnerAnalysisDTO> GetTopPartnerAnalysis(DateTime fromDate, DateTime toDate, int salesId)
        {
            var sql = @"
                        SELECT 
                          TABLEB.NUMBER_OF_PAX, 
                          TABLEB.AGENCY_ID, 
                          TABLEB.AGENCY_NAME, 
                          TABLEB.MONTH, 
                          TABLEB.YEAR 
                        FROM 
                          (
                            SELECT 
                              ROW_NUMBER() OVER(
                                PARTITION BY MONTH 
                                ORDER BY 
                                  TABLEA.NUMBER_OF_PAX DESC
                              ) AS _ROW, 
                              TABLEA.AGENCY_ID, 
                              TABLEA.AGENCY_NAME, 
                              TABLEA.NUMBER_OF_PAX, 
                              TABLEA.MONTH, 
                              TABLEA.YEAR 
                            FROM 
                              (
                                SELECT 
                                  COUNT(1) AS NUMBER_OF_PAX, 
                                  A.Id AS AGENCY_ID, 
                                  A.Name AS AGENCY_NAME, 
                                  DATEPART(MONTH, StartDate) AS MONTH, 
                                  DATEPART(YEAR, StartDate) AS YEAR 
                                FROM 
                                  os_Booking AS B 
                                  INNER JOIN os_Agency AS A ON B.AgencyId = A.Id 
                                  INNER JOIN os_Customer AS C ON C.BookingId = B.Id 
                                WHERE 
                                  B.Status = 1 
                                  AND A.SaleId = :P_SALES_ID 
                                  AND B.StartDate >= :P_FROM_DATE 
                                  AND B.StartDate <= :P_TO_DATE 
                                GROUP BY 
                                  DATEPART(MONTH, B.StartDate), 
                                  DATEPART(YEAR, B.StartDate), 
                                  A.Id, 
                                  A.Name
                              ) AS TABLEA
                          ) AS TABLEB 
                        WHERE 
                          TABLEB._ROW <= 10 
                        GROUP BY 
                          TABLEB.NUMBER_OF_PAX, 
                          TABLEB.MONTH, 
                          TABLEB.YEAR, 
                          TABLEB.AGENCY_ID, 
                          TABLEB.AGENCY_NAME 
                        ORDER BY 
                          TABLEB.YEAR, 
                          TABLEB.MONTH
                        ";
            var data = _session.CreateSQLQuery(sql)
               .SetParameter("P_SALES_ID", salesId)
               .SetParameter("P_FROM_DATE", fromDate)
               .SetParameter("P_TO_DATE", toDate)
               .SetResultTransformer(Transformers.AliasToBean<TopPartnerAnalysisDTO>())
               .List<TopPartnerAnalysisDTO>().ToList();
            return data;
        }

        public List<PartnerInChargeDTO> GetPartnerInCharge(int salesId)
        {
            var sql = @"SELECT COALESCE( TABLEA.AGENCY_ID, TABLEB.AGENCY_ID ) AS AGENCY_ID, COALESCE( TABLEA.AGENCY_NAME, TABLEB.AGENCY_NAME ) AS AGENCY_NAME, COALESCE( TABLEA.CONTRACT_STATUS, TABLEB.CONTRACT_STATUS ) AS CONTRACT_STATUS, COALESCE( TABLEA.ROLE_NAME, TABLEB.ROLE_NAME ) AS ROLE_NAME, LAST_BOOKING, LAST_MEETING, DETAIL FROM ( SELECT Id AS AGENCY_ID, AGENCY_NAME, ContractStatus AS CONTRACT_STATUS, ROLE_NAME, CreatedDate AS LAST_BOOKING FROM ( SELECT ROW_NUMBER() OVER( PARTITION BY A.Id ORDER BY B.CreatedDate DESC ) AS _ROW, A.Name AS AGENCY_NAME, A.Id, A.ContractStatus, R.name AS ROLE_NAME, B.CreatedDate FROM os_Agency AS A JOIN bitportal_role AS R ON A.RoleId = R.roleid JOIN os_Booking AS B ON B.AgencyId = A.Id WHERE A.SaleId = :P_SALES_ID ) AS TABLA WHERE TABLA._ROW < 2 ) TABLEA FULL JOIN ( SELECT Id AS AGENCY_ID, AGENCY_NAME, ContractStatus AS CONTRACT_STATUS, ROLE_NAME, DateMeeting AS LAST_MEETING, Note AS DETAIL FROM ( SELECT ROW_NUMBER() OVER( PARTITION BY A.Id ORDER BY ACT.DateMeeting DESC ) AS _ROW, A.Name AS AGENCY_NAME, A.Id, A.ContractStatus, R.name AS ROLE_NAME, ACT.DateMeeting, ACT.Note FROM os_Agency AS A JOIN bitportal_role AS R ON A.RoleId = R.roleid JOIN os_Activity AS ACT ON ACT.Params = A.Id WHERE A.SaleId = :P_SALES_ID ) AS TABLA WHERE TABLA._ROW < 2 ) TABLEB ON TABLEA.AGENCY_ID = TABLEB.AGENCY_ID";
            var data = _session.CreateSQLQuery(sql)
                .SetParameter("P_SALES_ID", salesId)
                .SetResultTransformer(Transformers.AliasToBean<PartnerInChargeDTO>())
                .List<PartnerInChargeDTO>().ToList();
            return data;
        }

        public List<NewPartnerDTO> GetNewPartner(int salesId, DateTime fromDate, DateTime toDate)
        {
            var sql = @"SELECT COALESCE( TABLEA.AGENCY_ID, TABLEB.AGENCY_ID ) AS AGENCY_ID, COALESCE( TABLEA.AGENCY_NAME, TABLEB.AGENCY_NAME ) AS AGENCY_NAME, MOST_RECENT_MEETING, LAST_BOOKING FROM ( SELECT Id AS AGENCY_ID, AGENCY_NAME, CreatedDate AS LAST_BOOKING FROM ( SELECT ROW_NUMBER() OVER( PARTITION BY A.Id ORDER BY B.CreatedDate DESC ) AS _ROW, A.Name AS AGENCY_NAME, A.Id, B.CreatedDate FROM os_Agency AS A JOIN bitportal_role AS R ON A.RoleId = R.roleid JOIN os_Booking AS B ON B.AgencyId = A.Id WHERE A.SaleId = :P_SALES_ID AND A.CreatedDate >= :P_FROM_DATE AND A.CreatedDate <= :P_TO_DATE ) AS TABLA WHERE TABLA._ROW < 2 ) TABLEA FULL JOIN ( SELECT Id AS AGENCY_ID, AGENCY_NAME, Note AS MOST_RECENT_MEETING FROM ( SELECT ROW_NUMBER() OVER( PARTITION BY A.Id ORDER BY ACT.DateMeeting DESC ) AS _ROW, A.Name AS AGENCY_NAME, A.Id, ACT.Note FROM os_Agency AS A JOIN bitportal_role AS R ON A.RoleId = R.roleid JOIN os_Activity AS ACT ON ACT.Params = A.Id WHERE A.SaleId = :P_SALES_ID AND A.CreatedDate >= :P_FROM_DATE AND A.CreatedDate <= :P_TO_DATE AND ACT.UserId = :P_SALES_ID ) AS TABLA WHERE TABLA._ROW < 2 ) TABLEB ON TABLEA.AGENCY_ID = TABLEB.AGENCY_ID"; 
            var data = _session.CreateSQLQuery(sql)
                 .SetParameter("P_SALES_ID", salesId)
                 .SetParameter("P_FROM_DATE", fromDate)
                 .SetParameter("P_TO_DATE", toDate)
                 .SetResultTransformer(Transformers.AliasToBean<NewPartnerDTO>())
                 .List<NewPartnerDTO>().ToList();
            return data;
        }
    }
}