using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CMS.Core.Domain;
using CMS.Modules.TourManagement.Domain;
using CMS.Modules.TourManagement.Web.UI;
using CMS.ServerControls;
using CMS.Web.Admin.Controls;
using log4net;
using AccessLevel=CMS.Core.Domain.AccessLevel;

namespace CMS.Modules.TourManagement.Web.Admin
{
    public partial class AdminComments : TourAdminBasePage
    {
        #region -- PRIVATE MEMBERS --

        private readonly ILog _logger = LogManager.GetLogger(typeof(AdminComments));
        //private Domain.Landscape _landscape;

        #endregion

        #region -- PAGE EVENTS --

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra quyền: chỉ có quyền Administrator hoặc toàn quyền với section mới được sử dụng trang này
                if (!UserIdentity.HasPermission(AccessLevel.Administrator))
                {
                    ShowError(Resources.textModifyDenied);
                    return;
                }

                if (!UserIdentity.HasFullPermission(Module.Section))
                {
                    ShowError(Resources.textModifyDenied);
                    return;
                }

                // Nếu có truyền vào hotel id thì đây là trang quản lý comment cho hotel
                // Nếu không thì là trang quản lý comment chung
                if (!string.IsNullOrEmpty(Request.QueryString["TourId"]))
                {
                    //_landscape = Module.LandscapeGetById(Convert.ToInt32(Request.QueryString["HotelId"]));
                    //HotelInfo.Hotel = _landscape;
                }
                else
                {
                    divHotelView.Visible = false;
                    //_landscape = null;
                    //HotelInfo.Visible = false;
                }
                pagerComments.AllowCustomPaging = true;

                if (!IsPostBack)
                {
                    // Gán tiêu đề, lấy data và gán lại dữ liệu trên query xuống search panel
                    Title = Resources.labelCommentManagement;
                    GetDataSources();
                    GetFromQueryString();
                }

                // Đăng ký file java script hover text
                ScriptManager.RegisterClientScriptInclude(Page, GetType(), "dropdown", "/js/dropdown.js");
            }
            catch (Exception ex)
            {
                _logger.Error("Page load error", ex);
                ShowError(ex.Message);
            }
        }

        #endregion

        #region -- CONTROLS EVENTS --

        protected void btnSearch_click(object sender, EventArgs e)
        {
            string queryString = string.Empty;
            if (((UserSelector)userPosted).SelectedUserId > 0)
            {
                queryString += "&UserId=" + ((UserSelector)userPosted).SelectedUserId;
            }

            if (!string.IsNullOrEmpty(txtPostedBefore.Text))
            {
                queryString += "&Before=" + txtPostedBefore.Text;
            }

            if (!string.IsNullOrEmpty(txtPostedAfter.Text))
            {
                queryString += "&After=" + txtPostedAfter.Text;
            }

            if (!string.IsNullOrEmpty(txtInContent.Text))
            {
                queryString += "&Content=" + txtInContent.Text;
            }

            if (!string.IsNullOrEmpty(txtIP.Text))
            {
                queryString += "&IP=" + txtIP.Text;
            }

            if (!string.IsNullOrEmpty(Request.QueryString["TourId"]))
            {
                queryString += "&TourId=" + Request.QueryString["TourId"];
            }

            PageRedirect(string.Format("AdminComments.aspx?NodeId={0}&SectionId={1}{2}", Node.Id, Section.Id,
                                       queryString));
        }

        protected void rptComments_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName.ToLower())
            {
                case "delete":
                    Module.Delete(Module.CommentGetById(Convert.ToInt32(e.CommandArgument)));
                    GetDataSources();
                    break;
            }
        }

        protected void rptComments_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is TourComment)
            {
                TourComment comment = (TourComment)e.Item.DataItem;
                HyperLink labelUserName = e.Item.FindControl("labelUserName") as HyperLink;
                if (labelUserName != null)
                {
                    if (comment.AuthorId != null)
                    {
                        labelUserName.Text = comment.AuthorId.UserName;
                        labelUserName.NavigateUrl =
                            string.Format("AdminComments.aspx?NodeId={0}&SectionId={1}&UserId={2}", Node.Id, Section.Id,
                                          comment.AuthorId.Id);
                    }
                    else
                    {
                        labelUserName.Text = "Anonymous";
                        labelUserName.NavigateUrl =
                            string.Format("AdminComments.aspx?NodeId={0}&SectionId={1}&UserId={2}", Node.Id, Section.Id,
                                          0);
                    }
                }

                Label labelFullName = e.Item.FindControl("labelFullName") as Label;
                if (labelFullName != null)
                {
                    if (comment.AuthorId != null)
                    {
                        labelFullName.Text = comment.AuthorId.FullName;
                    }
                    else
                    {
                        labelFullName.Text = comment.Author;
                    }
                }

                HyperLink labelEmail = e.Item.FindControl("labelEmail") as HyperLink;
                if (labelEmail != null)
                {
                    if (comment.AuthorId != null)
                    {
                        labelEmail.Text = comment.AuthorId.Email;
                    }
                    else
                    {
                        labelEmail.Text = comment.Email;
                    }
                    labelEmail.NavigateUrl = "mailto:" + comment.Email;
                }

                HyperLink labelIP = e.Item.FindControl("labelIP") as HyperLink;
                if (labelIP != null)
                {
                    labelIP.Text = comment.IP;
                    labelIP.NavigateUrl = string.Format("AdminComments.aspx?NodeId={0}&SectionId={1}&IP={2}", Node.Id,
                                                        Section.Id, comment.IP);
                }

                HyperLink labelLandscape = e.Item.FindControl("labelLandscape") as HyperLink;
                if (labelLandscape != null)
                {
                    labelLandscape.Text = comment.Tour.Name;
                    labelLandscape.NavigateUrl = string.Format("AdminComments.aspx?NodeId={0}&SectionId={1}&TourId={2}",
                                                           Node.Id,
                                                           Section.Id, comment.Tour.Id);
                }

                HtmlGenericControl labelContent = e.Item.FindControl("labelContent") as HtmlGenericControl;
                if (labelContent != null)
                {
                    labelContent.Attributes.Add("class", "hover_content");
                    labelContent.InnerHtml = comment.Comment;
                }

                Label labelOnHover = e.Item.FindControl("labelOnHover") as Label;
                if (labelOnHover != null && labelContent != null)
                {
                    labelOnHover.Text = "View content";
                    labelOnHover.Attributes.Add("onmouseover",
                                                "at_show_if('" + labelOnHover.ClientID + "','" + labelContent.ClientID +
                                                "');");
                }

                HyperLink labelPosted = e.Item.FindControl("labelPosted") as HyperLink;
                if (labelPosted != null)
                {
                    labelPosted.Text = comment.DateCreated.ToString("dd/MM/yyyy");
                    labelPosted.NavigateUrl = string.Format("AdminComments.aspx?NodeId={0}&SectionId={1}&Posted={2}",
                                                            Node.Id,
                                                            Section.Id, labelPosted.Text);
                }
            }
        }

        protected void pagerComments_PageChanged(object sender, PageChangedEventArgs e)
        {
            GetDataSources();
        }

        #endregion

        #region -- PRIVATE METHODS

        private void GetDataSources()
        {
            int count;
            rptComments.DataSource = Module.CommentsGetByQueryStringPaged(Request.QueryString,
                                                                          pagerComments.PageSize,
                                                                          pagerComments.CurrentPageIndex, out count);

            pagerComments.VirtualItemCount = count;
            rptComments.DataBind();
            return;
        }

        protected void GetFromQueryString()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["UserId"]))
            {
                int userId = Convert.ToInt32(Request.QueryString["UserId"]);
                if (userId > 0)
                {
                    User user = Module.UserGetById(Convert.ToInt32(Request.QueryString["UserId"]));
                    ((UserSelector)userPosted).SelectedUserId = user.Id;
                    ((UserSelector)userPosted).SelectedUser = user.UserName;
                }
            }

            if (!string.IsNullOrEmpty(Request.QueryString["Before"]))
            {
                txtPostedBefore.Text = Request.QueryString["Before"];
            }

            if (!string.IsNullOrEmpty(Request.QueryString["After"]))
            {
                txtPostedAfter.Text = Request.QueryString["After"];
            }

            if (!string.IsNullOrEmpty(Request.QueryString["Content"]))
            {
                txtInContent.Text = Request.QueryString["Content"];
            }

            if (!string.IsNullOrEmpty(Request.QueryString["IP"]))
            {
                txtIP.Text = Request.QueryString["IP"];
            }
        }

        #endregion
    }
}
