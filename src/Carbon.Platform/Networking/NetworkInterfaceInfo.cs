using System;
using System.Runtime.Serialization;
using System.Net;

using Carbon.Data.Annotations;
using Carbon.Net;
using Carbon.Platform.Sequences;
using Carbon.Platform.Resources;
using Carbon.Json;

namespace Carbon.Platform.Networking
{
    [Dataset("NetworkInterfaces")]
    [UniqueIndex("providerId", "resourceId")]
    public class NetworkInterfaceInfo : INetworkInterface
    {
        public NetworkInterfaceInfo() { }

        public NetworkInterfaceInfo(
            long id, 
            IPAddress[] ipAddresses,
            MacAddress macAddress,
            long[] securityGroupIds,
            long subnetId,
            ManagedResource resource,
            JsonObject properties = null)
        {
            #region Preconditions
            
            if (id <= 0)
                throw new ArgumentException("Must be > 0", nameof(id));

            #endregion

            Id               = id;
            Addresses        = ipAddresses;
            MacAddress       = macAddress;
            SecurityGroupIds = securityGroupIds;
            SubnetId         = subnetId;
            ProviderId       = resource.ProviderId;
            ResourceId       = resource.ResourceId;
            LocationId       = resource.LocationId;
            Properties       = properties;
        }

        // networkId | #
        [Member("id"), Key]
        public long Id { get; }

        [DataMember(EmitDefaultValue = false)]
        [Member("addresses")]
        public IPAddress[] Addresses { get; }

        [Member("macAddress"), FixedSize(6)]
        [Indexed]
        public MacAddress MacAddress { get; }

        [Member("securityGroupIds")]
        public long[] SecurityGroupIds { get; }

        [Member("subnetId")]
        public long SubnetId { get; }

        [IgnoreDataMember]
        [Member("hostId"), Mutable, Indexed]
        public long? HostId { get; set; }

        [IgnoreDataMember]
        public long NetworkId => ScopedId.GetScope(SubnetId);

        [Member("properties")]
        [StringLength(1000)]
        public JsonObject Properties { get; }

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

        ResourceType IResource.ResourceType => ResourceTypes.NetworkInterface;

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