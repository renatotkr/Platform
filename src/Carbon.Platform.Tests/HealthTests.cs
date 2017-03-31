using System;

using Xunit;

using Carbon.Json;

namespace Carbon.Platform.Computing.Tests
{
    public class HealthCheckTEsts
    {
        [Fact]
        public void Serialize()
        {
            var check = new HealthCheck {
                Interval = TimeSpan.FromSeconds(10),
                Timeout = TimeSpan.FromSeconds(10),
                Path = "/",
                HealthyThreshold = new Threshold(1, 5),
                UnhealthyThreshold = new Threshold(3, 10)
            };


            var json = JsonObject.FromObject(check);

            var d = JsonObject.Parse(json).As<HealthCheck>();

            Assert.Equal("1/5", d.HealthyThreshold.ToString());
            Assert.Equal("3/10", d.UnhealthyThreshold.ToString());

        }
    }
}
