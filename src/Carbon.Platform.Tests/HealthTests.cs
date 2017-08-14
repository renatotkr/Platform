using System;

using Xunit;

namespace Carbon.Platform.Computing.Tests
{
    public class HealthCheckTEsts
    {
        [Fact]
        public void Serialize()
        {
            var check = new HealthCheck(
                id       : 1,
                host     : null,
                path     : "/",
                port     : 80,
                ownerId  : 1,
                protocal : Net.NetworkProtocal.TCP,
                resource : default
            ) {
                Interval           = TimeSpan.FromSeconds(10),
                Timeout            = TimeSpan.FromSeconds(10),
                HealthyThreshold   = 5,
                UnhealthyThreshold = 10
            };


            /*
            var json = JsonObject.FromObject(check);

            var d = JsonObject.Parse(json).As<HealthCheck>();

            Assert.Equal("5", d.HealthyThreshold.ToString());
            Assert.Equal("10", d.UnhealthyThreshold.ToString());
            */

        }
    }
}
