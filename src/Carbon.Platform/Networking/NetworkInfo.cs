using System;
using System.Runtime.Serialization;
using System.Net;

using Carbon.Data.Annotations;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Networking
{
    [Dataset("Networks")]
    [UniqueIndex("providerId", "resourceId")]
    public class NetworkInfo : INetwork
    {
        public NetworkInfo() { }

        public NetworkInfo(
            long id, 
            string[] addressBlocks,
            long ownerId,
            ManagedResource resource, 
            IPAddress gatewayAddress = null,
            int? asn = null)
        {
            Id             = id;
            AddressBlocks  = addressBlocks ?? throw new ArgumentNullException(nameof(addressBlocks));
            GatewayAddress = gatewayAddress;
            Asn            = asn;
            ProviderId     = resource.ProviderId;
            LocationId     = resource.LocationId;
            ResourceId     = resource.ResourceId;
            OwnerId        = ownerId;
        }
        
        [Member("id"), Key(sequenceName: "networkId")]
        public long Id { get; }

        [Member("addressBlocks")]
        [StringLength(100)]
        public string[] AddressBlocks { get; }

        // Provides WAN access
        [Member("gatewayAddress")]
        [DataMember(Name = "gatewayAddress", EmitDefaultValue = false)]
        public IPAddress GatewayAddress { get; }

        /// <summary>
        /// Autonomous System Number
        /// e.g. AS226
        /// </summary>
        [Member("asn")]
        [DataMember(Name = "asn", EmitDefaultValue = false)]
        public int? Asn { get; }

        [Member("ownerId")]
        public long OwnerId { get; }

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

        ResourceType IResource.ResourceType => ResourceTypes.Network;

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
        public DateTime? Deleted { get; }

        #endregion
    }
}