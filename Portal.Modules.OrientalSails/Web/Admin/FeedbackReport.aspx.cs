using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CMS.Web.Util;
using GemBox.Spreadsheet;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.UI;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class FeedbackReport : SailsAdminBasePage
    {
        #region -- PRIVATE MEMBERS --
        private AnswerGroup _currentGroup;

        private Question _currentQuestion;
        #endregion

        #region -- PAGE EVENTS --
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                #region -- Search and data --
                ddlGroups.DataSource = Module.QuestionGroupGetAll();
                ddlGroups.DataTextField = "Name";
                ddlGroups.DataValueField = "Id";
                ddlGroups.DataBind();

                rptGroups.DataSource = Module.QuestionGroupGetAll();
                rptGroups.DataBind();

                ddlCruises.DataSource = Module.CruiseGetAll();
                ddlCruises.DataTextField = "Name";
                ddlCruises.DataValueField = "Id";
                ddlCruises.DataBind();
                ddlCruises.Items.Insert(0, "-- All cruises --");

                rptFeedback.DataSource = Module.FeedbackReport(Request.QueryString);
                rptFeedback.DataBind();

                if (Request.QueryString["group"] != null)
                {
                    QuestionGroup group = Module.QuestionGroupGetById(Convert.ToInt32(Request.QueryString["group"]));
                    rptQuestions.DataSource = group.Questions;
                    rptQuestions.DataBind();
                    ddlGroups.SelectedValue = group.Id.ToString();

                    #region -- Summarized --
                    if (Request.QueryString["group"] != null)
                    {
                        rptAnswers.DataSource = group.Selections;
                        rptAnswers.DataBind();
                        rptQuestionsReport.DataSource = group.Questions;
                        rptQuestionsReport.DataBind();
                    }
                    #endregion
                }

                if (Request.QueryString["cruise"] != null)
                {
                    ddlCruises.SelectedValue = Request.QueryString["cruise"];
                }
                if (Request.QueryString["from"] != null)
                {
                    DateTime from = DateTime.FromOADate(Convert.ToDouble(Request.QueryString["from"]));
                    txtFrom.Text = from.ToString("dd/MM/yyyy");
                }
                if (Request.QueryString["to"] != null)
                {
                    DateTime to = DateTime.FromOADate(Convert.ToDouble(Request.QueryString["to"]));
                    txtTo.Text = to.ToString("dd/MM/yyyy");
                }
                #endregion


            }
        }
        #endregion

        #region -- CONTROL EVENTS --

        #region -- Data grid --
        protected void rptOptions_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is Question)
            {
                Question question = (Question)e.Item.DataItem;
                AnswerOption op = _currentGroup.AnswerSheet.GetOption(question);
                if (op.Option > 0)
                    ValueBinder.BindLiteral(e.Item, "litOption", _currentGroup.Group.Selections[op.Option - 1]);
            }
        }

        protected void rptFeedback_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is AnswerGroup)
            {
                AnswerGroup group = (AnswerGroup)e.Item.DataItem;
                ValueBinder.BindLiteral(e.Item, "litDate", group.AnswerSheet.Date);
                //ValueBinder.BindLiteral(e.Item, "litCruise", group.AnswerSheet.Cruise.Name);
                ValueBinder.BindLiteral(e.Item, "litName", group.AnswerSheet.Name);
                ValueBinder.BindLiteral(e.Item, "litAddress", group.AnswerSheet.Address);
                ValueBinder.BindLiteral(e.Item, "litEmail", group.AnswerSheet.Email);
                ValueBinder.BindLiteral(e.Item, "litNote", group.Comment);
                int current = pagerFeedback.CurrentPageIndex;
                if (current < 0) current = 0;
                ValueBinder.BindLiteral(e.Item, "litIndex", e.Item.ItemIndex + current * pagerFeedback.PageSize + 1);
                _currentGroup = group;

                Repeater rptOptions = (Repeater)e.Item.FindControl("rptOptions");
                rptOptions.DataSource = group.Group.Questions;
                rptOptions.DataBind();

                HtmlAnchor anchorFeedback = e.Item.FindControl("anchorFeedback") as HtmlAnchor;
                if (anchorFeedback != null)
                {
                    string url = string.Format("SurveyInput.aspx?NodeId={0}&SectionId={1}&sheetid={2}", Node.Id,
                                               Section.Id, group.AnswerSheet.Id);
                    anchorFeedback.Attributes.Add("onclick",
                                                  CMS.ServerControls.Popup.OpenPopupScript(url, "Survey input", 600, 800));
                }

                HtmlAnchor anchorEmail = e.Item.FindControl("anchorEmail") as HtmlAnchor;
                if (anchorEmail != null)
                {
                    string url = string.Format("FeedbackMail.aspx?NodeId={0}&SectionId={1}&sheetid={2}", Node.Id,
                                               Section.Id, group.AnswerSheet.Id);
                    anchorEmail.Attributes.Add("onclick",
                                                  CMS.ServerControls.Popup.OpenPopupScript(url, "Survey input", 600, 800));
                }

                Literal trItem = (Literal)e.Item.FindControl("trItem");
                if (trItem != null)
                {
                    if (group.AnswerSheet.IsSent)
                    {
                        trItem.Text = @"<tr class='sent'>";
                    }
                }

                HyperLink hplBooking = e.Item.FindControl("hplBooking") as HyperLink;
                if (hplBooking != null)
                {
                    if (group.AnswerSheet.Booking != null)
                    {
                        hplBooking.Text = string.Format(BookingFormat, group.AnswerSheet.Booking.Id);
                        hplBooking.NavigateUrl = string.Format("BookingView.aspx?NodeId={0}&SectionId={1}&bookingid={2}",
                                                               Node.Id, Section.Id, group.AnswerSheet.Booking.Id);
                        ValueBinder.BindLiteral(e.Item, "litTrip", group.AnswerSheet.Booking.Trip.TripCode);
                    }
                }

                HyperLink hplCruise = e.Item.FindControl("hplCruise") as HyperLink;
                if (hplCruise != null)
                {
                    hplCruise.Text = group.AnswerSheet.Cruise.Name;
                    hplCruise.NavigateUrl = AddQuery("cruise", group.AnswerSheet.Cruise.Id.ToString());
                }

                HyperLink hplGuide = e.Item.FindControl("hplGuide") as HyperLink;
                if (hplGuide != null)
                {
                    hplGuide.Text = group.AnswerSheet.Guide;
                    hplGuide.NavigateUrl = AddQuery("guide", HttpUtility.UrlEncode(group.AnswerSheet.Guide));
                }

                HyperLink hplDriver = e.Item.FindControl("hplDriver") as HyperLink;
                if (hplDriver != null)
                {
                    hplDriver.Text = group.AnswerSheet.Driver;
                    hplDriver.NavigateUrl = AddQuery("driver", HttpUtility.UrlEncode(group.AnswerSheet.Guide));
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string query = string.Empty;
            if (!string.IsNullOrEmpty(txtFrom.Text))
            {
                DateTime from = DateTime.ParseExact(txtFrom.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                query += "&from=" + from.ToOADate();
            }
            if (!string.IsNullOrEmpty(txtTo.Text))
            {
                DateTime to = DateTime.ParseExact(txtTo.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                query += "&to=" + to.ToOADate();
            }
            query += "&group=" + ddlGroups.SelectedValue;
            if (ddlCruises.SelectedIndex > 0)
            {
                query += "&cruise=" + ddlCruises.SelectedValue;
            }
            PageRedirect(string.Format("FeedbackReport.aspx?NodeId={0}&SectionId={1}{2}", Node.Id, Section.Id, query));
        }

        protected void lbtDelete_Click(object sender, EventArgs e)
        {
            IButtonControl btn = (IButtonControl)sender;
            AnswerGroup group = Module.AnswerGroupGetById(Convert.ToInt32(btn.CommandArgument));
            group.AnswerSheet.Deleted = true;
            Module.SaveOrUpdate(group.AnswerSheet);
            //throw new NotImplementedException();

            rptFeedback.DataSource = Module.FeedbackReport(Request.QueryString);
            rptFeedback.DataBind();
        }

        protected void btnExportAll_Click(object sender, EventArgs e)
        {
            #region -- excel --
            NameValueCollection query = new NameValueCollection(Request.QueryString);

            #region -- Xuất dữ liệu ra excel dùng thư viện --

            string tpl = Server.MapPath("/Modules/Sails/Admin/ExportTemplates/Feedback.xls");
            ExcelFile excelFile = new ExcelFile();
            excelFile.LoadXls(tpl);

            ExcelWorksheet sheet = excelFile.Worksheets[0];

            int sheetIndex = 0;
            foreach (QuestionGroup qGroup in Module.QuestionGroupGetAll())
            {
                excelFile.Worksheets[sheetIndex].InsertCopy(qGroup.Name + qGroup.Id, excelFile.Worksheets[sheetIndex]);
                sheet = excelFile.Worksheets[sheetIndex];
                sheetIndex++;
                query["group"] = qGroup.Id.ToString();
                IList dataSource = Module.FeedbackReport(query);

                // Dòng dữ liệu đầu tiên
                const int firstrow = 6;
                int crow = firstrow;
                sheet.Rows[crow].InsertCopy(dataSource.Count - 1, sheet.Rows[firstrow]);

                for (int ii = qGroup.Questions.Count - 1; ii >= 0; ii--)
                {
                    Question q = qGroup.Questions[ii];
                    sheet.Columns[200].Delete();
                    sheet.Columns[10].InsertCopy(1, sheet.Columns[9]);
                    sheet.Cells[firstrow - 1, 10].Value = q.Name;
                }

                foreach (AnswerGroup group in dataSource)
                {
                    sheet.Cells[crow, 0].Value = crow - firstrow + 1;
                    sheet.Cells[crow, 1].Value = group.AnswerSheet.Date;
                    if (group.AnswerSheet.Booking != null)
                    {
                        sheet.Cells[crow, 2].Value = group.AnswerSheet.Booking.BookingIdOS;
                        sheet.Cells[crow, 5].Value = group.AnswerSheet.Booking.Trip.TripCode;
                    }
                    sheet.Cells[crow, 3].Value = group.AnswerSheet.Name;
                    sheet.Cells[crow, 4].Value = group.AnswerSheet.Cruise.Name;
                    sheet.Cells[crow, 6].Value = group.AnswerSheet.Guide;
                    sheet.Cells[crow, 7].Value = group.AnswerSheet.Driver;
                    sheet.Cells[crow, 8].Value = group.AnswerSheet.Address;
                    sheet.Cells[crow, 9].Value = group.AnswerSheet.Email;

                    int ii = 1;
                    foreach (Question q in qGroup.Questions)
                    {
                        int opt = group.AnswerSheet.GetOption(q).Option;
                        if (opt > 0)
                        {
                            sheet.Cells[crow, 9 + ii].Value = group.Group.Selections[opt - 1];
                        }
                        ii++;
                    }
                    sheet.Cells[crow, 9 + ii].Value = group.Comment;
                    crow += 1;
                }
            }
            if (sheetIndex > 0)
                excelFile.Worksheets[sheetIndex].Delete();

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AppendHeader("content-disposition",
                                  "attachment; filename=" + string.Format("feedback.xls"));

            MemoryStream m = new MemoryStream();

            excelFile.SaveXls(m);

            Response.OutputStream.Write(m.GetBuffer(), 0, m.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();

            m.Close();
            Response.End();
            #endregion

            #endregion
        }

        protected void rptGroups_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is QuestionGroup)
            {
                QuestionGroup group = (QuestionGroup)e.Item.DataItem;
                HtmlGenericControl liMenu = e.Item.FindControl("liMenu") as HtmlGenericControl;
                if (liMenu != null)
                {
                    if (group.Id.ToString() == Request.QueryString["group"])
                    {
                        liMenu.Attributes.Add("class", "selected");
                    }
                }

                HyperLink hplGroup = e.Item.FindControl("hplGroup") as HyperLink;
                if (hplGroup != null)
                {
                    hplGroup.Text = group.Name;
                    hplGroup.NavigateUrl = AddQuery("group", group.Id.ToString());
                }
            }
        }

        protected string AddQuery(string key, string value)
        {
            string query = Request.RawUrl;
            if (Request.QueryString[key] != null)
            {
                return query.Replace(string.Format("{0}={1}", key, Request.QueryString[key]), string.Format("{0}={1}", key, value));
            }
            query += string.Format("&{0}={1}", key, value);
            return query;
        }
        #endregion

        #region -- Summary --
        protected void rptQuestionsReport_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is Question)
            {
                _currentQuestion = (Question)e.Item.DataItem;
                ValueBinder.BindLiteral(e.Item, "litQuestion", _currentQuestion.Name);
                Repeater rptAnswerData = (Repeater)e.Item.FindControl("rptAnswerData");
                rptAnswerData.DataSource = _currentQuestion.Group.Selections;
                rptAnswerData.DataBind();
            }
        }

        protected void rptAnswerData_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is string)
            {
                string str = e.Item.DataItem.ToString();
                int choice = _currentQuestion.Group.Selections.IndexOf(str) + 1;
                // Đếm số choice = 1
                int total;
                int count = Module.ChoiceReport(Request.QueryString, _currentQuestion, choice, out total);

                ValueBinder.BindLiteral(e.Item, "litCount", count);
                ValueBinder.BindLiteral(e.Item, "litPercentage", ((double)count * 100 / total).ToString("#0.##") + "%");
            }
        }

        #endregion
        #endregion
    }
}