using System;
using System.Net;
using Xunit;

namespace Carbon.Cloud.Logging.Tests
{
    public class ClientTests
    {

        [Fact]
        public void A()
        {
            var cid = Guid.Parse("6defff5c-bea7-4aba-8f8b-1e9555c35aab");
            var cip = IPAddress.Parse("192.168.1.1");
            var cua = "Carbon/1.0.0";

            var hash = ClientHash.Compute(cid, cip, cua);

            Assert.Equal("sc4sgOUkcMkWufvkQwNI+llNlww=", Convert.ToBase64String(hash));

            // var hash = ClientHash.Compute()
        }
    }
}
