using System;
using System.Security.Cryptography;

namespace DatingCode.Security.Crypto
{
    public class RandomStringGenerator
    {
        private const string _possibleChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private readonly int _possibleCharsLen = _possibleChars.Length;
        private const int _tokenLen = 64;

        public string NewRandomString( int length)
        {
            if (length <= 0)
                throw new ArgumentException($"{length} <= 0", nameof(length));


            var randomBytes = new byte[length];
            using (var generator = new RNGCryptoServiceProvider())
            {
                generator.GetNonZeroBytes(randomBytes);
            }

            var resultChars = new char[length];
            for (int idx = 0; idx < resultChars.Length; idx++)
            {
                var currentRandomByte = randomBytes[idx];
                var nextCharPosition = currentRandomByte % _possibleCharsLen;
                resultChars[idx] = _possibleChars[nextCharPosition];
            }

            var result = new string(resultChars);
            return result;
        }
    }
}
