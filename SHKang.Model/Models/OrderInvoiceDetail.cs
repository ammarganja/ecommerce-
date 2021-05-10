namespace SHKang.Model.Models
{
    #region namespaces
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    public class OrderInvoiceDetail
    {
        /// <summary>
        /// Gets or sets the order detail identifier.
        /// </summary>
        /// <value>
        /// The order detail identifier.
        /// </value>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long OrderInvoiceDetailId { get; set; }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>
        /// The price.
        /// </value>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>
        /// The quantity.
        /// </value>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Gets or sets the sub total amount.
        /// </summary>
        /// <value>
        /// The sub total amount.
        /// </value>
        public decimal SubTotalAmount { get; set; }

        /// <summary>
        /// Gets or sets the GST amount.
        /// </summary>
        /// <value>
        /// The GST amount.
        /// </value>
        public decimal GSTAmount { get; set; }

        #region Foreign Key with OrderInvoice
        /// <summary>
        /// Gets or sets the order fk.
        /// </summary>
        /// <value>
        /// The order fk.
        /// </value>
        [ForeignKey("OrderInvoice")]
        public long OrderInvoiceFK { get; set; }
        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public virtual OrderInvoice OrderInvoice { get; set; }
        #endregion

        #region Foreign Key with Product
        /// <summary>
        /// Gets or sets the product fk.
        /// </summary>
        /// <value>
        /// The product fk.
        /// </value>
        [ForeignKey("Product")]
        public long ProductFK { get; set; }
        /// <summary>
        /// Gets or sets the product.
        /// </summary>
        /// <value>
        /// The product.
        /// </value>
        public virtual Product Product { get; set; }
        #endregion

        #region Foreign Key with ProductColor
        /// <summary>
        /// Gets or sets the product color fk.
        /// </summary>
        /// <value>
        /// The product color fk.
        /// </value>
        [ForeignKey("ProductColor")]
        public long ProductColorFK { get; set; }
        /// <summary>
        /// Gets or sets the color of the product.
        /// </summary>
        /// <value>
        /// The color of the product.
        /// </value>
        public virtual ProductColor ProductColor { get; set; }
        #endregion
    }
}
