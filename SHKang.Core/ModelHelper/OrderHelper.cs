namespace SHKang.Core.ModelHelper
{
    #region namespaces
    using SHKang.Core.Constant;
    using SHKang.Core.Helpers;
    using SHKang.Model.Models;
    using SHKang.Model.ViewModels.Admin;
    using SHKang.Model.ViewModels.Web;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    #endregion

    public static class OrderHelper
    {

        /// <summary>
        /// Converts to order detail model.
        /// </summary>
        /// <param name="orderDetail">The model.</param>
        /// <returns></returns>
        public static OrderDetailModel BindOrderDetailModel(List<OrderDetail> orderDetail, List<OrderInvoiceDetail> invoiceModel)
        {
            OrderDetailModel orderDetailModel = new OrderDetailModel();
            if (orderDetail != null)
            {
                orderDetailModel.Address1 = orderDetail[0].Order.UserAddress.Address1;
                orderDetailModel.Address2 = orderDetail[0].Order.UserAddress.Address2;
                orderDetailModel.City = orderDetail[0].Order.UserAddress.City;
                orderDetailModel.StateId = DBHelper.ParseString(orderDetail[0].Order.UserAddress.State.StateId);
                orderDetailModel.State = orderDetail[0].Order.UserAddress.State.Name;
                orderDetailModel.Country = orderDetail[0].Order.UserAddress.Country.Name;
                orderDetailModel.CountryId = DBHelper.ParseString(orderDetail[0].Order.UserAddress.Country.CountryId);
                orderDetailModel.CompanyName = orderDetail[0].Order.User.Companyname;
                orderDetailModel.ZipCode = orderDetail[0].Order.UserAddress.Zipcode;
                orderDetailModel.EmailId = orderDetail[0].Order.User.EmailAddress;
                orderDetailModel.MobileNumber = DBHelper.ParseString(orderDetail[0].Order.User.PhoneNumber);
                orderDetailModel.FirstName = orderDetail[0].Order.User.FirstName;
                orderDetailModel.LastName = orderDetail[0].Order.User.LastName;
                orderDetailModel.OrderDate = DBHelper.ParseString(Convert.ToDateTime(orderDetail[0].Order.OrderDate)).Split(' ')[0];
                orderDetailModel.PayableAmount = DBHelper.ParseString(orderDetail[0].Order.PayableAmount);
                orderDetailModel.PoNumber = DBHelper.ParseString(orderDetail[0].Order.PONumber);
                orderDetailModel.SubTotalAmount = DBHelper.ParseString(orderDetail[0].Order.SubTotalAmount);
                orderDetailModel.UniqueOrderId = DBHelper.ParseString(orderDetail[0].Order.UniqueOrderId);
                orderDetailModel.UserAddressId = DBHelper.ParseString(orderDetail[0].Order.UserAddressFK);
                orderDetailModel.UserId = DBHelper.ParseString(orderDetail[0].Order.UserFK);

                List<OrderDetailsProductModel> orderDetailsProductModelList = new List<OrderDetailsProductModel>();
                foreach (var item in orderDetail)
                {
                    long quantity = item.Quantity;
                    if (invoiceModel != null)
                    {
                        var product = invoiceModel.Where(x => x.ProductFK == item.ProductFK).ToList();
                        if (product != null)
                        {
                            foreach (var pro in product)
                            {
                                quantity = quantity - DBHelper.ParseInt64(pro.Quantity);
                            }
                        }
                    }
                    orderDetailsProductModelList.Add(new OrderDetailsProductModel
                    {
                        Color = item.ProductColor.Color.Name,
                        ColorId = DBHelper.ParseString(item.ProductColor.Color.ColorId),
                        Name = item.Product.Name,
                        Price = DBHelper.ParseString(item.Price),
                        ProductId = DBHelper.ParseString(item.ProductFK),
                        Quantity = DBHelper.ParseString(quantity),
                        //Size = item.Product.Size.Name,
                        //SizeId = DBHelper.ParseString(item.Product.Size.SizeId),
                        //SizeRatio = DBHelper.ParseString(item.Product.SizeRatio.Name),
                        //SizeRatioId = DBHelper.ParseString(item.Product.SizeRatio.SizeRatioId),
                        SubTotalAmount = DBHelper.ParseString((item.Price * DBHelper.ParseDecimal(quantity)))
                    });
                }
                orderDetailModel.ProductDetails = orderDetailsProductModelList;
            }
            return orderDetailModel;
        }

        /// <summary>
        /// Converts to order model.
        /// </summary>
        /// <param name="addNewOrderModel">The model.</param>
        /// <returns></returns>
        public static Order BindOrderModel(AddNewOrderModel addNewOrderModel, List<string> poNumber, List<string> uniqueOrderIds, long promoCodeId = 0, decimal percentage = 0, decimal discountAmount = 0)
        {
            Order orderModel = new Order();
            if (addNewOrderModel != null)
            {
                orderModel.UserFK = DBHelper.ParseInt64(addNewOrderModel.UserId);
                orderModel.UserAddressFK = DBHelper.ParseInt64(addNewOrderModel.UserAddressId);
                orderModel.UserFK = DBHelper.ParseInt64(addNewOrderModel.UserId);
                orderModel.SubTotalAmount = GetSubTotalAmount(addNewOrderModel.ProductDetail);
                orderModel.DiscountAmount = 0;
                if (promoCodeId > 0)
                {
                    orderModel.PromocodeFK = promoCodeId;
                }
                if (percentage > 0)
                {
                    orderModel.DiscountAmount = orderModel.SubTotalAmount * percentage / 100;
                }
                else if (discountAmount > 0)
                {
                    orderModel.DiscountAmount = discountAmount;
                }
                orderModel.PayableAmount = orderModel.SubTotalAmount - orderModel.DiscountAmount;
                if (!string.IsNullOrWhiteSpace(addNewOrderModel.OrderId) && DBHelper.ParseInt64(addNewOrderModel.OrderId) > 0)
                {
                    orderModel.OrderId = DBHelper.ParseInt64(addNewOrderModel.OrderId);
                    orderModel.ModifiedOn = DateTime.Now;
                    orderModel.UpdatedBy = DBHelper.ParseInt64(addNewOrderModel.CreatedBy);
                }
                else
                {
                    orderModel.CreatedBy = DBHelper.ParseInt64(addNewOrderModel.CreatedBy);
                    orderModel.CreatedOn = DateTime.Now;
                    orderModel.OrderDate = DateTime.Now;
                    orderModel.PONumber = CheckPoNumber(poNumber);
                    orderModel.UniqueOrderId = CheckOrderId(uniqueOrderIds);

                }
            }
            return orderModel;
        }

        /// <summary>
        /// Converts to order detail model.
        /// </summary>
        /// <param name="addNewOrderModel">The model.</param>
        /// <returns></returns>
        public static List<OrderDetail> BindOrderDetailModel(List<OrderProductDetail> addNewOrderModel)
        {
            List<OrderDetail> orderDetailList = new List<OrderDetail>();
            if (addNewOrderModel != null && addNewOrderModel.Count > 0)
            {
                foreach (var item in addNewOrderModel)
                {
                    orderDetailList.Add(new OrderDetail
                    {
                        ProductFK = DBHelper.ParseInt64(item.ProductId),
                        ProductColorFK = DBHelper.ParseInt64(item.ProductColorId),
                        Price = DBHelper.ParseDecimal(item.Price),
                        Quantity = DBHelper.ParseInt64(item.Quantity),
                        SubTotalAmount = DBHelper.ParseDecimal(DBHelper.ParseDecimal(item.Price) * DBHelper.ParseInt64(item.Quantity))
                    });
                }

            }
            return orderDetailList;
        }

        /// <summary>
        /// Checks the invoice identifier.
        /// </summary>
        /// <returns></returns>
        public static string CheckOrderId(List<string> orderIdsList)
        {
            string orderIds = RandomHelpers.GetUniqueOrderId();
            if (orderIdsList != null)
            {
            againCheck:
                var check = orderIdsList.Contains(orderIds);
                if (check)
                {
                    orderIds = RandomHelpers.GetUniqueOrderId();
                    goto againCheck;
                }
                else
                {
                    return orderIds;
                }
            }
            else
            {
                return orderIds;
            }
        }

        /// <summary>
        /// Checks the po number.
        /// </summary>
        /// <returns></returns>
        public static string CheckPoNumber(List<string> poNumbersList)
        {
            string poNumbers = RandomHelpers.GetUniquePONumber();
            if (poNumbersList != null)
            {
            againCheck:
                var check = poNumbersList.Contains(poNumbers);
                if (check)
                {
                    poNumbers = RandomHelpers.GetUniquePONumber();
                    goto againCheck;
                }
                else
                {
                    return poNumbers;
                }
            }
            else
            {
                return poNumbers;
            }
        }

        /// <summary>
        /// Gets the sub total amount.
        /// </summary>
        /// <param name="price">The price.</param>
        /// <param name="Quantity">The quantity.</param>
        /// <returns></returns>
        public static decimal GetSubTotalAmount(List<OrderProductDetail> addNewOrderModel)
        {
            decimal subTotalAmount = 0;
            if (addNewOrderModel != null && addNewOrderModel.Count > 0)
            {
                foreach (var item in addNewOrderModel)
                {
                    decimal Total = DBHelper.ParseDecimal(item.Price) * DBHelper.ParseDecimal(item.Quantity);
                    subTotalAmount = subTotalAmount + Total;
                }
            }

            return subTotalAmount;
        }

        /// <summary>
        /// Sends the reset password mail.
        /// </summary>
        /// <param name="toMail">To mail.</param>
        /// <param name="orderDetailModel">The model.</param>
        /// <returns></returns>
        public static async Task SendResetPasswordMail(string toMail, OrderDetailModel orderDetailModel, string hostPath, string clientEmail, string clientPassword, string port, string sMTPUrl, string host, string scheme)
        {
            await Task.Run(() =>
            {
                var hostingPath = hostPath;
                string path = string.Concat(hostingPath, "/" + Constants.OrderConfirmationMail);
                if (System.IO.File.Exists(path))
                {
                    string _clientEmail = clientEmail;
                    string clientEmailPassword = clientPassword;
                    string _port = port;
                    string mailHost = sMTPUrl;
                    string _host = host;
                    string _scheme = scheme;
                    string html = GetOrderConfirmationMailString(orderDetailModel, hostingPath);
                    MailHelper.SendMail(toMail, Constants.OrderConfirmationSubject, html, _clientEmail, mailHost, _port, clientEmailPassword);
                }
            });

        }

        /// <summary>
        /// Gets the order confirmation string.
        /// </summary>
        /// <param name="orderDetailModel">The model.</param>
        /// <returns></returns>
        public static string GetOrderConfirmationMailString(OrderDetailModel orderDetailModel, string hostPath)
        {
            var hostingPath = hostPath;
            string path = string.Concat(hostingPath, "/" + Constants.OrderConfirmationMail);
            string html = string.Empty;
            string tablepath = string.Concat(hostingPath, "/" + Constants.OrderProductTableMail);
            if (System.IO.File.Exists(path))
            {
                html = System.IO.File.ReadAllText(path);
                html = html.Replace("###UserName###!", orderDetailModel.FirstName + " !");
                html = html.Replace("###Address1###", orderDetailModel.Address1);
                html = html.Replace("###Address2###", orderDetailModel.Address2);
                html = html.Replace("###City###", orderDetailModel.City);
                html = html.Replace("###Country###", orderDetailModel.Country);
                html = html.Replace("###State###", orderDetailModel.State);
                html = html.Replace("###Zipcode###", orderDetailModel.ZipCode);
                html = html.Replace("###OrderId###", orderDetailModel.UniqueOrderId);
                html = html.Replace("###OrderDate###", orderDetailModel.OrderDate);
                html = html.Replace("###OrderTotal###", orderDetailModel.PayableAmount);
                string FinalTable = string.Empty;
                foreach (var item in orderDetailModel.ProductDetails)
                {
                    string ProductTable = System.IO.File.ReadAllText(tablepath);
                    ProductTable = ProductTable.Replace("###ProductImage###", "https://upload.wikimedia.org/wikipedia/commons/0/0a/No-image-available.png");
                    ProductTable = ProductTable.Replace("###ProductName###", item.Name);
                    ProductTable = ProductTable.Replace("###ProductColor###", item.Color);
                    ProductTable = ProductTable.Replace("###Quantity###", item.Quantity);
                    ProductTable = ProductTable.Replace("###Subtotal###", item.SubTotalAmount);
                    FinalTable += ProductTable;
                }
                html = html.Replace("###ProductTable###", FinalTable);
            }
            return html;
        }

        public static async Task SendInvoice(string toMail, OrderInvoiceDetailModel orderDetailModel, string hostPath, string clientEmail, string clientPassword, string port, string sMTPUrl, string host, string scheme)
        {
            await Task.Run(() =>
            {
                var hostingPath = hostPath;
                string path = string.Concat(hostingPath, "/" + Constants.OrderConfirmationMail);
                if (System.IO.File.Exists(path))
                {
                    string _clientEmail = clientEmail;
                    string clientEmailPassword = clientPassword;
                    string _port = port;
                    string mailHost = sMTPUrl;
                    string _host = host;
                    string _scheme = scheme;
                    string html = GetOrderInvoiceMailString(orderDetailModel, hostingPath);
                    MailHelper.SendMail(toMail, Constants.InvoiceDetail, html, _clientEmail, mailHost, _port, clientEmailPassword);
                }
            });

        }

        private static string GetOrderInvoiceMailString(OrderInvoiceDetailModel orderInvoiceDetailModel, string hostPath)
        {
            var hostingPath = hostPath;
            string path = string.Concat(hostingPath, "/" + Constants.InvoiceDetailPath);
            string logoPath = string.Concat(hostingPath, "/" + Constants.LogoPath);
            string html = string.Empty;
            string tablepath = string.Concat(hostingPath, "/" + Constants.InvoiceProductsPath);
            if (System.IO.File.Exists(path))
            {
                html = System.IO.File.ReadAllText(path);
                html = html.Replace("##COMPANYLOGO##", logoPath);
                html = html.Replace("##ADMIN_USERNAME##", orderInvoiceDetailModel.AdminUserDetails.FirstName);
                html = html.Replace("##ADMIN_Address1##", orderInvoiceDetailModel.AdminUserDetails.Address1);
                html = html.Replace("##ADMIN_Address2##", orderInvoiceDetailModel.AdminUserDetails.Address2);
                html = html.Replace("##ADMIN_User_COUNTRY##", orderInvoiceDetailModel.AdminUserDetails.CountryName);
                html = html.Replace("##ADMIN_User_CITY##", orderInvoiceDetailModel.AdminUserDetails.City);
                html = html.Replace("##ADMIN_USER_STATE##", orderInvoiceDetailModel.AdminUserDetails.StateName);
                html = html.Replace("##ADMIN_USER_ZIPCODE##", orderInvoiceDetailModel.AdminUserDetails.Zipcode);
                html = html.Replace("##SHIPPING_CHARGE##", orderInvoiceDetailModel.ShippingCharges);
                html = html.Replace("##PRODUCT_VAT##", orderInvoiceDetailModel.TotalGSTAmount);

                html = html.Replace("##USERNAME##", orderInvoiceDetailModel.FirstName + " !");
                html = html.Replace("##ADDRESS1##", orderInvoiceDetailModel.Address1);
                if(!string.IsNullOrEmpty(orderInvoiceDetailModel.Address2))
                {
                    html = html.Replace("##ADDRESS2##", orderInvoiceDetailModel.Address2);
                }
                
                html = html.Replace("##CITY##", orderInvoiceDetailModel.City);
                html = html.Replace("##COUNTRY##", orderInvoiceDetailModel.Country);
                html = html.Replace("##STATE##", orderInvoiceDetailModel.State);
                html = html.Replace("##ZIPCODE##", orderInvoiceDetailModel.Zipcode);
                //string convertDate = DateTime.ParseExact(orderInvoiceDetailModel.OrderDate, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                orderInvoiceDetailModel.OrderDate = DBHelper.ParseString(Convert.ToDateTime(orderInvoiceDetailModel.OrderDate).ToString(Constants.DateFormat));
                html = html.Replace("##ORDER_DATE##", orderInvoiceDetailModel.OrderDate);
                html = html.Replace("##ORDER_NUMBER##", orderInvoiceDetailModel.PONumber);
                
                html = html.Replace("##PAYBALE_AMOUNT##", orderInvoiceDetailModel.PayableAmount);
                html = html.Replace("##ORDER_STATUS##", orderInvoiceDetailModel.OrderStatus);
                string FinalTable = string.Empty;
                foreach (var item in orderInvoiceDetailModel.ProductDetails)
                {
                    string ProductTable = System.IO.File.ReadAllText(tablepath);
                    ProductTable = ProductTable.Replace("##PRODUCT_IMAGE##", item.Image);
                    ProductTable = ProductTable.Replace("##PRODUCT_NAME##", item.Name);
                    ProductTable = ProductTable.Replace("##PRODUCT_GROUP##", item.SizeRatio);
                    ProductTable = ProductTable.Replace("##PRODUCT_QTY##", item.Quantity);
                    ProductTable = ProductTable.Replace("##PRODUCT_UNITPRICE##", item.Price);
                    ProductTable = ProductTable.Replace("##PRODUCT_TOTAL##", item.Total);
                    FinalTable += ProductTable;
                }
                html = html.Replace("###ProductTable###", FinalTable);
            }
            return html;
        }

        public static async Task SendReadyForPaymentEmail(string toMail, string hosting, string clientEmail, string clientPassword, string port, string sMTPUrl,string returnURL,string logoURL)
        {
            await Task.Run(() =>
            {
                var webRoot = hosting;
                string path = string.Concat(webRoot, @"\" + Constants.ReadyForPaymentPath);

                if (System.IO.File.Exists(path))
                {
                    string html = System.IO.File.ReadAllText(path);
                    string _clientEmail = clientEmail;
                    string clientEmailPassword = clientPassword;
                    string _port = port;
                    string mailHost = sMTPUrl;
                    html = html.Replace("##RETURNURL##", returnURL);
                    html = html.Replace("##HEADERLOGO##", logoURL);
                    MailHelper.SendMail(toMail, Constants.ReadyForPaymentSubject, html, _clientEmail, mailHost, _port, clientEmailPassword);
                }
                else
                {
                    LogHelper.ExceptionLog("Ready For Payment Template Path Not Found " + path);
                }
            });
        }
    }

}
