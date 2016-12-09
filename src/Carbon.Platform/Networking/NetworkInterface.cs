using System;
using System.Collections.Generic;
using System.Net;

namespace Carbon.Platform.Networking
{
    using Data.Annotations;
    using Json;

    [Dataset("NetworkInterfaces")]
    public class NetworkInterfaceInfo
    {
        [Member("id"), Identity]
        public long Id { get; set; }

        [Member("description")]
        [StringLength(100)]
        public string Description { get; set; }

        [Member("macAddress"), Indexed] // AKA physicalAddress, format: MM:MM:MM:SS:SS:SS
        [StringLength(30)]
        public string MacAddress { get; set; }

        /*
        [Member("speed")] // in octects
        public long Speed { get; set; }
        */

        [Member("hostId"), Mutable]
        [Indexed] // Current Attachment
        public long? HostId { get; set; }

        [Member("details")]
        public JsonObject Details { get; set; }

        // [Member("networkId"), Indexed]
        // public long? NetworkId { get; set; }

        [Member("provider")]
        public PlatformProviderId Provider { get; set; }

        [Member("refId"), Unique]
        [Ascii, StringLength(50)]
        public string RefId { get; set; }

        [Member("addresses")]
        public List<IPAddress> Addresses { get; set; }

        [Member("created")]
        public DateTime Created { get; set; }
    }
}