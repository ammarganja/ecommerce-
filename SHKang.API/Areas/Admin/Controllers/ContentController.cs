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
    public class ContentController : ControllerBase
    {
        #region Private Variables

        /// <summary>
        /// The i content
        /// </summary>
        private readonly IContent iContent;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentController"/> class.
        /// </summary>
        /// <param name="_icontent">The icontent.</param>
        public ContentController(IContent _icontent)
        {
            iContent = _icontent;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Adds the content.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("add-content")]
        public IActionResult AddContent(AddContentModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ContentDetails contentModel = ContentHelper.BindContentDetailsModel(model);
                    long contentId = iContent.AddContent(contentModel);
                    if (contentId > 0)
                    {
                        return Ok(ResponseHelper.Success(MessageConstants.ContentAdded));
                    }
                    else if (contentId == ReturnCode.AlreadyExist.GetHashCode())
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.TryDifferentNameShortName));
                    }
                    else
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.ContentNotAdded));
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
        /// Deletes the content.
        /// </summary>
        /// <param name="ContentId">The content identifier.</param>
        /// <param name="UpdatedBy">The updated by.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete-content")]
        public IActionResult DeleteContent(DeleteContentModel deleteModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isDeleted = iContent.DeleteContent(DBHelper.ParseInt64(deleteModel.contentId), DBHelper.ParseInt64(deleteModel.updatedBy));
                    if (isDeleted)
                    {
                        return Ok(ResponseHelper.Success(MessageConstants.ContentDeleted));
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
        /// Updates the content.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("update-content")]
        public IActionResult UpdateContent(AddContentModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ContentDetails contentModel = ContentHelper.BindContentDetailsModel(model);
                    long contentId = iContent.UpdateContent(contentModel);
                    if (contentId > 0)
                    {
                        return Ok(ResponseHelper.Success(MessageConstants.ContentUpdated));
                    }
                    else if (contentId == ReturnCode.AlreadyExist.GetHashCode())
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.TryDifferentNameShortName));
                    }
                    else
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.ContentNotUpdated));
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
        /// Contents the list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("content-list")]
        public IActionResult ContentList(SearchPaginationListModel searchModel)
        {
            try
            {
                if (searchModel.pageNo <= 0)
                {
                    searchModel.pageNo = 1;
                }
                var contentList = iContent.ContentDetails(searchModel.searchString);
                if (contentList != null)
                {
                    List<ContentDetails> contentDetailsResult = new List<ContentDetails>();

                    #region Sorting
                    if (searchModel.column.ToLower().Equals(DBHelper.ParseString(SortContentType.contentid)) && searchModel.direction.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.asc)))
                    {
                        contentDetailsResult = contentList.OrderBy(x => x.ContentId).ToPagedList(searchModel.pageNo, searchModel.limit).ToList();
                    }
                    else if (searchModel.column.ToLower().Equals(DBHelper.ParseString(SortContentType.name)) && searchModel.direction.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.desc)))
                    {
                        contentDetailsResult = contentList.OrderByDescending(x => x.Name).ToPagedList(searchModel.pageNo, searchModel.limit).ToList();
                    }
                    else if (searchModel.column.ToLower().Equals(DBHelper.ParseString(SortContentType.contentid)) && searchModel.direction.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.desc)))
                    {
                        contentDetailsResult = contentList.OrderByDescending(x => x.ContentId).ToPagedList(searchModel.pageNo, searchModel.limit).ToList();
                    }
                    else if (searchModel.column.ToLower().Equals(DBHelper.ParseString(SortContentType.name)) && searchModel.direction.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.asc)))
                    {
                        contentDetailsResult = contentList.OrderBy(x => x.Name).ToPagedList(searchModel.pageNo, searchModel.limit).ToList();
                    }
                    else
                    {
                        contentDetailsResult = contentList.OrderBy(x => x.ContentId).ToPagedList(searchModel.pageNo, searchModel.limit).ToList();
                    }
                    #endregion

                    ////var contentPagedResult = contentList.OrderByDescending(x => x.ContentId).ToPagedList(searchModel.pageNo, searchModel.limit).ToList();
                    List<AddContentModel> addContentModelList = ContentHelper.BindAddContentModel(contentDetailsResult);
                    PagedListModel<ContentDetails> pagedListModel = new PagedListModel<ContentDetails>();
                    pagedListModel.Items = contentDetailsResult.ToList();
                    pagedListModel.Total = DBHelper.ParseInt32(contentList.Count);
                    return Ok(ResponseHelper.Success(pagedListModel));
                }
                else
                {
                    return Ok(ResponseHelper.Error(MessageConstants.ContentNotFound));
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