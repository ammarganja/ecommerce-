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
    using System.Text;
    #endregion

    public class ContentRepository : IContent
    {
        #region Private Variables

        /// <summary>
        /// The context
        /// </summary>
        private AppDbContext context;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentRepository"/> class.
        /// </summary>
        /// <param name="_context">The context.</param>
        public ContentRepository(AppDbContext _context)
        {
            context = _context;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Adds the content.
        /// </summary>
        /// <param name="contentDetailsModel">The model.</param>
        /// <returns></returns>
        public long AddContent(ContentDetails contentDetailsModel)
        {
            try
            {
                var contentDetails = context.ContentDetails.Where(x => (x.Name == contentDetailsModel.Name || x.ShortName == x.ShortName) && x.IsDelete == false).FirstOrDefault();
                if (contentDetails == null)
                {
                    context.ContentDetails.Add(contentDetailsModel);
                    context.SaveChanges();
                    return contentDetailsModel.ContentId;
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
        /// Contents the details.
        /// </summary>
        /// <returns></returns>
        public List<ContentDetails> ContentDetails(string searchString)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    return context.ContentDetails.Where(x => x.IsDelete == false && (x.Name.Contains(searchString) || x.ShortName.Contains(searchString))).ToList();
                }
                else
                {
                    return context.ContentDetails.Where(x => x.IsDelete == false).ToList();
                }
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  :::::  " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Deletes the content.
        /// </summary>
        /// <param name="ContentDetailId">The content detail identifier.</param>
        /// <returns></returns>
        public bool DeleteContent(long contentDetailId, long updatedBy)
        {
            try
            {
                var contentDetails = context.ContentDetails.Where(x => x.ContentId == contentDetailId).FirstOrDefault();
                if (contentDetails != null)
                {
                    contentDetails.IsDelete = true;
                    contentDetails.UpdatedBy = updatedBy;
                    contentDetails.ModifiedOn = DateTime.Now;
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
        /// Updates the content.
        /// </summary>
        /// <param name="contentModel">The model.</param>
        /// <returns></returns>
        public long UpdateContent(ContentDetails contentModel)
        {
            try
            {
                var contentDetails = context.ContentDetails.Where(x => (x.Name == contentModel.Name || x.ShortName == contentModel.ShortName) && x.IsDelete == false && x.ContentId != contentModel.ContentId).FirstOrDefault();
                if (contentDetails == null)
                {
                    var contentDetailsModel = context.ContentDetails.Where(x => x.ContentId == contentModel.ContentId && x.IsDelete == false).FirstOrDefault();
                    if (contentDetailsModel != null)
                    {
                        contentDetailsModel.ShortName = contentModel.ShortName;
                        contentDetailsModel.Name = contentModel.Name;
                        contentDetailsModel.Description = contentModel.Description;
                        contentDetailsModel.ModifiedOn = DateTime.Now;
                        contentDetailsModel.UpdatedBy = contentModel.UpdatedBy;
                        context.SaveChanges();
                        return contentModel.ContentId;
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
                return 0;
            }
        }

        /// <summary>
        /// Gets the content detail.
        /// </summary>
        /// <param name="ShortName"></param>
        /// <returns></returns>
        public ContentDetails GetContentDetail(string shortName)
        {
            try
            {
                return context.ContentDetails.Where(x => x.ShortName.ToLower().Contains(shortName.ToLower())).FirstOrDefault();
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
