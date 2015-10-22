namespace Carbon.Metrics.Tests
{
	using Xunit;

    public class MetricTests
    {
		/*
		[Test]
		public void ReportGeneratorion()
		{
			var series = new [] { 1d, 2d, 3d, 4d, 5d };

			var report = Report.Generate(series);

			Assert.AreEqual(1d, report.Min);
			Assert.AreEqual(5d, report.Max);
			Assert.AreEqual(15d, report.Sum);
			Assert.AreEqual(3d, report.Average);
		}
		*/

		[Fact]
		public void BitShift()
		{
			long a = 1767720501574127620L;

			Assert.Equal(401933, a >> 42);
			Assert.Equal(100483, a >> 44);

			Assert.Equal(4194303, (-1L ^ (-1L << 22)));
			Assert.Equal(20484, a & (-1L ^ (-1L << 22)));

			Assert.Equal(5, 20484 >> 12);

			Assert.Equal(4095, (-1L ^ (-1L << 12)));

			Assert.Equal(4, a & (-1L ^ (-1L << 12)));

			Assert.Equal(431572388079621, (a >> 12));

			Assert.Equal(421457410234, (a >> 22)); // 13 years of milliseconds

		}
    }
}