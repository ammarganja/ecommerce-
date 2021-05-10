namespace SHKang.Core.ModelHelper
{
    #region namespaces
    using SHKang.Core.Constant;
    using SHKang.Core.Helpers;
    using System.Threading.Tasks;
    #endregion

    public static class ForgotPasswordHelper
    {

        /// <summary>
        /// Sends the reset password mail.
        /// </summary>
        /// <param name="toMail">To mail.</param>
        /// <param name="code">The code.</param>
        /// <param name="hosting">The hosting.</param>
        /// <param name="clientEmail">The client email.</param>
        /// <param name="clientPassword">The client password.</param>
        /// <param name="port">The port.</param>
        /// <param name="sMTPUrl">The s MTP URL.</param>
        /// <param name="host">The host.</param>
        /// <param name="scheme">The scheme.</param>
        public static async Task SendResetPasswordMail(string toMail, string code, string hosting, string clientEmail, string clientPassword, string port, string sMTPUrl, string host, string scheme)
        {
            await Task.Run(() =>
            {
                var webRoot = hosting;
                string path = string.Concat(webRoot, @"\" + Constants.ForgotPasswordPath);

                if (System.IO.File.Exists(path))
                {
                    string html = System.IO.File.ReadAllText(path);
                    string _clientEmail = clientEmail;
                    string clientEmailPassword = clientPassword;
                    string _port = port;
                    string mailHost = sMTPUrl;
                    string _host = host;
                    string _scheme = scheme;
                    ///.To DO Reset Password Link
                    string returnURL = "http://203.109.113.154:8065/resetpassword?code=" + code; ///_scheme + "://" + _host + "/ResetPassword/" + code + "/" + toMail;
                    html = html.Replace("##RETURNURL##", returnURL);
                    MailHelper.SendMail(toMail, Constants.ForgotPasswordSubject, html, _clientEmail, mailHost, _port, clientEmailPassword);
                }
                else
                {
                    LogHelper.ExceptionLog("Forgot Password Template Path Not Found " + path);
                }
            });
        }
    }
}
