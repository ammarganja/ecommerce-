namespace SHKang.Model.ViewModels.Admin
{

    #region namespaces
    using System.ComponentModel.DataAnnotations;
    #endregion

    public class UpdateOrderInvoiceStatus
    {
        /// <summary>
        /// Gets or sets the order invoice identifier.
        /// </summary>
        /// <value>
        /// The order invoice identifier.
        /// </value>
        [Required]
        public string orderInvoiceId { get; set; }

        /// <summary>
        /// Gets or sets the order status identifier.
        /// </summary>
        /// <value>
        /// The order status identifier.
        /// </value>
        [Required]
        public string orderStatusId { get; set; }

        /// <summary>
        /// Gets or sets the updated by.
        /// </summary>
        /// <value>
        /// The updated by.
        /// </value>
        [Required]
        public string updatedBy { get; set; }
    }

    public class GetOrderInvoiceDetailModel
    {
        /// <summary>
        /// Gets or sets the order invoice identifier.
        /// </summary>
        /// <value>
        /// The order invoice identifier.
        /// </value>
        [Required]
        public string orderInvoiceId { get; set; }
    }
}
