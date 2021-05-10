namespace SHKang.API.Controllers
{
    #region namespaces

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using SHKang.Core.Constant;
    using SHKang.Core.Enums;
    using SHKang.Core.Helpers;
    using System;
    using System.IO;
    using System.Threading.Tasks;

    #endregion

    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {

        #region Private Variables

        /// <summary>
        /// The env
        /// </summary>
        private readonly IHostingEnvironment env;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FileController"/> class.
        /// </summary>
        /// <param name="_env">The env.</param>
        public FileController(IHostingEnvironment _env)
        {
            env = _env;
        }
        #endregion

        #region Public methods

        /// <summary>
        /// Gets the image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        [Route("get-image")]
        public async Task<IActionResult> GetImage(string image, string type)
        {

            try
            {
                if (!string.IsNullOrWhiteSpace(image) && !string.IsNullOrWhiteSpace(type))
                {
                    if (type.Equals(DBHelper.ParseString(FileType.Product.GetHashCode())))
                    {
                        var hostingPath = env.ContentRootPath;
                        string path = string.Concat(hostingPath, "/" + Constants.ProductImagesPath);
                        string fileName = image;
                        path = path + "\\" + fileName;
                        if (System.IO.File.Exists(path))
                        {
                            var dataBytes = System.IO.File.ReadAllBytes(path);
                            var dataStream = new MemoryStream(dataBytes);
                            return File(dataStream, "image/jpeg"); // returns a FileStreamResult
                        }
                    }
                }
                var noimage = env.ContentRootPath;
                string noimagepath = string.Concat(noimage, "/" + Constants.NoImageAvaliablePath);
                var noimagedataBytes = System.IO.File.ReadAllBytes(noimagepath);
                var noimagedataStream = new MemoryStream(noimagedataBytes);
                return File(noimagedataStream, "image/jpeg"); // returns a FileStreamResult
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex.Message + "  ::::  " + ex.StackTrace);
                return File(new byte[0], "");
            }
        } 
        #endregion
    }
}