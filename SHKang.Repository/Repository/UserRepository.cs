namespace SHKang.Repository.Repository
{
    #region namespaces
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using SHKang.Core.Enums;
    using SHKang.Core.Helpers;
    using SHKang.Model.Context;
    using SHKang.Model.Models;
    using SHKang.Model.ViewModels.Admin;
    using SHKang.Repository.Interface;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    #endregion

    public class UserRepository : IUser
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
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="_context">The context.</param>
        public UserRepository(AppDbContext _context, IOptions<ConnectionString> _settings)
        {
            context = _context;
            settings = _settings;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the user.
        /// </summary>
        /// <param name="user">The model.</param>
        /// <returns></returns>
        public long AddUser(User user)
        {
            try
            {
                User userModel = new User();
                userModel = context.User.Where(x => x.EmailAddress == user.EmailAddress && x.UserId != user.UserId && x.IsDelete == false).FirstOrDefault();
                if (userModel != null)
                {
                    return ReturnCode.AlreadyExist.GetHashCode();
                }

                if (user.UserId == 0)
                {
                    user = this.BindUserDetail(user);
                    context.User.Add(user);
                }
                else
                {
                    var userModelCheck = context.User.Where(x => x.EmailAddress == user.EmailAddress && x.IsDelete == false && x.UserId != user.UserId).FirstOrDefault();
                    if (userModelCheck == null)
                    {
                        userModel = context.User.Where(x => x.UserId == user.UserId && x.IsDelete == false).FirstOrDefault();
                        if (userModel != null)
                        {
                            userModel.FirstName = user.FirstName;
                            userModel.LastName = user.LastName;
                            userModel.EmailAddress = user.EmailAddress;
                            userModel.PhoneNumber = user.PhoneNumber;
                            userModel.ModifiedOn = DateTime.Now;
                            userModel.Companyname = user.Companyname;
                            userModel.RoleType = user.RoleType;
                        }
                    }
                }

                context.SaveChanges();
                return user.UserId;

            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="UserId">The user identifier.</param>
        /// <returns></returns>
        public bool DeleteUser(long userId)
        {
            try
            {
                var userModel = context.User.Where(x => x.UserId == userId && x.IsDelete == false).FirstOrDefault();
                if (userModel != null)
                {
                    userModel.IsDelete = true;
                    context.SaveChanges();
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
        /// Updates the user.
        /// </summary>
        /// <param name="user">The model.</param>
        /// <returns></returns>
        public long UpdateUser(User user)
        {
            try
            {
                var userModelCheck = context.User.Where(x => x.EmailAddress == user.EmailAddress && x.IsDelete == false && x.UserId != user.UserId).FirstOrDefault();
                if (userModelCheck == null)
                {
                    var userModel = context.User.Where(x => x.UserId == user.UserId && x.IsDelete == false).FirstOrDefault();
                    if (userModel != null)
                    {
                        userModel.FirstName = user.FirstName;
                        userModel.LastName = user.LastName;
                        userModel.Address1 = user.Address1;
                        userModel.Address2 = user.Address2;
                        userModel.EmailAddress = user.EmailAddress;
                        userModel.PhoneNumber = user.PhoneNumber;
                        if (!string.IsNullOrWhiteSpace(user.Password))
                        {
                            userModel.Password = user.Password;
                        }
                        userModel.CountryFK = user.CountryFK;
                        userModel.City = user.City;
                        userModel.StateFK = user.StateFK;
                        userModel.Zipcode = user.Zipcode;
                        userModel.ModifiedOn = DateTime.Now;
                        userModel.Companyname = user.Companyname;
                        userModel.RoleType = user.RoleType;
                        context.SaveChanges();
                        return userModel.UserId;
                    }
                    else
                    {
                        return ReturnCode.NotExist.GetHashCode();
                    }
                }
                else
                {
                    return ReturnCode.AlreadyExist.GetHashCode();
                }
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Gets the user list.
        /// </summary>
        /// <returns></returns>
        public PagedListModel<UserListModel> GetUserList(SearchPaginationListModel searchModel)
        {
            PagedListModel<UserListModel> userListModel = new PagedListModel<UserListModel>();
            try
            {
                SqlParameter[] parameters = new SqlParameter[6];
                parameters[0] = new SqlParameter("PAGENO", searchModel.pageNo);
                parameters[1] = new SqlParameter("RECORDPERPAGE", searchModel.limit);
                parameters[2] = new SqlParameter("SEARCHSTRING", searchModel.searchString);
                parameters[3] = new SqlParameter("COLUMN", searchModel.column);
                parameters[4] = new SqlParameter("DIRECTION", searchModel.direction);
                parameters[5] = new SqlParameter("STATUS", searchModel.Status);
                DataSet ds = DBHelper.GetDataTable("GETUSERLIST", parameters, DBHelper.ParseString(settings.Value.AppDbContext));
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    userListModel.Items = new List<UserListModel>();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow model in dt.Rows)
                        {
                            UserListModel uModel = new UserListModel();
                            if (DBHelper.ParseString(model["RoleType"]).Equals(1))
                            {
                                uModel.IsAdmin = "1";
                            }

                            uModel.UserId = DBHelper.ParseInt64(DBHelper.ParseString(model["UserId"]));
                            uModel.FirstName = DBHelper.ParseString(model["FirstName"]);
                            uModel.LastName = DBHelper.ParseString(model["LastName"]);
                            uModel.CompanyName = DBHelper.ParseString(model["Companyname"]);
                            uModel.EmailAddress = DBHelper.ParseString(model["EmailAddress"]);
                            uModel.PhoneNumber = DBHelper.ParseString(model["PhoneNumber"]);
                            uModel.Status = DBHelper.ParseInt32(model["Status"]);
                            uModel.RoleType = DBHelper.ParseInt32(model["RoleType"]);
                            uModel.UserAddressCount = DBHelper.ParseInt32(model["UserAddressCount"]);
                            userListModel.Items.Add(uModel);
                        }
                    }

                    DataTable dt1 = ds.Tables[1];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        var row = dt1.Rows[0];
                        userListModel.Total = DBHelper.ParseInt32(row["Total"]);
                    }
                }

                //return context.User.Include(x => x.Country).Include(x => x.State).Where(x => x.IsDelete == false).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
            return userListModel;
        }

        /// <summary>
        /// Gets the user detail.
        /// </summary>
        /// <param name="UserId">The user identifier.</param>
        /// <returns></returns>
        public User GetUserDetail(long userId)
        {
            try
            {
                var userModel = context.User.Include(x => x.Country).Include(x => x.State).Where(x => x.UserId == userId && x.IsDelete == false).FirstOrDefault();
                if (userModel != null)
                {
                    return userModel;
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

        /// <summary>
        /// Adds the new user address.
        /// </summary>
        /// <param name="userAddress">The model.</param>
        /// <returns></returns>
        public long AddNewUserAddress(UserAddress userAddress)
        {
            try
            {
                var userAddressModel = context.UserAddress.Where(x => x.Address1 == userAddress.Address1 && x.Address2 == userAddress.Address2 && x.City == userAddress.City && x.CountryFK == userAddress.CountryFK && x.StateFK == userAddress.StateFK && x.UserFK == x.UserFK && x.Zipcode == userAddress.Zipcode && x.IsDelete == false).FirstOrDefault();
                if (userAddressModel == null)
                {
                    var checkPrimary = context.UserAddress.Where(x => x.UserFK == userAddress.UserFK && x.IsPrimary == true && x.IsDelete == false).Count();
                    if (checkPrimary > 0)
                    {
                        return ReturnCode.IsPrimaryExist.GetHashCode();
                    }
                    context.UserAddress.Add(userAddress);
                    context.SaveChanges();
                    return userAddress.UserAddressId;
                }
                else
                {
                    return ReturnCode.AlreadyExist.GetHashCode();
                }
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Updates the user address.
        /// </summary>
        /// <param name="userAddress">The model.</param>
        /// <returns></returns>
        public long UpdateUserAddress(UserAddress userAddress)
        {
            try
            {
                var userAddressModelCheck = context.UserAddress.Where(x => x.Address1 == userAddress.Address1 && x.Address2 == userAddress.Address2 && x.City == userAddress.City && x.CountryFK == userAddress.CountryFK && x.StateFK == userAddress.StateFK && x.UserFK == x.UserFK && x.Zipcode == userAddress.Zipcode && x.IsDelete == false && x.UserAddressId != userAddress.UserAddressId).FirstOrDefault();
                if (userAddressModelCheck == null)
                {
                    var checkPrimary = context.UserAddress.Where(x => x.UserFK == userAddress.UserFK && x.UserAddressId != userAddress.UserAddressId && x.IsPrimary == true && x.IsDelete==false).Count();
                    if (checkPrimary > 0)
                    {
                        return ReturnCode.IsPrimaryExist.GetHashCode();
                    }
                    var userAddressModel = context.UserAddress.Where(x => x.IsDelete == false && x.UserAddressId == userAddress.UserAddressId).FirstOrDefault();
                    if (userAddressModel != null)
                    {
                        userAddressModel.Address1 = userAddress.Address1;
                        userAddressModel.Address2 = userAddress.Address2;
                        userAddressModel.City = userAddress.City;
                        userAddressModel.CountryFK = userAddress.CountryFK;
                        userAddressModel.StateFK = userAddress.StateFK;
                        userAddressModel.Zipcode = userAddress.Zipcode;
                        userAddressModel.CompanyName = userAddress.CompanyName;
                        userAddressModel.FullName = userAddress.FullName;
                        userAddressModel.PhoneNumber = userAddress.PhoneNumber;
                        userAddressModel.Email = userAddress.Email;
                        userAddressModel.ModifiedOn = userAddress.ModifiedOn;
                        userAddressModel.IsPrimary = userAddress.IsPrimary;
                        context.SaveChanges();
                        return userAddress.UserAddressId;
                    }
                    else
                    {
                        return ReturnCode.NotExist.GetHashCode();
                    }
                }
                else
                {
                    return ReturnCode.AlreadyExist.GetHashCode();
                }
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Deletes the user address.
        /// </summary>
        /// <param name="UserAddressId">The user address identifier.</param>
        /// <returns></returns>
        public bool DeleteUserAddress(long userAddressId)
        {
            try
            {
                var userAddressModel = context.UserAddress.Where(x => x.UserAddressId == userAddressId).FirstOrDefault();
                if (userAddressModel != null)
                {
                    userAddressModel.IsDelete = true;
                    context.SaveChanges();
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
        /// Users the address list.
        /// </summary>
        /// <param name="UserId">The user identifier.</param>
        /// <returns></returns>
        public UserAddressListModelAdmin UserAddressList(long userId, int pageNo, int recordsPerPage, string column, string direction)
        {
            UserAddressListModelAdmin uerAddressListModel = new UserAddressListModelAdmin();
            List<UserAddressListDataModel> userAddressListDataModel = new List<UserAddressListDataModel>();
            try
            {
                SqlParameter[] parameter = new SqlParameter[5];
                parameter[0] = new SqlParameter("PAGENO", pageNo);
                parameter[1] = new SqlParameter("RECORDPERPAGE", recordsPerPage);
                parameter[2] = new SqlParameter("COLUMN", column);
                parameter[3] = new SqlParameter("DIRECTION", direction);
                parameter[4] = new SqlParameter("USERID", userId);
                DataSet dataSet = DBHelper.GetDataTable("GETUSERADDRESSLIST", parameter, DBHelper.ParseString(settings.Value.AppDbContext));
                if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0)
                {
                    DataTable dtAddressList = dataSet.Tables[0];
                    DataTable dtTotal = dataSet.Tables[1];

                    if (dtAddressList != null && dtAddressList.Rows.Count > 0)
                    {
                        foreach (DataRow item in dtAddressList.Rows)
                        {
                            UserAddressListDataModel userAddressListDataModelObject = new UserAddressListDataModel();
                            userAddressListDataModelObject.Address1 = DBHelper.ParseString(item["Address1"]);
                            userAddressListDataModelObject.Address2 = DBHelper.ParseString(item["Address2"]);
                            userAddressListDataModelObject.CompanyName = DBHelper.ParseString(item["CompanyName"]);
                            userAddressListDataModelObject.FullName = DBHelper.ParseString(item["FullName"]);
                            userAddressListDataModelObject.PhoneNumber = DBHelper.ParseString(item["PhoneNumber"]);
                            userAddressListDataModelObject.State = DBHelper.ParseString(item["StateName"]);
                            userAddressListDataModelObject.Country = DBHelper.ParseString(item["CountryName"]);
                            userAddressListDataModelObject.City = DBHelper.ParseString(item["City"]);
                            userAddressListDataModelObject.Zipcode = DBHelper.ParseString(item["Zipcode"]);
                            userAddressListDataModelObject.UserAddressId = DBHelper.ParseInt64(item["UserAddressId"]);
                            userAddressListDataModelObject.CountryId = DBHelper.ParseInt64(item["CountryFK"]);
                            userAddressListDataModelObject.StateId = DBHelper.ParseInt64(item["StateFK"]);
                            userAddressListDataModelObject.EmailId = DBHelper.ParseString(item["Email"]);
                            userAddressListDataModelObject.EmailId = DBHelper.ParseString(item["Email"]);
                            userAddressListDataModelObject.IsPrimary = DBHelper.ParseBoolean(item["IsPrimary"]);
                            userAddressListDataModel.Add(userAddressListDataModelObject);
                        }
                        uerAddressListModel.Items = userAddressListDataModel;
                        uerAddressListModel.Total = DBHelper.ParseString(dtTotal.Rows[0][0]);
                    }
                }
                return uerAddressListModel;

            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Updates the password.
        /// </summary>
        /// <param name="oldPassword">The old password.</param>
        /// <param name="newPassword">The new password.</param>
        /// <param name="UserId">The user identifier.</param>
        /// <returns></returns>
        public long UpdatePassword(string oldPassword, string newPassword, long userId)
        {
            try
            {
                var userModel = context.User.Where(x => x.UserId == userId && x.IsDelete == false).FirstOrDefault();
                if (userModel != null)
                {
                    if (userModel.Password.Equals(oldPassword))
                    {
                        userModel.Password = DBHelper.ParseString(newPassword);
                        userModel.ModifiedOn = DateTime.Now;
                        context.SaveChanges();
                        return userModel.UserId;
                    }
                    else
                    {
                        return ReturnCode.NotMatching.GetHashCode();
                    }
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
        /// Gets the user password.
        /// </summary>
        /// <param name="UserId">The user identifier.</param>
        /// <returns></returns>
        public string GetUserPassword(long userId)
        {
            try
            {
                return context.User.Where(x => x.UserId == userId && x.IsDelete == false).Select(x => x.Password).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Changes the user status.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public bool ChangeUserStatus(long userId, byte status)
        {
            try
            {
                var user = context.User.Where(x => x.IsDelete == false && x.UserId == userId).FirstOrDefault();
                if (user != null)
                {
                    user.Status = status;
                    context.SaveChanges();
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
        /// Gets all user list for drop down.
        /// </summary>
        /// <returns></returns>
        public List<User> GetAllUserListForDropDown()
        {
            try
            {
                return context.User.Where(x => x.IsDelete == false).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Gets all user address list for drop down.
        /// </summary>
        /// <returns></returns>
        public List<UserAddress> GetAllUserAddressListForDropDown(long userId)
        {
            try
            {
                return context.UserAddress.Where(x => x.IsDelete == false && x.UserFK == userId).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Gets the user detail.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>
        /// User Details to save
        /// </returns>
        private User BindUserDetail(User user)
        {
            User userModel = new User();
            userModel.FirstName = user.FirstName;
            userModel.LastName = user.LastName;
            userModel.EmailAddress = user.EmailAddress;
            userModel.PhoneNumber = user.PhoneNumber;
            userModel.ModifiedOn = DateTime.Now;
            userModel.Companyname = user.Companyname;
            userModel.RoleType = user.RoleType;
            userModel.Password = user.Password;
            userModel.CountryFK = null;
            userModel.StateFK = null;
            userModel.CreatedOn = DateTime.Now;
            userModel.ModifiedOn = DateTime.Now;
            userModel.Status = (byte)UserStatus.Active.GetHashCode();
            return userModel;
        }
        #endregion
    }
}
