namespace SHKang.Core.ModelHelper
{
    using Microsoft.AspNetCore.Http;
    using SHKang.Core.Constant;
    using SHKang.Core.Enums;
    using SHKang.Core.Helpers;
    #region namespaces

    using SHKang.Model.Models;
    using SHKang.Model.ViewModels.Admin;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    #endregion

    public static class ProductCategoryTypeHelper
    {

        /// <summary>
        /// Converts to product category type model.
        /// </summary>
        /// <param name="addProductCategoryTypeModel">The model.</param>
        /// <returns></returns>
        public static ProductCategoryType BindProductCategoryTypeModel(AddProductCategoryTypeModel addProductCategoryTypeModel)
        {
            ProductCategoryType productCategoryTypeModel = new ProductCategoryType();
            if (addProductCategoryTypeModel != null)
            {
                productCategoryTypeModel.CategoryType = addProductCategoryTypeModel.CategoryType;
                //productCategoryTypeModel.ProductCategoryFK = addProductCategoryTypeModel.ProductCategoryId;
                productCategoryTypeModel.ProductCategoryFK = ProductCategoryEnum.Styles.GetHashCode();
                productCategoryTypeModel.CreatedBy = addProductCategoryTypeModel.CreatedBy;
                productCategoryTypeModel.CreatedOn = DateTime.Now;
            }
            return productCategoryTypeModel;
        }

        /// <summary>
        /// Converts to product category model.
        /// </summary>
        /// <param name="updateProductCategoryTypeModel">The model.</param>
        /// <returns></returns>
        public static ProductCategoryType BindProductCategoryModel(UpdateProductCategoryTypeModel updateProductCategoryTypeModel, bool isUpdate = false)
        {
            ProductCategoryType productCategoryTypeModel = new ProductCategoryType();
            if (updateProductCategoryTypeModel != null)
            {
                //productCategoryTypeModel.ProductCategoryFK = updateProductCategoryTypeModel.ProductCategoryId;
                productCategoryTypeModel.ProductCategoryTypeId = updateProductCategoryTypeModel.ProductCategoryTypeId;
                productCategoryTypeModel.CategoryType = updateProductCategoryTypeModel.CategoryType;
                productCategoryTypeModel.ProductCategoryFK = ProductCategoryEnum.Styles.GetHashCode();
                if (isUpdate)
                {
                    productCategoryTypeModel.UpdatedBy = updateProductCategoryTypeModel.UpdatedBy;
                    productCategoryTypeModel.ModifiedOn = DateTime.Now;
                }
                else
                {
                    productCategoryTypeModel.CreatedBy = updateProductCategoryTypeModel.UpdatedBy;
                    productCategoryTypeModel.CreatedOn = DateTime.Now;
                }
            }
            return productCategoryTypeModel;
        }

        /// <summary>
        /// Converts the product category type list model.
        /// </summary>
        /// <param name="productCategoryTypeModel">The model.</param>
        /// <returns></returns>
        public static List<ProductCategoryTypeDataModel> BindProductCategoryTypeListModel(List<ProductCategoryType> productCategoryTypeModel, List<ProductCategoryDetail> productCategoryDetailList)
        {
            List<ProductCategoryTypeDataModel> productCategoryTypeListModel = new List<ProductCategoryTypeDataModel>();
            if (productCategoryTypeModel != null)
            {
                foreach (var item in productCategoryTypeModel)
                {
                    var totalProducts = productCategoryDetailList.Where(x => x.ProductCategoryTypeFK == item.ProductCategoryTypeId).Count();

                    productCategoryTypeListModel.Add(new ProductCategoryTypeDataModel
                    {
                        //ProductCategory = item.ProductCategory.Name,
                        //ProductCategoryId = item.ProductCategory.ProductCategoryId,
                        Name = item.CategoryType,
                        ProductCategoryTypeId = item.ProductCategoryTypeId,
                        TotalProduct = DBHelper.ParseString(totalProducts)
                    });
                }
            }
            return productCategoryTypeListModel;
        }

        /// <summary>
        /// Converts the product category type list model.
        /// </summary>
        /// <param name="productCategoryTypeModel">The model.</param>
        /// <returns></returns>
        public static ProductCategoryTypeDataModel BindProductCategoryTypeListModel(ProductCategoryType productCategoryTypeModel)
        {
            ProductCategoryTypeDataModel productCategoryTypeListModel = new ProductCategoryTypeDataModel();
            if (productCategoryTypeModel != null)
            {
                productCategoryTypeListModel.Name = productCategoryTypeModel.CategoryType;
                productCategoryTypeListModel.ProductCategoryTypeId = productCategoryTypeModel.ProductCategoryTypeId;
                //productCategoryTypeListModel.ProductCategory = productCategoryTypeModel.ProductCategory.Name;
                //productCategoryTypeListModel.ProductCategoryId = productCategoryTypeModel.ProductCategoryFK;
            }
            return productCategoryTypeListModel;
        }

        /// <summary>
        /// Gets the product category type identifier.
        /// </summary>
        /// <param name="productCategoryTypeModelList">The product category type model list.</param>
        /// <returns></returns>
        public static List<long> GetProductCategoryTypeId(List<ProductCategoryType> productCategoryTypeModelList)
        {
            List<long> productCategoryTypeId = new List<long>();
            if (productCategoryTypeModelList != null)
            {
                foreach (var item in productCategoryTypeModelList)
                {
                    productCategoryTypeId.Add(item.ProductCategoryTypeId);
                }
            }
            return productCategoryTypeId;
        }

        /// <summary>
        /// Saves the product category type image to folder.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="hostPath">The host path.</param>
        /// <returns></returns>
        public static string SaveProductCategoryTypeImageToFolder(IFormFile file, string hostPath)
        {
            var hostingPath = hostPath;
            if (file != null)
            {
                long ticks = DateTime.Now.Ticks;
                string path = string.Concat(hostingPath, "/" + Constants.ProductCategoryTypeImagesPath);
                string fileName = ticks + Path.GetExtension(file.FileName);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = path + "\\" + fileName;
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                return fileName;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Binds the product category type model.
        /// </summary>
        /// <param name="addProductCategoryTypeModel">The add product category type model.</param>
        /// <param name="hostPath">The host path.</param>
        /// <returns></returns>
        public static ProductCategoryType BindProductCategoryTypeModel(AddUpdateCampaignModel addProductCategoryTypeModel, string hostPath, ProductCategoryType productCategoryType)
        {
            ProductCategoryType productCategoryTypeModel = new ProductCategoryType();
            if (addProductCategoryTypeModel != null)
            {
                if (DBHelper.ParseInt64(addProductCategoryTypeModel.CampaignId) <= 0)
                {
                    productCategoryTypeModel.CreatedBy = DBHelper.ParseInt64(addProductCategoryTypeModel.UpdatedBy);
                    productCategoryTypeModel.CreatedOn = DateTime.Now;
                    productCategoryTypeModel.Image = SaveProductCategoryTypeImageToFolder(addProductCategoryTypeModel.Image, hostPath);
                }
                else
                {
                    productCategoryTypeModel.ProductCategoryTypeId = DBHelper.ParseInt64(addProductCategoryTypeModel.CampaignId);
                    productCategoryTypeModel.UpdatedBy = DBHelper.ParseInt64(addProductCategoryTypeModel.UpdatedBy);
                    productCategoryTypeModel.ModifiedOn = DateTime.Now;
                    if (addProductCategoryTypeModel.IsImageDeleted)
                    {
                        DeleteProductImageToFolder(productCategoryType.Image, hostPath);
                        productCategoryTypeModel.Image = string.Empty;
                    }
                    else if(addProductCategoryTypeModel.Image==null)
                    {
                        productCategoryTypeModel.Image = productCategoryType.Image;
                    }
                    else
                    {
                        productCategoryTypeModel.Image = SaveProductCategoryTypeImageToFolder(addProductCategoryTypeModel.Image, hostPath);
                    }
                }
                productCategoryTypeModel.CategoryType = addProductCategoryTypeModel.CampaignName;
                productCategoryTypeModel.ProductCategoryFK = ProductCategoryEnum.Campaign.GetHashCode();
                productCategoryTypeModel.Description = addProductCategoryTypeModel.Description;
                productCategoryTypeModel.IsShowInFrontend = addProductCategoryTypeModel.IsShowinFrontend;

            }
            return productCategoryTypeModel;
        }

        /// <summary>
        /// Deletes the product image to folder.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="hostPath">The host path.</param>
        /// <returns></returns>
        public static bool DeleteProductImageToFolder(string fileName, string hostPath)
        {
            List<AddProductColorModel> addProductColorModelsList = new List<AddProductColorModel>();
            var hostingPath = hostPath;
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                long ticks = DateTime.Now.Ticks;
                string path = string.Concat(hostingPath, "\\" + Constants.ProductCategoryTypeImagesPath);
                path = path + "\\" + fileName;
                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
