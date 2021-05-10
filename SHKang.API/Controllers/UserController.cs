namespace SHKang.API.Controllers
{
    #region namespaces

    using Microsoft.AspNetCore.Mvc;
    using SHKang.API.Helper;
    using SHKang.Core;
    using SHKang.Core.Constant;
    using SHKang.Core.Enums;
    using SHKang.Core.Helpers;
    using SHKang.Model.ViewModels.Admin;
    using SHKang.Model.ViewModels.Web;
    using SHKang.Repository.Interface;
    using System;
    #endregion

    [Route("api/[controller]")]
    [ApiController]
    [AuthorizeHelper]
    public class UserController : ControllerBase
    {
        #region Private Variables

        /// <summary>
        /// The i user
        /// </summary>
        private readonly IUser iUser;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="_iUser">The i user.</param>
        public UserController(IUser _iUser)
        {
            iUser = _iUser;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the user password.
        /// </summary>
        /// <param name="oldPassword">The old password.</param>
        /// <param name="newPassword">The new password.</param>
        /// <param name="UserId">The user identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("update-user-password")]
        public IActionResult UpdateUserPassword(UpdateUserPasswordModel updateUserModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    updateUserModel.oldPassword = EncryptDecryptHelper.Encrypt(updateUserModel.oldPassword);
                    updateUserModel.newPassword = EncryptDecryptHelper.Encrypt(updateUserModel.newPassword);
                    var isUpdated = iUser.UpdatePassword(updateUserModel.oldPassword, updateUserModel.newPassword, updateUserModel.userId);
                    if (isUpdated > 0)
                    {
                        return Ok(ResponseHelper.Success(MessageConstants.PasswordUpdated));
                    }
                    else if (isUpdated == ReturnCode.NotMatching.GetHashCode())
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.OldPasswordNotMatching));
                    }
                    else
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.PasswordNotUpdated));
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
        /// Gets the user password.
        /// </summary>
        /// <param name="UserId">The user identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("get-user-password")]
        public IActionResult GetUserPassword(DeleteUserModel deleteModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var password = iUser.GetUserPassword(DBHelper.ParseInt64(deleteModel.userId));
                    if (!string.IsNullOrWhiteSpace(password))
                    {
                        password = EncryptDecryptHelper.Decrypt(password);
                        return Ok(ResponseHelper.Success(DBHelper.ParseString(password)));
                    }
                    else
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.DataNotFound));
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