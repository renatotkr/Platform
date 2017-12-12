using Carbon.Platform.Computing;
using Xunit;

namespace Carbon.Platform.Management.Tests
{
    public class InstanceTypeTests
    {
        [Fact]
        public void A()
        {
            Assert.Equal(4362403853, AwsInstanceTypes.M5Large.Id);
            Assert.Equal("m5.large", AwsInstanceType.Get(4362403853).Name);
        }
    }
}
