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
    public class PromoCodeRepository : IPromoCode
    {
        #region Private Variables

        /// <summary>
        /// The context
        /// </summary>
        private AppDbContext context;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="PromoCodeRepository"/> class.
        /// </summary>
        /// <param name="_context">The context.</param>
        public PromoCodeRepository(AppDbContext _context)
        {
            context = _context;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the promocode.
        /// </summary>
        /// <param name="promoCode">The model.</param>
        /// <returns></returns>
        public long AddPromocode(PromoCode promoCode)
        {
            try
            {
                var promoCodeModel = context.PromoCode.Where(x => x.Code == promoCode.Code && x.IsDelete == false).FirstOrDefault();
                if (promoCodeModel == null)
                {
                    context.PromoCode.Add(promoCode);
                    context.SaveChanges();
                    return promoCode.PromoCodeId;
                }
                else
                {
                    return ReturnCode.AlreadyExist.GetHashCode();
                }
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "   ::::   " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Deletes the promocode.
        /// </summary>
        /// <param name="PromocodeId">The promocode identifier.</param>
        /// <returns></returns>
        public bool DeletePromocode(long promocodeId, long updatedBy)
        {
            try
            {
                var promoCodeModel = context.PromoCode.Where(x => x.PromoCodeId == promocodeId).FirstOrDefault();
                if (promoCodeModel != null)
                {
                    promoCodeModel.IsDelete = true;
                    promoCodeModel.UpdatedBy = updatedBy;
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
                LogHelper.ExceptionLog(ex.Message + "   ::::   " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Gets the promocode detail.
        /// </summary>
        /// <param name="PromocodeId">The promocode identifier.</param>
        /// <returns></returns>
        public PromoCode GetPromocodeDetail(long promocodeId)
        {
            try
            {
                return context.PromoCode.Where(x => x.PromoCodeId == promocodeId && x.IsDelete == false).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "   ::::   " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Gets the promocode list.
        /// </summary>
        /// <returns></returns>
        public List<PromoCode> GetPromocodeList(string searchString)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    DateTime searchDate = new DateTime();
                    try
                    {
                        string[] dateSplitted = searchString.Split('-');
                        if (dateSplitted != null && dateSplitted.Length == 3)
                        {
                            searchDate = new DateTime(DBHelper.ParseInt32(dateSplitted[2]), DBHelper.ParseInt32(dateSplitted[0]), DBHelper.ParseInt32(dateSplitted[1]));
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    if (searchDate != DateTime.MinValue)
                    {
                        return context.PromoCode.Where(x => x.IsDelete == false
                                           && (x.StartDate.Equals(searchDate)
                                               || x.ExpiryDate.Equals(searchDate)
                                               || x.PromoCodeId.ToString().Contains(searchString)
                                               || x.Name.Contains(searchString)
                                               || x.Code.Contains(searchString))
                                           ).ToList();
                    }
                    else
                    {
                        return context.PromoCode.Where(x => x.IsDelete == false
                   && (x.PromoCodeId.ToString().Contains(searchString)
                       || x.Name.Contains(searchString)
                       || x.Code.Contains(searchString))
                   ).ToList();
                    }
                }
                else
                {
                    return context.PromoCode.Where(x => x.IsDelete == false).ToList();
                }
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "   ::::   " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Updates the promocode.
        /// </summary>
        /// <param name="promoCode">The model.</param>
        /// <returns></returns>
        public long UpdatePromocode(PromoCode promoCode)
        {
            try
            {
                var promoCodeModelCheck = context.PromoCode.Where(x => x.Code == promoCode.Code && x.IsDelete == false && x.PromoCodeId != promoCode.PromoCodeId).FirstOrDefault();
                if (promoCodeModelCheck == null)
                {
                    var promoCodeModel = context.PromoCode.Where(x => x.IsDelete == false && x.PromoCodeId == promoCode.PromoCodeId).FirstOrDefault();
                    if (promoCodeModel != null)
                    {
                        promoCodeModel.Name = promoCode.Name;
                        promoCodeModel.Code = promoCode.Code;
                        promoCodeModel.StartDate = promoCode.StartDate;
                        promoCodeModel.ExpiryDate = promoCode.ExpiryDate;
                        promoCodeModel.Amount = promoCode.Amount;
                        promoCodeModel.Percentage = promoCode.Percentage;
                        promoCodeModel.Description = promoCode.Description;
                        promoCodeModel.ModifiedOn = promoCode.ModifiedOn;
                        promoCodeModel.UpdatedBy = promoCode.UpdatedBy;
                        context.SaveChanges();
                        return promoCode.PromoCodeId;
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
                LogHelper.ExceptionLog(ex.Message + "   ::::   " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Gets all promo codes.
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllPromoCodes()
        {
            try
            {
                return context.PromoCode.Where(x => x.IsDelete == false).Select(x => x.Code).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "   ::::   " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Gets the total promocode count.
        /// </summary>
        /// <returns></returns>
        public long GetTotalPromocodeCount()
        {
            try
            {
                return context.PromoCode.Where(x => x.IsDelete == false).Count();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "   ::::   " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Changes the promocode status.
        /// </summary>
        /// <param name="promocodeId">The promocode identifier.</param>
        /// <param name="Status">The status.</param>
        /// <returns></returns>
        public bool ChangePromocodeStatus(long promocodeId, int Status)
        {
            try
            {
                var promocodeModel = context.PromoCode.FirstOrDefault(x => x.IsDelete == false && x.PromoCodeId == promocodeId);
                if (promocodeModel != null)
                {
                    if (Status == PromocodeStatus.Active.GetHashCode())
                    {
                        promocodeModel.Status = PromocodeStatus.Active.GetHashCode();
                    }
                    else if (Status == PromocodeStatus.Unactive.GetHashCode())
                    {
                        promocodeModel.Status = PromocodeStatus.Unactive.GetHashCode();
                    }
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
                LogHelper.ExceptionLog(ex.Message + "   ::::   " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Gets the promo code by code.
        /// </summary>
        /// <param name="promoCode">The promo code.</param>
        /// <returns></returns>
        public PromoCode GetPromoCodeByCode(string promoCode)
        {
            try
            {
                var promocodeModel = context.PromoCode.FirstOrDefault(x => x.IsDelete == false && x.Status == PromocodeStatus.Active.GetHashCode() && x.Code == promoCode);
                if (promocodeModel != null)
                {
                    return promocodeModel;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "   ::::   " + ex.StackTrace);
                throw ex;
            }
        }
        #endregion
    }
}
