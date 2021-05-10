namespace SHKang.Core.ModelHelper
{
    #region namespaces

    using SHKang.Core.Enums;
    using SHKang.Core.Helpers;
    using SHKang.Model.Models;
    using SHKang.Model.ViewModels.Admin;
    using SHKang.Model.ViewModels.Web;
    using System;
    using System.Collections.Generic;
    #endregion

    public static class UserHelper
    {
        /// <summary>
        /// Converts to user model.
        /// </summary>
        /// <param name="addNewUserModel">The model.</param>
        /// <returns></returns>
        public static User BindUserModel(AddNewUserModel addNewUserModel)
        {
            User userModel = new User();
            userModel.UserId = addNewUserModel.UserId;
            userModel.FirstName = addNewUserModel.FirstName;
            userModel.LastName = addNewUserModel.LastName;
            userModel.Address1 = addNewUserModel.Address1;
            userModel.Address2 = addNewUserModel.Address2;
            userModel.City = addNewUserModel.City;
            if (!string.IsNullOrWhiteSpace(addNewUserModel.CountryId))
                userModel.CountryFK = DBHelper.ParseInt64(addNewUserModel.CountryId);
            if (!string.IsNullOrWhiteSpace(addNewUserModel.StateId))
                userModel.StateFK = DBHelper.ParseInt64(addNewUserModel.StateId);
            userModel.Zipcode = addNewUserModel.ZipCode;
            userModel.EmailAddress = addNewUserModel.EmailAddress;
            userModel.Password = addNewUserModel.Password;
            userModel.Companyname = addNewUserModel.CompanyName;
            userModel.RoleType = 2;
            if (!string.IsNullOrWhiteSpace(addNewUserModel.RoleType) && addNewUserModel.RoleType.Equals("1"))
                userModel.RoleType = (int)RoleType.Admin;
            userModel.PhoneNumber = addNewUserModel.PhoneNumber;
            return userModel;
        }

        /// <summary>
        /// Converts to user list model.
        /// </summary>
        /// <param name="userList">The modellist.</param>
        /// <returns></returns>
        public static List<UserListModel> BindUserListModel(List<User> userList)
        {
            List<UserListModel> userListModel = new List<UserListModel>();
            foreach (var model in userList)
            {
                UserListModel uModel = new UserListModel();
                if (model.RoleType.Equals((int)RoleType.Admin))
                    uModel.IsAdmin = "1";
                uModel.UserId = model.UserId;
                uModel.FirstName = model.FirstName;
                uModel.LastName = model.LastName;
                uModel.Address1 = model.Address1;
                uModel.Address2 = model.Address2;
                uModel.City = model.City;
                uModel.Country = model.Country.Name;
                uModel.State = model.State.Name;
                uModel.ZipCode = model.Zipcode;
                uModel.EmailAddress = model.EmailAddress;
                uModel.Password = model.Password;
                uModel.CompanyName = model.Companyname;
                uModel.PhoneNumber = model.PhoneNumber;
                userListModel.Add(uModel);
            }
            return userListModel;
        }

        /// <summary>
        /// Converts to user list model v1.
        /// </summary>
        /// <param name="userModel">The model.</param>
        /// <returns></returns>
        public static UserListModel BindUserListModel(User userModel)
        {
            UserListModel userListModel = new UserListModel();
            if (userModel != null)
            {
                userListModel.UserId = userModel.UserId;
                userListModel.FirstName = userModel.FirstName;
                userListModel.LastName = userModel.LastName;
                userListModel.Password = EncryptDecryptHelper.Decrypt(userModel.Password);
                userListModel.PhoneNumber = userModel.PhoneNumber;
                userListModel.EmailAddress = userModel.EmailAddress;
                userListModel.Address1 = userModel.Address1;
                userListModel.Address2 = userModel.Address2;
                userListModel.City = userModel.City;
                userListModel.CompanyName = userModel.Companyname;
                if (userModel.Country!=null)
                {
                    userListModel.Country = userModel.Country.Name; 
                }
                userListModel.ZipCode = userModel.Zipcode;
                if (userModel.State != null)
                    userListModel.State = userModel.State.Name;
                if (userModel.RoleType.Equals((int)RoleType.Admin))
                    userListModel.IsAdmin = "1";
            }
            return userListModel;
        }

        /// <summary>
        /// Converts to user address.
        /// </summary>
        /// <param name="addNewUserModel">The model.</param>
        /// <returns></returns>
        public static UserAddress BindUserAddress(AddNewUserModel addNewUserModel)
        {
            UserAddress userAddressModel = new UserAddress();
            if (addNewUserModel != null)
            {
                userAddressModel.Address1 = addNewUserModel.Address1;
                userAddressModel.Address2 = addNewUserModel.Address2;
                userAddressModel.City = addNewUserModel.City;
                userAddressModel.CountryFK = DBHelper.ParseInt64(addNewUserModel.CountryId);
                userAddressModel.CreatedOn = DateTime.Now;
                userAddressModel.StateFK = DBHelper.ParseInt64(addNewUserModel.StateId);
                userAddressModel.Zipcode = addNewUserModel.ZipCode;
            }
            return userAddressModel;
        }

        /// <summary>
        /// Converts to user address.
        /// </summary>
        /// <param name="addUserAddressModel">The model.</param>
        /// <returns></returns>
        public static UserAddress BindUserAddress(AddUserAddressModel addUserAddressModel)
        {
            UserAddress userAddressModel = new UserAddress();
            if (!string.IsNullOrWhiteSpace(addUserAddressModel.UserAddressId) && DBHelper.ParseInt64(addUserAddressModel.UserAddressId) <= 0)
            {
                userAddressModel.CreatedOn = DateTime.Now;
            }
            else
            {
                userAddressModel.ModifiedOn = DateTime.Now;
                userAddressModel.UserAddressId = DBHelper.ParseInt64(addUserAddressModel.UserAddressId);
            }
            userAddressModel.Address1 = addUserAddressModel.Address1;
            userAddressModel.Address2 = addUserAddressModel.Address2;
            userAddressModel.City = addUserAddressModel.City;
            userAddressModel.CountryFK = DBHelper.ParseInt64(addUserAddressModel.CountryId);
            userAddressModel.StateFK = DBHelper.ParseInt64(addUserAddressModel.StateId);
            userAddressModel.Zipcode = addUserAddressModel.Zipcode;
            userAddressModel.CompanyName = addUserAddressModel.CompanyName;
            userAddressModel.Email = addUserAddressModel.Email;
            userAddressModel.FullName = addUserAddressModel.FullName;
            userAddressModel.PhoneNumber = addUserAddressModel.PhoneNumber;
            userAddressModel.IsPrimary = addUserAddressModel.IsPrimary;
            userAddressModel.UserFK = DBHelper.ParseInt64(addUserAddressModel.UserId);
            return userAddressModel;
        }

        /// <summary>
        /// Converts to user address list model.
        /// </summary>
        /// <param name="userAddressModel">The model.</param>
        /// <returns></returns>
        public static List<UserAddressListModel> BindUserAddressListModel(List<UserAddress> userAddressModel)
        {
            List<UserAddressListModel> userAddressListModel = new List<UserAddressListModel>();
            if (userAddressModel != null)
            {
                foreach (var item in userAddressModel)
                {
                    userAddressListModel.Add(new UserAddressListModel
                    {
                        Address1 = item.Address1,
                        Address2 = item.Address2,
                        City = item.City,
                        CountryId = DBHelper.ParseString(item.CountryFK),
                        StateId = DBHelper.ParseString(item.StateFK),
                        UserAddressId = DBHelper.ParseString(item.UserAddressId),
                        UserId = DBHelper.ParseString(item.UserFK),
                        Zipcode = item.Zipcode
                    });
                }
            }
            return userAddressListModel;
        }

        /// <summary>
        /// Binds the user order model.
        /// </summary>
        /// <param name="userDetail">The user detail.</param>
        /// <returns></returns>
        public static UserOrderModel BindUserOrderModel(User userDetail)
        {
            UserOrderModel userOrderModel = new UserOrderModel();
            if (userDetail != null)
            {
                userOrderModel.CompanyName = userDetail.Companyname;
                userOrderModel.FirstName = userDetail.FirstName;
                userOrderModel.LastName = userDetail.LastName;
                userOrderModel.MobileNumber = userDetail.PhoneNumber;
                userOrderModel.EmailNumber = userDetail.EmailAddress;
            }
            return userOrderModel;
        }
    }
}
