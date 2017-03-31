using System;
using System.Runtime.Serialization;

namespace Carbon.Platform.Networking
{
    using Data.Annotations;

    [Dataset("Subnets")]
    [DataIndex(IndexFlags.Unique, "providerId", "resourceId")]
    public class SubnetInfo : ISubnet, ICloudResource
    {
        // networkId + index
        [Member("id"), Key]
        public long Id { get; set; }

        [Member("cidr")] // 10.1.1.0/24
        [Ascii, StringLength(50)]
        public string Cidr { get; set; } 

        [Member("locationId")]
        public long LocationId { get; set; }

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [IgnoreDataMember]
        public long NetworkId => ScopedId.GetScope(Id);

        #region IResource

        // e.g. aws
        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; set; }

        // e.g. subnet-8360a9e7
        [IgnoreDataMember]
        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; set; }

        ResourceType ICloudResource.Type => ResourceType.Subnet;

        #endregion
    }
}

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
