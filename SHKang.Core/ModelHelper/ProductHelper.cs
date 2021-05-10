namespace SHKang.Core.ModelHelper
{
    using Microsoft.AspNetCore.Http;
    #region namespaces

    using SHKang.Core.Constant;
    using SHKang.Core.Enums;
    using SHKang.Core.Helpers;
    using SHKang.Model.Models;
    using SHKang.Model.ViewModels.Admin;
    using SHKang.Model.ViewModels.Web;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    #endregion

    public static class ProductHelper
    {
        /// <summary>
        /// Converts to product model.
        /// </summary>
        /// <param name="addProductModel">The model.</param>
        /// <returns></returns>
        public static Product BindProductModel(AddProductModel addProductModel)
        {
            Product productModel = new Product();
            if (addProductModel != null)
            {
                productModel.Name = addProductModel.Name;
                productModel.Price = DBHelper.ParseDecimal(addProductModel.Price);
                productModel.ProductDescription = addProductModel.ProductDescription;
                productModel.SizeFK = addProductModel.ProductSizeId;
                //productModel.ProductSizeRatioFK = addProductModel.ProductSizeRatioId;
                if (addProductModel.ProductId <= 0)
                {
                    productModel.CreatedBy = addProductModel.CreatedBy;
                    productModel.CreatedOn = DateTime.Now;
                    productModel.Status = ProductStatus.Active.GetHashCode();
                }
                else
                {
                    productModel.UpdatedBy = DBHelper.ParseInt64(addProductModel.UpdatedBy);
                    productModel.ModifiedOn = DateTime.Now;
                    productModel.ProductId = addProductModel.ProductId;
                }
            }
            return productModel;
        }

        /// <summary>
        /// Converts to product image list model.
        /// </summary>
        /// <param name="productModel">The model.</param>
        /// <returns></returns>
        public static List<ProductImage> BindProductImageListModel(AddProductModel productModel, string hostPath)
        {
            List<ProductImage> productImageModel = new List<ProductImage>();
            if (productModel != null)
            {
                var hostingPath = hostPath;
                //string[] sortNumberSplitted = productModel.SortNumber.Split(',');
                //if (productModel.ProductImage != null && productModel.ProductImage.Count > 0)
                //{
                //    for (int i = 0; i < productModel.ProductImage.Count; i++)
                //    {
                //        long ticks = DateTime.Now.Ticks;
                //        string path = string.Concat(hostingPath, "/" + Constants.ProductImagesPath);
                //        string fileName = ticks + Path.GetExtension(productModel.ProductImage[i].FileName);
                //        if (!Directory.Exists(path))
                //        {
                //            Directory.CreateDirectory(path);
                //        }
                //        path = path + "\\" + fileName;
                //        using (var fileStream = new FileStream(path, FileMode.Create))
                //        {
                //            productModel.ProductImage[i].CopyTo(fileStream);
                //        }
                //        productImageModel.Add(new ProductImage
                //        {
                //            Image = fileName,
                //            SortNumber = DBHelper.ParseInt32(sortNumberSplitted[i])
                //        });
                //    }
                //}
            }
            return productImageModel;
        }

        /// <summary>
        /// Converts to product color list model.
        /// </summary>
        /// <param name="addProductModel">The model.</param>
        /// <returns></returns>
        public static List<ProductColor> BindProductColorListModel(AddProductModel addProductModel)
        {
            List<ProductColor> productColorModel = new List<ProductColor>();
            //if (addProductModel != null)
            //{
            //    if (!string.IsNullOrWhiteSpace(addProductModel.ProductColorId))
            //    {
            //        string[] productColorIdSplitted = addProductModel.ProductColorId.Split(',');
            //        if (productColorIdSplitted != null && productColorIdSplitted.Length > 0)
            //        {
            //            foreach (var item in productColorIdSplitted)
            //            {
            //                if (!string.IsNullOrWhiteSpace(item) && !item.Equals("0"))
            //                {
            //                    productColorModel.Add(new ProductColor
            //                    {
            //                        ColorFK = DBHelper.ParseInt64(item)
            //                    });
            //                }
            //            }
            //        }
            //    }
            //}
            return productColorModel;
        }

        /// <summary>
        /// Converts to product category list model.
        /// </summary>
        /// <param name="addProductModel">The model.</param>
        /// <returns></returns>
        public static List<ProductCategoryDetail> BindProductCategoryListModel(AddProductModel addProductModel)
        {
            List<ProductCategoryDetail> productCategoryDetailModel = new List<ProductCategoryDetail>();
            if (addProductModel != null)
            {
                if (!string.IsNullOrWhiteSpace(addProductModel.ProductCategoryId))
                {
                    string[] productCategoryIdSplitted = addProductModel.ProductCategoryId.Split(',');
                    if (productCategoryIdSplitted != null && productCategoryIdSplitted.Length > 0)
                    {
                        foreach (var item in productCategoryIdSplitted)
                        {
                            if (!string.IsNullOrWhiteSpace(item) && !item.Equals("0"))
                            {
                                productCategoryDetailModel.Add(new ProductCategoryDetail
                                {
                                    ProductCategoryTypeFK = DBHelper.ParseInt64(item)
                                });
                            }
                        }
                    }
                }
            }
            return productCategoryDetailModel;
        }

        /// <summary>
        /// Deletes the product image.
        /// </summary>
        /// <param name="productImageList">The object list.</param>
        /// <returns></returns>
        public static bool DeleteProductImage(List<ProductImage> productImageList, string hostPath)
        {
            if (productImageList != null && productImageList.Count > 0)
            {
                foreach (var item in productImageList)
                {
                    var hostingPath = hostPath;
                    string path = string.Concat(hostingPath, "/" + Constants.ProductImagesPath);
                    string fileName = item.Image;
                    path = path + "\\" + fileName;
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Converts to campaign list model.
        /// </summary>
        /// <param name="productCategoryTypeModel">The model.</param>
        /// <returns></returns>
        public static List<CampaignListModel> BindCampaignListModel(List<ProductCategoryType> productCategoryTypeModel, List<ProductCategoryDetail> productList, string hostPath)
        {
            List<CampaignListModel> campaignListModel = new List<CampaignListModel>();
            if (productCategoryTypeModel != null)
            {
                foreach (var item in productCategoryTypeModel)
                {
                    var productCount = productList.Where(x => x.ProductCategoryTypeFK == item.ProductCategoryTypeId).Count();
                    var productIdsList = productList.Where(x => x.ProductCategoryTypeFK == item.ProductCategoryTypeId).ToList();
                    List<long> productids = new List<long>();
                    for (int i = 0; i < productIdsList.Count; i++)
                    {
                        productids.Add(DBHelper.ParseInt64(productIdsList[i].ProductFK));
                    }
                    string image = string.Empty;
                    if (string.IsNullOrWhiteSpace(item.Image))
                    {
                        image = hostPath  + "/" + Constants.NoImageAvaliablePath.Replace(@"\", "/");
                    }
                    else
                    {
                        image = hostPath + Constants.ProductCategoryTypeImagesPath.Replace(@"\", "/") + "/" + DBHelper.ParseString(item.Image);
                    }
                    campaignListModel.Add(new CampaignListModel
                    {
                        CampaignId = DBHelper.ParseString(item.ProductCategoryTypeId),
                        CampaignName = DBHelper.ParseString(item.CategoryType),
                        Description = DBHelper.ParseString(item.Description),
                        TotalProduct = DBHelper.ParseString(productCount),
                        Image = image,
                        ProductIds = productids,
                        IsShowinFrontend= item.IsShowInFrontend
                    });
                }
            }
            return campaignListModel;
        }

        /// <summary>
        /// Converts to product image list model.
        /// </summary>
        /// <param name="productModel">The model.</param>
        /// <returns></returns>
        public static string SaveProductImageToFolder(IFormFile file, string hostPath)
        {
            List<AddProductColorModel> addProductColorModelsList = new List<AddProductColorModel>();
            var hostingPath = hostPath;
            if (file != null)
            {
                long ticks = DateTime.Now.Ticks;
                string path = string.Concat(hostingPath, "/" + Constants.ProductImagesPath);
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
        /// Deletes the product image to folder.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="hostPath">The host path.</param>
        /// <returns></returns>
        public static bool DeleteProductImageToFolder(string fileName, string hostPath)
        {
            List<AddProductColorModel> addProductColorModelsList = new List<AddProductColorModel>();
            var hostingPath = hostPath;
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                long ticks = DateTime.Now.Ticks;
                string path = string.Concat(hostingPath, "\\" + Constants.ProductImagesPath);
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

        /// <summary>
        /// Binds the product category type ids.
        /// </summary>
        /// <param name="productCategoryTypesList">The product category types list.</param>
        /// <returns></returns>
        public static List<long> BindProductCategoryTypeIds(List<ProductCategoryType> productCategoryTypesList)
        {
            List<long> productCategoryTypeIds = new List<long>();
            if (productCategoryTypesList != null && productCategoryTypesList.Count > 0)
            {
                foreach (var item in productCategoryTypesList)
                {
                    productCategoryTypeIds.Add(item.ProductCategoryTypeId);
                }
            }
            return productCategoryTypeIds;
        }
    }
}
