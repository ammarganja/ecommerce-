namespace SHKang.Repository.Repository
{
    #region namespaces

    using SHKang.Core.Helpers;
    using SHKang.Core.Enums;
    using SHKang.Model.Context;
    using SHKang.Model.Models;
    using SHKang.Repository.Interface;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion

    public class ColorRepository : IColor
    {
        #region Private Variables

        /// <summary>
        /// The context
        /// </summary>
        private AppDbContext context;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorRepository"/> class.
        /// </summary>
        /// <param name="_context">The context.</param>
        public ColorRepository(AppDbContext _context)
        {
            context = _context;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns></returns>
        public long AddColor(Color color)
        {
            try
            {
                var colorModel = context.Color.FirstOrDefault(x => x.Name == color.Name && x.IsDelete == false);
                if (colorModel == null)
                {
                    context.Color.Add(color);
                    context.SaveChanges();
                    return color.ColorId;
                }
                else
                {
                    return ReturnCode.AlreadyExist.GetHashCode();
                }
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Deletes the color.
        /// </summary>
        /// <param name="colorId">The color identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool DeleteColor(long colorId)
        {
            try
            {
                var colorModel = context.Color.FirstOrDefault(x => x.ColorId == colorId && x.IsDelete == false);
                if (colorModel != null)
                {
                    colorModel.IsDelete = true;
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Gets the color list.
        /// </summary>
        /// <param name="searchString">The search string.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public List<Color> GetColorList(string searchString)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    return context.Color.Where(x => x.IsDelete == false && x.Name.Contains(searchString)).ToList();
                }
                else
                {
                    return context.Color.Where(x => x.IsDelete == false).ToList();
                }
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Updates the color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public long UpdateColor(Color color)
        {
            try
            {
                var colorModelCheck = context.Color.Where(x => x.Name == color.Name && x.IsDelete == false && x.ColorId != color.ColorId).FirstOrDefault();
                if (colorModelCheck == null)
                {
                    var colorModel = context.Color.Where(x => x.IsDelete == false && x.ColorId == color.ColorId).FirstOrDefault();
                    if (colorModel != null)
                    {
                        colorModel.Name = color.Name;
                        context.SaveChanges();
                        return color.ColorId;
                    }
                    else
                    {
                        return ReturnCode.NotExist.GetHashCode();
                    }
                }
                else
                {
                    return ReturnCode.AlreadyExist.GetHashCode();
                }
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Gets the product list by color ids.
        /// </summary>
        /// <param name="colorIds">The color ids.</param>
        /// <returns></returns>
        public List<ProductColor> GetProductListByColorIds(List<long> colorIds)
        {
            try
            {
                return context.ProductColor.Where(x => colorIds.Contains(x.ColorFK)).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Gets the total color list count.
        /// </summary>
        /// <returns></returns>
        public long GetTotalColorListCount()
        {
            try
            {
                return context.Color.Where(x => x.IsDelete == false).Count();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }
        #endregion
    }
}
