using System;
using System.Security.Cryptography;
using System.Text;

namespace DatingCode.Security.Crypto
{
    public class Sha256SaltedHasher
    {
        public string GetHashBytesString(string value, string salt)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("String is NULL / Empty / Whitespace.", nameof(value));
            if (string.IsNullOrWhiteSpace(salt))
                throw new ArgumentException("String is NULL / Empty / Whitespace.", nameof(salt));


            var saltedValue = value + salt;
            var saltedValueBytes = Encoding.UTF8.GetBytes(saltedValue);
            byte[] hashBytes = null;
            using (var theSha = new SHA256CryptoServiceProvider())
            {
                hashBytes = theSha.ComputeHash(saltedValueBytes);
            }

            var resultBuilder = new StringBuilder(hashBytes.Length);
            foreach (var bt in hashBytes)
            {
                resultBuilder.Append(bt.ToString());
            }
            var result = resultBuilder.ToString();
            return result;
        }
    }
}
