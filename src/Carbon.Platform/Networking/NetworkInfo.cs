using System;
using System.Runtime.Serialization;
using System.Net;

using Carbon.Data.Annotations;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Networking
{
    [Dataset("Networks")]
    [DataIndex(IndexFlags.Unique, "providerId", "resourceId")]
    public class NetworkInfo : INetwork
    {
        public NetworkInfo() { }

        public NetworkInfo(long id, string cidr, IPAddress gatewayAddress, ManagedResource resource)
        {
            Id             = id;
            Cidr           = cidr ?? throw new ArgumentNullException(nameof(cidr));
            GatewayAddress = gatewayAddress;
            ProviderId     = resource.ProviderId;
            LocationId     = resource.LocationId;
            ResourceId     = resource.ResourceId;
        }

        [Member("id"), Key]
        public long Id { get; }

        // AddressSpace?

        [Member("cidr")] // 10.1.1.0/24
        [Ascii, StringLength(100)]
        public string Cidr { get; }

        // Provides WAN access
        [DataMember(Name = "gatewayAddress", EmitDefaultValue = false)]
        [Member("gatewayAddress")]
        public IPAddress GatewayAddress { get; }

        [DataMember(Name = "asn", EmitDefaultValue = false)]
        [Member("asn")] // Autonomous System Number, e.g. AS226
        public int? ASN { get; set; }

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

        ResourceType IResource.ResourceType => ResourceType.Network;

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