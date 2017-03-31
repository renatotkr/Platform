using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;

namespace Carbon.Platform.Networking
{
    [Dataset("NetworkAcls")]
    [DataIndex(IndexFlags.Unique, "providerId", "resourceId")]
    public class NetworkAcl : ICloudResource
    {
        // network + index
        [Member("id"), Key]
        public long Id { get; set; }

        // Pretty name of the ACL (not unique)
        [Member("name")]
        public string Name { get; set; }
        
        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Deleted { get; }

        #region Provider Details

        // aws
        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; set; }

        [IgnoreDataMember]
        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; set; }

        ResourceType ICloudResource.Type => ResourceType.NetworkAcl;

        #endregion

        [IgnoreDataMember]
        public long NetworkId => ScopedId.GetScope(Id);
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
