namespace SHKang.Model.Models
{
    #region namespaces
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema; 
    #endregion

    public class UserCartDetails
    {
        /// <summary>
        /// Gets or sets the user cart identifier.
        /// </summary>
        /// <value>
        /// The user cart identifier.
        /// </value>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long UserCartId { get; set; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>
        /// The quantity.
        /// </value>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>
        /// The price.
        /// </value>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is delete.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is delete; otherwise, <c>false</c>.
        /// </value>
        public bool IsDelete { get; set; }

        /// <summary>
        /// Gets or sets the created on.
        /// </summary>
        /// <value>
        /// The created on.
        /// </value>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>
        /// The created by.
        /// </value>
        public long CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the modified on.
        /// </summary>
        /// <value>
        /// The modified on.
        /// </value>
        public DateTime? ModifiedOn { get; set; }

        /// <summary>
        /// Gets or sets the updated by.
        /// </summary>
        /// <value>
        /// The updated by.
        /// </value>
        public long? UpdatedBy { get; set; }


        /// <summary>
        /// Gets or sets the sub total amount.
        /// </summary>
        /// <value>
        /// The sub total amount.
        /// </value>
        public decimal SubTotalAmount { get; set; }

        #region Foreign Key with User
        /// <summary>
        /// Gets or sets the user fk.
        /// </summary>
        /// <value>
        /// The user fk.
        /// </value>
        [ForeignKey("User")]
        public long UserFK { get; set; }
        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public virtual User User { get; set; }
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
    }
}
