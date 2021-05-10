namespace SHKang.Model.ViewModels.Admin
{
    #region namespaces
    using System.Collections.Generic; 
    #endregion

    public class ProductDetailModel
    {
        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the product description.
        /// </summary>
        /// <value>
        /// The product description.
        /// </value>
        public string ProductDescription { get; set; }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>
        /// The price.
        /// </value>
        public string Price { get; set; }

        /// <summary>
        /// Gets or sets the is delete.
        /// </summary>
        /// <value>
        /// The is delete.
        /// </value>
        public string IsDelete { get; set; }

        /// <summary>
        /// Gets or sets the size of the product.
        /// </summary>
        /// <value>
        /// The size of the product.
        /// </value>
        public string ProductSize { get; set; }

        /// <summary>
        /// Gets or sets the product size identifier.
        /// </summary>
        /// <value>
        /// The product size identifier.
        /// </value>
        public string ProductSizeId { get; set; }

        /// <summary>
        /// Gets or sets the color list.
        /// </summary>
        /// <value>
        /// The color list.
        /// </value>
        public List<ProductDetailColorListModel> ColorList { get; set; }

        /// <summary>
        /// Gets or sets the image list.
        /// </summary>
        /// <value>
        /// The image list.
        /// </value>
        public List<ProductDetailImageListModel> ImageList { get; set; }

        /// <summary>
        /// Gets or sets the category list.
        /// </summary>
        /// <value>
        /// The category list.
        /// </value>
        public List<ProductDetailCategoryTypeListModel> CategoryList { get; set; }

        /// <summary>
        /// Gets or sets the size group list.
        /// </summary>
        /// <value>
        /// The size group list.
        /// </value>
        public List<ProductDetailSizeGroupListModel> SizeGroupList { get; set; }

    }

    public class ProductDetailImageListModel
    {
        /// <summary>
        /// Gets or sets the product color identifier.
        /// </summary>
        /// <value>
        /// The product color identifier.
        /// </value>
        public string ProductColorId { get; set; }

        /// <summary>
        /// Gets or sets the product image identifier.
        /// </summary>
        /// <value>
        /// The product image identifier.
        /// </value>
        public string ProductImageId { get; set; }

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>
        /// The image.
        /// </value>
        public string Image { get; set; }
    }

    public class ProductDetailCategoryTypeListModel
    {
        /// <summary>
        /// Gets or sets the product category type identifier.
        /// </summary>
        /// <value>
        /// The product category type identifier.
        /// </value>
        public string ProductCategoryTypeId { get; set; }

        /// <summary>
        /// Gets or sets the type of the category.
        /// </summary>
        /// <value>
        /// The type of the category.
        /// </value>
        public string CategoryType { get; set; }
    }

    public class ProductDetailColorListModel
    {
        /// <summary>
        /// Gets or sets the product color identifier.
        /// </summary>
        /// <value>
        /// The product color identifier.
        /// </value>
        public string ProductColorId { get; set; }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public string Color { get; set; }

        /// <summary>
        /// Gets or sets the product color identifier 1.
        /// </summary>
        /// <value>
        /// The product color identifier 1.
        /// </value>
        public string ProductColorId_1 { get; set; }
    }

    public class ProductDetailSizeGroupListModel
    {
        /// <summary>
        /// Gets or sets the product color identifier.
        /// </summary>
        /// <value>
        /// The product color identifier.
        /// </value>
        public string ItemName { get; set; }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public string ItemValue { get; set; }
    }

}
