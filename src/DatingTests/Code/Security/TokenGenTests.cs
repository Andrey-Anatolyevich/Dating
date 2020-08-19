using DatingCode.Security.Crypto;
using Xunit;

namespace DatingTests.Code.Security
{
    public class TokenGenTests
    {
        [Fact]
        public void CanGenerateToken()
        {
            var tokenGen = new RandomStringGenerator();
            var newToken = tokenGen.NewRandomString(60);

            Assert.NotEmpty(newToken);
        }
    }
}
