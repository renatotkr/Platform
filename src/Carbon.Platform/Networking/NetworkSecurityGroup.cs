﻿using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Platform.Resources;
using Carbon.Platform.Sequences;

namespace Carbon.Platform.Networking
{
    [Dataset("NetworkSecurityGroups")]
    [UniqueIndex("providerId", "resourceId")]
    public class NetworkSecurityGroup : INetworkSecurityGroup
    {
        public NetworkSecurityGroup() { }

        public NetworkSecurityGroup(long id, string name, ManagedResource resource)
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Must be > 0", nameof(id));

            #endregion

            Id   = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            ProviderId = resource.ProviderId;
            ResourceId = resource.ResourceId;
        }

        // networkId | #
        [Member("id"), Key]
        public long Id { get; }

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
        
        ResourceType IResource.ResourceType => ResourceTypes.NetworkSecurityGroup;

        #endregion

        #region Timestamps

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        public DateTime? Deleted { get; }

        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion
    }
}