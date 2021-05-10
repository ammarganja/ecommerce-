namespace SHKang.Repository.Interface
{
    #region namespaces
    using SHKang.Model.ViewModels.Admin;
    using System; 

    #endregion

    public interface IDashboard
    {
        /// <summary>
        /// Gets the dashboard data.
        /// </summary>
        /// <param name="FromDate">From date.</param>
        /// <param name="ToDate">To date.</param>
        /// <returns></returns>
        DashboardModel GetDashboardData(DateTime fromDate, DateTime toDate);
    }
}
