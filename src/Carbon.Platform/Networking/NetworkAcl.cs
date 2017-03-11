using System;

using Carbon.Data.Annotations;

namespace Carbon.Platform.Networking
{
    [Dataset("NetworkAcls")]
    [DataIndex(IndexFlags.Unique, "providerId", "name")]
    public class NetworkAcl
    {
        [Member("id"), Identity]
        public long Id { get; set; }
        
        [Member("providerId")]
        public int ProviderId { get; set; }

        [Member("name")]
        [Ascii, StringLength(50)]
        public string Name { get; set; }

        [Member("description")]
        public string Description { get; set; }

        [Member("networkId")]
        [Indexed]
        public long NetworkId { get; set; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; set; }
    }
}

// name = sg-b27400d7


// Inbound
// Outbound
/*
Rule #  Protocal	Source	Port	  Permit/Deny
100	    TCP         0.0.0.0/0	    3389	    Allow
101     *           
*/
