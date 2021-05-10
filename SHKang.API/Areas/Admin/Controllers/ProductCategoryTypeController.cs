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
    public class ProductCategoryTypeController : ControllerBase
    {
        #region Private Variables

        /// <summary>
        /// The i product category type
        /// </summary>
        private readonly IProductCategoryType iProductCategoryType;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCategoryTypeController"/> class.
        /// </summary>
        /// <param name="_iProductCategoryType">Type of the i product category.</param>
        public ProductCategoryTypeController(IProductCategoryType _iProductCategoryType)
        {
            iProductCategoryType = _iProductCategoryType;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the type of the product category.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("add-product-category-type")]
        public IActionResult AddProductCategoryType(UpdateProductCategoryTypeModel addProductCategoryType)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (addProductCategoryType.ProductCategoryTypeId <= 0)
                    {
                        ProductCategoryType productCategoryTypeModel = ProductCategoryTypeHelper.BindProductCategoryModel(addProductCategoryType);
                        long productCategoryTypeId = iProductCategoryType.AddNewProductCategoryType(productCategoryTypeModel);
                        if (productCategoryTypeId > 0)
                        {
                            return Ok(ResponseHelper.Success(MessageConstants.ProductCategoryTypeAdded));
                        }
                        else if (productCategoryTypeId == ReturnCode.AlreadyExist.GetHashCode())
                        {
                            return Ok(ResponseHelper.Error(MessageConstants.ProductCategoryTypeExists));
                        }
                        else
                        {
                            return Ok(ResponseHelper.Error(MessageConstants.ProductCategoryTypeNotAdded));
                        }
                    }
                    else
                    {
                        ProductCategoryType productCategoryTypeModel = ProductCategoryTypeHelper.BindProductCategoryModel(addProductCategoryType, true);
                        long productCategoryTypeId = iProductCategoryType.UpdateProductCategoryType(productCategoryTypeModel, DBHelper.ParseInt64(productCategoryTypeModel.UpdatedBy));
                        if (productCategoryTypeId > 0)
                        {
                            return Ok(ResponseHelper.Success(MessageConstants.ProductCategoryTypeUpdated));
                        }
                        else if (productCategoryTypeId == ReturnCode.AlreadyExist.GetHashCode())
                        {
                            return Ok(ResponseHelper.Error(MessageConstants.ProductCategoryTypeExists));
                        }
                        else
                        {
                            return Ok(ResponseHelper.Error(MessageConstants.ProductCategoryTypeNotUpdated));
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
        /// Deletes the type of the product category.
        /// </summary>
        /// <param name="ProductCategoryTypeId">The product category type identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete-product-category-type")]
        public IActionResult DeleteProductCategoryType(DeleteProductCategoryTypeModel deleteModel)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(deleteModel.productCategoryTypeId) && !string.IsNullOrWhiteSpace(deleteModel.updatedBy))
                {
                    bool isDeleted = iProductCategoryType.DeleteProductCategoryType(DBHelper.ParseInt64(deleteModel.productCategoryTypeId), DBHelper.ParseInt64(deleteModel.updatedBy));
                    if (isDeleted)
                    {
                        return Ok(ResponseHelper.Success(MessageConstants.ProductCategoryTypeDeleted));
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
        /// Updates the type of the product category.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("update-product-category-type")]
        public IActionResult UpdateProductCategoryType(UpdateProductCategoryTypeModel updateProductCategoryTypeModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ProductCategoryType productCategoryTypeModel = ProductCategoryTypeHelper.BindProductCategoryModel(updateProductCategoryTypeModel);
                    long productCategoryTypeId = iProductCategoryType.UpdateProductCategoryType(productCategoryTypeModel, DBHelper.ParseInt64(productCategoryTypeModel.UpdatedBy));
                    if (productCategoryTypeId > 0)
                    {
                        return Ok(ResponseHelper.Success(MessageConstants.ProductCategoryTypeUpdated));
                    }
                    else if (productCategoryTypeId == ReturnCode.AlreadyExist.GetHashCode())
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.TryDifferentName));
                    }
                    else
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.ProductCategoryTypeNotUpdated));
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
        /// Products the category list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("product-category-type-list")]
        public IActionResult ProductCategoryTypeList(SearchPaginationListModel searchModel)
        {
            try
            {
                if (searchModel.pageNo <= 0)
                {
                    searchModel.pageNo = 1;
                }
                var productCategoryTypeList = iProductCategoryType.ProductCategoryType(searchModel.searchString);
                if (productCategoryTypeList != null)
                {
                    List<ProductCategoryType> productCategoryTypePagedResult = new List<ProductCategoryType>();

                    #region Sorting
                    if (searchModel.column.ToLower().Equals(DBHelper.ParseString(SortingProductCategoryTypeColumnName.productcategorytypeid)) && searchModel.direction.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.asc)))
                    {
                        productCategoryTypePagedResult = productCategoryTypeList.OrderBy(x => x.ProductCategoryTypeId).ToPagedList(searchModel.pageNo, searchModel.limit).ToList();
                    }
                    else if (searchModel.column.ToLower().Equals(DBHelper.ParseString(SortingProductCategoryTypeColumnName.productcategorytypeid)) && searchModel.direction.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.desc)))
                    {
                        productCategoryTypePagedResult = productCategoryTypeList.OrderByDescending(x => x.ProductCategoryTypeId).ToPagedList(searchModel.pageNo, searchModel.limit).ToList();
                    }
                    else if (searchModel.column.ToLower().Equals(DBHelper.ParseString(SortingProductCategoryTypeColumnName.name)) && searchModel.direction.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.asc)))
                    {
                        productCategoryTypePagedResult = productCategoryTypeList.OrderBy(x => x.CategoryType).ToPagedList(searchModel.pageNo, searchModel.limit).ToList();
                    }
                    else if (searchModel.column.ToLower().Equals(DBHelper.ParseString(SortingProductCategoryTypeColumnName.name)) && searchModel.direction.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.desc)))
                    {
                        productCategoryTypePagedResult = productCategoryTypeList.OrderByDescending(x => x.CategoryType).ToPagedList(searchModel.pageNo, searchModel.limit).ToList();
                    }
                    else
                    {
                        productCategoryTypePagedResult = productCategoryTypeList.OrderBy(x => x.ProductCategoryTypeId).ToPagedList(searchModel.pageNo, searchModel.limit).ToList();
                    }
                    #endregion

                    var productCategoryTypeId = ProductCategoryTypeHelper.GetProductCategoryTypeId(productCategoryTypePagedResult);
                    var productList = iProductCategoryType.GetProductCategoryDetails(productCategoryTypeId);
                    ProductCategoryTypeListModel productCategoryTypeListModel = new ProductCategoryTypeListModel();
                    productCategoryTypeListModel.Items = ProductCategoryTypeHelper.BindProductCategoryTypeListModel(productCategoryTypePagedResult, productList);
                    //productCategoryTypeListModel.Total = DBHelper.ParseString(iProductCategoryType.GetTotalProductCategoryTypeCount());
                    productCategoryTypeListModel.Total =DBHelper.ParseString(productCategoryTypeList.Count);
                    return Ok(ResponseHelper.Success(productCategoryTypeListModel));
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
        /// Products the category detail.
        /// </summary>
        /// <param name="ProductCategoryId">The product category identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("product-category-type-detail")]
        public IActionResult ProductCategoryTypeDetail(ProductCategoryTypeDetailModel productCategoryTypeModel)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(productCategoryTypeModel.productCategoryTypeId))
                {
                    var productCategoryTypeDetail = iProductCategoryType.ProductCategoryTypeDetail(DBHelper.ParseInt64(productCategoryTypeModel.productCategoryTypeId));
                    if (productCategoryTypeDetail != null)
                    {
                        ProductCategoryTypeDataModel productCategoryTypeDataModel = ProductCategoryTypeHelper.BindProductCategoryTypeListModel(productCategoryTypeDetail);
                        return Ok(ResponseHelper.Success(productCategoryTypeDataModel));
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