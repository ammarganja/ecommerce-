namespace SHKang.Core.Helpers
{
    #region namespaces
    using System;
    using System.IO;
    #endregion

    public static class LogHelper
    {
        /// <summary>
        /// Exceptions the log.
        /// </summary>
        /// <param name="log">The log.</param>
        public static void ExceptionLog(string log)
        {
            try
            {
                string currentDir = DBHelper.ParseString(Environment.CurrentDirectory);
                string FileName = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt";
                string LogPath = Path.Combine(currentDir, "Log");
                if (!Directory.Exists(LogPath))
                {
                    Directory.CreateDirectory(LogPath);
                }
                LogPath = Path.Combine(LogPath, FileName);
                using (StreamWriter sw = new StreamWriter(LogPath, true))
                {
                    sw.WriteLine("\n");
                    sw.WriteLine("Exception Date : " + DateTime.Now);
                    sw.WriteLine(log);
                    sw.WriteLine("----------------------------------------------------------------------------------------------");
                }

            }
            catch (Exception ex)
            {

            }
        }
    }

}
