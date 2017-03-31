using System;
using System.Runtime.Serialization;
using System.Net;

namespace Carbon.Platform.Networking
{
    using Net;
    using Data.Annotations;

    [Dataset("Networks")]
    [DataIndex(IndexFlags.Unique, "providerId", "resourceId")]
    public class NetworkInfo : INetwork, IManagedResource
    {
        public NetworkInfo() { }

        public NetworkInfo(Cidr cidr)
        {
            CidrBlock = cidr.ToString();
        }

        [Member("id"), Key]
        public long Id { get; set; }

        [Member("cidr")] // 10.1.1.0/24
        [Ascii, StringLength(50)]
        public string CidrBlock { get; set; } 

        [Member("gateway")] // Provides WAN access
        public IPAddress Gateway { get; set; } // 10.1.1.0

        [DataMember(Name = "asn")]
        [Member("asn")] // Autonomous System Number, e.g. AS226
        public int? ASN { get; set; }

        #region IResource

        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; set; }

        [IgnoreDataMember]
        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; set; }

        ResourceType IManagedResource.Type => ResourceType.Network;

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