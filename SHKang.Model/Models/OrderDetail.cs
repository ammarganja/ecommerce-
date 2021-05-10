namespace SHKang.Model.Models
{
    #region namespaces
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    public class OrderDetail
    {

        /// <summary>
        /// Gets or sets the order detail identifier.
        /// </summary>
        /// <value>
        /// The order detail identifier.
        /// </value>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long OrderDetailId { get; set; }

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
        public long Quantity { get; set; }

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

        #region Foreign Key with Order
        /// <summary>
        /// Gets or sets the order fk.
        /// </summary>
        /// <value>
        /// The order fk.
        /// </value>
        [ForeignKey("Order")]
        public long OrderFK { get; set; }
        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public virtual Order Order { get; set; }
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
