namespace SHKang.API.Controllers
{
    #region namespaces

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using SHKang.Core;
    using SHKang.Core.Constant;
    using SHKang.Core.Enums;
    using SHKang.Core.Helpers;
    using SHKang.Model.ViewModels.Admin;
    using SHKang.Repository.Interface;
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    #endregion

    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        #region Private Variables

        /// <summary>
        /// The i login
        /// </summary>
        private readonly ILogin iLogin;

        /// <summary>
        /// The configuration
        /// </summary>
        private readonly IConfiguration config;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginController"/> class.
        /// </summary>
        /// <param name="_iLogin">The i login.</param>
        public LoginController(ILogin _iLogin, IConfiguration _config)
        {
            iLogin = _iLogin;
            config = _config;
        }

        #endregion

        #region Public Methods  

        /// <summary>
        /// Login Method 
        /// Parameter : EmailId, Password
        /// Method : POST
        /// </summary>
        /// <param name="json">The json.</param>
        /// <returns></returns>

        [Route("login")]
        [HttpPost]
        public IActionResult Login(LoginModel loginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int roleType = 2;
                    if (!string.IsNullOrWhiteSpace(loginModel.IsAdmin) && loginModel.IsAdmin.Equals(DBHelper.ParseString(RoleType.Admin.GetHashCode())))
                        roleType = 1;
                    loginModel.Password = EncryptDecryptHelper.Encrypt(loginModel.Password);
                    UserModel userModel = iLogin.Login(loginModel.EmailAddress, loginModel.Password, roleType);
                    if (userModel != null)
                    {
                        string token = GenerateJwtToken(userModel, roleType);
                        userModel.token = token;
                        return Ok(ResponseHelper.Success(userModel));
                    }
                    else
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.CheckEmailIdPassword));
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

        #region Private Methods

        /// <summary>
        /// Generates the JWT token.
        /// </summary>
        /// <param name="userModel">The model.</param>
        /// <returns></returns>
        private string GenerateJwtToken(UserModel userModel, int roleType)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["app_settings:Secret"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Email, userModel.EmailAddress),
                new Claim("RoleType",DBHelper.ParseString(roleType)),
                new Claim("UserId",DBHelper.ParseString(userModel.UserId)),
                new Claim(JwtRegisteredClaimNames.Jti, Convert.ToString(Guid.NewGuid()))
            };

            var token = new JwtSecurityToken(config["app_settings:Issuer"],
               config["app_settings:Issuer"], claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion
    }
}