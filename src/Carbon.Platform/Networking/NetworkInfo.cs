using System;
using System.Runtime.Serialization;
using System.Net;

using Carbon.Data.Annotations;
using Carbon.Platform.Resources;
using Carbon.Json;

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
            int? asn = null,
            JsonObject properties = null)
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Must be > 0", nameof(id));

            if (ownerId <= 0)
                throw new ArgumentException("Must be > 0", nameof(ownerId));

            #endregion

            Id             = id;
            AddressBlocks  = addressBlocks ?? throw new ArgumentNullException(nameof(addressBlocks));
            GatewayAddress = gatewayAddress;
            Asn            = asn;
            ProviderId     = resource.ProviderId;
            LocationId     = resource.LocationId;
            ResourceId     = resource.ResourceId;
            OwnerId        = ownerId;
            Properties     = properties;
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

        [Member("ownerId"), Indexed]
        public long OwnerId { get; }

        [Member("properties")]
        [StringLength(1000)]
        public JsonObject Properties { get; }

        #region IResource

        [Member("providerId")]
        public int ProviderId { get; }

        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; }

        [Member("locationId")]
        public int LocationId { get; }

        ResourceType IResource.ResourceType => ResourceTypes.Network;

        #endregion

        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [Member("deleted")]
        public DateTime? Deleted { get; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion
    }
}