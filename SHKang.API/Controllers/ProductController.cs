namespace SHKang.API.Controllers
{
    #region namespaces

    using Microsoft.AspNetCore.Mvc;
    using SHKang.API.Helper;
    using SHKang.Core;
    using SHKang.Core.Constant;
    using SHKang.Core.Enums;
    using SHKang.Core.Helpers;
    using SHKang.Core.ModelHelper;
    using SHKang.Model.ViewModels.Admin;
    using SHKang.Model.ViewModels.Web;
    using SHKang.Repository.Interface;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using X.PagedList;
    #endregion

    [Route("api/[controller]")]
    [ApiController]
    [AuthorizeHelper]
    public class ProductController : ControllerBase
    {

        #region Private Variables

        /// <summary>
        /// The i style campaign
        /// </summary>
        private readonly IStyleCampaign iStyleCampaign;

        /// <summary>
        /// The i product
        /// </summary>
        private readonly IProduct iProduct;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductController"/> class.
        /// </summary>
        /// <param name="_iStyleCampaign">The i style campaign.</param>
        public ProductController(IStyleCampaign _iStyleCampaign, IProduct _iProduct)
        {
            iProduct = _iProduct;
            iStyleCampaign = _iStyleCampaign;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the style product list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("get-style-product-list")]
        public IActionResult GetStyleProductList(SearchProductListModel searchModel)
        {
            try
            {
                string scheme = this.Request.Scheme;
                scheme += "://" + this.Request.Host + this.Request.PathBase;
                StyleCampaignList styleCampaignList = new StyleCampaignList();
                if (searchModel.pageNo <= 0)
                    searchModel.pageNo = 1;
                styleCampaignList = iStyleCampaign.GetStyleCampaignList(DBHelper.ParseString((int)ProductCategoryEnum.Styles), searchModel.searchString, searchModel.pageNo, searchModel.limit, searchModel.CategoryTypeId, scheme, searchModel.column, searchModel.direction);
                if (styleCampaignList != null)
                {
                    return Ok(ResponseHelper.Success(styleCampaignList));
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
        /// Gets the campaign product list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("get-campaign-product-list")]
        public IActionResult GetCampaignProductList(GetCampaignProductModel searchModel)
        {
            try
            {
                string scheme = this.Request.Scheme;
                scheme += "://" + this.Request.Host + this.Request.PathBase;
                StyleCampaignList styleCampaignList = new StyleCampaignList();
                if (searchModel.pageNo <= 0)
                    searchModel.pageNo = 1;
                styleCampaignList = iStyleCampaign.GetStyleCampaignList(DBHelper.ParseString((int)ProductCategoryEnum.Campaign), searchModel.searchString, searchModel.pageNo, searchModel.limit, searchModel.productCategoryTypeId, scheme, searchModel.column, searchModel.direction);
                if (styleCampaignList != null)
                {
                    return Ok(ResponseHelper.Success(styleCampaignList));
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
        /// Searches the product.
        /// </summary>
        /// <param name="SearchText">The search text.</param>
        /// <param name="PageNo">The page no.</param>
        /// <param name="ProductCategoryTypeId">The product category type identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("search-product")]
        public IActionResult SearchProduct(GetCampaignProductModel searchModel)
        {
            try
            {
                string scheme = this.Request.Scheme;
                scheme += "://" + this.Request.Host + this.Request.PathBase;
                if (searchModel.pageNo <= 0)
                {
                    searchModel.pageNo = 1;
                }
                StyleCampaignList styleCampaignList = new StyleCampaignList();
                styleCampaignList = iStyleCampaign.GetStyleCampaignList(DBHelper.ParseString((int)ProductCategoryEnum.Styles) + "," + DBHelper.ParseString((int)ProductCategoryEnum.Campaign), searchModel.searchString, searchModel.pageNo, searchModel.limit, searchModel.productCategoryTypeId, scheme, searchModel.column, searchModel.direction);
                if (styleCampaignList != null)
                {
                    return Ok(ResponseHelper.Success(styleCampaignList));
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
        /// Gets the product detail.
        /// </summary>
        /// <param name="ProductId">The product identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("get-product-detail")]
        public IActionResult GetProductDetail(GetProductDetailModel productModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string scheme = this.Request.Scheme;
                    scheme += "://" + this.Request.Host + this.Request.PathBase;
                    var productDetailModel = iProduct.GetProductDetail(productModel.productId, scheme);
                    if (productDetailModel != null)
                    {
                        return Ok(ResponseHelper.Success(productDetailModel));
                    }
                    else
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.ProductNotFound));
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

        [HttpPost]
        [Route("get-campaign-detail")]
        public IActionResult GetCampaignDetail(int campaignId)
        {
            try
            {
                string hostingPath = this.Request.Scheme;
                hostingPath += "://" + this.Request.Host + this.Request.PathBase;
                StyleCampaignDetailModel styleCampaignList = new StyleCampaignDetailModel();
                styleCampaignList = iStyleCampaign.GetCampaignDetail(campaignId, hostingPath);
                if (styleCampaignList != null)
                {
                    return Ok(ResponseHelper.Success(styleCampaignList));
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