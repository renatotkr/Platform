using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Platform.Resources;
using Carbon.Platform.Sequences;

namespace Carbon.Platform.Networking
{
    [Dataset("NetworkSecurityGroups")]
    [DataIndex(IndexFlags.Unique, "providerId", "resourceId")]
    public class NetworkSecurityGroup : INetworkSecurityGroup
    {
        public NetworkSecurityGroup() { }

        public NetworkSecurityGroup(long id, string name, ManagedResource resource)
        {
            #region Preconditions

            if (id <= 0) throw new ArgumentException("Invalid", nameof(id));

            #endregion

            Id   = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));

            ProviderId = resource.ProviderId;
            ResourceId = resource.ResourceId;
        }

        // networkId + sequenceNumber
        [Member("id"), Key]
        public long Id { get; }

        // a-z, A-Z, 0-9, spaces, and ._-:/()#,@[]+=;{}!$*
        [Member("name")]
        [StringLength(63)]
        public string Name { get; }
        
        [IgnoreDataMember]
        public long NetworkId => ScopedId.GetScope(Id);

        // TODO: Rules

        #region IResource

        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; }

        [IgnoreDataMember]
        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; }
        
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