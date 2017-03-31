using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Net;

namespace Carbon.Platform.Networking
{
    using Data.Annotations;

    [Dataset("NetworkInterfaces")]
    [DataIndex(IndexFlags.Unique, "providerId", "resourceId")]
    public class NetworkInterfaceInfo : INetworkInterface, IManagedResource
    {
        [Member("id"), Key]
        public long Id { get; set; }

        // AKA physicalAddress
        // format: MM:MM:MM:SS:SS:SS
        [Member("macAddress")]
        [StringLength(30)]
        [Indexed]
        public string MacAddress { get; set; }

        [Member("hostId"), Mutable]
        [Indexed] // Current Attachment
        public long? HostId { get; set; }

        [Member("networkId")]
        public long NetworkId { get; set; }

        // Do all network interfaces belong to a subnet?
        [Member("subnetId")]
        public long? SubnetId { get; set; }

        [Member("addresses")]
        public List<IPAddress> Addresses { get; set; }

        #region Stats & Health

        [Member("heartbeat")]
        public DateTime? Heartbeat { get; set; }

        [Member("bytesReceived")]
        public long BytesReceived { get; set; }

        [Member("bytesSent")]
        public long BytesSent { get; set; }

        [Member("packetsReceived")]
        public long PacketsReceived { get; set; }

        [Member("packetsSent")]
        public long PacketsSent { get; set; }

        [Member("packetsDiscarded")]
        public long PacketsDiscarded { get; set; }

        #endregion

        #region IResource

        // e.g. aws
        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; set; }

        [IgnoreDataMember]
        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; set; }

        ResourceType IManagedResource.Type => ResourceType.NetworkInterface;

        #endregion

        #region Timestamps

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Deleted { get; }

        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion
    }

    // May be attached to hosts or proxies (ALBS)
}