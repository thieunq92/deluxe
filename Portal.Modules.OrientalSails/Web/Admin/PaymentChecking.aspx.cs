using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CMS.Core.Domain;
using CMS.Web.Util;
using NHibernate.Criterion;
using Portal.Modules.OrientalSails.Web.UI;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.Util;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class PaymentChecking : SailsAdminBasePage
    {

        #region -- PAGE EVENTS --
        protected void Page_Load(object sender, EventArgs e)
        {
            pagerBookings.AllowCustomPaging = true;
            if (!IsPostBack)
            {
                GetDataSource();
            }
        }
        #endregion

        #region -- METHODS --
        private void GetDataSource(bool allMode = false)
        {
            ICriterion crit = Expression.Eq("Deleted", false);
            crit = Expression.And(crit, Expression.Eq("Status", StatusType.Approved));

            // Điều kiện: giá trị thanh toán guide chưa hết như đã paid
            // Độ lệch nằm lớn hơn $0.5 thì mới lấy
            

            var guideCrit = Expression.And(Expression.Or(Expression.Gt("GuideCollectedRemain", 0.5D), Expression.Lt("GuideCollectedRemain", -0.5D)),
                                           Expression.Eq("GuideCollected", true));
            if (!allMode)
            {
                guideCrit = Expression.And(guideCrit, Expression.Eq("GuideConfirmed", false));
            }

            // Điều kiện: giá trị thanh toán booking chưa hết nhưng đã paid
            var agencyCrit = Expression.And(Expression.Or(Expression.Gt("AgencyRemain", 0.5D), Expression.Lt("AgencyRemain", -0.5D)),
                                    Expression.Eq("IsPaid", true));

            if (!allMode)
            {
                agencyCrit = Expression.And(agencyCrit, Expression.Eq("AgencyConfirmed", false));
            }

            crit = Expression.And(crit, Expression.Or(agencyCrit, guideCrit));

            rptBookings.DataSource = Module.BookingGetByCriterion(crit, Order.Desc("StartDate"), pagerBookings.PageSize, pagerBookings.CurrentPageIndex);
            pagerBookings.VirtualItemCount = Module.CountBookingByCriterion(crit);
            rptBookings.DataBind();
        }
        #endregion

        protected void rptBookings_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is Booking)
            {                
                var booking = (Booking) e.Item.DataItem;
                if (booking.GuideCollectedRemain + booking.AgencyRemain == 0)
                {
                    e.Item.Visible = false;
                    return;
                }

                ValueBinder.BindLiteral(e.Item, "litGuideRemain", booking.GuideCollectedRemain);
                ValueBinder.BindLiteral(e.Item, "litPartnerRemain", booking.AgencyRemain);
                ValueBinder.BindLiteral(e.Item, "litService", booking.Trip.Name);

                var hplBookingCode = (HyperLink) e.Item.FindControl("hplBookingCode");
                if (hplBookingCode != null)
                {
                    hplBookingCode.Text = Module.BookingCode(booking);
                    hplBookingCode.NavigateUrl = string.Format(
                        "BookingView.aspx?NodeId={0}&SectionId={1}&bookingid={2}", Node.Id, Section.Id, booking.Id);
                }

                var hplDate = (HyperLink) e.Item.FindControl("hplDate");
                hplDate.Text = booking.StartDate.ToString("dd/MM/yyyy");

                if (booking.Agency != null && booking.Agency.Sale != null)
                {
                    var hplSale = (HyperLink) e.Item.FindControl("hplSale");
                    hplSale.Text = booking.Agency.Sale.AllName;
                }

                if (booking.Agency != null)
                {
                    var hplPartner = (HyperLink) e.Item.FindControl("hplPartner");
                    hplPartner.Text = booking.Agency.Name;
                }

                var aPayment = e.Item.FindControl("aPayment") as HtmlAnchor;
                if (aPayment != null)
                {
                    string url = string.Format("BookingPayment.aspx?NodeId={0}&SectionId={1}&BookingId={2}", Node.Id,
                                               Section.Id, booking.Id);
                    aPayment.Attributes.Add("onclick",
                                            CMS.ServerControls.Popup.OpenPopupScript(url, "Payment", 600, 500));
                }

                if (UserIdentity.HasPermission(AccessLevel.Administrator))
                {
                    if (booking.GuideCollectedRemain != 0 && booking.GuideCollected && !booking.GuideConfirmed)
                    {
                        ValueBinder.ShowControl(e.Item, "lbtGuideConfirm");
                    }

                    if (booking.AgencyRemain != 0 && booking.IsPaid && !booking.AgencyConfirmed)
                    {
                        ValueBinder.ShowControl(e.Item, "lbtAgencyConfirm");
                    }

                    if (Math.Abs(booking.AgencyRemain) > 10 || Math.Abs(booking.GuideCollectedRemain) > 10)
                    {
                        var trRow = (HtmlTableRow) e.Item.FindControl("trRow");
                        trRow.Style.Add("background-color", "#FBFB00");
                    }

                    if (Math.Abs(booking.AgencyRemain) > 200 || Math.Abs(booking.GuideCollectedRemain) > 200)
                    {
                        var trRow = (HtmlTableRow)e.Item.FindControl("trRow");
                        trRow.Style.Add("background-color", "#FF7F7F");
                    }
                }
            }
        }

        protected void lbtGuideConfirm_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(((IButtonControl) sender).CommandArgument);
            Booking booking = Module.BookingGetById(id);
            booking.GuideConfirmed = true;
            Module.SaveOrUpdate(booking);
            GetDataSource();
        }

        protected void lbtAgencyConfirm_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(((IButtonControl)sender).CommandArgument);
            Booking booking = Module.BookingGetById(id);
            booking.AgencyConfirmed = true;
            Module.SaveOrUpdate(booking);
            GetDataSource();
        }
    }
}