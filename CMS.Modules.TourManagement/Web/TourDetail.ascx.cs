using System;
using System.Web.UI.WebControls;
using CMS.Core.Domain;
using CMS.Modules.TourManagement.Domain;
using CMS.Modules.TourManagement.Web.UI;
using CMS.Web.Util;

namespace CMS.Modules.TourManagement.Web
{
    public partial class TourDetail : TourControlBase
    {
        private Tour _tour;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Module != null)
            {
                string cssfile = Module.ThemePath + "tour.css";
                RegisterStylesheet("tourcss", cssfile);
            }

            if (Module != null)
            {
                _tour = Module.TourGetById(Module.TourId);

                #region -- Hien Thi --

                {
                    if (Page.User.Identity is User)
                    {
                        textBoxAuthor.Visible = false;
                        textBoxEmail.Visible = false;
                    }

                    #region -- Edit link --

                    if (Page.User.Identity is User && ((User) Page.User.Identity).CanEdit(Module.Section))
                    {
                        //hpEditLandscape.NavigateUrl =
                        //    String.Format(
                        //        "/Modules/Landscape/Admin/LandscapeEdit.aspx?NodeId={0}&SectionId={1}&LandscapeID={2}",
                        //        PageEngine.ActiveNode.Id, Module.Section.Id, _tour.Id);
                    }
                    else
                    {
                        //hpEditLandscape.Visible = false;
                    }

                    #endregion

                    #region -- Image --

                    if (string.IsNullOrEmpty(_tour.Image))
                    {
                        imageTour.ImageUrl = GlobalConst.NO_IMAGE;
                    }
                    else
                    {
                        imageTour.ImageUrl = _tour.Image;
                    }

                    #endregion

                    #region -- Album --

                    try
                    {
                        //if (Module.GallerySection != null)
                        //{
                        //    if (_tour.Album != null)
                        //    {
                        //        hyperLinkAlbum.NavigateUrl = Module.GetGalleryLinkFromAlbum(_tour.Album);
                        //        hyperLinkAlbum.Text = Strings.hyperLinkAlbum;
                        //        imageAlbum.ImageUrl = Module.ThemePath + "Images/album.gif";
                        //    }
                        //    else
                        //    {
                        //        hyperLinkAlbum.Visible = false;
                        //        hyperLinkAlbum.Text = String.Empty;
                        //        imageAlbum.Visible = false;
                        //    }
                        //}
                    }
                    catch (Exception)
                    {
                        //hyperLinkAlbum.Visible = false;
                        //hyperLinkAlbum.Text = String.Empty;
                        //imageAlbum.Visible = false;
                    }

                    #endregion

                    #region -- Info --

                    if (!IsPostBack)
                    {
                        labelDescription.Text = _tour.Name;
                        Description.InnerHtml = _tour.Summary;
                        pActivities.InnerHtml = _tour.Activities;
                        pHighLight.InnerHtml = _tour.TripHighLight;
                        pInclusion.InnerHtml = _tour.Inclusion;
                        pExclusion.InnerHtml = _tour.Exclusion;
                        pWhatToTake.InnerHtml = _tour.WhatToTake;
                        pTourInstruction.InnerHtml = _tour.TourInstruction;
                        rptReviews.DataSource = _tour.Comments;
                        rptReviews.DataBind();
                        if (_tour.StartFrom != null && _tour.EndIn != null)
                            labelLocation.Text = string.Format("Start from {0} and end in {1}", _tour.StartFrom.Name,
                                                               _tour.EndIn.Name);
                        if (_tour.NumberOfDay > 1)
                        {
                            labelTime.Text = string.Format("Duration: {0} days.", _tour.NumberOfDay);
                        }
                        else
                        {
                            labelTime.Text = string.Format("Duration: {0} day.", _tour.NumberOfDay);
                        }
                        hplOrder.Text = "Order now";
                        hplOrder.NavigateUrl = _tour.GetOrderUrl(Module.Section);
                    }

                    #endregion
                }

                #endregion
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            string[] item = new string[2];
            item[0] = _tour.Name;
            item[1] = Request.RawUrl;
            PageEngine.NavigationPath.Add(item);

            Module.Section.Title = _tour.Name.ToUpper();

            base.OnPreRender(e);
        }

        protected void buttonSubmit_Click(object sender, EventArgs e)
        {
            // Nếu đã đánh dấu I agree, có nội dung comment, có tên author, email hoặc đã đăng nhập
            if (chkAgree.Checked && !string.IsNullOrEmpty(textBoxComment.Text) && ((!string.IsNullOrEmpty(textBoxAuthor.Text) && !string.IsNullOrEmpty(textBoxEmail.Text))|| PageEngine.User.Identity is User))
            {
                if (textBoxVerifierCode.Text == imageVerifier.Text)
                {
                    TourComment comment = new TourComment();
                    comment.Deleted = false;
                    if (Page.User.Identity is User)
                    {
                        comment.Author = ((User) Page.User.Identity).FullName;
                        comment.AuthorId = ((User) Page.User.Identity);
                        comment.Email = ((User) Page.User.Identity).Email;
                    }
                    else
                    {
                        comment.Author = textBoxAuthor.Text;
                        comment.AuthorId = null;
                        comment.Email = textBoxEmail.Text;
                    }

                    comment.DateCreated = DateTime.Now;
                    comment.DateModified = DateTime.Now;
                    comment.Comment = textBoxComment.Text;
                    comment.Status = 0;
                    comment.Tour = _tour;
                    comment.IP = Request.UserHostAddress;
                    chkAgree.Checked = false;
                    textBoxComment.Text = string.Empty;

                    Module.SaveOrUpdate(comment);

                    rptReviews.DataSource = _tour.Comments;
                    rptReviews.DataBind();

                    imageVerifier.Refresh();
                }
                else
                {
                    labelFailedVerified.Visible = true;
                }
            }
        }

        protected void rptReviews_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is TourComment)
            {
                TourComment comment = (TourComment) e.Item.DataItem;
                Label labelAuthor = e.Item.FindControl("labelAuthor") as Label;
                if (labelAuthor != null)
                {
                    if (comment.AuthorId != null)
                    {
                        comment.Author = comment.AuthorId.FullName;
                    }
                    labelAuthor.Text = string.Format("{0:F} by {1}", comment.DateCreated, comment.Author);
                }

                Label labelContent = e.Item.FindControl("labelContent") as Label;
                if (labelContent != null)
                {
                    labelContent.Text = string.Format("{0}", comment.Comment);
                }
            }
        }
    }
}