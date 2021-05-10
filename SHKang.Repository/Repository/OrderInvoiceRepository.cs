namespace SHKang.Repository.Repository
{
    #region namespaces
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using SHKang.Core.Enums;
    using SHKang.Core.Helpers;
    using SHKang.Model.Context;
    using SHKang.Model.Models;
    using SHKang.Model.ViewModels.Admin;
    using SHKang.Repository.Interface;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;

    #endregion
    public class OrderInvoiceRepository : IOrderInvoice
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
        /// Initializes a new instance of the <see cref="OrderInvoiceRepository"/> class.
        /// </summary>
        /// <param name="_context">The context.</param>
        public OrderInvoiceRepository(AppDbContext _context, IOptions<ConnectionString> _settings)
        {
            context = _context;
            settings = _settings;
        }
        #endregion

        #region Public  Methods

        /// <summary>
        /// Updates the order invoice status.
        /// </summary>
        /// <param name="OrderInvoiceId">The order invoice identifier.</param>
        /// <param name="OrderStatusId">The order status identifier.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool UpdateOrderInvoiceStatus(long orderInvoiceId, long orderStatusId, long updatedBy)
        {
            try
            {
                var orderInvoiceModel = context.OrderInvoice.Where(x => x.OrderInvoiceId == orderInvoiceId).FirstOrDefault();
                if (orderInvoiceModel != null)
                {
                    orderInvoiceModel.UpdatedBy = updatedBy;
                    orderInvoiceModel.ModifiedOn = DateTime.Now;
                    orderInvoiceModel.OrderStatusFK = orderStatusId;
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
        /// Adds the order invoice.
        /// </summary>
        /// <param name="orderInvoiceModel">The model.</param>
        /// <param name="detailModel">The detail model.</param>
        /// <returns></returns>
        public long AddOrderInvoice(OrderInvoice orderInvoiceModel, List<OrderInvoiceDetail> detailModel, List<AddOrderInvoiceProductDetail> productSizeDetail)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    context.OrderInvoice.Add(orderInvoiceModel);
                    context.SaveChanges();
                    long OrderInvoiceId = orderInvoiceModel.OrderInvoiceId;
                    if (OrderInvoiceId > 0)
                    {
                        foreach (var item in detailModel)
                        {
                            item.OrderInvoiceFK = OrderInvoiceId;
                            context.OrderInvoiceDetail.Add(item);
                            context.SaveChanges();
                            if (item.OrderInvoiceDetailId <= 0)
                            {
                                transaction.Rollback();
                                return 0;
                            }
                            else
                            {
                                var size = productSizeDetail.Where(x => DBHelper.ParseInt64(x.ProductId) == item.ProductFK && DBHelper.ParseInt64(x.ProductColorId) == item.ProductColorFK).FirstOrDefault();
                                if (size != null)
                                {
                                    string[] sizeSplitted = size.SizeRatio.Split(',');
                                    if (sizeSplitted != null && sizeSplitted.Length > 0)
                                    {
                                        for (int i = 0; i < sizeSplitted.Length; i++)
                                        {
                                            OrderInvoiceSizeDetail orderInvoiceSizeDetail = new OrderInvoiceSizeDetail();
                                            orderInvoiceSizeDetail.OrderInvoiceDetailFK = item.OrderInvoiceDetailId;
                                            orderInvoiceSizeDetail.Size = sizeSplitted[i];
                                            context.OrderInvoiceSizeDetail.Add(orderInvoiceSizeDetail);
                                            context.SaveChanges();
                                            if (orderInvoiceSizeDetail.OrderInvoiceSizeDetailId <= 0)
                                            {
                                                transaction.Rollback();
                                                return 0;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        transaction.Commit();
                        return OrderInvoiceId;
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
        /// Gets the order invoice detail.
        /// </summary>
        /// <param name="OrderInvoiceId">The order invoice identifier.</param>
        /// <returns></returns>
        public List<OrderInvoiceDetail> GetOrderInvoiceDetail(long orderInvoiceId, long orderId = 0)
        {
            try
            {
                if (orderId > 0)
                {
                    return context.OrderInvoiceDetail.
                      Include(x => x.OrderInvoice).
                      Include(x => x.Product).
                      Include(x => x.ProductColor).
                      Include(x => x.ProductColor.Color).
                      Include(x => x.Product.Size).
                      //Include(x => x.Product.SizeRatio).
                      Include(x => x.OrderInvoice.OrderStatus).
                      Include(x => x.OrderInvoice.User).
                      Include(x => x.OrderInvoice.Order).
                      Include(x => x.OrderInvoice.Order.UserAddress).
                      Include(x => x.OrderInvoice.Order.UserAddress.Country).
                       Include(x => x.OrderInvoice.Order.UserAddress.State)
                      .Where(x => x.OrderInvoice.Order.OrderId == orderId).ToList();
                }
                else
                {
                    return context.OrderInvoiceDetail.
                   Include(x => x.OrderInvoice).
                   Include(x => x.Product).
                   Include(x => x.ProductColor).
                   Include(x => x.ProductColor.Color).
                   Include(x => x.Product.Size).
                   Include(x => x.OrderInvoice.OrderStatus).
                   Include(x => x.OrderInvoice.User).
                   Include(x => x.OrderInvoice.Order).
                   Include(x => x.OrderInvoice.Order.UserAddress).
                   Include(x => x.OrderInvoice.Order.UserAddress.Country).
                    Include(x => x.OrderInvoice.Order.UserAddress.State)
                   .Where(x => x.OrderInvoiceFK == orderInvoiceId).ToList();

                }
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Gets all po number.
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllUniqueInvoiceIds()
        {
            try
            {
                return context.OrderInvoice.Select(x => x.UniqueInvoiceId).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Gets the order invoice detail.
        /// </summary>
        /// <param name="orderInvoiceId">The order invoice identifier.</param>
        /// <returns></returns>
        public List<OrderInvoiceDetailProductModel> GetOrderinvoiceDetailsByInvoiceId(long orderInvoiceId)
        {
            try
            {

                return (from orderinvoicedetail in context.OrderInvoiceDetail
                        join orderinvoice in context.OrderInvoice on orderinvoicedetail.OrderInvoiceFK equals orderinvoice.OrderInvoiceId
                        join product in context.Product on orderinvoicedetail.ProductFK equals product.ProductId
                        join productcolor in context.ProductColor on orderinvoicedetail.ProductColorFK equals productcolor.ProductColorId
                        join size in context.Size on product.SizeFK equals size.SizeId
                        join status in context.OrderStatus on orderinvoice.OrderStatusFK equals status.OrderStatusId
                        join user in context.User on orderinvoice.UserFK equals user.UserId
                        join order in context.Order on orderinvoice.OrderFK equals order.OrderId
                        join address in context.UserAddress on order.UserAddressFK equals address.UserAddressId
                        join country in context.Country on address.CountryFK equals country.CountryId
                        join state in context.State on address.StateFK equals state.StateId
                        join productimage in context.ProductImage on productcolor.ProductColorId equals productimage.ProductColorFK
                        where orderinvoice.OrderInvoiceId == orderInvoiceId
                        select new OrderInvoiceDetailProductModel
                        {
                            GSTAmount = orderinvoice.TotalGSTAmount,
                            OrderInvoice = orderinvoice,
                            OrderInvoiceDetailId = orderinvoicedetail.OrderInvoiceDetailId,
                            Price = orderinvoicedetail.Price,
                            Product = product,
                            ProductColor = productcolor,
                            ProductImage = productimage,
                            Quantity = orderinvoicedetail.Quantity,
                            SubTotalAmount = orderinvoicedetail.SubTotalAmount,
                            ProductFK = orderinvoicedetail.ProductFK
                        }).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Gets the order invoice details.
        /// </summary>
        /// <param name="OrderId">The order identifier.</param>
        /// <returns></returns>
        public List<OrderInvoiceSizeDetail> GetOrderInvoiceSizeDetails(long OrderId)
        {
            try
            {
                return context.OrderInvoiceSizeDetail.Include(x => x.OrderInvoiceDetail).Include(x => x.OrderInvoiceDetail.OrderInvoice).Where(x => x.OrderInvoiceDetail.OrderInvoice.OrderFK == OrderId).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Gets the invoice count by order identifier.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <returns></returns>
        public long GetInvoiceCountByOrderId(long orderId)
        {
            try
            {
                return context.OrderInvoice.Where(x => x.IsDelete == false && x.OrderFK == orderId).Count();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Gets the admin user address details.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public AdminUserDetails GetAdminUserAddressDetails(int userId)
        {
            AdminUserDetails adminUserDetails = new AdminUserDetails();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("adminUserId", userId);
                DataSet dataSet = DBHelper.GetDataTable("GetAdminLastAddress", parameters, DBHelper.ParseString(settings.Value.AppDbContext));
                if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0)
                {
                    DataTable dt = dataSet.Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        var row = dt.Rows[0];
                        adminUserDetails.Address1 = DBHelper.ParseString(row["Address1"]);
                        adminUserDetails.Address2 = DBHelper.ParseString(row["Address2"]);
                        adminUserDetails.City = DBHelper.ParseString(row["City"]);
                        adminUserDetails.CountryName = DBHelper.ParseString(row["CountryName"]);
                        adminUserDetails.FirstName = DBHelper.ParseString(row["FirstName"]);
                        adminUserDetails.StateName = DBHelper.ParseString(row["StateName"]);
                    }
                }
                return adminUserDetails;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Gets the order user email.
        /// </summary>
        /// <param name="orderInvoiceId">The order invoice identifier.</param>
        /// <returns></returns>
        public string GetOrderUserEmail(int orderInvoiceId)
        {
            string email = string.Empty;
            try
            {
                var orderInvoiceModel = context.OrderInvoice.Where(x => x.OrderInvoiceId == orderInvoiceId).FirstOrDefault();
                if (orderInvoiceModel != null)
                {
                    var userId = orderInvoiceModel.UserFK;
                    var user = context.User.Where(x => x.UserId == userId).FirstOrDefault();
                    if (user != null)
                    {
                        email = user.EmailAddress;
                    }
                }
                return email;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Gets the user address.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="orderInvoiceId">The order invoice identifier.</param>
        /// <returns></returns>
        public AdminUserDetails GetUserAddress(int userId, long orderInvoiceId)
        {
            AdminUserDetails userDetails = new AdminUserDetails();
            try
            {
                var orderInvoice = context.OrderInvoice.Where(a => a.OrderInvoiceId == orderInvoiceId).FirstOrDefault();
                if (orderInvoice != null)
                {
                    var order = context.Order.Where(a => a.OrderId == orderInvoice.OrderFK).FirstOrDefault();

                    if (order != null)
                    {
                        var userAddress = context.UserAddress.Where(a => a.UserAddressId == order.UserAddressFK).FirstOrDefault();
                        if (userAddress != null)
                        {
                            var country = context.Country.Where(a => a.CountryId == userAddress.CountryFK).FirstOrDefault();
                            var state = context.State.Where(a => a.StateId == userAddress.StateFK).FirstOrDefault();

                            userDetails.Address1 = userAddress.Address1;
                            userDetails.Address2 = userAddress.Address2;
                            userDetails.City = userAddress.City;
                            userDetails.CountryName = country.Name;
                            userDetails.StateName = state.Name;
                            userDetails.Zipcode = userAddress.Zipcode;
                            userDetails.PhoneNumber = userAddress.PhoneNumber;
                            userDetails.FirstName = userAddress.FullName;
                            userDetails.EmailAddress = userAddress.Email;
                        }
                        
                    }
                }
                return userDetails;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Updates the invoice stripe details.
        /// </summary>
        /// <param name="orderInvoiceId">The order invoice identifier.</param>
        /// <param name="stripeCustomerId">The stripe customer identifier.</param>
        /// <param name="stripePaymentId">The stripe payment identifier.</param>
        public void UpdateInvoiceStripeDetails(long orderInvoiceId, string stripeCustomerId, string stripePaymentId)
        {
            try
            {
                var orderInvoice = context.OrderInvoice.Where(a => a.OrderInvoiceId == orderInvoiceId).FirstOrDefault();
                if (orderInvoice != null)
                {
                    orderInvoice.OrderStatusFK = OrderStatusEnum.Paid.GetHashCode();
                    orderInvoice.StripeCustomerId = stripeCustomerId;
                    orderInvoice.StripePaymentId = stripePaymentId;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
