namespace SHKang.Repository.Interface
{
    #region namespaces
    using SHKang.Model.Models;
    using System.Collections.Generic;
    #endregion

    public interface IPromoCode
    {
        /// <summary>
        /// Adds the promocode.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        long AddPromocode(PromoCode promoCode);

        /// <summary>
        /// Updates the promocode.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        long UpdatePromocode(PromoCode promoCode);

        /// <summary>
        /// Deletes the promocode.
        /// </summary>
        /// <param name="PromocodeId">The promocode identifier.</param>
        /// <returns></returns>
        bool DeletePromocode(long promocodeId, long updatedBy);

        /// <summary>
        /// Gets the promocode detail.
        /// </summary>
        /// <param name="PromocodeId">The promocode identifier.</param>
        /// <returns></returns>
        PromoCode GetPromocodeDetail(long promocodeId);

        /// <summary>
        /// Gets the promocode list.
        /// </summary>
        /// <returns></returns>
        List<PromoCode> GetPromocodeList(string searchString);

        /// <summary>
        /// Gets all promo codes.
        /// </summary>
        /// <returns></returns>
        List<string> GetAllPromoCodes();

        /// <summary>
        /// Gets the total promocode count.
        /// </summary>
        /// <returns></returns>
        long GetTotalPromocodeCount();

        /// <summary>
        /// Changes the promocode status.
        /// </summary>
        /// <param name="promocodeId">The promocode identifier.</param>
        /// <param name="Status">The status.</param>
        /// <returns></returns>
        bool ChangePromocodeStatus(long promocodeId, int Status);

        /// <summary>
        /// Gets the promo code by code.
        /// </summary>
        /// <param name="promoCode">The promo code.</param>
        /// <returns></returns>
        PromoCode GetPromoCodeByCode(string promoCode);
    }

}
