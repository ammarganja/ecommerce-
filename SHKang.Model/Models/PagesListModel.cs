using System.Collections.Generic;

namespace SHKang.Model.Models
{
    /// <summary>
    /// The PagesListModel
    /// </summary>
    public class PagedListModel<T>
    {
        #region Public Properties
        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public List<T> Items { get; set; }
        /// <summary>
        /// Gets or sets the total.
        /// </summary>
        /// <value>
        /// The total.
        /// </value>
        public long Total { get; set; }
        #endregion
    }
}
