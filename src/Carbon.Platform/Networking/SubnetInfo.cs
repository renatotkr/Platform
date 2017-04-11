using System;
using System.Runtime.Serialization;
using System.Net;

using Carbon.Data.Annotations;

namespace Carbon.Platform.Networking
{
    [Dataset("Subnets")]
    [DataIndex(IndexFlags.Unique, "providerId", "resourceId")]
    public class SubnetInfo : ISubnet
    {
        public SubnetInfo() { }

        public SubnetInfo(long id, string cidr, ManagedResource resource)
        {
            Id         = id;
            CidrBlock  = cidr ?? throw new ArgumentNullException(nameof(cidr));
            ProviderId = resource.ProviderId;
            LocationId = resource.LocationId;
            ResourceId = resource.ResourceId;
        }

        // networkId + index
        [Member("id"), Key]
        public long Id { get; }

        [Member("cidrBlock")]
        public string CidrBlock { get; } 

        [Member("gatewayAddress")]
        public IPAddress GatewayAddress { get; set; }

        [IgnoreDataMember]
        public long NetworkId => ScopedId.GetScope(Id);

        // [a-z]([-a-z0-9]*[a-z0-9])?
        // Validate alphanumeric

        [Member("name")]
        [StringLength(63)]
        public string Name { get; set; }

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
        public long LocationId { get; }

        ResourceType IManagedResource.ResourceType => ResourceType.Subnet;

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
