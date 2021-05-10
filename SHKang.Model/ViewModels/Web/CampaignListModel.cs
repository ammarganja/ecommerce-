using System.Collections.Generic;

namespace SHKang.Model.ViewModels.Web
{
    public class CampaignListModel
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
        /// Gets or sets the total product.
        /// </summary>
        /// <value>
        /// The total product.
        /// </value>
        public string TotalProduct { get; set; }

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>
        /// The image.
        /// </value>
        public string Image { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the product ids.
        /// </summary>
        /// <value>
        /// The product ids.
        /// </value>
        public List<long> ProductIds { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is showin frontend.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is showin frontend; otherwise, <c>false</c>.
        /// </value>
        public bool IsShowinFrontend { get; set; }
    }
}
