namespace SHKang.Repository.Repository
{

    #region namespaces
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using SHKang.Core.Helpers;
    using SHKang.Model.Context;
    using SHKang.Model.Models;
    using SHKang.Repository.Interface;

    #endregion

    public class TestimonialRepository : ITestimonial
    {
        #region Private Variables

        /// <summary>
        /// The context
        /// </summary>
        private AppDbContext context;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TestimonialRepository"/> class.
        /// </summary>
        /// <param name="_context">The context.</param>
        public TestimonialRepository(AppDbContext _context)
        {
            context = _context;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the testimonial.
        /// </summary>
        /// <param name="testimonials">The testimonials.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public long AddTestimonial(Testimonials testimonials)
        {
            try
            {
                context.Testimonials.Add(testimonials);
                context.SaveChanges();
                return testimonials.TestimonialId;
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Deletes the testimonial.
        /// </summary>
        /// <param name="testimonialId">The testimonial identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool DeleteTestimonial(long testimonialId)
        {
            try
            {
                var testimonialModel = context.Testimonials.Where(x => x.TestimonialId == testimonialId).FirstOrDefault();
                if (testimonialModel != null)
                {
                    testimonialModel.IsDelete = true;
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Gets the testimonials.
        /// </summary>
        /// <param name="searchString">The search string.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public List<Testimonials> GetTestimonials(string searchString)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchString))
                {
                    return context.Testimonials.Include(x=>x.User).Where(x => x.IsDelete == false).ToList();
                }
                else
                {
                    return context.Testimonials.Include(x => x.User).Where(x => x.IsDelete == false && x.Message.Contains(searchString)).ToList();
                }
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Gets the testimonials by identifier.
        /// </summary>
        /// <param name="testimonialId">The testimonial identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Testimonials GetTestimonialsById(long testimonialId)
        {
            try
            {
                return context.Testimonials.Where(x => x.IsDelete == false && x.TestimonialId == testimonialId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Updates the testimonial.
        /// </summary>
        /// <param name="testimonials">The testimonials.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public long UpdateTestimonial(Testimonials testimonials)
        {
            try
            {
                var testimonialModel = context.Testimonials.Where(x => x.TestimonialId == testimonials.TestimonialId).FirstOrDefault();
                if (testimonialModel != null)
                {
                    testimonialModel.Message = testimonials.Message;
                    testimonialModel.ModifiedOn = testimonials.ModifiedOn;
                    context.SaveChanges();
                    return testimonials.TestimonialId;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }
        #endregion
    }
}
