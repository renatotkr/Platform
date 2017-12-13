using System;
using System.Linq;
using System.Text;
using Xunit;

namespace Carbon.Platform.Metrics.Tests
{
    public class AggregateTests
    {
        [Fact]
        public void A()
        {
            var point = MetricData.Parse("transfer,accountId=1,country=AU,type=egress value=100 1422568543702900257");

            Assert.Equal(3, point.Dimensions.Length);

            var a = Aggregates.GetSeriesPermutations(point).ToArray();

            Assert.Equal(@"
transfer
transfer,accountId=1
transfer,accountId=1,country=AU
transfer,accountId=1,country=AU,type=egress
transfer,country=AU
transfer,country=AU,type=egress
transfer,type=egress".Trim(), string.Join(Environment.NewLine, a));
        }


        public static string GetA(in MetricData data)
        {
            var sb = new StringBuilder();

            sb.Append(data.Name);

            if (data.Dimensions != null)
            {
                foreach (var dimension in data.Dimensions)
                {
                    sb.Append(',');

                    sb.Append(dimension.Name);
                    sb.Append('=');
                    sb.Append(dimension.Value);
                }
            }

            return sb.ToString();
        }

        [Fact]
        public void B()
        {
            var point = MetricData.Parse("bandwidth,accountId=1 value=1 1");

            var a = Aggregates.GetSeriesPermutations(point).ToArray();

            Assert.Equal(@"
bandwidth
bandwidth,accountId=1".Trim(), string.Join(Environment.NewLine, a));
        }

        [Fact]
        public void C()
        {
            var point = MetricData.Parse("compute,accountId=1,country=AU,hostId=18,type=egress value=100 1");

            var a = Aggregates.GetSeriesPermutations(point).ToArray();

            // 20 bytes vs 8 bytes

            Assert.Equal(@"
compute
compute,accountId=1
compute,accountId=1,country=AU
compute,accountId=1,country=AU,hostId=18
compute,accountId=1,country=AU,hostId=18,type=egress
compute,country=AU
compute,country=AU,hostId=18
compute,country=AU,hostId=18,type=egress
compute,hostId=18
compute,hostId=18,type=egress
compute,type=egress".Trim(), string.Join(Environment.NewLine, a));

        }
    }
}
