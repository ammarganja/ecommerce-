namespace SHKang.Model.ViewModels.Admin
{

    #region namespaces

    using System.ComponentModel.DataAnnotations; 

    #endregion

    public class AddSizeModel
    {
        /// <summary>
        /// Gets or sets the size identifier.
        /// </summary>
        /// <value>
        /// The size identifier.
        /// </value>
        [Required]
        public string SizeId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the units.
        /// </summary>
        /// <value>
        /// The units.
        /// </value>
        [Required]
        public string Units { get; set; }
    }
}
