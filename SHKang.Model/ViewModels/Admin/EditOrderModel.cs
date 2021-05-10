using System.Collections.Generic;

namespace SHKang.Model.ViewModels.Admin
{
    public class EditOrderModel
    {
        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        /// <value>
        /// The customer identifier.
        /// </value>
        public string CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the customer address identifier.
        /// </summary>
        /// <value>
        /// The customer address identifier.
        /// </value>
        public string CustomerAddressId { get; set; }

        /// <summary>
        /// Gets or sets the product list.
        /// </summary>
        /// <value>
        /// The product list.
        /// </value>
        public OrderProductListModel ProductList { get; set; }
    }
}
