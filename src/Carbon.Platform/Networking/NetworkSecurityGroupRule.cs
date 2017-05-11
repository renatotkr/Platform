/*
using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Net;
using Carbon.Platform.Sequences;

namespace Carbon.Platform.Networking
{
    [Dataset("NetworkSecurityGroupRule")]
    public class NetworkSecurityGroupRule : IFirewallRule
    {
        public NetworkSecurityGroupRule() { }

        public NetworkSecurityGroupRule(
            long id,
            TrafficDirection direction,
            NetworkProtocal protocal)
        {
            Id = id;
            Direction = direction;
            Protocal = protocal;
        }

        // networkSecurityGroupId + index
        [Member("id"), Key]
        public long Id { get; }

        [Member("name")]
        public string Name { get; set; }

        [Member("direction")]
        public TrafficDirection Direction { get; }

        [Member("protocal")]
        public NetworkProtocal Protocal { get; }
        
        [Member("source")]
        [StringLength(63)]
        public string Source { get; set; }         // e.g. 0.0.0.0/0	
        
        [Member("sourcePorts")]                    // e.g. 80, 8000-9000
        [StringLength(50)]
        public string SourcePorts { get; set; }  

        [Member("destination")]
        [StringLength(63)]
        public string Destination { get; set; } // e.g. *

        [Member("destinationPorts")]
        public string DestinationPorts { get; set; }

        [Member("action")]
        public FirewallRuleAction Action { get; set; }

        [Member("priority")]
        public int Priority { get; set; }

        public long NetworkSecurityGroupId => ScopedId.GetScope(Id);

        #region Timestamps

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Deleted { get; }

        #endregion
    }
}
*/