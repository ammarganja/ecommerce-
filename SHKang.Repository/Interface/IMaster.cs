namespace SHKang.Repository.Interface
{
    #region namespaces
    using SHKang.Model.Models;
    using SHKang.Model.ViewModels.Admin;
    using System.Collections.Generic; 
    #endregion

    public interface IMaster
    {
        /// <summary>
        /// Gets the colors.
        /// </summary>
        /// <returns></returns>
        List<Color> GetColors();

        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <returns></returns>
        List<Size> GetSize();

        /// <summary>
        /// Gets the size ratio.
        /// </summary>
        /// <returns></returns>
        List<SizeRatio> GetSizeRatio();

        /// <summary>
        /// Gets the country list.
        /// </summary>
        /// <returns></returns>
        List<SelectListItemModel> GetCountryList();

        /// <summary>
        /// Gets the state list.
        /// </summary>
        /// <param name="countryId">The country identifier.</param>
        /// <returns></returns>
        List<SelectListItemModel> GetStateList(long countryId);
    }
}
