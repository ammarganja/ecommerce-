namespace SHKang.Repository.Interface
{
    #region namespaces
    using SHKang.Model.Models;
    using System.Collections.Generic; 
    #endregion

    public interface ISizeRatio
    {

        /// <summary>
        /// Adds the size ratio.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        long AddSizeRatio(SizeRatio sizeRatio);


        /// <summary>
        /// Updates the size ratio.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        long UpdateSizeRatio(SizeRatio sizeRatio);


        /// <summary>
        /// Deletes the size ratio.
        /// </summary>
        /// <param name="SizeRatioId">The size ratio identifier.</param>
        /// <returns></returns>
        bool DeleteSizeRatio(long sizeRatioId);


        /// <summary>
        /// Gets the size ratio list.
        /// </summary>
        /// <param name="searchString">The search string.</param>
        /// <returns></returns>
        List<SizeRatio> GetSizeRatioList(string searchString);
    }
}
