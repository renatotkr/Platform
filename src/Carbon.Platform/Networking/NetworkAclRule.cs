using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;

namespace Carbon.Platform.Networking
{
    [Dataset("NetworkAclRules")]
    public class NetworkAclRule
    {
        // networkId + index
        [Member("id"), Key]
        public long Id { get; set; }

        [Member("priority")]
        public int Priority { get; set; }

        [Member("condition")]
        public string Condition { get; set; }

        // e.g. allow, deny, drop, log, forward
        [Member("action")]
        public string Action { get; set; }

        public long NetworkId => ScopedId.GetScope(Id);

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; set; }

        [IgnoreDataMember]
        [Member("deleted")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Deleted { get; }
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