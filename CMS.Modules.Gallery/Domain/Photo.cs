using System;
using System.Collections;

using CMS.Core.Domain;

namespace CMS.Modules.Gallery.Domain
{
	/// <summary>
    /// 
	/// </summary>
	public class Photo
    {
        #region private members

        private int _id;
		private int _createdById;
		private int _modifiedById;
		private string _filePath;
		private string _title;
		private int _size;
		private int _nrOfViews;
		
        private int _imagewidth;
		private int _imageheight;
		private int _thumbwidth;
		private int _thumbheight;
        private float _rating;
	    private int _order;

		private DateTime _dateCreated;
		private DateTime _dateModified;
		private Section _section;
		private User _createdBy;
		private User _modifiedBy;
		private Album _album;

        #endregion //private members

        #region properties

        /// <summary>
		/// Property Id (int)
		/// </summary>
        public virtual int Id
		{
			get { return this._id; }
			set { this._id = value; }
		}

        public virtual string FileName
		{
			get { return this._filePath; }
			set { this._filePath = value; }
		}
		
		/// <summary>
		/// Property Title (string)
		/// </summary>
        public virtual string Title
		{
			get { return this._title; }
			set { this._title = value; }
		}
		
		/// <summary>
		/// The title that is shown.
		/// </summary>
        public virtual string DisplayTitle
		{
			get 
			{
				if (this._title != null && this._title != String.Empty)
				{
					return this._title;
				}
				else
				{
					return this._filePath;
				}
			}
		}		

		/// <summary>
		/// Property Size (int)
		/// </summary>
        public virtual int Size
		{
			get { return this._size; }
			set { this._size = value; }
		}

		/// <summary>
		/// Property NrOfDownloads (int)
		/// </summary>
        public virtual int NrOfViews
		{
			get { return this._nrOfViews; }
			set { this._nrOfViews = value; }
		}
		
		/// <summary>
		/// Property DateCreated (DateTime)
		/// </summary>
        public virtual DateTime DateCreated
		{
			get { return this._dateCreated; }
			set { this._dateCreated = value; }
		}

		/// <summary>
		/// Property DateModified (DateTime)
		/// </summary>
        public virtual DateTime DateModified
		{
			get { return this._dateModified; }
			set { this._dateModified = value; }
		}

		/// <summary>
		/// Property Section (Section)
		/// </summary>
        public virtual Section Section
		{
			get { return this._section; }
			set { this._section = value; }
		}

		/// <summary>
		/// Property CreatedBy (User)
		/// </summary>
        public virtual CMS.Core.Domain.User CreatedBy
		{
			get { return this._createdBy; }
			set { this._createdBy = value; }
		}

		/// <summary>
		/// Property CreatedById (int)
		/// </summary>
        public virtual int CreatedById
		{
			get { return this._createdById; }
			set { this._createdById = value; }
		}

		/// <summary>
		/// Property ModifiedBy (User)
		/// </summary>
        public virtual CMS.Core.Domain.User ModifiedBy
		{
			get { return this._modifiedBy; }
			set { this._modifiedBy = value; }
		}

		/// <summary>
		/// Property ModifiedById (int)
		/// </summary>
        public virtual int ModifiedById
		{
			get { return this._modifiedById; }
			set { this._modifiedById = value; }
		}

		/// <summary>
		/// Property Album (Album)
		/// </summary>
        public virtual Album Album
		{
			get { return this._album; }
			set { this._album = value; }
		}

        public virtual int ImageWidth
        {
            get { return this._imagewidth; }
            set { this._imagewidth = value; }
        }

        public virtual int ImageHeight
        {
            get { return this._imageheight; }
            set { this._imageheight = value; }
        }

        public virtual int ThumbWidth
        {
            get { return this._thumbwidth; }
            set { this._thumbwidth = value; }
        }

        public virtual int ThumbHeight
        {
            get { return this._thumbheight; }
            set { this._thumbheight = value; }
        }

        public virtual float Rating
        {
            get { return this._rating; }
            set { this._rating = value; }
        }

	    public virtual int Order
	    {
            get { return _order; }
            set { _order = value; }
	    }

		#endregion

		#region constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public Photo()
		{
			this._id = -1;
			this._nrOfViews = 0;
			this._createdById = -1;
			this._modifiedById = -1;

            this._imagewidth = 0;
            this._imageheight = 0;
            this._thumbwidth = 0;
            this._thumbheight = 0;

            this._rating = 0;

			this._dateCreated = DateTime.Now;
			this._dateModified = DateTime.Now;
		}

		#endregion
	}
}
