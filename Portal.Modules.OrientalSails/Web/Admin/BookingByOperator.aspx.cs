using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.Core.Domain;
using CMS.Web.Util;
using Portal.Modules.OrientalSails.Web.UI;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class BookingByOperator : SailsAdminBasePage
    {
        #region -- PRIVATE MEMBERS --

        private DateTime _from;
        private DateTime _to;

        private IList _operators;
        private IList Operators
        {
            get
            {
                if (_operators == null)
                {
                    _operators = Module.OperatorGetAll();
                }
                return _operators;
            }
        }

        /// <summary>
        /// Ngày hiện tại đang nhắc đến
        /// </summary>
        private DateTime _date;

        /// <summary>
        /// Map giữa userid và tổng số pax
        /// </summary>
        private readonly Dictionary<int, int> _totalMap = new Dictionary<int, int>();

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["from"]!=null)
                {
                    _from = DateTime.FromOADate(Convert.ToDouble(Request.QueryString["from"]));
                }
                else
                {
                    _from = new DateTime(DateTime.Today.Year,DateTime.Today.Month,1);
                }

                if (Request.QueryString["to"] != null)
                {
                    _to = DateTime.FromOADate(Convert.ToDouble(Request.QueryString["to"]));
                }
                else
                {
                    _to = _from.AddMonths(1).AddDays(-1);
                }

                IList<DateTime> dates = new List<DateTime>();
                DateTime current = _from;
                while (current <= _to)
                {
                    dates.Add(current);
                    current = current.AddDays(1);
                }

                rptDates.DataSource = dates;
                rptDates.DataBind();
            }
        }

        protected void rptDates_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                var rptOperators = (Repeater)e.Item.FindControl("rptOperators");
                rptOperators.DataSource = Operators;
                rptOperators.DataBind();
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                var rptOperators = (Repeater)e.Item.FindControl("rptOperators");
                rptOperators.DataSource = Operators;
                rptOperators.DataBind();
            }
            else
            {
                var date = (DateTime) e.Item.DataItem;
                ValueBinder.BindLiteral(e.Item, "litDate", date.ToString("dd/MM/yyyy"));
                var rptOperators = (Repeater) e.Item.FindControl("rptOperators");
                _date = date;
                rptOperators.DataSource = Operators;
                rptOperators.DataBind();
            }
        }

        protected void rptOperators_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is User)
            {
                var user = (User) e.Item.DataItem;
                ValueBinder.BindLiteral(e.Item, "litName", user.FullName);

                var litCount = e.Item.FindControl("litCount") as Literal;
                if (litCount!=null)
                {
                    int total = Module.TotalCustomer(_date, _date, user);

                    if (!_totalMap.ContainsKey(user.Id))
                    {
                        _totalMap.Add(user.Id, 0);
                    }
                    _totalMap[user.Id] += total;

                    ValueBinder.BindLiteral(e.Item, "litCount", total.ToString());
                }

                var litTotal = e.Item.FindControl("litTotal") as Literal;
                if (litTotal!=null)
                {
                    litTotal.Text = _totalMap[user.Id].ToString();
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            DateTime from;
            if (!string.IsNullOrEmpty(txtFrom.Text))
            {
                from = DateTime.ParseExact(txtFrom.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            else
            {
                from = DateTime.Today;
            }

            DateTime to;
            if (!string.IsNullOrEmpty(txtTo.Text))
            {
                to = DateTime.ParseExact(txtTo.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            else
            {
                to = from.AddMonths(1).AddDays(-1);
            }

            PageRedirect(string.Format("BookingByOperator.aspx?NodeId={0}&SectionId={1}&from={2}&to={3}", Node.Id, Section.Id, from.ToOADate(), to.ToOADate()));
        }
    }
}