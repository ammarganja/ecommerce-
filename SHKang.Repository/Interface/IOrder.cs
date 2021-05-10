namespace SHKang.Repository.Interface
{
    #region namespace
    using SHKang.Model.Models;
    using SHKang.Model.ViewModels.Admin;
    using SHKang.Model.ViewModels.Web;
    using System.Collections.Generic; 
    #endregion

    public interface IOrder
    {
        /// <summary>
        /// Gets the order list.
        /// </summary>
        /// <param name="pageNo">The page no.</param>
        /// <param name="rowsPerPage">The rows per page.</param>
        /// <param name="searchString">The search string.</param>
        /// <param name="column">The column.</param>
        /// <param name="direction">The direction.</param>
        /// <returns></returns>
        OrderListModel GetOrderList(int pageNo,int rowsPerPage, string searchString, string column, string direction);

        /// <summary>
        /// Gets the order detail.
        /// </summary>
        /// <param name="OrderId">The order identifier.</param>
        /// <returns></returns>
        List<OrderDetail> GetOrderDetail(long orderId);

        /// <summary>
        /// Adds the order.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="detailModel">The detail model.</param>
        /// <returns></returns>
        long AddOrder(Order orderModel, List<OrderDetail> detailModel);

        /// <summary>
        /// Gets all unique invoice ids.
        /// </summary>
        /// <returns></returns>
        List<string> GetAllUniqueInvoiceIds();

        /// <summary>
        /// Gets all unique po number.
        /// </summary>
        /// <returns></returns>
        List<string> GetAllUniquePoNumber();

        /// <summary>
        /// Gets the user order list.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="hostingPath">The hosting path.</param>
        /// <returns></returns>
        List<UserOrderList> GetUserOrderList(long userId, string hostingPath);

        /// <summary>
        /// Gets the status by tracking number.
        /// </summary>
        /// <param name="OrderInvoiceId">The order invoice identifier.</param>
        /// <returns></returns>
        long GetInvoiceStatusByInvoiceId(long orderInvoiceId);

        /// <summary>
        /// Deletes the order.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <returns></returns>
        bool DeleteOrder(long orderId);

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
        EditOrderModel GetOrderForEdit(long OrderId, int pageNo, int rowsPerPage, string searchString, string column, string direction,string hostingURL);

        /// <summary>
        /// Edits the order.
        /// </summary>
        /// <param name="orderModel">The order model.</param>
        /// <param name="detailModel">The detail model.</param>
        /// <returns></returns>
        long EditOrder(Order orderModel, List<OrderDetail> detailModel);

        /// <summary>
        /// Gets the order model by order identifier.
        /// </summary>
        /// <param name="OrderId">The order identifier.</param>
        /// <returns></returns>
        Order GetOrderModelByOrderId(long OrderId);
    }
}
