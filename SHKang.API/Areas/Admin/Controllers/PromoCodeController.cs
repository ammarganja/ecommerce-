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
    public class PromoCodeController : ControllerBase
    {
        #region Private Variables

        /// <summary>
        /// The i promo code
        /// </summary>
        private readonly IPromoCode iPromoCode;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PromoCodeController"/> class.
        /// </summary>
        /// <param name="_iPromoCode">The i promo code.</param>
        public PromoCodeController(IPromoCode _iPromoCode)
        {
            iPromoCode = _iPromoCode;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the promo code.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("add-promo-code")]
        public IActionResult AddPromoCode(AddPromoCodeModel addPromoCodeModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    PromoCode promoCodeModel = PromoCodeHelper.BindPromoCode(addPromoCodeModel);
                    if (!string.IsNullOrWhiteSpace(addPromoCodeModel.PromoCodeId) && DBHelper.ParseInt64(addPromoCodeModel.PromoCodeId) <= 0)
                    {
                        promoCodeModel.CreatedOn = DateTime.Now;
                        long promoCodeId = iPromoCode.AddPromocode(promoCodeModel);
                        if (promoCodeId > 0)
                        {
                            return Ok(ResponseHelper.Success(MessageConstants.PromocodeAdded));
                        }
                        else if (promoCodeId == ReturnCode.AlreadyExist.GetHashCode())
                        {
                            return Ok(ResponseHelper.Error(MessageConstants.TryDifferentCode));
                        }
                        else
                        {
                            return Ok(ResponseHelper.Error(MessageConstants.PromocodeNotAdded));
                        }
                    }
                    else
                    {
                        promoCodeModel.ModifiedOn = DateTime.Now;
                        long promoCodeId = iPromoCode.UpdatePromocode(promoCodeModel);
                        if (promoCodeId > 0)
                        {
                            return Ok(ResponseHelper.Success(MessageConstants.PromocodeUpdated));
                        }
                        else if (promoCodeId == ReturnCode.AlreadyExist.GetHashCode())
                        {
                            return Ok(ResponseHelper.Error(MessageConstants.TryDifferentCode));
                        }
                        else
                        {
                            return Ok(ResponseHelper.Error(MessageConstants.PromocodeNotUpdated));
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
        /// Deletes the promo code.
        /// </summary>
        /// <param name="PromoCodeId">The promo code identifier.</param>
        /// <param name="UpdatedBy">The updated by.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete-promo-code")]
        public IActionResult DeletePromoCode(DeletePromocodeModel deleteModel)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(deleteModel.promoCodeId) && !string.IsNullOrWhiteSpace(deleteModel.updatedBy))
                {
                    bool isDeleted = iPromoCode.DeletePromocode(DBHelper.ParseInt64(deleteModel.promoCodeId), DBHelper.ParseInt64(deleteModel.updatedBy));
                    if (isDeleted)
                    {
                        return Ok(ResponseHelper.Success(MessageConstants.PromocodeDeleted));
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
        /// Updates the product category.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("update-promo-code")]
        public IActionResult UpdatePromoCode(AddPromoCodeModel addPromoCodeModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    PromoCode promoCodeModel = PromoCodeHelper.BindPromoCode(addPromoCodeModel);
                    long promoCodeId = iPromoCode.UpdatePromocode(promoCodeModel);
                    if (promoCodeId > 0)
                    {
                        return Ok(ResponseHelper.Success(MessageConstants.PromocodeUpdated));
                    }
                    else if (promoCodeId == ReturnCode.AlreadyExist.GetHashCode())
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.TryDifferentCode));
                    }
                    else
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.PromocodeNotUpdated));
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
        /// Promocodes the list.
        /// </summary>
        /// <param name="PageNo">The page no.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("promo-code-list")]
        public IActionResult PromocodeList(SearchPaginationListModel searchModel)
        {
            try
            {
                if (searchModel.pageNo <= 0)
                {
                    searchModel.pageNo = 1;
                }
                var promoCodeList = iPromoCode.GetPromocodeList(searchModel.searchString);
                if (promoCodeList != null)
                {
                    PromoCodeListModel promoCodeListModel = new PromoCodeListModel();
                    List<PromoCode> promoCodePagedresult = new List<PromoCode>();
                    promoCodePagedresult = promoCodeList.OrderByDescending(x => x.PromoCodeId).ToPagedList(searchModel.pageNo, searchModel.limit).ToList();

                    #region Sorting
                    if (searchModel.column.ToLower().Equals(DBHelper.ParseString(SortingPromocodeColumnName.promocodeid)) && searchModel.direction.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.asc)))
                    {
                        promoCodePagedresult = promoCodeList.OrderBy(x => x.PromoCodeId).ToPagedList(searchModel.pageNo, searchModel.limit).ToList();
                    }
                    else if (searchModel.column.ToLower().Equals(DBHelper.ParseString(SortingPromocodeColumnName.promocodeid)) && searchModel.direction.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.desc)))
                    {
                        promoCodePagedresult = promoCodeList.OrderByDescending(x => x.PromoCodeId).ToPagedList(searchModel.pageNo, searchModel.limit).ToList();
                    }
                    else if (searchModel.column.ToLower().Equals(DBHelper.ParseString(SortingPromocodeColumnName.name)) && searchModel.direction.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.asc)))
                    {
                        promoCodePagedresult = promoCodeList.OrderBy(x => x.Name).ToPagedList(searchModel.pageNo, searchModel.limit).ToList();
                    }
                    else if (searchModel.column.ToLower().Equals(DBHelper.ParseString(SortingPromocodeColumnName.name)) && searchModel.direction.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.desc)))
                    {
                        promoCodePagedresult = promoCodeList.OrderByDescending(x => x.Name).ToPagedList(searchModel.pageNo, searchModel.limit).ToList();
                    }
                    else if (searchModel.column.ToLower().Equals(DBHelper.ParseString(SortingPromocodeColumnName.code)) && searchModel.direction.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.asc)))
                    {
                        promoCodePagedresult = promoCodeList.OrderBy(x => x.Code).ToPagedList(searchModel.pageNo, searchModel.limit).ToList();
                    }
                    else if (searchModel.column.ToLower().Equals(DBHelper.ParseString(SortingPromocodeColumnName.code)) && searchModel.direction.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.desc)))
                    {
                        promoCodePagedresult = promoCodeList.OrderByDescending(x => x.Code).ToPagedList(searchModel.pageNo, searchModel.limit).ToList();
                    }
                    else if (searchModel.column.ToLower().Equals(DBHelper.ParseString(SortingPromocodeColumnName.startdate)) && searchModel.direction.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.asc)))
                    {
                        promoCodePagedresult = promoCodeList.OrderBy(x => x.StartDate).ToPagedList(searchModel.pageNo, searchModel.limit).ToList();
                    }
                    else if (searchModel.column.ToLower().Equals(DBHelper.ParseString(SortingPromocodeColumnName.startdate)) && searchModel.direction.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.desc)))
                    {
                        promoCodePagedresult = promoCodeList.OrderByDescending(x => x.StartDate).ToPagedList(searchModel.pageNo, searchModel.limit).ToList();
                    }
                    else if (searchModel.column.ToLower().Equals(DBHelper.ParseString(SortingPromocodeColumnName.expirydate)) && searchModel.direction.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.asc)))
                    {
                        promoCodePagedresult = promoCodeList.OrderBy(x => x.ExpiryDate).ToPagedList(searchModel.pageNo, searchModel.limit).ToList();
                    }
                    else if (searchModel.column.ToLower().Equals(DBHelper.ParseString(SortingPromocodeColumnName.expirydate)) && searchModel.direction.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.desc)))
                    {
                        promoCodePagedresult = promoCodeList.OrderByDescending(x => x.ExpiryDate).ToPagedList(searchModel.pageNo, searchModel.limit).ToList();
                    }
                    else if (searchModel.column.ToLower().Equals(DBHelper.ParseString(SortingPromocodeColumnName.discount)) && searchModel.direction.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.asc)))
                    {
                        promoCodePagedresult = promoCodeList.OrderBy(x => x.Amount).ThenBy(x => x.Percentage).ToPagedList(searchModel.pageNo, searchModel.limit).ToList();
                    }
                    else if (searchModel.column.ToLower().Equals(DBHelper.ParseString(SortingPromocodeColumnName.discount)) && searchModel.direction.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.desc)))
                    {
                        promoCodePagedresult = promoCodeList.OrderByDescending(x => x.Amount).ThenByDescending(x => x.Percentage).ToPagedList(searchModel.pageNo, searchModel.limit).ToList();
                    }
                    else
                    {
                        promoCodePagedresult = promoCodeList.OrderBy(x => x.PromoCodeId).ToPagedList(searchModel.pageNo, searchModel.limit).ToList();
                    }
                    #endregion

                    List<PromoCodeDataListModel> promoCodeDataListModel = PromoCodeHelper.BindPromoCodeListModel(promoCodePagedresult);
                    //promoCodeListModel.Total =DBHelper.ParseString(iPromoCode.GetTotalPromocodeCount());
                    promoCodeListModel.Total = DBHelper.ParseString(promoCodeList.Count);
                    promoCodeListModel.Items = promoCodeDataListModel;
                    return Ok(ResponseHelper.Success(promoCodeListModel));
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

        /// <summary>
        /// Promocodes the detail.
        /// </summary>
        /// <param name="PromocodeId">The promocode identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("promo-code-detail")]
        public IActionResult PromocodeDetail(GetPromoCodeDetailModel promoCodeDetailModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var promocodeDetail = iPromoCode.GetPromocodeDetail(DBHelper.ParseInt64(promoCodeDetailModel.promoCodeId));
                    if (promocodeDetail != null)
                    {
                        PromoCodeDataListModel promoCodeListModel = PromoCodeHelper.BindPromoCodeListModel(promocodeDetail);
                        return Ok(ResponseHelper.Success(promoCodeListModel));
                    }
                    else
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.DataNotFound));
                    }
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

        /// <summary>
        /// Changes the promo code status.
        /// </summary>
        /// <param name="promoCodeDetailModel">The promo code detail model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("change-promo-code-status")]
        public IActionResult ChangePromoCodeStatus(ChangePromoCodeStatusModel promoCodeDetailModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var promocodeDetail = iPromoCode.ChangePromocodeStatus(DBHelper.ParseInt64(promoCodeDetailModel.promoCodeId), DBHelper.ParseInt32(promoCodeDetailModel.Status));
                    if (promocodeDetail)
                    {
                        return Ok(ResponseHelper.Success(MessageConstants.PromocodeStatusUpdated));
                    }
                    else
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.PromocodeStatusNotUpdated));
                    }
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