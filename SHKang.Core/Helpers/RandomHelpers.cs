namespace SHKang.Core.Helpers
{
    #region namespaces
    using SHKang.Core.Constant;
    using System;
    using System.Linq;
    #endregion

    public class RandomHelpers
    {
        #region Private Variables
        /// <summary>
        /// The random
        /// </summary>
        private static Random random = new Random();
        #endregion

        #region Public Methods
        
        /// <summary>
        /// Returns Random string alphanumeric
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static string RandomString()
        {
            return new string(Enumerable.Repeat(Constants.chars, Constants.Length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Gets the unique invoice identifier.
        /// </summary>
        /// <returns></returns>
        public static string GetUniqueOrderInvoiceId()
        {
            return new string(Enumerable.Repeat(Constants.numeric, Constants.OrderInvoiceLength)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Gets the unique order identifier.
        /// </summary>
        /// <returns></returns>
        public static string GetUniqueOrderId()
        {
            return new string(Enumerable.Repeat(Constants.numeric, Constants.OrderIdLength)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Gets the unique po number.
        /// </summary>
        /// <returns></returns>
        public static string GetUniquePONumber()
        {
            return new string(Enumerable.Repeat(Constants.numeric, Constants.PoNumberLength)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Gets the unique promo code.
        /// </summary>
        /// <returns></returns>
        public static string GetUniquePromoCode()
        {
            return new string(Enumerable.Repeat(Constants.numeric, Constants.PromoCodeLength)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        #endregion
    }
}
