namespace SHKang.Model.ViewModels.Admin
{
    #region namespaces

    using System.ComponentModel.DataAnnotations; 

    #endregion

    public class DeleteColorModel
    {
        /// <summary>
        /// Gets or sets the color identifier.
        /// </summary>
        /// <value>
        /// The color identifier.
        /// </value>
        [Required]
        public string colorId { get; set; }
    }
}
