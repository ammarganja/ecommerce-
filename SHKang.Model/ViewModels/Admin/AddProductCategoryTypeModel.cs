using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SHKang.Model.ViewModels.Admin
{
    public class AddProductCategoryTypeModel
    {
        /// <summary>
        /// Gets or sets the product category.
        /// </summary>
        /// <value>
        /// The product category.
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
        /// Gets or sets the created by.
        /// </summary>
        /// <value>
        /// The created by.
        /// </value>
        [Required]
        public long CreatedBy { get; set; }
    }
}
