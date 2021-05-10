namespace SHKang.Model.ViewModels.Admin
{
    public class SearchProductListModel:SearchPaginationListModel
    {
        /// <summary>
        /// Gets or sets the color identifier.
        /// </summary>
        /// <value>
        /// The color identifier.
        /// </value>
        public string ColorId { get; set; }

        /// <summary>
        /// Gets or sets the category type identifier.
        /// </summary>
        /// <value>
        /// The category type identifier.
        /// </value>
        public string CategoryTypeId { get; set; }

        /// <summary>
        /// Gets or sets the size identifier.
        /// </summary>
        /// <value>
        /// The size identifier.
        /// </value>
        public string SizeId { get; set; }
    }
}
