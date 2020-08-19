using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DatingCode.Security.Crypto
{
    public class Crypter
    {
        // This constant is used to determine the keysize of the encryption algorithm in bits. We
        // divide this by 8 within the code below to get the equivalent number of bytes.
        private const int _keysize = 128;
        private const int _blockSize = 128;

        // This constant determines the number of iterations for the password bytes generation function.
        private const int DerivationIterations = 1000;

        public string Encrypt(string data, string salt)
        {
            // Salt and IV is randomly generated each time, but is preprended to encrypted cipher
            // text so that the same Salt and IV values can be used when decrypting.
            var saltStringBytes = Generate128BitsOfRandomEntropy();
            var ivStringBytes = Generate128BitsOfRandomEntropy();
            using (var password = new Rfc2898DeriveBytes(salt, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(_keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = _blockSize;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;

                    byte[] cipherTextBytes = saltStringBytes;

                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                    using (MemoryStream memoryStream = new MemoryStream())
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        var plainTextBytes = Encoding.UTF8.GetBytes(data);
                        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                        cryptoStream.FlushFinalBlock();
                        // Create the final bytes as a concatenation of the random salt
                        // bytes, the random iv bytes and the cipher bytes.

                        cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                        cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                    }
                    var result = Convert.ToBase64String(cipherTextBytes);
                    return result;
                }
            }
        }

        public string Decrypt(string codedData, string salt)
        {
            // Get the complete stream of bytes that represent: [32 bytes of Salt] + [32 bytes of
            // IV] + [n bytes of CipherText]
            var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(codedData);
            // Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
            var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(_keysize / 8).ToArray();
            // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
            var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(_keysize / 8).Take(_keysize / 8).ToArray();
            // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
            var cipherTextBytes = cipherTextBytesWithSaltAndIv
                .Skip((_keysize / 8) * 2)
                .Take(cipherTextBytesWithSaltAndIv.Length - ((_keysize / 8) * 2))
                .ToArray();

            var resultPassword = string.Empty;
            using (var password = new Rfc2898DeriveBytes(salt, saltStringBytes, DerivationIterations))
            using (var symmetricKey = new RijndaelManaged())
            {
                symmetricKey.BlockSize = _blockSize;
                symmetricKey.Mode = CipherMode.CBC;
                symmetricKey.Padding = PaddingMode.PKCS7;

                var keyBytes = password.GetBytes(_keysize / 8);
                using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                using (var memoryStream = new MemoryStream(cipherTextBytes))
                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    var plainTextBytes = new byte[cipherTextBytes.Length];
                    var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                    resultPassword = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                }
            }
            return resultPassword;
        }

        private byte[] Generate128BitsOfRandomEntropy()
        {
            var randomBytes = new byte[16]; // 16 Bytes will give us 128 bits.
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }
    }
}