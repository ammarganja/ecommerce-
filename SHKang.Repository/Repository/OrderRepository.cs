namespace SHKang.Repository.Repository
{
    #region namespaces
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using SHKang.Core.Constant;
    using SHKang.Core.Enums;
    using SHKang.Core.Helpers;
    using SHKang.Model.Context;
    using SHKang.Model.Models;
    using SHKang.Model.ViewModels.Admin;
    using SHKang.Model.ViewModels.Web;
    using SHKang.Repository.Interface;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;

    #endregion

    public class OrderRepository : IOrder
    {

        #region Private Variables

        /// <summary>
        /// The context
        /// </summary>
        private AppDbContext context;

        /// <summary>
        /// The settings
        /// </summary>
        private IOptions<ConnectionString> settings;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderRepository"/> class.
        /// </summary>
        /// <param name="_context">The context.</param>
        public OrderRepository(AppDbContext _context, IOptions<ConnectionString> _settings)
        {
            context = _context;
            settings = _settings;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the order list.
        /// </summary>
        /// <param name="pageNo">The page no.</param>
        /// <param name="rowsPerPage">The rows per page.</param>
        /// <param name="searchString">The search string.</param>
        /// <param name="column">The column.</param>
        /// <param name="direction">The direction.</param>
        /// <returns></returns>
        public OrderListModel GetOrderList(int pageNo, int rowsPerPage, string searchString, string column, string direction)
        {
            OrderListModel orderList = new OrderListModel();
            List<OrderListDataModel> orderListDataModel = new List<OrderListDataModel>();
            try
            {
                SqlParameter[] parameters = new SqlParameter[5];
                parameters[0] = new SqlParameter("PAGENO", pageNo);
                parameters[1] = new SqlParameter("RECORDPERPAGE", rowsPerPage);
                parameters[2] = new SqlParameter("SEARCHSTRING", searchString);
                parameters[3] = new SqlParameter("COLUMN", column);
                parameters[4] = new SqlParameter("DIRECTION", direction);
                DataSet dataSet = DBHelper.GetDataTable("ORDERLIST", parameters, DBHelper.ParseString(settings.Value.AppDbContext));
                if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0)
                {
                    DataTable dtOrders = dataSet.Tables[0];
                    DataTable dtOrderInvoice = dataSet.Tables[1];
                    DataTable dtTotal = dataSet.Tables[2];
                    if (dtOrders != null && dtOrders.Rows.Count > 0)
                    {
                        foreach (DataRow item in dtOrders.Rows)
                        {
                            OrderListDataModel orderListModel = new OrderListDataModel();
                            orderListModel.CompanyName = DBHelper.ParseString(item["CompanyName"]);
                            orderListModel.FirstName = DBHelper.ParseString(item["FirstName"]);
                            orderListModel.LastName = DBHelper.ParseString(item["LastName"]);
                            orderListModel.EmailAddress = DBHelper.ParseString(item["EmailAddress"]);
                            orderListModel.MobileNumber = DBHelper.ParseString(item["PhoneNumber"]);
                            orderListModel.TotalQuantity = DBHelper.ParseString(item["TotalQty"]);
                            orderListModel.OrderDate = DBHelper.ParseString(Convert.ToDateTime(item["OrderDate"])).Split(' ')[0];
                            orderListModel.OrderId = DBHelper.ParseString(item["OrderId"]);
                            orderListModel.PayableAmount = DBHelper.ParseString(item["PayableAmount"]);
                            orderListModel.PONumber = DBHelper.ParseString(item["PONumber"]);
                            orderListModel.SubTotalAmount = DBHelper.ParseString(item["SubTotalAmount"]);
                            orderListModel.UniqueOrderId = DBHelper.ParseString(item["UniqueOrderId"]);

                            DataRow[] drInvoice = dtOrderInvoice.Select("OrderFK='" + DBHelper.ParseString(item["OrderId"]) + "'");
                            if (drInvoice != null && drInvoice.Length > 0)
                            {
                                List<OrderListInvoiceModel> invoiceListModel = new List<OrderListInvoiceModel>();
                                for (int x = 0; x < drInvoice.Length; x++)
                                {
                                    invoiceListModel.Add(new OrderListInvoiceModel
                                    {
                                        InvoiceId = DBHelper.ParseString(drInvoice[x]["OrderInvoiceId"]),
                                        UniqueInvoiceNo = DBHelper.ParseString(drInvoice[x]["UniqueInvoiceId"]),
                                        Status = DBHelper.ParseString(drInvoice[x]["OrderStatusId"])
                                    });
                                }
                                //UniqueInvoiceId = UniqueInvoiceId.TrimEnd(',');
                                //OrderStatusId = OrderStatusId.TrimEnd(',');
                                //Status = Status.TrimEnd(',');
                                orderListModel.invoiceList = invoiceListModel;
                            }
                            orderListDataModel.Add(orderListModel);
                        }
                        orderList.Items = orderListDataModel;
                        orderList.Total = DBHelper.ParseString(dtTotal.Rows.Count);
                    }
                }
                return orderList;
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Gets the order detail.
        /// </summary>
        /// <param name="OrderId">The order identifier.</param>
        /// <returns></returns>
        public List<OrderDetail> GetOrderDetail(long orderId)
        {
            try
            {
                return context.OrderDetail.Include(x => x.Order)
                    .Include(x => x.Order.User)
                    .Include(x => x.Order.UserAddress)
                    .Include(x => x.Order.UserAddress.State)
                    .Include(x => x.Order.UserAddress.Country)
                    .Include(x => x.Product)
                    .Include(x => x.ProductColor)
                    .Include(x => x.ProductColor.Color)
                    .Include(x => x.Product.Size)
                    //.Include(x => x.Product.SizeRatio)
                    .Where(x => x.Order.OrderId == orderId).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Adds the order.
        /// </summary>
        /// <param name="orderModel">The model.</param>
        /// <param name="detailModel">The detail model.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public long AddOrder(Order orderModel, List<OrderDetail> detailModel)
        {

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    context.Order.Add(orderModel);
                    context.SaveChanges();
                    long OrderId = orderModel.OrderId;
                    if (OrderId > 0)
                    {
                        foreach (var item in detailModel)
                        {
                            item.OrderFK = OrderId;
                            context.OrderDetail.Add(item);
                            context.SaveChanges();
                            long detailId = item.OrderDetailId;
                            if (detailId <= 0)
                            {
                                transaction.Rollback();
                                return 0;
                            }
                        }
                        transaction.Commit();
                        return OrderId;
                    }
                    else
                    {
                        transaction.Rollback();
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                    throw ex;
                }
            }

        }

        /// <summary>
        /// Gets all unique invoice ids.
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllUniqueInvoiceIds()
        {
            try
            {
                return context.Order.Select(x => x.UniqueOrderId).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Gets all unique po number.
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllUniquePoNumber()
        {
            try
            {
                return context.Order.Select(x => x.PONumber).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Gets the user order list.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="hostingPath">The hosting path.</param>
        /// <returns></returns>
        public List<UserOrderList> GetUserOrderList(long userId, string hostingPath)
        {
            List<UserOrderList> UserOrderList = new List<UserOrderList>();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("USERID", userId);
                DataSet ds = DBHelper.GetDataTable("USERORDERLIST", parameters, DBHelper.ParseString(settings.Value.AppDbContext));
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                {
                    DataTable Order = ds.Tables[0];
                    DataTable OrderProduct = ds.Tables[1];
                    DataTable Invoice = ds.Tables[2];
                    DataTable InvoiceProduct = ds.Tables[3];
                    DataTable SizeDetail = ds.Tables[4];
                    DataTable Size = ds.Tables[5];
                    DataTable Category = ds.Tables[6];

                    if (Order != null && Order.Rows.Count > 0)
                    {
                        for (int o = 0; o < Order.Rows.Count; o++)
                        {
                            UserOrderList orderModel = new UserOrderList();
                            orderModel.OrderDate = DBHelper.ParseString(Convert.ToDateTime(Order.Rows[o]["OrderDate"])).Split(' ')[0];
                            orderModel.OrderId = DBHelper.ParseString(Order.Rows[o]["OrderId"]);
                            orderModel.PayableAmount = DBHelper.ParseString(Order.Rows[o]["PayableAmount"]);
                            orderModel.PONumber = DBHelper.ParseString(Order.Rows[o]["PONumber"]);
                            orderModel.SubtotalAmount = DBHelper.ParseString(Order.Rows[o]["SubtotalAmount"]);
                            orderModel.UniqueOrderId = DBHelper.ParseString(Order.Rows[o]["UniqueOrderId"]);
                            List<UserOrderProductList> orderProductLists = new List<UserOrderProductList>();
                            List<UserOrderInvoiceList> userOrderInvoiceLists = new List<UserOrderInvoiceList>();
                            if (OrderProduct != null && OrderProduct.Rows.Count > 0)
                            {
                                DataRow[] drProduct = OrderProduct.Select("OrderFK='" + DBHelper.ParseString(Order.Rows[o]["OrderId"]) + "'");
                                if (drProduct != null && drProduct.Length > 0)
                                {
                                    foreach (DataRow item in drProduct)
                                    {
                                        #region Image
                                        string image = string.Empty;
                                        if (!string.IsNullOrWhiteSpace(DBHelper.ParseString(item["Image"])))
                                        {
                                            image = hostingPath + Constants.ProductImagesPath.Replace(@"\", "/") + "/" + DBHelper.ParseString(item["Image"]);
                                        }
                                        else
                                        {
                                            image = hostingPath + Constants.NoImageAvaliablePath.Replace(@"\", "/");
                                        }
                                        #endregion

                                        #region Category
                                        string category = string.Empty;
                                        DataRow[] drCategory = Category.Select("ProductFK='" + DBHelper.ParseString(item["ProductId"]) + "'");
                                        if (drCategory != null && drCategory.Length > 0)
                                        {
                                            foreach (var cat in drCategory)
                                            {
                                                category += cat["CategoryType"] + ",";
                                            }
                                            category = category.TrimEnd(',');
                                        }
                                        #endregion

                                        #region SizeRatio
                                        string sizeId = string.Empty;
                                        string sizeRatio = string.Empty;
                                        DataRow[] drSizeDetail = SizeDetail.Select("ProductFK='" + DBHelper.ParseString(item["ProductId"]) + "'");
                                        if (drSizeDetail != null && drSizeDetail.Length > 0)
                                        {
                                            sizeId = DBHelper.ParseString(drSizeDetail[0]["SizeFK"]);
                                            DataRow[] drSize = Size.Select("SizeFK='" + sizeId + "'");
                                            if (drSize != null && drSize.Length > 0)
                                            {
                                                foreach (var size in drSize)
                                                {
                                                    sizeRatio += DBHelper.ParseString(size["Name"]) + "/";
                                                }
                                                sizeRatio = sizeRatio.TrimEnd('/');
                                                sizeRatio += " - ";
                                                foreach (var size in drSizeDetail)
                                                {
                                                    sizeRatio += DBHelper.ParseString(size["Name"]) + "/";
                                                }
                                                sizeRatio = sizeRatio.TrimEnd('/');
                                            }
                                        }
                                        #endregion

                                        orderProductLists.Add(new UserOrderProductList
                                        {
                                            Price = DBHelper.ParseString(item["Price"]),
                                            ProductId = DBHelper.ParseString(item["ProductId"]),
                                            Quantity = DBHelper.ParseString(item["Quantity"]),
                                            ProductName = DBHelper.ParseString(item["Name"]),
                                            Subtotal = DBHelper.ParseString(item["SubtotalAmount"]),
                                            Image = image,
                                            Color = DBHelper.ParseString(item["ColorName"]),
                                            Category = category,
                                            SizeRatio = sizeRatio,
                                            ProductColorId= DBHelper.ParseString(item["ProductColorFK"]),
                                        });
                                    }
                                }
                                orderModel.ProductList = orderProductLists;
                                orderModel.TotalItems = DBHelper.ParseString(drProduct.Length);
                            }

                            if (Invoice != null && Invoice.Rows.Count > 0)
                            {
                                DataRow[] drInvoice = Invoice.Select("OrderFK='" + DBHelper.ParseString(Order.Rows[o]["OrderId"]) + "'");
                                if (drInvoice != null && drInvoice.Length > 0)
                                {
                                    for (int i = 0; i < drInvoice.Length; i++)
                                    {
                                        UserOrderInvoiceList invoiceModel = new UserOrderInvoiceList();
                                        invoiceModel.InvoiceDate = DBHelper.ParseString(Convert.ToDateTime(drInvoice[i]["InvoiceDate"])).Split(' ')[0];
                                        invoiceModel.OrderId = DBHelper.ParseString(drInvoice[i]["OrderFK"]);
                                        invoiceModel.OrderInvoiceId = DBHelper.ParseString(drInvoice[i]["OrderInvoiceId"]);
                                        invoiceModel.PayableAmount = DBHelper.ParseString(drInvoice[i]["PayableAmount"]);
                                        invoiceModel.ShippingCharges = DBHelper.ParseString(drInvoice[i]["ShippingCharges"]);
                                        invoiceModel.Status = DBHelper.ParseString(drInvoice[i]["OrderStatus"]);
                                        invoiceModel.SubtotalAmount = DBHelper.ParseString(drInvoice[i]["SubtotalAmount"]);
                                        invoiceModel.TotalGSTAmount = DBHelper.ParseString(drInvoice[i]["TotalGSTAmount"]);
                                        invoiceModel.TrackingNumber = DBHelper.ParseString(drInvoice[i]["TrackingNumber"]);
                                        invoiceModel.UniqueOrderInvoiceId = DBHelper.ParseString(drInvoice[i]["UniqueInvoiceId"]);
                                        List<UserOrderProductList> orderInvoiceProductLists = new List<UserOrderProductList>();
                                        DataRow[] drInvoiceProduct = InvoiceProduct.Select("OrderInvoiceFK='" + DBHelper.ParseString(drInvoice[i]["OrderInvoiceId"]) + "'");
                                        if (drInvoiceProduct != null && drInvoiceProduct.Length > 0)
                                        {
                                            foreach (DataRow item in drInvoiceProduct)
                                            {

                                                #region Image
                                                string image = string.Empty;
                                                if (!string.IsNullOrWhiteSpace(DBHelper.ParseString(item["Image"])))
                                                {
                                                    image = hostingPath + Constants.ProductImagesPath.Replace(@"\", "/") + "/" + DBHelper.ParseString(item["Image"]);
                                                }
                                                else
                                                {
                                                    image = hostingPath + Constants.NoImageAvaliablePath.Replace(@"\", "/");
                                                }
                                                #endregion

                                                #region Category
                                                string category = string.Empty;
                                                DataRow[] drCategory = Category.Select("ProductFK='" + DBHelper.ParseString(item["ProductId"]) + "'");
                                                if (drCategory != null && drCategory.Length > 0)
                                                {
                                                    foreach (var cat in drCategory)
                                                    {
                                                        category += cat["CategoryType"] + ",";
                                                    }
                                                    category = category.TrimEnd(',');
                                                }
                                                #endregion

                                                #region SizeRatio
                                                string sizeId = string.Empty;
                                                string sizeRatio = string.Empty;
                                                DataRow[] drSizeDetail = SizeDetail.Select("ProductFK='" + DBHelper.ParseString(item["ProductId"]) + "'");
                                                if (drSizeDetail != null && drSizeDetail.Length > 0)
                                                {
                                                    sizeId = DBHelper.ParseString(drSizeDetail[0]["SizeFK"]);
                                                    DataRow[] drSize = Size.Select("SizeFK='" + sizeId + "'");
                                                    if (drSize != null && drSize.Length > 0)
                                                    {
                                                        foreach (var size in drSize)
                                                        {
                                                            sizeRatio += DBHelper.ParseString(size["Name"]) + "/";
                                                        }
                                                        sizeRatio = sizeRatio.TrimEnd('/');
                                                        sizeRatio += " - ";
                                                        foreach (var size in drSizeDetail)
                                                        {
                                                            sizeRatio += DBHelper.ParseString(size["Name"]) + "/";
                                                        }
                                                        sizeRatio = sizeRatio.TrimEnd('/');
                                                    }
                                                }
                                                #endregion

                                                orderInvoiceProductLists.Add(new UserOrderProductList
                                                {
                                                    Price = DBHelper.ParseString(item["Price"]),
                                                    ProductId = DBHelper.ParseString(item["ProductId"]),
                                                    Quantity = DBHelper.ParseString(item["Quantity"]),
                                                    ProductName = DBHelper.ParseString(item["Name"]),
                                                    Subtotal = DBHelper.ParseString(item["SubtotalAmount"]),
                                                    Image = image,
                                                    Color = DBHelper.ParseString(item["ColorName"]),
                                                    Category = category,
                                                    SizeRatio = sizeRatio,
                                                    ProductColorId = DBHelper.ParseString(item["ProductColorFK"]),

                                                });
                                            }
                                        }
                                        invoiceModel.ProductLists = orderInvoiceProductLists;
                                        invoiceModel.TotalItems = DBHelper.ParseString(drInvoiceProduct.Length);
                                        userOrderInvoiceLists.Add(invoiceModel);
                                    }
                                }
                                orderModel.InvoiceList = userOrderInvoiceLists;
                            }
                            UserOrderList.Add(orderModel);
                        }
                        return UserOrderList;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
            return UserOrderList;
        }

        /// <summary>
        /// Gets the status by tracking number.
        /// </summary>
        /// <param name="OrderInvoiceId">The order invoice identifier.</param>
        /// <returns></returns>
        public long GetInvoiceStatusByInvoiceId(long orderInvoiceId)
        {
            try
            {
                return context.OrderInvoice.Where(x => x.OrderInvoiceId == orderInvoiceId).Select(x => x.OrderStatusFK).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Deletes the order.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <returns></returns>
        public bool DeleteOrder(long orderId)
        {
            try
            {
                var order = context.Order.Where(x => x.IsDelete == false && x.OrderId == orderId).FirstOrDefault();
                if (order != null)
                {
                    order.IsDelete = true;
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Gets the order for edit.
        /// </summary>
        /// <param name="OrderId">The order identifier.</param>
        /// <param name="pageNo">The page no.</param>
        /// <param name="rowsPerPage">The rows per page.</param>
        /// <param name="searchString">The search string.</param>
        /// <param name="column">The column.</param>
        /// <param name="direction">The direction.</param>
        /// <returns></returns>
        public EditOrderModel GetOrderForEdit(long OrderId, int pageNo, int rowsPerPage, string searchString, string column, string direction, string hostingURL)
        {
            EditOrderModel editOrderModel = new EditOrderModel();
            OrderProductListModel orderProductListModel = new OrderProductListModel();
            List<OrderProductListDataModel> orderProductListDataModels = new List<OrderProductListDataModel>();
            try
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[6];
                    parameter[0] = new SqlParameter("PAGENO", pageNo);
                    parameter[1] = new SqlParameter("RECORDPERPAGE", rowsPerPage);
                    parameter[2] = new SqlParameter("SEARCHSTRING", searchString);
                    parameter[3] = new SqlParameter("COLUMN", column);
                    parameter[4] = new SqlParameter("DIRECTION", direction);
                    parameter[5] = new SqlParameter("ORDERID", OrderId);
                    DataSet dataSet = DBHelper.GetDataTable("ORDERPRODUCTLIST", parameter, DBHelper.ParseString(settings.Value.AppDbContext));
                    if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0)
                    {
                        DataTable dtProducts = dataSet.Tables[0];
                        DataTable dtTotal = dataSet.Tables[1];
                        DataTable dtImage = dataSet.Tables[2];
                        DataTable dtSizeGroup = dataSet.Tables[3];
                        DataTable dtProductSizeGroup = dataSet.Tables[4];
                        DataTable dtOrder = new DataTable();
                        DataTable dtOrderDetail = new DataTable();

                        if (dataSet.Tables.Count > 5)
                        {
                            dtOrder = dataSet.Tables[5];
                            dtOrderDetail = dataSet.Tables[6];
                        }

                        if (dtProducts != null && dtProducts.Rows.Count > 0)
                        {
                            foreach (DataRow item in dtProducts.Rows)
                            {
                                OrderProductListDataModel orderProductListModelObject = new OrderProductListDataModel();
                                orderProductListModelObject.ColorId = DBHelper.ParseString(item["ProductColorId"]);
                                orderProductListModelObject.Color = DBHelper.ParseString(item["ColorName"]);
                                orderProductListModelObject.Price = DBHelper.ParseString(item["Price"]);
                                orderProductListModelObject.ProductId = DBHelper.ParseString(item["ProductId"]);
                                orderProductListModelObject.ProductName = DBHelper.ParseString(item["Name"]);
                                orderProductListModelObject.SubTotal = "0";
                                orderProductListModelObject.Quantity = "0";
                                if (dtOrderDetail != null && dtOrderDetail.Rows.Count > 0)
                                {
                                    DataRow[] drOrderDetail = dtOrderDetail.Select("ProductColorFK='" + DBHelper.ParseString(item["ProductColorId"]) + "' and ProductFK='" + DBHelper.ParseString(item["ProductId"]) + "'");
                                    if (drOrderDetail != null && drOrderDetail.Length > 0)
                                    {
                                        orderProductListModelObject.IsSelected = true;
                                        orderProductListModelObject.Quantity = DBHelper.ParseString(drOrderDetail[0]["Quantity"]);
                                        orderProductListModelObject.SubTotal = DBHelper.ParseString(DBHelper.ParseDouble(orderProductListModelObject.Quantity) * DBHelper.ParseDouble(orderProductListModelObject.Price));
                                    }
                                }
                                DataRow[] drSizeGroup = dtSizeGroup.Select("SizeFK='" + DBHelper.ParseString(item["SizeId"]) + "'");
                                if (drSizeGroup != null && drSizeGroup.Length > 0)
                                {
                                    foreach (var size in drSizeGroup)
                                    {
                                        orderProductListModelObject.SizeGroup += size["Name"] + "/";
                                    }
                                    orderProductListModelObject.SizeGroup = orderProductListModelObject.SizeGroup.TrimEnd('/');
                                    orderProductListModelObject.SizeGroup += " - ";
                                }

                                DataRow[] drProductSizeGroup = dtProductSizeGroup.Select("ProductFK='" + DBHelper.ParseString(item["ProductId"]) + "'");
                                if (drProductSizeGroup != null && drProductSizeGroup.Length > 0)
                                {
                                    foreach (var size in drProductSizeGroup)
                                    {
                                        orderProductListModelObject.SizeGroup += size["Name"] + "/";
                                    }
                                    orderProductListModelObject.SizeGroup = orderProductListModelObject.SizeGroup.TrimEnd('/');
                                }

                                DataRow[] drImage = dtImage.Select("ProductColorFK='" + item["ProductColorId"] + "'");
                                if (drImage != null && drImage.Length > 0)
                                {
                                    orderProductListModelObject.Image = hostingURL + Constants.ProductImagesPath.Replace(@"\", "/") + "/" + DBHelper.ParseString(drImage[0]["Image"]);
                                }

                                orderProductListDataModels.Add(orderProductListModelObject);
                            }
                            orderProductListModel.Items = orderProductListDataModels;
                            orderProductListModel.Total = DBHelper.ParseString(dtTotal.Rows.Count);
                            editOrderModel.ProductList = orderProductListModel;
                        }
                        if (dtOrder != null && dtOrder.Rows.Count > 0)
                        {
                            editOrderModel.CustomerAddressId = DBHelper.ParseString(dtOrder.Rows[0]["UserAddressFK"]);
                            editOrderModel.CustomerId = DBHelper.ParseString(dtOrder.Rows[0]["UserFK"]);
                        }
                    }
                    return editOrderModel;
                }
                catch (Exception ex)
                {
                    LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                    throw ex;
                }
            }
            catch (Exception ex)
            {

                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Edits the order.
        /// </summary>
        /// <param name="orderModel">The order model.</param>
        /// <param name="detailModel">The detail model.</param>
        /// <returns></returns>
        public long EditOrder(Order orderModel, List<OrderDetail> detailModel)
        {

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var orderResult = context.Order.Where(x => x.OrderId == orderModel.OrderId).FirstOrDefault();
                    if (orderResult != null)
                    {
                        orderResult.ModifiedOn = orderModel.ModifiedOn;
                        orderResult.UpdatedBy = orderModel.UpdatedBy;
                        orderResult.PayableAmount = orderModel.PayableAmount;
                        orderResult.SubTotalAmount = orderModel.SubTotalAmount;
                        orderResult.TotalGSTAmount = orderModel.TotalGSTAmount;
                        orderResult.UserAddressFK = orderModel.UserAddressFK;
                        orderResult.PromocodeFK = orderModel.PromocodeFK;
                        orderResult.UserFK = orderModel.UserFK;
                        context.SaveChanges();
                        long OrderId = orderModel.OrderId;
                        if (OrderId > 0)
                        {
                            var orderDetailModel = context.OrderDetail.Where(x => x.OrderFK == orderModel.OrderId).ToList();
                            if (orderDetailModel != null && orderDetailModel.Count > 0)
                            {
                                foreach (var item in orderDetailModel)
                                {
                                    context.OrderDetail.Remove(item);
                                    context.SaveChanges();
                                }
                            }
                            foreach (var item in detailModel)
                            {
                                item.OrderFK = OrderId;
                                context.OrderDetail.Add(item);
                                context.SaveChanges();
                                long detailId = item.OrderDetailId;
                                if (detailId <= 0)
                                {
                                    transaction.Rollback();
                                    return 0;
                                }
                            }
                            transaction.Commit();
                            return OrderId;
                        }
                        else
                        {
                            transaction.Rollback();
                            return 0;
                        }
                    }
                    else
                    {
                        transaction.Rollback();
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                    throw ex;
                }
            }

        }

        /// <summary>
        /// Gets the order model by order identifier.
        /// </summary>
        /// <param name="OrderId">The order identifier.</param>
        /// <returns></returns>
        public Order GetOrderModelByOrderId(long OrderId)
        {
            try
            {
                return context.Order.Where(x => x.IsDelete == false && x.OrderId == OrderId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }
        #endregion

    }
}
