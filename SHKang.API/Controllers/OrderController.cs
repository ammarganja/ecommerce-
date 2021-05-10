namespace SHKang.API.Controllers
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
    #endregion

    [Route("api/[controller]")]
    [ApiController]
    [AuthorizeHelper]
    public class OrderController : ControllerBase
    {
        #region Private Variables

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
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderController"/> class.
        /// </summary>
        /// <param name="_iOrder">The i order.</param>
        /// <param name="_iOrderInvoice">The i order invoice.</param>
        /// <param name="_env">The env.</param>
        /// <param name="_config">The configuration.</param>
        /// <param name="iCart">The i cart.</param>
        public OrderController(IOrder _iOrder, IOrderInvoice _iOrderInvoice, IHostingEnvironment _env, IConfiguration _config, ICart _iCart)
        {
            iOrder = _iOrder;
            iOrderInvoice = _iOrderInvoice;
            env = _env;
            config = _config;
            iCart = _iCart;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Users the order detail.
        /// </summary>
        /// <param name="UserId">The user identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("user-order-detail")]
        public IActionResult UserOrderDetail(DeleteUserModel userModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string scheme = this.Request.Scheme;
                    scheme += "://" + this.Request.Host + this.Request.PathBase;
                    var userOrderlist = iOrder.GetUserOrderList(DBHelper.ParseInt64(userModel.userId), scheme);
                    if (userOrderlist != null)
                    { 
                        return Ok(ResponseHelper.Success(userOrderlist));
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
        /// Tracks the order.
        /// </summary>
        /// <param name="OrderInvoiceId">The order invoice identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("track-order")]
        public IActionResult TrackOrder(GetOrderInvoiceDetailModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var orderStatus = iOrder.GetInvoiceStatusByInvoiceId(DBHelper.ParseInt64(model.orderInvoiceId));
                    if (orderStatus > 0)
                    {
                        if (orderStatus != OrderStatusEnum.Cancelled.GetHashCode())
                        {
                            List<SelectListItemModel> orderStatusList = new List<SelectListItemModel>();
                            int limit = DBHelper.ParseInt32(orderStatus);
                            for (int i = 1; i <= limit; i++)
                            {
                                var status = (OrderStatusEnum)i;
                                orderStatusList.Add(new SelectListItemModel
                                {
                                    itemName = DBHelper.ParseString(status),
                                    id = DBHelper.ParseString(i)
                                });
                            }
                            return Ok(ResponseHelper.Success(orderStatusList));
                        }
                        else
                        {
                            var value = (OrderStatusEnum)orderStatus;
                            return Ok(ResponseHelper.Success(DBHelper.ParseString(value)));
                        }
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


        [HttpPost]
        [Route("process-invoice-payment")]
        /// <summary>
        /// Orders the list.
        /// </summary>
        /// <param name="OrderId">The order identifier.</param>
        /// <param name="OrderStatusId">The order status identifier.</param>
        /// <returns></returns>
        public IActionResult ProcessInvoicePayment(OrderInvoicePayment model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    StripeConfiguration.SetApiKey(config.GetValue<string>("app_settings:StripePublishKey"));
                    StripeConfiguration.ApiKey = config.GetValue<string>("app_settings:StripeSecreteKey");

                    AdminUserDetails adminUserDetails = iOrderInvoice.GetUserAddress(model.LoggedInUserId, model.OrderInvoiceId);
                    var customers = new CustomerService();
                    var charges = new ChargeService();

                    var customer = customers.Create(new CustomerCreateOptions
                    {
                        Email = model.StripeEmail,
                        Source = model.StripeToken,
                        Name = adminUserDetails.FirstName,
                        Address = new AddressOptions
                        {
                            Line1 = adminUserDetails.Address1,
                            Line2 = adminUserDetails.Address2,
                            PostalCode = adminUserDetails.Zipcode,
                            City = adminUserDetails.City,
                            State = adminUserDetails.StateName,
                            Country = adminUserDetails.CountryName
                        },
                    });

                    var charge = charges.Create(new ChargeCreateOptions
                    {
                        Amount = DBHelper.ParseInt64(model.Amount),
                        Description = string.Concat("Payment by ", adminUserDetails.FirstName, " For InvoiceId ", model.OrderInvoiceId),
                        Currency = "usd",
                        Customer = customer.Id,
                        Shipping = new ChargeShippingOptions {
                            Name = adminUserDetails.FirstName,
                            Phone = adminUserDetails.PhoneNumber,
                            Address = new AddressOptions
                            {
                                Line1 = adminUserDetails.Address1,
                                Line2 = adminUserDetails.Address2,
                                PostalCode = adminUserDetails.Zipcode,
                                City = adminUserDetails.City,
                                State = adminUserDetails.StateName,
                                Country = adminUserDetails.CountryName
                            }
                        }
                    });

                    iOrderInvoice.UpdateInvoiceStripeDetails(model.OrderInvoiceId, customer.Id, charge.Id);

                    return Ok(ResponseHelper.Success(MessageConstants.PaymentSuccess));
                }
                return Ok(ResponseHelper.Error(MessageConstants.CompulsoryData));
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
