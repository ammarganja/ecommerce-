namespace SHKang.Model.ViewModels.Admin
{

    #region namespaces
    using System.ComponentModel.DataAnnotations; 
    #endregion

    public  class GetEditOrderModel:SearchPaginationListModel
    {
        [Required]
        public long OrderId { get; set; }
    }
}
