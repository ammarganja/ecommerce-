namespace SHKang.API.Areas.Admin.Controllers
{
    #region namespaces

    using Microsoft.AspNetCore.Mvc;
    using SHKang.API.Helper;
    using SHKang.Core;
    using SHKang.Core.Constant;
    using SHKang.Core.Enums;
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
    public class SizeController : ControllerBase
    {
        #region Private Variables

        /// <summary>
        /// The i size
        /// </summary>
        private readonly ISize iSize;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SizeController"/> class.
        /// </summary>
        /// <param name="_iSize">Size of the i.</param>
        public SizeController(ISize _iSize)
        {
            iSize = _iSize;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the size.
        /// </summary>
        /// <param name="Name">The name.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("add-size")]
        public IActionResult AddSize(AddSizeModel addSizeModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var sizeModel = SizeHelper.BindSizeModel(addSizeModel);
                    var sizeDetailModel = SizeHelper.BindSizeDetailModel(addSizeModel);
                    if (sizeModel.SizeId <= 0)
                    {
                        long sizeId = iSize.AddSize(sizeModel, sizeDetailModel);
                        if (sizeId > 0)
                        {
                            return Ok(ResponseHelper.Success(MessageConstants.SizeAdded));
                        }
                        else if (sizeId == ReturnCode.AlreadyExist.GetHashCode())
                        {
                            return Ok(ResponseHelper.Error(MessageConstants.SizeExists));
                        }
                        else
                        {
                            return Ok(ResponseHelper.Error(MessageConstants.SizeNotAdded));
                        }
                    }
                    else
                    {
                        long sizeId = iSize.UpdateSize(sizeModel,sizeDetailModel);
                        if (sizeId > 0)
                        {
                            return Ok(ResponseHelper.Success(MessageConstants.SizeUpdated));
                        }
                        else if (sizeId == ReturnCode.AlreadyExist.GetHashCode())
                        {
                            return Ok(ResponseHelper.Error(MessageConstants.TryDifferentName));
                        }
                        else
                        {
                            return Ok(ResponseHelper.Error(MessageConstants.SizeNotUpdated));
                        }
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
        /// Deletes the size.
        /// </summary>
        /// <param name="SizeId">The size identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete-size")]
        public IActionResult DeleteSize(DeleteSizeModel deleteModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isDeleted = iSize.DeleteSize(DBHelper.ParseInt64(deleteModel.sizeId));
                    if (isDeleted)
                    {
                        return Ok(ResponseHelper.Success(MessageConstants.SizeDeleted));
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
        /// Sizes the list.
        /// </summary>
        /// <param name="pageNo">The page no.</param>
        /// <param name="limit">The limit.</param>
        /// <param name="searchString">The search string.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("size-list")]
        public IActionResult SizeList(SearchPaginationListModel searchModel)
        {
            try
            {
                if (searchModel.pageNo <= 0)
                {
                    searchModel.pageNo = 1;
                }
                var sizeList = iSize.GetSizeList(searchModel.searchString);
                if (sizeList != null)
                {
                    List<SizeListModel> sizeResult = new List<SizeListModel>();
                    sizeResult = SizeHelper.BindSizeListModel(sizeList);
                    var sizeIds = SizeHelper.GetSizeIds(sizeResult);
                    var productList = iSize.GetTotalProductSizeCount(sizeIds);
                    sizeResult = SizeHelper.BindSizeListModel(sizeResult, productList);

                    #region Sorting
                    var sizeResult1 = SortingHelper.GetSortedSizes(searchModel.column, searchModel.direction, sizeResult).ToPagedList(searchModel.pageNo, searchModel.limit).ToList();
                    #endregion

                    PagedListModel<SizeListModel> pagedListModel = new PagedListModel<SizeListModel>();
                    pagedListModel.Items = sizeResult1.ToList();
                    pagedListModel.Total = sizeResult.Select(x=>x.SizeId).Distinct().Count();
                    return Ok(ResponseHelper.Success(pagedListModel));
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