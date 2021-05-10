namespace SHKang.Repository.Interface
{
    public interface IForgotPassword
    {
        /// <summary>
        /// Determines whether [is email exist] [the specified email identifier].
        /// </summary>
        /// <param name="emailId">The email identifier.</param>
        /// <param name="IsAdmin">The is admin.</param>
        /// <returns>
        ///   <c>true</c> if [is email exist] [the specified email identifier]; otherwise, <c>false</c>.
        /// </returns>
        bool IsEmailExist(string emailId, string IsAdmin);

        /// <summary>
        /// Updates the forgot password string.
        /// </summary>
        /// <param name="resetCode">The reset code.</param>
        /// <param name="emailId">The email identifier.</param>
        /// <returns></returns>
        int UpdateForgotPasswordString(string resetCode, string emailId);

        /// <summary>
        /// Determines whether [is code correct] [the specified code].
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        bool IsCodeCorrect(string code);

        /// <summary>
        /// Resets the password with code.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        int ResetPasswordWithCode(string password, string code);

        /// <summary>
        /// Resets the code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        int ResetCode(string code);

    }
}
