namespace SHKang.Model.Models
{
    #region namespaces
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion


   public class ProductSizeDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ProductSizeDetailId { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public string Name { get; set; }


        #region Foreign Key with Product

        /// <summary>
        /// Gets or sets the color fk.
        /// </summary>
        /// <value>
        /// The color fk.
        /// </value>
        [ForeignKey("Product")]
        public long ProductFK { get; set; }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public virtual Product Product { get; set; }
        #endregion
    }
}
