namespace SHKang.Model.Models
{
    #region namespaces
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema; 
    #endregion

    public class ProductColor
    {
        /// <summary>
        /// Gets or sets the product color identifier.
        /// </summary>
        /// <value>
        /// The product color identifier.
        /// </value>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ProductColorId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is delete.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is delete; otherwise, <c>false</c>.
        /// </value>
        public bool IsDelete { get; set; }

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

        #region Foreign Key with Color

        /// <summary>
        /// Gets or sets the color fk.
        /// </summary>
        /// <value>
        /// The color fk.
        /// </value>
        [ForeignKey("Color")]
        public long ColorFK { get; set; }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public virtual Color Color { get; set; }
        #endregion
    }
}
