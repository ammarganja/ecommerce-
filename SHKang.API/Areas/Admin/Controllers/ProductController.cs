namespace SHKang.API.Areas.Admin.Controllers
{
    #region namespaces

    using Microsoft.AspNetCore.Hosting;
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
    #endregion

    [Route("api/admin/[controller]")]
    [ApiController]
    [AuthorizeHelper]
    public class ProductController : ControllerBase
    {
        #region Private Variables

        /// <summary>
        /// The i product
        /// </summary>
        private readonly IProduct iProduct;

        /// <summary>
        /// The env
        /// </summary>
        private readonly IHostingEnvironment env;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductController"/> class.
        /// </summary>
        /// <param name="_iProduct">The i product.</param>
        /// <param name="_env">The env.</param>
        public ProductController(IProduct _iProduct, IHostingEnvironment _env)
        {
            iProduct = _iProduct;
            env = _env;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the product.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("add-product")]
        public IActionResult AddProduct(AddProductModel addProduct)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var hostingPath = env.ContentRootPath;
                    Product productModel = ProductHelper.BindProductModel(addProduct);
                    List<ProductCategoryDetail> productCategoryDetailModel = ProductHelper.BindProductCategoryListModel(addProduct);
                    string[] ProductSizeValues = addProduct.ProductSizeValue.Split(',');
                    if (productModel != null && productCategoryDetailModel != null && productCategoryDetailModel.Count > 0)
                    {
                        if (productModel.ProductId <= 0)
                        {
                            long productId = iProduct.AddProduct(productModel, productCategoryDetailModel, hostingPath, ProductSizeValues);
                            if (productId > 0)
                            {
                                return Ok(ResponseHelper.Success(productId));
                            }
                            else if (productId == ReturnCode.AlreadyExist.GetHashCode())
                            {
                                return Ok(ResponseHelper.Error(MessageConstants.TryDifferentName));
                            }
                            else
                            {
                                return Ok(ResponseHelper.Error(MessageConstants.ProductNotAdded));
                            }
                        }
                        else
                        {
                            long productId = iProduct.UpdateProduct(productModel, productCategoryDetailModel, hostingPath, ProductSizeValues);
                            if (productId > 0)
                            {
                                return Ok(ResponseHelper.Success(productId));
                            }
                            else if (productId == ReturnCode.AlreadyExist.GetHashCode())
                            {
                                return Ok(ResponseHelper.Error(MessageConstants.TryDifferentName));
                            }
                            else
                            {
                                return Ok(ResponseHelper.Error(MessageConstants.ProductNotUpdated));
                            }
                        }
                    }
                    else
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.CompulsoryData));
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
        /// Adds the product.
        /// </summary>
        /// <param name="addProduct">The add product.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("add-product-color")]
        public IActionResult AddProductColorImage([FromForm] AddProductColorModel addProduct)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existImageCount = iProduct.GetImageCountByProductColorId(DBHelper.ParseInt64(addProduct.productId), addProduct.ColorId);
                    string[] deleteImage = null;
                    if (!string.IsNullOrWhiteSpace(addProduct.DeleteImageId))
                    {
                        deleteImage = addProduct.DeleteImageId.Split(',');
                    }
                    if (existImageCount == 0 && (addProduct.image == null || Request.Form.Files.Count <= 0))
                    {
                        if (deleteImage != null && deleteImage.Length == existImageCount)
                        {
                            return Ok(ResponseHelper.Error(MessageConstants.AtleastOneImageColor));
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(addProduct.DeleteImageId))
                    {
                        if ((addProduct.image == null || Request.Form.Files.Count <= 0) && deleteImage != null && existImageCount == deleteImage.Length)
                            return Ok(ResponseHelper.Error(MessageConstants.AtleastOneImageColor));
                    }

                    if (addProduct.image == null && Request.Form != null && Request.Form.Files.Count > 0)
                    {
                        addProduct.image = new List<Microsoft.AspNetCore.Http.IFormFile>();
                        foreach (var file in Request.Form.Files)
                        {
                            addProduct.image.Add(file);
                        }
                    }
                    var hostingPath = env.ContentRootPath;
                    long productColorId = iProduct.AddProductColorImage(DBHelper.ParseInt64(addProduct.productId), addProduct.ColorId, addProduct.image, hostingPath, addProduct.DeleteImageId);
                    if (productColorId > 0)
                    {
                        return Ok(ResponseHelper.Success(MessageConstants.ProductColorImageUpdated));
                    }
                    else if (productColorId == ReturnCode.AlreadyExist.GetHashCode())
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.TryDifferentName));
                    }
                    else
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.ProductColorImageNotUpdated));
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
        /// Products the list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("product-list")]
        public IActionResult ProductList(SearchProductListModel searchModel)
        {
            try
            {
                if (searchModel.pageNo <= 0)
                {
                    searchModel.pageNo = 1;
                }
                string scheme = this.Request.Scheme;
                scheme += "://" + this.Request.Host + this.Request.PathBase;
                var productList = iProduct.GetProductList(searchModel.pageNo, searchModel.limit, searchModel.searchString, scheme, searchModel.column, searchModel.direction, searchModel.ColorId, searchModel.CategoryTypeId, searchModel.SizeId);
                if (productList != null)
                {
                    return Ok(ResponseHelper.Success(productList));
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
        /// Deletes the product.
        /// </summary>
        /// <param name="ProductId">The product identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete-product")]
        public IActionResult DeleteProduct(DeleteProductModel deleteModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var isDeleted = iProduct.DeleteProduct(deleteModel.productId, deleteModel.updatedBy);
                    if (isDeleted)
                    {
                        return Ok(ResponseHelper.Success(MessageConstants.ProductDeleted));
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
        /// Products the detail.
        /// </summary>
        /// <param name="ProductId">The product identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("product-detail")]
        public IActionResult ProductDetail(GetProductDetailModel productDetailModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string scheme = this.Request.Scheme;
                    scheme += "://" + this.Request.Host + this.Request.PathBase;
                    var productDetail = iProduct.GetProductDetail(productDetailModel.productId, scheme);
                    if (productDetail != null)
                    {
                        return Ok(ResponseHelper.Success(productDetail));
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
        /// Changes the product status.
        /// </summary>
        /// <param name="productStatusModel">The product status model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("change-product-status")]
        public IActionResult ChangeProductStatus(ChangeProductStatus productStatusModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var productDetail = iProduct.ChangeProductStatus(productStatusModel.productId, productStatusModel.Status);
                    if (productDetail)
                    {
                        return Ok(ResponseHelper.Success(MessageConstants.ProductStatusUpdated));
                    }
                    else
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.ProductStatusNotUpdated));
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
        /// Products the order list.
        /// </summary>
        /// <param name="searchModel">The search model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("product-order-list")]
        public IActionResult ProductOrderList(SearchProductListModel searchModel)
        {
            try
            {
                string scheme = this.Request.Scheme;
                scheme += "://" + this.Request.Host + this.Request.PathBase;
                if (searchModel.pageNo <= 0)
                {
                    searchModel.pageNo = 1;
                }
                var productList = iProduct.GetOrderProductList(searchModel.pageNo, searchModel.limit, searchModel.searchString, scheme, searchModel.column, searchModel.direction);
                if (productList != null)
                {
                    return Ok(ResponseHelper.Success(productList));
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
        /// Deletes the color of the product.
        /// </summary>
        /// <param name="deleteModel">The delete model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete-product-color")]
        public IActionResult DeleteProductColor(DeleteProductColorModel deleteModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var hostingPath = env.ContentRootPath;
                    var isDeleted = iProduct.DeleteProductColor(deleteModel.ProductId, deleteModel.ColorId, hostingPath);
                    if (isDeleted)
                    {
                        return Ok(ResponseHelper.Success(MessageConstants.ProductColorDeleted));
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