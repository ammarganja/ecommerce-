namespace SHKang.Model.ViewModels.Admin
{
    #region namespaces
    using Microsoft.AspNetCore.Http;
    using System.ComponentModel.DataAnnotations; 
    #endregion

    public class AddUpdateCampaignModel
    {
        /// <summary>
        /// Gets or sets the campaign identifier.
        /// </summary>
        /// <value>
        /// The campaign identifier.
        /// </value>
        [Required]
        public long CampaignId { get; set; }

        /// <summary>
        /// Gets or sets the name of the campaign.
        /// </summary>
        /// <value>
        /// The name of the campaign.
        /// </value>
        [Required]
        public string CampaignName { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>
        /// The image.
        /// </value>
        public IFormFile Image { get; set; }

        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        [Required]
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the updated by.
        /// </summary>
        /// <value>
        /// The updated by.
        /// </value>
        [Required]
        public string UpdatedBy { get; set; }

        [Required]
        /// <summary>
        /// Gets or sets a value indicating whether this instance is image deleted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is image deleted; otherwise, <c>false</c>.
        /// </value>
        public bool IsImageDeleted{ get; set; }

        [Required]
        /// <summary>
        /// Gets or sets a value indicating whether this instance is showin frontend.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is showin frontend; otherwise, <c>false</c>.
        /// </value>
        public bool IsShowinFrontend { get; set; }
    }
}
