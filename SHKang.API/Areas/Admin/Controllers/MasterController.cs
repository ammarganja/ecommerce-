namespace SHKang.API.Areas.Admin.Controllers
{
    #region namespaces
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using SHKang.API.Helper;
    using SHKang.Core;
    using SHKang.Core.Constant;
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
    public class MasterController : ControllerBase
    {
        #region Private Variables

        /// <summary>
        /// The i common
        /// </summary>
        private readonly IMaster iMaster;

        /// <summary>
        /// The i product category type
        /// </summary>
        private readonly IProductCategoryType iProductCategoryType;

        /// <summary>
        /// The i user
        /// </summary>
        private readonly IUser iUser;

        /// <summary>
        /// The i size
        /// </summary>
        private readonly ISize iSize;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CommonController" /> class.
        /// </summary>
        /// <param name="_iMaster">The i common.</param>
        /// <param name="_iProductCategoryType">Type of the i product category.</param>
        /// <param name="_iUser">The i user.</param>
        /// <param name="_iSize">Size of the i.</param>
        public MasterController(IMaster _iMaster, IProductCategoryType _iProductCategoryType, IUser _iUser, ISize _iSize)
        {
            iMaster = _iMaster;
            iProductCategoryType = _iProductCategoryType;
            iUser = _iUser;
            iSize = _iSize;

        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the color list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("get-color-list")]
        public IActionResult GetColorList()
        {
            try
            {
                var colorList = iMaster.GetColors();
                List<SelectListItemModel> selectListItemModel = SelectHelper.BindSelectListItem(colorList);
                return Ok(ResponseHelper.Success(selectListItemModel));
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                return Ok(ResponseHelper.Error(ex.Message));
            }
        }

        /// <summary>
        /// Gets the size list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("get-size-list")]
        public IActionResult GetSizeList()
        {
            try
            {
                var sizeList = iMaster.GetSize();
                List<SelectListItemModel> selectListItemModel = SelectHelper.BindSelectListItem(sizeList);
                return Ok(ResponseHelper.Success(selectListItemModel));
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                return Ok(ResponseHelper.Error(ex.Message));
            }
        }

        /// <summary>
        /// Gets the size ratio list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("get-size-ratio-list")]
        public IActionResult GetSizeRatioList()
        {
            try
            {
                var sizeRatioList = iMaster.GetSizeRatio();
                List<SelectListItemModel> selectListItemModel = SelectHelper.BindSelectListItem(sizeRatioList);
                return Ok(ResponseHelper.Success(selectListItemModel));
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                return Ok(ResponseHelper.Error(ex.Message));
            }
        }

        /// <summary>
        /// Gets the prduct category.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("get-product-category")]
        public IActionResult GetPrductCategory()
        {
            try
            {
                var categoryList = iProductCategoryType.GetAllProductCategoryType();
                List<SelectListItemModel> selectListItemModel = SelectHelper.BindSelectListItem(categoryList);
                return Ok(ResponseHelper.Success(selectListItemModel));
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                return Ok(ResponseHelper.Error(ex.Message));
            }
        }

        /// <summary>
        /// Gets the user list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("get-user-list")]
        public IActionResult GetUserList()
        {
            try
            {
                var userList = iUser.GetAllUserListForDropDown();
                List<SelectListItemModel> selectListItemModel = SelectHelper.BindSelectListItem(userList);
                return Ok(ResponseHelper.Success(selectListItemModel));
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                return Ok(ResponseHelper.Error(ex.Message));
            }
        }

        /// <summary>
        /// Gets the country list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("get-country-list")]
        public IActionResult GetCountryList()
        {
            try
            {
                var countries = iMaster.GetCountryList();
                return Ok(ResponseHelper.Success(countries));
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                return Ok(ResponseHelper.Error(ex.Message));
            }
        }

        /// <summary>
        /// Gets the state list.
        /// </summary>
        /// <param name="countryId">The country identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("get-state-list")]
        public IActionResult GetStateList(Country country)
        {
            try
            {
                var states = iMaster.GetStateList(country.CountryId);
                return Ok(ResponseHelper.Success(states));
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                return Ok(ResponseHelper.Error(ex.Message));
            }
        }

        /// <summary>
        /// Gets the size detail list.
        /// </summary>
        /// <param name="json">The json.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("get-size-detail-list")]
        public IActionResult GetSizeDetailList([FromBody] JObject json)
        {
            try
            {
                JObject jObject = JsonConvert.DeserializeObject<JObject>(DBHelper.ParseString(json));
                if (jObject != null)
                {
                    long sizeId = DBHelper.ParseInt64(jObject["sizeId"]);
                    var sizeList = iSize.GetSizeDetails(sizeId);
                    string[] sizeArray = SelectHelper.BindSizeArray(sizeList);
                    return Ok(ResponseHelper.Success(sizeArray));
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