namespace SHKang.Repository.Interface
{

    #region namespaces

    using SHKang.Model.Models;
    using System.Collections.Generic; 

    #endregion

    public interface IColor
    {

        /// <summary>
        /// Adds the color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns></returns>
        long AddColor(Color color);


        /// <summary>
        /// Updates the color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns></returns>
        long UpdateColor(Color color);


        /// <summary>
        /// Deletes the color.
        /// </summary>
        /// <param name="colorId">The color identifier.</param>
        /// <returns></returns>
        bool DeleteColor(long colorId);


        /// <summary>
        /// Gets the color list.
        /// </summary>
        /// <param name="searchString">The search string.</param>
        /// <returns></returns>
        List<Color> GetColorList(string searchString);


        /// <summary>
        /// Gets the product list by color ids.
        /// </summary>
        /// <param name="colorIds">The color ids.</param>
        /// <returns></returns>
        List<ProductColor> GetProductListByColorIds(List<long> colorIds);


        /// <summary>
        /// Gets the total color list count.
        /// </summary>
        /// <returns></returns>
        long GetTotalColorListCount();
    }
}
