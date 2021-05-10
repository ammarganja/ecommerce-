namespace SHKang.Model.ViewModels.Admin
{

    #region namespaces
    using System.Collections.Generic; 
    #endregion

    public class ProductCategoryTypeListModel
    {
        /// <summary>
        /// Gets or sets the total type of the product category.
        /// </summary>
        /// <value>
        /// The total type of the product category.
        /// </value>
        public string Total { get; set; }

        /// <summary>
        /// Gets or sets the product category type data.
        /// </summary>
        /// <value>
        /// The product category type data.
        /// </value>
        public List<ProductCategoryTypeDataModel> Items { get; set; }
    }

    public class ProductCategoryTypeDataModel
    {
        /// <summary>
        /// Gets or sets the product category type identifier.
        /// </summary>
        /// <value>
        /// The product category type identifier.
        /// </value>
        public long ProductCategoryTypeId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the total product.
        /// </summary>
        /// <value>
        /// The total product.
        /// </value>
        public string TotalProduct { get; set; }
    }


}
