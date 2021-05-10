namespace SHKang.Model.Models
{

    #region namespaces
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema; 
    #endregion

    public class OrderInvoiceSizeDetail
    {

        /// <summary>
        /// Gets or sets the order invoice size detail identifier.
        /// </summary>
        /// <value>
        /// The order invoice size detail identifier.
        /// </value>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long OrderInvoiceSizeDetailId { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public string Size { get; set; }


        #region Foreign Key with OrderInvoiceDetail

        /// <summary>
        /// Gets or sets the order invoice detail fk.
        /// </summary>
        /// <value>
        /// The order invoice detail fk.
        /// </value>
        [ForeignKey("OrderInvoiceDetail")]
        public long OrderInvoiceDetailFK { get; set; }


        /// <summary>
        /// Gets or sets the order invoice detail.
        /// </summary>
        /// <value>
        /// The order invoice detail.
        /// </value>
        public virtual OrderInvoiceDetail OrderInvoiceDetail { get; set; }
        #endregion

    }
}
