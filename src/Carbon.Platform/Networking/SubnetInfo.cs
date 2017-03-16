using System;
using System.Runtime.Serialization;

namespace Carbon.Platform.Networking
{
    using Data.Annotations;
    using Json;

    [Dataset("Subnets")]
    [DataIndex(IndexFlags.Unique, "providerId", "name")]
    public class SubnetInfo : ICloudResource
    {
        [Member("id"), Identity]
        public long Id { get; set; }
        
        [Member("providerId")]
        public int ProviderId { get; set; }

        // subnet-8360a9e7
        [Member("name")]
        [Ascii, StringLength(100)]
        public string Name { get; set; }

        [Member("cidr")] // 10.1.1.0/24
        [Ascii, StringLength(50)]
        public string Cidr { get; set; } 

        [Member("networkId")]
        [Indexed]
        public long NetworkId { get; set; }

        [Member("locationId")]
        public long LocationId { get; set; }

        [Member("details")]
        [StringLength(1000)]
        public JsonObject Details { get; set; }

        [Member("created"), Timestamp]
        public DateTime Created { get; set; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; set; }

        #region IResource

        ResourceType ICloudResource.Type => ResourceType.Network;

        [IgnoreDataMember]
        public CloudProvider Provider => CloudProvider.Get(ProviderId);

        #endregion
    }
}

// A Network may be a VPC

// Known as a Subnetwork in Google


// Details
// -gatewayAddress

/*
us-west1	    10.138.0.0/20	10.138.0.1
us-central1	    10.128.0.0/20	10.128.0.1
us-east1	    10.142.0.0/20	10.142.0.1
europe-west1	10.132.0.0/20	10.132.0.1
asia-east1	    10.140.0.0/20	10.140.0.1
*/
