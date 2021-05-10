namespace SHKang.Repository.Interface
{
    #region namespace
    using SHKang.Model.Models;
    using SHKang.Model.ViewModels.Web;
    using System.Collections.Generic; 
    #endregion

    public interface ICart
    {

        /// <summary>
        /// Adds to cart.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        long AddToCart(UserCartDetails userCartDetails);

        /// <summary>
        /// Deletes the item from cart.
        /// </summary>
        /// <param name="CartId">The cart identifier.</param>
        /// <returns></returns>
        bool DeleteItemFromCart(long cartId, long userId);

        /// <summary>
        /// Updates the quantity.
        /// </summary>
        /// <param name="CartId">The cart identifier.</param>
        /// <param name="ProductId">The product identifier.</param>
        /// <param name="ProductColorId">The product color identifier.</param>
        /// <param name="Quantity">The quantity.</param>
        /// <returns></returns>
        long UpdateQuantity(long cartId, long productId, long productColorId, decimal quantity, long userId, long updatedBy);

        /// <summary>
        /// Gets the user cart detail.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="hostingPath">The hosting path.</param>
        /// <returns></returns>
        UserCartDetailModel GetUserCartDetail(long userId,string hostingPath);

        /// <summary>
        /// Empties the cart.
        /// </summary>
        /// <param name="UserId">The user identifier.</param>
        /// <returns></returns>
        bool EmptyCart(long userId);

        /// <summary>
        /// Users the cart item count.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        long UserCartItemCount(long userId);

    }
}
