namespace SHKang.Repository.Interface
{
    #region namespaces
    using SHKang.Model.Models;
    using System.Collections.Generic;

    #endregion
    public interface ISize
    {
        /// <summary>
        /// Adds the size.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <param name="sizeDetailModel">The size detail model.</param>
        /// <returns></returns>
        long AddSize(Size size,List<SizeDetail> sizeDetailModel);

        /// <summary>
        /// Updates the size.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <param name="sizeDetailModel">The size detail model.</param>
        /// <returns></returns>
        long UpdateSize(Size size, List<SizeDetail> sizeDetailModel);

        /// <summary>
        /// Deletes the size.
        /// </summary>
        /// <param name="SizeId">The size identifier.</param>
        /// <returns></returns>
        bool DeleteSize(long sizeId);

        /// <summary>
        /// Gets the size list.
        /// </summary>
        /// <returns></returns>
        List<SizeDetail> GetSizeList(string searchString);

        /// <summary>
        /// Gets the total size count.
        /// </summary>
        /// <returns></returns>
        long GetTotalSizeCount();

        /// <summary>
        /// Gets the total product size count.
        /// </summary>
        /// <param name="sizeIds">The size ids.</param>
        /// <returns></returns>
        List<Product> GetTotalProductSizeCount(List<long> sizeIds);

        /// <summary>
        /// Gets the size details.
        /// </summary>
        /// <param name="SizeId">The size identifier.</param>
        /// <returns></returns>
        List<SizeDetail> GetSizeDetails(long SizeId);

        /// <summary>
        /// Gets the size details.
        /// </summary>
        /// <param name="productIds">The product ids.</param>
        /// <returns></returns>
        List<SizeDetail> GetSizeDetailsBySizeIds(List<long> sizeIds);
    }
}
