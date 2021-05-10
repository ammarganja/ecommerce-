namespace SHKang.Core
{
    #region namespaces
    using SHKang.Model.ViewModels.Admin; 
    #endregion

    public class ResponseHelper
    {
        /// <summary>
        /// Successes the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static ResponseModel Success(object data)
        {
            ResponseModel returnModel = new ResponseModel();
            returnModel.Result = true;
            returnModel.Message = "Success";
            returnModel.Data = data;
            returnModel.Status = 200;
            return returnModel;
        }

        /// <summary>
        /// Errors the specified message.
        /// </summary>
        /// <param name="Message">The message.</param>
        /// <returns></returns>
        public static ResponseModel Error(string Message)
        {
            ResponseModel returnModel = new ResponseModel();
            returnModel.Result = false;
            returnModel.Message = Message;
            returnModel.Data = null;
            return returnModel;
        }
    }
}
