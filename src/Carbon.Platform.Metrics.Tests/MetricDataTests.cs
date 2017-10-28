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

            var data = new MetricData("compute", new[] { new Dimension("hostId", "1") }, null, 1000, TimestampHelper.Get(date));
            
            Assert.Equal("compute,hostId=1 value=1000 946732532000000", data.ToString());
        }

        [Fact]
        public void Parse()
        {
            var a = MetricData.Parse("requestCount,appId=1,appVersion=5.1.1 value=1000 1422568543702900257");


            Assert.Equal("requestCount", a.Name);

            Assert.Equal("appId",      a.Dimensions[0].Name);
            Assert.Equal("1",          a.Dimensions[0].Value);
            Assert.Equal("appVersion", a.Dimensions[1].Name);
            Assert.Equal("5.1.1",      a.Dimensions[1].Value);

            Assert.Equal(1000d, a.Value);
            Assert.Equal(1422568543702900257, a.Timestamp);
        }
    }
}
