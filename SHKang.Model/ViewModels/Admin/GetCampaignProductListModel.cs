namespace SHKang.Model.ViewModels.Admin
{

    #region namespaces
    using System.ComponentModel.DataAnnotations; 
    #endregion

    public class GetCampaignProductListModel : SearchPaginationListModel
    {
        [Required]
        /// <summary>
        /// Gets or sets the product category type identifier.
        /// </summary>
        /// <value>
        /// The product category type identifier.
        /// </value>
        public long ProductCategoryTypeId { get; set; }
    }
}
