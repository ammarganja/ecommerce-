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
    public class ProductCategoryController : ControllerBase
    {

        #region Private Variables

        /// <summary>
        /// The i product category
        /// </summary>
        private readonly IProductCategory iProductCategory;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCategoryController"/> class.
        /// </summary>
        /// <param name="_iProductCategory">The i product category.</param>
        public ProductCategoryController(IProductCategory _iProductCategory)
        {
            iProductCategory = _iProductCategory;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the product category.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("add-product-category")]
        public IActionResult AddProductCategory(AddNewProductCategoryModel addproductCategorymodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ProductCategory productCategoryModel = ProductCategoryHelper.BindProductCategoryModel(addproductCategorymodel);
                    long productCategoryId = iProductCategory.AddProductCategory(productCategoryModel);
                    if (productCategoryId > 0)
                    {
                        return Ok(ResponseHelper.Success(MessageConstants.ProductCategoryAdded));
                    }
                    else if (productCategoryId == ReturnCode.AlreadyExist.GetHashCode())
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.TryDifferentName));
                    }
                    else
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.ProductCategoryNotAdded));
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
        /// Deletes the product category.
        /// </summary>
        /// <param name="ProductCategoryId">The product category identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete-product-category")]
        public IActionResult DeleteProductCategory(DeleteProductCategoryModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isDeleted = iProductCategory.DeleteProductCategory(DBHelper.ParseInt64(model.productCategoryId), DBHelper.ParseInt64(model.updatedBy));
                    if (isDeleted)
                    {
                        return Ok(ResponseHelper.Success(MessageConstants.ProductCategoryDeleted));
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
        [Route("update-product-category")]
        public IActionResult UpdateProductCategory(UpdateProductCategoryModel updateProdutCategorymodel)
        {
            if (ModelState.IsValid)
            {
                ProductCategory produtCategoryModel = ProductCategoryHelper.BindProductCategoryModel(updateProdutCategorymodel);
                long productCategoryId = iProductCategory.UpdateProductCategory(produtCategoryModel,DBHelper.ParseInt64(produtCategoryModel.UpdatedBy));
                if (productCategoryId > 0)
                {
                    return Ok(ResponseHelper.Success(MessageConstants.ProductCategoryUpdated));
                }
                else if (productCategoryId == ReturnCode.AlreadyExist.GetHashCode())
                {
                    return Ok(ResponseHelper.Error(MessageConstants.TryDifferentName));
                }
                else
                {
                    return Ok(ResponseHelper.Error(MessageConstants.ProductCategoryNotUpdated));
                }
            }
            else
            {
                return Ok(ResponseHelper.Error(MessageConstants.CompulsoryData));
            }
        }

        /// <summary>
        /// Products the category list.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("product-category-list")]
        public IActionResult ProductCategoryList(SearchPaginationListModel model)
        {
            try
            {
                if (model.pageNo <= 0)
                {
                    model.pageNo = 1;
                }
                var productCategoryList = iProductCategory.GetProductCategory(model.searchString);
                if (productCategoryList != null)
                {
                    var productCategoryPagedresult = productCategoryList.OrderByDescending(x => x.ProductCategoryId).ToPagedList(model.pageNo, model.limit).ToList();
                    List<ProductCategoryListModel> productCategoryListModel = ProductCategoryHelper.BindProductCategoryListModel(productCategoryPagedresult);
                    return Ok(ResponseHelper.Success(productCategoryListModel));
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
        /// Products the cateogy detail.
        /// </summary>
        /// <param name="ProductCategoryId">The product cateogy identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("product-category-detail")]
        public IActionResult ProductCategoryDetail(ProductCategoryDetailModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var productCategoryDetail = iProductCategory.GetProductCategoryDetail(DBHelper.ParseInt64(model.productCategoryId));
                    if (productCategoryDetail != null)
                    {
                        ProductCategoryListModel productCategoryListModel = ProductCategoryHelper.BindProductCategoryListModel(productCategoryDetail);
                        return Ok(ResponseHelper.Success(productCategoryListModel));
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