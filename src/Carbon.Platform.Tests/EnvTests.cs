using Xunit;

namespace Carbon.Platform.Environments.Tests
{
    using static EnvironmentType;

    public class EnvTests
    {
        [Theory]
        [InlineData(Production,   1)]
        [InlineData(Production,   5)]
        [InlineData(Staging,      2)]
        [InlineData(Staging,      6)]
        [InlineData(Intergration, 3)]
        [InlineData(Intergration, 7)]
        [InlineData(Development,  4)]
        [InlineData(Development,  8)]
        public void TypeMapsAreCorrect(EnvironmentType type, long id)
        {
            Assert.Equal(type, new EnvironmentInfo(id, "", 1).Type);
        }
    }
}