namespace SHKang.Repository.Repository
{
    #region namespaces
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using SHKang.Core.Constant;
    using SHKang.Core.Enums;
    using SHKang.Core.Helpers;
    using SHKang.Model.Context;
    using SHKang.Model.Models;
    using SHKang.Model.ViewModels.Admin;
    using SHKang.Model.ViewModels.Web;
    using SHKang.Repository.Interface;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    #endregion
    public class CartRepository : ICart
    {

        #region Private Variables

        /// <summary>
        /// The context
        /// </summary>
        private AppDbContext context;

        /// <summary>
        /// The settings
        /// </summary>
        private IOptions<ConnectionString> settings;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CartRepository" /> class.
        /// </summary>
        /// <param name="_context">The context.</param>
        /// <param name="_settings">The settings.</param>
        public CartRepository(AppDbContext _context, IOptions<ConnectionString> _settings)
        {
            context = _context;
            settings = _settings;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Adds to cart.
        /// </summary>
        /// <param name="userCartDetailsModel">The model.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public long AddToCart(UserCartDetails userCartDetailsModel)
        {
            try
            {
                var userCartDetails = context.UserCartDetails.Where(x => x.UserFK == userCartDetailsModel.UserFK && x.ProductFK == userCartDetailsModel.ProductFK && x.ProductColorFK == userCartDetailsModel.ProductColorFK && x.Price == userCartDetailsModel.Price && x.IsDelete == false).FirstOrDefault();
                if (userCartDetails == null)
                {
                    context.UserCartDetails.Add(userCartDetailsModel);
                    context.SaveChanges();
                    return userCartDetailsModel.UserCartId;
                }
                else
                {
                    return ReturnCode.AlreadyExist.GetHashCode();
                }
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Deletes the item from cart.
        /// </summary>
        /// <param name="CartId">The cart identifier.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool DeleteItemFromCart(long cartId, long userId)
        {
            try
            {
                var userCartDetails = context.UserCartDetails.Where(x => x.UserFK == userId && x.UserCartId == cartId && x.IsDelete == false).FirstOrDefault();
                if (userCartDetails != null)
                {
                    userCartDetails.IsDelete = true;
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Updates the quantity.
        /// </summary>
        /// <param name="CartId">The cart identifier.</param>
        /// <param name="ProductId">The product identifier.</param>
        /// <param name="ProductColorId">The product color identifier.</param>
        /// <param name="Quantity">The quantity.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public long UpdateQuantity(long cartId, long productId, long productColorId, decimal quantity, long userId, long updatedBy)
        {
            try
            {
                var userCartDetails = context.UserCartDetails.Where(x => x.UserFK == userId && x.UserCartId == cartId && x.IsDelete == false && x.ProductFK == productId && x.ProductColorFK == productColorId).FirstOrDefault();
                if (userCartDetails != null)
                {
                    userCartDetails.Quantity = quantity;
                    userCartDetails.SubTotalAmount = userCartDetails.Quantity * DBHelper.ParseDecimal(userCartDetails.Price);
                    userCartDetails.ModifiedOn = DateTime.Now;
                    userCartDetails.UpdatedBy = updatedBy;
                    context.SaveChanges();
                    return userCartDetails.UserCartId;
                }
                else
                {
                    return ReturnCode.NotExist.GetHashCode();
                }
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Gets the user cart detail.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="hostingPath">The hosting path.</param>
        /// <returns></returns>
        public UserCartDetailModel GetUserCartDetail(long userId, string hostingPath)
        {
            UserCartDetailModel userCartDetailModel = new UserCartDetailModel();
            try
            {
                List<UserCartDetailProductModel> productDetailModel = new List<UserCartDetailProductModel>();
                SqlParameter[] parameter = new SqlParameter[1];
                parameter[0] = new SqlParameter("USERID", userId);
                DataSet dataSet = DBHelper.GetDataTable("GETUSERCARTDETAIL", parameter, DBHelper.ParseString(settings.Value.AppDbContext));
                if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0)
                {
                    DataTable dtProducts = dataSet.Tables[0];
                    DataTable dtImage = dataSet.Tables[1];
                    if (dtProducts != null && dtProducts.Rows.Count > 0)
                    {
                        double grandTotal = 0;
                        foreach (DataRow item in dtProducts.Rows)
                        {
                            #region Image
                            string image = string.Empty;
                            DataRow[] drImage = dtImage.Select("ProductFK='" + DBHelper.ParseString(item["ProductId"]) + "' and ProductColorFK='" + DBHelper.ParseString(item["ProductColorFK"]) + "'");
                            if (drImage != null && drImage.Length > 0)
                            {
                                image = hostingPath + Constants.ProductImagesPath.Replace(@"\", "/") + "/" + DBHelper.ParseString(drImage[0]["Image"]);
                            }
                            #endregion

                            productDetailModel.Add(new UserCartDetailProductModel
                            {
                                Price = DBHelper.ParseString(item["Price"]),
                                Name = DBHelper.ParseString(item["Name"]),
                                ProductId = DBHelper.ParseString(item["ProductId"]),
                                Color = DBHelper.ParseString(item["Color"]),
                                Image = image,
                                ColorId = DBHelper.ParseString(item["ProductColorFK"]),
                                Quantity = DBHelper.ParseString(item["Quantity"]),
                                SubTotal = DBHelper.ParseString(item["SubTotalAmount"]),
                                CartId= DBHelper.ParseString(item["UserCartId"]),
                            });
                            grandTotal = grandTotal + DBHelper.ParseDouble(item["SubTotalAmount"]);
                        }
                        userCartDetailModel.productDetail = productDetailModel;
                        userCartDetailModel.CartId = DBHelper.ParseString(dtProducts.Rows[0]["UserCartId"]);
                        userCartDetailModel.UserId = DBHelper.ParseString(dtProducts.Rows[0]["UserFK"]);
                        userCartDetailModel.GrandTotal = DBHelper.ParseString(grandTotal);
                        userCartDetailModel.TotalItems = DBHelper.ParseString(dtProducts.Rows.Count);   
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
            return userCartDetailModel;
        }

        /// <summary>
        /// Empties the cart.
        /// </summary>
        /// <param name="UserId">The user identifier.</param>
        /// <returns></returns>
        public bool EmptyCart(long userId)
        {
            try
            {
                var userCartDetails = context.UserCartDetails.Where(x => x.UserFK == userId && x.IsDelete == false).ToList();
                if (userCartDetails != null && userCartDetails.Count > 0)
                {
                    foreach (var item in userCartDetails)
                    {
                        context.UserCartDetails.Remove(item);
                        context.SaveChanges();
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Users the cart item count.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public long UserCartItemCount(long userId)
        {

            try
            {
                return context.UserCartDetails.Where(x => x.UserFK == userId && x.IsDelete == false).Count();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }
        #endregion
    }
}
