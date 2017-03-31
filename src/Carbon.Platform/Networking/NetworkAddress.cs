using System;
using System.Net;
using System.Runtime.Serialization;

namespace Carbon.Platform.Networking
{
    using Data.Annotations;

    [Dataset("NetworkAddresses")]
    [DataIndex(IndexFlags.Unique, "providerId", "resourceId")]
    public class NetworkAddress : ICloudResource
    {
        [Member("id"), Key]
        public long Id { get; set; }

        [Member("status")]
        public NetworkAddressStatus Status { get; set; }

        [Member("networkInterfaceId")]
        [Indexed]
        public long? NetworkInterfaceId { get; set; }

        [Member("value")]
        public IPAddress Value { get; set; }

        [Member("created"), Timestamp]
        public DateTime Created { get; set; }

        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Deleted { get; }

        #region Provider Details

        // aws
        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; set; }

        // e.g. eipalloc-5723d13e
        [IgnoreDataMember]
        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; set; }

        ResourceType ICloudResource.Type => ResourceType.NetworkAddress;

        #endregion
    }
    
    public enum NetworkAddressStatus : byte
    {
        Assigned  = 1,
        Available = 2,

    }

    // eipalloc-08229861
    // AllocationId = Name
}