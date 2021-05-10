namespace SHKang.API.Areas.Admin.Controllers
{

    #region namespace

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
    public class ColorController : ControllerBase
    {
        #region Private Variables

        /// <summary>
        /// The i size
        /// </summary>
        private readonly IColor iColor;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorController"/> class.
        /// </summary>
        /// <param name="_iColor">Color of the i.</param>
        public ColorController(IColor _iColor)
        {
            iColor = _iColor;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the color.
        /// </summary>
        /// <param name="addColorModel">The color model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("add-color")]
        public IActionResult AddColor(AddColorModel addColorModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Color colorModel = ColorHelper.BindColorModel(addColorModel);
                    if (!string.IsNullOrWhiteSpace(addColorModel.ColorId) && DBHelper.ParseInt64(addColorModel.ColorId) <= 0)
                    {
                        long colorId = iColor.AddColor(colorModel);
                        if (colorId > 0)
                        {
                            return Ok(ResponseHelper.Success(MessageConstants.ColorAdded));
                        }
                        else if (colorId == ReturnCode.AlreadyExist.GetHashCode())
                        {
                            return Ok(ResponseHelper.Error(MessageConstants.ColorExists));
                        }
                        else
                        {
                            return Ok(ResponseHelper.Error(MessageConstants.ColorNotAdded));
                        }
                    }
                    else
                    {
                        long colorId = iColor.UpdateColor(colorModel);
                        if (colorId > 0)
                        {
                            return Ok(ResponseHelper.Success(MessageConstants.ColorUpdated));
                        }
                        else if (colorId == ReturnCode.AlreadyExist.GetHashCode())
                        {
                            return Ok(ResponseHelper.Error(MessageConstants.ColorExists));
                        }
                        else
                        {
                            return Ok(ResponseHelper.Error(MessageConstants.ColorNotUpdated));
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
        /// Deletes the color.
        /// </summary>
        /// <param name="colorId">The color identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete-color")]
        public IActionResult DeleteColor(DeleteColorModel deleteModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isDeleted = iColor.DeleteColor(DBHelper.ParseInt64(deleteModel.colorId));
                    if (isDeleted)
                    {
                        return Ok(ResponseHelper.Success(MessageConstants.ColorDeleted));
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
        /// Updates the sizecolor.
        /// </summary>
        /// <param name="addColorModel">The color model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("update-color")]
        public IActionResult Updatecolor(AddColorModel addColorModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Color colorModel = ColorHelper.BindColorModel(addColorModel);
                    long colorId = iColor.UpdateColor(colorModel);
                    if (colorId > 0)
                    {
                        return Ok(ResponseHelper.Success(MessageConstants.ColorUpdated));
                    }
                    else if (colorId == ReturnCode.AlreadyExist.GetHashCode())
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.ColorExists));
                    }
                    else
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.ColorNotUpdated));
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
        /// Colors the list.
        /// </summary>
        /// <param name="pageNo">The page no.</param>
        /// <param name="limit">The limit.</param>
        /// <param name="searchString">The search string.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("color-list")]
        public IActionResult ColorList(SearchPaginationListModel searchModel)
        {
            try
            {
                if (searchModel.pageNo <= 0)
                {
                    searchModel.pageNo = 1;
                }
                var colorList = iColor.GetColorList(searchModel.searchString);
                if (colorList != null)
                {
                    List<Color> sizePagedresult = new List<Color>();
                    #region Sorting
                    if (searchModel.column.ToLower().Equals(DBHelper.ParseString(SortingColorColumnName.colorid)) && searchModel.direction.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.asc)))
                    {
                        sizePagedresult = colorList.OrderBy(x => x.ColorId).ToPagedList(searchModel.pageNo, searchModel.limit).ToList();
                    }
                    else if (searchModel.column.ToLower().Equals(DBHelper.ParseString(SortingColorColumnName.colorid)) && searchModel.direction.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.desc)))
                    {
                        sizePagedresult = colorList.OrderByDescending(x => x.ColorId).ToPagedList(searchModel.pageNo, searchModel.limit).ToList();
                    }
                    else if (searchModel.column.ToLower().Equals(DBHelper.ParseString(SortingColorColumnName.name)) && searchModel.direction.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.asc)))
                    {
                        sizePagedresult = colorList.OrderBy(x => x.Name).ToPagedList(searchModel.pageNo, searchModel.limit).ToList();
                    }
                    else if (searchModel.column.ToLower().Equals(DBHelper.ParseString(SortingColorColumnName.name)) && searchModel.direction.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.desc)))
                    {
                        sizePagedresult = colorList.OrderByDescending(x => x.Name).ToPagedList(searchModel.pageNo, searchModel.limit).ToList();
                    }
                    else
                    {
                        sizePagedresult = colorList.OrderBy(x => x.ColorId).ToPagedList(searchModel.pageNo, searchModel.limit).ToList();
                    }
                    #endregion
                    var colorId = ColorHelper.GetColorId(sizePagedresult);
                    var productColor = iColor.GetProductListByColorIds(colorId);
                    ColorListModel colorListModel = new ColorListModel();
                    colorListModel.Total = DBHelper.ParseString(colorList.Count);
                    colorListModel.Items = ColorHelper.BindColorListModel(sizePagedresult, productColor);
                    return Ok(ResponseHelper.Success(colorListModel));
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