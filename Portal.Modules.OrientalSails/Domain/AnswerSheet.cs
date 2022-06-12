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
    /// <summary>
    ///  Domain dùng để thao tác với bảng os_AnswerShet
    /// </summary>
    public class AnswerSheet
    {
        protected int _id;
        protected string _name;
        protected string _address;
        protected string _email;
        protected Nationality _nationality;
        protected DateTime? _date;
        protected Booking _booking;
        protected Cruise _cruise;
        protected bool _isSent;

        protected string _guide;
        protected string _driver;
        protected string _roomNumber;
        protected bool _deleted;

        protected IList<AnswerOption> _options;
        protected IList<AnswerGroup> _groups;

        public AnswerSheet()
        {
            _id = -1;
        }

        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public virtual string Address
        {
            get { return _address; }
            set { _address = value; }
        }

        public virtual string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public virtual Nationality Nationality
        {
            get { return _nationality; }
            set { _nationality = value; }
        }

        public virtual DateTime? Date
        {
            get { return _date; }
            set { _date = value; }
        }

        public virtual Booking Booking
        {
            get { return _booking; }
            set { _booking = value; }
        }

        public virtual Cruise Cruise
        {
            get { return _cruise; }
            set { _cruise = value; }
        }

        public virtual bool IsSent
        {
            get { return _isSent; }
            set { _isSent = value; }
        }

        public virtual string Guide
        {
            get { return _guide; }
            set { _guide = value; }
        }

        public virtual string Driver
        {
            get { return _driver; }
            set { _driver = value; }
        }

        public virtual string RoomNumber
        {
            get { return _roomNumber; }
            set { _roomNumber = value; }
        }

        public virtual bool Deleted
        {
            get { return _deleted; }
            set { _deleted = value; }
        }

        public virtual IList<AnswerOption> Options
        {
            get
            {
                if (_options == null)
                {
                    _options = new List<AnswerOption>();
                }
                return _options;
            }
            set { _options = value; }
        }

        public virtual IList<AnswerGroup> Groups
        {
            get
            {
                if (_groups == null)
                {
                    _groups = new List<AnswerGroup>();
                }
                return _groups;
            }
            set { _groups = value; }
        }
        /// <summary>
        /// Láy AnswerGroup theo QuestionGroup
        /// </summary>
        /// <param name="group">QuestionGroup cần tìm AnswerGroup</param>
        /// <returns>Trả về AnswerGroup cần tìm</returns>
        public virtual AnswerGroup GetGroup(QuestionGroup group)
        {
            foreach (AnswerGroup answerGroup in Groups)
            {
                if (answerGroup.Group.Id == group.Id)
                {
                    return answerGroup;
                }
            }
            return new AnswerGroup();
        }
        /// <summary>
        /// Lấy AnswerOption cho Question
        /// </summary>
        /// <param name="question">Question cần lấy AnswerOption </param>
        /// <returns>Trả về AnswerOption cần tìm</returns>
        public virtual AnswerOption GetOption(Question question)
        {
            foreach (AnswerOption option in Options)
            {
                if (option.Question.Id == question.Id)
                {
                    return option;
                }
            }
            return new AnswerOption();
        }

    }
}
