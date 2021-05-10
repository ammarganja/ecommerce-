namespace SHKang.Repository.Interface
{
    #region namespaces
    using SHKang.Model.Models;
    using System.Collections.Generic;

    #endregion
    public interface IContent
    {
        /// <summary>
        /// Adds the content.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        long AddContent(ContentDetails contentDetails);

        /// <summary>
        /// Deletes the content.
        /// </summary>
        /// <param name="ContentDetailId">The content detail identifier.</param>
        /// <returns></returns>
        bool DeleteContent(long contentDetailId, long updatedBy);

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        long UpdateContent(ContentDetails contentModel);

        /// <summary>
        /// Contents the details.
        /// </summary>
        /// <returns></returns>
        List<ContentDetails> ContentDetails(string searchString);


        /// <summary>
        /// Gets the content detail.
        /// </summary>
        /// <param name="Content">The content.</param>
        /// <returns></returns>
        ContentDetails GetContentDetail(string shortName);
    }
}
