namespace SHKang.Repository.Interface
{
    #region namespaces
    using SHKang.Model.Models;
    using SHKang.Model.ViewModels.Admin;
    using SHKang.Model.ViewModels.Web;
    using System.Collections.Generic;

    #endregion

    public interface IStyleCampaign
    {

        /// <summary>
        /// Gets the style campaign list.
        /// </summary>
        /// <param name="productCategoryId">The product category identifier.</param>
        /// <param name="searchText">The search text.</param>
        /// <param name="pageNo">The page no.</param>
        /// <param name="recordPerPage">The record per page.</param>
        /// <param name="productCategoryTypeId">The product category type identifier.</param>
        /// <param name="hostingPath">The hosting path.</param>
        /// <param name="column">The column.</param>
        /// <param name="direction">The direction.</param>
        /// <returns></returns>
        StyleCampaignList GetStyleCampaignList(string productCategoryId, string searchText, int pageNo, int recordPerPage, string productCategoryTypeId, string hostingPath, string column, string direction);

        /// <summary>
        /// Gets the campaign list.
        /// </summary>
        /// <param name="searchString">The search string.</param>
        /// <param name="IsShowFrontend">if set to <c>true</c> [is show frontend].</param>
        /// <returns></returns>
        List<ProductCategoryType> GetCampaignList(string searchString,bool IsShowFrontend);

        /// <summary>
        /// Gets the campaign product count.
        /// </summary>
        /// <param name="productCategoryTypeId">The product category type identifier.</param>
        /// <returns></returns>
        List<ProductCategoryDetail> GetCampaignProductCount(List<long> productCategoryTypeId);

        /// <summary>
        /// Gets the campaign product list.
        /// </summary>
        /// <param name="pageNo">The page no.</param>
        /// <param name="rowsPerPage">The rows per page.</param>
        /// <param name="searchString">The search string.</param>
        /// <param name="hostingURL">The hosting URL.</param>
        /// <param name="column">The column.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="productCategoryTypeId">The product category type identifier.</param>
        /// <returns></returns>
        OrderProductListModel GetCampaignProductList(int pageNo, int rowsPerPage, string searchString, string hostingURL, string column, string direction, long productCategoryTypeId);

        /// <summary>
        /// Gets the campaign detail.
        /// </summary>
        /// <param name="campaignId">The campaign identifier.</param>
        /// <param name="hostingPath">The hosting path.</param>
        /// <returns></returns>
        StyleCampaignDetailModel GetCampaignDetail(int campaignId,string hostingPath);
    }
}
