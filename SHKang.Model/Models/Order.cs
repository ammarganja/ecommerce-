namespace SHKang.Model.Models
{
    #region namespaces
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion
    public class Order
    {
        /// <summary>
        /// Gets or sets the order identifier.
        /// </summary>
        /// <value>
        /// The order identifier.
        /// </value>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long OrderId { get; set; }

        /// <summary>
        /// Gets or sets the po number.
        /// </summary>
        /// <value>
        /// The po number.
        /// </value>
        public string PONumber { get; set; }

        /// <summary>
        /// Gets or sets the unique order identifier.
        /// </summary>
        /// <value>
        /// The unique order identifier.
        /// </value>
        public string UniqueOrderId { get; set; }

        /// <summary>
        /// Gets or sets the order date.
        /// </summary>
        /// <value>
        /// The order date.
        /// </value>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// Gets or sets the sub total amount.
        /// </summary>
        /// <value>
        /// The sub total amount.
        /// </value>
        public decimal SubTotalAmount { get; set; }

        /// <summary>
        /// Gets or sets the total GST amount.
        /// </summary>
        /// <value>
        /// The total GST amount.
        /// </value>
        public decimal TotalGSTAmount { get; set; }

        /// <summary>
        /// Gets or sets the payable amount.
        /// </summary>
        /// <value>
        /// The payable amount.
        /// </value>
        public decimal PayableAmount { get; set; }

        /// <summary>
        /// Gets or sets the discount amount.
        /// </summary>
        /// <value>
        /// The discount amount.
        /// </value>
        public decimal DiscountAmount { get; set; }

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

        #region Foreign Key with UserAddress
        /// <summary>
        /// Gets or sets the user fk.
        /// </summary>
        /// <value>
        /// The user fk.
        /// </value>
        [ForeignKey("UserAddress")]
        public long UserAddressFK { get; set; }
        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public virtual UserAddress UserAddress { get; set; }
        #endregion

        #region Foreign Key with PromoCode

        /// <summary>
        /// Gets or sets the promocode fk.
        /// </summary>
        /// <value>
        /// The promocode fk.
        /// </value>
        [ForeignKey("PromoCode")]
        public long? PromocodeFK { get; set; }

        /// <summary>
        /// Gets or sets the promo code.
        /// </summary>
        /// <value>
        /// The promo code.
        /// </value>
        public virtual PromoCode PromoCode { get; set; }
        #endregion
    }
}
