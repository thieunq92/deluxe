using CMS.Core.Domain;
using NHibernate;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.BusinessLogic
{
    public class DashBoardManagerBLL
    {
        public UserRepository UserRepository { get; set; }
        public BookingRepository BookingRepository { get; set; }
        public ActivityRepository ActivityRepository { get; set; }
        public ExpenseServiceRepository ExpenseServiceRepository { get; set; }
        public AgencyRepository AgencyRepository { get; set; }
        public CustomerRepository CustomerRepository { get; set; }
        public CampaignRepository CampaignRepository { get; set; }
        public DashBoardManagerBLL()
        {
            UserRepository = new UserRepository();
            BookingRepository = new BookingRepository();
            ActivityRepository = new ActivityRepository();
            ExpenseServiceRepository = new ExpenseServiceRepository();
            AgencyRepository = new AgencyRepository();
            CustomerRepository = new CustomerRepository();
            CampaignRepository = new CampaignRepository();
        }
        public void Dispose()
        {
            if (UserRepository != null)
            {
                UserRepository.Dispose();
                UserRepository = null;
            }
            if (BookingRepository != null)
            {
                BookingRepository.Dispose();
                BookingRepository = null;
            }
            if (ActivityRepository != null)
            {
                ActivityRepository.Dispose();
                ActivityRepository = null;
            }
            if (AgencyRepository != null)
            {
                AgencyRepository.Dispose();
                AgencyRepository = null;
            }
            if (CustomerRepository != null)
            {
                CustomerRepository.Dispose();
                CustomerRepository = null;
            }
            if (CampaignRepository != null)
            {
                CampaignRepository.Dispose();
                CampaignRepository = null;
            }
            if (ExpenseServiceRepository != null)
            {
                ExpenseServiceRepository.Dispose();
                ExpenseServiceRepository = null;
            }
        }

        public IList<ExpenseService> ExpenseServiceGetAllTodayByTrip(SailsTrip trip)
        {
            return ExpenseServiceRepository.ExpenseServiceGetAllTodayByTrip(trip);
        }

        public IEnumerable<Booking> BookingGetAllNewBookings(DateTime date, List<Organization> organizations, User sales)
        {
            return BookingRepository.BookingGetAllNewBookings(date, organizations, sales);
        }

        public IEnumerable<Booking> BookingGetAllCancelledBookingOnDate(DateTime date, List<Organization> organizations, User sales)
        {
            return BookingRepository.BookingGetAllCancelledBookingOnDate(date, organizations, sales);
        }

        public object AgencyGetTop10(List<Organization> organizations, int year, int month)
        {
            return AgencyRepository.AgencyGetTop10(organizations, year, month);
        }

        public object CustomerGetCountByTripAndStartDate(List<SailsTrip> trips, List<DateTime> dates)
        {
            return CustomerRepository.CustomerGetCountByTripsAndDate(trips, dates);
        }

        public object CustomerGetCountByBookings(List<Booking> bookings)
        {
            return CustomerRepository.CustomerGetCountByBookings(bookings);
        }

        public object GetMonthSummary(int month, int year, List<User> saleses)
        {
            return UserRepository.GetMonthSummary(month, year, saleses);
        }

        public object BookingGetAllByEventCodes(List<Web.Admin.EventCode> tripCodes)
        {
            return BookingRepository.BookingGetAllByEventCodes(tripCodes);
        }

        public object CampaignGetAll()
        {
            return CampaignRepository.CampaignGetAll();
        }
    }
}