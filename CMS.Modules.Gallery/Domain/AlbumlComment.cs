using System;
using CMS.Core.Domain;

namespace CMS.Modules.Gallery.Domain
{
    /// <summary>
    /// HotelComment object for NHibernate mapped table 'tmh_HotelComment'.
    /// </summary>
    public class AlbumComment
    {
        #region Static Columns Name

        public static string AUTHOR = "Author";
        public static string AUTHORIP = "IP";
        public static string COMMENT = "Comment";
        public static string DATECREATED = "DateCreated";
        public static string DATEMODIFIED = "DateModified";
        public static string DELETED = "Deleted";
        public static string EMAIL = "Email";
        public static string STATUS = "Status";

        #endregion

        #region Member Variables

        protected string _author;
        protected User _authorId;
        protected string _comment;
        protected DateTime _dateCreated;
        protected DateTime _dateModified;
        protected bool _deleted;
        protected string _email;
        protected Album _album;
        protected int _id;
        protected string _iP;
        protected int _status;

        #endregion

        #region Constructors

        public AlbumComment()
        {
            _id = -1;
        }

        public AlbumComment(bool deleted, DateTime dateModified, DateTime dateCreated, string author, string email,
                            string iP, int status, string comment, User authorId, Album album)
        {
            _deleted = deleted;
            _dateModified = dateModified;
            _dateCreated = dateCreated;
            _author = author;
            _email = email;
            _iP = iP;
            _status = status;
            _comment = comment;
            _authorId = authorId;
            _album = album;
        }

        #endregion

        #region Public Properties

        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
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
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for Author", value, value);
                _author = value;
            }
        }

        public virtual string Email
        {
            get { return _email; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for Email", value, value);
                _email = value;
            }
        }

        public virtual string IP
        {
            get { return _iP; }
            set
            {
                if (value != null && value.Length > 15)
                    throw new ArgumentOutOfRangeException("Invalid value for IP", value, value);
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
                if (value != null && value.Length > 2000)
                    throw new ArgumentOutOfRangeException("Invalid value for Comment", value, value);
                _comment = value;
            }
        }

        public virtual User AuthorId
        {
            get { return _authorId; }
            set { _authorId = value; }
        }

        public virtual Album Album
        {
            get { return _album; }
            set { _album = value; }
        }

        #endregion
    }
}