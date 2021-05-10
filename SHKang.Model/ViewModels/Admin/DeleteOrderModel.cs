namespace SHKang.Model.ViewModels.Admin
{

    #region namespaces
    using System.ComponentModel.DataAnnotations;
    #endregion

    public class DeleteOrderModel
    {
        /// <summary>
        /// Gets or sets the order identifier.
        /// </summary>
        /// <value>
        /// The order identifier.
        /// </value>
        [Required]
        public long OrderId { get; set; }
    }
}
