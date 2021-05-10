namespace SHKang.Core.ModelHelper
{
    using SHKang.Core.Constant;
    #region namespaces
    using SHKang.Core.Enums;
    using SHKang.Core.Helpers;
    using SHKang.Model.Models;
    using SHKang.Model.ViewModels.Admin;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion

    public static class OrderInvoiceHelper
    {
        /// <summary>
        /// Converts to order invoice.
        /// </summary>
        /// <param name="addOrderInvoiceModel">The model.</param>
        /// <returns></returns>
        public static OrderInvoice BindOrderInvoice(AddOrderInvoiceModel addOrderInvoiceModel, List<string> invoiceIds, decimal discountAmount = 0, decimal percentage = 0)
        {
            OrderInvoice orderInvoiceModel = new OrderInvoice();
            if (addOrderInvoiceModel != null)
            {
                orderInvoiceModel.OrderFK = DBHelper.ParseInt64(addOrderInvoiceModel.OrderId);
                orderInvoiceModel.CreatedOn = DateTime.Now;
                orderInvoiceModel.CreatedBy = DBHelper.ParseInt64(addOrderInvoiceModel.CreatedBy);
                orderInvoiceModel.UniqueInvoiceId = CheckInvoiceId(invoiceIds);
                orderInvoiceModel.InvoiceDate = DateTime.Now;
                orderInvoiceModel.UserFK = DBHelper.ParseInt64(addOrderInvoiceModel.UserId);
                orderInvoiceModel.SubTotalAmount = GetSubTotalAmount(addOrderInvoiceModel.productDetail);
                orderInvoiceModel.TotalGSTAmount = (orderInvoiceModel.SubTotalAmount * DBHelper.ParseDecimal(addOrderInvoiceModel.VatCharges)) / 100;
                orderInvoiceModel.DiscountAmount = 0;
                decimal total = orderInvoiceModel.SubTotalAmount + orderInvoiceModel.TotalGSTAmount + DBHelper.ParseDecimal(addOrderInvoiceModel.ShippingCharges);
                if (discountAmount > 0)
                {
                    orderInvoiceModel.DiscountAmount = discountAmount;
                }
                else if (percentage > 0)
                {
                    orderInvoiceModel.DiscountAmount = total * percentage / 100;
                }
                orderInvoiceModel.PayableAmount = total - orderInvoiceModel.DiscountAmount;
                orderInvoiceModel.ShippingCharges = DBHelper.ParseDecimal(addOrderInvoiceModel.ShippingCharges);
                orderInvoiceModel.IsInvoiceGenerated = true;
                orderInvoiceModel.OrderStatusFK = (long)OrderStatusEnum.ReadyforPayment;
                orderInvoiceModel.TrackingNumber = addOrderInvoiceModel.TrackingNumber;
            }
            return orderInvoiceModel;
        }

        /// <summary>
        /// Converts to order invoice detail model.
        /// </summary>
        /// <param name="addOrderInvoiceProductModel">The model.</param>
        /// <returns></returns>
        public static List<OrderInvoiceDetail> BindOrderInvoiceDetailModel(List<AddOrderInvoiceProductDetail> addOrderInvoiceProductModel)
        {
            List<OrderInvoiceDetail> orderInvoiceDetailList = new List<OrderInvoiceDetail>();
            if (addOrderInvoiceProductModel != null && addOrderInvoiceProductModel.Count > 0)
            {
                for (int i = 0; i < addOrderInvoiceProductModel.Count; i++)
                {
                    //decimal price = 0;
                    //int ratio = 0;
                    //var size = sizeDetailModel.Where(x => x.ProductFK == DBHelper.ParseInt64(addOrderInvoiceProductModel[i].ProductId)).ToList();
                    //if (size != null && size.Count > 0)
                    //{
                    //    foreach (var item in size)
                    //    {
                    //        ratio = ratio + DBHelper.ParseInt32(item.Name);
                    //    }
                    //}
                    //price = DBHelper.ParseDecimal(addOrderInvoiceProductModel[i].Price) / ratio;
                    //price = DBHelper.RoundOff(DBHelper.ParseDecimal(price));
                    orderInvoiceDetailList.Add(new OrderInvoiceDetail
                    {
                        ProductFK = DBHelper.ParseInt64(addOrderInvoiceProductModel[i].ProductId),
                        ProductColorFK = DBHelper.ParseInt64(addOrderInvoiceProductModel[i].ProductColorId),
                        Price = DBHelper.ParseDecimal(addOrderInvoiceProductModel[i].Price),
                        Quantity = DBHelper.ParseDecimal(addOrderInvoiceProductModel[i].Quantity),
                        SubTotalAmount = DBHelper.ParseDecimal(DBHelper.ParseDecimal(addOrderInvoiceProductModel[i].Price) * DBHelper.ParseDecimal(addOrderInvoiceProductModel[i].Quantity))
                    });
                }
            }
            return orderInvoiceDetailList;
        }

        /// <summary>
        /// Binds the product ids.
        /// </summary>
        /// <param name="addOrderInvoiceProductModel">The add order invoice product model.</param>
        /// <returns></returns>
        public static List<long> BindProductIds(List<AddOrderInvoiceProductDetail> addOrderInvoiceProductModel)
        {
            List<long> productIds = new List<long>();
            if (addOrderInvoiceProductModel != null && addOrderInvoiceProductModel.Count > 0)
            {
                for (int i = 0; i < addOrderInvoiceProductModel.Count; i++)
                {
                    productIds.Add(DBHelper.ParseInt64(addOrderInvoiceProductModel[i].ProductId));

                }
            }
            return productIds;
        }

        /// <summary>
        /// Gets the sub total amount.
        /// </summary>
        /// <param name="price">The price.</param>
        /// <returns></returns>
        public static decimal GetSubTotalAmount(List<AddOrderInvoiceProductDetail> productDetailsList)
        {
            decimal subTotalAmount = 0;
            if (productDetailsList != null && productDetailsList.Count > 0)
            {
                for (int i = 0; i < productDetailsList.Count; i++)
                {
                    decimal Total = DBHelper.ParseDecimal(productDetailsList[i].Price) * DBHelper.ParseDecimal(productDetailsList[i].Quantity);
                    subTotalAmount = subTotalAmount + Total;
                }
            }

            return subTotalAmount;
        }

        /// <summary>
        /// Converts to order invoice detail model.
        /// </summary>
        /// <param name="orderInvoiceDetail">The model.</param>
        /// <returns></returns>
        public static OrderInvoiceDetailModel BindOrderInvoiceDetailModel(List<OrderInvoiceDetail> orderInvoiceDetail, PromoCode promoCode = null)
        {
            OrderInvoiceDetailModel orderInvoiceDetailModel = new OrderInvoiceDetailModel();
            if (orderInvoiceDetail != null)
            {
                orderInvoiceDetailModel.OrderDate = DBHelper.ParseString(Convert.ToDateTime(orderInvoiceDetail[0].OrderInvoice.Order.OrderDate));
                orderInvoiceDetailModel.Address1 = orderInvoiceDetail[0].OrderInvoice.Order.UserAddress.Address1;
                orderInvoiceDetailModel.Address2 = orderInvoiceDetail[0].OrderInvoice.Order.UserAddress.Address2;
                orderInvoiceDetailModel.City = orderInvoiceDetail[0].OrderInvoice.Order.UserAddress.City;
                orderInvoiceDetailModel.State = orderInvoiceDetail[0].OrderInvoice.Order.UserAddress.State.Name;
                orderInvoiceDetailModel.Country = orderInvoiceDetail[0].OrderInvoice.Order.UserAddress.Country.Name;
                orderInvoiceDetailModel.CompanyName = orderInvoiceDetail[0].OrderInvoice.User.Companyname;
                orderInvoiceDetailModel.FirstName = orderInvoiceDetail[0].OrderInvoice.User.FirstName;
                orderInvoiceDetailModel.LastName = orderInvoiceDetail[0].OrderInvoice.User.LastName;
                orderInvoiceDetailModel.Email = orderInvoiceDetail[0].OrderInvoice.User.EmailAddress;
                orderInvoiceDetailModel.InvoiceDate = DBHelper.ParseString(Convert.ToDateTime(orderInvoiceDetail[0].OrderInvoice.InvoiceDate)).Split(' ')[0];
                orderInvoiceDetailModel.InvoiceId = orderInvoiceDetail[0].OrderInvoice.UniqueInvoiceId;
                orderInvoiceDetailModel.OrderStatus = orderInvoiceDetail[0].OrderInvoice.OrderStatus.Status;
                orderInvoiceDetailModel.PayableAmount = DBHelper.ParseString(orderInvoiceDetail[0].OrderInvoice.PayableAmount);
                orderInvoiceDetailModel.PONumber = orderInvoiceDetail[0].OrderInvoice.Order.PONumber;
                orderInvoiceDetailModel.ShippingCharges = DBHelper.ParseString(orderInvoiceDetail[0].OrderInvoice.ShippingCharges);
                orderInvoiceDetailModel.SubTotalAmount = DBHelper.ParseString(orderInvoiceDetail[0].OrderInvoice.SubTotalAmount);
                orderInvoiceDetailModel.DiscountAmount = DBHelper.ParseString(orderInvoiceDetail[0].OrderInvoice.DiscountAmount);
                if (orderInvoiceDetail[0].OrderInvoice.TotalGSTAmount != null)
                    orderInvoiceDetailModel.TotalGSTAmount = DBHelper.ParseString(orderInvoiceDetail[0].OrderInvoice.TotalGSTAmount);
                if (orderInvoiceDetail[0].OrderInvoice.TrackingNumber != null)
                    orderInvoiceDetailModel.TrackingNumber = orderInvoiceDetail[0].OrderInvoice.TrackingNumber;
                orderInvoiceDetailModel.UserId = DBHelper.ParseString(orderInvoiceDetail[0].OrderInvoice.User.UserId);
                orderInvoiceDetailModel.Zipcode = orderInvoiceDetail[0].OrderInvoice.Order.UserAddress.Zipcode;
                if (promoCode != null)
                {
                    if (promoCode.Amount > 0)
                    {
                        orderInvoiceDetailModel.Discount = "Flat " + promoCode.Amount + " Off";
                    }
                    else
                    {
                        orderInvoiceDetailModel.Discount = promoCode.Percentage + "% Off";
                    }
                    orderInvoiceDetailModel.PromoCode = promoCode.Code;
                }
                List<OrderInvoiceDetailProduct> orderInvoiceDetailProductList = new List<OrderInvoiceDetailProduct>();
                foreach (var item in orderInvoiceDetail)
                {
                    orderInvoiceDetailProductList.Add(new OrderInvoiceDetailProduct
                    {
                        Color = item.ProductColor.Color.Name,
                        Name = item.Product.Name,
                        Price = DBHelper.ParseString(item.Price),
                        ProductId = DBHelper.ParseString(item.ProductFK),
                        Quantity = DBHelper.ParseString(item.Quantity),
                        Total = DBHelper.ParseString(item.SubTotalAmount),
                        ColorId = DBHelper.ParseString(item.ProductColorFK),
                        SizeId= DBHelper.ParseString(item.Product.SizeFK),
                    });
                }
                orderInvoiceDetailModel.ProductDetails = orderInvoiceDetailProductList;
            }
            return orderInvoiceDetailModel;
        }

        /// <summary>
        /// Checks the invoice identifier.
        /// </summary>
        /// <returns></returns>
        public static string CheckInvoiceId(List<string> invoiceIds)
        {
            string uniqId = RandomHelpers.GetUniqueOrderInvoiceId();
            if (invoiceIds != null)
            {
            againCheck:
                var check = invoiceIds.Contains(uniqId);
                if (check)
                {
                    uniqId = RandomHelpers.GetUniqueOrderInvoiceId();
                    goto againCheck;
                }
                else
                {
                    return uniqId;
                }
            }
            else
            {
                return uniqId;
            }
        }

        /// <summary>
        /// Gets the product ids.
        /// </summary>
        /// <param name="orderInvoiceDetail">The order invoice detail.</param>
        /// <returns></returns>
        public static List<long> GetProductIds(OrderInvoiceDetailModel orderInvoiceDetail)
        {
            List<long> productIds = new List<long>();
            if (orderInvoiceDetail != null)
            {
                foreach (var item in orderInvoiceDetail.ProductDetails)
                {
                    productIds.Add(DBHelper.ParseInt64(item.ProductId));
                }
            }
            return productIds;
        }

        /// <summary>
        /// Gets the size ids.
        /// </summary>
        /// <param name="orderInvoiceDetail">The order invoice detail.</param>
        /// <returns></returns>
        public static List<long> GetSizeIds(OrderInvoiceDetailModel orderInvoiceDetail)
        {
            List<long> sizeIds = new List<long>();
            if (orderInvoiceDetail != null)
            {
                foreach (var item in orderInvoiceDetail.ProductDetails)
                {
                    sizeIds.Add(DBHelper.ParseInt64(item.SizeId));
                }
            }
            return sizeIds;
        }

        /// <summary>
        /// Binds the order invoice detail image to model.
        /// </summary>
        /// <param name="orderInvoiceDetail">The order invoice detail.</param>
        /// <param name="scheme">The scheme.</param>
        /// <param name="productImageModel">The product image model.</param>
        /// <returns></returns>
        public static OrderInvoiceDetailModel BindOrderInvoiceDetailImageToModel(OrderInvoiceDetailModel orderInvoiceDetail, string scheme, List<ProductImage> productImageModel)
        {
            if (orderInvoiceDetail != null)
            {
                foreach (var item in orderInvoiceDetail.ProductDetails)
                {
                    var image = productImageModel.Where(x => x.ProductColor.ProductFK == DBHelper.ParseInt64(item.ProductId) && x.ProductColorFK == DBHelper.ParseInt64(item.ColorId)).FirstOrDefault();
                    if (image != null)
                    {
                        item.Image = scheme + Constants.ProductImagesPath.Replace(@"\", "/") + "/" + image.Image;
                    }
                }
            }
            return orderInvoiceDetail;
        }

        /// <summary>
        /// Gets the product ids.
        /// </summary>
        /// <param name="orderDetailModel">The order detail model.</param>
        /// <returns></returns>
        public static List<long> GetProductIds(List<OrderDetail> orderDetailModel)
        {
            List<long> productIds = new List<long>();
            if (orderDetailModel != null && orderDetailModel.Count > 0)
            {
                foreach (var item in orderDetailModel)
                {
                    productIds.Add(item.ProductFK);
                }
            }
            return productIds;
        }

        /// <summary>
        /// Gets the size ids.
        /// </summary>
        /// <param name="orderDetailModel">The order detail model.</param>
        /// <returns></returns>
        public static List<long> GetSizeIds(List<OrderDetail> orderDetailModel)
        {
            List<long> sizeIds = new List<long>();
            if (orderDetailModel != null && orderDetailModel.Count > 0)
            {
                foreach (var item in orderDetailModel)
                {
                    sizeIds.Add(item.Product.SizeFK);
                }
            }
            return sizeIds;
        }

        /// <summary>
        /// Binds the getorder invoice detail model.
        /// </summary>
        /// <param name="orderDetailModel">The order detail model.</param>
        /// <param name="orderInvoiceSizeDetailModel">The order invoice size detail model.</param>
        /// <param name="productSizeDetailsModel">The product size details model.</param>
        /// <returns></returns>
        public static GetOrderInvoiceDetail BindGetorderInvoiceDetailModel(List<OrderDetail> orderDetailModel, List<OrderInvoiceSizeDetail> orderInvoiceSizeDetailModel, List<ProductSizeDetail> productSizeDetailsModel, string hostingURL, List<ProductImage> productImagesModel, List<SizeDetail> sizeDetailsModel)
        {
            GetOrderInvoiceDetail getOrderInvoiceDetailModel = new GetOrderInvoiceDetail();
            if (orderDetailModel != null)
            {
                getOrderInvoiceDetailModel.invoiceDate = DBHelper.ParseString(orderDetailModel[0].Order.OrderDate).Split(' ')[0];
                getOrderInvoiceDetailModel.OrderId = DBHelper.ParseString(orderDetailModel[0].Order.OrderId);
                getOrderInvoiceDetailModel.poNumber = DBHelper.ParseString(orderDetailModel[0].Order.UniqueOrderId);
                getOrderInvoiceDetailModel.UserId = DBHelper.ParseString(orderDetailModel[0].Order.UserFK);
                getOrderInvoiceDetailModel.UserAddressId = DBHelper.ParseString(orderDetailModel[0].Order.UserAddressFK);

                #region Address Detail
                getOrderInvoiceDetailModel.Address1 = DBHelper.ParseString(orderDetailModel[0].Order.UserAddress.Address1);
                getOrderInvoiceDetailModel.Address2 = DBHelper.ParseString(orderDetailModel[0].Order.UserAddress.Address2);
                getOrderInvoiceDetailModel.City = DBHelper.ParseString(orderDetailModel[0].Order.UserAddress.City);
                getOrderInvoiceDetailModel.CompanyName = DBHelper.ParseString(orderDetailModel[0].Order.UserAddress.CompanyName);
                getOrderInvoiceDetailModel.Country = DBHelper.ParseString(orderDetailModel[0].Order.UserAddress.Country.Name);
                getOrderInvoiceDetailModel.EmailId = DBHelper.ParseString(orderDetailModel[0].Order.UserAddress.Email);
                getOrderInvoiceDetailModel.firstName = DBHelper.ParseString(orderDetailModel[0].Order.UserAddress.FullName);
                getOrderInvoiceDetailModel.PhoneNumber = DBHelper.ParseString(orderDetailModel[0].Order.UserAddress.PhoneNumber);
                getOrderInvoiceDetailModel.State = DBHelper.ParseString(orderDetailModel[0].Order.UserAddress.State.Name);
                getOrderInvoiceDetailModel.Zipcode = DBHelper.ParseString(orderDetailModel[0].Order.UserAddress.Zipcode);
                #endregion

                #region Product Detail
                List<GetOrderInvoiceProductDetail> productDetailsList = new List<GetOrderInvoiceProductDetail>();
                foreach (var orderdetail in orderDetailModel)
                {
                    GetOrderInvoiceProductDetail productDetailObject = new GetOrderInvoiceProductDetail();
                    productDetailObject.Price = DBHelper.ParseString(orderdetail.Price);
                    productDetailObject.ProductId = DBHelper.ParseString(orderdetail.ProductFK);
                    productDetailObject.ProductName = DBHelper.ParseString(orderdetail.Product.Name);
                    productDetailObject.Qty = DBHelper.ParseString(orderdetail.Quantity);
                    productDetailObject.TotalQty = DBHelper.ParseString(orderdetail.Quantity);
                    productDetailObject.SubTotal = DBHelper.ParseString(orderdetail.SubTotalAmount);
                    productDetailObject.Color = DBHelper.ParseString(orderdetail.ProductColor.Color.Name);
                    productDetailObject.ProductColorId = DBHelper.ParseString(orderdetail.ProductColorFK);
                    productDetailObject.ShippedQty = "0";
                    var ShippedProductQty = orderInvoiceSizeDetailModel.Where(x => x.OrderInvoiceDetail.ProductFK == orderdetail.ProductFK && x.OrderInvoiceDetail.ProductColorFK == orderdetail.ProductColorFK).Select(x => x.OrderInvoiceDetail).Distinct().ToList();
                    if (ShippedProductQty != null)
                    {
                        double ShippedQty = 0;
                        foreach (var item in ShippedProductQty)
                        {
                            ShippedQty = ShippedQty + DBHelper.ParseDouble(item.Quantity);
                        }
                        productDetailObject.ShippedQty = DBHelper.ParseString(ShippedQty);
                    }
                    if (!string.IsNullOrWhiteSpace(productDetailObject.ShippedQty) && !string.IsNullOrWhiteSpace(productDetailObject.ShippedQty))
                    {
                        productDetailObject.RemainingQty = DBHelper.ParseString(DBHelper.ParseDouble(productDetailObject.Qty) - DBHelper.ParseDouble(productDetailObject.ShippedQty));
                        productDetailObject.TotalQty = DBHelper.ParseString(productDetailObject.RemainingQty);
                    }

                    var productImage = productImagesModel.Where(x => x.ProductColorFK == orderdetail.ProductColorFK).ToList();
                    if (productImage != null && productImage.Count > 0)
                    {
                        productDetailObject.ProductImage = hostingURL + Constants.ProductImagesPath.Replace(@"\", "/") + "/" + DBHelper.ParseString(productImage[0].Image);
                    }
                    var productSize = productSizeDetailsModel.Where(x => x.ProductFK == orderdetail.ProductFK).ToList();
                    if (productSize != null && productSize.Count > 0)
                    {
                        double sizeTotal = 0;
                        foreach (var size in productSize)
                        {
                            productDetailObject.ProductSizeRatio += DBHelper.ParseString(DBHelper.ParseDouble(size.Name)) + "/";
                            productDetailObject.RequiredRatio += DBHelper.ParseString(DBHelper.ParseDouble(size.Name) * DBHelper.ParseDouble(orderdetail.Quantity)) + "/";
                            sizeTotal = sizeTotal + (DBHelper.ParseDouble(size.Name) * DBHelper.ParseDouble(orderdetail.Quantity));
                        }
                        productDetailObject.RequiredRatio = productDetailObject.RequiredRatio.TrimEnd('/');
                        productDetailObject.ProductSizeRatio = productDetailObject.ProductSizeRatio.TrimEnd('/');
                        productDetailObject.RequiredTotalSize = DBHelper.ParseString(sizeTotal);
                    }
                    var orderInvoiceProductList = orderInvoiceSizeDetailModel.Where(x => x.OrderInvoiceDetail.ProductFK == orderdetail.ProductFK && x.OrderInvoiceDetail.ProductColorFK == orderdetail.ProductColorFK).ToList();
                    if (orderInvoiceProductList != null && orderInvoiceProductList.Count > 0)
                    {
                        double[] shippedSizeRatio = new double[productSize.Count];
                        double sizeTotal = 0;
                        for (int i = 0; i < orderInvoiceProductList.Count;)
                        {
                            for (int j = 0; j < productSize.Count; j++, i++)
                            {
                                shippedSizeRatio[j] = shippedSizeRatio[j] + DBHelper.ParseDouble(orderInvoiceProductList[i].Size);
                                sizeTotal = sizeTotal + DBHelper.ParseDouble(orderInvoiceProductList[i].Size);
                            }
                        }
                        foreach (var size in shippedSizeRatio)
                        {
                            productDetailObject.ShippedRatio += DBHelper.ParseString(size) + "/";
                        }

                        productDetailObject.ShippedRatio = productDetailObject.ShippedRatio.Trim('/');
                        productDetailObject.RequiredTotalSize = DBHelper.ParseString(sizeTotal);
                    }
                    else
                    {
                        for (int i = 0; i < productSize.Count; i++)
                        {
                            productDetailObject.ShippedRatio += "0/";
                        }
                        productDetailObject.ShippedRatio = productDetailObject.ShippedRatio.Trim('/');
                    }
                    string[] requiredRatio = productDetailObject.RequiredRatio.Split('/');
                    string[] shippedRatio = productDetailObject.ShippedRatio.Split('/');
                    if (requiredRatio != null && requiredRatio.Length > 0)
                    {
                        List<GetOrderInvoiceRemainingSizeList> remainingSizeLists = new List<GetOrderInvoiceRemainingSizeList>();
                        for (int i = 0; i < requiredRatio.Length; i++)
                        {
                            remainingSizeLists.Add(new GetOrderInvoiceRemainingSizeList
                            {
                                value = DBHelper.ParseString(DBHelper.ParseDouble(requiredRatio[i]) - DBHelper.ParseDouble(shippedRatio[i]))
                            });
                            productDetailObject.RemainingRatio += DBHelper.ParseString(DBHelper.ParseDouble(requiredRatio[i]) - DBHelper.ParseDouble(shippedRatio[i])) + "/";
                        }
                        productDetailObject.RemainingRatio = productDetailObject.RemainingRatio.Trim('/');
                        productDetailObject.remainingSizeLists = remainingSizeLists;
                    }
                    GetOrderInvoiceMainSizeGroupList sizeGroupListObject = new GetOrderInvoiceMainSizeGroupList();
                    List<GetOrderInvoiceSizeGroupList> sizeGroupList = new List<GetOrderInvoiceSizeGroupList>();
                    var productSizeDetailModel = sizeDetailsModel.Where(x => x.SizeFK == orderdetail.Product.SizeFK).ToList();
                    if (productSizeDetailModel != null)
                    {
                        foreach (var size in productSizeDetailModel)
                        {
                            sizeGroupList.Add(new GetOrderInvoiceSizeGroupList
                            {
                                Id = DBHelper.ParseString(size.SizeDetailId),
                                Name = size.Name
                            });
                        }
                    }
                    sizeGroupListObject.sizeGroupList = sizeGroupList;
                    productDetailObject.sizeGroupList = sizeGroupListObject;
                    productDetailsList.Add(productDetailObject);
                }
                getOrderInvoiceDetailModel.productDetails = productDetailsList;
                #endregion
            }
            return getOrderInvoiceDetailModel;
        }

        /// <summary>
        /// Binds the order invoice product category to model.
        /// </summary>
        /// <param name="orderInvoiceDetail">The order invoice detail.</param>
        /// <param name="scheme">The scheme.</param>
        /// <param name="productImageModel">The product image model.</param>
        /// <returns></returns>
        public static OrderInvoiceDetailModel BindOrderInvoiceProductCategoryToModel(OrderInvoiceDetailModel orderInvoiceDetail, List<ProductCategoryDetail> categoryDetails)
        {
            if (orderInvoiceDetail != null)
            {
                foreach (var item in orderInvoiceDetail.ProductDetails)
                {
                    var category = categoryDetails.Where(x => x.ProductFK == DBHelper.ParseInt64(item.ProductId)).ToList();
                    if (category != null)
                    {
                        string categories = "";
                        foreach (var cat in category)
                        {
                            categories += cat.ProductCategoryType.CategoryType + ",";
                        }
                        categories = categories.TrimEnd(',');
                        item.Category = categories;
                    }
                }
            }
            return orderInvoiceDetail;
        }

        /// <summary>
        /// Binds the order invoice product category to model.
        /// </summary>
        /// <param name="orderInvoiceDetail">The order invoice detail.</param>
        /// <param name="scheme">The scheme.</param>
        /// <param name="productImageModel">The product image model.</param>
        /// <returns></returns>
        public static OrderInvoiceDetailModel BindOrderInvoiceProductSizeToModel(OrderInvoiceDetailModel orderInvoiceDetail, List<ProductSizeDetail> productSizeDetails, List<SizeDetail> sizeDetails)
        {
            if (orderInvoiceDetail != null)
            {
                foreach (var item in orderInvoiceDetail.ProductDetails)
                {
                    var size = sizeDetails.Where(x => x.SizeFK == DBHelper.ParseInt64(item.SizeId)).ToList();
                    if (size != null)
                    {
                        string sizeRatio = "";
                        foreach (var cat in size)
                        {
                            sizeRatio += cat.Name + "/";
                        }
                        sizeRatio = sizeRatio.TrimEnd('/');
                        sizeRatio += " - ";

                        var sizeDetail = productSizeDetails.Where(x => x.ProductFK == DBHelper.ParseInt64(item.ProductId)).ToList();
                        if (sizeDetail!=null)
                        {
                            foreach (var cat in sizeDetail)
                            {
                                sizeRatio += cat.Name + "/";
                            }
                            sizeRatio = sizeRatio.TrimEnd('/');
                        }

                        item.SizeRatio = sizeRatio;

                    }
                }
            }
            return orderInvoiceDetail;
        }
    }
}
