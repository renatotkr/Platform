using System.Collections.Generic;
using System.Net;

namespace Carbon.Platform.Networking
{
    using Data.Annotations;

    [Dataset("NetworkInterfaces")]
    public class NetworkInterfaceInfo
    {
        [Member("id"), Identity]
        public long Id { get; set; }

        [Member("networkId")]
        [Indexed]
        public long NetworkId { get; set; }

        [Member("description")]
        [StringLength(100)]
        public string Description { get; set; }

        [Member("mac"), Indexed] // AKA physicalAddress, format: MM:MM:MM:SS:SS:SS
        [StringLength(30)]
        public string Mac { get; set; }

        [Member("speed")] // in octects
        public long Speed { get; set; }

        [Member("hostId"), Mutable]
        [Indexed] // Current Attachment
        public long? HostId { get; set; }

        [Member("refId"), Indexed]
        [Ascii, StringLength(50)]
        public string RefId { get; set; }

        [Member("addresses")]
        public List<IPAddress> Addresses { get; set; }
    }
}