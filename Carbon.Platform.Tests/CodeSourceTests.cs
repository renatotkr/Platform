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

        [Fact]
        public void ParseUrl()
        {
            foreach (var url in new[] {
                "git://github.com/carbon/cropper.git#branch",
                "git+https://github.com/carbon/cropper.git#branch",
                "git+ssh://github.com/carbon/cropper.git#branch",
                "https://github.com/carbon/cropper.git#branch",
                "http://github.com/carbon/cropper.git#branch",
                "github:carbon/cropper#branch",
                "GITHUB:carbon/cropper.git#branch",
                "carbon/cropper#branch",
                "carbon/cropper.git#branch"
            })
            {
                var x = RepositoryDetails.Parse(url);

                Assert.Equal(RepositoryProviderId.GitHub, x.Provider);
                Assert.Equal("carbon", x.AccountName);
                Assert.Equal("cropper", x.Name);
                Assert.Equal("branch", x.Revision);
            }
        }

        [Fact]
        public void ParseBitbucket()
        {
            foreach (var url in new[] {
                "bitbucket:example/repo",
                "https://bitbucket.org/example/repo"
            })
            {
                var x = RepositoryDetails.Parse(url);

                Assert.Equal(RepositoryProviderId.BitBucket, x.Provider);
                Assert.Equal("example", x.AccountName);
                Assert.Equal("repo", x.Name);
            }
        }
    }
}
