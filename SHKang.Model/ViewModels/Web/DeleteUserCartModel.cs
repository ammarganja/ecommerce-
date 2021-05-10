namespace SHKang.Model.ViewModels.Web
{

    #region namespaces
    using System.ComponentModel.DataAnnotations; 
    #endregion


    public class DeleteUserCartModel
    {
        [Required]
        public long cartId { get; set; }

        [Required]
        public long userId { get; set; }
    }
}
