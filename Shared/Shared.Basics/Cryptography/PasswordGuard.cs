using System;
using System.Security.Cryptography;

namespace WhiteRaven.Shared.Basics.Cryptography
{
    public class PasswordGuard : IPasswordGuard
    {
        private const int Iterations = 10000;

        public bool IsUserPasswordValid(string savedPasswordHash, string enteredPassword)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(savedPasswordHash))
            {
                throw new Exception("The password hash was blank");
            }

            if (string.IsNullOrWhiteSpace(enteredPassword))
            {
                throw new ArgumentException("The entered password was blank");
            }

            // Extract the bytes
            var hashBytes = Convert.FromBase64String(savedPasswordHash);

            // Get the salt
            var salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            // Compute the hash on the password the user entered
            var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, salt, Iterations);
            var hash = pbkdf2.GetBytes(20);

            // Compare the results
            for (var i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                {
                    return false;
                }
            }

            return true;
        }

        public string GeneratePasswordHash(string password)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Cannot generate hash for blank password");
            }

            // Create the salt value with a cryptographic PRNG
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            // Create the Rfc2898DeriveBytes and get the hash value
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations);
            var hash = pbkdf2.GetBytes(20);

            // Combine the salt and password bytes for later use
            var hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            // Turn the combined salt+hash into a string for storage
            return Convert.ToBase64String(hashBytes);
        }
    }
}