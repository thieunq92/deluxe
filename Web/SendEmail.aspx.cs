using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Net;
using System.Net.Mail;

using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CMS.Web.Util;
using log4net;

namespace CMS.Web
{
    public partial class SendEmail : System.Web.UI.Page
    {
        #region -- Private Member --

        private readonly ILog _logger = LogManager.GetLogger(typeof(SendEmail));
        private MailMessage _message = new MailMessage();
        private SmtpClient _smtpClient = new SmtpClient();

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            string from = HttpContext.Current.Request.Url.Host;
            switch (from)
            {
                case "mekongtravels.com":
                case "www.mekongtravels.com":
                    labelYourName.Text = "Your name";
                    labelYourEmail.Text = "Your email";
                    labelSendTo.Text = "Send to";
                    labelSendToCC.Text = "CC to";
                    labelSubject.Text = "Subject";
                    labelMessage.Text = "Message";
                    buttonSendEmail.Text = "Send";
                    buttonClose.Text = "Close";
                    break;
                default:
                    break;
            }
        }

        protected void buttonSendEmail_Click(object sender, EventArgs e)
        {
            try
            {
                MailAddress fromAddress;

                string from = HttpContext.Current.Request.Url.Host;
                switch (from)
                {
                    case "quanlytour.com":
                        fromAddress = new MailAddress("no-reply@quanlytour.com", "Quan Ly Tour");
                        break;
                    case "www.quanlytour.com":
                        fromAddress = new MailAddress("no-reply@quanlytour.com", "Quan Ly Tour");
                        break;
                    case "mekongtravels.com":
                        fromAddress = new MailAddress("no-reply@mekongtravels.com", "Mekong travels");
                        break;
                    case "www.mekongtravels.com":
                        fromAddress = new MailAddress("no-reply@mekongtravels.com", "Mekong travels");
                        break;
                    default:
                        fromAddress = new MailAddress("no-reply@conggiaoduc.com", "Cong Giao Duc");
                        break;
                }
                
                MailAddress toAddress = new MailAddress(textBoxSendTo.Text);

                _smtpClient.Host = "smtp.gmail.com";
                _smtpClient.Port = 587;
                _smtpClient.Credentials = new NetworkCredential("no-reply@bitcorp.vn","8Z5235");
                _smtpClient.EnableSsl = true;
                _message.From = fromAddress;
                _message.To.Add(toAddress);
                if(!string.IsNullOrEmpty(textBoxSendToCC.Text))
                {
                    MailAddress ccAddress = new MailAddress(textBoxSendToCC.Text);
                    _message.CC.Add(ccAddress);
                }
                
                _message.Subject = textBoxSubject.Text;
                _message.IsBodyHtml = true;
                _message.BodyEncoding = Encoding.UTF8;

                string content =  string.Format(@"{0} (<a href='mailto:{1}' target='_blank'>{1}</a>) <br /> Gửi cho bạn: {2} <br /> {3}",textBoxYourName.Text,textBoxYourEmail.Text,Request.QueryString["url"],textBoxMessage.Text);

                _message.Body = content;
                
                _smtpClient.Send(_message);
                labelSendStatus.Visible = true;
                labelSendStatus.Text = "OK!";
            }
            catch (Exception ex)
            {
                _logger.Error("Error when Send Email", ex);
                labelSendStatus.Text = ex.Message;
            }
        }
    }
}
