namespace SHKang.Model.ViewModels.Admin
{

    #region namespaces

    using System.ComponentModel.DataAnnotations; 

    #endregion

    public class DeleteProductCategoryTypeModel
    {
        /// <summary>
        /// Gets or sets the product category type identifier.
        /// </summary>
        /// <value>
        /// The product category type identifier.
        /// </value>
        [Required]
        public string productCategoryTypeId { get; set; }

        /// <summary>
        /// Gets or sets the updated by.
        /// </summary>
        /// <value>
        /// The updated by.
        /// </value>
        [Required]
        public string updatedBy { get; set; }
    }


    public class ProductCategoryTypeDetailModel
    {
        /// <summary>
        /// Gets or sets the product category type identifier.
        /// </summary>
        /// <value>
        /// The product category type identifier.
        /// </value>
        [Required]
        public string productCategoryTypeId { get; set; }
    }

}
