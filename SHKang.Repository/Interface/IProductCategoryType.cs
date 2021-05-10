namespace SHKang.Repository.Interface
{
    #region namespaces
    using SHKang.Model.Models;
    using System.Collections.Generic;
    #endregion

    public interface IProductCategoryType
    {
        /// <summary>
        /// Adds the new type of the product category.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        long AddNewProductCategoryType(ProductCategoryType productCategoryType, string productIds = "");

        /// <summary>
        /// Updates the type of the product category.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        long UpdateProductCategoryType(ProductCategoryType productCategoryType, long updatedBy, string productIds = "");

        /// <summary>
        /// Deletes the type of the product category.
        /// </summary>
        /// <param name="ProductCategoryTypeId">The product category type identifier.</param>
        /// <returns></returns>
        bool DeleteProductCategoryType(long productCategoryTypeId, long updatedBy);

        /// <summary>
        /// Products the type of the category.
        /// </summary>
        /// <returns></returns>
        List<ProductCategoryType> ProductCategoryType(string searchString);

        /// <summary>
        /// Products the category type detail.
        /// </summary>
        /// <param name="ProductCategoryTypeId">The product category type identifier.</param>
        /// <returns></returns>
        ProductCategoryType ProductCategoryTypeDetail(long productCategoryTypeId);

        /// <summary>
        /// Gets the product category details.
        /// </summary>
        /// <param name="productCategoryTypeId">The product category type identifier.</param>
        /// <returns></returns>
        List<ProductCategoryDetail> GetProductCategoryDetails(List<long> productCategoryTypeId);

        /// <summary>
        /// Gets the total product category type count.
        /// </summary>
        /// <returns></returns>
        long GetTotalProductCategoryTypeCount();

        /// <summary>
        /// Gets the type of all product category.
        /// </summary>
        /// <returns></returns>
        List<ProductCategoryType> GetAllProductCategoryType();
    }
}
