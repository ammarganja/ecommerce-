namespace SHKang.Repository.Interface
{
    using Microsoft.AspNetCore.Http;
    #region Namespaces
    using SHKang.Model.Models;
    using SHKang.Model.ViewModels.Admin;
    using System.Collections.Generic; 
    #endregion

    public interface IProduct
    {
        /// <summary>
        /// Adds the product.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="productImages">The product images.</param>
        /// <param name="productColors">The product colors.</param>
        /// <param name="productCategoryDetails">The product category details.</param>
        /// <returns></returns>
        long AddProduct(Product model, List<ProductCategoryDetail> productCategoryDetails,string hostPath, string[] ProductSizeValues);

        /// <summary>
        /// Gets the product list.
        /// </summary>
        /// <param name="pageNo">The page no.</param>
        /// <param name="rowsPerPage">The rows per page.</param>
        /// <param name="searchString">The search string.</param>
        /// <param name="hostingPath">The hosting path.</param>
        /// <param name="column">The column.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="colorId">The color identifier.</param>
        /// <param name="categoryTypeId">The category type identifier.</param>
        /// <param name="sizeId">The size identifier.</param>
        /// <returns></returns>
        ProductListModel GetProductList(int pageNo,int rowsPerPage,string searchString, string hostingPath,string column,string direction,string colorId,string categoryTypeId,string sizeId);

        /// <summary>
        /// Deletes the product.
        /// </summary>
        /// <param name="ProductId">The product identifier.</param>
        /// <returns></returns>
        bool DeleteProduct(long productId,long updatedBy);

        /// <summary>
        /// Gets the product detail.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="hostingURL">The hosting URL.</param>
        /// <returns></returns>
        ProductDetailModel GetProductDetail(long productId, string hostingURL);

        /// <summary>
        /// Updates the product.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="productColors">The product colors.</param>
        /// <param name="productCategoryDetails">The product category details.</param>
        /// <returns></returns>
        long UpdateProduct(Product model, List<ProductCategoryDetail> productCategoryDetails, string hostPath, string[] ProductSizeValues);

        /// <summary>
        /// Gets the product images.
        /// </summary>
        /// <param name="ProductId">The product identifier.</param>
        /// <returns></returns>
        List<ProductImage> GetProductImages(long productId);

        /// <summary>
        /// Changes the product status.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        bool ChangeProductStatus(long productId, int status);

        /// <summary>
        /// Gets the product images by product ids.
        /// </summary>
        /// <param name="productsIds">The products ids.</param>
        /// <returns></returns>
        List<ProductImage> GetProductImagesByProductIds(List<long> productsIds);

        /// <summary>
        /// Gets the order product list.
        /// </summary>
        /// <param name="pageNo">The page no.</param>
        /// <param name="rowsPerPage">The rows per page.</param>
        /// <param name="searchString">The search string.</param>
        /// <param name="hostingURL">The hosting URL.</param>
        /// <param name="column">The column.</param>
        /// <param name="direction">The direction.</param>
        /// <returns></returns>
        OrderProductListModel GetOrderProductList(int pageNo, int rowsPerPage, string searchString, string hostingURL, string column, string direction);

        /// <summary>
        /// Gets the product size details.
        /// </summary>
        /// <param name="productIds">The product ids.</param>
        /// <returns></returns>
        List<ProductSizeDetail> GetProductSizeDetails(List<long> productIds);

        /// <summary>
        /// Adds the product color image.
        /// </summary>
        /// <param name="ProductId">The product identifier.</param>
        /// <param name="ColorId">The color identifier.</param>
        /// <param name="productImages">The product images.</param>
        /// <param name="hostPath">The host path.</param>
        /// <param name="deleteImageIds">The delete image ids.</param>
        /// <returns></returns>
        long AddProductColorImage(long ProductId,long ColorId, List<IFormFile> productImages, string hostPath,string deleteImageIds);

        /// <summary>
        /// Deletes the color of the product.
        /// </summary>
        /// <param name="ProductId">The product identifier.</param>
        /// <param name="ColorId">The color identifier.</param>
        /// <param name="hostPath">The host path.</param>
        /// <returns></returns>
        bool DeleteProductColor(long ProductId, long ColorId, string hostPath);

        /// <summary>
        /// Gets the image count by product color identifier.
        /// </summary>
        /// <param name="ProductId">The product identifier.</param>
        /// <param name="ColorId">The color identifier.</param>
        /// <returns></returns>
        long GetImageCountByProductColorId(long ProductId, long ColorId);

        /// <summary>
        /// Gets the product category detail.
        /// </summary>
        /// <param name="productIds">The product ids.</param>
        /// <returns></returns>
        List<ProductCategoryDetail> GetProductCategoryDetail(List<long> productIds);
    }
}
