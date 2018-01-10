using Carbon.Platform.Security;
using Xunit;

namespace Carbon.Platform.Client.Tests
{
    public class AccessKeyCredentialTests
    {
        [Fact]
        public void A()
        {
            var credential = new AccessKeyCredential("kid", "secret", 1, "read");

            Assert.Equal("kid",     credential.AccessKeyId);
            Assert.Equal("secret",  credential.AccessKeySecret);
            Assert.Equal(1,         credential.AccountId);
            Assert.Equal("read",    credential.Scope);
        }
    }
}
