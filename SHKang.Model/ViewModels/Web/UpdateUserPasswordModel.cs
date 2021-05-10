namespace SHKang.Model.ViewModels.Web
{
    #region namespaces
    using System.ComponentModel.DataAnnotations; 
    #endregion


    public class UpdateUserPasswordModel
    {
        [Required]
        /// <summary>
        /// Gets or sets the old password.
        /// </summary>
        /// <value>
        /// The old password.
        /// </value>
        public string oldPassword { get; set; }

        [Required]
        /// <summary>
        /// Gets or sets the new password.
        /// </summary>
        /// <value>
        /// The new password.
        /// </value>
        public string newPassword { get; set; }

        [Required]
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public long userId { get; set; }
    }
}
