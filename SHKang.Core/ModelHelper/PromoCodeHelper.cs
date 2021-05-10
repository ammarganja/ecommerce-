namespace SHKang.Core.ModelHelper
{
    using SHKang.Core.Constant;
    #region namespaces
    using SHKang.Core.Helpers;
    using SHKang.Model.Models;
    using SHKang.Model.ViewModels.Admin;
    using System;
    using System.Collections.Generic;
    #endregion

    public static class PromoCodeHelper
    {

        /// <summary>
        /// Convers to promo code.
        /// </summary>
        /// <param name="addPromoCodeModel">The model.</param>
        /// <returns></returns>
        public static PromoCode BindPromoCode(AddPromoCodeModel addPromoCodeModel)
        {
            PromoCode promoCodeModel = new PromoCode();
            if (addPromoCodeModel != null)
            {
                if (!string.IsNullOrEmpty(addPromoCodeModel.PromoCodeId))
                    promoCodeModel.PromoCodeId =DBHelper.ParseInt64(addPromoCodeModel.PromoCodeId);
                promoCodeModel.Name = addPromoCodeModel.Name;
                promoCodeModel.Code = addPromoCodeModel.Code;
                if (!string.IsNullOrEmpty(addPromoCodeModel.StartDate))
                    promoCodeModel.StartDate = Convert.ToDateTime(addPromoCodeModel.StartDate);
                if (!string.IsNullOrEmpty(addPromoCodeModel.ExpiryDate))
                    promoCodeModel.ExpiryDate = Convert.ToDateTime(addPromoCodeModel.ExpiryDate);
                promoCodeModel.Description = addPromoCodeModel.Description;
                promoCodeModel.Amount = DBHelper.ParseDecimal(addPromoCodeModel.Amount);
                promoCodeModel.Percentage = DBHelper.ParseDecimal(addPromoCodeModel.Percentage);
                promoCodeModel.CreatedBy = DBHelper.ParseInt64(addPromoCodeModel.CreatedBy);
                promoCodeModel.Status = DBHelper.ParseInt32(addPromoCodeModel.Status);
                if (!string.IsNullOrEmpty(addPromoCodeModel.UpdatedBy))
                    promoCodeModel.UpdatedBy = DBHelper.ParseInt64(addPromoCodeModel.UpdatedBy);
            }
            return promoCodeModel;
        }

        /// <summary>
        /// Checks the promo code.
        /// </summary>
        /// <param name="promoCodeslist">The result.</param>
        /// <returns></returns>
        public static string CheckPromoCode(List<string> promoCodeslist)
        {
            string uniqPromoCode = RandomHelpers.GetUniquePromoCode();
            if (promoCodeslist != null)
            {
            againCheck:
                var isExist = promoCodeslist.Contains(uniqPromoCode);
                if (isExist)
                {
                    uniqPromoCode = RandomHelpers.GetUniquePromoCode();
                    goto againCheck;
                }
                else
                {
                    return uniqPromoCode;
                }
            }
            else
            {
                return uniqPromoCode;
            }
        }

        /// <summary>
        /// Converts to promo code list model.
        /// </summary>
        /// <param name="promoCodeModel">The model.</param>
        /// <returns></returns>
        public static List<PromoCodeDataListModel> BindPromoCodeListModel(List<PromoCode> promoCodeModel)
        {
            List<PromoCodeDataListModel> promoCodeListModel = new List<PromoCodeDataListModel>();
            if (promoCodeModel != null)
            {
                foreach (var item in promoCodeModel)
                {
                    PromoCodeDataListModel promoCodeDataListModel = new PromoCodeDataListModel();
                    if (item.Amount != null)
                        promoCodeDataListModel.Amount =DBHelper.ParseString(item.Amount);
                    promoCodeDataListModel.Code = item.Code;
                    if (item.StartDate.HasValue)
                        promoCodeDataListModel.StartDate = DBHelper.ParseString(Convert.ToDateTime(item.StartDate).ToString(Constants.PromocodeDateFormat));
                    if (item.StartDate.HasValue)
                        promoCodeDataListModel.ExpiryDate = DBHelper.ParseString(Convert.ToDateTime(item.ExpiryDate).ToString(Constants.PromocodeDateFormat)); 
                    promoCodeDataListModel.Name = item.Name;
                    promoCodeDataListModel.Status = item.Status;
                    promoCodeDataListModel.Description = item.Description;
                    promoCodeDataListModel.PromoCodeId = DBHelper.ParseString(item.PromoCodeId);
                    if (item.Percentage != null)
                        promoCodeDataListModel.Percentage = DBHelper.ParseString(item.Percentage);
                    promoCodeListModel.Add(promoCodeDataListModel);
                }
            }
            return promoCodeListModel;
        }

        /// <summary>
        /// Converts to promo code list model.
        /// </summary>
        /// <param name="promoCodeModel">The model.</param>
        /// <returns></returns>
        public static PromoCodeDataListModel BindPromoCodeListModel(PromoCode promoCodeModel)
        {
            PromoCodeDataListModel promoCodeListModel = new PromoCodeDataListModel();
            if (promoCodeModel != null)
            {

                if (promoCodeModel.Amount != null)
                    promoCodeListModel.Amount = DBHelper.ParseString(promoCodeModel.Amount);
                promoCodeListModel.Code = promoCodeModel.Code;
                promoCodeListModel.StartDate = DBHelper.ParseString(Convert.ToDateTime(promoCodeModel.StartDate));
                promoCodeListModel.ExpiryDate = DBHelper.ParseString(Convert.ToDateTime(promoCodeModel.ExpiryDate)); ;
                promoCodeListModel.Name = promoCodeModel.Name;
                promoCodeListModel.Description = promoCodeModel.Description;
                promoCodeListModel.PromoCodeId = DBHelper.ParseString(promoCodeModel.PromoCodeId);
                if (promoCodeModel.Percentage != null)
                    promoCodeListModel.Percentage = DBHelper.ParseString(promoCodeModel.Percentage);
            }
            return promoCodeListModel;
        }

    }
}
