namespace Carbon.Platform.Tests
{
	using Xunit;
	
	public class BindingInfoTests
	{
		[Fact]
		public void ParseBindingInfoTest()
		{
			var bindingInfo = BindingInfo.Parse("*:80:");

			Assert.Equal("*", bindingInfo.IpAddress);
			Assert.Equal(80,  bindingInfo.Port);
			Assert.Equal("", bindingInfo.HostName);
		}
	}
}
