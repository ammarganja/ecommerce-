namespace SHKang.Model.ViewModels.Admin
{

    #region namespaces
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations; 
    #endregion

    public class AddOrderInvoiceModel
    {
        /// <summary>
        /// Gets or sets the order invoice identifier.
        /// </summary>
        /// <value>
        /// The order invoice identifier.
        /// </value>
        [Required]
        public string OrderInvoiceId { get; set; }

        /// <summary>
        /// Gets or sets the order identifier.
        /// </summary>
        /// <value>
        /// The order identifier.
        /// </value>
        [Required]
        public string OrderId { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        [Required]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>
        /// The created by.
        /// </value>
        [Required]
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the updated by.
        /// </summary>
        /// <value>
        /// The updated by.
        /// </value>
        [Required]
        public string UpdatedBy { get; set; }

        /// <summary>
        /// Gets or sets the shipping charges.
        /// </summary>
        /// <value>
        /// The shipping charges.
        /// </value>
        [Required]
        public string ShippingCharges { get; set; }

        /// <summary>
        /// Gets or sets the variable charges.
        /// </summary>
        /// <value>
        /// The variable charges.
        /// </value>
        [Required]
        public string VatCharges { get; set; }

        /// <summary>
        /// Gets or sets the tracking number.
        /// </summary>
        /// <value>
        /// The tracking number.
        /// </value>
        public string TrackingNumber { get; set; }

        /// <summary>
        /// Gets or sets the product detail.
        /// </summary>
        /// <value>
        /// The product detail.
        /// </value>
        public List<AddOrderInvoiceProductDetail> productDetail { get; set; }
    }

    public class AddOrderInvoiceProductDetail
    {
        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        [Required]
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the product color identifier.
        /// </summary>
        /// <value>
        /// The product color identifier.
        /// </value>
        [Required]
        public string ProductColorId { get; set; }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>
        /// The price.
        /// </value>
        [Required]
        public string Price { get; set; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>
        /// The quantity.
        /// </value>
        [Required]
        public string Quantity { get; set; }

        /// <summary>
        /// Gets or sets the size ratio.
        /// </summary>
        /// <value>
        /// The size ratio.
        /// </value>
        public string SizeRatio { get; set; }
    }

}
