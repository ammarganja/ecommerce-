namespace SHKang.Repository.Repository
{
    #region namespaces
    using SHKang.Core.Enums;
    using SHKang.Core.Helpers;
    using SHKang.Model.Context;
    using SHKang.Model.Models;
    using SHKang.Repository.Interface;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion

    public class SizeRatioRepository : ISizeRatio
    {
        #region Private Variables

        /// <summary>
        /// The context
        /// </summary>
        private AppDbContext context;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SizeRepository"/> class.
        /// </summary>
        /// <param name="_context">The context.</param>
        public SizeRatioRepository(AppDbContext _context)
        {
            context = _context;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the size ratio.
        /// </summary>
        /// <param name="sizeRatio">The model.</param>
        /// <returns></returns>
        public long AddSizeRatio(SizeRatio sizeRatio)
        {
            try
            {
                var sizeRatioModel = context.SizeRatio.Where(x => x.Name == sizeRatio.Name && x.IsDelete == false).FirstOrDefault();
                if (sizeRatioModel == null)
                {
                    context.SizeRatio.Add(sizeRatio);
                    context.SaveChanges();
                    return sizeRatio.SizeRatioId;
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
        /// Deletes the size ratio.
        /// </summary>
        /// <param name="SizeRatioId">The size ratio identifier.</param>
        /// <returns></returns>
        public bool DeleteSizeRatio(long sizeRatioId)
        {
            try
            {
                var sizeRatioModel = context.SizeRatio.Where(x => x.SizeRatioId == sizeRatioId).FirstOrDefault();
                if (sizeRatioModel != null)
                {
                    sizeRatioModel.IsDelete = true;
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
        /// Gets the size ratio list.
        /// </summary>
        /// <returns></returns>
        public List<SizeRatio> GetSizeRatioList(string searchString)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    return context.SizeRatio.Where(x => x.IsDelete == false && x.Name.Contains(searchString)).ToList();
                }
                else
                {
                    return context.SizeRatio.Where(x => x.IsDelete == false).ToList();
                }
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }


        /// <summary>
        /// Updates the size ratio.
        /// </summary>
        /// <param name="sizeRatio">The model.</param>
        /// <returns></returns>
        public long UpdateSizeRatio(SizeRatio sizeRatio)
        {
            try
            {
                var sizeRatioModelCheck = context.SizeRatio.Where(x => x.Name == sizeRatio.Name && x.IsDelete == false && x.SizeRatioId != sizeRatio.SizeRatioId).FirstOrDefault();
                if (sizeRatioModelCheck == null)
                {
                    var sizeRatioModel = context.SizeRatio.Where(x => x.IsDelete == false && x.SizeRatioId == sizeRatio.SizeRatioId).FirstOrDefault();
                    if (sizeRatioModel != null)
                    {
                        sizeRatioModel.Name = sizeRatio.Name;
                        context.SaveChanges();
                        return sizeRatio.SizeRatioId;
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

        #endregion
    }
}
