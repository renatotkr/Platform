using Carbon.Data.Annotations;

namespace Carbon.Platform.Networking
{
    [Dataset("NetworkRules")]
    public class NetworkRule
    {
        [Member("id"), Identity]
        public long Id { get; set; }

        [Member("networkId")]
        public long NetworkId { get; set; }

        [Member("proxyId")]
        public long? ProxyId { get; set; }

        [Member("priority")]
        public int Priority { get; set; }

        [Member("backendId")]
        public long? BackendId { get; set; }

        [Member("condition")]
        public string Condition { get; set; }

        [Member("action")]
        public string Action { get; set; }
    }
    
    // protocal = "TPC" && port == 80 && source matches 0.0.0.0
    // action = DENY

    // path matches "/images/*" && remoteIp matches "10.0.0.0/8"
    // forward backend#100

    // TODO: Finalize condition & action syntax...
}

// Google Cloud Notes:

// By default, all incoming traffic from outside a network is blocked and no packet is allowed into an instance without an appropriate firewall rule.
// Firewall rules only regulate incoming traffic to an instance.
// Once a connection has been established with an instance, traffic is permitted in both directions over that connection. 
// All instances are configured with a "hidden" firewall rule that drops TCP connections after 10 minutes of inactivity. 