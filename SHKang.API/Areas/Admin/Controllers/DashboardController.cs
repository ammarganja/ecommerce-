namespace SHKang.API.Areas.Admin.Controllers
{
    #region namespaces
    using Microsoft.AspNetCore.Mvc;
    using SHKang.API.Helper;
    using SHKang.Core;
    using SHKang.Core.Constant;
    using SHKang.Core.Helpers;
    using SHKang.Repository.Interface;
    using System;

    #endregion

    [Route("api/admin/[controller]")]
    [ApiController]
    [AuthorizeHelper]
    public class DashboardController : ControllerBase
    {

        #region Private Variables
        /// <summary>
        /// The i dashboard
        /// </summary>
        private readonly IDashboard iDashboard;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardController"/> class.
        /// </summary>
        /// <param name="_iDashboard">The i dashboard.</param>
        public DashboardController(IDashboard _iDashboard)
        {
            iDashboard = _iDashboard;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Dashboards the data.
        /// </summary>
        /// <param name="FromDate">From date.</param>
        /// <param name="ToDate">To date.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("dashboard-data")]
        public IActionResult DashboardData()
        {
            try
            {
                DateTime fromDate = new DateTime(DateTime.Now.Year, 1, 1);
                DateTime toDate = new DateTime(DateTime.Now.Year, 12, 31);
                var dashboardData = iDashboard.GetDashboardData(fromDate, toDate);
                if (dashboardData != null)
                {
                    return Ok(ResponseHelper.Success(dashboardData));
                }
                else
                {
                    return Ok(ResponseHelper.Error(MessageConstants.DataNotFound));
                }
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                return Ok(ResponseHelper.Error(ex.Message));
            }
        }

        #endregion

    }
}