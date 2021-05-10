namespace SHKang.Model.ViewModels.Admin
{
    #region namespaces
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    #endregion

    public class AddProductModel
    {
        [Required]
        public long ProductId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>
        /// The price.
        /// </value>
        [Required]
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the product description.
        /// </summary>
        /// <value>
        /// The product description.
        /// </value>
        public string ProductDescription { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>
        /// The created by.
        /// </value>
        [Required]
        public long CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the product size identifier.
        /// </summary>
        /// <value>
        /// The product size identifier.
        /// </value>
        [Required]
        public long ProductSizeId { get; set; }

        /// <summary>
        /// Gets or sets the product size value.
        /// </summary>
        /// <value>
        /// The product size value.
        /// </value>
        [Required]
        public string ProductSizeValue { get; set; }

        /// <summary>
        /// Gets or sets the product category identifier.
        /// </summary>
        /// <value>
        /// The product category identifier.
        /// </value>
        [Required]
        public string ProductCategoryId { get; set; }
       
        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>
        /// The created by.
        /// </value>
        [Required]
        public long UpdatedBy { get; set; }

    }

    public class AddProductColorModel
    {
        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        public string productId { get; set; }
        /// <summary>
        /// Gets or sets the color identifier.
        /// </summary>
        /// <value>
        /// The color identifier.
        /// </value>
        public long ColorId { get; set; }

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>
        /// The image.
        /// </value>
        public List<IFormFile> image { get; set; }

        /// <summary>
        /// Gets or sets the delete image identifier.
        /// </summary>
        /// <value>
        /// The delete image identifier.
        /// </value>
        public string DeleteImageId { get; set; }

    }
}
