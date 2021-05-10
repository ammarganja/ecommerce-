namespace SHKang.Model.ViewModels.Web
{
    using System.Collections.Generic;
    #region namespaces
    using System.ComponentModel.DataAnnotations;
    #endregion

    public class AddNewOrderModel
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        [Required]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the user address identifier.
        /// </summary>
        /// <value>
        /// The user address identifier.
        /// </value>
        [Required]
        public string UserAddressId { get; set; }

        /// <summary>
        /// Gets or sets the product detail.
        /// </summary>
        /// <value>
        /// The product detail.
        /// </value>
        public List<OrderProductDetail> ProductDetail { get; set; }

        /// <summary>
        /// Gets or sets the is mail send.
        /// </summary>
        /// <value>
        /// The is mail send.
        /// </value>
        [Required]
        public string IsMailSend { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>
        /// The created by.
        /// </value>
        [Required]
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the order identifier.
        /// </summary>
        /// <value>
        /// The order identifier.
        /// </value>
        [Required]
        public string OrderId { get; set; }

        /// <summary>
        /// Gets or sets the promocode.
        /// </summary>
        /// <value>
        /// The promocode.
        /// </value>
        public string Promocode { get; set; }
    }

    public class OrderProductDetail
    {
        [Required]
        public string ProductId { get; set; }

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
        /// Gets or sets the product color identifier.
        /// </summary>
        /// <value>
        /// The product color identifier.
        /// </value>
        [Required]
        public string ProductColorId { get; set; }
    }

}
