﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.Enums
{
    public enum PermissionEnum
    {
        FORM_BOOKINGREPORTPERIODALL = 2,
        FORM_BOOKINGREPORTPERIOD = 3,
        FORM_ORDERREPORT = 4,
        FORM_TRACKINGREPORT = 5,
        FORM_INCOMEREPORT = 6,
        FORM_PAYMENTREPORT = 7,
        FORM_EXPENSEREPORT = 8,
        FORM_PAYABLELIST = 9,
        FORM_BALANCEREPORT = 10,
        FORM_AGENCYEDIT = 11,
        FORM_AGENCYLIST = 12,
        FORM_AGENTLIST = 13,
        FORM_SAILSTRIPEDIT = 14,
        FORM_SAILSTRIPLIST = 15,
        FORM_CRUISESEDIT = 16,
        FORM_CRUISESLIST = 17,
        FORM_ROOMCLASSEDIT = 18,
        FORM_ROOMTYPEXEDIT = 19,
        FORM_ROOMEDIT = 20,
        FORM_ROOMLIST = 21,
        FORM_EXTRAOPTIONEDIT = 22,
        FORM_COSTING = 23,
        FORM_CRUISECONFIG = 24,
        FORM_EXCHANGERATE = 25,
        FORM_COSTTYPES = 26,
        FORM_ADDBOOKING = 27,
        FORM_BOOKINGLIST = 28,
        ACTION_EXPORTCONGNO = 29,
        ACTION_EXPORTSELFSALES = 30,
        ACTION_EXPORTREVENUE = 31,
        ACTION_EXPORTREVENUEBYSALE = 32,
        ACTION_EXPORTAGENCY = 33,
        FORM_EXPENSEPERIOD = 34,
        FORM_AGENCYSELECTORPAGE = 35,
        VIEW_ALLBOOKINGRECEIVABLE = 36,
        FORM_BOOKINGPAYMENT = 37,
        FORM_RECEIVABLETOTAL = 38,
        ACTION_EDITAGENCY = 39,
        LOCK_INCOME = 41,
        EDIT_AFTER_LOCK = 42,
        EDIT_TOTAL = 43,
        VIEW_ALL_MEETING = 44,
        AllowLockDate = 45,
        AllowUnlockDate = 46,
        AllowAccessBookingByDatePage = 47,
        AllowAddDailyExpense = 48,
        AllowEditDailyExpense = 49,
        AllowDeleteDailyExpense = 50,
        AllowAccessBookingView = 51,
        AllowEditTotalBooking = 52,
        AllowLockIncomeBooking = 53,
        AllowLockBooking = 54,
        DASHBOARD_ACCESS = 57,
        DASHBOARDMANAGER_ACCESS = 58,
        AllowAccessDashBoardOperationPage = 59,
    }
}