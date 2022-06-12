
using System;
using System.Collections;
using System.Web.UI.WebControls;
using CMS.Core.Domain;

namespace CMS.Modules.TourManagement.Domain
{
	#region TourComment

	/// <summary>
	/// TourComment object for NHibernate mapped table 'tmt_TourComment'.
	/// </summary>
	public class TourComment 
		{
		#region Static Columns Name
		
		public static string DELETED = "Deleted";
		public static string DATEMODIFIED = "DateModified";
		public static string DATECREATED = "DateCreated";
		public static string AUTHOR = "Author";
		public static string EMAIL = "Email";
		public static string AUTHORIP = "IP";
		public static string STATUS = "Status";
		public static string COMMENT = "Comment";
		
		#endregion
			
		#region Member Variables
		
		protected int _id;
		protected bool _deleted;
		protected DateTime _dateModified;
		protected DateTime _dateCreated;
		protected string _author;
		protected string _email;
		protected string _iP;
		protected int _status;
		protected string _comment;
		protected User _authorId;
		protected Tour _tour;

		#endregion

		#region Constructors

		public TourComment() { 
				_id=-1;
			}

		public TourComment( bool deleted, DateTime dateModified, DateTime dateCreated, string author, string email, string iP, int status, string comment, User authorId, Tour tour )
		{
			this._deleted = deleted;
			this._dateModified = dateModified;
			this._dateCreated = dateCreated;
			this._author = author;
			this._email = email;
			this._iP = iP;
			this._status = status;
			this._comment = comment;
			this._authorId = authorId;
			this._tour = tour;
		}

		#endregion

		#region Public Properties

		public virtual int Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public virtual bool Deleted
		{
			get { return _deleted; }
			set { _deleted = value; }
		}

		public virtual DateTime DateModified
		{
			get { return _dateModified; }
			set { _dateModified = value; }
		}

		public virtual DateTime DateCreated
		{
			get { return _dateCreated; }
			set { _dateCreated = value; }
		}

		public virtual string Author
		{
			get { return _author; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Author", value, value.ToString());
				_author = value;
			}
		}

		public virtual string Email
		{
			get { return _email; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Email", value, value.ToString());
				_email = value;
			}
		}

		public virtual string IP
		{
			get { return _iP; }
			set
			{
				if ( value != null && value.Length > 15)
					throw new ArgumentOutOfRangeException("Invalid value for IP", value, value.ToString());
				_iP = value;
			}
		}

		public virtual int Status
		{
			get { return _status; }
			set { _status = value; }
		}

		public virtual string Comment
		{
			get { return _comment; }
			set
			{
				if ( value != null && value.Length > 2000)
					throw new ArgumentOutOfRangeException("Invalid value for Comment", value, value.ToString());
				_comment = value;
			}
		}

		public virtual User AuthorId
		{
			get { return _authorId; }
			set { _authorId = value; }
		}

		public virtual Tour Tour
		{
			get { return _tour; }
			set { _tour = value; }
		}


		#endregion
		
	}

	#endregion

}
