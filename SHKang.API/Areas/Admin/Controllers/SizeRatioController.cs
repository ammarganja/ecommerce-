namespace SHKang.API.Areas.Admin.Controllers
{
    #region namespaces

    using Microsoft.AspNetCore.Mvc;
    using SHKang.API.Helper;
    using SHKang.Core;
    using SHKang.Core.Constant;
    using SHKang.Core.Enums;
    using SHKang.Core.Helpers;
    using SHKang.Model.Models;
    using SHKang.Repository.Interface;
    using System;
    using System.Linq;
    using X.PagedList; 
    #endregion

    [Route("api/admin/[controller]")]
    [ApiController]
    [AuthorizeHelper]
    public class SizeRatioController : ControllerBase
    {
        #region Private Variables

        /// <summary>
        /// The i size ratio
        /// </summary>
        private readonly ISizeRatio iSizeRatio;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SizeRatioController"/> class.
        /// </summary>
        /// <param name="_iSizeRatio">The i size ratio.</param>
        public SizeRatioController(ISizeRatio _iSizeRatio)
        {
            iSizeRatio = _iSizeRatio;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the size ratio.
        /// </summary>
        /// <param name="sizeRatioModel">The size ratio model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("add-size-ratio")]
        public IActionResult AddSizeRatio(SizeRatio sizeRatioModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    long sizeRatioId = iSizeRatio.AddSizeRatio(sizeRatioModel);
                    if (sizeRatioId > 0)
                    {
                        return Ok(ResponseHelper.Success(MessageConstants.SizeRatioAdded));
                    }
                    else if (sizeRatioId == ReturnCode.AlreadyExist.GetHashCode())
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.TryDifferentName));
                    }
                    else
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.SizeRatioNotAdded));
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

        /// <summary>
        /// Deletes the size ratio.
        /// </summary>
        /// <param name="sizeRatioId">The size ratio identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete-size-ratio")]
        public IActionResult DeleteSizeRatio(string sizeRatioId)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(sizeRatioId))
                {
                    bool isDeleted = iSizeRatio.DeleteSizeRatio(DBHelper.ParseInt64(sizeRatioId));
                    if (isDeleted)
                    {
                        return Ok(ResponseHelper.Success(MessageConstants.SizeRatioDeleted));
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

        /// <summary>
        /// Updates the size ratio.
        /// </summary>
        /// <param name="sizeRatioModel">The size ratio model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("update-size-ratio")]
        public IActionResult UpdateSizeRatio(SizeRatio sizeRatioModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    long sizeRatioId = iSizeRatio.UpdateSizeRatio(sizeRatioModel);
                    if (sizeRatioId > 0)
                    {
                        return Ok(ResponseHelper.Success(MessageConstants.SizeRatioUpdated));
                    }
                    else if (sizeRatioId == ReturnCode.AlreadyExist.GetHashCode())
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.TryDifferentName));
                    }
                    else
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.SizeRatioNotUpdated));
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

        /// <summary>
        /// Sizes the ratio list.
        /// </summary>
        /// <param name="pageNo">The page no.</param>
        /// <param name="limit">The limit.</param>
        /// <param name="searchString">The search string.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("size-ratio-list")]
        public IActionResult SizeRatioList(int pageNo,int limit,string searchString)
        {
            try
            {
                if (pageNo <= 0)
                {
                    pageNo = 1;
                }
                var sizeRatioList = iSizeRatio.GetSizeRatioList(searchString);
                if (sizeRatioList != null)
                {
                    var sizeRatiopagedresult = sizeRatioList.OrderByDescending(x => x.SizeRatioId).ToPagedList(pageNo, limit).ToList();
                    return Ok(ResponseHelper.Success(sizeRatiopagedresult));
                }
                else
                {
                    return Ok(ResponseHelper.Error(MessageConstants.DataNotFound));
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