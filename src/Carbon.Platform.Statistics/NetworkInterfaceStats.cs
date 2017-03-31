using System.Runtime.Serialization;

namespace Carbon.Platform
{
    [DataContract]
    public class NetworkInterfaceStats
    {
        [DataMember(Name = "bytesReceived", Order = 2)]
        public long BytesReceived { get; set; }

        [DataMember(Name = "bytesSent", Order = 3)]
        public long BytesSent { get; set; }

        [DataMember(Name = "packetsReceived", Order = 4)]
        public long PacketsReceived { get; set; }

        [DataMember(Name = "packetsSent", Order = 5)]
        public long PacketsSent { get; set; }

        [DataMember(Name = "packetsDiscarded", Order = 6)]
        public long PacketsDiscarded { get; set; }

        // [DataMember(Name = "outputQueueLength", Order = 7)]
        // public long OutputQueueLength { get; set; }
    }
}
