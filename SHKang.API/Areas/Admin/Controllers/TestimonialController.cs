namespace SHKang.API.Areas.Admin.Controllers
{
    #region namespaces

    using Microsoft.AspNetCore.Mvc;
    using SHKang.API.Helper;
    using SHKang.Core;
    using SHKang.Core.Constant;
    using SHKang.Core.Helpers;
    using SHKang.Core.ModelHelper;
    using SHKang.Model.Models;
    using SHKang.Model.ViewModels.Admin;
    using SHKang.Repository.Interface;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using X.PagedList;

    #endregion

    [Route("api/admin/[controller]")]
    [ApiController]
    [AuthorizeHelper]
    public class TestimonialController : ControllerBase
    {
        #region Private Variables

        /// <summary>
        /// The i testimonial
        /// </summary>
        private readonly ITestimonial iTestimonial;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TestimonialController"/> class.
        /// </summary>
        /// <param name="_iTestimonial">The i testimonial.</param>
        public TestimonialController(ITestimonial _iTestimonial)
        {
            iTestimonial = _iTestimonial;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Deletes the testimonial.
        /// </summary>
        /// <param name="deleteModel">The delete model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete-testimonial")]
        public IActionResult DeleteTestimonial(DeleteTestimonialModel deleteModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isDeleted = iTestimonial.DeleteTestimonial(DBHelper.ParseInt64(deleteModel.TestimonialId));
                    if (isDeleted)
                    {
                        return Ok(ResponseHelper.Success(MessageConstants.TestimonialDeleted));
                    }
                    else
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.DataNotFound));
                    }

                }
                else
                {
                    return Ok(ResponseHelper.Error(MessageConstants.CompulsoryData));
                }
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                return Ok(ResponseHelper.Error(ex.Message));
            }
        }

        #endregion

    }
}