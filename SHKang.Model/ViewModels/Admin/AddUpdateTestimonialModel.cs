namespace SHKang.Model.ViewModels.Admin
{

    #region namespaces

    using System.ComponentModel.DataAnnotations; 

    #endregion

    public class AddUpdateTestimonialModel
    {
        /// <summary>
        /// Gets or sets the testimonial identifier.
        /// </summary>
        /// <value>
        /// The testimonial identifier.
        /// </value>
        [Required]
        public long TestimonialId { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        [Required]
        public long UserId { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        [Required]
        public string Message { get; set; }

    }
}
