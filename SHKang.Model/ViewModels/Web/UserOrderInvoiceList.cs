namespace SHKang.Model.ViewModels.Web
{
    #region namespaces
    using System.Collections.Generic;
    #endregion

    public class UserOrderInvoiceList
    {
        /// <summary>
        /// Gets or sets the order date.
        /// </summary>
        /// <value>
        /// The order date.
        /// </value>
        public string InvoiceDate { get; set; }

        /// <summary>
        /// Gets or sets the order identifier.
        /// </summary>
        /// <value>
        /// The order identifier.
        /// </value>
        public string OrderId { get; set; }

        /// <summary>
        /// Gets or sets the order invoice identifier.
        /// </summary>
        /// <value>
        /// The order invoice identifier.
        /// </value>
        public string OrderInvoiceId { get; set; }

        /// <summary>
        /// Gets or sets the unique order identifier.
        /// </summary>
        /// <value>
        /// The unique order identifier.
        /// </value>
        public string UniqueOrderInvoiceId { get; set; }

        /// <summary>
        /// Gets or sets the payable amount.
        /// </summary>
        /// <value>
        /// The payable amount.
        /// </value>
        public string PayableAmount { get; set; }

        /// <summary>
        /// Gets or sets the po number.
        /// </summary>
        /// <value>
        /// The po number.
        /// </value>
        public string ShippingCharges { get; set; }

        /// <summary>
        /// Gets or sets the subtotal amount.
        /// </summary>
        /// <value>
        /// The subtotal amount.
        /// </value>
        public string SubtotalAmount { get; set; }

        /// <summary>
        /// Gets or sets the total items.
        /// </summary>
        /// <value>
        /// The total items.
        /// </value>
        public string TotalItems { get; set; }

        /// <summary>
        /// Gets or sets the total GST amount.
        /// </summary>
        /// <value>
        /// The total GST amount.
        /// </value>
        public string TotalGSTAmount { get; set; }

        /// <summary>
        /// Gets or sets the tracking number.
        /// </summary>
        /// <value>
        /// The tracking number.
        /// </value>
        public string TrackingNumber { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the product lists.
        /// </summary>
        /// <value>
        /// The product lists.
        /// </value>
        public List<UserOrderProductList> ProductLists { get; set; }
    }
}
