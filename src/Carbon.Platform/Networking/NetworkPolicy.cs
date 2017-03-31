using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;

namespace Carbon.Platform.Networking
{
    [Dataset("NetworkPolicies")]
    [DataIndex(IndexFlags.Unique, "providerId", "resourceId")]
    public class NetworkPolicy : INetworkPolicy, IManagedResource
    {
        // network + index
        [Member("id"), Key]
        public long Id { get; set; }

        // Pretty name
        [Member("name")]
        public string Name { get; set; }
        
        [IgnoreDataMember]
        public long NetworkId => ScopedId.GetScope(Id);

        #region IResource

        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; set; }

        [IgnoreDataMember]
        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; set; }

        ResourceType IManagedResource.Type => ResourceType.NetworkPolicy;

        #endregion

        #region Timestamps

        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Deleted { get; }

        #endregion
    }
}