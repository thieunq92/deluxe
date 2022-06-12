using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Core.Domain;

namespace Portal.Modules.OrientalSails.BusinessLogic
{
    public class DashBoardOperationBLL
    {
        public ExpenseServiceRepository ExpenseServiceRepository { get; set; }
        public BookingRepository BookingRepository { get; set; }
        public CustomerRepository CustomerRepository { get; set; }
        public TripCode_NoteRepository TripCode_NoteRepository { get; set; }
        public RoleRepository RoleRepository { get; set; }
        public DashBoardOperationBLL()
        {
            ExpenseServiceRepository = new ExpenseServiceRepository();
            BookingRepository = new BookingRepository();
            CustomerRepository = new CustomerRepository();
            TripCode_NoteRepository = new TripCode_NoteRepository();
            RoleRepository = new RoleRepository();
        }

        public void Dispose()
        {
            if (ExpenseServiceRepository != null)
            {
                ExpenseServiceRepository.Dispose();
                ExpenseServiceRepository = null;
            }
            if (BookingRepository != null)
            {
                BookingRepository.Dispose();
                BookingRepository = null;
            }
            if (CustomerRepository != null)
            {
                CustomerRepository.Dispose();
                CustomerRepository = null;
            }
            if (TripCode_NoteRepository != null)
            {
                TripCode_NoteRepository.Dispose();
                TripCode_NoteRepository = null;
            }
            if (RoleRepository != null)
            {
                RoleRepository.Dispose();
                RoleRepository = null;
            }
        }

        public IList<ExpenseService> ExpeseServiceGetAllTodayByTrip(SailsTrip trip)
        {
            return ExpenseServiceRepository.ExpenseServiceGetAllTodayByTrip(trip);
        }
        public IList<ExpenseService> ExpeseServiceGetAllTodayByTripAndDate(SailsTrip trip, DateTime date)
        {
            return ExpenseServiceRepository.ExpenseServiceGetAllTodayByTripAndDate(trip, date);
        }

        public object BookingGetAllByEventCodes(List<Web.Admin.EventCode> tripCodes)
        {
            return BookingRepository.BookingGetAllByEventCodes(tripCodes);
        }

        public object CustomerGetCountByBookings(List<Booking> bookings)
        {
            return CustomerRepository.CustomerGetCountByBookings(bookings);
        }

        public void TripCode_NoteSaveOrUpdate(TripCode_Note tripCode_Note)
        {
            TripCode_NoteRepository.SaveOrUpdate(tripCode_Note);
        }
        public List<TripCode_Note> TripCode_NoteGetAllByTripCode(string tripCode)
        {
            return TripCode_NoteRepository.TripCode_NoteGetAllByTripCode(tripCode);
        }

        public Role RoleGetById(int roleId)
        {
            return RoleRepository.GetById(roleId);
        }
    }
}