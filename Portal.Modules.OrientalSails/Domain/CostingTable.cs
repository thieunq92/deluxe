using System;
using System.Collections;
using System.Collections.Generic;
using Portal.Modules.OrientalSails.Web.Util;

namespace Portal.Modules.OrientalSails.Domain
{

    #region Customer

    /// <summary>
    /// Customer object for NHibernate mapped table 'os_Customer'.
    /// </summary>
    public class CostingTable
    {
        #region Static Columns Name

        public static string BIRTHDAY = "Birthday";
        public static string BOOKING = "Booking";
        public static string BOOKINGROOM = "BookingRoom";
        public static string COUNTRY = "Country";
        public static string FULLNAME = "Fullname";
        public static string PASSPORT = "Passport";

        #endregion

        #region Member Variables

        protected IList _costs;
        protected int _id;
        protected DateTime _validFrom;
        protected DateTime? _validTo;
        protected SailsTrip _trip;
        protected TripOption _option;
        protected IList _activeCosts;
        protected Dictionary<CostType, Costing> _activeMap;

        #endregion

        #region Constructors

        public CostingTable()
        {
            _id = -1;
        }

        #endregion

        #region Public Properties

        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual DateTime ValidFrom
        {
            get { return _validFrom; }
            set { _validFrom = value; }
        }

        public virtual DateTime? ValidTo
        {
            get { return _validTo; }
            set { _validTo = value; }
        }

        public virtual SailsTrip Trip
        {
            get { return _trip; }
            set { _trip = value; }
        }

        public virtual TripOption Option
        {
            get { return _option; }
            set { _option = value; }
        }

        public virtual IList Costs
        {
            get
            {
                if (_costs == null)
                {
                    _costs = new ArrayList();
                }
                return _costs;
            }
            set { _costs = value; }
        }

        public virtual IList GetActiveCost(IList costTypes)
        {
            if (_activeCosts==null)
            {
                _activeCosts = new ArrayList();
                Dictionary<CostType, Costing> map = new Dictionary<CostType, Costing>();
                foreach (Costing cost in Costs)
                {
                    if (!map.ContainsKey(cost.Type))
                    {
                        map.Add(cost.Type, cost);
                    }
                }

                foreach (CostType type in costTypes)
                {
                    if (map.ContainsKey(type))
                    {
                        _activeCosts.Add(map[type]);
                    }
                    else
                    {
                        Costing newCost = new Costing();
                        newCost.Type = type;
                        newCost.Table = this;
                        _activeCosts.Add(newCost);
                    }
                }
            }
            return _activeCosts;
        }

        /// <summary>
        /// Lấy map giá từ bảng giá hiện tại theo danh sách dịch vụ đưa vào
        /// </summary>
        /// <param name="costTypes"></param>
        /// <returns></returns>
        public virtual Dictionary<CostType, Costing> GetCostMap(IList costTypes)
        {
            if (_activeMap == null)
            {
                _activeMap = new Dictionary<CostType, Costing>();
                foreach (Costing cost in Costs)
                {
                    if (!_activeMap.ContainsKey(cost.Type))
                    {
                        _activeMap.Add(cost.Type, cost);
                    }
                }
            }
            return _activeMap;
        }

        #endregion
    }

    #endregion
}