using DatingCode.Security.Crypto;
using Xunit;

namespace DatingTests.Code.Security
{
    public  class Sha256SaltedHasherTests
    {
        [Fact]
        public void CanHashValues()
        {
            var pass1 = "FeKI*9IZlcQ2Bk$;EEUx";
            var salt1 = "7wEPvh7KFgIX3B1vo1McOPYhkVLUQipLnwZuHEL2l5hAwfGVDjHPTFCtuNVqh33qFfrdboxqidBTKZWtlaJuTSIrS6vvVz7fRktdracm0Q1WBDFNuRnwStmgbYv7bZcV";

            var saltedHasher = new Sha256SaltedHasher();
            var hash1 = saltedHasher.GetHashBytesString(pass1, salt1);
            var hash2 = saltedHasher.GetHashBytesString(pass1, salt1);

            Assert.NotEmpty(hash1);
            Assert.Equal(hash1, hash2);
        }
    }
}
