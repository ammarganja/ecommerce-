namespace SHKang.Repository.Repository
{
    #region namespaces
    using SHKang.Core.Enums;
    using SHKang.Core.Helpers;
    using SHKang.Model.Context;
    using SHKang.Model.Models;
    using SHKang.Repository.Interface;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion
    public class ProductCategoryRepository : IProductCategory
    {

        #region Private Variables

        /// <summary>
        /// The context
        /// </summary>
        private AppDbContext context;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCategoryRepository"/> class.
        /// </summary>
        /// <param name="_context">The context.</param>
        public ProductCategoryRepository(AppDbContext _context)
        {
            context = _context;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the product category.
        /// </summary>
        /// <param name="productCategory">The model.</param>
        /// <returns></returns>
        public long AddProductCategory(ProductCategory productCategory)
        {
            try
            {
                var productCategoryModel = context.ProductCategory.Where(x => x.Name == productCategory.Name && x.IsDelete == false).FirstOrDefault();
                if (productCategoryModel == null)
                {
                    context.ProductCategory.Add(productCategory);
                    context.SaveChanges();
                    return productCategory.ProductCategoryId;
                }
                else
                {
                    return ReturnCode.AlreadyExist.GetHashCode();
                }
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }


        /// <summary>
        /// Deletes the product category.
        /// </summary>
        /// <param name="ProductCategoryId">The product category identifier.</param>
        /// <returns></returns>
        public bool DeleteProductCategory(long productCategoryId, long updatedBy)
        {
            try
            {
                var productCategoryModel = context.ProductCategory.Where(x => x.ProductCategoryId == productCategoryId).FirstOrDefault();
                if (productCategoryModel != null)
                {
                    productCategoryModel.ModifiedOn = DateTime.Now;
                    productCategoryModel.UpdatedBy = updatedBy;
                    productCategoryModel.IsDelete = true;
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }


        /// <summary>
        /// Gets the product category.
        /// </summary>
        /// <returns></returns>
        public List<ProductCategory> GetProductCategory(string searchString)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    return context.ProductCategory.Where(x => x.IsDelete == false && x.Name.Contains(searchString)).ToList();
                }
                else
                {
                    return context.ProductCategory.Where(x => x.IsDelete == false).ToList();
                }
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }


        /// <summary>
        /// Gets the product category detail.
        /// </summary>
        /// <param name="ProductCategoryId">The product category identifier.</param>
        /// <returns></returns>
        public ProductCategory GetProductCategoryDetail(long productCategoryId)
        {
            try
            {
                var productCategoryModel = context.ProductCategory.Where(x => x.ProductCategoryId == productCategoryId && x.IsDelete == false).FirstOrDefault();
                if (productCategoryModel != null)
                {

                    return productCategoryModel;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }


        /// <summary>
        /// Updates the product category.
        /// </summary>
        /// <param name="productCategory">The model.</param>
        /// <returns></returns>
        public long UpdateProductCategory(ProductCategory productCategory, long updatedBy)
        {
            try
            {
                var productCategoryModelCheck = context.ProductCategory.Where(x => x.Name == productCategory.Name && x.IsDelete == false && x.ProductCategoryId != productCategory.ProductCategoryId).FirstOrDefault();
                if (productCategoryModelCheck == null)
                {
                    var productCategoryModel = context.ProductCategory.Where(x => x.ProductCategoryId == productCategory.ProductCategoryId && x.IsDelete == false).FirstOrDefault();
                    if (productCategoryModel != null)
                    {
                        productCategoryModel.ModifiedOn = DateTime.Now;
                        productCategoryModel.UpdatedBy = updatedBy;
                        productCategoryModel.Name = productCategory.Name;
                        productCategoryModel.ModifiedOn = DateTime.Now;
                        productCategoryModel.UpdatedBy = productCategory.UpdatedBy;
                        context.SaveChanges();
                        return productCategory.ProductCategoryId;
                    }
                    else
                    {
                        return ReturnCode.NotExist.GetHashCode();
                    }
                }
                else
                {
                    return ReturnCode.AlreadyExist.GetHashCode();
                }

            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }
        #endregion

    }
}
