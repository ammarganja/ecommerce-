namespace SHKang.Core.Extension
{
    using SHKang.Core.Enums;
    using SHKang.Core.Helpers;

    /// <summary>
    /// The Extension Helper
    /// </summary>
    public static class ExtensionHelper
    {
        /// <summary>
        /// Determines whether [is admin role].
        /// </summary>
        /// <param name="roleType">Type of the role.</param>
        /// <returns>
        ///   <c>true</c> if [is admin role] [the specified role type]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAdminRole(this string roleType)
        {
            if (roleType == DBHelper.ParseString(RoleType.Admin.GetHashCode()))
            {
                return true;
            }
            return false;
        }
    }
}
