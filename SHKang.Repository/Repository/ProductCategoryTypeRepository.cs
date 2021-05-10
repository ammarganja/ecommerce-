
namespace SHKang.Repository.Repository
{
    #region namespaces

    using Microsoft.EntityFrameworkCore;
    using SHKang.Core.Enums;
    using SHKang.Core.Helpers;
    using SHKang.Model.Context;
    using SHKang.Model.Models;
    using SHKang.Repository.Interface;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion
    public class ProductCategoryTypeRepository : IProductCategoryType
    {

        #region Private Variables

        /// <summary>
        /// The context
        /// </summary>
        private AppDbContext context;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCategoryTypeRepository"/> class.
        /// </summary>
        /// <param name="_context">The context.</param>
        public ProductCategoryTypeRepository(AppDbContext _context)
        {
            context = _context;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the new type of the product category.
        /// </summary>
        /// <param name="productCategoryType">The model.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public long AddNewProductCategoryType(ProductCategoryType productCategoryType, string productIds = "")
        {
            try
            {
                var productCategoryTypeModel = context.ProductCategoryType.Where(x => x.CategoryType == productCategoryType.CategoryType && x.IsDelete == false).FirstOrDefault();
                if (productCategoryTypeModel == null)
                {
                    context.ProductCategoryType.Add(productCategoryType);
                    context.SaveChanges();
                    if (!string.IsNullOrWhiteSpace(productIds))
                    {
                        string[] splitProductIds = productIds.Split(',');
                        if (splitProductIds != null && splitProductIds.Length > 0)
                        {
                            foreach (var item in splitProductIds)
                            {
                                if (!string.IsNullOrWhiteSpace(item))
                                {
                                    ProductCategoryDetail productCategoryDetail = new ProductCategoryDetail();
                                    productCategoryDetail.ProductCategoryTypeFK = productCategoryType.ProductCategoryTypeId;
                                    productCategoryDetail.ProductFK = DBHelper.ParseInt64(item);
                                    context.ProductCategoryDetail.Add(productCategoryDetail);
                                    context.SaveChanges();
                                }
                            }
                        }
                    }
                    return productCategoryType.ProductCategoryTypeId;
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
        /// Deletes the type of the product category.
        /// </summary>
        /// <param name="ProductCategoryTypeId">The product category type identifier.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool DeleteProductCategoryType(long productCategoryTypeId, long updatedBy)
        {
            try
            {
                var productCategoryTypeModel = context.ProductCategoryType.Where(x => x.ProductCategoryTypeId == productCategoryTypeId && x.IsDelete == false).FirstOrDefault();
                if (productCategoryTypeModel != null)
                {
                    productCategoryTypeModel.ModifiedOn = DateTime.Now;
                    productCategoryTypeModel.UpdatedBy = updatedBy;
                    productCategoryTypeModel.IsDelete = true;
                    context.SaveChanges();
                    var productList = context.ProductCategoryDetail.Where(x => x.ProductCategoryTypeFK == productCategoryTypeId).ToList();
                    if (productList != null)
                    {
                        foreach (var item in productList)
                        {
                            context.ProductCategoryDetail.Remove(item);
                            context.SaveChanges();
                        }
                    }
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
        /// Products the type of the category.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<ProductCategoryType> ProductCategoryType(string searchString)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    return context.ProductCategoryType.Include(x => x.ProductCategory).Where(x => x.IsDelete == false && x.CategoryType.Contains(searchString)).ToList();
                }
                else
                {
                    return context.ProductCategoryType.Include(x => x.ProductCategory).Where(x => x.IsDelete == false).ToList();
                }
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Products the category type detail.
        /// </summary>
        /// <param name="ProductCategoryTypeId">The product category type identifier.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ProductCategoryType ProductCategoryTypeDetail(long productCategoryTypeId)
        {
            try
            {
                var productCategoryTypeModel = context.ProductCategoryType.Include(x => x.ProductCategory).Where(x => x.ProductCategoryTypeId == productCategoryTypeId && x.IsDelete == false).FirstOrDefault();
                if (productCategoryTypeModel != null)
                {
                    return productCategoryTypeModel;
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
        /// Updates the type of the product category.
        /// </summary>
        /// <param name="productCategoryType">The model.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public long UpdateProductCategoryType(ProductCategoryType productCategoryType, long updatedBy, string productIds = "")
        {
            try
            {
                var productCategoryTypeModelCheck = context.ProductCategoryType.Where(x => x.CategoryType == productCategoryType.CategoryType && x.IsDelete == false && x.ProductCategoryTypeId != productCategoryType.ProductCategoryTypeId).FirstOrDefault();
                if (productCategoryTypeModelCheck == null)
                {
                    var productCategoryTypeModel = context.ProductCategoryType.Where(x => x.ProductCategoryTypeId == productCategoryType.ProductCategoryTypeId && x.IsDelete == false).FirstOrDefault();
                    if (productCategoryTypeModel != null)
                    {
                        productCategoryTypeModel.ModifiedOn = DateTime.Now;
                        productCategoryTypeModel.UpdatedBy = updatedBy;
                        productCategoryTypeModel.CategoryType = productCategoryType.CategoryType;
                        productCategoryTypeModel.Image = productCategoryType.Image;
                        productCategoryTypeModel.Description = productCategoryType.Description;
                        productCategoryTypeModel.IsShowInFrontend = productCategoryType.IsShowInFrontend;
                        productCategoryTypeModel.ProductCategoryFK = productCategoryType.ProductCategoryFK;
                        context.SaveChanges();

                        if (!string.IsNullOrWhiteSpace(productIds))
                        {
                            var productCategory = context.ProductCategoryDetail.Where(x => x.ProductCategoryTypeFK == productCategoryTypeModel.ProductCategoryTypeId).ToList();
                            if (productCategory != null)
                            {
                                foreach (var item in productCategory)
                                {
                                    context.ProductCategoryDetail.Remove(item);
                                    context.SaveChanges();
                                }
                            }
                            string[] splitProductIds = productIds.Split(',');
                            if (splitProductIds != null && splitProductIds.Length > 0)
                            {
                                foreach (var item in splitProductIds)
                                {
                                    if (!string.IsNullOrWhiteSpace(item))
                                    {
                                        ProductCategoryDetail productCategoryDetail = new ProductCategoryDetail();
                                        productCategoryDetail.ProductCategoryTypeFK = productCategoryType.ProductCategoryTypeId;
                                        productCategoryDetail.ProductFK = DBHelper.ParseInt64(item);
                                        context.ProductCategoryDetail.Add(productCategoryDetail);
                                        context.SaveChanges();
                                    }
                                }
                            }
                        }

                        return productCategoryTypeModel.ProductCategoryTypeId;
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

        /// <summary>
        /// Gets the product category details.
        /// </summary>
        /// <param name="productCategoryTypeId">The product category type identifier.</param>
        /// <returns></returns>
        public List<ProductCategoryDetail> GetProductCategoryDetails(List<long> productCategoryTypeId)
        {
            try
            {
                return context.ProductCategoryDetail.Where(x => productCategoryTypeId.Contains(x.ProductCategoryTypeFK)).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Gets the total product category type count.
        /// </summary>
        /// <returns></returns>
        public long GetTotalProductCategoryTypeCount()
        {
            try
            {
                return context.ProductCategoryType.Where(x => x.IsDelete == false).Count();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Gets the type of all product category.
        /// </summary>
        /// <returns></returns>
        public List<ProductCategoryType> GetAllProductCategoryType()
        {
            try
            {
                return context.ProductCategoryType.Where(x => x.IsDelete == false && x.ProductCategoryFK == ProductCategoryEnum.Styles.GetHashCode()).ToList();
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
