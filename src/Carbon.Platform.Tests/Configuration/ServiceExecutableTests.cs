using Xunit;

namespace Carbon.Platform.Configuration.Tests
{
    public class ServiceExecutableTests
    {
        [Fact]
        public void ParseWithArguments()
        {
            var x = ServiceExecutable.Parse("Accelerator -port 5000");

            Assert.Equal("Accelerator", x.Name);
            Assert.Equal("-port 5000", x.Arguments);
        }

        [Fact]
        public void ParseWithoutArguments()
        {
            var x = ServiceExecutable.Parse("Nginx");

            Assert.Equal("Nginx", x.Name);
            Assert.Null(null);
        }
    }
}