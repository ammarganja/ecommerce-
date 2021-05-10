namespace SHKang.Model.ViewModels.Admin
{
    public class GetCampaignProductModel:SearchPaginationListModel
    {
        /// <summary>
        /// Gets or sets the product category type identifier.
        /// </summary>
        /// <value>
        /// The product category type identifier.
        /// </value>
        public string productCategoryTypeId { get; set; }
    }
}
