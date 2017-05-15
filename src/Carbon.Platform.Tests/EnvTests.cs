using Xunit;

namespace Carbon.Platform.Environments.Tests
{
    public class EnvTests
    {
        [Fact]
        public void TypeMapsAreCorrect()
        {
            Assert.Equal(EnvironmentType.Production, new EnvironmentInfo(1, "").Type);
            Assert.Equal(EnvironmentType.Production, new EnvironmentInfo(5, "").Type);

            Assert.Equal(EnvironmentType.Staging, new EnvironmentInfo(2, "").Type);
            Assert.Equal(EnvironmentType.Staging, new EnvironmentInfo(6, "").Type);

            Assert.Equal(EnvironmentType.Intergration, new EnvironmentInfo(3, "").Type);
            Assert.Equal(EnvironmentType.Intergration, new EnvironmentInfo(7, "").Type);

            Assert.Equal(EnvironmentType.Development, new EnvironmentInfo(4, "").Type);
            Assert.Equal(EnvironmentType.Development, new EnvironmentInfo(8, "").Type);
        }
    }
}
