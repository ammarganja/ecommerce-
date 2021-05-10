using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SHKang.Core;
using SHKang.Core.Constant;
using SHKang.Core.Enums;
using SHKang.Core.Helpers;
using SHKang.Core.ModelHelper;
using SHKang.Model.Models;
using SHKang.Model.ViewModels.Admin;
using SHKang.Model.ViewModels.Web;
using SHKang.Repository.Interface;
using X.PagedList;

namespace SHKang.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenLessController : ControllerBase
    {
        #region Private Variables

        /// <summary>
        /// The i style campaign
        /// </summary>
        private readonly IStyleCampaign iStyleCampaign;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductController" /> class.
        /// </summary>
        /// <param name="_iStyleCampaign">The i style campaign.</param>
        public TokenLessController(IStyleCampaign _iStyleCampaign)
        {
            iStyleCampaign = _iStyleCampaign;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the campaign list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("get-campaign-list")]
        public IActionResult GetCampaignList(SearchPaginationListModel searchModel)
        {
            try
            {
                string scheme = this.Request.Scheme;
                scheme += "://" + this.Request.Host + this.Request.PathBase;
                if (searchModel.pageNo <= 0)
                {
                    searchModel.pageNo = 1;
                }
                bool isShowFrontend = false;
                if (searchModel.Status.HasValue && searchModel.Status.Equals(1))
                {
                    isShowFrontend = true;
                }
                var campaignList = iStyleCampaign.GetCampaignList(searchModel.searchString, isShowFrontend);
                var productCategoryTypeIds = ProductHelper.BindProductCategoryTypeIds(campaignList);
                var productList = iStyleCampaign.GetCampaignProductCount(productCategoryTypeIds);
                if (campaignList != null)
                {
                    var campaignListPagedResult = campaignList.OrderBy(x => x.ProductCategoryTypeId).ToPagedList(searchModel.pageNo, searchModel.limit).ToList();

                    List<CampaignListModel> model = ProductHelper.BindCampaignListModel(campaignListPagedResult, productList, scheme);

                    #region Sorting
                    if (searchModel.column.ToLower().Equals(DBHelper.ParseString(SortingCampaignColumnName.campaignid)) && searchModel.direction.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.asc)))
                    {
                        model = model.OrderBy(x => x.CampaignId).ToList();
                    }
                    else if (searchModel.column.ToLower().Equals(DBHelper.ParseString(SortingCampaignColumnName.campaignid)) && searchModel.direction.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.desc)))
                    {
                        model = model.OrderByDescending(x => x.CampaignId).ToList();
                    }
                    else if (searchModel.column.ToLower().Equals(DBHelper.ParseString(SortingCampaignColumnName.campaignname)) && searchModel.direction.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.asc)))
                    {
                        model = model.OrderBy(x => x.CampaignName).ToList();
                    }
                    else if (searchModel.column.ToLower().Equals(DBHelper.ParseString(SortingCampaignColumnName.campaignname)) && searchModel.direction.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.desc)))
                    {
                        model = model.OrderByDescending(x => x.CampaignName).ToList();
                    }
                    else if (searchModel.column.ToLower().Equals(DBHelper.ParseString(SortingCampaignColumnName.description)) && searchModel.direction.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.asc)))
                    {
                        model = model.OrderBy(x => x.Description).ToList();
                    }
                    else if (searchModel.column.ToLower().Equals(DBHelper.ParseString(SortingCampaignColumnName.description)) && searchModel.direction.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.desc)))
                    {
                        model = model.OrderByDescending(x => x.Description).ToList();
                    }
                    else if (searchModel.column.ToLower().Equals(DBHelper.ParseString(SortingCampaignColumnName.totalProduct)) && searchModel.direction.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.asc)))
                    {
                        model = model.OrderBy(x => x.TotalProduct).ToList();
                    }
                    else if (searchModel.column.ToLower().Equals(DBHelper.ParseString(SortingCampaignColumnName.totalProduct)) && searchModel.direction.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.desc)))
                    {
                        model = model.OrderByDescending(x => x.TotalProduct).ToList();
                    }
                    else
                    {
                        model = model.OrderBy(x => x.CampaignId).ToList();
                    }
                    #endregion

                    PagedListModel<CampaignListModel> pagedListModel = new PagedListModel<CampaignListModel>();
                    pagedListModel.Items = model;
                    pagedListModel.Total = campaignList.Count;
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
        #endregion
    }
}