namespace Carbon.Libraries.Tests
{
    using Carbon.Platform;

	using Xunit;
	
	public class LibraryTests
    {
		[Fact]
		public void FormattingTests()
		{
            var lib = new Library {
                Name = "cropper",
                Version = new Semver(1, 2, 3)
           };

           Assert.Equal("cropper@1.2.3", lib.ToString());
        }


        [Fact]
        public void DepTests()
        {
            var lib = new PackageDependency("a", "1.0.0");

            Assert.False(lib.IsFile);

            lib = new PackageDependency("b", "./hi.jpeg");

            Assert.True(lib.IsFile);

        }
    }
}
