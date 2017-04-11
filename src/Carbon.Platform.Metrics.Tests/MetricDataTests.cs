using System;

using Xunit;

namespace Carbon.Platform.Metrics.Tests
{
    public class MetricDataTests
    {
        [Fact]
        public void SerializeIsCorrect()
        {
            var date = new DateTime(2000, 01, 01, 05, 15, 32);

            var data = new MetricData(KnownMetrics.MemoryUsedBytes, new[] { new Dimension("hostId", "1") }, 1000, date);

            Assert.Equal("memory/usedBytes,hostId=1 value=1000 946743332000000", data.ToString());
        }
    }
}
