namespace SHKang.Core.Helpers
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    public class DBHelper
    {

        /// <summary>
        /// Gets the data table.
        /// </summary>
        /// <param name="spName">Name of the sp.</param>
        /// <param name="sqlParameters">The SQL parameters.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        public static DataSet GetDataTable(string spName, SqlParameter[] sqlParameters, string connectionString)
        {
            DataSet dt = new DataSet();
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(spName, sqlConnection);

            try
            {
                sqlDataAdapter.SelectCommand.CommandTimeout = 180;
                sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                if (sqlParameters != null && sqlParameters.Length > 0)
                {
                    sqlDataAdapter.SelectCommand.Parameters.AddRange(sqlParameters);
                }

                sqlDataAdapter.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "   ::::   " + ex.StackTrace);
                throw ex;
            }
            finally
            {
                sqlDataAdapter.Dispose();
                dt.Dispose();
                sqlConnection.Close();
            }
        }

        /// <summary>
        /// Parses the boolean.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
        /// <returns></returns>
        public static bool ParseBoolean(object value, bool defaultValue = false)
        {
            return Convert.IsDBNull(value) ? defaultValue : Convert.ToBoolean(value);
        }

        /// <summary>
        /// Parses the string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static string ParseString(object value, string defaultValue = "")
        {
            return Convert.IsDBNull(value) ? defaultValue : Convert.ToString(value);
        }

        /// <summary>
        /// Parses the int32.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static int ParseInt32(object value, int defaultValue = 0)
        {
            return Convert.IsDBNull(value) ? defaultValue : Convert.ToInt32(value);
        }

        /// <summary>
        /// Parses the int64.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static long ParseInt64(object value, long defaultValue = 0)
        {
            return Convert.IsDBNull(value) ? defaultValue : Convert.ToInt64(value);

        }

        /// <summary>
        /// Parses the decimal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static decimal ParseDecimal(object value, decimal defaultValue = 0)
        {
            return Convert.IsDBNull(value) ? defaultValue : Convert.ToDecimal(value);
        }

        /// <summary>
        /// Parses the double.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static double ParseDouble(object value, double defaultValue = 0)
        {
            return Convert.IsDBNull(value) ? defaultValue : Convert.ToDouble(value);
        }

        public static decimal RoundOff(decimal value)
        {
            return Math.Round(value, 2);
        }

    }
}
