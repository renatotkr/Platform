using Xunit;

namespace Carbon.Platform.Configuration.Tests
{
    public class ServiceExecutableTests
    {
        [Fact]
        public void ParseWithArguments()
        {
            var x = ProgramExecutable.Parse("Accelerator -port 5000");

            Assert.Equal("Accelerator", x.FileName);
            Assert.Equal("-port 5000", x.Arguments);
        }

        [Fact]
        public void ParseWithoutArguments()
        {
            var x = ProgramExecutable.Parse("Nginx");

            Assert.Equal("Nginx", x.FileName);
            Assert.Null(null);
        }
    }
}