namespace SHKang.Core.ModelHelper
{
    #region namespaces
    using SHKang.Core.Helpers;
    using SHKang.Model.Models;
    using SHKang.Model.ViewModels.Admin;
    using System;
    using System.Collections.Generic;
    #endregion

    public static class ContentHelper
    {
        /// <summary>
        /// Converts to content details model.
        /// </summary>
        /// <param name="addContentModel">The model.</param>
        /// <returns></returns>
        public static ContentDetails BindContentDetailsModel(AddContentModel addContentModel)
        {
            ContentDetails contentDetails = new ContentDetails();
            if (addContentModel != null)
            {
                contentDetails.ContentId = addContentModel.ContentId;
                contentDetails.ShortName = addContentModel.ShortName;
                contentDetails.Name = addContentModel.Name;
                contentDetails.Description = addContentModel.Description;
                contentDetails.CreatedOn = DateTime.Now;
                contentDetails.CreatedBy = DBHelper.ParseInt64(addContentModel.CreatedBy);
                contentDetails.UpdatedBy = DBHelper.ParseInt64(addContentModel.UpdatedBy);
            }
            return contentDetails;
        }

        /// <summary>
        /// Converts to add content model.
        /// </summary>
        /// <param name="contentDetailsList">The model.</param>
        /// <returns></returns>
        public static List<AddContentModel> BindAddContentModel(List<ContentDetails> contentDetailsList)
        {
            List<AddContentModel> addContentModel = new List<AddContentModel>();
            if (contentDetailsList != null && contentDetailsList.Count > 0)
            {
                foreach (var item in contentDetailsList)
                {
                    addContentModel.Add(new AddContentModel
                    {
                        ContentId = item.ContentId,
                        ShortName = item.ShortName,
                        Name = item.Name,
                        Description = item.Description
                    });
                }
            }
            return addContentModel;
        }

        /// <summary>
        /// Converts to content details model.
        /// </summary>
        /// <param name="contentDetails">The model.</param>
        /// <returns></returns>
        public static AddContentModel BindContentDetailsModel(ContentDetails contentDetails)
        {
            AddContentModel addContentModel = new AddContentModel();
            if (contentDetails != null)
            {
                addContentModel.ContentId = contentDetails.ContentId;
                addContentModel.ShortName = contentDetails.ShortName;
                addContentModel.Name = contentDetails.Name;
                addContentModel.Description = contentDetails.Description;
            }
            return addContentModel;
        }
    }
}
