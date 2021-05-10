namespace SHKang.Core.ModelHelper
{
    #region namespaces

    using SHKang.Core.Helpers;
    using SHKang.Model.Models;
    using SHKang.Model.ViewModels.Admin;
    using System.Collections.Generic;
    using System.Linq;
    #endregion

    public static class SelectHelper
    {
        /// <summary>
        /// Converts to select list item.
        /// </summary>
        /// <param name="colorModel">The colormodel.</param>
        /// <returns></returns>
        public static List<SelectListItemModel> BindSelectListItem(List<Color> colorModel)
        {
            List<SelectListItemModel> selectListItemModel = new List<SelectListItemModel>();
            if (colorModel != null)
            {
                foreach (var item in colorModel)
                {
                    selectListItemModel.Add(new SelectListItemModel
                    {
                        name = item.Name,
                        itemName = item.Name,
                        id = DBHelper.ParseString(item.ColorId)
                    });
                }
            }
            return selectListItemModel;
        }

        /// <summary>
        /// Converts to select list item.
        /// </summary>
        /// <param name="sizeModel">The sizemodel.</param>
        /// <returns></returns>
        public static List<SelectListItemModel> BindSelectListItem(List<Size> sizeModel)
        {
            List<SelectListItemModel> selectListItemModel = new List<SelectListItemModel>();
            if (sizeModel != null)
            {
                foreach (var item in sizeModel)
                {
                    selectListItemModel.Add(new SelectListItemModel
                    {
                        name = item.Name,
                        itemName = item.Name,
                        id = DBHelper.ParseString(item.SizeId)
                    });
                }
            }
            return selectListItemModel;
        }

        /// <summary>
        /// Converts to select list item.
        /// </summary>
        /// <param name="sizeRatioModel">The sizeratiomodel.</param>
        /// <returns></returns>
        public static List<SelectListItemModel> BindSelectListItem(List<SizeRatio> sizeRatioModel)
        {
            List<SelectListItemModel> selectListItemModel = new List<SelectListItemModel>();
            if (sizeRatioModel != null)
            {
                foreach (var item in sizeRatioModel)
                {
                    selectListItemModel.Add(new SelectListItemModel
                    {
                        name = item.Name,
                        itemName = item.Name,
                        id = DBHelper.ParseString(item.SizeRatioId)
                    });
                }
            }
            return selectListItemModel;
        }

        /// <summary>
        /// Binds the select list item.
        /// </summary>
        /// <param name="productCategoryTypeModel">The product category type model.</param>
        /// <returns></returns>
        public static List<SelectListItemModel> BindSelectListItem(List<ProductCategoryType> productCategoryTypeModel)
        {
            List<SelectListItemModel> selectListItemModel = new List<SelectListItemModel>();
            if (productCategoryTypeModel != null)
            {
                foreach (var item in productCategoryTypeModel)
                {
                    selectListItemModel.Add(new SelectListItemModel
                    {
                        name = item.CategoryType,
                        itemName = item.CategoryType,
                        id = DBHelper.ParseString(item.ProductCategoryTypeId)
                    });
                }
            }
            return selectListItemModel;
        }

        /// <summary>
        /// Binds the select list item.
        /// </summary>
        /// <param name="userModel">The user model.</param>
        /// <returns></returns>
        public static List<SelectListItemModel> BindSelectListItem(List<User> userModel)
        {
            List<SelectListItemModel> selectListItemModel = new List<SelectListItemModel>();
            if (userModel != null)
            {
                foreach (var item in userModel)
                {
                    selectListItemModel.Add(new SelectListItemModel
                    {
                        name = item.FirstName + " " + item.LastName,
                        itemName = item.FirstName + " " + item.LastName,
                        id = DBHelper.ParseString(item.UserId)
                    });
                }
            }
            return selectListItemModel;
        }

        /// <summary>
        /// Binds the select list item.
        /// </summary>
        /// <param name="userAddressModel">The user address model.</param>
        /// <returns></returns>
        public static List<SelectListItemModel> BindSelectListItem(List<UserAddress> userAddressModel)
        {
            List<SelectListItemModel> selectListItemModel = new List<SelectListItemModel>();
            if (userAddressModel != null)
            {
                foreach (var item in userAddressModel)
                {
                    selectListItemModel.Add(new SelectListItemModel
                    {
                        name = item.Address1 + " " + item.Address2,
                        itemName = item.Address1 + " " + item.Address2,
                        id = DBHelper.ParseString(item.UserAddressId)
                    });
                }
            }
            return selectListItemModel;
        }

        /// <summary>
        /// Binds the select list item.
        /// </summary>
        /// <param name="sizeDetailModel">The size detail model.</param>
        /// <returns></returns>
        public static string[] BindSizeArray(List<SizeDetail> sizeDetailModel)
        {
            string[] sizeDetail = new string[sizeDetailModel.Count];
            if (sizeDetailModel != null)
            {
                for (int x = 0; x < sizeDetailModel.Count; x++)
                {
                    sizeDetail[x] = sizeDetailModel[x].Name;
                }
            }
            return sizeDetail;
        }
    }
}
