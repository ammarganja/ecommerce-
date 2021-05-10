using SHKang.Core.Helpers;
using SHKang.Model.Models;
using SHKang.Model.ViewModels.Admin;
using System.Collections.Generic;
using System.Linq;

namespace SHKang.Core.ModelHelper
{
    public static class ColorHelper
    {

        /// <summary>
        /// Gets the color identifier.
        /// </summary>
        /// <param name="colorList">The color list.</param>
        /// <returns></returns>
        public static List<long> GetColorId(List<Color> colorList)
        {
            List<long> colorIds = new List<long>();
            if (colorList != null)
            {
                foreach (var item in colorList)
                {
                    colorIds.Add(item.ColorId);
                }
            }
            return colorIds;
        }


        /// <summary>
        /// Binds the color list model.
        /// </summary>
        /// <param name="colorList">The color list.</param>
        /// <param name="productColorList">The product color list.</param>
        /// <returns></returns>
        public static List<ColorListDataModel> BindColorListModel(List<Color> colorList,List<ProductColor> productColorList)
        {
            List<ColorListDataModel> colorListModel = new List<ColorListDataModel>();
            if (colorList != null)
            {
                foreach (var item in colorList)
                {
                    var totalProducts = productColorList.Where(x => x.ColorFK == item.ColorId).Count();
                    colorListModel.Add(new ColorListDataModel
                    {
                        ColorId=DBHelper.ParseString(item.ColorId),
                        Name=item.Name,
                        TotalProducts = DBHelper.ParseString(totalProducts),
                    });
                }
            }
            return colorListModel;
        }

        /// <summary>
        /// Binds the color model.
        /// </summary>
        /// <param name="addColorModel">The add color model.</param>
        /// <returns></returns>
        public static Color BindColorModel(AddColorModel addColorModel)
        {
            Color colorModel = new Color();
            colorModel.ColorId =DBHelper.ParseInt64(addColorModel.ColorId);
            colorModel.Name = addColorModel.Name;
            return colorModel;
        }

    }
}
