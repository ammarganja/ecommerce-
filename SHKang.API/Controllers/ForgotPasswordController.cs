namespace SHKang.API.Controllers
{
    #region namespaces
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using SHKang.Core;
    using SHKang.Core.Constant;
    using SHKang.Core.Helpers;
    using SHKang.Core.ModelHelper;
    using SHKang.Repository.Interface;
    using System;

    #endregion

    [Route("api/[controller]")]
    [ApiController]
    public class ForgotPasswordController : ControllerBase
    {
        #region Private Variables

        /// <summary>
        /// The iforgot
        /// </summary>
        private readonly IForgotPassword iforgot;

        /// <summary>
        /// The hosting environment
        /// </summary>
        private readonly IHostingEnvironment env;

        /// <summary>
        /// The configuration
        /// </summary>
        private readonly IConfiguration config;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ForgotPasswordController"/> class.
        /// </summary>
        /// <param name="_iforgot">The iforgot.</param>
        public ForgotPasswordController(IForgotPassword _iforgot, IHostingEnvironment _env, IConfiguration _config)
        {
            iforgot = _iforgot;
            env = _env;
            config = _config;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Forgots the password.
        /// </summary>
        /// <param name="json">The json.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("forgot-password")]
        public IActionResult ForgotPassword([FromBody] JObject json)
        {
            try
            {
                JObject jObject = JsonConvert.DeserializeObject<JObject>(DBHelper.ParseString(json));
                if (jObject != null)
                {
                    string emailId = DBHelper.ParseString(jObject["emailId"]);
                    string IsAdmin = DBHelper.ParseString(jObject["IsAdmin"]);
                    if (!string.IsNullOrWhiteSpace(emailId))
                    {
                        if (iforgot.IsEmailExist(emailId, IsAdmin))
                        {
                            string resetCode = RandomHelpers.RandomString();
                            long isUpdated = iforgot.UpdateForgotPasswordString(resetCode, emailId);
                            if (isUpdated > 0)
                            {
                                var HostingPath = env.ContentRootPath;

                                string clientEmail = config.GetValue<string>("app_settings:ClientEmail");
                                string clientEmailPassword = config.GetValue<string>("app_settings:ClientEmailPassword");
                                string port = config.GetValue<string>("app_settings:Port");
                                string mailHost = config.GetValue<string>("app_settings:SMTPURL");
                                string host = this.Request.Host.Value;
                                string scheme = this.Request.Scheme;
                                ForgotPasswordHelper.SendResetPasswordMail(emailId, resetCode, HostingPath, clientEmail, clientEmailPassword, port, mailHost, host, scheme);
                                return Ok(ResponseHelper.Success(MessageConstants.ResetPassordLink));

                            }
                            else
                            {
                                return Ok(ResponseHelper.Error(MessageConstants.DataNotFound));
                            }
                        }
                        else
                        {
                            return Ok(ResponseHelper.Error(MessageConstants.CheckEmailId));
                        }
                    }
                    else
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.CompulsoryData));
                    }
                }
                else
                {
                    return Ok(ResponseHelper.Error(MessageConstants.CompulsoryData));
                }
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                return Ok(ResponseHelper.Error(ex.Message));
            }
        }

        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("reset-password")]
        public IActionResult ResetPassword([FromBody] JObject json)
        {
            try
            {
                JObject jObject = JsonConvert.DeserializeObject<JObject>(DBHelper.ParseString(json));
                if (jObject != null)
                {
                    string code = DBHelper.ParseString(jObject["code"]);
                    string password = DBHelper.ParseString(jObject["password"]);
                    if (!string.IsNullOrWhiteSpace(code) && !string.IsNullOrWhiteSpace(password))
                    {
                        if (iforgot.IsCodeCorrect(code))
                        {
                            password = EncryptDecryptHelper.Encrypt(password);
                            long isPasswordReset = iforgot.ResetPasswordWithCode(password, code);
                            long isResetCode = iforgot.ResetCode(code);
                            if (isPasswordReset > 0 && isResetCode > 0)
                            {
                                return Ok(ResponseHelper.Success(MessageConstants.PasswordUpdated));
                            }
                            else
                            {
                                return Ok(ResponseHelper.Error(MessageConstants.DataNotFound));
                            }
                        }
                        else
                        {
                            return Ok(ResponseHelper.Error(MessageConstants.IncorrectPasswordCode));
                        }
                    }
                    else
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.CompulsoryData));
                    }
                }
                else
                {
                    return Ok(ResponseHelper.Error(MessageConstants.CompulsoryData));
                }
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                return Ok(ResponseHelper.Error(ex.Message));
            }
        }

        #endregion
    }
}