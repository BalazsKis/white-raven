namespace WhiteRaven.Shared.Library.Cryptography
{
    /// <summary>
    /// Contains password hash creation and validation
    /// </summary>
    public interface IPasswordGuard
    {
        /// <summary>
        /// Determines whether a password is valid based on its hashed version
        /// </summary>
        /// <param name="savedPasswordHash">The saved password hash</param>
        /// <param name="enteredPassword">The entered password</param>
        /// <returns>
        /// True, if the password is valid
        /// </returns>
        bool IsUserPasswordValid(string savedPasswordHash, string enteredPassword);

        /// <summary>
        /// Generates the hash for the given password
        /// </summary>
        /// <param name="password">The password to generate hash for</param>
        /// <returns>The generated password hash</returns>
        string GeneratePasswordHash(string password);
    }
}