namespace SHKang.API.Areas.Admin.Controllers
{
    #region namespaces

    using Microsoft.AspNetCore.Mvc;
    using SHKang.API.Helper;
    using SHKang.Core;
    using SHKang.Core.Constant;
    using SHKang.Core.Enums;
    using SHKang.Core.Helpers;
    using SHKang.Core.ModelHelper;
    using SHKang.Model.Models;
    using SHKang.Model.ViewModels.Admin;
    using SHKang.Repository.Interface;
    using System;
    using System.Collections.Generic;

    #endregion

    [Route("api/admin/[controller]")]
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
        /// Adds the new user.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("add-user")]
        public IActionResult AddNewUser(AddNewUserModel addNewUserModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User userModel = UserHelper.BindUserModel(addNewUserModel);
                    userModel.CreatedOn = DateTime.Now;
                    if (userModel.Password != null && addNewUserModel.UserId == 0)
                    {
                        userModel.Password = EncryptDecryptHelper.Encrypt(userModel.Password);
                    }
                    
                    long userId = iUser.AddUser(userModel);
                    if (userId > 0)
                    {
                        ////UserAddress addmodel = UserHelper.BindUserAddress(addNewUserModel);
                        ////addmodel.UserFK = userId;
                        ////long userAddressId = iUser.AddNewUserAddress(addmodel);
                        return Ok(ResponseHelper.Success(MessageConstants.UserAdded));
                    }
                    else if (userId == ReturnCode.AlreadyExist.GetHashCode())
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.TryDifferentEmailId));
                    }
                    else
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.UserNotAdded));
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
        /// Deletes the user.
        /// </summary>
        /// <param name="UserId">The user identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete-user")]
        public IActionResult DeleteUser(DeleteUserModel deleteModel)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(deleteModel.userId))
                {
                    bool isDeleted = iUser.DeleteUser(DBHelper.ParseInt64(deleteModel.userId));
                    if (isDeleted)
                    {
                        return Ok(ResponseHelper.Success(MessageConstants.UserDeleted));
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

        /// <summary>
        /// Users the list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("user-list")]
        public IActionResult UserList(SearchPaginationListModel searchModel)
        {
            try
            {
                if (searchModel.pageNo <= 0)
                {
                    searchModel.pageNo = 1;
                }
                PagedListModel<UserListModel> userList = iUser.GetUserList(searchModel);
                if (userList != null)
                {
                    return Ok(ResponseHelper.Success(userList));
                }
                else
                {
                    return Ok(ResponseHelper.Error(MessageConstants.DataNotFound));
                }
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                return Ok(ResponseHelper.Error(ex.Message));
            }
        }

        /// <summary>
        /// Changes the user status.
        /// </summary>
        /// <param name="changeUserStatus">The change user status.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("change-user-status")]
        public IActionResult ChangeUserStatus(ChangeUserStatus changeUserStatus)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userDetail = iUser.ChangeUserStatus(changeUserStatus.UserId, changeUserStatus.Status);
                    if (userDetail)
                    {
                        return Ok(ResponseHelper.Success(MessageConstants.UserStatusUpdated));
                    }
                    else
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.UserStatusNotUpdated));
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
        /// Users the address list.
        /// </summary>
        /// <param name="deleteModel">The delete model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("user-address-list")]
        public IActionResult UserAddressList(DeleteUserModel deleteModel)
        {
            try
            {
                var addressList = SelectHelper.BindSelectListItem(iUser.GetAllUserAddressListForDropDown(DBHelper.ParseInt64( deleteModel.userId)));
                var userDetail = iUser.GetUserDetail(DBHelper.ParseInt64(deleteModel.userId));
                var userOrderModel = UserHelper.BindUserOrderModel(userDetail);
                userOrderModel.addressList = addressList;
                if (userDetail != null)
                {
                    return Ok(ResponseHelper.Success(userOrderModel));
                }
                else
                {
                    return Ok(ResponseHelper.Error(MessageConstants.DataNotFound));
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