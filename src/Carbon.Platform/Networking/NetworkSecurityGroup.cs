using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Networking
{
    [Dataset("NetworkSecurityGroups")]
    [DataIndex(IndexFlags.Unique, "providerId", "resourceId")]
    public class NetworkSecurityGroup : INetworkSecurityGroup
    {
        public NetworkSecurityGroup() { }

        public NetworkSecurityGroup(long id, string name)
        {
            Id   = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        // networkId + index
        [Member("id"), Key]
        public long Id { get; }

        [Member("name")]
        [StringLength(63)]
        public string Name { get; }
        
        [IgnoreDataMember]
        public long NetworkId => ScopedId.GetScope(Id);

        // Inline rules?

        #region IResource

        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; set; }

        [IgnoreDataMember]
        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; set; }
        
        int IManagedResource.LocationId => 0;

        ResourceType IResource.ResourceType => ResourceType.NetworkSecurityGroup;

        #endregion

        #region Timestamps

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

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