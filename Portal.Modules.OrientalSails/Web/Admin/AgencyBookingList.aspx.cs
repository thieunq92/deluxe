using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CMS.ServerControls;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.UI;
using Portal.Modules.OrientalSails.Web.Util;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class AgencyBookingList : SailsAgencyAdminBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            pagerBookings.AllowCustomPaging = true;
            try
            {
                btnAll.Value = base.GetText("textStatusAll");
                btnApproved.Value = base.GetText("textStatusApproved");
                btnPending.Value = base.GetText("textStatusPending");
                btnRejected.Value = base.GetText("textStatusRejected");
                btnCancelled.Value = "Cancelled";

                if (!IsPostBack)
                {
                    LoadInfo();
                    GetDataSource();
                    rptBookingList.DataBind();
                }

                DateTime? date = DateTime.Today;
                int approved = Module.CountBookingByStatus(StatusType.Approved, date, UserIdentity);
                int rejected = Module.CountBookingByStatus(StatusType.Rejected, date, UserIdentity);
                int pending = Module.CountBookingByStatus(StatusType.Pending, date, UserIdentity);
                int cancelled = Module.CountBookingByStatus(StatusType.Cancelled, date, UserIdentity);
                int charter = Module.CountBookingByCharter(true, DateTime.Today);
                int onCharter = Module.CountBookingOnCharter(false, DateTime.Today);
                int needTransfer = Module.CountBookingOnCharter(true, DateTime.Today);

                string url = Request.RawUrl;

                url = ClearQueryString(url, "charter", "blocked", "Status", "transfer");
                url += "&Status=";
                btnApproved.Attributes.Add("onclick", string.Format("window.location='{0}{1}'", url, (int)StatusType.Approved));
                btnApproved.Value += string.Format("({0})", approved);
                btnRejected.Attributes.Add("onclick", string.Format("window.location='{0}{1}'", url, (int)StatusType.Rejected));
                btnRejected.Value += string.Format("({0})", rejected);
                btnPending.Attributes.Add("onclick", string.Format("window.location='{0}{1}'", url, (int)StatusType.Pending));
                btnPending.Value += string.Format("({0})", pending);
                btnCancelled.Attributes.Add("onclick", string.Format("window.location='{0}{1}'", url, (int)StatusType.Cancelled));
                btnCancelled.Value += string.Format("({0})", cancelled);

                url = ClearQueryString(url, "charter", "blocked", "Status", "transfer");
                btnAll.Attributes.Add("onclick", string.Format("window.location='{0}'", url));

                int accNew = Module.CountBookingByAccounting(AccountingStatus.New, null);
                int accModified = Module.CountBookingByAccounting(AccountingStatus.Modified, null);
                int accUpdated = Module.CountBookingByAccounting(AccountingStatus.Updated, null);
                url = Request.RawUrl;
                url = ClearQueryString(url, "Accounting");
                url += "&Accounting=";


                url = Request.RawUrl;
                url = ClearQueryString(url, "charter", "blocked", "Status", "transfer");
                url += "&blocked=";

                url = Request.RawUrl;
                url = ClearQueryString(url, "charter", "blocked", "Status", "transfer");

            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private void LoadInfo()
        {
            IList list = Module.TripGetAll(false);
            var trip = new SailsTrip();
            trip.Name = "All";
            list.Insert(0, trip);
            ddlOrgs.DataSource = Module.OrganizationGetAllRoot();
            ddlOrgs.DataTextField = "Name";
            ddlOrgs.DataValueField = "Id";
            ddlOrgs.DataBind();

            var list2 = Module.OrganizationGetByUser(UserIdentity);
            if (list2.Count == 1)
            {
                ddlOrgs.SelectedValue = ((UserOrganization)list2[0]).Organization.Id.ToString();
            }

            cddlTrips.DataSource = Module.TripGetAll(false, UserIdentity);
            cddlTrips.DataTextField = "Name";
            cddlTrips.DataValueField = "Id";
            cddlTrips.DataParentField = "OrgId";
            cddlTrips.ParentClientID = ddlOrgs.ClientID;
            cddlTrips.DataBind();

            if (!string.IsNullOrEmpty(Request.QueryString["TripId"]))
            {
                cddlTrips.SelectedValue = Request.QueryString["TripId"];
                var strip = Module.TripGetById(Convert.ToInt32(Request.QueryString["TripId"]));
                ddlOrgs.SelectedValue = strip.Organization.Id.ToString();
            }
            if (!string.IsNullOrEmpty(Request.QueryString["Status"]))
            {
                //ddlStatus.SelectedValue = Request.QueryString["Status"];
            }
            if (!string.IsNullOrEmpty(Request.QueryString["StartDate"]))
            {
                textBoxStartDate.Text = Request.QueryString["StartDate"];
            }

            if (!string.IsNullOrEmpty(Request.QueryString["Booking"]))
            {
                txtBookingId.Text = Request.QueryString["Booking"];
            }

            if (!string.IsNullOrEmpty(Request.QueryString["Customer"]))
            {
                txtCustomerName.Text = Request.QueryString["Customer"];
            }
        }

        private void GetDataSource()
        {
            var quey = new NameValueCollection(Request.QueryString);
            quey.Add("Deleted", "1");
            int count;
            rptBookingList.DataSource = Module.BookingSearchFromQueryString(quey, UseCustomBookingId, pagerBookings.PageSize, pagerBookings.CurrentPageIndex, out count, UserIdentity);
            pagerBookings.VirtualItemCount = count;
        }

        protected virtual string BuildQueryString()
        {
            string query = string.Empty;
            if (cddlTrips.SelectedIndex > 0)
            {
                query += string.Format("&TripId={0}", cddlTrips.SelectedValue);
            }
            //if (ddlStatus.SelectedIndex > 0)
            //{
            //    query += string.Format("&Status={0}", ddlStatus.SelectedValue);
            //}
            if (!string.IsNullOrEmpty(textBoxStartDate.Text))
            {
                query += string.Format("&StartDate={0}", textBoxStartDate.Text);
            }

            if (!string.IsNullOrEmpty(txtBookingId.Text))
            {
                query += string.Format("&Booking={0}", txtBookingId.Text);
            }

            if (!string.IsNullOrEmpty(txtCustomerName.Text))
            {
                query += string.Format("&Customer={0}", txtCustomerName.Text);
            }

            return query;
        }

        protected void rptBookingList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Control plhAccounting = e.Item.FindControl("plhAccounting");
            if (plhAccounting != null)
            {
                plhAccounting.Visible = CheckAccountStatus;
            }

            Booking item = e.Item.DataItem as Booking;
            if (item != null)
            {
                using (HyperLink hyperLink_Trip = e.Item.FindControl("hyperLink_Trip") as HyperLink)
                {
                    if (hyperLink_Trip != null)
                    {
                        if (item.Trip != null)
                        {
                            hyperLink_Trip.Text = item.Trip.Name;
                            hyperLink_Trip.NavigateUrl =
                                string.Format("AgencyBookingView.aspx?NodeId={0}&SectionId={1}&BookingId={2}", Node.Id,
                                    Section.Id,
                                    item.Id);
                        }
                    }
                }

                Literal litCode = e.Item.FindControl("litCode") as Literal;
                if (litCode != null)
                {
                    if (item.CustomBookingId > 0 && UseCustomBookingId)
                    {
                        litCode.Text = string.Format(BookingFormat, item.CustomBookingId);
                    }
                    else
                    {
                        litCode.Text = string.Format(BookingFormat, item.Id);
                    }
                }

                HyperLink hplCode = e.Item.FindControl("hplCode") as HyperLink;
                if (hplCode != null)
                {
                    if (item.CustomBookingId > 0 && UseCustomBookingId)
                    {
                        hplCode.Text = string.Format(BookingFormat, item.CustomBookingId);
                    }
                    else
                    {
                        hplCode.Text = string.Format(BookingFormat, item.Id);
                    }
                    hplCode.NavigateUrl = string.Format("AgencyBookingView.aspx?NodeId={0}&SectionId={1}&BookingId={2}",
                        Node.Id, Section.Id, item.Id);
                }

                Literal litPax = e.Item.FindControl("litPax") as Literal;
                if (litPax != null)
                {
                    litPax.Text = item.Pax.ToString();
                }

                if (ShowCustomerName)
                {
                    Label labelName = e.Item.FindControl("labelName") as Label;
                    if (labelName != null)
                    {
                        labelName.Text = item.CustomerName;
                    }
                }
                else
                {
                    e.Item.FindControl("columnCustomerName").Visible = false;
                }

                Label labelConfirmBy = e.Item.FindControl("labelConfirmBy") as Label;
                if (labelConfirmBy != null)
                {
                    if (item.ConfirmedBy != null)
                    {
                        labelConfirmBy.Text = item.Confirmer;
                    }
                    else
                    {
                        labelConfirmBy.Text = "";
                    }
                }

                HyperLink hplAgency = e.Item.FindControl("hplAgency") as HyperLink;
                if (hplAgency != null)
                {
                    if (item.Agency != null)
                    {
                        hplAgency.Text = item.Agency.Name;
                        //                        hplAgency.NavigateUrl = string.Format("AgencyEdit.aspx?NodeId={0}&SectionId={1}&AgencyId={2}",
                        //                            Node.Id, Section.Id, item.Agency.Id);
                        hplAgency.NavigateUrl = "#";
                    }
                    else
                    {
                        hplAgency.Text = SailsModule.NOAGENCY;
                    }
                }

                Literal litAgencyCode = e.Item.FindControl("litAgencyCode") as Literal;
                if (litAgencyCode != null)
                {
                    if (item.Agency != null)
                    {
                        litAgencyCode.Text = item.AgencyCode;
                    }
                }

                using (Label label_startDate = e.Item.FindControl("label_startDate") as Label)
                {
                    if (label_startDate != null)
                    {
                        label_startDate.Text = item.StartDate.ToString("dd/MM/yyyy");
                    }
                }

                using (Label label_Status = e.Item.FindControl("label_Status") as Label)
                {
                    if (label_Status != null)
                    {
                        label_Status.Text = Enum.GetName(typeof(StatusType), item.Status);
                    }
                }

                HtmlTableRow row = (HtmlTableRow)e.Item.FindControl("trItem");
                if (item.Status == StatusType.Pending)
                {
                    row.Attributes.Add("class", "--pending");
                }
                if (item.Status == StatusType.Rejected)
                {
                    row.Attributes.Add("class", "--cancelled");
                }
                if (item.Status == StatusType.Approved)
                {
                    row.Attributes.Add("class", "--approved");
                }

                if (!item.IsCharter)
                {
                    Locked locked = Module.LockedCheckByDate(item.Cruise, item.StartDate, item.EndDate);

                    if (locked != null && Module.LockedCheckCharter(locked))
                    {
                        if (item.IsTransferred)
                        {
                            row.Attributes.Add("style", "background-color: #AAAAAA");
                        }
                        else
                        {
                            row.Attributes.Add("style", "background-color: #9ABAFF");
                        }
                    }
                }
                else
                {
                    if (item.Status == StatusType.Approved)
                    {
                        row.Attributes.Add("style", "background-color: #FF00FF");
                    }
                }

                Literal litAccounting = e.Item.FindControl("litAccounting") as Literal;
                if (litAccounting != null)
                {
                    switch (item.AccountingStatus)
                    {
                        case AccountingStatus.New:
                            litAccounting.Text = Resources.textAccountingNew;
                            break;
                        case AccountingStatus.Modified:
                            litAccounting.Text = Resources.textAccountingModified;
                            break;
                        case AccountingStatus.Updated:
                            litAccounting.Text = Resources.textAccountingUpdated;
                            break;
                    }
                }
                using (HyperLink hyperLinkView = e.Item.FindControl("hyperLinkView") as HyperLink)
                {
                    if (hyperLinkView != null)
                    {
                        hyperLinkView.NavigateUrl =
                            string.Format("AgencyBookingView.aspx?NodeId={0}&SectionId={1}&BookingId={2}", Node.Id, Section.Id,
                                item.Id);
                    }
                }

                Literal litNote = e.Item.FindControl("litNote") as Literal;
                Image imgNote = e.Item.FindControl("imgNote") as Image;
                if (litNote != null && imgNote != null)
                {
                    if (!string.IsNullOrEmpty(item.Note))
                    {
                        litNote.Text = item.Note;
                    }
                    else
                    {
                        imgNote.Visible = false;
                    }
                }
            }
            else
            {
                e.Item.FindControl("columnCustomerName").Visible = ShowCustomerName;
            }
        }

        protected void rptBookingList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                Booking item = Module.BookingGetById(Convert.ToInt32(e.CommandArgument));
                switch (e.CommandName)
                {
                    case "Delete":
                        Module.Delete(item);
                        GetDataSource();
                        rptBookingList.DataBind();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        protected void buttonSearch_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                PageRedirect(string.Format("AgencyBookingList.aspx?NodeId={0}&SectionId={1}{2}", Node.Id, Section.Id,
                    BuildQueryString()));
            }
        }

        protected void pagerBookings_PageChanged(object sender, PageChangedEventArgs e)
        {
            GetDataSource();
            rptBookingList.DataBind();
        }

        private string ClearQueryString(string url, params string[] queryKey)
        {
            foreach (string str in queryKey)
            {
                if (Request.QueryString[str] != null)
                {
                    url = url.Replace(string.Format("&{1}={0}", Request.QueryString[str], str), "");
                }
            }
            return url;
        }
    }
}