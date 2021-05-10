namespace SHKang.Model.ViewModels.Web
{

    #region Namespaces
    using System.ComponentModel.DataAnnotations; 
    #endregion


    public class CheckPromocodeModel
    {

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
         [Required]
        public long UserId { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [Required]
        public string Code { get; set; }
    }
}
