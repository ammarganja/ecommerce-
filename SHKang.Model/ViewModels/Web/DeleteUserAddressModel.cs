namespace SHKang.Model.ViewModels.Web
{

    #region namespaces
    using System.ComponentModel.DataAnnotations; 
    #endregion

    public class DeleteUserAddressModel
    {
        /// <summary>
        /// Gets or sets the user address identifier.
        /// </summary>
        /// <value>
        /// The user address identifier.
        /// </value>
        [Required]
        public long userAddressId { get; set; }
    }
}
