namespace SHKang.Repository.Repository
{
    #region namespaces
    using Microsoft.Extensions.Options;
    using SHKang.Core.Enums;
    using SHKang.Core.Helpers;
    using SHKang.Model.Context;
    using SHKang.Model.ViewModels.Admin;
    using SHKang.Repository.Interface;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;

    #endregion
    public class DashboardRepository : IDashboard
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
        /// Initializes a new instance of the <see cref="DashboardRepository"/> class.
        /// </summary>
        /// <param name="_context">The context.</param>
        public DashboardRepository(AppDbContext _context, IOptions<ConnectionString> _settings)
        {
            context = _context;
            settings = _settings;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the dashboard data.
        /// </summary>
        /// <param name="FromDate">From date.</param>
        /// <param name="ToDate">To date.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public DashboardModel GetDashboardData(DateTime fromDate, DateTime toDate)
        {
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("FROMDATE", fromDate);
            parameters[1] = new SqlParameter("TODATE", toDate);
            try
            {
                DataSet dataSet = DBHelper.GetDataTable("DASHBOARDDATA", parameters, DBHelper.ParseString(settings.Value.AppDbContext));
                if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0)
                {
                    DashboardModel dashboardModel = new DashboardModel();
                    DataTable dtDashboardDataCount = dataSet.Tables[0];
                    DataTable dtMonthWiseOrder = dataSet.Tables[1];
                    DataTable dtMonthWiseUser = dataSet.Tables[2];
                    
                    #region Master Count
                    if (dtDashboardDataCount != null && dtDashboardDataCount.Rows.Count > 0)
                    {
                        dashboardModel.TotalOrder = DBHelper.ParseString(dtDashboardDataCount.Rows[0]["TotalOrder"]);
                        dashboardModel.TotalCompletedOrder = DBHelper.ParseString(dtDashboardDataCount.Rows[0]["TotalCompletedOrder"]);
                        dashboardModel.TotalPendingOrder = DBHelper.ParseString(dtDashboardDataCount.Rows[0]["TotalPendingOrder"]);
                        dashboardModel.TotalUser = DBHelper.ParseString(dtDashboardDataCount.Rows[0]["TotalUser"]);
                        dashboardModel.TotalProduct = DBHelper.ParseString(dtDashboardDataCount.Rows[0]["TotalProduct"]);
                    } 
                    #endregion

                    #region Order Month Wise
                    List<DashboardOrderMonthWise> dashboardOrderMonthWiseList = new List<DashboardOrderMonthWise>();
                    if (dtMonthWiseOrder != null && dtMonthWiseOrder.Rows.Count > 0)
                    {
                        foreach (int month in Enum.GetValues(typeof(Months)))
                        {
                            DataRow[] drMonth = dtMonthWiseOrder.Select("Month='" + month + "'");
                            if (drMonth != null && drMonth.Length > 0)
                            {
                                DashboardOrderMonthWise dashboardOrderMonthWiseObject = new DashboardOrderMonthWise();
                                dashboardOrderMonthWiseObject.Month = Enum.GetName(typeof(Months), month);
                                dashboardOrderMonthWiseObject.TotalOrder = DBHelper.ParseString(drMonth[0]["TotalOrder"]);
                                dashboardOrderMonthWiseList.Add(dashboardOrderMonthWiseObject);
                            }
                            else
                            {
                                DashboardOrderMonthWise dashboardOrderMonthWiseObject = new DashboardOrderMonthWise();
                                dashboardOrderMonthWiseObject.Month = Enum.GetName(typeof(Months), month);
                                dashboardOrderMonthWiseObject.TotalOrder = "0";
                                dashboardOrderMonthWiseList.Add(dashboardOrderMonthWiseObject);
                            }
                        }
                        dashboardModel.MonthwiseOrder = dashboardOrderMonthWiseList;
                    }
                    #endregion

                    #region User Month Wise
                    List<DashboardUserMonthWise> dashboardUserMonthWiseList = new List<DashboardUserMonthWise>();
                    if (dtMonthWiseUser != null && dtMonthWiseUser.Rows.Count > 0)
                    {
                        foreach (int month in Enum.GetValues(typeof(Months)))
                        {
                            DataRow[] drMonth = dtMonthWiseUser.Select("Month='" + month + "'");
                            if (drMonth != null && drMonth.Length > 0)
                            {
                                DashboardUserMonthWise dashboardUserMonthWiseObject = new DashboardUserMonthWise();
                                dashboardUserMonthWiseObject.Month = Enum.GetName(typeof(Months), month);
                                dashboardUserMonthWiseObject.TotalUser = DBHelper.ParseString(drMonth[0]["TotalUser"]);
                                dashboardUserMonthWiseList.Add(dashboardUserMonthWiseObject);
                            }
                            else
                            {
                                DashboardUserMonthWise dashboardUserMonthWiseObject = new DashboardUserMonthWise();
                                dashboardUserMonthWiseObject.Month = Enum.GetName(typeof(Months), month);
                                dashboardUserMonthWiseObject.TotalUser = "0";
                                dashboardUserMonthWiseList.Add(dashboardUserMonthWiseObject);
                            }
                        }
                        dashboardModel.MonthwiseUser = dashboardUserMonthWiseList;
                    } 
                    #endregion

                    return dashboardModel;
                }
                else
                {
                    return null;
                }
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
