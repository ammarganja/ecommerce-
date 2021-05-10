namespace SHKang.API.Controllers
{
    #region namespaces

    using Microsoft.AspNetCore.Mvc;
    using SHKang.Core;
    using SHKang.Core.Constant;
    using SHKang.Core.Enums;
    using SHKang.Core.Helpers;
    using SHKang.Core.ModelHelper;
    using SHKang.Model.Models;
    using SHKang.Model.ViewModels.Admin;
    using SHKang.Repository.Interface;
    using System; 
    #endregion

    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
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
        public RegisterController(IUser _iUser)
        {
            iUser = _iUser;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Registrations the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("registration")]
        public IActionResult Registration(AddNewUserModel addNewUserModel)
        {
            if (ModelState.IsValid)
            {
                User userModel = UserHelper.BindUserModel(addNewUserModel);
                userModel.CreatedOn = DateTime.Now;
                userModel.Password = EncryptDecryptHelper.Encrypt(userModel.Password);
                long userId = iUser.AddUser(userModel);
                if (userId > 0)
                {
                    //UserAddress addmodel = UserHelper.BindUserAddress(addNewUserModel);
                    //addmodel.UserFK = userId;
                    //long userAddressId = iUser.AddNewUserAddress(addmodel);
                    return Ok(ResponseHelper.Success(MessageConstants.UserRegistered));
                }
                else if (userId == ReturnCode.AlreadyExist.GetHashCode())
                {
                    return Ok(ResponseHelper.Error(MessageConstants.TryDifferentEmailId));
                }
                else
                {
                    return Ok(ResponseHelper.Error(MessageConstants.UserNotRegistered));
                }

            }
            else
            {
                return Ok(ResponseHelper.Error(MessageConstants.CompulsoryData));
            }
        }

        #endregion
    }
}