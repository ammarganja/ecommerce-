namespace SHKang.Core.Helpers
{
    #region namespaces
        using SHKang.Core.Enums;
        using SHKang.Model.Models;
    using SHKang.Model.ViewModels.Admin;
    using System.Collections.Generic;
        using System.Linq;
    #endregion

    /// <summary>
    /// The Sorting Helper
    /// </summary>
    public static class SortingHelper
    {
        #region Public Methods
        public static List<SizeListModel> GetSortedSizes(string sortColumn, string sortDirection, List<SizeListModel> sizeList)
        {
            List<SizeListModel> sizes = new List<SizeListModel>();
            if (sortColumn.ToLower().Equals(DBHelper.ParseString(SortSize.sizeid)) && sortDirection.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.asc)))
            {
                sizes = sizeList.OrderBy(x => x.SizeId).ToList();
            }
            else if (sortColumn.ToLower().Equals(DBHelper.ParseString(SortSize.sizeid)) && sortDirection.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.desc)))
            {
                sizes = sizeList.OrderByDescending(x => x.SizeId).ToList();
            }
            else if (sortColumn.ToLower().Equals(DBHelper.ParseString(SortSize.sizename)) && sortDirection.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.asc)))
            {
                sizes = sizeList.OrderBy(x => x.SizeName).ToList();
            }
            else if (sortColumn.ToLower().Equals(DBHelper.ParseString(SortSize.sizename)) && sortDirection.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.desc)))
            {
                sizes = sizeList.OrderByDescending(x => x.SizeName).ToList();
            }
            else if (sortColumn.ToLower().Equals(DBHelper.ParseString(SortSize.units)) && sortDirection.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.asc)))
            {
                sizes = sizeList.OrderBy(x => x.Units).ToList();
            }
            else if (sortColumn.ToLower().Equals(DBHelper.ParseString(SortSize.units)) && sortDirection.ToLower().Equals(DBHelper.ParseString(SortingDirectionType.desc)))
            {
                sizes = sizeList.OrderByDescending(x => x.Units).ToList();
            }
            else
            {
                sizes = sizeList.OrderBy(x => x.SizeId).ToList();
            }

            return sizes;
        }
        #endregion
    }
}
