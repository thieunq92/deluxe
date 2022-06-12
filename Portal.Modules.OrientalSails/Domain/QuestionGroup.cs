using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CMS.Core.Domain;

namespace Portal.Modules.OrientalSails.Domain
{
    #region Customer

    /// <summary>
    /// Customer object for NHibernate mapped table 'os_Customer'.
    /// </summary>
    public class QuestionGroup
    {
        #region Member Variables

        protected int _id;
        protected bool _deleted;
        protected User _createdBy;
        protected DateTime _createdDate;
        protected User _modifiedBy;
        protected DateTime? _modifiedDate;
        protected string _name;
        protected string _selection1;
        protected string _selection2;
        protected string _selection3;
        protected string _selection4;
        protected string _selection5;
        protected IList<Question> _questions;
        protected int _priority;

        #endregion

        #region Constructors

        public QuestionGroup()
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

        public virtual bool Deleted
        {
            get { return _deleted; }
            set { _deleted = value; }
        }

        public virtual User CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; }
        }

        public virtual DateTime CreatedDate
        {
            get { return _createdDate; }
            set { _createdDate = value; }
        }

        public virtual User ModifiedBy
        {
            get { return _modifiedBy; }
            set { _modifiedBy = value; }
        }

        public virtual DateTime? ModifiedDate
        {
            get { return _modifiedDate; }
            set { _modifiedDate = value; }
        }

        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public virtual string Selection1
        {
            get { return _selection1; }
            set { _selection1 = value; }
        }

        public virtual string Selection2
        {
            get { return _selection2; }
            set { _selection2 = value; }
        }

        public virtual string Selection3
        {
            get { return _selection3; }
            set { _selection3 = value; }
        }

        public virtual string Selection4
        {
            get { return _selection4; }
            set { _selection4 = value; }
        }

        public virtual string Selection5
        {
            get { return _selection5; }
            set { _selection5 = value; }
        }

        public virtual int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        public virtual IList<Question> Questions
        {
            get
            {
                if (_questions == null)
                {
                    _questions = new List<Question>();
                }
                return _questions;
            }
            set { _questions = value; }
        }

        public virtual IList<string> Selections
        {
            get
            {
                IList<string> result = new List<string>();
                if (!string.IsNullOrEmpty(_selection1))
                {
                    result.Add(_selection1);
                }
                if (!string.IsNullOrEmpty(_selection2))
                {
                    result.Add(_selection2);
                }
                if (!string.IsNullOrEmpty(_selection3))
                {
                    result.Add(_selection3);
                }
                if (!string.IsNullOrEmpty(_selection4))
                {
                    result.Add(_selection4);
                }
                if (!string.IsNullOrEmpty(_selection5))
                {
                    result.Add(_selection5);
                }
                return result;
            }
        }

        #endregion
    }

    #endregion
}
