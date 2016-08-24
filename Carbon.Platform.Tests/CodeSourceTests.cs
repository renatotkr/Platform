using Xunit;

namespace Carbon.Platform.Tests
{
    public class CodeSourceTests
    {
        [Fact]
        public void ParseTests()
        {
            var x = RepositoryDetails.Parse("npm/npm");

            Assert.Equal(RepositoryProviderId.GitHub, x.Provider);
            Assert.Equal("npm", x.AccountName);
            Assert.Equal("npm", x.Name);
        }

        [Fact]
        public void ParseTests2()
        {
            var x = RepositoryDetails.Parse("carbon/cropper#master");

            Assert.Equal(RepositoryProviderId.GitHub, x.Provider);
            Assert.Equal("carbon", x.AccountName);
            Assert.Equal("cropper", x.Name);
            Assert.Equal("master", x.Revision);

            Assert.Equal("carbon/cropper#master", x.ToString());
        }

        [Theory]
        [InlineData("git://github.com/carbon/cropper.git#branch")]
        [InlineData("git+https://github.com/carbon/cropper.git#branch")]
        [InlineData("git+ssh://github.com/carbon/cropper.git#branch")]
        [InlineData("https://github.com/carbon/cropper.git#branch")]
        [InlineData("http://github.com/carbon/cropper.git#branch")]
        [InlineData("github:carbon/cropper#branch")]
        [InlineData("GITHUB:carbon/cropper.git#branch")]
        [InlineData("carbon/cropper#branch")]
        [InlineData("carbon/cropper.git#branch")]
        public void ParseUrl(string url)
        {
            var x = RepositoryDetails.Parse(url);

            Assert.Equal(RepositoryProviderId.GitHub, x.Provider);
            Assert.Equal("carbon", x.AccountName);
            Assert.Equal("cropper", x.Name);
            Assert.Equal("branch", x.Revision);
            
        }

        [Theory]
        [InlineData("bitbucket:example/repo")]
        [InlineData("https://bitbucket.org/example/repo")]
        public void ParseBitbucket(string url)
        {
            var x = RepositoryDetails.Parse(url);

            Assert.Equal(RepositoryProviderId.BitBucket, x.Provider);
            Assert.Equal("example", x.AccountName);
            Assert.Equal("repo", x.Name);
            
        }
    }
}
