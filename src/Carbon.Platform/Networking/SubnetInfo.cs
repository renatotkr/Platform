﻿using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Platform.Sequences;
using Carbon.Platform.Resources;
using Carbon.Json;

namespace Carbon.Platform.Networking
{
    [Dataset("Subnets")]
    [UniqueIndex("providerId", "resourceId")]
    public class SubnetInfo : ISubnet
    {
        public SubnetInfo() { }

        public SubnetInfo(
            long id,
            string[] addressBlocks,
            ManagedResource resource)
        {
            Id            = id;
            AddressBlocks = addressBlocks ?? throw new ArgumentNullException(nameof(addressBlocks));
            ProviderId    = resource.ProviderId;
            LocationId    = resource.LocationId;
            ResourceId    = resource.ResourceId;
        }

        // networkId + index
        [Member("id"), Key]
        public long Id { get; }
        
        /// <summary>
        /// A list of address blocks in CIDR notation
        /// </summary>
        [Member("addressBlocks")]
        [StringLength(100)]
        public string[] AddressBlocks { get; }

        [Member("name")]
        [StringLength(63)]
        public string Name { get; set; }

        [Member("properties")]
        [StringLength(1000)]
        public JsonObject Properties { get; }

        [IgnoreDataMember]
        public long NetworkId => ScopedId.GetScope(Id);
        
        #region IResource

        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; }

        [IgnoreDataMember]
        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; }

        [IgnoreDataMember]
        [Member("locationId")]
        public int LocationId { get; }

        ResourceType IResource.ResourceType => ResourceTypes.Subnet;

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