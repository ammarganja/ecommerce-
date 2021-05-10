
namespace SHKang.Repository.Repository
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    #region namespaces
    using Microsoft.Extensions.Options;
    using SHKang.Core.Constant;
    using SHKang.Core.Enums;
    using SHKang.Core.Helpers;
    using SHKang.Core.ModelHelper;
    using SHKang.Model.Context;
    using SHKang.Model.Models;
    using SHKang.Model.ViewModels.Admin;
    using SHKang.Repository.Interface;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;

    #endregion

    public class ProductRepository : IProduct
    {
        #region Private Variables

        /// <summary>
        /// The context
        /// </summary>
        private AppDbContext context;

        /// <summary>
        /// The settings
        /// </summary>
        private IOptions<ConnectionString> settings;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductRepository"/> class.
        /// </summary>
        /// <param name="_context">The context.</param>
        public ProductRepository(AppDbContext _context, IOptions<ConnectionString> _settings)
        {
            context = _context;
            settings = _settings;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the product.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="productImages">The product images.</param>
        /// <param name="addProductColorModel">The product colors.</param>
        /// <param name="productCategoryDetails">The product category details.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public long AddProduct(Product model, List<ProductCategoryDetail> productCategoryDetails, string hostPath, string[] ProductSizeValues)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var productModel = context.Product.Where(x => x.Name == model.Name && x.IsDelete == false).FirstOrDefault();
                    if (productModel == null)
                    {
                        context.Product.Add(model);
                        context.SaveChanges();
                        long ProductId = model.ProductId;
                        if (ProductId > 0)
                        {
                            #region ProductSizeValues 
                            if (ProductSizeValues != null && ProductSizeValues.Length > 0)
                            {
                                foreach (var item in ProductSizeValues)
                                {
                                    if (!string.IsNullOrWhiteSpace(item))
                                    {
                                        ProductSizeDetail productSizeDetail = new ProductSizeDetail();
                                        productSizeDetail.ProductFK = ProductId;
                                        productSizeDetail.Name = item;
                                        context.ProductSizeDetail.Add(productSizeDetail);
                                        context.SaveChanges();
                                        if (productSizeDetail.ProductSizeDetailId <= 0)
                                        {
                                            transaction.Rollback();
                                            return 0;
                                        }
                                    }

                                }
                            }
                            #endregion

                            #region Product Category
                            if (productCategoryDetails != null && productCategoryDetails.Count > 0)
                            {
                                foreach (var item in productCategoryDetails)
                                {
                                    if (item.ProductCategoryTypeFK > 0)
                                    {
                                        item.ProductFK = ProductId;
                                        context.ProductCategoryDetail.Add(item);
                                        context.SaveChanges();
                                        if (item.ProductCategoryDetailId <= 0)
                                        {
                                            transaction.Rollback();
                                            return 0;
                                        }
                                    }

                                }
                            }
                            #endregion

                            transaction.Commit();
                            return ProductId;
                        }
                        else
                        {
                            transaction.Rollback();
                            return 0;
                        }
                    }
                    else
                    {
                        transaction.Rollback();
                        return ReturnCode.AlreadyExist.GetHashCode();
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                    throw ex;
                }
            }
        }


        /// <summary>
        /// Gets the product list.
        /// </summary>
        /// <param name="pageNo">The page no.</param>
        /// <param name="rowsPerPage">The rows per page.</param>
        /// <param name="searchString">The search string.</param>
        /// <param name="hostingPath">The hosting path.</param>
        /// <param name="column">The column.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="colorId">The color identifier.</param>
        /// <param name="categoryTypeId">The category type identifier.</param>
        /// <param name="sizeId">The size identifier.</param>
        /// <returns></returns>
        public ProductListModel GetProductList(int pageNo, int rowsPerPage, string searchString, string hostingPath, string column, string direction, string colorId, string categoryTypeId, string sizeId)
        {
            ProductListModel productListModel = new ProductListModel();
            List<ProductListDataModel> productListDataModel = new List<ProductListDataModel>();
            try
            {
                SqlParameter[] parameter = new SqlParameter[8];
                parameter[0] = new SqlParameter("PAGENO", pageNo);
                parameter[1] = new SqlParameter("RECORDPERPAGE", rowsPerPage);
                parameter[2] = new SqlParameter("SEARCHSTRING", searchString);
                parameter[3] = new SqlParameter("COLUMN", column);
                parameter[4] = new SqlParameter("DIRECTION", direction);
                parameter[5] = new SqlParameter("COLORID", colorId);
                parameter[6] = new SqlParameter("PRODUCTCATEGORYTYPEID", categoryTypeId);
                parameter[7] = new SqlParameter("SIZEID", sizeId);
                DataSet dataSet = DBHelper.GetDataTable("PRODUCTLIST", parameter, DBHelper.ParseString(settings.Value.AppDbContext));
                if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0)
                {
                    DataTable dtProducts = dataSet.Tables[0];
                    DataTable dtCategory = dataSet.Tables[1];
                    DataTable dtColor = dataSet.Tables[2];
                    DataTable dtImage = dataSet.Tables[3];
                    DataTable dtTotal = dataSet.Tables[4];
                    if (dtProducts != null && dtProducts.Rows.Count > 0)
                    {
                        foreach (DataRow item in dtProducts.Rows)
                        {
                            #region Product Category
                            string categoryType = string.Empty;
                            DataRow[] drFind = dtCategory.Select("ProductFK='" + DBHelper.ParseString(item["ProductId"]) + "'");
                            if (drFind != null && drFind.Length > 0)
                            {
                                foreach (var dr in drFind)
                                {
                                    categoryType += DBHelper.ParseString(dr["categoryType"]) + ", ";
                                }
                                categoryType = categoryType.Trim().TrimEnd(',');
                            }
                            #endregion

                            #region Image
                            string image = string.Empty;
                            DataRow[] drImage = dtImage.Select("ProductFK='" + DBHelper.ParseString(item["ProductId"]) + "'");
                            if (drImage != null && drImage.Length > 0)
                            {
                                image = hostingPath + Constants.ProductImagesPath.Replace(@"\", "/") + "/" + DBHelper.ParseString(drImage[0]["Image"]);
                            }
                            #endregion

                            #region Color
                            string color = string.Empty;
                            DataRow[] drColor = dtColor.Select("ProductFK='" + DBHelper.ParseString(item["ProductId"]) + "'");
                            if (drColor != null && drColor.Length > 0)
                            {
                                foreach (var dr in drColor)
                                {
                                    color += DBHelper.ParseString(dr["Name"]) + ", ";
                                }
                                color = color.Trim().TrimEnd(',');
                            }
                            #endregion

                            productListDataModel.Add(new ProductListDataModel
                            {
                                CategoryType = categoryType,
                                IsDelete = DBHelper.ParseString(item["IsDelete"]),
                                Price = DBHelper.ParseString(item["Price"]),
                                ProductDescription = DBHelper.ParseString(item["ProductDescription"]),
                                ProductName = DBHelper.ParseString(item["Name"]),
                                ProductId = DBHelper.ParseString(item["ProductId"]),
                                Color = color,
                                Image = image,
                                Status = DBHelper.ParseInt32(item["Status"]),
                                Size = DBHelper.ParseString(item["SizeName"])
                            });
                        }
                        productListModel.Items = productListDataModel;
                        productListModel.Total = DBHelper.ParseString(dtTotal.Rows.Count);
                    }
                }
                return productListModel;
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }


        /// <summary>
        /// Deletes the product.
        /// </summary>
        /// <param name="ProductId">The product identifier.</param>
        /// <returns></returns>
        public bool DeleteProduct(long productId, long updatedBy)
        {
            try
            {
                var productModel = context.Product.Where(x => x.ProductId == productId).FirstOrDefault();
                if (productModel != null)
                {
                    productModel.UpdatedBy = updatedBy;
                    productModel.ModifiedOn = DateTime.Now;
                    productModel.IsDelete = true;
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
        /// Gets the product detail.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="hostingURL">The hosting URL.</param>
        /// <returns></returns>
        public ProductDetailModel GetProductDetail(long productId, string hostingURL)
        {
            try
            {
                ProductDetailModel model = new ProductDetailModel();
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@PRODUCTID", productId);
                DataSet ds = DBHelper.GetDataTable("PRODUCTDETAIL", parameters, DBHelper.ParseString(settings.Value.AppDbContext));
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                {
                    DataTable dtProduct = ds.Tables[0];
                    DataTable dtImage = ds.Tables[1];
                    DataTable dtCategory = ds.Tables[3];
                    DataTable dtColor = ds.Tables[2];
                    DataTable dtProductSize = ds.Tables[4];

                    if (dtProduct != null && dtProduct.Rows.Count > 0)
                    {
                        model.ProductId = DBHelper.ParseString(dtProduct.Rows[0]["ProductId"]);
                        model.IsDelete = DBHelper.ParseString(dtProduct.Rows[0]["IsDelete"]);
                        model.Name = DBHelper.ParseString(dtProduct.Rows[0]["Name"]);
                        model.Price = DBHelper.ParseString(dtProduct.Rows[0]["Price"]);
                        model.ProductDescription = DBHelper.ParseString(dtProduct.Rows[0]["ProductDescription"]);
                        model.ProductSizeId = DBHelper.ParseString(dtProduct.Rows[0]["SizeId"]);
                        foreach (DataRow item in dtProduct.Rows)
                        {
                            model.ProductSize += DBHelper.ParseString(item["SizeName"]) + "/";
                        }
                        model.ProductSize = model.ProductSize.TrimEnd('/');
                        model.ProductSize += " - ";
                        foreach (DataRow item in dtProductSize.Rows)
                        {
                            model.ProductSize += DBHelper.ParseString(item["Name"]) + "/";
                        }
                        model.ProductSize = model.ProductSize.TrimEnd('/');
                        List<ProductDetailSizeGroupListModel> sizeGroupListModels = new List<ProductDetailSizeGroupListModel>();
                        for (int i = 0; i < dtProduct.Rows.Count; i++)
                        {
                            sizeGroupListModels.Add(new ProductDetailSizeGroupListModel
                            {
                                ItemName = DBHelper.ParseString(dtProduct.Rows[i]["SizeName"]),
                                ItemValue = DBHelper.ParseString(dtProductSize.Rows[i]["Name"]),
                            });
                        }
                        model.SizeGroupList = sizeGroupListModels;
                    }

                    if (dtImage != null && dtImage.Rows.Count > 0)
                    {
                        List<ProductDetailImageListModel> productDetailImageListModel = new List<ProductDetailImageListModel>();
                        foreach (DataRow item in dtImage.Rows)
                        {
                            productDetailImageListModel.Add(new ProductDetailImageListModel
                            {
                                Image = hostingURL + Constants.ProductImagesPath.Replace(@"\", "/") + "/" + DBHelper.ParseString(item["Image"]),
                                ProductImageId = DBHelper.ParseString(item["ProductImageId"]),
                                ProductColorId = DBHelper.ParseString(item["ColorFK"])

                            });
                        }
                        model.ImageList = productDetailImageListModel;
                    }

                    if (dtColor != null && dtColor.Rows.Count > 0)
                    {
                        List<ProductDetailColorListModel> productDetailColorListModel = new List<ProductDetailColorListModel>();
                        foreach (DataRow item in dtColor.Rows)
                        {
                            productDetailColorListModel.Add(new ProductDetailColorListModel
                            {
                                Color = DBHelper.ParseString(item["Name"]),
                                ProductColorId = DBHelper.ParseString(item["ColorId"]),
                                ProductColorId_1 = DBHelper.ParseString(item["ProductColorId"]),
                            });
                        }
                        model.ColorList = productDetailColorListModel;
                    }

                    if (dtCategory != null && dtCategory.Rows.Count > 0)
                    {
                        List<ProductDetailCategoryTypeListModel> productDetailCategoryTypeListModel = new List<ProductDetailCategoryTypeListModel>();
                        foreach (DataRow item in dtCategory.Rows)
                        {
                            productDetailCategoryTypeListModel.Add(new ProductDetailCategoryTypeListModel
                            {
                                CategoryType = DBHelper.ParseString(item["CategoryType"]),
                                ProductCategoryTypeId = DBHelper.ParseString(item["ProductCategoryTypeId"])
                            });
                        }
                        model.CategoryList = productDetailCategoryTypeListModel;
                    }

                    return model;
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
        /// Updates the product.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="productColors">The product colors.</param>
        /// <param name="productCategoryDetails">The product category details.</param>
        /// <returns></returns>
        public long UpdateProduct(Product model, List<ProductCategoryDetail> productCategoryDetails, string hostPath, string[] ProductSizeValues)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var productModelCheck = context.Product.Where(x => x.Name == model.Name && x.IsDelete == false && x.ProductId != model.ProductId).FirstOrDefault();
                    if (productModelCheck == null)
                    {
                        var productModel = context.Product.Where(x => x.ProductId == model.ProductId).FirstOrDefault();
                        if (productModel != null)
                        {
                            productModel.ModifiedOn = model.ModifiedOn;
                            productModel.UpdatedBy = model.UpdatedBy;
                            productModel.Name = model.Name;
                            productModel.ProductDescription = model.ProductDescription;
                            productModel.Price = model.Price;
                            productModel.SizeFK = model.SizeFK;
                            context.SaveChanges();
                            long ProductId = model.ProductId;
                            if (ProductId > 0)
                            {
                                #region ProductSizeValues 
                                var sizeCount = context.ProductSizeDetail.Where(x => x.ProductFK == ProductId).ToList();
                                if (sizeCount != null)
                                {
                                    foreach (var item in sizeCount)
                                    {
                                        context.ProductSizeDetail.Remove(item);
                                        context.SaveChanges();
                                    }
                                }

                                if (ProductSizeValues != null && ProductSizeValues.Length > 0)
                                {
                                    foreach (var item in ProductSizeValues)
                                    {
                                        if (!string.IsNullOrWhiteSpace(item))
                                        {
                                            ProductSizeDetail productSizeDetail = new ProductSizeDetail();
                                            productSizeDetail.Name = item;
                                            productSizeDetail.ProductFK = ProductId;
                                            context.ProductSizeDetail.Add(productSizeDetail);
                                            context.SaveChanges();
                                            if (productSizeDetail.ProductSizeDetailId <= 0)
                                            {
                                                transaction.Rollback();
                                                return 0;
                                            }
                                        }

                                    }
                                }
                                #endregion

                                #region Product Category
                                var categoryCount = context.ProductCategoryDetail.Where(x => x.ProductFK == ProductId).ToList();
                                if (categoryCount != null)
                                {
                                    foreach (var item in categoryCount)
                                    {
                                        context.ProductCategoryDetail.Remove(item);
                                        context.SaveChanges();
                                    }
                                }

                                if (productCategoryDetails != null && productCategoryDetails.Count > 0)
                                {
                                    foreach (var item in productCategoryDetails)
                                    {
                                        if (item.ProductCategoryTypeFK > 0)
                                        {
                                            item.ProductFK = ProductId;
                                            context.ProductCategoryDetail.Add(item);
                                            context.SaveChanges();
                                            if (item.ProductCategoryDetailId <= 0)
                                            {
                                                transaction.Rollback();
                                                return 0;
                                            }
                                        }

                                    }
                                }
                                #endregion

                                transaction.Commit();
                                return ProductId;
                            }
                            else
                            {
                                transaction.Rollback();
                                return 0;
                            }
                        }
                        else
                        {
                            transaction.Rollback();
                            return 0;
                        }
                    }
                    else
                    {
                        transaction.Rollback();
                        return ReturnCode.AlreadyExist.GetHashCode();
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                    transaction.Rollback();
                    throw ex;
                }
            }
        }


        /// <summary>
        /// Gets the product images.
        /// </summary>
        /// <param name="ProductId">The product identifier.</param>
        /// <returns></returns>
        public List<ProductImage> GetProductImages(long productId)
        {
            try
            {
                //return context.ProductImage.Where(x => x.ProductFK == productId).ToList();
                return context.ProductImage.ToList();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }


        /// <summary>
        /// Changes the product status.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public bool ChangeProductStatus(long productId, int status)
        {
            try
            {
                var product = context.Product.Where(x => x.IsDelete == false && x.ProductId == productId).FirstOrDefault();
                if (product != null)
                {
                    if (status == ProductStatus.Active.GetHashCode())
                    {
                        product.Status = ProductStatus.Active.GetHashCode();
                    }
                    else if (status == ProductStatus.Unactive.GetHashCode())
                    {
                        product.Status = ProductStatus.Unactive.GetHashCode();
                    }
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
        /// Gets the product images by product ids.
        /// </summary>
        /// <param name="productsIds">The products ids.</param>
        /// <returns></returns>
        public List<ProductImage> GetProductImagesByProductIds(List<long> productsIds)
        {
            try
            {
                return context.ProductImage.Include(x => x.ProductColor).Where(x => productsIds.Contains(x.ProductColor.ProductFK)).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }


        /// <summary>
        /// Gets the order product list.
        /// </summary>
        /// <returns></returns>
        public OrderProductListModel GetOrderProductList(int pageNo, int rowsPerPage, string searchString, string hostingURL, string column, string direction)
        {
            OrderProductListModel orderProductListModel = new OrderProductListModel();
            List<OrderProductListDataModel> orderProductListDataModels = new List<OrderProductListDataModel>();
            try
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[5];
                    parameter[0] = new SqlParameter("PAGENO", pageNo);
                    parameter[1] = new SqlParameter("RECORDPERPAGE", rowsPerPage);
                    parameter[2] = new SqlParameter("SEARCHSTRING", searchString);
                    parameter[3] = new SqlParameter("COLUMN", column);
                    parameter[4] = new SqlParameter("DIRECTION", direction);
                    DataSet dataSet = DBHelper.GetDataTable("ORDERPRODUCTLIST", parameter, DBHelper.ParseString(settings.Value.AppDbContext));
                    if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0)
                    {
                        DataTable dtProducts = dataSet.Tables[0];
                        DataTable dtTotal = dataSet.Tables[1];
                        DataTable dtImage = dataSet.Tables[2];
                        DataTable dtSizeGroup = dataSet.Tables[3];
                        DataTable dtProductSizeGroup = dataSet.Tables[4];

                        if (dtProducts != null && dtProducts.Rows.Count > 0)
                        {
                            foreach (DataRow item in dtProducts.Rows)
                            {
                                OrderProductListDataModel orderProductListModelObject = new OrderProductListDataModel();
                                orderProductListModelObject.ColorId = DBHelper.ParseString(item["ProductColorId"]);
                                orderProductListModelObject.Color = DBHelper.ParseString(item["ColorName"]);
                                orderProductListModelObject.Price = DBHelper.ParseString(item["Price"]);
                                orderProductListModelObject.ProductId = DBHelper.ParseString(item["ProductId"]);
                                orderProductListModelObject.ProductName = DBHelper.ParseString(item["Name"]);
                                DataRow[] drSizeGroup = dtSizeGroup.Select("SizeFK='" + DBHelper.ParseString(item["SizeId"]) + "'");
                                if (drSizeGroup != null && drSizeGroup.Length > 0)
                                {
                                    foreach (var size in drSizeGroup)
                                    {
                                        orderProductListModelObject.SizeGroup += size["Name"] + "/";
                                    }
                                    orderProductListModelObject.SizeGroup = orderProductListModelObject.SizeGroup.TrimEnd('/');
                                    orderProductListModelObject.SizeGroup += " - ";
                                }

                                DataRow[] drProductSizeGroup = dtProductSizeGroup.Select("ProductFK='" + DBHelper.ParseString(item["ProductId"]) + "'");
                                if (drProductSizeGroup != null && drProductSizeGroup.Length > 0)
                                {
                                    foreach (var size in drProductSizeGroup)
                                    {
                                        orderProductListModelObject.SizeGroup += size["Name"] + "/";
                                    }
                                    orderProductListModelObject.SizeGroup = orderProductListModelObject.SizeGroup.TrimEnd('/');
                                }

                                DataRow[] drImage = dtImage.Select("ProductColorFK='" + item["ProductColorId"] + "'");
                                if (drImage != null && drImage.Length > 0)
                                {
                                    orderProductListModelObject.Image = hostingURL + Constants.ProductImagesPath.Replace(@"\", "/") + "/" + DBHelper.ParseString(drImage[0]["Image"]);
                                }

                                orderProductListDataModels.Add(orderProductListModelObject);
                            }
                            orderProductListModel.Items = orderProductListDataModels;
                            orderProductListModel.Total = DBHelper.ParseString(dtTotal.Rows.Count);
                        }
                    }
                    return orderProductListModel;
                }
                catch (Exception ex)
                {
                    LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                    throw ex;
                }
            }
            catch (Exception ex)
            {

                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Gets the product size details.
        /// </summary>
        /// <param name="productIds">The product ids.</param>
        /// <returns></returns>
        public List<ProductSizeDetail> GetProductSizeDetails(List<long> productIds)
        {
            try
            {
                return context.ProductSizeDetail.Where(x => productIds.Contains(x.ProductFK)).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Adds the product color image.
        /// </summary>
        /// <param name="ProductId">The product identifier.</param>
        /// <param name="ColorId">The color identifier.</param>
        /// <param name="productImages">The product images.</param>
        /// <param name="hostPath">The host path.</param>
        /// <param name="deleteImageIds">The delete image ids.</param>
        /// <returns></returns>
        public long AddProductColorImage(long ProductId, long ColorId, List<IFormFile> productImages, string hostPath, string deleteImageIds)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    ProductColor productColor = new ProductColor();
                    var colorList = context.ProductColor.Where(x => x.ProductFK == ProductId && x.ColorFK == ColorId).FirstOrDefault();
                    if (colorList == null)
                    {
                        productColor.ColorFK = ColorId;
                        productColor.ProductFK = ProductId;
                        context.ProductColor.Add(productColor);
                        context.SaveChanges();
                    }
                    else
                    {
                        productColor = colorList;
                    }
                    if (productColor.ProductColorId <= 0)
                    {
                        transaction.Rollback();
                        return 0;
                    }
                    else
                    {
                        if (productImages != null)
                        {
                            foreach (var item in productImages)
                            {
                                ProductImage productImage = new ProductImage();
                                productImage.ProductColorFK = productColor.ProductColorId;
                                productImage.Image = ProductHelper.SaveProductImageToFolder(item, hostPath);
                                context.ProductImage.Add(productImage);
                                context.SaveChanges();
                                if (productImage.ProductImageId <= 0)
                                {
                                    transaction.Rollback();
                                    return 0;
                                }
                            }
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(deleteImageIds))
                    {
                        string[] splittedImageId = deleteImageIds.Split(',');
                        if (splittedImageId != null && splittedImageId.Length > 0)
                        {
                            foreach (string item in splittedImageId)
                            {
                                if (!string.IsNullOrWhiteSpace(item))
                                {

                                    long imageId = DBHelper.ParseInt64(item);
                                    var productImage = context.ProductImage.Where(x => x.ProductImageId == imageId).FirstOrDefault();
                                    if (productImage != null)
                                    {
                                        ProductHelper.DeleteProductImageToFolder(productImage.Image, hostPath);
                                        context.ProductImage.Remove(productImage);
                                        context.SaveChanges();
                                    }
                                }
                            }
                        }
                    }

                    transaction.Commit();
                    return productColor.ProductColorId;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                    throw ex;
                }
            }

        }

        /// <summary>
        /// Deletes the color of the product.
        /// </summary>
        /// <param name="ProductId">The product identifier.</param>
        /// <param name="ColorId">The color identifier.</param>
        /// <param name="hostPath">The host path.</param>
        /// <returns></returns>
        public bool DeleteProductColor(long ProductId, long ColorId, string hostPath)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var colorList = context.ProductColor.Where(x => x.ProductFK == ProductId && x.ColorFK == ColorId).FirstOrDefault();
                    if(colorList!=null)
                    {
                        colorList.IsDelete = true;
                        context.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    //var colorList = context.ProductColor.Where(x => x.ProductFK == ProductId && x.ColorFK == ColorId).ToList();
                    //if (colorList != null)
                    //{
                    //    foreach (var item in colorList)
                    //    {
                    //        var imageList = context.ProductImage.Where(x => x.ProductColorFK == item.ProductColorId).ToList();
                    //        if (imageList != null)
                    //        {
                    //            foreach (var image in imageList)
                    //            {
                    //                ProductHelper.DeleteProductImageToFolder(image.Image, hostPath);
                    //                context.ProductImage.Remove(image);
                    //                context.SaveChanges();
                    //            }
                    //        }
                    //        context.ProductColor.Remove(item);
                    //        context.SaveChanges();
                    //    }
                    //}
                    //transaction.Commit();
                    //return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Gets the image count by product color identifier.
        /// </summary>
        /// <param name="ProductId">The product identifier.</param>
        /// <param name="ColorId">The color identifier.</param>
        /// <returns></returns>
        public long GetImageCountByProductColorId(long ProductId, long ColorId)
        {
            try
            {
                var colorList = context.ProductColor.Where(x => x.ProductFK == ProductId && x.ColorFK == ColorId).FirstOrDefault();
                if (colorList != null)
                {
                    return context.ProductImage.Where(x => x.ProductColorFK == colorList.ProductColorId).Count();
                }
                else
                {
                    return 0;
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
        /// <param name="productIds">The product ids.</param>
        /// <returns></returns>
        public List<ProductCategoryDetail> GetProductCategoryDetail(List<long> productIds)
        {
            try
            {
                return context.ProductCategoryDetail.Include(x=>x.ProductCategoryType).Where(x => productIds.Contains(x.ProductFK)).ToList();
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
