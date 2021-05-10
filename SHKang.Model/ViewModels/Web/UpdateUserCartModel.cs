using System.ComponentModel.DataAnnotations;

namespace SHKang.Model.ViewModels.Web
{
    public class UpdateUserCartModel
    {
        [Required]
        /// <summary>
        /// Gets or sets the cart identifier.
        /// </summary>
        /// <value>
        /// The cart identifier.
        /// </value>
        public long cartId { get; set; }

        [Required]
        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        public long productId { get; set; }

        [Required]
        /// <summary>
        /// Gets or sets the product color identifier.
        /// </summary>
        /// <value>
        /// The product color identifier.
        /// </value>
        public long productColorId { get; set; }

        [Required]
        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>
        /// The quantity.
        /// </value>
        public decimal quantity { get; set; }

        [Required]
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public long userId { get; set; }

        [Required]
        /// <summary>
        /// Gets or sets the updated by.
        /// </summary>
        /// <value>
        /// The updated by.
        /// </value>
        public long updatedBy { get; set; }

    }
}
