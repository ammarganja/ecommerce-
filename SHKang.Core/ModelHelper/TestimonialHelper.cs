namespace SHKang.Core.ModelHelper
{
    using SHKang.Core.Constant;
    using SHKang.Core.Enums;
    using SHKang.Core.Helpers;
    #region namespaces

    using SHKang.Model.Models;
    using SHKang.Model.ViewModels.Admin;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion

    public static class TestimonialHelper
    {
        /// <summary>
        /// Binds the testimonial model.
        /// </summary>
        /// <param name="addTestimonial">The add testimonial.</param>
        /// <returns></returns>
        public static Testimonials BindTestimonialModel(AddUpdateTestimonialModel addTestimonial)
        {
            Testimonials testimonialModel = new Testimonials();
            if (addTestimonial != null)
            {
                if (addTestimonial.TestimonialId <= 0)
                {
                    testimonialModel.CreatedOn = DateTime.Now;
                    testimonialModel.UserFK = addTestimonial.UserId;
                }
                else
                {
                    testimonialModel.ModifiedOn = DateTime.Now;
                    testimonialModel.TestimonialId = addTestimonial.TestimonialId;
                }
                testimonialModel.Message = addTestimonial.Message;

            }
            return testimonialModel;
        }

        /// <summary>
        /// Binds the testimonial list model.
        /// </summary>
        /// <param name="testimonialList">The testimonial list.</param>
        /// <returns></returns>
        public static List<TestimonialListModel> BindTestimonialListModel(List<Testimonials> testimonialList, string hostPath)
        {
            List<TestimonialListModel> testimonialListModel = new List<TestimonialListModel>();
            if (testimonialList != null)
            {
                foreach (var item in testimonialList)
                {
                    testimonialListModel.Add(new TestimonialListModel
                    {
                        Message = item.Message,
                        TestimonialDate = DBHelper.ParseString(item.CreatedOn).Split(' ')[0],
                        TestimonialId = DBHelper.ParseString(item.TestimonialId),
                        UserEmail = item.User.EmailAddress,
                        UserFirstName = item.User.FirstName,
                        UserId = DBHelper.ParseString(item.UserFK),
                        UserLastName = item.User.LastName,
                        UserMobileNumber = DBHelper.ParseString(item.User.PhoneNumber),
                        UserImage = hostPath + "/" + Constants.DefaultUserPath.Replace(@"\", "/")
                    });
                }

            }
            return testimonialListModel;
        }

        /// <summary>
        /// Gets the sorted testimonial.
        /// </summary>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="sortDirection">The sort direction.</param>
        /// <param name="testimonialList">The testimonial list.</param>
        /// <returns></returns>
        public static List<TestimonialListModel> GetSortedTestimonial(string sortColumn, string sortDirection, List<TestimonialListModel> testimonialList)
        {
            List<TestimonialListModel> testimonials = new List<TestimonialListModel>();
            if (sortColumn.ToLower().Equals(DBHelper.ParseString(TestimonialSize.testimonialid)) && sortDirection.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.asc)))
            {
                testimonials = testimonialList.OrderBy(x => x.TestimonialId).ToList();
            }
            else if (sortColumn.ToLower().Equals(DBHelper.ParseString(TestimonialSize.testimonialid)) && sortDirection.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.desc)))
            {
                testimonials = testimonialList.OrderByDescending(x => x.TestimonialId).ToList();
            }
            else if (sortColumn.ToLower().Equals(DBHelper.ParseString(TestimonialSize.message)) && sortDirection.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.asc)))
            {
                testimonials = testimonialList.OrderBy(x => x.Message).ToList();
            }
            else if (sortColumn.ToLower().Equals(DBHelper.ParseString(TestimonialSize.message)) && sortDirection.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.desc)))
            {
                testimonials = testimonialList.OrderByDescending(x => x.Message).ToList();
            }
            else if (sortColumn.ToLower().Equals(DBHelper.ParseString(TestimonialSize.testimonialdate)) && sortDirection.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.asc)))
            {
                testimonials = testimonialList.OrderBy(x => x.TestimonialDate).ToList();
            }
            else if (sortColumn.ToLower().Equals(DBHelper.ParseString(TestimonialSize.testimonialdate)) && sortDirection.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.desc)))
            {
                testimonials = testimonialList.OrderByDescending(x => x.TestimonialDate).ToList();
            }
            else if (sortColumn.ToLower().Equals(DBHelper.ParseString(TestimonialSize.useremail)) && sortDirection.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.asc)))
            {
                testimonials = testimonialList.OrderBy(x => x.UserEmail).ToList();
            }
            else if (sortColumn.ToLower().Equals(DBHelper.ParseString(TestimonialSize.useremail)) && sortDirection.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.desc)))
            {
                testimonials = testimonialList.OrderByDescending(x => x.UserEmail).ToList();
            }
            else if (sortColumn.ToLower().Equals(DBHelper.ParseString(TestimonialSize.usermobilenumber)) && sortDirection.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.asc)))
            {
                testimonials = testimonialList.OrderBy(x => x.UserMobileNumber).ToList();
            }
            else if (sortColumn.ToLower().Equals(DBHelper.ParseString(TestimonialSize.usermobilenumber)) && sortDirection.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.desc)))
            {
                testimonials = testimonialList.OrderByDescending(x => x.UserMobileNumber).ToList();
            }
            else
            {
                testimonials = testimonialList.OrderBy(x => x.TestimonialId).ToList();
            }

            return testimonials;
        }
    }
}
