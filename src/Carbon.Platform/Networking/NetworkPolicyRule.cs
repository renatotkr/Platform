using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;

namespace Carbon.Platform.Networking
{
    [Dataset("NetworkPolicyRule")]
    public class NetworkPolicyRule : INetworkRule
    {
        // networkId + index
        [Member("id"), Key]
        public long Id { get; set; }

        [Member("direction")] // inbound | outbound
        public TrafficDirection Direction { get; set; }

        [Member("condition")]
        public string Condition { get; set; }

        [Member("action")]
        public string Action { get; set; }

        [Member("priority")]
        public int Priority { get; set; }

        public long NetworkId => ScopedId.GetScope(Id);

        #region Timestamps

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; set; }

        [IgnoreDataMember]
        [Member("deleted")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Deleted { get; }

        #endregion
    }
}


/* 
Rule #  Protocal	Source	          Port	    Action      Priority
100	    TCP         0.0.0.0/0	      3389	    Allow       1
101     UPC         0.0.0.0/9         1000      DENY        2
102     *                                                   3
*/
