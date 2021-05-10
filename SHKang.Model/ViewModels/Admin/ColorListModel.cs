namespace SHKang.Model.ViewModels.Admin
{

    #region namespaces

    using System.Collections.Generic; 

    #endregion

    public class ColorListModel
    {
        /// <summary>
        /// Gets or sets the total color.
        /// </summary>
        /// <value>
        /// The total color.
        /// </value>
        public string Total { get; set; }

        /// <summary>
        /// Gets or sets the color list.
        /// </summary>
        /// <value>
        /// The color list.
        /// </value>
        public List<ColorListDataModel> Items { get; set; }

    }

    public class ColorListDataModel
    {
        /// <summary>
        /// Gets or sets the color identifier.
        /// </summary>
        /// <value>
        /// The color identifier.
        /// </value>
        public string ColorId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the total products.
        /// </summary>
        /// <value>
        /// The total products.
        /// </value>
        public string TotalProducts { get; set; }

    }
}
