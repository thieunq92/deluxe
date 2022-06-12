using System;
using System.Configuration;
using System.Data;
using aspNetEmail;
using BIT.Core.Extensions.Services.Email.Interface;
using BIT.Core.Extensions.Util;

namespace BIT.Core.Extensions.Services.Email
{
    public class aspNetEmailSender : IEmailSender
    {
        #region Private properties

        private readonly EmailMessage _mail;

        private int _batchSize;
        private string _charset;
        private int _pause;

        #endregion

        #region Public properties

        public string ToMail
        {
            get { return _mail.To; }
            set { _mail.To = value; }
        }

        public string Cc
        {
            get { return _mail.Cc; }
            set { _mail.Cc = value; }
        }

        public string Bcc
        {
            get { return _mail.Bcc; }
            set { _mail.Bcc = value; }
        }

        public string ReplyTo
        {
            get { return _mail.ReplyTo; }
            set { _mail.ReplyTo = value; }
        }

        public string FromName
        {
            get { return _mail.FromName; }
            set { _mail.FromName = value; }
        }

        public string FromMail
        {
            get { return _mail.FromAddress; }
            set { _mail.FromAddress = value; }
        }

        public string Subject
        {
            get { return _mail.Subject; }
            set { _mail.Subject = value; }
        }

        public string Body
        {
            get { return _mail.Body; }
            set { _mail.Body = value; }
        }

        public emailFormat BodyFormat
        {
            get { return (emailFormat) _mail.BodyFormat; }
            set { _mail.BodyFormat = ((MailFormat) value); }
        }

        public string Server
        {
            get { return _mail.Server; }
            //set { this._mail.Server = value; }
        }

        public bool CancelMailMerge
        {
            get { return _mail.CancelMailMerge; }
            set { _mail.CancelMailMerge = value; }
        }

        public int BatchSize
        {
            set { _batchSize = value; }
        }

        public string Charset
        {
            set { _charset = value; }
        }

        public int Pause
        {
            set { _pause = value; }
        }

        #endregion

        #region Public Events

        public virtual event MMProgressType MMProgress;

        #endregion

        #region Constructor

        public aspNetEmailSender()
        {
            //New ASPnetMail Object
            _mail = new EmailMessage(ConfigurationManager.AppSettings.Get("smtpHost"));

            //Don't block the sending event
            _mail.IgnoreRecipientErrors = true;

            //Init of the PickUp Directory
            _mail.MSPickupDirectory = @"C:\Inetpub\mailroot\Pickup\";

            //Don't use log option
            _mail.LogInMemory = false;

            //Event of sending progress
            _mail.MMProgress += EmailMessage_OnSend;
        }

        public aspNetEmailSender(String VBPmailServer)
        {
            //New ASPnetMail Object
            _mail = new EmailMessage(VBPmailServer);

            //Don't block the sending event
            _mail.IgnoreRecipientErrors = true;

            //Init of the PickUp Directory
            _mail.MSPickupDirectory = @"C:\Inetpub\mailroot\Pickup\";

            //Don't use log option
            _mail.LogInMemory = false;

            //Event of sending progress
            _mail.MMProgress += EmailMessage_OnSend;
        }

        public aspNetEmailSender(String VBPmailServer, String PickupDirectory)
        {
            //New ASPnetMail Object
            _mail = new EmailMessage(VBPmailServer);

            //Don't block the sending event
            _mail.IgnoreRecipientErrors = true;

            //Init of the PickUp Directory
            _mail.MSPickupDirectory = PickupDirectory;

            //Don't use log option
            _mail.LogInMemory = false;

            //Event of sending progress
            _mail.MMProgress += EmailMessage_OnSend;
        }

        #endregion

        #region Private event

        private void EmailMessage_OnSend(DataRow dr, Boolean success)
        {
            if (MMProgress != null)
            {
                MMProgress(dr, success);
            }
        }

        #endregion

        #region Public Function

        /// <summary>
        /// Simple sending mail
        /// </summary>
        /// <returns>return the sending result</returns>
        public bool SendMail()
        {
            if (_charset == null)
            {
                _charset = "ISO-8859-1";
            }

            _mail.CharSet = _charset;
            _mail.CharSetHeader = _charset;
            _mail.HeaderEncoding = MailEncoding.Base64;
            _mail.ContentTransferEncoding = MailEncoding.QuotedPrintable;

            return _mail.Send();
        }

        /// <summary>
        /// Simple sending mail throw the pickup directory
        /// </summary>
        /// <returns>return the sending result</returns>
        public bool SendMailToMSPickup()
        {
            if (_charset == null)
            {
                _charset = "ISO-8859-1";
            }

            _mail.CharSet = _charset;
            _mail.CharSetHeader = _charset;
            _mail.HeaderEncoding = MailEncoding.Base64;
            _mail.ContentTransferEncoding = MailEncoding.QuotedPrintable;

            return _mail.SendToMSPickup();
        }

        /// <summary>
        /// Send a mailing to a datatable
        /// </summary>
        /// <param name="MailMergeData">Datatable with user's data</param>
        /// <returns>return the sending result</returns>
        public bool SendMailMerge(DataTable MailMergeData)
        {
            if (_charset == null)
            {
                _charset = "ISO-8859-1";
            }

            _mail.CharSet = _charset;
            _mail.CharSetHeader = _charset;
            _mail.HeaderEncoding = MailEncoding.Base64;
            _mail.ContentTransferEncoding = MailEncoding.QuotedPrintable;

            if (MailMergeData.Rows.Count > 0)
            {
                return _mail.SendMailMerge(MailMergeData, -1, -1);
            }

            return false;
        }

        /// <summary>
        /// Send a mailing to a datatable throw the pickup directory
        /// </summary>
        /// <param name="MailMergeData">Datatable with user's data</param>
        /// <returns>return the sending result</returns>
        public bool SendMailMergeToMSPickup(DataTable MailMergeData)
        {
            if (_charset == null)
            {
                _charset = "ISO-8859-1";
            }

            _mail.CharSet = _charset;
            _mail.CharSetHeader = _charset;
            _mail.HeaderEncoding = MailEncoding.Base64;
            _mail.ContentTransferEncoding = MailEncoding.QuotedPrintable;

            if (MailMergeData.Rows.Count > 0)
            {
                return _mail.SendMailMergeToMSPickup(MailMergeData);
            }

            return false;
        }

        /// <summary>
        /// Add an email to send
        /// </summary>
        public Boolean AddTo(String EmailAddresses)
        {
            return _mail.AddTo(EmailAddresses);
        }

        /// <summary>
        /// Add an email to send with a name
        /// </summary>
        public Boolean AddTo(String EmailAddresses, String Name)
        {
            return _mail.AddTo(EmailAddresses, Name);
        }

        /// <summary>
        /// Add an attachment to the mail
        /// </summary>
        public Boolean AddAttachment(String FilePathAndName)
        {
            return _mail.AddAttachment(FilePathAndName);
        }

        /// <summary>
        /// Clear all attachment of the mail
        /// </summary>
        public void ClearAttachment()
        {
            _mail.ClearAttachments();
        }

        /// <summary>
        /// Append the body of the mail from a url
        /// </summary>
        public Boolean AppendBodyFromUrl(String VBPUrl)
        {
            try
            {
                return _mail.AppendBodyFromUrl(VBPUrl);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Append the body of the mail from a file
        /// </summary>
        public Boolean AppendBodyFromFile(String FilePath)
        {
            try
            {
                return _mail.AppendBodyFromFile(FilePath);
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}