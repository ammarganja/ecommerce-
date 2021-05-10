namespace SHKang.API.Controllers
{
    #region namespaces
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using SHKang.Core;
    using SHKang.Core.Constant;
    using SHKang.Core.Enums;
    using SHKang.Core.Helpers;
    using SHKang.Core.ModelHelper;
    using SHKang.Model.Models;
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
    [Authorize]
    public class CommonController : ControllerBase
    {

        #region Private Variables

        /// <summary>
        /// The i user
        /// </summary>
        private readonly IUser iUser;

        /// <summary>
        /// The i order
        /// </summary>
        private readonly IOrder iOrder;

        /// <summary>
        /// The i order invoice
        /// </summary>
        private readonly IOrderInvoice iOrderInvoice;

        /// <summary>
        /// The env
        /// </summary>
        private readonly IHostingEnvironment env;

        /// <summary>
        /// The configuration
        /// </summary>
        private readonly IConfiguration config;

        /// <summary>
        /// The i cart
        /// </summary>
        private readonly ICart iCart;

        /// <summary>
        /// The i style campaign
        /// </summary>
        private readonly IStyleCampaign iStyleCampaign;

        /// <summary>
        /// The i testimonial
        /// </summary>
        private readonly ITestimonial iTestimonial;

        /// <summary>
        /// The i promo code
        /// </summary>
        private readonly IPromoCode iPromoCode;

        /// <summary>
        /// The i master
        /// </summary>
        private readonly IMaster iMaster;

        /// <summary>
        /// The i product category type
        /// </summary>
        private readonly IProductCategoryType iProductCategoryType;

        /// <summary>
        /// The i product
        /// </summary>
        private readonly IProduct iProduct;

        /// <summary>
        /// The i size
        /// </summary>
        private readonly ISize iSize;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CommonController" /> class.
        /// </summary>
        /// <param name="_iUser">The i user.</param>
        /// <param name="_iOrder">The i order.</param>
        /// <param name="_iOrderInvoice">The i order invoice.</param>
        /// <param name="_env">The env.</param>
        /// <param name="_config">The configuration.</param>
        /// <param name="_iCart">The i cart.</param>
        /// <param name="_iContent">Content of the i.</param>
        /// <param name="_iStyleCampaign">The i style campaign.</param>
        /// <param name="_iTestimonial">The i testimonial.</param>
        /// <param name="_iPromoCode">The i promo code.</param>
        /// <param name="_iMaster">The i master.</param>
        /// <param name="_iProductCategoryType">Type of the i product category.</param>
        /// <param name="_iProduct">The i product.</param>
        /// <param name="_iSize">Size of the i.</param>
        public CommonController(IUser _iUser, IOrder _iOrder, IOrderInvoice _iOrderInvoice, IHostingEnvironment _env, IConfiguration _config, ICart _iCart, IContent _iContent, IStyleCampaign _iStyleCampaign, ITestimonial _iTestimonial, IPromoCode _iPromoCode, IMaster _iMaster, IProductCategoryType _iProductCategoryType, IProduct _iProduct, ISize _iSize)
        {
            iUser = _iUser;
            iOrder = _iOrder;
            iOrderInvoice = _iOrderInvoice;
            env = _env;
            config = _config;
            iCart = _iCart;
            iStyleCampaign = _iStyleCampaign;
            iTestimonial = _iTestimonial;
            iPromoCode = _iPromoCode;
            iMaster = _iMaster;
            iProductCategoryType = _iProductCategoryType;
            iProduct = _iProduct;
            iSize = _iSize;
        }
        #endregion

        #region Public Methods

        #region User

        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("update-user")]
        public IActionResult UpdateUser(AddNewUserModel addNewUserModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User userModel = UserHelper.BindUserModel(addNewUserModel);
                    if (!string.IsNullOrWhiteSpace(userModel.Password))
                    {
                        userModel.Password = EncryptDecryptHelper.Encrypt(userModel.Password);
                    }
                    long userId = iUser.UpdateUser(userModel);
                    if (userId > 0)
                    {
                        return Ok(ResponseHelper.Success(MessageConstants.UserUpdated));
                    }
                    else if (userId == ReturnCode.AlreadyExist.GetHashCode())
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.TryDifferentEmailId));
                    }
                    else
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.UserNotUpdated));
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
        /// Users the detail.
        /// </summary>
        /// <param name="UserId">The user identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("user-detail")]
        public IActionResult UserDetail(DeleteUserModel userModel)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(userModel.userId))
                {
                    var userDetail = iUser.GetUserDetail(DBHelper.ParseInt64(userModel.userId));
                    if (userDetail != null)
                    {
                        UserListModel userListModel = UserHelper.BindUserListModel(userDetail);
                        return Ok(ResponseHelper.Success(userListModel));
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

        #region Order

        [HttpPost]
        [Route("order-list")]
        /// <summary>
        /// Orders the list.
        /// </summary>
        /// <param name="UserId">The user identifier.</param>
        /// <returns></returns>
        public IActionResult OrderList(SearchPaginationListModel searchModel)
        {
            try
            {
                if (searchModel.pageNo <= 0)
                {
                    searchModel.pageNo = 1;
                }
                var orderList = iOrder.GetOrderList(searchModel.pageNo, searchModel.limit, searchModel.searchString, searchModel.column, searchModel.direction);
                if (orderList != null)
                {
                    return Ok(ResponseHelper.Success(orderList));
                }
                else
                {
                    return Ok(ResponseHelper.Error(MessageConstants.NoOrderFound));
                }
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                return Ok(ResponseHelper.Error(ex.Message));
            }
        }

        /// <summary>
        /// Orders the detail.
        /// </summary>
        /// <param name="OrderId">The order identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("order-detail")]
        public IActionResult OrderDetail(long orderId)
        {
            try
            {
                if (orderId > 0)
                {
                    var orderDetail = iOrder.GetOrderDetail(orderId);
                    var invoiceList = iOrderInvoice.GetOrderInvoiceDetail(0, orderId);
                    if (orderDetail != null)
                    {
                        OrderDetailModel orderDetailModel = OrderHelper.BindOrderDetailModel(orderDetail, invoiceList);
                        return Ok(ResponseHelper.Success(orderDetailModel));
                    }
                    else
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.NoOrderFound));
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
        /// Contents the details.
        /// </summary>
        /// <param name="Shortname">The shortname.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete-order")]
        public IActionResult DeleteOrder(DeleteOrderModel deleteModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var contentDetailModel = iOrder.DeleteOrder(deleteModel.OrderId);
                    if (contentDetailModel)
                    {

                        return Ok(ResponseHelper.Success(MessageConstants.OrderDeleted));
                    }
                    else
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.OrderNotDeleted));
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
        /// Adds the order.
        /// </summary>
        /// <param name="addNewOrderModel">The add new order model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("add-order")]
        public IActionResult AddOrder(AddNewOrderModel addNewOrderModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var uniquePONumber = iOrder.GetAllUniquePoNumber();
                    var uniqueOrderIds = iOrder.GetAllUniqueInvoiceIds();
                    long promoCodeId = 0;
                    decimal percentage = 0;
                    decimal discountAmount = 0;
                    if (!string.IsNullOrWhiteSpace(addNewOrderModel.Promocode))
                    {
                        var promocodeModel = iPromoCode.GetPromoCodeByCode(addNewOrderModel.Promocode);
                        if (promocodeModel != null)
                        {
                            promoCodeId = promocodeModel.PromoCodeId;
                            percentage = promocodeModel.Percentage;
                            discountAmount = promocodeModel.Amount;
                        }
                    }
                    Order orderModel = OrderHelper.BindOrderModel(addNewOrderModel, uniquePONumber, uniqueOrderIds, promoCodeId, percentage, discountAmount);
                    List<OrderDetail> detailModel = OrderHelper.BindOrderDetailModel(addNewOrderModel.ProductDetail);
                    long orderId = 0;
                    if (orderModel.OrderId <= 0)
                    {
                        orderId = iOrder.AddOrder(orderModel, detailModel);
                    }
                    else
                    {
                        orderId = iOrder.EditOrder(orderModel, detailModel);
                    }
                    if (orderId > 0)
                    {
                        var cart = iCart.EmptyCart(DBHelper.ParseInt64(addNewOrderModel.UserId));
                        var orderDetailModel = iOrder.GetOrderDetail(orderId);
                        if (!string.IsNullOrWhiteSpace(addNewOrderModel.IsMailSend) && addNewOrderModel.IsMailSend.Equals("1"))
                        {
                            if (orderDetailModel != null)
                            {
                                var HostingPath = env.ContentRootPath;
                                string clientEmail = config.GetValue<string>("app_settings:ClientEmail");
                                string clientEmailPassword = config.GetValue<string>("app_settings:ClientEmailPassword");
                                string port = config.GetValue<string>("app_settings:Port");
                                string mailHost = config.GetValue<string>("app_settings:SMTPURL");
                                string host = this.Request.Host.Value;
                                string scheme = this.Request.Scheme;
                                OrderDetailModel orderDetailmodel = OrderHelper.BindOrderDetailModel(orderDetailModel, null);
                                OrderHelper.SendResetPasswordMail(orderDetailmodel.EmailId, orderDetailmodel, DBHelper.ParseString(HostingPath), clientEmail, clientEmailPassword, port, mailHost, host, scheme);
                            }
                        }
                        return Ok(ResponseHelper.Success(MessageConstants.OrderPlaced + orderDetailModel[0].Order.UniqueOrderId));
                    }
                    else
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.OrderNotPlaced));
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
        /// Adds the order.
        /// </summary>
        /// <param name="addNewOrderModel">The add new order model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("get-edit-order")]
        public IActionResult GetEditOrder(GetEditOrderModel deleteOrderModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string scheme = this.Request.Scheme;
                    scheme += "://" + this.Request.Host + this.Request.PathBase;
                    if (deleteOrderModel.pageNo <= 0)
                    {
                        deleteOrderModel.pageNo = 1;
                    }
                    var productList = iOrder.GetOrderForEdit(deleteOrderModel.OrderId, deleteOrderModel.pageNo, deleteOrderModel.limit, deleteOrderModel.searchString, deleteOrderModel.column, deleteOrderModel.direction, scheme);
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

        #region User Address

        /// <summary>
        /// Users the address list.
        /// </summary>
        /// <param name="UserId">The user identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("user-address-list")]
        public IActionResult UserAddressList(DeleteUserModel userModel)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(userModel.userId) && DBHelper.ParseInt64(userModel.userId) > 0)
                {
                    var userAddressList = iUser.UserAddressList(DBHelper.ParseInt64(userModel.userId), userModel.pageNo, userModel.limit, userModel.column, userModel.direction);
                    if (userAddressList != null)
                    {
                        //List<UserAddressListModel> userAddressListModel = UserHelper.BindUserAddressListModel(userAddressList);
                        return Ok(ResponseHelper.Success(userAddressList));
                    }
                    else
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.UserAddressNotDeleted));
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
        /// Deletes the user address.
        /// </summary>
        /// <param name="UserAddressId">The user address identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete-user-address")]
        public IActionResult DeleteUserAddress(DeleteUserAddressModel deleteModel)
        {
            try
            {
                if (deleteModel.userAddressId > 0)
                {
                    bool isDeleted = iUser.DeleteUserAddress(deleteModel.userAddressId);
                    if (isDeleted)
                    {
                        return Ok(ResponseHelper.Success(MessageConstants.UserAddressDeleted));
                    }
                    else
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.UserAddressNotDeleted));
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
        /// Adds the user address.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("add-user-address")]
        public IActionResult AddUserAddress(AddUserAddressModel addUserAddressModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserAddress addModel = UserHelper.BindUserAddress(addUserAddressModel);
                    if (addModel.UserAddressId <= 0)
                    {
                        long userAddressId = iUser.AddNewUserAddress(addModel);
                        if (userAddressId > 0)
                        {
                            return Ok(ResponseHelper.Success(MessageConstants.UserAddressAdded));
                        }
                        else if (userAddressId == ReturnCode.IsPrimaryExist.GetHashCode())
                        {
                            return Ok(ResponseHelper.Error(MessageConstants.OnlyPrimaryAddress));
                        }
                        else if (userAddressId == ReturnCode.AlreadyExist.GetHashCode())
                        {
                            return Ok(ResponseHelper.Error(MessageConstants.TryDifferentAddress));
                        }
                        else
                        {
                            return Ok(ResponseHelper.Error(MessageConstants.UserAddressNotAdded));
                        }
                    }
                    else
                    {
                        long userAddressId = iUser.UpdateUserAddress(addModel);
                        if (userAddressId > 0)
                        {
                            return Ok(ResponseHelper.Success(MessageConstants.UserAddressUpdated));
                        }
                        else if (userAddressId == ReturnCode.IsPrimaryExist.GetHashCode())
                        {
                            return Ok(ResponseHelper.Error(MessageConstants.OnlyPrimaryAddress));
                        }
                        else if (userAddressId == ReturnCode.AlreadyExist.GetHashCode())
                        {
                            return Ok(ResponseHelper.Error(MessageConstants.TryDifferentAddress));
                        }
                        else
                        {
                            return Ok(ResponseHelper.Error(MessageConstants.UserAddressNotUpdated));
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

        #endregion

        #region Campaign

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
                    List<ProductCategoryType> campaignListPagedResult = new List<ProductCategoryType>();
                    if (searchModel.limit == 0)
                    {
                        campaignListPagedResult = campaignList.OrderBy(x => x.ProductCategoryTypeId).ToList();
                    }
                    else
                    {
                        campaignListPagedResult = campaignList.OrderBy(x => x.ProductCategoryTypeId).ToPagedList(searchModel.pageNo, searchModel.limit).ToList();
                    }

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
        #endregion

        #region Testimonials

        /// <summary>
        /// Adds the testimonial.
        /// </summary>
        /// <param name="addTestimonial">The add testimonial.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("add-testimonial")]
        public IActionResult AddTestimonial(AddUpdateTestimonialModel addTestimonial)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var testimonialModel = TestimonialHelper.BindTestimonialModel(addTestimonial);
                    if (testimonialModel.TestimonialId <= 0)
                    {
                        long testimonialId = iTestimonial.AddTestimonial(testimonialModel);
                        if (testimonialId > 0)
                        {
                            return Ok(ResponseHelper.Success(MessageConstants.TestimonialAdded));
                        }
                        else
                        {
                            return Ok(ResponseHelper.Error(MessageConstants.TestimonialNotAdded));
                        }
                    }
                    else
                    {
                        long testimonialId = iTestimonial.UpdateTestimonial(testimonialModel);
                        if (testimonialId > 0)
                        {
                            return Ok(ResponseHelper.Success(MessageConstants.TestimonialUpdated));
                        }
                        else
                        {
                            return Ok(ResponseHelper.Error(MessageConstants.TestimonialNotUpdated));
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
        /// Testimonials the list.
        /// </summary>
        /// <param name="searchModel">The search model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("testimonial-list")]
        public IActionResult TestimonialList(SearchPaginationListModel searchModel)
        {
            try
            {
                string scheme = this.Request.Scheme;
                scheme += "://" + this.Request.Host + this.Request.PathBase;
                if (searchModel.pageNo <= 0)
                {
                    searchModel.pageNo = 1;
                }
                var testimonialList = iTestimonial.GetTestimonials(searchModel.searchString);
                if (testimonialList != null)
                {
                    List<TestimonialListModel> testimonialResult = new List<TestimonialListModel>();
                    testimonialResult = TestimonialHelper.BindTestimonialListModel(testimonialList, scheme);

                    #region Sorting
                    testimonialResult = TestimonialHelper.GetSortedTestimonial(searchModel.column, searchModel.direction, testimonialResult).ToPagedList(searchModel.pageNo, searchModel.limit).ToList();
                    #endregion

                    PagedListModel<TestimonialListModel> pagedListModel = new PagedListModel<TestimonialListModel>();
                    pagedListModel.Items = testimonialResult.ToList();
                    pagedListModel.Total = testimonialList.Count();
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

        #endregion

        #region Master

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
        #endregion

        #region Promocode

        /// <summary>
        /// Checks the promocode.
        /// </summary>
        /// <param name="promocodeModel">The promocode model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("check-promocode")]
        public IActionResult CheckPromocode(CheckPromocodeModel promocodeModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var promocode = iPromoCode.GetPromoCodeByCode(promocodeModel.Code);
                    if (promocode != null)
                    {
                        var cartDetails = iCart.GetUserCartDetail(promocodeModel.UserId, "");
                        var cartReturnModel = CartHelper.BindCheckPromocodeReturnModel(promocode, cartDetails);
                        return Ok(ResponseHelper.Success(cartReturnModel));

                    }
                    else
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.PromocodeNotExists));
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

        #region Order Invoice

        /// <summary>
        /// Orders the invoice detail.
        /// </summary>
        /// <param name="OrderInvoiceId">The order invoice identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("order-invoice-detail")]
        public IActionResult OrderInvoiceDetail(GetOrderInvoiceDetailModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string scheme = this.Request.Scheme;
                    scheme += "://" + this.Request.Host + this.Request.PathBase;

                    var orderInvoiceDetail = iOrderInvoice.GetOrderInvoiceDetail(DBHelper.ParseInt64(model.orderInvoiceId));
                    if (orderInvoiceDetail != null)
                    {
                        PromoCode promoCode = null;
                        if (orderInvoiceDetail[0].OrderInvoice.DiscountAmount > 0)
                        {
                            promoCode = iPromoCode.GetPromocodeDetail(DBHelper.ParseInt64(orderInvoiceDetail[0].OrderInvoice.Order.PromocodeFK));
                        }
                        OrderInvoiceDetailModel orderInvoiceDetailList = OrderInvoiceHelper.BindOrderInvoiceDetailModel(orderInvoiceDetail, promoCode);
                        List<long> productIds = OrderInvoiceHelper.GetProductIds(orderInvoiceDetailList);
                        List<long> sizeIds = OrderInvoiceHelper.GetSizeIds(orderInvoiceDetailList);
                        var productCategoryDetails = iProduct.GetProductCategoryDetail(productIds);
                        var productSizeDetail = iProduct.GetProductSizeDetails(productIds);
                        var sizeDetail = iSize.GetSizeDetailsBySizeIds(sizeIds);
                        List<ProductImage> productImages = iProduct.GetProductImagesByProductIds(productIds);
                        orderInvoiceDetailList = OrderInvoiceHelper.BindOrderInvoiceDetailImageToModel(orderInvoiceDetailList, scheme, productImages);
                        orderInvoiceDetailList = OrderInvoiceHelper.BindOrderInvoiceProductCategoryToModel(orderInvoiceDetailList, productCategoryDetails);

                        orderInvoiceDetailList = OrderInvoiceHelper.BindOrderInvoiceProductSizeToModel(orderInvoiceDetailList, productSizeDetail, sizeDetail);
                        return Ok(ResponseHelper.Success(orderInvoiceDetailList));
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

        #endregion
    }
}