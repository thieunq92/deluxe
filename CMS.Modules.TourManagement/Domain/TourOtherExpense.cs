
using System;

namespace CMS.Modules.TourManagement.Domain
{
	#region TourPackageConfig

	/// <summary>
	/// TourBoatPriceConfig object for NHibernate mapped table 'tmt_TourBoatPriceConfig'.
	/// </summary>
	[Serializable]
	public class TourOtherExpense 
		{
		#region Static Columns Name
		
		#endregion
			
		#region Member Variables
		
		protected int _id;
		protected Tour _tour;
		protected string _name;
	    protected int _dayFrom;
	    protected int _dayTo;

		#endregion

		#region Constructors

        public TourOtherExpense()
        { 
				_id=-1;
		}

		#endregion

		#region Public Properties

		public virtual int Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public virtual Tour Tour
		{
			get { return _tour; }
            set { _tour = value; }
		}

		public virtual string Name
		{
			get { return _name; }
            set { _name = value; }
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

		#endregion
		
	}

	#endregion

}
