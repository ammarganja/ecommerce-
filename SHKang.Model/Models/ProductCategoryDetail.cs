namespace SHKang.Model.Models
{
    #region namespaces
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    public class ProductCategoryDetail
    {
        /// <summary>
        /// Gets or sets the product category detail identifier.
        /// </summary>
        /// <value>
        /// The product category detail identifier.
        /// </value>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ProductCategoryDetailId { get; set; }

        #region Foreign Key with Product
       
        [ForeignKey("Product")]
        public long ProductFK { get; set; }
       
        public virtual Product Product { get; set; }
        #endregion

        #region Foreign Key with ProductCategoryType

        [ForeignKey("ProductCategoryType")]
        public long ProductCategoryTypeFK { get; set; }

        public virtual ProductCategoryType ProductCategoryType { get; set; }
        #endregion
    }
}
