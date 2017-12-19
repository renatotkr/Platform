using System;
using Carbon.Time;
using Xunit;

namespace Carbon.Platform.Metrics.Tests
{
    public class MetricDataTests
    {
        [Fact]
        public void SerializeIsCorrect()
        {
            var timestamp = new Timestamp(new DateTime(2000, 01, 01, 05, 15, 32));

            var data = new MetricData(
                name       : "compute",
                dimensions : new[] { new Dimension("hostId", "1") }, 
                value      : 1000,
                timestamp  : timestamp
            );
            
            Assert.Equal("compute,hostId=1 value=1000 946703732000000000", data.ToString());
        }

        [Fact]
        public void SerializeWithoutDimensionsIsCorrect()
        {
            var timestamp = new Timestamp(new DateTime(2000, 01, 01, 05, 15, 32)).Value;

            var data = new MetricData("compute", null, 1000, timestamp);

            Assert.Equal("compute value=1000 946703732000000000", data.ToString());
        }

        [Fact]
        public void ParseWithoutDimensionsIsCorrect()
        {
            var timestamp = new Timestamp(new DateTime(2000, 01, 01, 05, 15, 32)).Value;

            var data = MetricData.Parse("compute value=1000 946703732000000000");

            Assert.Equal("value", data.Properties[0].Name);
            Assert.Equal(1000d, data.Properties[0].Value);

            Assert.Equal(timestamp, data.Timestamp.Value);
        }

        [Fact]
        public void SerializeMultiplePropertiesIsCorrect()
        {
            var timestamp = new Timestamp(new DateTime(2000, 01, 01, 05, 15, 32)).Value;

            var data = new MetricData(
                name       : "compute",
                dimensions : new[] { new Dimension("hostId", "1"), new Dimension("locationId", "10") },
                properties : new[] { new MetricDataProperty("count", 1), new MetricDataProperty("value", 100d) },
                timestamp  : timestamp
            );

            Assert.Equal("compute,hostId=1,locationId=10 count=1i,value=100 946703732000000000", data.ToString());

            var data2 = MetricData.Parse(data.ToString());

            Assert.Equal("compute,hostId=1,locationId=10 count=1i,value=100 946703732000000000", data2.ToString());
        }

        [Fact]
        public void SerializeMultipleProperties2IsCorrect()
        {
            var data = new MetricData(
                name       : "ƒ/decode",
                dimensions : null,
                properties : new[] {
                    new MetricDataProperty("count",  4565234),
                    new MetricDataProperty("pixels", 100),
                    new MetricDataProperty("time",   new TimeSpan((long)(54.4522d * TimeSpan.TicksPerSecond)))
                },
                timestamp  : null
            );

            Assert.Equal("ƒ/decode count=4565234i,pixels=100i,time=54.4522", data.ToString());

            var data2 = MetricData.Parse(data.ToString());

            Assert.Equal("ƒ/decode count=4565234i,pixels=100i,time=54.4522", data2.ToString());
        }


        [Fact]
        public void NameCanIncludeTags()
        {
            var data = new MetricData("memory,hostId=1", null, 1000, null);

            Assert.Equal("memory,hostId=1 value=1000", data.ToString());
        }

        [Fact]
        public void SerializeWithoutTimestamp()
        {
            var data = new MetricData("memory", new[] { new Dimension("hostId", "1") }, 1000, null);

            Assert.Equal("memory,hostId=1 value=1000", data.ToString());
        }

        [Fact]
        public void Parse()
        {
            var a = MetricData.Parse("requestCount,appId=1,appVersion=5.1.1 value=1000 0");
            
            Assert.Equal("requestCount", a.Name);

            Assert.Equal("appId",      a.Dimensions[0].Name);
            Assert.Equal("1",          a.Dimensions[0].Value);
            Assert.Equal("appVersion", a.Dimensions[1].Name);
            Assert.Equal("5.1.1",      a.Dimensions[1].Value);

            Assert.Equal(1000d, a.Properties[0].Value);
            Assert.Equal(0, a.Timestamp);
        }

        [Fact]
        public void ParseWithoutTimestamp()
        {
            var a = MetricData.Parse("requestCount,appId=1 value=1000");

            Assert.Equal("requestCount", a.Name);

            Assert.Equal("appId", a.Dimensions[0].Name);
            Assert.Equal("1", a.Dimensions[0].Value);

            Assert.Equal(1000d, a.Properties[0].Value);

            Assert.Null(a.Timestamp);
        }
    }
}
