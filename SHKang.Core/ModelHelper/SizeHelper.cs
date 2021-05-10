namespace SHKang.Core.ModelHelper
{

    #region namespaces

    using SHKang.Core.Helpers;
    using SHKang.Model.Models;
    using SHKang.Model.ViewModels.Admin;
    using System.Collections.Generic;
    using System.Linq;

    #endregion

    public static class SizeHelper
    {

        /// <summary>
        /// Binds the size list model.
        /// </summary>
        /// <param name="sizeDetailModel">The size detail model.</param>
        /// <returns></returns>
        public static List<SizeListModel> BindSizeListModel(List<SizeDetail> sizeDetailModel)
        {
            List<SizeListModel> sizeListModel = new List<SizeListModel>();
            if (sizeDetailModel != null)
            {
                var sizeIds = sizeDetailModel.Select(x => x.Size.SizeId).Distinct().ToList();
                foreach (var item in sizeIds)
                {
                    var sizeResult = sizeDetailModel.Where(x => x.Size.SizeId == item).ToList();
                    if (sizeResult != null)
                    {
                        string units = string.Empty;
                        foreach (var size in sizeResult)
                        {
                            units += size.Name + "/";
                        }
                        units = units.TrimEnd('/');
                        sizeListModel.Add(new SizeListModel
                        {
                            SizeId = DBHelper.ParseString(sizeResult[0].Size.SizeId),
                            SizeName = DBHelper.ParseString(sizeResult[0].Size.Name),
                            Units = units

                        });
                    }
                }
            }
            return sizeListModel;
        }

        /// <summary>
        /// Gets the size ids.
        /// </summary>
        /// <param name="sizeListModel">The size list model.</param>
        /// <returns></returns>
        public static List<long> GetSizeIds(List<SizeListModel> sizeListModel)
        {
            List<long> sizeIdsList = new List<long>();
            if (sizeListModel != null)
            {
                foreach (var item in sizeListModel)
                {
                    sizeIdsList.Add(DBHelper.ParseInt64(item.SizeId));
                }
            }
            return sizeIdsList;
        }

        /// <summary>
        /// Binds the size list model.
        /// </summary>
        /// <param name="sizeDetailModel">The size detail model.</param>
        /// <param name="productList">The product list.</param>
        /// <returns></returns>
        public static List<SizeListModel> BindSizeListModel(List<SizeListModel> sizeDetailModel, List<Product> productList)
        {
            if (sizeDetailModel != null)
            {
                foreach (var item in sizeDetailModel
)
                {
                    var productCount = productList.Where(x => x.SizeFK == DBHelper.ParseInt64(item.SizeId)).Count();
                    item.TotalProducts = DBHelper.ParseString(productCount);
                }
            }
            return sizeDetailModel;
        }

        /// <summary>
        /// Binds the size model.
        /// </summary>
        /// <param name="addSizeModel">The add size model.</param>
        /// <returns></returns>
        public static Size BindSizeModel(AddSizeModel addSizeModel)
        {
            Size sizeModel = new Size();
            if (addSizeModel != null)
            {
                sizeModel.Name = addSizeModel.Name;
                if (!string.IsNullOrWhiteSpace(addSizeModel.SizeId) && DBHelper.ParseInt64(addSizeModel.SizeId) > 0)
                {
                    sizeModel.SizeId = DBHelper.ParseInt64(addSizeModel.SizeId);
                }
            }
            return sizeModel;
        }

        /// <summary>
        /// Binds the size detail model.
        /// </summary>
        /// <param name="addSizeModel">The add size model.</param>
        /// <returns></returns>
        public static List<SizeDetail> BindSizeDetailModel(AddSizeModel addSizeModel)
        {
            List<SizeDetail> sizeDetialModel = new List<SizeDetail>();
            if (addSizeModel != null)
            {
                string[] splittedUnits = addSizeModel.Units.Split(',');
                if (splittedUnits != null && splittedUnits.Length > 0)
                {
                    foreach (var item in splittedUnits)
                    {
                        if (!string.IsNullOrWhiteSpace(item))
                        {
                            sizeDetialModel.Add(new SizeDetail
                            {
                                Name = item
                            });
                        }
                    }
                }
            }
            return sizeDetialModel;
        }
    }
}
