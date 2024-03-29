using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using Castle.Services.Transaction;
using CMS.Core;
using CMS.Core.Communication;
using CMS.Core.DataAccess;
using CMS.Core.Domain;
using CMS.Core.Util;
using CMS.Modules.Gallery.Domain;
using CMS.Modules.TourManagement.DataAccess;
using CMS.Modules.TourManagement.Domain;
using NHibernate.Criterion;

namespace CMS.Modules.TourManagement
{
    [Transactional]
    public class TourManagementModule : TourModuleBase, INHibernateModule, IActionProvider, IActionConsumer
    {
        #region -- CONSTANTS --

        public const string ACTION_LIST = "Listing";
        public const string ACTION_HOTEL_CONFIG = "HotelConfig";
        public const string ACTION_RESTAURANT_CONFIG = "RestaurantConfig";
        public const string ACTION_GUIDE_CONFIG = "GuideConfig";
        public const string ACTION_TRANSPORT_CONFIG = "TransportConfig";
        public const string ACTION_BOAT_CONFIG = "BoatConfig";
        public const string ACTION_ENTRANCEFEE_CONFIG = "EntranceFeeConfig";
        public const int TITLECOLUMN = 2;

        public string ThemePath = "/Modules/TourManagement/Theme/Standard/";
        #endregion

        #region -- PROPERTIES --
        public Section HotelSection
        {
            get { return Section.Connections[ACTION_HOTEL_CONFIG] as Section; }
        }

        public Section RestaurantSection
        {
            get { return Section.Connections[ACTION_RESTAURANT_CONFIG] as Section; }
        }

        public Section GuideSection
        {
            get { return Section.Connections[ACTION_GUIDE_CONFIG] as Section; }
        }

        public Section TransportSection
        {
            get { return Section.Connections[ACTION_TRANSPORT_CONFIG] as Section; }
        }

        public Section LandscapeSection
        {
            get { return Section.Connections[ACTION_ENTRANCEFEE_CONFIG] as Section; }
        }


        public Section BoatSection
        {
            get { return Section.Connections[ACTION_BOAT_CONFIG] as Section; }
        }

        public Section ListSection
        {
            get { return Section.Connections[ACTION_LIST] as Section; }
        }

        public Section LocationGallery
        {
            get { return Section.Connections["Album"] as Section; }
        }

        #endregion

        #region -- PRIVATE MEMBERS --

        private readonly ICommonDao _commonDao;
        private readonly ITourDao _tourDao;
        private bool _isTour;
        private int _tourId;

        public int TourId
        {
            get { return _tourId;}
        }

        #endregion

        #region -- CONSTRUCTORS --

        public TourManagementModule(ICommonDao commonDao, ITourDao hotelDao)
        {
            _tourDao = hotelDao;
            _commonDao = commonDao;
        }

        #endregion

        #region -- Data Access --

        #region -- Location --

        /// <summary>
        /// Lưu thông tin địa điểm
        /// </summary>
        /// <param name="location">địa điểm cần lưu</param>        
        public void Save(Location location)
        {
            _commonDao.SaveObject(location);
        }

        /// <summary>
        /// Cập nhật thông tin địa điểm
        /// </summary>
        /// <param name="location">địa điểm cần lưu</param>
        public void Update(Location location)
        {
            _commonDao.UpdateObject(location);
        }

        /// <summary>
        /// Xóa thông tin địa điểm
        /// </summary>
        /// <param name="location">địa điểm cần xóa</param>
        public void Delete(Location location)
        {
            _commonDao.DeleteObject(location);
        }

        /// <summary>
        /// Lấy danh sách toàn bộ các địa điểm
        /// </summary>
        /// <returns>Danh sách địa điểm</returns>
        public IList LocationGetAll()
        {
            return _commonDao.GetAll(typeof (Location));
        }

        /// <summary>
        /// Lấy về location theo id cho trước
        /// </summary>
        /// <param name="id">Id của location lấy về</param>
        /// <returns>Location</returns>
        public Location LocationGetById(int id)
        {
            return (Location) _commonDao.GetObjectById(typeof (Location), id);
        }

        public IList LocationGetAllByLevel(int level)
        {
            return _tourDao.LocationGetRoot();
        }

        public IList LocationGetByParentID(int parentID)
        {
            return _tourDao.LocationGetByParentID(parentID);
        }

        /// <summary>
        /// Lấy tất cả các location mà không có cha.
        /// </summary>
        /// <returns></returns>
        public IList LocationGetRoot()
        {
            return _tourDao.LocationGetRoot();
        }

        /// <summary>
        /// Sắp xếp lại location
        /// </summary>
        public void Resort(Location NewlyLocation, bool isUp)
        {
            _tourDao.Resort(NewlyLocation, isUp);
        }

        #endregion

        #region -- AgencyPolicy --

        /// <summary>
        /// Lưu thông tin loại đại lý
        /// </summary>
        /// <param name="agency">loại đại lý cần lưu</param>
        public void Save(AgencyPolicy agency)
        {
            _commonDao.SaveObject(agency);
        }

        /// <summary>
        /// Cập nhật thông tin loại đại lý
        /// </summary>
        /// <param name="agency">loại đại lý cần lưu</param>
        public void Update(AgencyPolicy agency)
        {
            _commonDao.UpdateObject(agency);
        }

        /// <summary>
        /// Xóa thông tin loại đại lý
        /// </summary>
        /// <param name="agency">loại đại lý cần xóa</param>
        public void Delete(AgencyPolicy agency)
        {
            _commonDao.DeleteObject(agency);
        }

        /// <summary>
        /// Lấy danh sách toàn bộ các loại đại lý
        /// </summary>
        /// <returns>Danh sách loại đại lý</returns>
        public IList AgencyPolicyGetAll(string moduleType)
        {
            return _tourDao.AgencyPolicyGetAll(moduleType);
        }

        /// <summary>
        /// Lấy về đại lý theo id cho trước
        /// </summary>
        /// <param name="id">Id của đại lý lấy về</param>
        /// <returns>AgencyPolicy</returns>
        public AgencyPolicy AgencyPolicyGetById(int id)
        {
            return (AgencyPolicy) _commonDao.GetObjectById(typeof (AgencyPolicy), id);
        }

        /// <summary>
        /// Lấy toàn bộ các nhóm người dùng
        /// </summary>
        /// <returns></returns>
        public IList RoleGetAll()
        {
            return _commonDao.GetAll(typeof (Role));
        }

        /// <summary>
        /// Lấy role theo id
        /// </summary>
        /// <returns></returns>
        public Role RoleGetById(int id)
        {
            return _commonDao.GetObjectById(typeof (Role), id) as Role;
        }

        /// <summary>
        /// Lấy về đại lý theo role cho trước
        /// </summary>
        /// <param name="role">Role đại lý lấy về</param>
        /// <param name="moduleType"></param>
        /// <returns>AgencyPolicy</returns>
        public IList AgencyPolicyGetByRole(Role role, string moduleType)
        {
            return _tourDao.AgencyPolicyGetByRole(role, moduleType);
        }

        #endregion

        #region -- Currency --

        public IList CurrencyGetAll()
        {
            return _tourDao.CurrencyGetAll();
        }

        public void Save(Currency currency)
        {
            _commonDao.SaveObject(currency);
        }

        public Currency CurrencyGetById(int id)
        {
            return _commonDao.GetObjectById(typeof (Currency), id) as Currency;
        }

        public void Update(Currency currency)
        {
            _commonDao.SaveOrUpdateObject(currency);
        }

        public void Delete(Currency currency)
        {
            _tourDao.Delete(currency);
        }

        #endregion

        #region -- Provider --

        /// <summary>
        /// Thêm mới 1 provider
        /// </summary>
        /// <param name="provider"></param>
        public void Save(Provider provider)
        {
            provider.CreatedDate = DateTime.Now;
            provider.ModifiedDate = DateTime.Now;
            _commonDao.SaveObject(provider);
        }

        /// <summary>
        /// Chỉnh sửa 1 provider
        /// </summary>
        /// <param name="provider"> provider cần sửa</param>
        public void Update(Provider provider)
        {
            provider.ModifiedDate = DateTime.Now;
            _commonDao.UpdateObject(provider);
        }

        /// <summary>
        /// Xóa 1 provider
        /// </summary>
        /// <param name="provider">Provider cần xóa</param>
        public void Delete(Provider provider)
        {
            provider.ModifiedDate = DateTime.Now;
            provider.Deleted = true;
            _commonDao.UpdateObject(provider);
        }

        /// <summary>
        /// Lấy 1 provider theo ID
        /// </summary>
        /// <param name="providerID">ID của provider cần lấy</param>
        /// <returns></returns>
        public Provider ProviderGetByID(int providerID)
        {
            return _commonDao.GetObjectById(typeof (Provider), providerID) as Provider;
        }

        /// <summary>
        /// Lấy về danh sách tất cả các provider
        /// </summary>
        /// <returns></returns>
        public IList ProviderGetAll()
        {
            return _tourDao.ProviderGetAll();
        }

        public IList ProviderGetAll(ProviderType type)
        {
            return _tourDao.ProviderGetAll(type);
        }

        /// <summary>
        /// Tìm kiếm Provider theo query string
        /// </summary>
        /// <param name="queryString">Query string</param>
        /// <returns></returns>
        public IList ProviderSearchFromQueryString(NameValueCollection queryString)
        {
            ICriterion criterion = Expression.Eq(Provider.DELETED, false);
            int i;

            #region -Name-

            if (!string.IsNullOrEmpty(queryString["Name"]))
            {
                criterion = Expression.And(criterion,
                                           Expression.Like(Provider.NAME, queryString["Name"],
                                                           MatchMode.Anywhere));
            }

            #endregion

            #region -Location-

            if (!string.IsNullOrEmpty(queryString["Location"]) && Int32.TryParse(queryString["Location"], out i))
            {
                criterion = Expression.And(criterion, Expression.Eq(Provider.LOCATION, LocationGetById(i)));
            }

            #endregion

            #region -Order-

            Order order = null;
            if (!string.IsNullOrEmpty(queryString["Order"]))
            {
                order = new Order(queryString["Order"].Substring(3),
                                  queryString["Order"].Substring(0, 3).ToLower() == "asc");
            }

            #endregion

            #region -- Provider Type --
            if (!string.IsNullOrEmpty(queryString["ProviderType"]))
            {
                criterion = Expression.And(criterion,
                                           Expression.Eq(Provider.PROVIDERTYPE,
                                                         Enum.Parse(typeof (ProviderType), queryString["ProviderType"])));
            }
            #endregion

            return _tourDao.ProviderSearch(criterion, order);
        }

        public IList ProviderCategoryGetAll()
        {
            return _commonDao.GetAll(typeof (ProviderCategory));
        }

        public ProviderCategory ProviderCategoryGetById(int id)
        {
            return _commonDao.GetObjectById(typeof (ProviderCategory), id) as ProviderCategory;
        }

        public void SaveOrUpdate(ProviderCategory category)
        {
            _commonDao.SaveOrUpdateObject(category);
        }

        public void Delete(ProviderCategory category)
        {
            _commonDao.DeleteObject(category);
        }

        public IList Provider_CategoryGetByProvider(Provider provider)
        {
            return _commonDao.GetObjectByProperty(typeof (Provider_Category), "Provider", provider);
        }

        public void Delete(Provider_Category link)
        {
            _commonDao.DeleteObject(link);
        }

        public Provider_Category Provider_CategoryGetById(int id)
        {
            return _commonDao.GetObjectById(typeof (Provider_Category), id) as Provider_Category;
        }

        public void SaveProviderCategory(Provider provider, ProviderCategory category)
        {
            Provider_Category link = new Provider_Category();
            link.Provider = provider;
            link.Category = category;
            _commonDao.SaveObject(link);
        }
        #endregion

        #region -- TourType --
        public IList TourTypeGetAll()
        {
            return _commonDao.GetAll(typeof (TourType));
        }

        public IList TourTypeGetAllRoot()
        {
            return _commonDao.GetObjectByCriterion(typeof (TourType), Expression.IsNull("Parent"));
        }

        public TourType TourTypeGetById(int id)
        {
            return _commonDao.GetObjectById(typeof (TourType), id) as TourType;
        }

        public void Save(TourType tourType)
        {
            _commonDao.SaveObject(tourType);
        }

        public void Update(TourType tourType)
        {
            _commonDao.UpdateObject(tourType);
        }

        public void Delete(TourType tourType)
        {
            _commonDao.DeleteObject(tourType);
        }
        #endregion

        #region -- Tour --
        public IList TourGetAll()
        {
            return _commonDao.GetAll(typeof (Tour));
        }

        public Tour TourGetById(int id)
        {
            return _commonDao.GetObjectById(typeof (Tour), id) as Tour;
        }

        public void Save(Tour tour)
        {
            tour.CreatedDate = DateTime.Now;
            tour.ModifiedDate = DateTime.Now;
            _commonDao.SaveObject(tour);
        }

        public void Update(Tour tour)
        {
            tour.ModifiedDate = DateTime.Now;
            _commonDao.UpdateObject(tour);
        }

        public void Delete(Tour tour)
        {
            tour.Deleted = true;
            _commonDao.UpdateObject(tour);
        }

        public IList TourSearchByQueryString(NameValueCollection queryString)
        {
            ICriterion criterion = Expression.Eq("Deleted", false);
            criterion = Expression.And(criterion,
                                       Expression.Or(Expression.IsNull(Tour.PACKAGESTATUS),
                                                     Expression.Not(Expression.Eq(Tour.PACKAGESTATUS,
                                                                                  PackageStatus.PartPackage))));
            if (!string.IsNullOrEmpty(queryString["Name"]))
            {
                criterion = Expression.And(criterion, Expression.Like("Name", queryString["Name"], MatchMode.Anywhere));
            }


            #region -- Location --

            Location start = null;
            if (!string.IsNullOrEmpty(queryString["StartId"]) && Convert.ToInt32(queryString["StartId"]) > 0)
            {
                start = LocationGetById(Convert.ToInt32(queryString["StartId"]));
            }

            Location end = null;
            if (!string.IsNullOrEmpty(queryString["EndId"]) && Convert.ToInt32(queryString["EndId"]) > 0)
            {
                end = LocationGetById(Convert.ToInt32(queryString["EndId"]));
            }

            if (start!=null)
            {
                switch (start.Level)
                {
                    case 3:
                        criterion = Expression.And(criterion, Expression.Eq("CountryStart", start));
                        break;
                    case 4:
                        criterion = Expression.And(criterion, Expression.Eq("RegionStart", start));
                        break;
                    case 5:
                        criterion = Expression.And(criterion, Expression.Eq("CityStart", start));
                        break;
                    default: //Chưa sử lý nếu Level>5. Để mặc định load ra Location
                        criterion = Expression.And(criterion, Expression.Eq("StartFrom", start));
                        break;
                }
            }

            if (end != null)
            {
                switch (end.Level)
                {
                    case 3:
                        criterion = Expression.And(criterion, Expression.Eq("CountryEnd", end));
                        break;
                    case 4:
                        criterion = Expression.And(criterion, Expression.Eq("RegionEnd", end));
                        break;
                    case 5:
                        criterion = Expression.And(criterion, Expression.Eq("CityEnd", end));
                        break;
                    default: //Chưa sử lý nếu Level>5. Để mặc định load ra Location
                        criterion = Expression.And(criterion, Expression.Eq("EndIn", end));
                        break;
                }
            }

            #endregion

            if (!string.IsNullOrEmpty(queryString["Type"]))
            {
                TourType type = TourTypeGetById(Convert.ToInt32(queryString["Type"]));
                criterion = Expression.And(criterion, Expression.Eq("TourType", type));
            }

            if (!string.IsNullOrEmpty(queryString["TimeLt"]))
            {
                criterion = Expression.And(criterion, Expression.Lt("NumberOfDay", Convert.ToInt32(queryString["TimeLt"])));
            }

            if (!string.IsNullOrEmpty(queryString["TimeGt"]))
            {
                criterion = Expression.And(criterion, Expression.Gt("NumberOfDay", Convert.ToInt32(queryString["TimeGt"])));
            }

            if (!string.IsNullOrEmpty(queryString["LengthLt"]))
            {
                criterion = Expression.And(criterion, Expression.Lt("LengthTrip", Convert.ToInt32(queryString["LengthLt"])));
            }

            if (!string.IsNullOrEmpty(queryString["LengthGt"]))
            {
                criterion = Expression.And(criterion, Expression.Gt("LengthTrip", Convert.ToInt32(queryString["LengthGt"])));
            }

            Order order = RepeaterOrder.GetOrderFromQueryString(queryString);

            if (!string.IsNullOrEmpty(queryString["Region"]))
            {
                TourRegion region = TourRegionGetById(Convert.ToInt32(queryString["Region"]));
                return _tourDao.TourSearch(criterion, order, region, 0);
            }
            
            return _tourDao.TourSearch(criterion, order, 0);
        }

        public IList TourRegionGetAll()
        {
            return _commonDao.GetAll(typeof (TourRegion));            
        }

        public TourRegion TourRegionGetById(int id)
        {
            return _commonDao.GetObjectById(typeof(TourRegion),id) as TourRegion;
        }

        public IList TourRegionGetAllRoot()
        {
            return _tourDao.TourRegionSearch(Expression.IsNull(TourRegion.PARENT), Order.Asc(TourRegion.ORDER));
        }

        public void SaveOrUpdate(TourRegion region)
        {
            _commonDao.SaveOrUpdateObject(region);
        }

        public void Delete(TourRegion region)
        {
            _commonDao.DeleteObject(region);
        }

        public TourPackagePrice TourPackerPriceGetByTourAndCustomer(Tour tour, int customers)
        {
            ICriterion criterion = Expression.And(Expression.Eq("Tour", tour),
                                                  Expression.Eq("NumberOfCustomers", customers));
            IList list = _commonDao.GetObjectByCriterion(typeof (TourPackagePrice), criterion);
            if (list.Count > 0)
            {
                return list[0] as TourPackagePrice;
            }
            return new TourPackagePrice();
        }

        public TourPrice TourPriceGetByTourAndCustomers(Tour tour, int customers)
        {
            ICriterion criterion = Expression.And(Expression.Eq("Tour", tour),
                                                  Expression.Eq("NumberOfCustomers", customers));
            IList list = _commonDao.GetObjectByCriterion(typeof(TourPrice), criterion);
            if (list.Count > 0)
            {
                return list[0] as TourPrice;
            }
            return new TourPrice();
        }

        public IList TourPriceGetByTour(Tour tour)
        {
            ICriterion criterion = Expression.Eq("Tour", tour);
            IList list = _commonDao.GetObjectByCriterion(typeof(TourPrice), criterion);
            return list;
        }

        public IList TourSalePriceGetByTour(Tour tour)
        {
            ICriterion criterion = Expression.Eq("Tour", tour);
            IList list = _commonDao.GetObjectByCriterion(typeof(TourSalePrice), criterion);
            return list;
        }

        public TourSalePrice TourSalePriceGet(Tour tour, int customers, int roleid)
        {
            ICriterion criterion = Expression.And(Expression.Eq("Tour", tour),
                                      Expression.Eq("NumberOfCustomers", customers));
            criterion = Expression.And(criterion, Expression.Eq("RoleId", roleid));
            IList list = _commonDao.GetObjectByCriterion(typeof(TourSalePrice), criterion);
            if (list.Count > 0)
            {
                return list[0] as TourSalePrice;
            }
            return new TourSalePrice();
        }

        public void SaveOrUpdate(TourPackagePrice package)
        {
            _commonDao.SaveOrUpdateObject(package);
        }

        public void SaveOrUpdate(TourPrice tourPrice)
        {
            _commonDao.SaveOrUpdateObject(tourPrice);
        }

        public void SaveOrUpdate(TourSalePrice salePrice)
        {
            _commonDao.SaveOrUpdateObject(salePrice);
        }
        #endregion

        #region -- Comments --
        public TourComment CommentGetById(int id)
        {
            return _commonDao.GetObjectById(typeof (TourComment), id) as TourComment;
        }

        public void Delete(TourComment comment)
        {
            _commonDao.DeleteObject(comment);
        }

        public User UserGetById(int id)
        {
            return _commonDao.GetObjectById(typeof (User), id) as User;
        }

        public IList CommentsGetByQueryStringPaged(NameValueCollection queryString, int pageSize, int pageIndex, out int count)
        {
            ICriterion criterion = Expression.Eq(TourComment.DELETED, false);
            #region -- landscape id --
            if (!string.IsNullOrEmpty(queryString["TourId"]))
            {
                criterion = Expression.And(criterion,
                                           Expression.Eq("Tour",
                                                         _commonDao.GetObjectById(typeof(Tour),
                                                                                  Convert.ToInt32(queryString["TourId"]))));
            }
            #endregion

            #region -- user id --
            if (!string.IsNullOrEmpty(queryString["UserId"]))
            {
                int userid = Convert.ToInt32(queryString["UserId"]);
                if (userid > 0)
                {
                    criterion = Expression.And(criterion,
                                               Expression.Eq("AuthorId", _commonDao.GetObjectById(typeof(User), userid)));
                }
                else
                {
                    criterion = Expression.And(criterion, Expression.IsNull("AuthorId"));
                }
            }
            #endregion

            #region -- ip --
            if (!string.IsNullOrEmpty(queryString["IP"]))
            {
                criterion = Expression.And(criterion,
                                           Expression.Eq(TourComment.AUTHORIP, queryString["IP"]));
            }
            #endregion

            #region -- posted --
            if (!string.IsNullOrEmpty(queryString["Posted"]))
            {
                DateTime date = DateTime.ParseExact(queryString["Posted"], "dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
                criterion = Expression.And(criterion,
                                           Expression.Between(TourComment.DATECREATED, date, date.AddDays(1)));
            }
            #endregion

            #region -- before & after --
            if (!string.IsNullOrEmpty(queryString["Before"]))
            {
                DateTime date = DateTime.ParseExact(queryString["Before"], "dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
                criterion = Expression.And(criterion, Expression.Lt(TourComment.DATECREATED, date));
            }

            if (!string.IsNullOrEmpty(queryString["After"]))
            {
                DateTime date = DateTime.ParseExact(queryString["After"], "dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
                criterion = Expression.And(criterion, Expression.Gt(TourComment.DATECREATED, date.AddDays(1)));
            }
            #endregion

            #region -- content --
            if (!string.IsNullOrEmpty(queryString["Content"]))
            {
                criterion = Expression.And(criterion,
                                           Expression.Like(TourComment.COMMENT, queryString["Content"],
                                                           MatchMode.Anywhere));
            }
            #endregion

            count = _commonDao.CountObjectByCriterion(typeof(TourComment), criterion);
            return _commonDao.GetObjectByCriterionPaged(typeof(TourComment), criterion, pageIndex, pageSize,
                                                   Order.Desc(TourComment.DATECREATED));
        }
        #endregion

        public IList TourGetPartialPackage()
        {
            ICriterion criterion = Expression.Or(Expression.Eq(Tour.PACKAGESTATUS, PackageStatus.PartPackage),
                                                 Expression.Eq(Tour.PACKAGESTATUS, PackageStatus.Both));
            return _commonDao.GetObjectByCriterion(typeof (Tour), criterion);
        }

        public IList TourPackageConfigGetByTour(Tour tour)
        {
            return _commonDao.GetObjectByProperty(typeof (TourPackageConfig), "Tour", tour);
        }

        public TourPackageConfig TourPackageConfigGetById(int id)
        {
            return _commonDao.GetObjectById(typeof(TourPackageConfig), id) as TourPackageConfig;
        }

        public void SaveOrUpdate(TourPackageConfig config)
        {
            _commonDao.SaveOrUpdateObject(config);
        }

        public void Delete(TourPackageConfig config)
        {
            _commonDao.DeleteObject(config);
        }

        #region -- Gallery --
        public IList AlbumGetAll()
        {
            return _commonDao.GetAll(typeof (Album));
        }

        public Album AlbumGetById(int id)
        {
            return _commonDao.GetObjectById(typeof (Album), id) as Album;
        }

        public IList AlbumGetByName(string name)
        {
            return _tourDao.AlbumGetByName(name);
        }
        #endregion
        public int UserCount()
        {
            return _tourDao.UserCount();
        }

        #region -- Other expenses --
        public IList ExpenseGetByTour(Tour tour)
        {
            return _commonDao.GetObjectByProperty(typeof (TourOtherExpense), "Tour", tour);
        }

        public void SaveOrUpdate(TourOtherExpense expense)
        {
            _commonDao.SaveOrUpdateObject(expense);
        }

        public void Delete(TourOtherExpense expense)
        {
            _commonDao.DeleteObject(expense);
        }

        public void SaveOrUpdate(TourOtherExpensePrice price)
        {
            _commonDao.SaveOrUpdateObject(price);
        }
        #endregion

        #region -- Related --
        public IList RelatedTourGetByTour(Tour tour)
        {
            ICriterion criterion = Expression.Or(Expression.Eq("Tour", tour), Expression.Eq("Related", tour));
            return _commonDao.GetObjectByCriterion(typeof (TourRelated), criterion);
        }

        public bool CheckRelated(Tour tour1, Tour tour2)
        {
            ICriterion criterion = Expression.And(Expression.Eq("Tour", tour1), Expression.Eq("Related", tour2));
            criterion = Expression.Or(criterion,
                                      Expression.And(Expression.Eq("Related", tour1), Expression.Eq("Tour", tour2)));
            return _commonDao.CountObjectByCriterion(typeof (TourRelated), criterion) > 0;
        }

        public void DeleteRelated(Tour tour1, Tour tour2)
        {
            ICriterion criterion = Expression.And(Expression.Eq("Tour", tour1), Expression.Eq("Related", tour2));
            criterion = Expression.Or(criterion,
                                      Expression.And(Expression.Eq("Related", tour1), Expression.Eq("Tour", tour2)));
            IList list = _commonDao.GetObjectByCriterion(typeof(TourRelated), criterion);
            list.Clear();            
        }

        public void InsertRelated(Tour tour1, Tour tour2)
        {
            if (!CheckRelated(tour1, tour2))
            {
                TourRelated related = new TourRelated();
                related.Tour = tour1;
                related.Related = tour2;
                _commonDao.SaveObject(related);
            }
        }
        #endregion

        #region -- Comment --
        public void SaveOrUpdate(TourComment comment)
        {
            _commonDao.SaveOrUpdateObject(comment);            
        }

        public IList TourCommentGetByTour(Tour tour)
        {
            return _commonDao.GetObjectByProperty(typeof (TourComment), "Tour", tour);
        }
        #endregion

        #endregion

        #region IActionProvider Members

        public ActionCollection GetOutboundActions()
        {
            ActionCollection outboundActions = new ActionCollection();
            outboundActions.Add(new CMS.Core.Communication.Action(ACTION_LIST, new string[0]));
            outboundActions.Add(new CMS.Core.Communication.Action(ACTION_HOTEL_CONFIG, new string[0]));
            outboundActions.Add(new CMS.Core.Communication.Action(ACTION_RESTAURANT_CONFIG, new string[0]));
            outboundActions.Add(new CMS.Core.Communication.Action(ACTION_GUIDE_CONFIG, new string[0]));
            outboundActions.Add(new CMS.Core.Communication.Action(ACTION_TRANSPORT_CONFIG, new string[0]));
            outboundActions.Add(new CMS.Core.Communication.Action(ACTION_BOAT_CONFIG, new string[0]));
            outboundActions.Add(new CMS.Core.Communication.Action(ACTION_ENTRANCEFEE_CONFIG, new string[0]));
            outboundActions.Add(new CMS.Core.Communication.Action("Album", new string[0]));
            return outboundActions;
        }

        #endregion

        #region IActionProvider Members

        public ActionCollection GetInboundActions()
        {
            ActionCollection inboundActions = new ActionCollection();
            inboundActions.Add(new CMS.Core.Communication.Action("TourDetail", new string[0]));
            return inboundActions;
        }

        #endregion

        #region -- Override --
        public override void ReadSectionSettings()
        {
            base.ReadSectionSettings();
            if(Section.Settings["IsTourModule"]!=null)
            {
                _isTour = Convert.ToBoolean(Section.Settings["IsTourModule"]);
            }
            else
            {
                _isTour = false;
            }
        }

        protected override void ParsePathInfo()
        {
            try
            {
                base.ParsePathInfo();
                if (ModuleParams.Length > 0)
                {
                    if (ModuleParams.Length >= 2)
                    {
                        switch (ModuleParams[0])
                        {
                            case "tour":
                                _tourId = Convert.ToInt32(ModuleParams[1]);
                                break;
                            case "order":
                                _tourId = Convert.ToInt32(ModuleParams[1]);
                                break;
                            default:
                                break;
                        }
                        //category = _newsdao.GetNewsCategoryByFriendlyName(ModuleParams[1]);
                    }
                    else
                    {
                        _tourId = 0;
                    }
                }
                else
                {
                    _tourId = 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Bad request" + ModuleParams[1], ex);
            }
        }

        public override string CurrentViewControlPath
        {
            get
            {
                if (_tourId > 0)
                {
                    if (ModuleParams[0]!="order")
                    return "Modules/TourManagement/TourDetail.ascx";
                return "Modules/TourManagement/Order.ascx";
                }
                if (!_isTour)
                {
                    return base.CurrentViewControlPath;
                }
                return "Modules/TourManagement/Tours.ascx";
            }
        }
        #endregion

        public override void ExportTourConfigToDataTable(int tourid, DataTable table, IList roles, int numberOfCustomer)
        {
            //throw new System.NotImplementedException();
        }
    }
}