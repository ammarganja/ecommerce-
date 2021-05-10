namespace SHKang.Model.ViewModels.Admin
{
    #region Namespaces

    using System.ComponentModel.DataAnnotations;

    #endregion
    public class DeleteTestimonialModel
    {

        /// <summary>
        /// Gets or sets the testimonial identifier.
        /// </summary>
        /// <value>
        /// The testimonial identifier.
        /// </value>
        [Required]
        public string TestimonialId { get; set; }
    }
}
