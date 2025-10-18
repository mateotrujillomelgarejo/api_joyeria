using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace api_joyeria.Helpers
{
    public class PasswordHasher
    {
        // Longitud del salt en bytes
        private const int SaltSize = 16;
        // Longitud del hash en bytes
        private const int HashSize = 32;
        // Iterations (puedes ajustar)
        private const int Iterations = 10000;

        public static string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            byte[] hash = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: Iterations,
                numBytesRequested: HashSize);

            // concat salt + hash
            byte[] result = new byte[SaltSize + HashSize];
            Buffer.BlockCopy(salt, 0, result, 0, SaltSize);
            Buffer.BlockCopy(hash, 0, result, SaltSize, HashSize);

            return Convert.ToBase64String(result);
        }

        public static bool VerifyPassword(string password, string storedHash)
        {
            if (string.IsNullOrWhiteSpace(storedHash)) return false;

            byte[] bytes;
            try
            {
                bytes = Convert.FromBase64String(storedHash);
            }
            catch
            {
                return false;
            }

            if (bytes.Length != SaltSize + HashSize) return false;

            byte[] salt = new byte[SaltSize];
            Buffer.BlockCopy(bytes, 0, salt, 0, SaltSize);

            byte[] hash = new byte[HashSize];
            Buffer.BlockCopy(bytes, SaltSize, hash, 0, HashSize);

            byte[] newHash = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: Iterations,
                numBytesRequested: HashSize);

            return hash.SequenceEqual(newHash);
        }
    }
}
