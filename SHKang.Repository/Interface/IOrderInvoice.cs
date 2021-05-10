namespace SHKang.Repository.Interface
{
    #region namespaces
    using SHKang.Model.Models;
    using SHKang.Model.ViewModels.Admin;
    using System.Collections.Generic;

    #endregion

    public interface IOrderInvoice
    {
        /// <summary>
        /// Updates the order invoice status.
        /// </summary>
        /// <param name="OrderInvoiceId">The order invoice identifier.</param>
        /// <param name="OrderStatusId">The order status identifier.</param>
        /// <returns></returns>
        bool UpdateOrderInvoiceStatus(long orderInvoiceId, long orderStatusId, long updatedBy);

        /// <summary>
        /// Adds the order invoice.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="detailModel">The detail model.</param>
        /// <returns></returns>
        long AddOrderInvoice(OrderInvoice orderInvoiceModel, List<OrderInvoiceDetail> detailModel, List<AddOrderInvoiceProductDetail> productSizeDetail);

        /// <summary>
        /// Gets the order invoice detail.
        /// </summary>
        /// <param name="OrderInvoiceId">The order invoice identifier.</param>
        /// <returns></returns>
        List<OrderInvoiceDetail> GetOrderInvoiceDetail(long orderInvoiceId, long orderId = 0);

        /// <summary>
        /// Gets all po number.
        /// </summary>
        /// <returns></returns>
        List<string> GetAllUniqueInvoiceIds();

        /// <summary>
        /// Gets the orderinvoice details.
        /// </summary>
        /// <param name="orderInvoiceId">The order invoice identifier.</param>
        /// <returns></returns>
        List<OrderInvoiceDetailProductModel> GetOrderinvoiceDetailsByInvoiceId(long orderInvoiceId);

        /// <summary>
        /// Gets the order invoice details.
        /// </summary>
        /// <param name="OrderId">The order identifier.</param>
        /// <returns></returns>
        List<OrderInvoiceSizeDetail> GetOrderInvoiceSizeDetails(long OrderId);

        /// <summary>
        /// Gets the invoice count by order identifier.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <returns></returns>
        long GetInvoiceCountByOrderId(long orderId);

        /// <summary>
        /// Gets the admin user address details.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        AdminUserDetails GetAdminUserAddressDetails(int userId);

        /// <summary>
        /// Gets the order user email.
        /// </summary>
        /// <param name="orderInvoiceId">The order invoice identifier.</param>
        /// <returns></returns>
        string GetOrderUserEmail(int orderInvoiceId);

        /// <summary>
        /// Gets the user address.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="orderInvoiceId">The order invoice identifier.</param>
        /// <returns></returns>
        AdminUserDetails GetUserAddress(int userId, long orderInvoiceId);

        /// <summary>
        /// Updates the invoice stripe details.
        /// </summary>
        /// <param name="orderInvoiceId">The order invoice identifier.</param>
        /// <param name="stripeCustomerId">The stripe customer identifier.</param>
        /// <param name="stripePaymentId">The stripe payment identifier.</param>
        void UpdateInvoiceStripeDetails(long orderInvoiceId, string stripeCustomerId, string stripePaymentId);
    }
}
