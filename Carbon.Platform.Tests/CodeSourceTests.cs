namespace Carbon.Platform.Tests
{
	using Xunit;
	
	public class CodeSourceTests
	{
		[Fact]
		public void ParseTests()
		{
            var x = CodeSource.Parse("npm/npm");

            Assert.Equal(CodeHost.GitHub, x.Host);
            Assert.Equal("npm", x.AccountName);
            Assert.Equal("npm", x.RepositoryName);
        }

        [Fact]
        public void ParseTests2()
        {
            var x = CodeSource.Parse("carbon/cropper#master");

            Assert.Equal(CodeHost.GitHub, x.Host);
            Assert.Equal("carbon", x.AccountName);
            Assert.Equal("cropper", x.RepositoryName);
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
                var x = CodeSource.Parse(url);

                Assert.Equal(CodeHost.GitHub, x.Host);
                Assert.Equal("carbon", x.AccountName);
                Assert.Equal("cropper", x.RepositoryName);
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
                var x = CodeSource.Parse(url);

                Assert.Equal(CodeHost.BitBucket, x.Host);
                Assert.Equal("example", x.AccountName);
                Assert.Equal("repo", x.RepositoryName);
            }
        }

    }
}
