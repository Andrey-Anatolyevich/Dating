using DatingCode.Security.Crypto;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DatingTests.Code.Security
{
    public class CrypterTests
    {
        [Fact]
        public void CanEncryptDecryptPass()
        {
            var crypter = new Crypter();
            var randomStringGenerator = new RandomStringGenerator();

            var salt = randomStringGenerator.NewRandomString(128);
            var password = "duppa_paaawa";

            var cryptedPass = crypter.Encrypt(password, salt);
            var cryptedPass2 = crypter.Encrypt(password, salt);
            var decryptedPass = crypter.Decrypt(cryptedPass, salt);

            Assert.Equal(password, decryptedPass);
        }
    }
}
