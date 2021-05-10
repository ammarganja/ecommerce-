namespace SHKang.Repository.Repository
{
    #region namespaces
    using SHKang.Core.Helpers;
    using SHKang.Model.Context;
    using SHKang.Model.ViewModels.Admin;
    using SHKang.Repository.Interface;
    using System;
    using System.Linq;

    #endregion

    public class LoginRepository : ILogin
    {
        #region Private Variables

        /// <summary>
        /// The context
        /// </summary>
        private AppDbContext context;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginRepository"/> class.
        /// </summary>
        /// <param name="_context">The context.</param>

        public LoginRepository(AppDbContext _context)
        {
            context = _context;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Logins the specified email identifier.
        /// </summary>
        /// <param name="EmailId">The email identifier.</param>
        /// <param name="Password">The password.</param>
        /// <returns></returns>
        public UserModel Login(string emailId, string password,int roleType)
        {
            try
            {
                UserModel model = new UserModel();
                var userModel = context.User.Where(x => x.EmailAddress == emailId && x.Password == password && x.IsDelete == false && x.RoleType == roleType).FirstOrDefault();
                if (userModel != null)
                {
                    model.EmailAddress = userModel.EmailAddress;
                    model.FirstName = userModel.FirstName;
                    model.LastName = userModel.LastName;
                    model.PhoneNumber = userModel.PhoneNumber;
                    model.UserId = userModel.UserId;
                    return model;
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
