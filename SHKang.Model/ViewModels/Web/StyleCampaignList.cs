namespace SHKang.Model.ViewModels.Web
{
    #region namespaces
    using SHKang.Model.ViewModels.Admin;
    using System.Collections.Generic; 
    #endregion

    public class StyleCampaignList
    {
        /// <summary>
        /// Gets or sets the product lists.
        /// </summary>
        /// <value>
        /// The product lists.
        /// </value>
        public List<StyleCampaignProductList> ProductLists { get; set; }

        /// <summary>
        /// Gets or sets the category list.
        /// </summary>
        /// <value>
        /// The category list.
        /// </value>
        public List<ProductCategoryListModel> CategoryList { get; set; }

        /// <summary>
        /// Gets or sets the total products.
        /// </summary>
        /// <value>
        /// The total products.
        /// </value>
        public string TotalProducts { get; set; }

        /// <summary>
        /// Gets or sets the campaign model.
        /// </summary>
        /// <value>
        /// The campaign model.
        /// </value>
        public StyleCampaignDetailModel CampaignModel { get; set; }
    }


    public class StyleCampaignDetailModel
    {
        /// <summary>
        /// Gets or sets the campaign identifier.
        /// </summary>
        /// <value>
        /// The campaign identifier.
        /// </value>
        public string CampaignId { get; set; }

        /// <summary>
        /// Gets or sets the name of the campaign.
        /// </summary>
        /// <value>
        /// The name of the campaign.
        /// </value>
        public string CampaignName { get; set; }

        /// <summary>
        /// Gets or sets the campaign image.
        /// </summary>
        /// <value>
        /// The campaign image.
        /// </value>
        public string CampaignImage { get; set; }

    }
 
    
}
