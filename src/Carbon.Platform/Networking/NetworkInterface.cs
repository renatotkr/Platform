using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Net;

namespace Carbon.Platform.Networking
{
    using Data.Annotations;
    using Json;

    [Dataset("NetworkInterfaces")]
    [DataIndex(IndexFlags.Unique, "providerId", "name")]
    public class NetworkInterfaceInfo : ICloudResource
    {
        [Member("id"), Identity]
        public long Id { get; set; }

        [Member("macAddress"), Indexed] // AKA physicalAddress, format: MM:MM:MM:SS:SS:SS
        [StringLength(30)]
        public string MacAddress { get; set; }

        [Member("providerId")]
        public int ProviderId { get; set; }

        [Member("name")]
        [Ascii, StringLength(100)]
        public string Name { get; set; }

        [Member("hostId"), Mutable]
        [Indexed] // Current Attachment
        public long? HostId { get; set; }

        [Member("networkId")]
        public long NetworkId { get; set; }

        [Member("subnetId")]
        public long? SubnetId { get; set; }

        [Member("addresses")]
        public List<IPAddress> Addresses { get; set; }

        [Member("details")]
        [StringLength(1000)]
        public JsonObject Details { get; set; }

        [Member("created"), Timestamp]
        public DateTime Created { get; set; }

        [Member("deleted")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Deleted { get; set; }
        
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; set; }

        #region Helpers

        [IgnoreDataMember]
        public bool IsDeleted => Deleted != null;

        #endregion

        #region IResource

        ResourceType ICloudResource.Type => ResourceType.NetworkInterface;

        [IgnoreDataMember]
        public CloudProvider Provider => CloudProvider.Get(ProviderId);

        #endregion
    }

    // May be attached to hosts or proxies (ALBS)
}