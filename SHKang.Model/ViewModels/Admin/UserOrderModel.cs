namespace SHKang.Model.ViewModels.Admin
{

    #region namespaces
    using System.Collections.Generic; 
    #endregion

    public class UserOrderModel
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        /// <value>
        /// The name of the company.
        /// </value>
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the mobile number.
        /// </summary>
        /// <value>
        /// The mobile number.
        /// </value>
        public string MobileNumber { get; set; }

        /// <summary>
        /// Gets or sets the email numver.
        /// </summary>
        /// <value>
        /// The email numver.
        /// </value>
        public string EmailNumber { get; set; }

        /// <summary>
        /// Gets or sets the address list.
        /// </summary>
        /// <value>
        /// The address list.
        /// </value>
        public List<SelectListItemModel> addressList { get; set; }
    }
}
