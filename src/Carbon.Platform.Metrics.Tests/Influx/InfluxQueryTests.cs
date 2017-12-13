using Xunit;

namespace Influx.Tests
{
    public class InfluxQueryTests
    {
        [Fact]
        public void A()
        {
            var q = new InfluxQuery(
                database : "carbon",
                command  : "select sum(value) from transfer where accountId = '10000' group by time(1m)",
                pretty   : true
            );

            Assert.Equal("?db=carbon&q=select%20sum(value)%20from%20transfer%20where%20accountId%20%3D%20%2710000%27%20group%20by%20time(1m)&pretty=true", q.ToQueryString());
        }
    }
}
