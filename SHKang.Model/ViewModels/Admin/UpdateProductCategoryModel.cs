namespace SHKang.Model.ViewModels.Admin
{
    #region namespaces
    using System.ComponentModel.DataAnnotations;
    #endregion

    public class UpdateProductCategoryModel
    {

        /// <summary>
        /// Gets or sets the product category identifier.
        /// </summary>
        /// <value>
        /// The product category identifier.
        /// </value>
        [Required]
        public long ProductCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the updated by.
        /// </summary>
        /// <value>
        /// The updated by.
        /// </value>
        [Required]
        public long UpdatedBy { get; set; }
    }
}
