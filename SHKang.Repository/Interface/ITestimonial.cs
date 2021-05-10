
namespace SHKang.Repository.Interface
{
    #region namespaces

    using SHKang.Model.Models;
    using System.Collections.Generic;

    #endregion

    public interface ITestimonial
    {
        /// <summary>
        /// Adds the testimonial.
        /// </summary>
        /// <param name="testimonials">The testimonials.</param>
        /// <returns></returns>
        long AddTestimonial(Testimonials testimonials);

        /// <summary>
        /// Updates the testimonial.
        /// </summary>
        /// <param name="testimonials">The testimonials.</param>
        /// <returns></returns>
        long UpdateTestimonial(Testimonials testimonials);

        /// <summary>
        /// Deletes the testimonial.
        /// </summary>
        /// <param name="testimonialId">The testimonial identifier.</param>
        /// <returns></returns>
        bool DeleteTestimonial(long testimonialId);

        /// <summary>
        /// Gets the testimonials.
        /// </summary>
        /// <param name="searchString">The search string.</param>
        /// <returns></returns>
        List<Testimonials> GetTestimonials(string searchString);

        /// <summary>
        /// Gets the testimonials by identifier.
        /// </summary>
        /// <param name="testimonialId">The testimonial identifier.</param>
        /// <returns></returns>
        Testimonials GetTestimonialsById(long testimonialId);

    }
}
