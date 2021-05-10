namespace SHKang.Model.ViewModels.Admin
{

    #region namespaces
    using System.ComponentModel.DataAnnotations; 
    #endregion


    public class DeletePromocodeModel
    {
        /// <summary>
        /// Gets or sets the promo code identifier.
        /// </summary>
        /// <value>
        /// The promo code identifier.
        /// </value>
        [Required]
        public string promoCodeId { get; set; }

        /// <summary>
        /// Gets or sets the updated by.
        /// </summary>
        /// <value>
        /// The updated by.
        /// </value>
        [Required]
        public string updatedBy { get; set; }
    }

    public class GetPromoCodeDetailModel
    {
        /// <summary>
        /// Gets or sets the promo code identifier.
        /// </summary>
        /// <value>
        /// The promo code identifier.
        /// </value>
        [Required]
        public string promoCodeId { get; set; }
    }

    public class ChangePromoCodeStatusModel
    {
        /// <summary>
        /// Gets or sets the promo code identifier.
        /// </summary>
        /// <value>
        /// The promo code identifier.
        /// </value>
        [Required]
        public string promoCodeId { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [Required]
        public string Status { get; set; }
    }
}
