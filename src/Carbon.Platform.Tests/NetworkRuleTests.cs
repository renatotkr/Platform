using Carbon.Net;

using Xunit;

namespace Carbon.Platform.Networking.Tests
{
    public class NetworkRuleTests
    {
        [Fact]
        public void A()
        {
            var rule = new NetworkSecurityGroupRule(
                id        : 1, 
                direction : TrafficDirection.In,
                protocal  : NetworkProtocal.TCP 
            )
            {
                SourcePorts      = "49152-65535",
                Source           = "10.0.0.0/24",
                Destination      = "*",
                DestinationPorts = "*",
                Action           = FirewallRuleAction.Allow,
                Priority         = 10
            };
        }

    }
}
