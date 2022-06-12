
using System;

namespace CMS.Modules.TourManagement.Domain
{
	#region TourPackageConfig

	/// <summary>
	/// TourBoatPriceConfig object for NHibernate mapped table 'tmt_TourBoatPriceConfig'.
	/// </summary>
	[Serializable]
	public class TourPackageConfig 
		{
		#region Static Columns Name
		
		#endregion
			
		#region Member Variables
		
		protected int _id;
		protected Tour _tour;
		protected Tour _package;

		#endregion

		#region Constructors

        public TourPackageConfig()
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

		public virtual Tour Package
		{
			get { return _package; }
            set { _package = value; }
		}

		#endregion
		
	}

	#endregion

}
