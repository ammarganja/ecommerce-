namespace SHKang.Model.ViewModels.Admin
{
    #region namespaces

    using System.ComponentModel.DataAnnotations;

    #endregion

    public class DeleteProductCategoryModel
    {
        /// <summary>
        /// Gets or sets the product category identifier.
        /// </summary>
        /// <value>
        /// The product category identifier.
        /// </value>
        [Required]
        public string productCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the updated by.
        /// </summary>
        /// <value>
        /// The updated by.
        /// </value>
        [Required]
        public string updatedBy { get; set; }
    }

    public class ProductCategoryDetailModel
    {
        /// <summary>
        /// Gets or sets the product category identifier.
        /// </summary>
        /// <value>
        /// The product category identifier.
        /// </value>
        [Required]
        public string productCategoryId { get; set; }
    }
}
