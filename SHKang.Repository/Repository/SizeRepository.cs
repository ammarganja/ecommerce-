namespace SHKang.Repository.Repository
{
    using Microsoft.EntityFrameworkCore;
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

    public class SizeRepository : ISize
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
        public SizeRepository(AppDbContext _context)
        {
            context = _context;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the size.
        /// </summary>
        /// <param name="size">The model.</param>
        /// <param name="sizeDetailModel">The size detail model.</param>
        /// <returns></returns>
        public long AddSize(Size size, List<SizeDetail> sizeDetailModel)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {

                    var sizeModel = context.Size.Where(x => x.Name == size.Name && x.IsDelete == false).FirstOrDefault();
                    if (sizeModel == null)
                    {
                        context.Size.Add(size);
                        context.SaveChanges();
                        if (size.SizeId <= 0)
                        {
                            transaction.Rollback();
                            return 0;
                        }
                        else
                        {
                            foreach (var item in sizeDetailModel)
                            {
                                item.SizeFK = size.SizeId;
                                context.SizeDetail.Add(item);
                                context.SaveChanges();
                                if (item.SizeDetailId <= 0)
                                {
                                    transaction.Rollback();
                                    return 0;
                                }
                            }
                            transaction.Commit();
                            return size.SizeId;
                        }
                    }
                    else
                    {
                        return ReturnCode.AlreadyExist.GetHashCode();
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                    throw ex;
                }
            }

        }

        /// <summary>
        /// Deletes the size.
        /// </summary>
        /// <param name="SizeId">The size identifier.</param>
        /// <returns></returns>
        public bool DeleteSize(long sizeId)
        {
            try
            {
                var sizeModel = context.Size.Where(x => x.SizeId == sizeId).FirstOrDefault();
                if (sizeModel != null)
                {
                    sizeModel.IsDelete = true;
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
        /// Gets the size list.
        /// </summary>
        /// <returns></returns>
        public List<SizeDetail> GetSizeList(string searchString)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    return context.SizeDetail.Include(x => x.Size).Where(
                        x => x.Size.IsDelete == false 
                        && (x.Name.Contains(searchString)
                        || x.Size.Name.Contains(searchString))).ToList();
                }
                else
                {
                    return context.SizeDetail.Include(x => x.Size).Where(x => x.Size.IsDelete == false).ToList();
                }
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Updates the size.
        /// </summary>
        /// <param name="size">The model.</param>
        /// <param name="sizeDetailModel">The size detail model.</param>
        /// <returns></returns>
        public long UpdateSize(Size size, List<SizeDetail> sizeDetailModel)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var sizeModelCheck = context.Size.Where(x => x.Name == size.Name && x.IsDelete == false && x.SizeId != size.SizeId).FirstOrDefault();
                    if (sizeModelCheck == null)
                    {
                        var sizeModel = context.Size.Where(x => x.IsDelete == false && x.SizeId == size.SizeId).FirstOrDefault();
                        if (sizeModel != null)
                        {
                            sizeModel.Name = size.Name;
                            context.SaveChanges();
                            if (sizeModel.SizeId <= 0)
                            {
                                transaction.Rollback();
                                return 0;
                            }
                            else
                            {
                                #region Old Delete
                                var sizeDetail = context.SizeDetail.Where(x => x.SizeFK == sizeModel.SizeId).ToList();
                                if (sizeDetailModel != null)
                                {
                                    foreach (var item in sizeDetail)
                                    {
                                        context.SizeDetail.Remove(item);
                                        context.SaveChanges();
                                    }
                                } 
                                #endregion

                                foreach (var item in sizeDetailModel)
                                {
                                    item.SizeFK = size.SizeId;
                                    context.SizeDetail.Add(item);
                                    context.SaveChanges();
                                    if (item.SizeDetailId <= 0)
                                    {
                                        transaction.Rollback();
                                        return 0;
                                    }
                                }
                                transaction.Commit();
                                return size.SizeId;
                            }
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
                    transaction.Rollback();
                    LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Gets the total size count.
        /// </summary>
        /// <returns></returns>
        public long GetTotalSizeCount()
        {
            try
            {
                return context.Size.Where(x => x.IsDelete == false).Count();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Gets the total product size count.
        /// </summary>
        /// <param name="sizeIds">The size ids.</param>
        /// <returns></returns>
        public List<Product> GetTotalProductSizeCount(List<long> sizeIds)
        {
            try
            {
                return context.Product.Where(x => x.IsDelete == false && sizeIds.Contains(x.SizeFK)).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Gets the size details.
        /// </summary>
        /// <param name="SizeId">The size identifier.</param>
        /// <returns></returns>
        public List<SizeDetail> GetSizeDetails(long SizeId)
        {
            try
            {
                return context.SizeDetail.Where(x => x.SizeFK == SizeId).ToList();
            }
            catch (Exception ex)
            {

                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Gets the size details.
        /// </summary>
        /// <param name="sizeIds"></param>
        /// <returns></returns>
        public List<SizeDetail> GetSizeDetailsBySizeIds(List<long> sizeIds)
        {
            try
            {
                return context.SizeDetail.Include(x=>x.Size).Where(x => sizeIds.Contains(x.Size.SizeId)).ToList();
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
