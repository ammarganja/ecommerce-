using SHKang.Model.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace SHKang.Model.ViewModels.Admin
{
   public  class OrderInvoiceDetailProductModel
    {

        /// <summary>
        /// Gets or sets the order detail identifier.
        /// </summary>
        /// <value>
        /// The order detail identifier.
        /// </value>
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
        /// Gets or sets the color of the product.
        /// </summary>
        /// <value>
        /// The color of the product.
        /// </value>
        public virtual ProductColor ProductColor { get; set; }
        #endregion

        #region Foreign Key with ProductImage

        /// <summary>
        /// Gets or sets the color of the product.
        /// </summary>
        /// <value>
        /// The color of the product.
        /// </value>
        public virtual ProductImage ProductImage { get; set; }
        #endregion
    }
}
