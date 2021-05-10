namespace SHKang.Repository.Interface
{
    #region namespaces
    using SHKang.Model.ViewModels.Admin; 

    #endregion

    public interface ILogin
    {
        /// <summary>
        /// Logins the specified email identifier.
        /// </summary>
        /// <param name="EmailId">The email identifier.</param>
        /// <param name="Password">The password.</param>
        /// <returns></returns>
        UserModel Login(string emailId, string password, int roleType);
    }
}
