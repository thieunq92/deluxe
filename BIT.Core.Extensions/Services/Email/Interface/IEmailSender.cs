using System;
using System.Data;
using BIT.Core.Extensions.Util;

namespace BIT.Core.Extensions.Services.Email.Interface
{
    public interface IEmailSender
    {
        #region Properties

        string ToMail { set; get; }

        string Cc { set; get; }

        string Bcc { set; get; }

        string ReplyTo { set; get; }

        string FromName { set; get; }

        string FromMail { set; get; }

        string Subject { set; get; }

        string Body { set; get; }

        emailFormat BodyFormat { set; get; }

        string Server { //set;
            get; }

        bool CancelMailMerge { set; get; }

        int BatchSize { set; }

        string Charset { set; }

        int Pause { set; }

        #endregion

        #region Event

        event MMProgressType MMProgress;

        #endregion

        #region functions

        /// <summary>
        /// Simple sending mail
        /// </summary>
        /// <returns>return the sending result</returns>
        bool SendMail();

        /// <summary>
        /// Simple sending mail throw the pickup directory
        /// </summary>
        /// <returns>return the sending result</returns>
        bool SendMailToMSPickup();

        /// <summary>
        /// Send a mailing to a datatable
        /// </summary>
        /// <param name="MailMergeData">Datatable with user's data</param>
        /// <returns>return the sending result</returns>
        bool SendMailMerge(DataTable MailMergeData);

        /// <summary>
        /// Send a mailing to a datatable throw the pickup directory
        /// </summary>
        /// <param name="MailMergeData">Datatable with user's data</param>
        /// <returns>return the sending result</returns>
        bool SendMailMergeToMSPickup(DataTable MailMergeData);

        /// <summary>
        /// Add an email to send
        /// </summary>
        Boolean AddTo(String EmailAddresses);

        /// <summary>
        /// Add an email to send with a name
        /// </summary>
        Boolean AddTo(String EmailAddresses, String Name);

        /// <summary>
        /// Add an attachment to the mail
        /// </summary>
        Boolean AddAttachment(String FilePathAndName);

        /// <summary>
        /// Clear all attachment of the mail
        /// </summary>
        void ClearAttachment();

        /// <summary>
        /// Append the body of the mail from a url
        /// </summary>
        Boolean AppendBodyFromUrl(String VBPUrl);

        /// <summary>
        /// Append the body of the mail from a file
        /// </summary>
        Boolean AppendBodyFromFile(String FilePath);

        #endregion
    }
}