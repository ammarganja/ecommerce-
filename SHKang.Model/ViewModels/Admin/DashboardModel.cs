namespace SHKang.Model.ViewModels.Admin
{
    #region namespaces
    using System.Collections.Generic; 
    #endregion

    public class DashboardModel
    {
        /// <summary>
        /// Gets or sets the total user.
        /// </summary>
        /// <value>
        /// The total user.
        /// </value>
        public string TotalUser { get; set; }

        /// <summary>
        /// Gets or sets the total order.
        /// </summary>
        /// <value>
        /// The total order.
        /// </value>
        public string TotalOrder { get; set; }

        /// <summary>
        /// Gets or sets the total product.
        /// </summary>
        /// <value>
        /// The total product.
        /// </value>
        public string TotalProduct { get; set; }

        /// <summary>
        /// Gets or sets the total pending order.
        /// </summary>
        /// <value>
        /// The total pending order.
        /// </value>
        public string TotalPendingOrder { get; set; }

        /// <summary>
        /// Gets or sets the total completed order.
        /// </summary>
        /// <value>
        /// The total completed order.
        /// </value>
        public string TotalCompletedOrder { get; set; }

        /// <summary>
        /// Gets or sets the monthwise order.
        /// </summary>
        /// <value>
        /// The monthwise order.
        /// </value>
        public List<DashboardOrderMonthWise> MonthwiseOrder { get; set; }

        /// <summary>
        /// Gets or sets the monthwise user.
        /// </summary>
        /// <value>
        /// The monthwise user.
        /// </value>
        public List<DashboardUserMonthWise> MonthwiseUser { get; set; }
    }

    public class DashboardOrderMonthWise
    {
        /// <summary>
        /// Gets or sets the month.
        /// </summary>
        /// <value>
        /// The month.
        /// </value>
        public string Month { get; set; }

        /// <summary>
        /// Gets or sets the total order.
        /// </summary>
        /// <value>
        /// The total order.
        /// </value>
        public string TotalOrder { get; set; }
    }

    public class DashboardUserMonthWise
    {
        /// <summary>
        /// Gets or sets the month.
        /// </summary>
        /// <value>
        /// The month.
        /// </value>
        public string Month { get; set; }


        /// <summary>
        /// Gets or sets the total user.
        /// </summary>
        /// <value>
        /// The total user.
        /// </value>
        public string TotalUser { get; set; }
    }
}
