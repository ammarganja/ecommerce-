namespace SHKang.Model.ViewModels.Web
{
  public  class CheckPromocodeReturnModel
    {
        /// <summary>
        /// Gets or sets the promocode model.
        /// </summary>
        /// <value>
        /// The promocode model.
        /// </value>
        public CheckPromocodeDetailModel PromocodeModel { get; set; }

        /// <summary>
        /// Gets or sets the amount model.
        /// </summary>
        /// <value>
        /// The amount model.
        /// </value>
        public CheckPromocodeTotalModel AmountModel { get; set; }
    }

    public class CheckPromocodeDetailModel
    {
        /// <summary>
        /// Gets or sets the promocode identifier.
        /// </summary>
        /// <value>
        /// The promocode identifier.
        /// </value>
        public string PromocodeId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the promocode.
        /// </summary>
        /// <value>
        /// The promocode.
        /// </value>
        public string Promocode { get; set; }
    }

    public class CheckPromocodeTotalModel
    {
        /// <summary>
        /// Gets or sets the payable amount.
        /// </summary>
        /// <value>
        /// The payable amount.
        /// </value>
        public string PayableAmount { get; set; }

        /// <summary>
        /// Gets or sets the discount amount.
        /// </summary>
        /// <value>
        /// The discount amount.
        /// </value>
        public string DiscountAmount { get; set; }

        /// <summary>
        /// Gets or sets the total amount.
        /// </summary>
        /// <value>
        /// The total amount.
        /// </value>
        public string TotalAmount { get; set; }
    }
}
