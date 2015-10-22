namespace Carbon.Platform.Tests
{
	using System;

	using Xunit;

	public class FrontendTests
	{
		[Fact]
		public void GitUrlOK()
		{
			var url = new Uri("https://github.com/orgName/repoName.git");

			Assert.Equal("/orgName/repoName.git", url.AbsolutePath);
		}
	}
}
