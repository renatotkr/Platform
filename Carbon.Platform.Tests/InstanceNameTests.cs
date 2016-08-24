using Xunit;


namespace Carbon.Platform.Tests
{
    public class InstanceNameTests
    {
        [Fact]
        public void DescriptionToInstanceNameTests()
        {
            Assert.Equal("abc", InstanceName.FromDescription("abc"));
            Assert.Equal("abc_123", InstanceName.FromDescription("abc/123"));
        }
    }
}