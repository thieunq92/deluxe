using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using CMS.Core;
using CMS.Core.Communication;
using CMS.Core.DataAccess;
using CMS.Core.Domain;
using CMS.Core.Service.Email;
using NHibernate;
using NHibernate.Criterion;
using Portal.Modules.OrientalSails.DataAccess;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.Util;
using CMS.Core.Service;
using System.Web;
using CMS.Web.Util;
using CMS.Web.Components;

namespace Portal.Modules.OrientalSails
{
    public partial class SailsModule : ModuleBase, INHibernateModule, IActionProvider, IActionConsumer
    {
        #region -- Const --

        #region -- Cấu hình cứng --
        public const int TRIP1 = 2;
        public const int TRIP2 = 3;
        public const int DOUBLE = 2;
        public const int TWIN = 3;
        public const int DOUBLE_COUNT = 6;
        public const int TWIN_COUNT = 10;
        public const int TRANSFER = 1;
        #endregion

        #region -- Thông tin chung --

        public const string ANONYMOUS = "Anonymous";
        public const string NOAGENCY = "ATM Sales team";
        public const string IMPORTANT = "#FF7F7F";
        public const string WARNING = "#FFFF00";
        public const string GOOD = "#92D050";

        public const string TICKET = "TICKET";
        public const string MEAL = "MEAL";
        public const string KAYAKING = "KAYAKING";
        public const string SERVICE = "SERVICE";
        public const string RENT = "RENT";

        public const string ADD_BK_CUSTOMPRICE = "ADD_BK_CUSTOMPRICE";
        public const string ROOM_CUSTOMPRICE = "ROOM_CUSTOMPRICE";
        public const string PARTNERSHIP = "PARTNERSHIP";
        public const string ACCOUNT_STATUS = "ACCOUNT_STATUS";
        public const string CHECK_CHARTER = "CHECK_CHARTER";
        public const string SHOW_EXPENSE_BY_DATE = "SHOW_EXPENSE_BY_DATE";
        public const string BAR_REVENUE = "BAR_REVENUE";
        public const string NO_AGENCY_BK = "NO_AGENCY_BK";
        public const string DETAIL_SERVICE = "DETAIL_SERVICE";
        public const string OVERALL_EXPENSE = "OVERALL_EXPENSE";
        public const string USE_VND_EXPENSE = "USE_VND_EXPENSE";
        public const string APPROVED_DEFAULT = "APPROVED_DEFAULT";
        public const string PUREQUIRED = "PUREQUIRED";
        public const string PERIOD_EXPENSE_AVG = "PERIOD_EXPENSE_AVG";
        public const string APPROVED_LOCK = "APPROVED_LOCK";
        public const string CUSTOMER_PRICE = "CUSTOMER_PRICE";

        public const int GUIDE_COST = 20;
        public const int OPERATOR = 4;
        public const int TRANSPORT = 19;
        public const int DAYBOAT = 3;
        public const int HAIPHONG = 11;

        public const int HOTEL = 18;
        #endregion

        #region -- Thông tin tham số --
        public const string ACTION_VIEW_TRIP_PARAM = "Trip";
        private const string ACTION_VIEW_TRIP_DETAIL_PATH = "Modules/Sails/TripDetail.ascx";
        private const string ACTION_VIEW_TRIP_LIST_PATH = "Modules/Sails/TripList.ascx";
        private const string ACTION_SELECT_ROOM_PATH = "Modules/Sails/SelectRooms.ascx";
        private const string ACTION_CUSTOMER_INFO_PATH = "Modules/Sails/CustomersInfo.ascx";
        private const string ACTION_BOOKING_FINISH_PATH = "Modules/Sails/BookingFinish.ascx";
        private const string ACTION_PREFERED_ROOM_PATH = "Modules/Sails/PreferedRooms.ascx";
        public const string ACTION_ORDER_PARAM = "SelectRoom";
        public const string ACTION_CUSTOMER_INFO_PARAM = "CustomerInfo";
        public const string ACTION_PREFERED_ROOM_PARAM = "PreferedRooms";
        public const string ACTION_BOOKING_FINISH_PARAM = "Finish";
        private string CURRENT_ACTION_PARAM;
        #endregion
        #endregion

        #region -- Private Member --

        private readonly ICommonDao _commonDao;
        private readonly ISailsDao _sailsDao;
        private readonly IEmailSender _emailSender;
        private int _tripId;
        private int _optionId;

        #endregion

        #region -- Constructor --
        public static SailsModule GetInstance()
        {
            CoreRepository CoreRepository = HttpContext.Current.Items["CoreRepository"] as CoreRepository;
            int nodeId = 1;
            Node node = (Node)CoreRepository.GetObjectById(typeof(Node), nodeId);
            int sectionId = 15;
            Section section = (Section)CoreRepository.GetObjectById(typeof(Section), sectionId);
            SailsModule module = (SailsModule)ContainerAccessorUtil.GetContainer().Resolve<ModuleLoader>().GetModuleFromSection(section);
            return module;

        }
        public SailsModule(ICommonDao commonDao, ISailsDao sailsDao, IEmailSender emailSender)
        {
            _sailsDao = sailsDao;
            _commonDao = commonDao;
            _emailSender = emailSender;
        }

        #endregion

        #region -- Override Method --

        public override string CurrentViewControlPath
        {
            get
            {
                switch (CURRENT_ACTION_PARAM)
                {
                    case ACTION_VIEW_TRIP_PARAM:
                        if (_tripId > 0)
                        {
                            return ACTION_VIEW_TRIP_DETAIL_PATH;
                        }
                        if (_tripId == 0)
                        {
                            return ACTION_VIEW_TRIP_LIST_PATH;
                        }
                        //return DefaultViewControlPath;
                        return ACTION_VIEW_TRIP_LIST_PATH;
                    case ACTION_ORDER_PARAM:
                        return ACTION_SELECT_ROOM_PATH;
                    case ACTION_CUSTOMER_INFO_PARAM:
                        return ACTION_CUSTOMER_INFO_PATH;
                    case ACTION_BOOKING_FINISH_PARAM:
                        return ACTION_BOOKING_FINISH_PATH;
                    case ACTION_PREFERED_ROOM_PARAM:
                        return ACTION_PREFERED_ROOM_PATH;
                    default :
                        return ACTION_VIEW_TRIP_LIST_PATH;
                }
            }
        }

        protected override void ParsePathInfo()
        {
            base.ParsePathInfo();
            _tripId = -1;
            _optionId = 0;
            if(ModuleParams.Length>0)
            {
                switch (ModuleParams[0])
                {
                    case ACTION_VIEW_TRIP_PARAM:
                        CURRENT_ACTION_PARAM = ACTION_VIEW_TRIP_PARAM;
                        if (ModuleParams.Length >= 2)
                        {
                            _tripId = Convert.ToInt32(ModuleParams[1]);
                        }
                        break;
                    case ACTION_ORDER_PARAM:
                        CURRENT_ACTION_PARAM = ACTION_ORDER_PARAM;
                        if (ModuleParams.Length>=3)
                        {
                            _tripId = Convert.ToInt32(ModuleParams[1]);
                            _optionId = Convert.ToInt32(ModuleParams[2]);
                        }
                        break;
                    case ACTION_CUSTOMER_INFO_PARAM:
                        CURRENT_ACTION_PARAM = ACTION_CUSTOMER_INFO_PARAM;
                        if (ModuleParams.Length >= 3)
                        {
                            _tripId = Convert.ToInt32(ModuleParams[1]);
                            _optionId = Convert.ToInt32(ModuleParams[2]);
                        }
                        break;
                    case ACTION_BOOKING_FINISH_PARAM:
                        CURRENT_ACTION_PARAM = ACTION_BOOKING_FINISH_PARAM;
                        break;
                    case ACTION_PREFERED_ROOM_PARAM:
                        CURRENT_ACTION_PARAM = ACTION_PREFERED_ROOM_PARAM;
                        if (ModuleParams.Length >= 3)
                        {
                            _tripId = Convert.ToInt32(ModuleParams[1]);
                            _optionId = Convert.ToInt32(ModuleParams[2]);
                        }
                        break;
                    default:
                        CURRENT_ACTION_PARAM = ACTION_VIEW_TRIP_PARAM;
                        break;
                }
            }
            else
            {
                CURRENT_ACTION_PARAM = ACTION_VIEW_TRIP_PARAM;
            }
        }

        public override void ReadSectionSettings()
        {
            base.ReadSectionSettings();
            //_babyPrice = Convert.ToInt32(Section.Settings["BABY_PRICE"]);
            //_childPrice = Convert.ToInt32(Section.Settings["CHILD_PRICE"]);
            //_adultPrice = Convert.ToInt32(Section.Settings["ADULT_PRICE"]);
        }

        public void Send(MailMessage message)
        {
            _emailSender.Send(message);
        }

        #endregion

        #region -- Public Properties --

        public int TripId
        {
            get { return _tripId; }
        }

        public TripOption TripOption
        {
            get
            {
                switch (_optionId)
                {
                    case 2:
                        return TripOption.Option2;
                    case 3:
                        return TripOption.Option3;
                    default:
                        return TripOption.Option1;
                }
            }
        }

        #endregion

        #region -- Session Method --
        public void SaveRoomCountData()
        {
            
        }
        #endregion

        #region Implementation of IActionProvider

        public ActionCollection GetOutboundActions()
        {
            ActionCollection outboundActions = new ActionCollection();
            outboundActions.Add(new CMS.Core.Communication.Action("Sails", new string[0]));
            return outboundActions;
        }

        #endregion

        #region Implementation of IActionConsumer

        public ActionCollection GetInboundActions()
        {
            ActionCollection inboundActions = new ActionCollection();
            inboundActions.Add(new CMS.Core.Communication.Action("Sails", new string[0]));
            return inboundActions;
        }

        #endregion

        /// <summary>
        /// Trả về mã booking đã format
        /// </summary>
        /// <param name="booking"></param>
        /// <returns></returns>
        public string BookingCode(Booking booking)
        {
            return string.Format("ATM{0:00000}", booking.Id);
        }

        //Debugging

        public T GetObject<T>(int id)
        {
            using (var session = _commonDao.OpenSession())
            {
                return session.Get<T>(id);
            }
        }

        public int CountObjet<T>(ICriterion criterion)
        {
            using (var session = _commonDao.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(T));

                criteria.Add(criterion);

                criteria.SetProjection(Projections.RowCount());

                var list = criteria.List();
                if (list.Count > 0 && list[0] != null)
                {
                    return (int)list[0];
                }
                return 0;
            }
        }

        public IList<T> GetObject<T>(ICriterion criterion, int pageSize, int pageIndex, params Order[] orders)
        {
            using (var session = _commonDao.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(T));
                if (criterion != null)
                    criteria.Add(criterion);

                if (pageSize > 0)
                {
                    if (pageIndex < 0) pageIndex = 0;
                    criteria.SetFirstResult(pageSize * pageIndex);
                    criteria.SetMaxResults(pageSize);
                }

                foreach (Order order in orders)
                {
                    if (order != null)
                    {
                        criteria.AddOrder(order);
                    }
                }

                return criteria.List<T>();
            }
        }


        public void SaveOrUpdate(ExpenseService e)
        {
            if (e.Id > 0)
            {
                _commonDao.UpdateObject(e);
                return;
            }
            _commonDao.SaveOrUpdateObject(e);
        }

        public Organization OrganizationGetById(int id)
        {
            return _commonDao.GetObjectById(typeof (Organization), id) as Organization;
        }

        public IList OrganizationGetAllRoot()
        {
            return _commonDao.GetObjectByCriterion(typeof(Organization),Expression.IsNull("Parent"));
        }

        public IList OrganizationGetByUser(User user)
        {
            var list = _commonDao.GetObjectByCriterion(typeof(UserOrganization), Expression.Eq("User", user));
            return list;
        }
    }
}