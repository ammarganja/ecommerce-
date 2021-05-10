namespace SHKang.Repository.Interface
{
    #region namespaces
    using SHKang.Model.Models;
    using System.Collections.Generic;

    #endregion

    public interface IProductCategory
    {
        /// <summary>
        /// Adds the product category.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        long AddProductCategory(ProductCategory productCategory);

        /// <summary>
        /// Deletes the product category.
        /// </summary>
        /// <param name="ProductCategoryId">The product category identifier.</param>
        /// <returns></returns>
        bool DeleteProductCategory(long productCategoryId, long updatedBy);

        /// <summary>
        /// Updates the product category.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        long UpdateProductCategory(ProductCategory productCategory, long updatedBy);

        /// <summary>
        /// Gets the product category.
        /// </summary>
        /// <returns></returns>
        List<ProductCategory> GetProductCategory(string searchString);

        /// <summary>
        /// Gets the product category detail.
        /// </summary>
        /// <param name="ProductCategoryId">The product category identifier.</param>
        /// <returns></returns>
        ProductCategory GetProductCategoryDetail(long productCategoryId);
    }
}
