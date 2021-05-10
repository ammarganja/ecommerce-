namespace SHKang.Core.Helpers
{
    #region using namespace
    using SHKang.Core.Enums;
    using SHKang.Model.ViewModels.Admin;
    using System;
    using System.Net.Mail;
    #endregion

    /// <summary>
    /// The mail helper class.
    /// </summary>
    public class MailHelper
    {
        #region static function

        /// <summary>
        /// Sends the mail.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <returns>
        /// returns ResponseStatus
        /// </returns>

        public static ResponseModel SendMail(string address, string subject, string body, string emailFrom = "", string host = "", string port = "", string pwd = "")
        {
            string msg = string.Empty;
            try
            {
                MailAddress fromAddress = new MailAddress(emailFrom);
                MailMessage message = new MailMessage();
                message.From = fromAddress;
                message.To.Add(address);

                //foreach (var address in toList)
                //{
                //    message.To.Add(address);
                //}

                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = body;

                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Host = host;
                smtpClient.Port = DBHelper.ParseInt32(port);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = true;
                smtpClient.Credentials = new System.Net.NetworkCredential(emailFrom, pwd);

                smtpClient.Send(message);

                return ResponseHelper.Success(ResponseStatus.Success);
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "   ::::   " + ex.StackTrace);
                return ResponseHelper.Error(ex.Message);
            }
        }

        #endregion
    }
}
