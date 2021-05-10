namespace SHKang.API.Controllers
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
    using SHKang.Model.ViewModels.Web;
    using SHKang.Repository.Interface;
    using System;
    using System.Collections.Generic;
    #endregion

    [Route("api/[controller]")]
    [ApiController]
    [AuthorizeHelper]
    public class UserCartController : ControllerBase
    {
        #region Private Variables

        /// <summary>
        /// The i cart
        /// </summary>
        private readonly ICart iCart;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="UserCartController"/> class.
        /// </summary>
        /// <param name="_iCart">The i cart.</param>
        public UserCartController(ICart _iCart)
        {
            iCart = _iCart;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Adds to user cart.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("add-to-cart")]
        public IActionResult AddToUserCart(AddToCartModel addToCartModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserCartDetails cartModel = CartHelper.BindUserCartDetails(addToCartModel);
                    cartModel.CreatedOn = DateTime.Now;
                    long cartId = iCart.AddToCart(cartModel);
                    if (cartId > 0)
                    {
                        //return Ok(ResponseHelper.Success(MessageConstants.AddedToCart));
                        return Ok(ResponseHelper.Success(cartId));
                    }
                    else if (cartId == ReturnCode.AlreadyExist.GetHashCode())
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.ExistInCart));
                    }
                    else
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.NotAddedToCart));
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
        /// Deletes the user cart.
        /// </summary>
        /// <param name="CartId">The cart identifier.</param>
        /// <param name="UserId">The user identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete-item-cart")]
        public IActionResult DeleteUserCart(DeleteUserCartModel deleteUserCartModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isDeleted = iCart.DeleteItemFromCart(deleteUserCartModel.cartId, deleteUserCartModel.userId);
                    if (isDeleted)
                    {
                        return Ok(ResponseHelper.Success(MessageConstants.DeletedFromCart));
                    }
                    else
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.NotDeletedFromCart));
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
        /// Updates the user cart.
        /// </summary>
        /// <param name="CartId">The cart identifier.</param>
        /// <param name="ProductId">The product identifier.</param>
        /// <param name="ProductColorId">The product color identifier.</param>
        /// <param name="Quantity">The quantity.</param>
        /// <param name="UserId">The user identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("update-cart")]
        public IActionResult UpdateUserCart(UpdateUserCartModel updateUserCartModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    long isUpdated = iCart.UpdateQuantity(updateUserCartModel.cartId, updateUserCartModel.productId, updateUserCartModel.productColorId, updateUserCartModel.quantity, updateUserCartModel.userId, updateUserCartModel.updatedBy); ;

                    if (isUpdated > 0)
                    {
                        return Ok(ResponseHelper.Success(MessageConstants.QuantityUpdated));
                    }
                    else
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.NotQuantityUpdated));
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
        /// Gets the user cart details.
        /// </summary>
        /// <param name="UserId">The user identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("get-cart-details")]
        public IActionResult GetUserCartDetails(DeleteUserModel userModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string scheme = this.Request.Scheme;
                    scheme += "://" + this.Request.Host + this.Request.PathBase;
                    var userCartDetail = iCart.GetUserCartDetail(DBHelper.ParseInt64(userModel.userId), scheme);
                    if (userCartDetail != null)
                    {
                        return Ok(ResponseHelper.Success(userCartDetail));
                    }
                    else
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.UserCartEmpty));
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
        /// Empties the user cart.
        /// </summary>
        /// <param name="UserId">The user identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("empty-cart")]
        public IActionResult EmptyUserCart(DeleteUserModel userModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var isEmpty = iCart.EmptyCart(DBHelper.ParseInt64(userModel.userId));
                    if (isEmpty)
                    {
                        return Ok(ResponseHelper.Success(MessageConstants.RemovedItemFromCart));
                    }
                    else
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.CartIsEmpty));
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
        /// Gets the user cart count.
        /// </summary>
        /// <param name="userModel">The user model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("get-cart-count")]
        public IActionResult GetUserCartCount(DeleteUserModel userModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userCartCount = iCart.UserCartItemCount(DBHelper.ParseInt64(userModel.userId));
                    return Ok(ResponseHelper.Success(userCartCount));
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