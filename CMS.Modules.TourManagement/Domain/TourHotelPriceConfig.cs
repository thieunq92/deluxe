using System;
using System.Collections;
using System.Web.UI.WebControls;

namespace CMS.Modules.TourManagement.Domain
{
	#region TourHotelPriceConfig

	/// <summary>
	/// TourHotelPriceConfig object for NHibernate mapped table 'tmt_TourHotelPriceConfig'.
	/// </summary>
	public class TourHotelPriceConfig
		{
		#region Member Variables
		
		protected int _id;
		protected int _tourId;
		protected int _hotelId;
		protected int _roomTypeId;
		protected int _roomClassId;
		protected int _roomCapicity;
		protected decimal _netPrice;
		protected int _currencyId;
	    protected int _dayFrom;
	    protected int _dayTo;

		#endregion

		#region Constructors

		public TourHotelPriceConfig() 
		{
			_id = -1;
		}

		public TourHotelPriceConfig( int tourId, int hotelId, int roomTypeId, int roomClassId, int roomCapicity, decimal netPrice, int currencyId, int numberOfDays )
		{
			this._tourId = tourId;
			this._hotelId = hotelId;
			this._roomTypeId = roomTypeId;
			this._roomClassId = roomClassId;
			this._roomCapicity = roomCapicity;
			this._netPrice = netPrice;
			this._currencyId = currencyId;
		}

		#endregion

		#region Public Properties

		public virtual int Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public virtual int TourId
		{
			get { return _tourId; }
			set { _tourId = value; }
		}

		public virtual int HotelId
		{
			get { return _hotelId; }
			set { _hotelId = value; }
		}

		public virtual int RoomTypeId
		{
			get { return _roomTypeId; }
			set { _roomTypeId = value; }
		}

		public virtual int RoomClassId
		{
			get { return _roomClassId; }
			set { _roomClassId = value; }
		}

		public virtual int RoomCapicity
		{
			get { return _roomCapicity; }
			set { _roomCapicity = value; }
		}

		public virtual decimal NetPrice
		{
			get { return _netPrice; }
			set { _netPrice = value; }
		}

		public virtual int CurrencyId
		{
			get { return _currencyId; }
			set { _currencyId = value; }
		}

	    public virtual int DayFrom
	    {
            get { return _dayFrom; }
            set { _dayFrom = value; }
	    }

        public virtual int DayTo
        {
            get { return _dayTo; }
            set { _dayTo = value; }
        }

		public virtual int NumberOfDays
		{
            get
            {
                if (_dayTo == _dayFrom)
                    return 1;
                return _dayTo - _dayFrom;
            }
		}

		#endregion
	}

	#endregion
}
