namespace SHKang.Repository.Interface
{
    #region namespaces
    using SHKang.Model.Models;
    using SHKang.Model.ViewModels.Admin;
    using System.Collections.Generic;

    #endregion
    public interface IUser
    {
        /// <summary>
        /// Adds the user.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        long AddUser(User user);

        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="UserId">The user identifier.</param>
        /// <returns></returns>
        bool DeleteUser(long userId);

        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        long UpdateUser(User user);

        /// <summary>
        /// Gets the user list.
        /// </summary>
        /// <param name="searchModel">The search model.</param>
        /// <returns></returns>
        PagedListModel<UserListModel> GetUserList(SearchPaginationListModel searchModel);

        /// <summary>
        /// Gets the user detail.
        /// </summary>
        /// <param name="UserId">The user identifier.</param>
        /// <returns></returns>
        User GetUserDetail(long userId);

        /// <summary>
        /// Adds the new user address.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        long AddNewUserAddress(UserAddress userAddress);

        /// <summary>
        /// Updates the user address.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        long UpdateUserAddress(UserAddress userAddress);

        /// <summary>
        /// Deletes the user address.
        /// </summary>
        /// <param name="UserAddressId">The user address identifier.</param>
        /// <returns></returns>
        bool DeleteUserAddress(long userAddressId);

        /// <summary>
        /// Users the address list.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="pageNo">The page no.</param>
        /// <param name="recordsPerPage">The records per page.</param>
        /// <param name="searchString">The search string.</param>
        /// <param name="column">The column.</param>
        /// <param name="direction">The direction.</param>
        /// <returns></returns>
        UserAddressListModelAdmin UserAddressList(long userId, int pageNo, int recordsPerPage,string column,string direction);

        /// <summary>
        /// Updates the password.
        /// </summary>
        /// <param name="oldPassword">The old password.</param>
        /// <param name="newPassword">The new password.</param>
        /// <param name="UserId">The user identifier.</param>
        /// <returns></returns>
        long UpdatePassword(string oldPassword, string newPassword, long userId);

        /// <summary>
        /// Gets the user password.
        /// </summary>
        /// <param name="UserId">The user identifier.</param>
        /// <returns></returns>
        string GetUserPassword(long userId);

        /// <summary>
        /// Changes the user status.
        /// </summary>
        /// <param name="userIdId">The user identifier identifier.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        bool ChangeUserStatus(long userIdId, byte status);

        /// <summary>
        /// Gets all user list for drop down.
        /// </summary>
        /// <returns></returns>
        List<User> GetAllUserListForDropDown();

        /// <summary>
        /// Gets all user address list for drop down.
        /// </summary>
        /// <returns></returns>
        List<UserAddress> GetAllUserAddressListForDropDown(long userId);
    }
}
