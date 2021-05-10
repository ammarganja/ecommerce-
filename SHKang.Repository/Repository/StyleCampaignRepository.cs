
namespace SHKang.Repository.Repository
{
    #region namespaces
    using Microsoft.Extensions.Options;
    using SHKang.Core.Constant;
    using SHKang.Core.Enums;
    using SHKang.Core.Helpers;
    using SHKang.Model.Context;
    using SHKang.Model.Models;
    using SHKang.Model.ViewModels.Admin;
    using SHKang.Model.ViewModels.Web;
    using SHKang.Repository.Interface;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;

    #endregion

    public class StyleCampaignRepository : IStyleCampaign
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
        /// Initializes a new instance of the <see cref="StyleCampaignRepository"/> class.
        /// </summary>
        /// <param name="_context">The context.</param>
        /// <param name="_settings">The settings.</param>
        public StyleCampaignRepository(AppDbContext _context, IOptions<ConnectionString> _settings)
        {
            context = _context;
            settings = _settings;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets the style campaign list.
        /// </summary>
        /// <param name="productCategoryId">The product category identifier.</param>
        /// <param name="searchText">The search text.</param>
        /// <param name="pageNo">The page no.</param>
        /// <param name="recordPerPage">The record per page.</param>
        /// <param name="productCategoryTypeId">The product category type identifier.</param>
        /// <param name="hostingPath">The hosting path.</param>
        /// <param name="column">The column.</param>
        /// <param name="direction">The direction.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public StyleCampaignList GetStyleCampaignList(string productCategoryId, string searchText, int pageNo, int recordPerPage, string productCategoryTypeId, string hostingPath, string column, string direction)
        {
            StyleCampaignList styleCampaignListModel = new StyleCampaignList();
            try
            {
                SqlParameter[] parameters = new SqlParameter[7];
                parameters[0] = new SqlParameter("PRODUCTCATEGORYID", productCategoryId);
                parameters[1] = new SqlParameter("SEARCHSTRING", searchText);
                parameters[2] = new SqlParameter("PAGENO", pageNo);
                parameters[3] = new SqlParameter("RECORDPERPAGE", recordPerPage);
                parameters[4] = new SqlParameter("PRODUCTCATEGORYTYPEID", productCategoryTypeId);
                parameters[5] = new SqlParameter("COLUMN", column);
                parameters[6] = new SqlParameter("DIRECTION", direction);
                DataSet dataSet = DBHelper.GetDataTable("GETPRODUCTSLIST", parameters, DBHelper.ParseString(settings.Value.AppDbContext));
                if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0)
                {
                    DataTable dtProduct = dataSet.Tables[0];
                    DataTable dtCategory = dataSet.Tables[1];
                    DataTable dtProductCategory = dataSet.Tables[2];
                    DataTable dtTotal = dataSet.Tables[3];
                    DataTable dtPopular = new DataTable();
                    if (dataSet.Tables.Count > 4)
                    {
                        dtPopular = dataSet.Tables[4];
                    }
                    List<StyleCampaignProductList> ProductList = new List<StyleCampaignProductList>();
                    List<ProductCategoryListModel> productCategoryList = new List<ProductCategoryListModel>();
                    if (dtProduct != null && dtProduct.Rows.Count > 0)
                    {
                        if (dtPopular == null || dtPopular.Rows.Count <= 0)
                        {
                            foreach (DataRow item in dtProduct.Rows)
                            {
                                string category = string.Empty;
                                DataRow[] drCategory = dtProductCategory.Select("ProductFK='" + DBHelper.ParseString(item["ProductId"]) + "'");
                                if (drCategory != null && drCategory.Length > 0)
                                {
                                    foreach (var cat in drCategory)
                                    {
                                        category += DBHelper.ParseString(cat["CategoryType"]) + ",";
                                    }
                                    category = category.TrimEnd(',');
                                }

                                string image = string.Empty;
                                if (!string.IsNullOrWhiteSpace(DBHelper.ParseString(item["Image"])))
                                {
                                    image = hostingPath + Constants.ProductImagesPath.Replace(@"\", "/") + "/" + DBHelper.ParseString(item["Image"]);
                                }
                                else
                                {
                                    image = hostingPath + Constants.NoImageAvaliablePath.Replace(@"\", "/");
                                }
                                ProductList.Add(new StyleCampaignProductList
                                {
                                    Category = category,
                                    Image = image,
                                    Price = DBHelper.ParseString(item["Price"]),
                                    ProductId = DBHelper.ParseString(item["ProductId"]),
                                    ProductName = DBHelper.ParseString(item["ProductName"]),
                                    ProductColorId= DBHelper.ParseString(item["ProductColorId"]),
                                });
                            }
                        }
                        else
                        {
                            foreach (DataRow popular in dtProduct.Rows)
                            {
                                DataRow[] item = dtProduct.Select("ProductId='" + popular["ProductId"] + "'");
                                if (item != null && item.Length > 0)
                                {
                                    string category = string.Empty;
                                    DataRow[] drCategory = dtProductCategory.Select("ProductFK='" + DBHelper.ParseString(item[0]["ProductId"]) + "'");
                                    if (drCategory != null && drCategory.Length > 0)
                                    {
                                        foreach (var cat in drCategory)
                                        {
                                            category += DBHelper.ParseString(cat["CategoryType"]) + ",";
                                        }
                                        category = category.TrimEnd(',');
                                    }

                                    string image = string.Empty;
                                    if (!string.IsNullOrWhiteSpace(DBHelper.ParseString(item[0]["Image"])))
                                    {
                                        image = hostingPath + Constants.ProductImagesPath.Replace(@"\", "/") + "/" + DBHelper.ParseString(item[0]["Image"]);
                                    }
                                    else
                                    {
                                        image = hostingPath + Constants.NoImageAvaliablePath.Replace(@"\", "/");
                                    }
                                    ProductList.Add(new StyleCampaignProductList
                                    {
                                        Category = category,
                                        Image = image,
                                        Price = DBHelper.ParseString(item[0]["Price"]),
                                        ProductId = DBHelper.ParseString(item[0]["ProductId"]),
                                        ProductName = DBHelper.ParseString(item[0]["ProductName"]),
                                        ProductColorId = DBHelper.ParseString(item[0]["ProductColorId"]),
                                    });
                                }
                            }
                        }
                    }
                    if (dtCategory != null && dtCategory.Rows.Count > 0)
                    {
                        foreach (DataRow item in dtCategory.Rows)
                        {
                            productCategoryList.Add(new ProductCategoryListModel
                            {
                                Name = DBHelper.ParseString(item["CategoryType"]),
                                ProductCategoryId = DBHelper.ParseInt64(DBHelper.ParseString(item["ProductCategoryTypeId"]))
                            });
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(productCategoryTypeId))
                    {
                        string[] splitted = productCategoryTypeId.Split(',');
                        if (splitted != null && splitted.Length > 0)
                        {
                            DataRow[] drfind = dtCategory.Select("ProductCategoryTypeId='" + splitted[0] + "'");
                            if (drfind!=null && drfind.Length>0)
                            {
                                StyleCampaignDetailModel campaignDetailModel = new StyleCampaignDetailModel();
                                campaignDetailModel.CampaignId =DBHelper.ParseString(drfind[0]["ProductCategoryTypeId"]);
                                campaignDetailModel.CampaignName = DBHelper.ParseString(drfind[0]["CategoryType"]);
                                string image = string.Empty;
                                if (string.IsNullOrWhiteSpace(DBHelper.ParseString(drfind[0]["Image"])))
                                {
                                    image = hostingPath + "/" + Constants.NoImageAvaliablePath.Replace(@"\", "/");
                                }
                                else
                                {
                                    image = hostingPath + Constants.ProductCategoryTypeImagesPath.Replace(@"\", "/") + "/" + DBHelper.ParseString(drfind[0]["Image"]);
                                }
                                campaignDetailModel.CampaignImage = image;
                                styleCampaignListModel.CampaignModel= campaignDetailModel;
                            }
                        }
                    }
                    styleCampaignListModel.ProductLists = ProductList;
                    styleCampaignListModel.CategoryList = productCategoryList;
                    styleCampaignListModel.TotalProducts = DBHelper.ParseString(dtTotal.Rows.Count);
                    return styleCampaignListModel;
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
        /// Gets the campaign list.
        /// </summary>
        /// <returns></returns>
        public List<ProductCategoryType> GetCampaignList(string searchString, bool IsShowFrontend)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    return context.ProductCategoryType.Where(x => x.IsDelete == false && x.ProductCategoryFK == (long)ProductCategoryEnum.Campaign && (x.CategoryType.Contains(searchString) || x.Description.Contains(searchString))).ToList();
                }
                else if (IsShowFrontend)
                {
                    return context.ProductCategoryType.Where(x => x.IsDelete == false && x.ProductCategoryFK == (long)ProductCategoryEnum.Campaign && x.IsShowInFrontend == IsShowFrontend).ToList();
                }
                else
                {
                    return context.ProductCategoryType.Where(x => x.IsDelete == false && x.ProductCategoryFK == (long)ProductCategoryEnum.Campaign).ToList();
                }
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Gets the campaign product count.
        /// </summary>
        /// <param name="productCategoryTypeId">The product category type identifier.</param>
        /// <returns></returns>
        public List<ProductCategoryDetail> GetCampaignProductCount(List<long> productCategoryTypeId)
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
        /// Gets the campaign product list.
        /// </summary>
        /// <param name="pageNo">The page no.</param>
        /// <param name="rowsPerPage">The rows per page.</param>
        /// <param name="searchString">The search string.</param>
        /// <param name="hostingURL">The hosting URL.</param>
        /// <param name="column">The column.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="productCategoryTypeId">The product category type identifier.</param>
        /// <returns></returns>
        public OrderProductListModel GetCampaignProductList(int pageNo, int rowsPerPage, string searchString, string hostingURL, string column, string direction, long productCategoryTypeId)
        {
            OrderProductListModel orderProductListModel = new OrderProductListModel();
            List<OrderProductListDataModel> orderProductListDataModels = new List<OrderProductListDataModel>();
            try
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[6];
                    parameter[0] = new SqlParameter("PAGENO", pageNo);
                    parameter[1] = new SqlParameter("RECORDPERPAGE", rowsPerPage);
                    parameter[2] = new SqlParameter("SEARCHSTRING", searchString);
                    parameter[3] = new SqlParameter("COLUMN", column);
                    parameter[4] = new SqlParameter("DIRECTION", direction);
                    parameter[5] = new SqlParameter("PRODUCTCATEGORYTYPEID", productCategoryTypeId);
                    DataSet dataSet = DBHelper.GetDataTable("CAMPAIGNPRODUCTLIST", parameter, DBHelper.ParseString(settings.Value.AppDbContext));
                    if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0)
                    {
                        DataTable dtProducts = dataSet.Tables[0];
                        DataTable dtTotal = dataSet.Tables[1];
                        DataTable dtColor = dataSet.Tables[2];
                        DataTable dtImage = dataSet.Tables[3];
                        DataTable dtSizeGroup = dataSet.Tables[4];
                        DataTable dtProductSizeGroup = dataSet.Tables[5];
                        DataTable dtCategory = dataSet.Tables[6];
                        DataTable dtProductCategory = new DataTable();

                        if (dataSet.Tables.Count > 7)
                        {
                            dtProductCategory = dataSet.Tables[7];
                        }
                        if (dtProducts != null && dtProducts.Rows.Count > 0)
                        {
                            foreach (DataRow item in dtProducts.Rows)
                            {
                                OrderProductListDataModel orderProductListModelObject = new OrderProductListDataModel();

                                orderProductListModelObject.Price = DBHelper.ParseString(item["Price"]);
                                orderProductListModelObject.ProductId = DBHelper.ParseString(item["ProductId"]);
                                orderProductListModelObject.ProductName = DBHelper.ParseString(item["Name"]);
                                orderProductListModelObject.IsSelected = false;
                                if (dtProductCategory != null && dtProductCategory.Rows.Count > 0)
                                {
                                    DataRow[] drProductCategory = dtProductCategory.Select("ProductFK='" + DBHelper.ParseString(item["ProductId"]) + "'");
                                    if (drProductCategory != null && drProductCategory.Length > 0)
                                    {
                                        orderProductListModelObject.IsSelected = true;
                                    }
                                }

                                DataRow[] drColor = dtColor.Select("ProductFK='" + DBHelper.ParseString(item["ProductId"]) + "'");
                                if (drColor != null && drColor.Length > 0)
                                {
                                    foreach (var color in drColor)
                                    {
                                        orderProductListModelObject.ColorId += DBHelper.ParseString(color["ProductColorId"]) + ", ";
                                        orderProductListModelObject.Color += DBHelper.ParseString(color["Name"]) + ", ";
                                    }
                                    orderProductListModelObject.ColorId = orderProductListModelObject.ColorId.Trim().TrimEnd(',');
                                    orderProductListModelObject.Color = orderProductListModelObject.Color.Trim().TrimEnd(',');
                                }

                                DataRow[] drCategory = dtCategory.Select("ProductFK='" + DBHelper.ParseString(item["ProductId"]) + "'");
                                if (drCategory != null && drCategory.Length > 0)
                                {
                                    foreach (var category in drCategory)
                                    {
                                        orderProductListModelObject.Category += DBHelper.ParseString(category["CategoryType"]) + ", ";
                                    }
                                    orderProductListModelObject.Category = orderProductListModelObject.Category.Trim().TrimEnd(',');
                                }

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

                                DataRow[] drImage = dtImage.Select("ProductFK='" + item["ProductId"] + "'");
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
        /// Gets the campaign detail.
        /// </summary>
        /// <param name="campaignId">The campaign identifier.</param>
        /// <param name="hostingPath">The hosting path.</param>
        /// <returns></returns>
        public StyleCampaignDetailModel GetCampaignDetail(int campaignId, string hostingPath)
        {
            StyleCampaignDetailModel styleCampaignDetailModel = new StyleCampaignDetailModel();
            var camapignDetail = context.ProductCategoryType.Where(x => x.IsDelete == false && x.ProductCategoryFK == (long)ProductCategoryEnum.Campaign
            && (x.ProductCategoryTypeId == campaignId)).FirstOrDefault();
            if (camapignDetail != null)
            {
                styleCampaignDetailModel.CampaignName = camapignDetail.CategoryType;
                styleCampaignDetailModel.CampaignId = camapignDetail.ProductCategoryTypeId.ToString();
                string image = string.Empty;
                if (string.IsNullOrWhiteSpace(DBHelper.ParseString(camapignDetail.Image)))
                {
                    image = hostingPath + "/" + Constants.NoImageAvaliablePath.Replace(@"\", "/");
                }
                else
                {
                    image = hostingPath + Constants.ProductCategoryTypeImagesPath.Replace(@"\", "/") + "/" + DBHelper.ParseString(camapignDetail.Image);
                }
                styleCampaignDetailModel.CampaignImage = image;
            }

            return styleCampaignDetailModel;
        }
    }
    #endregion
}
