namespace SHKang.API.Areas.Admin.Controllers
{
    #region namespaces

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using SHKang.API.Helper;
    using SHKang.Core;
    using SHKang.Core.Constant;
    using SHKang.Core.Enums;
    using SHKang.Core.Helpers;
    using SHKang.Core.ModelHelper;
    using SHKang.Model.Models;
    using SHKang.Model.ViewModels.Admin;
    using SHKang.Model.ViewModels.Web;
    using SHKang.Repository.Interface;
    using Stripe;
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    #endregion

    [Route("api/admin/[controller]")]
    [ApiController]
    [AuthorizeHelper]
    public class OrderInvoiceController : ControllerBase
    {
        #region Private Variables

        /// <summary>
        /// The i order invoice
        /// </summary>
        private readonly IOrderInvoice iOrderInvoice;

        /// <summary>
        /// The env
        /// </summary>
        private readonly IHostingEnvironment env;

        /// <summary>
        /// The i product
        /// </summary>
        private readonly IProduct iProduct;

        /// <summary>
        /// The i order
        /// </summary>
        private readonly IOrder iOrder;

        /// <summary>
        /// The i size
        /// </summary>
        private readonly ISize iSize;

        /// <summary>
        /// The i promo code
        /// </summary>
        private readonly IPromoCode iPromoCode;

        /// <summary>
        /// The configuration
        /// </summary>
        private readonly IConfiguration config;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderInvoiceController" /> class.
        /// </summary>
        /// <param name="_iOrderInvoice">The i order invoice.</param>
        /// <param name="_env">The env.</param>
        /// <param name="_iProduct">The i product.</param>
        /// <param name="_iOrder">The i order.</param>
        /// <param name="_iSize">Size of the i.</param>
        /// <param name="_iPromoCode">The i promo code.</param>
        public OrderInvoiceController(IOrderInvoice _iOrderInvoice, IHostingEnvironment _env, IProduct _iProduct, IOrder _iOrder, ISize _iSize, IPromoCode _iPromoCode, IConfiguration _config)
        {
            iOrderInvoice = _iOrderInvoice;
            env = _env;
            iProduct = _iProduct;
            iOrder = _iOrder;
            iSize = _iSize;
            iPromoCode = _iPromoCode;
            config = _config;
        }
        #endregion

        #region Public Methods

        [HttpPost]
        [Route("update-order-invoice-status")]
        /// <summary>
        /// Orders the list.
        /// </summary>
        /// <param name="OrderId">The order identifier.</param>
        /// <param name="OrderStatusId">The order status identifier.</param>
        /// <returns></returns>
        public IActionResult UpdateOrderInvoiceStatus(UpdateOrderInvoiceStatus model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (iOrderInvoice.UpdateOrderInvoiceStatus(DBHelper.ParseInt64(model.orderInvoiceId), DBHelper.ParseInt64(model.orderStatusId), DBHelper.ParseInt64(model.updatedBy)))
                    {
                        var orderStatus = DBHelper.ParseInt32(model.orderStatusId);
                        if (orderStatus == OrderStatusEnum.ReadyforPayment.GetHashCode())
                        {
                            SendPaymentRelatedEmail(DBHelper.ParseInt32(model.orderInvoiceId));
                        }
                        return Ok(ResponseHelper.Success(MessageConstants.InvoiceStatusUpdated));
                    }
                    else
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.NoOrderFound));
                    }
                }
                return Ok(ResponseHelper.Error(MessageConstants.CompulsoryData));
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                return Ok(ResponseHelper.Error(ex.Message));
            }
        }

        /// <summary>
        /// Adds the order invoice.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("add-order-invoice")]
        public IActionResult AddOrderInvoice(AddOrderInvoiceModel orderInvoiceModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    decimal discountAmount = 0;
                    decimal percentage = 0;
                    long invoiceCount = iOrderInvoice.GetInvoiceCountByOrderId(DBHelper.ParseInt64(orderInvoiceModel.OrderId));
                    if (invoiceCount <= 0)
                    {
                        var orderModel = iOrder.GetOrderModelByOrderId(DBHelper.ParseInt64(orderInvoiceModel.OrderId));
                        if (orderModel != null && orderModel.PromocodeFK.HasValue)
                        {
                            var promoCodeModel = iPromoCode.GetPromocodeDetail(DBHelper.ParseInt64(orderModel.PromocodeFK));
                            if (promoCodeModel != null)
                            {
                                discountAmount = promoCodeModel.Amount;
                                percentage = promoCodeModel.Percentage;
                            }
                        }
                    }
                    var uniqueInvoiceIds = iOrderInvoice.GetAllUniqueInvoiceIds();
                    OrderInvoice orderInvModel = OrderInvoiceHelper.BindOrderInvoice(orderInvoiceModel, uniqueInvoiceIds, discountAmount, percentage);
                    List<OrderInvoiceDetail> invoiceDetailsList = OrderInvoiceHelper.BindOrderInvoiceDetailModel(orderInvoiceModel.productDetail);
                    long orderInvoiceId = iOrderInvoice.AddOrderInvoice(orderInvModel, invoiceDetailsList, orderInvoiceModel.productDetail);
                    if (orderInvoiceId > 0)
                    {
                        if (orderInvModel.OrderStatusFK == OrderStatusEnum.ReadyforPayment.GetHashCode())
                        {
                            SendPaymentRelatedEmail(DBHelper.ParseInt32(orderInvoiceId));
                        }
                        return Ok(ResponseHelper.Success(MessageConstants.OrderInvoiceGenerated));
                    }
                    else
                    {
                        return Ok(ResponseHelper.Error(MessageConstants.OrderInvoiceNotGenerated));
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
                    var header = this.Request.Headers;
                    var authToken = header["Authorization"];
                    var jwt = authToken[0].Replace("Bearer ", "");
                    var handler = new JwtSecurityTokenHandler();
                    var token = handler.ReadJwtToken(jwt);
                    var loginUserId = DBHelper.ParseInt32(token.Payload["UserId"]);

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
                        List<ProductImage> productImages = iProduct.GetProductImagesByProductIds(productIds);
                        orderInvoiceDetailList = OrderInvoiceHelper.BindOrderInvoiceDetailImageToModel(orderInvoiceDetailList, scheme, productImages);
                        var adminUserAddressDetails = iOrderInvoice.GetAdminUserAddressDetails(loginUserId);
                        orderInvoiceDetailList.AdminUserDetails = adminUserAddressDetails;
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

        /// <summary>
        /// Gets the order invoice.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("get-order-invoice")]
        public IActionResult GetOrderInvoice(DeleteOrderModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var header = this.Request.Headers;
                    var authToken = header["Authorization"];
                    var jwt = authToken[0].Replace("Bearer ", "");
                    var handler = new JwtSecurityTokenHandler();
                    var token = handler.ReadJwtToken(jwt);
                    var loginUserId = DBHelper.ParseInt32(token.Payload["UserId"]);

                    string scheme = this.Request.Scheme;
                    scheme += "://" + this.Request.Host + this.Request.PathBase;
                    var orderDetailModel = iOrder.GetOrderDetail(model.OrderId);
                    var orderInvoiceSizeDetail = iOrderInvoice.GetOrderInvoiceSizeDetails(model.OrderId);
                    var productIds = OrderInvoiceHelper.GetProductIds(orderDetailModel);
                    var sizeIds = OrderInvoiceHelper.GetSizeIds(orderDetailModel);
                    var productSizeRatio = iProduct.GetProductSizeDetails(productIds);
                    var productImages = iProduct.GetProductImagesByProductIds(productIds);
                    var sizeDetail = iSize.GetSizeDetailsBySizeIds(sizeIds);
                    var getOrderInvoiceModel = OrderInvoiceHelper.BindGetorderInvoiceDetailModel(orderDetailModel, orderInvoiceSizeDetail, productSizeRatio, scheme, productImages, sizeDetail);
                    var adminUserAddressDetails = iOrderInvoice.GetAdminUserAddressDetails(loginUserId);
                    getOrderInvoiceModel.AdminUserDetails = adminUserAddressDetails;
                    return Ok(ResponseHelper.Success(getOrderInvoiceModel));
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
        [Route("email-order-invoice")]
        public IActionResult EmailOrderInvoice(GetOrderInvoiceDetailModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var header = this.Request.Headers;
                    var authToken = header["Authorization"];
                    var jwt = authToken[0].Replace("Bearer ","");
                    var handler = new JwtSecurityTokenHandler();
                    var token = handler.ReadJwtToken(jwt);
                    var loginUserId = DBHelper.ParseInt32(token.Payload["UserId"]);
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
                        List<ProductImage> productImages = iProduct.GetProductImagesByProductIds(productIds);
                        orderInvoiceDetailList = OrderInvoiceHelper.BindOrderInvoiceDetailImageToModel(orderInvoiceDetailList, scheme, productImages);
                        var adminUserAddressDetails = iOrderInvoice.GetAdminUserAddressDetails(loginUserId);
                        orderInvoiceDetailList.AdminUserDetails = adminUserAddressDetails;
                        if (orderInvoiceDetailList != null)
                        {
                            var HostingPath = env.ContentRootPath;
                            string clientEmail = config.GetValue<string>("app_settings:ClientEmail");
                            string clientEmailPassword = config.GetValue<string>("app_settings:ClientEmailPassword");
                            string port = config.GetValue<string>("app_settings:Port");
                            string mailHost = config.GetValue<string>("app_settings:SMTPURL");
                            string host = this.Request.Host.Value;
                            OrderHelper.SendInvoice(orderInvoiceDetailList.Email, orderInvoiceDetailList, DBHelper.ParseString(HostingPath), clientEmail, clientEmailPassword, port, mailHost, host, scheme);
                        }
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


        #region Private Methods
        /// <summary>
        /// Sends the payment related email.
        /// </summary>
        /// <param name="orderInvoiceId">The order invoice identifier.</param>
        private void SendPaymentRelatedEmail(int orderInvoiceId)
        {
            string email = iOrderInvoice.GetOrderUserEmail(orderInvoiceId);
            var HostingPath = env.ContentRootPath;
            string clientEmail = config.GetValue<string>("app_settings:ClientEmail");
            string clientEmailPassword = config.GetValue<string>("app_settings:ClientEmailPassword");
            string port = config.GetValue<string>("app_settings:Port");
            string mailHost = config.GetValue<string>("app_settings:SMTPURL");
            string returnURL = string.Concat(config.GetValue<string>("app_settings:ReturnURL"), "customer/payment/", orderInvoiceId);
            string logoURL = config.GetValue<string>("app_settings:LogoURL");
            OrderHelper.SendReadyForPaymentEmail(email, DBHelper.ParseString(HostingPath), clientEmail, clientEmailPassword, port, mailHost, returnURL, logoURL);
        }
        #endregion

        #endregion

    }
}