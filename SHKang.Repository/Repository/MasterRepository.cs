
namespace SHKang.Repository.Repository
{
    #region namespaces

    using Microsoft.EntityFrameworkCore;
    using SHKang.Core.Helpers;
    using SHKang.Model.Context;
    using SHKang.Model.Models;
    using SHKang.Model.ViewModels.Admin;
    using SHKang.Repository.Interface;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion

    public class MasterRepository : IMaster
    {
        #region Private Variables

        /// <summary>
        /// The context
        /// </summary>
        private AppDbContext context;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MasterRepository"/> class.
        /// </summary>
        /// <param name="_context">The context.</param>
        public MasterRepository(AppDbContext _context)
        {
            context = _context;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets the colors.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<Color> GetColors()
        {
            try
            {
                return context.Color.Where(x => x.IsDelete == false).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<Size> GetSize()
        {
            try
            {
                return context.Size.Where(x => x.IsDelete == false).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Gets the size ratio.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<SizeRatio> GetSizeRatio()
        {
            try
            {
                return context.SizeRatio.Where(x => x.IsDelete == false).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Gets the colors.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<SelectListItemModel> GetCountryList()
        {
            try
            {
                var countries = (from c in context.Country
                                 select new SelectListItemModel
                                 {
                                     itemName = c.Name,
                                     id = c.CountryId.ToString()
                                 }).ToList();
                return countries;
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Gets the state list.
        /// </summary>
        /// <param name="countryId">The country identifier.</param>
        /// <returns></returns>
        public List<SelectListItemModel> GetStateList(long countryId)
        {
            try
            {
                var countries = (from s in context.State
                                 where s.CountryFK == countryId
                                 select new SelectListItemModel
                                 {
                                     itemName = s.Name,
                                     id = s.StateId.ToString(),
                                 }).ToList();
                return countries;
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
