namespace SHKang.Core.Enums
{
    /// <summary>
    /// OrderStatusEnum
    /// </summary>
    public enum OrderStatusEnum
    {
        /// <summary>
        /// The pending
        /// </summary>
        Paid = 1,

        /// <summary>
        /// The readyfor payment
        /// </summary>
        ReadyforPayment = 2,

        /// <summary>
        /// The shipped
        /// </summary>
        Shipped = 3,

        /// <summary>
        /// The delivered
        /// </summary>
        Delivered = 4,

        /// <summary>
        /// The cancelled
        /// </summary>
        Cancelled = 5

    }

    /// <summary>
    /// ResponseStatus
    /// </summary>
    public enum ResponseStatus
    {
        #region The enumerator property

        /// <summary>
        /// The success
        /// </summary>
        Success = 1,

        /// <summary>
        /// The fail
        /// </summary>
        Fail = 0,

        /// <summary>
        /// The invalid
        /// </summary>
        Invalid = 2,

        /// <summary>
        /// The valid
        /// </summary>
        Valid = 3,

        /// <summary>
        /// The not confirm
        /// </summary>
        NotConfirm = 4,

        /// <summary>
        /// The not found
        /// </summary>
        NotFound = 5,

        #endregion
    }


    /// <summary>
    /// RoleType
    /// </summary>
    public enum RoleType
    {
        /// <summary>
        /// The admin
        /// </summary>
        Admin = 1,

        /// <summary>
        /// The user
        /// </summary>
        User = 2
    }


    /// <summary>
    /// ProductCategoryEnum
    /// </summary>
    public enum ProductCategoryEnum
    {
        /// <summary>
        /// The styles
        /// </summary>
        Styles = 1,

        /// <summary>
        /// The campaign
        /// </summary>
        Campaign = 2
    }


    /// <summary>
    /// ReturnCode
    /// </summary>
    public enum ReturnCode
    {
        #region Proprties

        /// <summary>
        /// The is item alrady exist
        /// </summary>
        AlreadyExist = -10,

        /// <summary>
        /// The not exist
        /// </summary>
        NotExist = 0,

        /// <summary>
        /// The not matching
        /// </summary>
        NotMatching = -20,

        /// <summary>
        /// The is primary exist
        /// </summary>
        IsPrimaryExist = -30

        #endregion
    }


    /// <summary>
    /// FileType
    /// </summary>
    public enum FileType
    {
        /// <summary>
        /// The product
        /// </summary>
        Product = 1
    }


    /// <summary>
    /// Months
    /// </summary>
    public enum Months
    {
        /// <summary>
        /// The january
        /// </summary>
        January = 1,

        /// <summary>
        /// The february
        /// </summary>
        February = 2,

        /// <summary>
        /// The march
        /// </summary>
        March = 3,

        /// <summary>
        /// The april
        /// </summary>
        April = 4,

        /// <summary>
        /// The may
        /// </summary>
        May = 5,

        /// <summary>
        /// The june
        /// </summary>
        June = 6,

        /// <summary>
        /// The july
        /// </summary>
        July = 7,

        /// <summary>
        /// The august
        /// </summary>
        August = 8,

        /// <summary>
        /// The september
        /// </summary>
        September = 9,

        /// <summary>
        /// The october
        /// </summary>
        October = 10,

        /// <summary>
        /// The november
        /// </summary>
        November = 11,

        /// <summary>
        /// The december
        /// </summary>
        December = 12
    }

    /// <summary>
    /// SortingDirectionType
    /// </summary>
    public enum SortingDirectionType
    {
        /// <summary>
        /// The asc
        /// </summary>
        asc = 1,

        /// <summary>
        /// The desc
        /// </summary>
        desc = 2
    }

    /// <summary>
    /// SortingColorColumnName
    /// </summary>
    public enum SortingColorColumnName
    {
        /// <summary>
        /// The colorid
        /// </summary>
        colorid = 1,

        /// <summary>
        /// The name
        /// </summary>
        name = 2

    }

    /// <summary>
    /// SortingProductCategoryTypeColumnName
    /// </summary>
    public enum SortingProductCategoryTypeColumnName
    {
        /// <summary>
        /// The productcategorytypeid
        /// </summary>
        productcategorytypeid = 1,

        /// <summary>
        /// The name
        /// </summary>
        name = 2
    }

    /// <summary>
    /// PromocodeStatus
    /// </summary>
    public enum PromocodeStatus
    {
        /// <summary>
        /// The active
        /// </summary>
        Active = 1,

        /// <summary>
        /// The unactive
        /// </summary>
        Unactive = 2
    }

    /// <summary>
    /// SortingPromocodeColumnName
    /// </summary>
    public enum SortingPromocodeColumnName
    {
        /// <summary>
        /// The promocodeid
        /// </summary>
        promocodeid = 1,

        /// <summary>
        /// The name
        /// </summary>
        name = 2,

        /// <summary>
        /// The code
        /// </summary>
        code = 3,

        /// <summary>
        /// The expirydate
        /// </summary>
        expirydate = 4,

        /// <summary>
        /// The startdate
        /// </summary>
        startdate = 5,

        /// <summary>
        /// The discount
        /// </summary>
        discount = 6
    }

    /// <summary>
    /// ProductStatus
    /// </summary>
    public enum ProductStatus
    {
        /// <summary>
        /// The active
        /// </summary>
        Active = 1,

        /// <summary>
        /// The unactive
        /// </summary>
        Unactive = 2
    }

    /// <summary>
    /// The SortContentType
    /// </summary>
    public enum SortContentType
    {
        /// <summary>
        /// The contentid
        /// </summary>
        contentid = 1,

        /// <summary>
        /// The name
        /// </summary>
        name = 2
    }

    /// <summary>
    /// The SortSize
    /// </summary>
    public enum SortSize
    {
        /// <summary>
        /// The sizeid
        /// </summary>
        sizeid = 1,

        /// <summary>
        /// The sizename
        /// </summary>
        sizename = 2,

        /// <summary>
        /// The units
        /// </summary>
        units = 3
    }

    /// <summary>
    /// The UserStatus
    /// </summary>
    public enum UserStatus
    {
        /// <summary>
        /// The active
        /// </summary>
        Active = 1,

        /// <summary>
        /// The in active
        /// </summary>
        InActive = 2,

        /// <summary>
        /// The suspended
        /// </summary>
        Suspended = 3
    }

    /// <summary>
    /// SortingCampaignColumnName
    /// </summary>
    public enum SortingCampaignColumnName
    {
        /// <summary>
        /// The campaignid
        /// </summary>
        campaignid = 1,

        /// <summary>
        /// The campaignname
        /// </summary>
        campaignname = 2,

        /// <summary>
        /// The description
        /// </summary>
        description = 3,

        /// <summary>
        /// The total product
        /// </summary>
        totalProduct = 4
    }

    /// <summary>
    /// TestimonialSize
    /// </summary>
    public enum TestimonialSize
    {
        /// <summary>
        /// The testimonialid
        /// </summary>
        testimonialid = 1,

        /// <summary>
        /// The message
        /// </summary>
        message = 2,

        /// <summary>
        /// The useremail
        /// </summary>
        useremail = 3,

        /// <summary>
        /// The usermobilenumber
        /// </summary>
        usermobilenumber = 4,

        /// <summary>
        /// The testimonialdate
        /// </summary>
        testimonialdate = 5,
    }
}
