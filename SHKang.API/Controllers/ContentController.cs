namespace SHKang.API.Controllers
{

    #region namespaces
    using Microsoft.AspNetCore.Mvc;
    using SHKang.Core;
    using SHKang.Core.Constant;
    using SHKang.Core.Helpers;
    using SHKang.Core.ModelHelper;
    using SHKang.Model.ViewModels.Admin;
    using SHKang.Model.ViewModels.Web;
    using SHKang.Repository.Interface;
    using System;
    #endregion


    [Route("api/[controller]")]
    [ApiController]
    public class ContentController : ControllerBase
    {

        #region Private Variables

        /// <summary>
        /// The i content
        /// </summary>
        private readonly IContent iContent; 
        #endregion

        #region Constructor

        public ContentController(IContent _iContent)
        {
            iContent = _iContent;
        } 
        #endregion

        #region Public Methods

        /// <summary>
        /// Contents the details.
        /// </summary>
        /// <param name="Shortname">The shortname.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("content-detail")]
        public IActionResult ContentDetails(ContentDetailModel contentModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var contentDetailModel = iContent.GetContentDetail(contentModel.shortName);
                    if (contentDetailModel != null)
                    {
                        AddContentModel addContentModel = ContentHelper.BindContentDetailsModel(contentDetailModel);
                        return Ok(ResponseHelper.Success(addContentModel));
                    }
                    else
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.ContentNotFound));
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