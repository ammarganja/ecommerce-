namespace SHKang.Model.Context
{
    #region namespaces
    using Microsoft.EntityFrameworkCore;
    using SHKang.Model.Models;
    #endregion

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        /// <summary>
        /// Card Type Table
        /// Created by Yash Gajjar
        /// Dated : 05-03-2021
        /// </summary>
        public DbSet<CardType> CardType { get; set; }

        /// <summary>
        /// Content Details Table
        /// Created by Yash Gajjar
        /// Dated : 05-03-2021
        /// </summary>
        public DbSet<ContentDetails> ContentDetails { get; set; }

        /// <summary>
        /// Country Table
        /// Created by Yash Gajjar
        /// Dated : 05-03-2021
        /// </summary>
        public DbSet<Country> Country { get; set; }

        /// <summary>
        /// Order Table
        /// Created by Yash Gajjar
        /// Dated : 05-03-2021
        /// </summary>
        public DbSet<Order> Order { get; set; }

        /// <summary>
        /// Order Detail Table
        /// Created by Yash Gajjar
        /// Dated : 05-03-2021
        /// </summary>
        public DbSet<OrderDetail> OrderDetail { get; set; }

        /// <summary>
        /// Order Status Table
        /// Created by Yash Gajjar
        /// Dated : 05-03-2021
        /// </summary>
        public DbSet<OrderStatus> OrderStatus { get; set; }

        /// <summary>
        /// Product Table
        /// Created by Yash Gajjar
        /// Dated : 05-03-2021
        /// </summary>
        public DbSet<Product> Product { get; set; }

        /// <summary>
        /// Product Category Table
        /// Created by Yash Gajjar
        /// Dated : 05-03-2021
        /// </summary>
        public DbSet<ProductCategory> ProductCategory { get; set; }

        /// <summary>
        /// Product Color Table
        /// Created by Yash Gajjar
        /// Dated : 05-03-2021
        /// </summary>
        public DbSet<ProductColor> ProductColor { get; set; }

        /// <summary>
        /// Product Image Table
        /// Created by Yash Gajjar
        /// Dated : 05-03-2021
        /// </summary>
        public DbSet<ProductImage> ProductImage { get; set; }

        /// <summary>
        /// Product Quantity Table
        /// Created by Yash Gajjar
        /// Dated : 05-03-2021
        /// </summary>
        public DbSet<ProductQuantity> ProductQuantity { get; set; }

        /// <summary>
        /// Product Size Table
        /// Created by Yash Gajjar
        /// Dated : 05-03-2021
        /// </summary>
        public DbSet<Size> ProductSize { get; set; }

        /// <summary>
        /// PromoCode Table
        /// Created by Yash Gajjar
        /// Dated : 05-03-2021
        /// </summary>
        public DbSet<PromoCode> PromoCode { get; set; }

        /// <summary>
        /// Settings Table
        /// Created by Yash Gajjar
        /// Dated : 05-03-2021
        /// </summary>
        public DbSet<Settings> Settings { get; set; }

        /// <summary>
        /// State Table
        /// Created by Yash Gajjar
        /// Dated : 05-03-2021
        /// </summary>
        public DbSet<State> State { get; set; }

        /// <summary>
        /// User Table
        /// Created by Yash Gajjar
        /// Dated : 05-03-2021
        /// </summary>
        public DbSet<User> User { get; set; }

        /// <summary>
        /// User Cart Details Table
        /// Created by Yash Gajjar
        /// Dated : 05-03-2021
        /// </summary>
        public DbSet<UserCartDetails> UserCartDetails { get; set; }


        /// <summary>
        /// Product Category Type Table
        /// Created by Yash Gajjar
        /// Dated : 09-03-2021
        /// </summary>
        public DbSet<ProductCategoryType> ProductCategoryType { get; set; }

        /// <summary>
        /// Order Invoice Table
        /// Created by Yash Gajjar
        /// Dated : 09-03-2021
        /// </summary>
        public DbSet<OrderInvoice> OrderInvoice { get; set; }

        /// <summary>
        /// Order Invoice Detail Table
        /// Created by Yash Gajjar
        /// Dated : 09-03-2021
        /// </summary>
        public DbSet<OrderInvoiceDetail> OrderInvoiceDetail { get; set; }

        /// <summary>
        /// Product Category Detail Table
        /// Created by Yash Gajjar
        /// Dated : 09-03-2021
        /// </summary>
        public DbSet<ProductCategoryDetail> ProductCategoryDetail { get; set; }

        /// <summary>
        ///Size Table
        /// Created by Yash Gajjar
        /// Dated : 09-03-2021
        /// </summary>
        public DbSet<Size> Size { get; set; }


        /// <summary>
        ///Color Table
        /// Created by Yash Gajjar
        /// Dated : 09-03-2021
        /// </summary>
        public DbSet<Color> Color { get; set; }

        /// <summary>
        ///Size Ratio Table
        /// Created by Yash Gajjar
        /// Dated : 09-03-2021
        /// </summary>
        public DbSet<SizeRatio> SizeRatio { get; set; }

        /// <summary>
        ///User Address Table
        /// Created by Yash Gajjar
        /// Dated : 10-03-2021
        /// </summary>
        public DbSet<UserAddress> UserAddress { get; set; }

        /// <summary>
        ///Size Detail Table
        /// Created by Yash Gajjar
        /// Dated : 08-04-2021
        /// </summary>
        public DbSet<SizeDetail> SizeDetail { get; set; }

        /// <summary>
        /// Product Size Detail Table
        /// Created by Yash Gajjar
        /// Dated : 08-04-2021
        /// </summary>
        public DbSet<ProductSizeDetail> ProductSizeDetail { get; set; }

        /// <summary>
        /// Order Invoice Size Detail Table
        /// Created by Yash Gajjar
        /// Dated : 12-04-2021
        /// </summary>
        public DbSet<OrderInvoiceSizeDetail> OrderInvoiceSizeDetail { get; set; }

        /// <summary>
        /// Testimonials Table
        /// Created by Yash Gajjar
        /// Dated : 19-04-2021
        /// </summary>
        public DbSet<Testimonials> Testimonials { get; set; }
    }
}
