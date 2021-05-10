namespace SHKang.API.Areas.Admin.Controllers
{
    #region namespaces
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
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
    using System.Threading.Tasks;
    #endregion


    [Route("api/admin/[controller]")]
    [ApiController]
    [AuthorizeHelper]
    public class CampaignController : ControllerBase
    {
        #region Private Variables

        /// <summary>
        /// The i style campaign
        /// </summary>
        private readonly IStyleCampaign iStyleCampaign;

        /// <summary>
        /// The env
        /// </summary>
        private readonly IHostingEnvironment env;

        /// <summary>
        /// The i product category type
        /// </summary>
        private readonly IProductCategoryType iProductCategoryType;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignController" /> class.
        /// </summary>
        /// <param name="_iStyleCampaign">The i style campaign.</param>
        /// <param name="_env">The env.</param>
        /// <param name="_iProductCategoryType">Type of the i product category.</param>
        public CampaignController(IStyleCampaign _iStyleCampaign, IHostingEnvironment _env, IProductCategoryType _iProductCategoryType)
        {
            iStyleCampaign = _iStyleCampaign;
            env = _env;
            iProductCategoryType = _iProductCategoryType;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the color.
        /// </summary>
        /// <param name="addCampaignModel">The color model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("add-campaign")]
        public IActionResult AddCampaign([FromForm] AddUpdateCampaignModel addCampaignModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var productCategoryTypeModel = iProductCategoryType.ProductCategoryTypeDetail(DBHelper.ParseInt64(addCampaignModel.CampaignId));
                    var hostingPath = env.ContentRootPath;
                    ProductCategoryType productCategoryType = ProductCategoryTypeHelper.BindProductCategoryTypeModel(addCampaignModel, hostingPath, productCategoryTypeModel);
                    if (addCampaignModel.CampaignId <= 0)
                    {
                        long productCategoryTypeId = iProductCategoryType.AddNewProductCategoryType(productCategoryType, addCampaignModel.ProductId);
                        if (productCategoryTypeId > 0)
                        {
                            return Ok(ResponseHelper.Success(MessageConstants.CampaignAdded));
                        }
                        else if (productCategoryTypeId == ReturnCode.AlreadyExist.GetHashCode())
                        {
                            ProductCategoryTypeHelper.DeleteProductImageToFolder(productCategoryType.Image, hostingPath);
                            return Ok(ResponseHelper.Error(MessageConstants.CampaignExists));
                        }
                        else
                        {
                            ProductCategoryTypeHelper.DeleteProductImageToFolder(productCategoryType.Image, hostingPath);
                            return Ok(ResponseHelper.Error(MessageConstants.CampaignNotAdded));
                        }
                    }
                    else
                    {
                        long productCategoryTypeId = iProductCategoryType.UpdateProductCategoryType(productCategoryType, DBHelper.ParseInt64(productCategoryType.UpdatedBy), addCampaignModel.ProductId);
                        if (productCategoryTypeId > 0)
                        {
                            return Ok(ResponseHelper.Success(MessageConstants.CampaignUpdated));
                        }
                        else if (productCategoryTypeId == ReturnCode.AlreadyExist.GetHashCode())
                        {
                            ProductCategoryTypeHelper.DeleteProductImageToFolder(productCategoryType.Image, hostingPath);
                            return Ok(ResponseHelper.Error(MessageConstants.CampaignExists));
                        }
                        else
                        {
                            ProductCategoryTypeHelper.DeleteProductImageToFolder(productCategoryType.Image, hostingPath);
                            return Ok(ResponseHelper.Error(MessageConstants.CampaignNotUpdated));
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
        /// <param name="deleteModel">The delete model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete-campaign")]
        public IActionResult DeleteCampaign(DeleteCampaignModel deleteModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var hostingPath = env.ContentRootPath;
                    var productCategory = iProductCategoryType.ProductCategoryTypeDetail(deleteModel.CampaignId);
                    if (productCategory != null)
                    {
                        ProductCategoryTypeHelper.DeleteProductImageToFolder(productCategory.Image, hostingPath);
                    }
                    bool isDeleted = iProductCategoryType.DeleteProductCategoryType(deleteModel.CampaignId, deleteModel.UpdatedBy);
                    if (isDeleted)
                    {
                        return Ok(ResponseHelper.Success(MessageConstants.CampaignDeleted));
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
        /// Gets the campaign product list.
        /// </summary>
        /// <param name="getCampaignProductListModel">The get campaign product list model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("get-campaign-product-list")]
        public IActionResult GetCampaignProductList(GetCampaignProductListModel getCampaignProductListModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string scheme = this.Request.Scheme;
                    scheme += "://" + this.Request.Host + this.Request.PathBase;
                    if (getCampaignProductListModel.pageNo <= 0)
                    {
                        getCampaignProductListModel.pageNo = 1;
                    }
                    var productList = iStyleCampaign.GetCampaignProductList(getCampaignProductListModel.pageNo, getCampaignProductListModel.limit, getCampaignProductListModel.searchString, scheme, getCampaignProductListModel.column, getCampaignProductListModel.direction, getCampaignProductListModel.ProductCategoryTypeId);
                    if (productList != null)
                    {
                        return Ok(ResponseHelper.Success(productList));
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