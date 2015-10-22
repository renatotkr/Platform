namespace Carbon.Infrastructure
{
	using System;

	using Carbon.Data;
	using Carbon.Platform;
	using Xunit;

	
	public class TestIdTests
	{
		[Fact]
		public void TagIdTests()
		{
			var id_1		= AppTagId.Create(1, 1);
			var id2_1		= AppTagId.Create(2, 1);
			var id2_2		= AppTagId.Create(2, 2);
			var id2_1000	= AppTagId.Create(2, 1000);
			var id3_15		= AppTagId.Create(3, 15);

			Assert.Equal(4294967297,		id_1.Value);
			Assert.Equal(8589934593,		id2_1.Value);
			Assert.Equal(8589934594,		id2_2.Value);
			Assert.Equal(8589935592,		id2_1000.Value);
			Assert.Equal(12884901903,	id3_15.Value);

			var tag = AppTagId.Create(12884901903);

			Assert.Equal(3,	tag.AppId);
			Assert.Equal(15, tag.Version);

			Assert.True(id2_1.Value > id_1.Value);
			Assert.True(id3_15.Value > id_1.Value);
		}

		[Fact]
		public void AppInsatnceTests()
		{
			// Assert.Equal("9a9971410300000005000000", new AppInstance { TagId = 13982865818, MachineId = 5 }.GetKey());
			// Assert.Equal("9a9971410300000011000000", new AppInstance { TagId = 13982865818, MachineId = 17 }.GetKey());

			Assert.Equal(17, BitConverter.ToInt32(HexString.ToBytes("9a9971410300000011000000"), 8));
			Assert.Equal(13982865818, BitConverter.ToInt64(HexString.ToBytes("9a9971410300000011000000"), 0));

		}

	}
}