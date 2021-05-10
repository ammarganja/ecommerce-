namespace SHKang.Model.Models
{

    #region namespaces
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema; 
    #endregion

    public class SizeDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SizeDetailId { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public string Name { get; set; }

        #region Foreign Key with Size

        /// <summary>
        /// Gets or sets the color fk.
        /// </summary>
        /// <value>
        /// The color fk.
        /// </value>
        [ForeignKey("Size")]
        public long SizeFK { get; set; }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public virtual Size Size { get; set; }
        #endregion
    }
}
