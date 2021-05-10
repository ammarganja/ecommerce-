namespace SHKang.Model.ViewModels.Admin
{
    using SHKang.Model.Models;

    #region namespaces
    using System.Collections.Generic;
    #endregion

    public class GetOrderInvoiceDetail
    {

        /// <summary>
        /// Gets or sets the order identifier.
        /// </summary>
        /// <value>
        /// The order identifier.
        /// </value>
        public string OrderId { get; set; }

        /// <summary>
        /// Gets or sets the order date.
        /// </summary>
        /// <value>
        /// The order date.
        /// </value>
        public string invoiceDate { get; set; }

        /// <summary>
        /// Gets or sets the order unique no.
        /// </summary>
        /// <value>
        /// The order unique no.
        /// </value>
        public string poNumber { get; set; }

        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        /// <value>
        /// The name of the company.
        /// </value>
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the email identifier.
        /// </summary>
        /// <value>
        /// The email identifier.
        /// </value>
        public string EmailId { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        /// <value>
        /// The phone number.
        /// </value>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        /// <value>
        /// The full name.
        /// </value>
        public string firstName { get; set; }

        /// <summary>
        /// Gets or sets the address1.
        /// </summary>
        /// <value>
        /// The address1.
        /// </value>
        public string Address1 { get; set; }

        /// <summary>
        /// Gets or sets the address2.
        /// </summary>
        /// <value>
        /// The address2.
        /// </value>
        public string Address2 { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>
        /// The city.
        /// </value>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <value>
        /// The country.
        /// </value>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the zipcode.
        /// </summary>
        /// <value>
        /// The zipcode.
        /// </value>
        public string Zipcode { get; set; }

        /// <summary>
        /// Gets or sets the product details.
        /// </summary>
        /// <value>
        /// The product details.
        /// </value>
        public List<GetOrderInvoiceProductDetail> productDetails { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the user address identifier.
        /// </summary>
        /// <value>
        /// The user address identifier.
        /// </value>
        public string UserAddressId { get; set; }

        /// <summary>
        /// Gets or sets the admin user details.
        /// </summary>
        /// <value>
        /// The admin user details.
        /// </value>
        public AdminUserDetails AdminUserDetails { get; set; }

    }

    public class GetOrderInvoiceProductDetail
    {
        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        /// <value>
        /// The name of the product.
        /// </value>
        public string ProductName { get; set; }

        /// <summary>
        /// Gets or sets the product image.
        /// </summary>
        /// <value>
        /// The product image.
        /// </value>
        public string ProductImage { get; set; }

        /// <summary>
        /// Gets or sets the required ratio.
        /// </summary>
        /// <value>
        /// The required ratio.
        /// </value>
        public string RequiredRatio { get; set; }

        /// <summary>
        /// Gets or sets the shipped ratio.
        /// </summary>
        /// <value>
        /// The shipped ratio.
        /// </value>
        public string ShippedRatio { get; set; }

        /// <summary>
        /// Gets or sets the remaining ratio.
        /// </summary>
        /// <value>
        /// The remaining ratio.
        /// </value>
        public string RemainingRatio { get; set; }

        /// <summary>
        /// Gets or sets the product size ratio.
        /// </summary>
        /// <value>
        /// The product size ratio.
        /// </value>
        public string ProductSizeRatio { get; set; }

        /// <summary>
        /// Gets or sets the qty.
        /// </summary>
        /// <value>
        /// The qty.
        /// </value>
        public string Qty { get; set; }

        /// <summary>
        /// Gets or sets the remaining qty.
        /// </summary>
        /// <value>
        /// The remaining qty.
        /// </value>
        public string RemainingQty { get; set; }

        /// <summary>
        /// Gets or sets the shipped qty.
        /// </summary>
        /// <value>
        /// The shipped qty.
        /// </value>
        public string ShippedQty { get; set; }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>
        /// The price.
        /// </value>
        public string Price { get; set; }

        /// <summary>
        /// Gets or sets the sub total.
        /// </summary>
        /// <value>
        /// The sub total.
        /// </value>
        public string SubTotal { get; set; }

        /// <summary>
        /// Gets or sets the product color identifier.
        /// </summary>
        /// <value>
        /// The product color identifier.
        /// </value>
        public string ProductColorId { get; set; }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public string Color { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is selected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is selected; otherwise, <c>false</c>.
        /// </value>
        public bool isSelected { get; set; }

        /// <summary>
        /// Gets or sets the total size of the required.
        /// </summary>
        /// <value>
        /// The total size of the required.
        /// </value>
        public string RequiredTotalSize { get; set; }

        /// <summary>
        /// Gets or sets the size group list.
        /// </summary>
        /// <value>
        /// The size group list.
        /// </value>
        public GetOrderInvoiceMainSizeGroupList sizeGroupList { get; set; }
        /// <summary>
        /// Gets or sets the remaining size lists.
        /// </summary>
        /// <value>
        /// The remaining size lists.
        /// </value>
        public List<GetOrderInvoiceRemainingSizeList> remainingSizeLists { get; set; }

        /// <summary>
        /// Gets or sets the total qty.
        /// </summary>
        /// <value>
        /// The total qty.
        /// </value>
        public string TotalQty { get; set; }


    }

    public class GetOrderInvoiceMainSizeGroupList
    {
        /// <summary>
        /// Gets or sets the qty sub total.
        /// </summary>
        /// <value>
        /// The qty sub total.
        /// </value>
        public string QtySubTotal { get; set; }

        /// <summary>
        /// Gets or sets the size group list.
        /// </summary>
        /// <value>
        /// The size group list.
        /// </value>
        public List<GetOrderInvoiceSizeGroupList> sizeGroupList { get; set; }
    }

    public class GetOrderInvoiceSizeGroupList
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }
    }

    public class GetOrderInvoiceRemainingSizeList
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string value { get; set; }
    }

}
