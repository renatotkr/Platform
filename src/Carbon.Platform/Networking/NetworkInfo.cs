using System;
using System.Runtime.Serialization;
using System.Net;

namespace Carbon.Platform.Networking
{
    using Net;
    using Data.Annotations;

    [Dataset("Networks")]
    [DataIndex(IndexFlags.Unique, "providerId", "resourceId")]
    public class NetworkInfo : INetwork, ICloudResource
    {
        public NetworkInfo() { }

        public NetworkInfo(Cidr cidr)
        {
            Cidr = cidr.ToString();
        }

        [Member("id"), Key]
        public long Id { get; set; }

        [Member("cidr")] // 10.1.1.0/24
        [Ascii, StringLength(50)]
        public string Cidr { get; set; } 

        [Member("gateway")] // Provides WAN access
        public IPAddress Gateway { get; set; } // 10.1.1.0

        [DataMember(Name = "asn")]
        [Member("asn")] // Autonomous System Number, e.g. AS226
        public int? ASN { get; set; }

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

        // The rules form the firewall
        // public IList<NetworkRule> Rules { get; set; }

        // Tells packets where to go leaving a network interface
        // public IList<NetworkRoute> Routes { get; set; }

        // public IList<SubnetInfo> Subnets { get; set; }

        #region Provider Details

        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; set; }

        [IgnoreDataMember]
        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; set; }

        ResourceType ICloudResource.Type => ResourceType.Network;

        #endregion
    }

    // subnet
}

// Known as a VPC in AWS
// A VPC may have multiple cidr blocks
// AssociateVpcCidrBlock



/*
us-west1	    10.138.0.0/20	10.138.0.1
us-central1	    10.128.0.0/20	10.128.0.1
us-east1	    10.142.0.0/20	10.142.0.1
europe-west1	10.132.0.0/20	10.132.0.1
asia-east1	    10.140.0.0/20	10.140.0.1
*/
