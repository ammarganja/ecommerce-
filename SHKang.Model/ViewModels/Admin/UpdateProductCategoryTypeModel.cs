namespace SHKang.Model.ViewModels.Admin
{
    #region namespaces
    using System.ComponentModel.DataAnnotations; 
    #endregion

    public class UpdateProductCategoryTypeModel
    {
        /// <summary>
        /// Gets or sets the product category type identifier.
        /// </summary>
        /// <value>
        /// The product category type identifier.
        /// </value>
        [Required]
        public long ProductCategoryTypeId { get; set; }


        /// <summary>
        /// Gets or sets the type of the category.
        /// </summary>
        /// <value>
        /// The type of the category.
        /// </value>
        [Required]
        public string CategoryType { get; set; }


        ///// <summary>
        ///// Gets or sets the product category identifier.
        ///// </summary>
        ///// <value>
        ///// The product category identifier.
        ///// </value>
        //[Required]
        //public long ProductCategoryId { get; set; }


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
