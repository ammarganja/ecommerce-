namespace SHKang.API.Helper
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using SHKang.Core.Enums;
    using SHKang.Core.Extension;
    using SHKang.Core.Helpers;
    using System;
    using System.Security.Claims;

    public class AuthorizeHelper : TypeFilterAttribute
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizeHelper"/> class.
        /// </summary>
        public AuthorizeHelper() : base(typeof(AuthorizeFilter))
        {
        } 
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <seealso cref="Microsoft.AspNetCore.Mvc.Filters.IAuthorizationFilter" />
        public class AuthorizeFilter : IAuthorizationFilter
        {
            /// <summary>
            /// Called early in the filter pipeline to confirm request is authorized.
            /// </summary>
            /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext" />.</param>
            public void OnAuthorization(AuthorizationFilterContext context)
            {
                var IsAuthenticated = context.HttpContext.User.Identity.IsAuthenticated;
                var claimsIndentity = context.HttpContext.User.Identity as ClaimsIdentity;
                var requestPath = DBHelper.ParseString(context.HttpContext.Request.Path);

                if (IsAuthenticated)
                {
                    string roleType = DBHelper.ParseString(claimsIndentity.FindFirst("RoleType").Value);
                    string[] Splitted = requestPath.Split('/');
                    if (Splitted != null && Splitted.Length > 0)
                    {
                        if (roleType.IsAdminRole())
                        {
                            if (Array.IndexOf(Splitted, DBHelper.ParseString(RoleType.Admin).ToLower()) < 0)
                            {
                                context.Result = new UnauthorizedResult();
                            }
                        }
                        if (!roleType.IsAdminRole())
                        {
                            if (Array.IndexOf(Splitted, DBHelper.ParseString(RoleType.Admin).ToLower()) >= 0)
                            {
                                context.Result = new UnauthorizedResult();
                            }
                        }
                    }
                }
                else
                {
                    context.Result = new UnauthorizedResult();
                }
                return;
            }
        }
    }
}
