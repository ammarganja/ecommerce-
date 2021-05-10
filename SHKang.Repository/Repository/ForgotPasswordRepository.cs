namespace SHKang.Repository.Repository
{
    #region namespaces
    using SHKang.Core.Enums;
    using SHKang.Core.Helpers;
    using SHKang.Model.Context;
    using SHKang.Repository.Interface;
    using System;
    using System.Linq;

    #endregion


    public class ForgotPasswordRepository : IForgotPassword
    {
        #region Private Variables

        /// <summary>
        /// The context
        /// </summary>
        private AppDbContext context;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ForgotPasswordRepository"/> class.
        /// </summary>
        /// <param name="_iforgotpassword">The iforgotpassword.</param>
        public ForgotPasswordRepository(AppDbContext _context)
        {
            context = _context;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Forgots the password.
        /// </summary>
        /// <param name="emailId">The email identifier.</param>
        /// <param name="IsAdmin">The is admin.</param>
        /// <returns>
        /// <c>true</c> if [is email exist] [the specified email identifier]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsEmailExist(string emailId, string IsAdmin)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(IsAdmin) && IsAdmin.Equals("1"))
                {
                    var userModel = context.User.Where(x => x.EmailAddress == emailId && x.IsDelete == false && x.RoleType == RoleType.Admin.GetHashCode()).FirstOrDefault();
                    if (userModel != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    var userModel = context.User.Where(x => x.EmailAddress == emailId && x.IsDelete == false && x.RoleType == RoleType.User.GetHashCode()).FirstOrDefault();
                    if (userModel != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Resets the password with code.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public int ResetPasswordWithCode(string password, string code)
        {
            try
            {
                var userModel = context.User.Where(x => x.ResetCode == code && x.IsDelete == false).FirstOrDefault();
                if (userModel != null)
                {
                    userModel.Password = password;
                    context.SaveChanges();
                    return 1;
                }
                else
                {
                    return ReturnCode.NotExist.GetHashCode();
                }
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Updates the forgot password string.
        /// </summary>
        /// <param name="resetCode">The reset code.</param>
        /// <param name="emailId">The email identifier.</param>
        /// <returns></returns>
        public int UpdateForgotPasswordString(string resetCode, string emailId)
        {
            try
            {
                var userModel = context.User.Where(x => x.EmailAddress == emailId && x.IsDelete == false).FirstOrDefault();
                if (userModel != null)
                {
                    userModel.ResetCode = resetCode;
                    context.SaveChanges();
                    return 1;
                }
                else
                {
                    return ReturnCode.NotExist.GetHashCode();
                }
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Determines whether [is code correct] [the specified code].
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool IsCodeCorrect(string code)
        {
            try
            {
                var userModel = context.User.Where(x => x.ResetCode == code && x.IsDelete == false).FirstOrDefault();
                if (userModel != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Resets the code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public int ResetCode(string code)
        {
            try
            {
                var userModel = context.User.Where(x => x.ResetCode == code && x.IsDelete == false).FirstOrDefault();
                if (userModel != null)
                {
                    userModel.ResetCode = string.Empty;
                    context.SaveChanges();
                    return 1;
                }
                else
                {
                    return ReturnCode.NotExist.GetHashCode();
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
