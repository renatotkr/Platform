using System;
using System.Collections.Generic;
using System.Net;

namespace Carbon.Platform.Networking
{
    using Data.Annotations;
    using Json;

    [Dataset("NetworkInterfaces")]
    public class NetworkInterfaceInfo : ICloudResource
    {
        [Member("id"), Identity]
        public long Id { get; set; }

        [Member("macAddress"), Indexed] // AKA physicalAddress, format: MM:MM:MM:SS:SS:SS
        [StringLength(30)]
        public string MacAddress { get; set; }

        [Member("hostId"), Mutable]
        [Indexed] // Current Attachment
        public long? HostId { get; set; }

        [Member("details")]
        [StringLength(1000)]
        public JsonObject Details { get; set; }

        [Member("networkId")]
        public long NetworkId { get; set; }

        [Member("resourceName"), Unique]
        [Ascii, StringLength(100)]
        public string ResourceName { get; set; }

        [Member("addresses")]
        public List<IPAddress> Addresses { get; set; }

        [Member("created")]
        public DateTime Created { get; set; }

        #region IResource

        ResourceType ICloudResource.Type => ResourceType.NetworkInterface;

        CloudProvider ICloudResource.Provider => CloudResourceInfo.Parse(ResourceName).Provider;

        #endregion
    }
}