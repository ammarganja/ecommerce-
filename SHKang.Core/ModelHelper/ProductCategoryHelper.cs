namespace SHKang.Core.ModelHelper
{
    #region namespaces

    using SHKang.Model.Models;
    using SHKang.Model.ViewModels.Admin;
    using System;
    using System.Collections.Generic;
    #endregion

    public static class ProductCategoryHelper
    {
        /// <summary>
        /// Converts to product category model.
        /// </summary>
        /// <param name="addNewProductCategoryModel">The model.</param>
        /// <returns></returns>
        public static ProductCategory BindProductCategoryModel(AddNewProductCategoryModel addNewProductCategoryModel)
        {
            ProductCategory productCategoryModel = new ProductCategory();
            if (addNewProductCategoryModel != null)
            {
                productCategoryModel.Name = addNewProductCategoryModel.Name;
                productCategoryModel.CreatedBy = addNewProductCategoryModel.CreatedBy;
                productCategoryModel.CreatedOn = DateTime.Now;
            }
            return productCategoryModel;
        }

        /// <summary>
        /// Converts to product category model.
        /// </summary>
        /// <param name="updateProductCategoryModel">The model.</param>
        /// <returns></returns>
        public static ProductCategory BindProductCategoryModel(UpdateProductCategoryModel updateProductCategoryModel)
        {
            ProductCategory productCategoryModel = new ProductCategory();
            if (updateProductCategoryModel != null)
            {
                productCategoryModel.ProductCategoryId = updateProductCategoryModel.ProductCategoryId;
                productCategoryModel.Name = updateProductCategoryModel.Name;
                productCategoryModel.UpdatedBy = updateProductCategoryModel.UpdatedBy;
                productCategoryModel.ModifiedOn = DateTime.Now;
            }
            return productCategoryModel;
        }

        /// <summary>
        /// Converts the product category list model.
        /// </summary>
        /// <param name="productCategoryModel">The model.</param>
        /// <returns></returns>
        public static List<ProductCategoryListModel> BindProductCategoryListModel(List<ProductCategory> productCategoryModel)
        {
            List<ProductCategoryListModel> productCategoryListModel = new List<ProductCategoryListModel>();
            if (productCategoryModel != null)
            {
                foreach (var item in productCategoryModel)
                {
                    productCategoryListModel.Add(new ProductCategoryListModel
                    {
                        Name = item.Name,
                        ProductCategoryId = item.ProductCategoryId
                    });
                }
            }
            return productCategoryListModel;
        }

        /// <summary>
        /// Converts the product category list model.
        /// </summary>
        /// <param name="productCategoryModel">The model.</param>
        /// <returns></returns>
        public static ProductCategoryListModel BindProductCategoryListModel(ProductCategory productCategoryModel)
        {
            ProductCategoryListModel productCategoryListModel = new ProductCategoryListModel();
            if (productCategoryModel != null)
            {
                productCategoryListModel.Name = productCategoryModel.Name;
                productCategoryListModel.ProductCategoryId = productCategoryModel.ProductCategoryId;
            }
            return productCategoryListModel;
        }
    }
}
