using System;
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
    /// Domain dùng để thao tác với bảng os_AnswerOption
    /// </summary>
    public class AnswerOption
    {
        protected int _id;
        protected Question _question;
        protected AnswerSheet _answerSheet;
        protected int _option;

        public AnswerOption()
        {
            _id = -1;
        }

        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual Question Question
        {
            get { return _question; }
            set { _question = value; }
        }

        public virtual AnswerSheet AnswerSheet
        {
            get { return _answerSheet; }
            set { _answerSheet = value; }
        }

        public virtual int Option
        {
            get { return _option; }
            set { _option = value; }
        }
    }
}
