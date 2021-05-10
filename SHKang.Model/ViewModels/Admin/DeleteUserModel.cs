namespace SHKang.Model.ViewModels.Admin
{

    #region namespaces
    using System.ComponentModel.DataAnnotations; 
    #endregion

    public class DeleteUserModel:SearchPaginationListModel
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        [Required]
        public string userId { get; set; }
    }

    public class ChangeUserStatus
    {
        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        [Required]
        public long UserId { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [Required]
        public byte Status { get; set; }
    }
}
