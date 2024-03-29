using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.UI;
using Portal.Modules.OrientalSails.Web.Util;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class SendEmailPage : SailsAdminBasePage
    {
        private const string NO_EMAIL = "Unable to obtain email address";
        private const string APPROVED_SUBJECT = "Approved for your booking in {0:dd/MM/yyyy}";
        private const string REJECTED_SUBJECT = "Booking in {0:dd/MM/yyyy} rejected";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Booking booking = Module.BookingGetById(Convert.ToInt32(Request.QueryString["BookId"]));
                if (!string.IsNullOrEmpty(booking.Email))
                {
                    txtEmailTo.Text = booking.Email;
                }
                else
                {
                    if (booking.CreatedBy != null)
                    {
                        txtEmailTo.Text = booking.CreatedBy.Email;
                    }
                    else
                    {
                        txtEmailTo.Text = NO_EMAIL;
                    }
                }

                StatusType status = (StatusType)Convert.ToInt32(Request.QueryString["status"]);
                if (Request.QueryString["status"] == null)
                {
                    status = booking.Status;
                }
                switch (status)
                {
                    case StatusType.Approved:
                        StreamReader appReader = new StreamReader(Server.MapPath("/Modules/Sails/Admin/EmailTemplate/Approved.txt"));
                        string appFormat = appReader.ReadToEnd();
                        txtSubject.Text = string.Format(APPROVED_SUBJECT, booking.StartDate);
                        fckContent.Value = string.Format(appFormat,
                            booking.BookerName, booking.AgencyCode, string.Format(BookingFormat, booking.Id), booking.Trip.Name, booking.StartDate.ToString("dd/MM/yyyy"),
                            booking.Pax, booking.CustomerName, "", booking.PickupAddress, booking.SpecialRequest,booking.Total,"","","");
                        break;
                    case StatusType.Rejected:
                        StreamReader rejReader = new StreamReader(Server.MapPath("/Modules/Sails/Admin/EmailTemplate/Approved.txt"));
                        string rejFormat = rejReader.ReadToEnd();
                        txtSubject.Text = string.Format(REJECTED_SUBJECT, booking.StartDate);
                        fckContent.Value =
                            string.Format(
                                "Dear {0},<br/>We are very sorry to inform  you that we can not accept your bookings:<br/>"
                                +
                                "Booking ID: {1}<br/>Partner: {2}<br/>Start date: {3:dd/MM/yyyy}<br/>Number of pax: {4}<br/>"
                                +
                                @"Customer name:<br/> {5}<br/>Service: {6}<br/>{7}<br/>Room type:{8}<br/>Confirm by {9}<br/> Thank you very much for your supports,",
                                booking.ContactName, booking.Id, booking.CreatedBy.FullName, booking.StartDate,
                                booking.Adult,
                                booking.CustomerName, booking.Trip.Name, "", booking.RoomName, UserIdentity.FullName);
                        break;
                    default:
                        break;
                }
            }
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            // Đăng nhập            
            SmtpClient smtpClient = new SmtpClient("mail.asianatravelmate.com");
            smtpClient.Credentials = new NetworkCredential("dailygroup@asianatravelmate.com", "group@##123");
            //smtpClient.EnableSsl = true;

            // Địa chỉ email người gửi
            MailAddress fromAddress = new MailAddress(UserIdentity.Email);


            MailMessage message = new MailMessage();
            message.From = fromAddress;
            message.To.Add(txtEmailTo.Text);
            message.Subject = txtSubject.Text;
            message.IsBodyHtml = true;
            message.BodyEncoding = Encoding.UTF8;
            message.Body = fckContent.Value;

            smtpClient.Send(message);
            ClientScript.RegisterClientScriptBlock(typeof(SendEmail), "closure", "window.close()", true);
        }
    }
}
