namespace SHKang.Model.ViewModels.Web
{

    #region namespaces

    using System.ComponentModel.DataAnnotations; 

    #endregion

    public class ContentDetailModel
    {
        [Required]
        /// <summary>
        /// Gets or sets the short name.
        /// </summary>
        /// <value>
        /// The short name.
        /// </value>
        public string shortName { get; set; }
    }
}
