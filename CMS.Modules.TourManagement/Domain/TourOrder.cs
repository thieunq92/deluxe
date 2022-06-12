using System;
using System.Text;
using CMS.Modules.TourManagement.Domain;

namespace CMS.Modules.TourManagement.Domain
{
    /// <summary>
    /// HotelOrder object for NHibernate mapped table 'tmh_HotelOrder'.
    /// </summary>
    public class TourOrder : BaseOrder
    {
        #region Static Columns Name

        #endregion

        #region Member Variables

        protected DateTime _departureDate;
        protected Tour _hotelId;
        protected string _flexiability;
        protected int _adult;
        protected int _infant;
        protected int _children;

        #endregion

        #region Constructors

        public TourOrder()
        {
            _id = -1;
        }

        #endregion

        #region Public Properties

        public virtual Tour Tour
        {
            get { return _hotelId; }
            set { _hotelId = value; }
        }

        public virtual string Flexiability
        {
            get { return _flexiability;}
            set { _flexiability = value; }
        }

        public virtual int Adult
        {
            get { return _adult; }
            set { _adult = value; }
        }

        public virtual int Infant
        {
            get { return _infant; }
            set { _infant = value; }
        }

        public virtual int Children
        {
            get { return _children; }
            set { _children = value; }
        }

        public override string OrderContent()
        {
            StringBuilder sb = new StringBuilder();
            if (_userId!=null)
            {
                sb.AppendFormat("User: {0}<br/>", _userId.UserName);
            }
            sb.AppendFormat("Order at: {0}<br/>", _orderDate.ToString("dd/MM/yyyy HH:mm"));
            sb.AppendFormat("Departure: {0}<br/>", _departureDate.ToString("dd/MM/yyyy"));
            sb.AppendFormat("Flexiability: {0}<br/>", _flexiability);
            sb.AppendFormat("Adult: {0}<br/>", _adult);
            sb.AppendFormat("Children: {0}<br/>", _children);
            sb.AppendFormat("Infant: {0}<br/>", _infant);
            sb.AppendFormat("Speical request: {0}", _specialRequest);
            return sb.ToString();
        }

        #endregion
    }
}