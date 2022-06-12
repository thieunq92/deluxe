using System;
using System.Collections;
using System.IO;
using CMS.Core.Domain;

namespace CMS.Modules.Gallery.Domain
{
	public class Album
	{
		private int _id;
        private int _createdById;
        private int _modifiedById;
		private string _title;
		private string _description;
		private int _photocount;
        private User _createdBy;
        private User _modifiedBy;
		private bool _active;
		private IList _photos;
		private Section _section;
		private Site _site;
		private DateTime _updateTimestamp;
	    private bool _useSimpleViewer;
	    private IList _comments;

	    private GalleryModule _galleryModule;

		#region properties

		/// <description>
		/// Property Id (int)
		/// </description>
		public virtual int Id
		{
			get { return this._id; }
			set { this._id = value; }
		}

		/// <description>
		/// Property Title (string)
		/// </description>
		public virtual string Title
		{
			get { return this._title; }
			set { this._title = value; }
		}

		/// <description>
		/// Property description (string)
		/// </description>
		public virtual string Description
		{
			get { return this._description; }
			set { this._description = value; }
		}

		/// <description>
		/// Property active (bool)
		/// </description>
		public virtual bool Active
		{
			get { return this._active; }
			set { this._active = value; }
		}

		/// <description>
		/// Property photos (IList)
		/// </description>
		public virtual IList Photos
		{
			get { return this._photos; }
			set { this._photos = value; }
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
        public virtual User ModifiedBy
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
		
		/// <description>
		/// The site where this category belongs to.
		/// </description>
		public virtual Site Site
		{
			get { return this._site; }
			set { this._site = value; }
		}

		/// <description>
		/// Property UpdateTimestamp (DateTime)
		/// </description>
		public virtual DateTime UpdateTimestamp
		{
			get { return this._updateTimestamp; }
			set { this._updateTimestamp = value; }
		}

		public virtual int PhotoCount
		{
			get { return this._photocount; }
			set { this._photocount = value; }
		}

        public virtual IList Comments
        {
            get
            {
                if (_comments == null)
                {
                    _comments = new ArrayList();
                }
                return _comments;
            }
            set { _comments = value; }
        }

	    #endregion

		/// <description>
		/// Default constructor.
		/// </description>
		public Album()
		{
			this._id = -1;
			this._active = true;
			this._photocount = 0;
		    _useSimpleViewer = true;
		}

        public Album(GalleryModule galleryModule) : this()
        {
            _galleryModule = galleryModule;
        }

        public virtual void SetGalleryModule(GalleryModule galleryModule)
        {
            _galleryModule = galleryModule;
        }

	    public virtual bool UseSimpleViewer
	    {
	        get { return _useSimpleViewer;}
            set { _useSimpleViewer = value; }
	    }

	    public virtual DirectoryInfo directoryInfoGetDirectoryInfo()
	    {
            return new DirectoryInfo(_galleryModule.PathBuilder.GetAlbumDirectory(Id));
	    }

        public virtual DirectoryInfo GetDirectoryMassImport()
        {
            DirectoryInfo info = new DirectoryInfo(_galleryModule.PathBuilder.GetAlbumDirectory(Id));
            return new DirectoryInfo(info.FullName + "\\toImport");
        }


	}
}