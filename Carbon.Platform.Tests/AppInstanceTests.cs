namespace Carbon.Infrastructure
{
	using Carbon.Data;
	using Carbon.Platform;
	using System;

	using Xunit;


	public class AppInstanceTests
	{
		[Fact]
		public void X()
		{
			var hex = HexString.ToBytes("1a0000000a0000002a000000");

			var int1 = BitConverter.ToInt32(hex, 0);
			var int2 = BitConverter.ToInt32(hex, 4);

			Assert.Equal(26, int1);
			Assert.Equal(10, int2);
		}

		[Fact]
		public void AppInstanceKey()
		{
			var instance = new AppInstance {
				AppId		= 3,
				AppVersion	= 35,
				MachineId	= 45
			};

			Assert.Equal("23000000030000002d000000", instance.GetKey());

			var i2 = AppInstance.FromKey("23000000030000002d000000");

			Assert.Equal(3,	i2.AppId);
			Assert.Equal(35, i2.AppVersion);
			Assert.Equal(45, i2.MachineId);
		}

	}
}