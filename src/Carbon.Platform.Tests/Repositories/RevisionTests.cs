using Xunit;

namespace Carbon.VersionControl.Tests
{
    public class RepositoryTests
    {
        [Fact]
        public void ParseTests()
        {
            var x = RevisionSource.Parse("npm/npm");

            Assert.Equal("github", x.Provider.Name);
            Assert.Equal("npm", x.AccountName);
            Assert.Equal("npm", x.Name);

            Assert.Equal("npm/npm", x.ToString());
        }

        [Fact]
        public void ParseTests2()
        {
            var x = RevisionSource.Parse("carbon/cropper#master");

            Assert.Equal("github", x.Provider.Name);
            Assert.Equal("carbon", x.AccountName);
            Assert.Equal("cropper", x.Name);
            Assert.Equal("master", x.Revision.Value.Name);

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
            var x = RevisionSource.Parse(url);

            Assert.Equal("github", x.Provider.Name);
            Assert.Equal("carbon", x.AccountName);
            Assert.Equal("cropper", x.Name);
            Assert.Equal("branch", x.Revision.Value.Name);

        }

        [Theory]
        [InlineData("bitbucket:example/repo")]
        [InlineData("https://bitbucket.org/example/repo")]
        public void ParseBitbucket(string url)
        {
            var x = RevisionSource.Parse(url);

            Assert.Equal("bitbucket", x.Provider.Name);
            Assert.Equal("example", x.AccountName);
            Assert.Equal("repo", x.Name);
        }
    }
}
