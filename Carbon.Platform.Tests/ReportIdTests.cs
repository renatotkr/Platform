namespace Carbon.Platform.Tests
{
	using Xunit;

	using System;
	
	public class ReportIdTests
	{
		[Fact]
		public void ReportIdTests1()
		{
			var reportId = ReportIdentity.Create(new DateTime(2010, 01, 01), TimeSpan.FromSeconds(30));

			Assert.Equal(5421554397609984030, reportId.Value);

			// Assert.Equal(new DateTime(2010, 01, 01), reportId.ToDateRange().Start);
			// Assert.Equal(30, reportId.ToDateRange().TimeSpan.TotalSeconds);

			reportId = ReportIdentity.Create(new DateTime(2010, 01, 01), TimeSpan.FromSeconds(60));

			Assert.Equal(5421554397609984060, reportId.Value);
		}
	}
}