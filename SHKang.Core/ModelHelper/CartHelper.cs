namespace SHKang.Core.ModelHelper
{
    #region namespaces
    using SHKang.Core.Helpers;
    using SHKang.Model.Models;
    using SHKang.Model.ViewModels.Web;
    using System.Collections.Generic;
    #endregion

    public static class CartHelper
    {

        /// <summary>
        /// Converts to user cart details.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static UserCartDetails BindUserCartDetails(AddToCartModel addToCartModel)
        {
            UserCartDetails userCartDetails = new UserCartDetails();
            if (addToCartModel != null)
            {
                userCartDetails.CreatedBy = DBHelper.ParseInt64(addToCartModel.CreatedBy);
                userCartDetails.UpdatedBy = DBHelper.ParseInt64(addToCartModel.UpdatedBy);
                userCartDetails.ProductFK = DBHelper.ParseInt64(addToCartModel.ProductId);
                userCartDetails.ProductColorFK = DBHelper.ParseInt64(addToCartModel.ProductColorId);
                userCartDetails.Quantity = DBHelper.ParseDecimal(addToCartModel.Quantity);
                userCartDetails.Price = DBHelper.ParseDecimal(addToCartModel.Price);
                userCartDetails.UserCartId = DBHelper.ParseInt64(addToCartModel.CartId);
                userCartDetails.UserFK = DBHelper.ParseInt64(addToCartModel.UserId);
                userCartDetails.SubTotalAmount = DBHelper.ParseDecimal(userCartDetails.Price * DBHelper.ParseDecimal(userCartDetails.Quantity));
            }
            return userCartDetails;
        }

        /// <summary>
        /// Converts to user cart detail model.
        /// </summary>
        /// <param name="userCartDetailsList">The model.</param>
        /// <returns></returns>
        public static List<UserCartDetailModel> BindUserCartDetailModel(List<UserCartDetails> userCartDetailsList)
        {
            List<UserCartDetailModel> userCartDetailModel = new List<UserCartDetailModel>();
            //if (userCartDetailsList != null)
            //{
            //    foreach (var item in userCartDetailsList)
            //    {
            //        userCartDetailModel.Add(new UserCartDetailModel
            //        {
            //            CartId =DBHelper.ParseString(item.UserCartId),
            //            Color = DBHelper.ParseString(item.ProductColor.Color.Name),
            //            ColorId = DBHelper.ParseString(item.ProductColor.Color.ColorId),
            //            Description = item.Product.ProductDescription,
            //            Name = item.Product.Name,
            //            Price = DBHelper.ParseString(item.Price),
            //            ProductId = DBHelper.ParseString(item.ProductFK),
            //            Quantity = DBHelper.ParseString(item.Quantity),
            //            Size = item.Product.Size.Name,
            //            SizeId = DBHelper.ParseString(item.Product.SizeFK),
            //            SubTotalAmount = DBHelper.ParseString(item.SubTotalAmount),
            //            UserId = DBHelper.ParseString(item.UserFK)
            //        });
            //    }
            //}
            return userCartDetailModel;
        }

        /// <summary>
        /// Binds the check promocode return model.
        /// </summary>
        /// <param name="promoCode">The promo code.</param>
        /// <param name="cartDetailModel">The cart detail model.</param>
        /// <returns></returns>
        public static CheckPromocodeReturnModel BindCheckPromocodeReturnModel(PromoCode promoCode, UserCartDetailModel cartDetailModel)
        {
            CheckPromocodeReturnModel promocodeReturnModel = new CheckPromocodeReturnModel();
            if (promoCode != null)
            {
                CheckPromocodeDetailModel promocodeDetailModel = new CheckPromocodeDetailModel();
                promocodeDetailModel.PromocodeId = DBHelper.ParseString(promoCode.PromoCodeId);
                promocodeDetailModel.Name = DBHelper.ParseString(promoCode.Name);
                promocodeDetailModel.Promocode = DBHelper.ParseString(promoCode.Code);
                promocodeReturnModel.PromocodeModel = promocodeDetailModel;
            }
            if (cartDetailModel != null)
            {
                CheckPromocodeTotalModel totalModel = new CheckPromocodeTotalModel();
                double total = 0;
                double discount = 0;
                foreach (var item in cartDetailModel.productDetail)
                {
                    total = total + DBHelper.ParseDouble(item.SubTotal);
                }
                if (promoCode != null)
                {
                    if (promoCode.Amount > 0)
                    {
                        discount = DBHelper.ParseDouble(promoCode.Amount);
                    }
                    else
                    {
                        discount = total * DBHelper.ParseDouble(promoCode.Percentage) / 100;
                    }
                    totalModel.TotalAmount = DBHelper.ParseString(total);
                    total = total - discount;
                    totalModel.DiscountAmount = DBHelper.ParseString(discount);
                    totalModel.PayableAmount = DBHelper.ParseString(total);
                }
                promocodeReturnModel.AmountModel = totalModel;
            }
            return promocodeReturnModel;
        }
    }
}
