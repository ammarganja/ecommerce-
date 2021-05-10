namespace SHKang.Model.ViewModels.Web
{
    /// <summary>
    /// Order Invoice Payment
    /// </summary>
    public class OrderInvoicePayment
    {
        #region Public Variables
        /// <summary>
        /// Gets or sets the stripe email.
        /// </summary>
        /// <value>
        /// The stripe email.
        /// </value>
        public string StripeEmail { get; set; }
        /// <summary>
        /// Gets or sets the stripe token.
        /// </summary>
        /// <value>
        /// The stripe token.
        /// </value>
        public string StripeToken { get; set; }
        /// <summary>
        /// Gets or sets the logged in user identifier.
        /// </summary>
        /// <value>
        /// The logged in user identifier.
        /// </value>
        public int LoggedInUserId { get; set; }
        /// <summary>
        /// Gets or sets the order invoice identifier.
        /// </summary>
        /// <value>
        /// The order invoice identifier.
        /// </value>
        public long OrderInvoiceId { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>
        /// The amount.
        /// </value>
        public decimal Amount { get; set; }
        #endregion
    }
}
