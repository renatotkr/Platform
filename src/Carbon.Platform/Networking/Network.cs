using System;
using System.Net;

namespace Carbon.Platform.Networking
{
    using Data.Annotations;
    using Json;

    [Dataset("Networks")]
    public class Network : ICloudResource
    {
        public Network() { }

        public Network(Cidr cidr)
        {
            Cidr = cidr.ToString();
        }

        [Member("id"), Identity]
        public long Id { get; set; }
        
        [Member("cidr")] // 10.1.1.0/24
        [Ascii, StringLength(50)]
        public string Cidr { get; set; } 

        [Member("gateway")] // Provides WAN access
        public IPAddress Gateway { get; set; } // 10.1.1.0
        
        [Member("asn")] // Autonomous System Number, e.g. AS226
        public int? ASN { get; set; }

        [Member("details")]
        [StringLength(1000)]
        public JsonObject Details { get; set; }

        [Member("resourceName"), Unique]
        [Ascii, StringLength(100)]
        public string ResourceName { get; set; }

        [Member("created"), Timestamp]
        public DateTime Created { get; set; }

        // The rules form the firewall
        // public IList<NetworkRule> Rules { get; set; }

        // Tells packets where to go leaving a network interface
        // public IList<NetworkRoute> Routes { get; set; }

        #region IResource

        ResourceType ICloudResource.Type => ResourceType.Network;

        CloudPlatformProvider ICloudResource.Provider => CloudResourceInfo.Parse(ResourceName).Provider;

        #endregion
    }

    // subnet
}

// A VPC may have multiple cidr blocks
// AssociateVpcCidrBlock


// A Network may be a VPC

/*
us-west1	    10.138.0.0/20	10.138.0.1
us-central1	    10.128.0.0/20	10.128.0.1
us-east1	    10.142.0.0/20	10.142.0.1
europe-west1	10.132.0.0/20	10.132.0.1
asia-east1	    10.140.0.0/20	10.140.0.1
*/
