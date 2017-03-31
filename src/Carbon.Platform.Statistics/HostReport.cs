
using System.IO;
using System.Runtime.Serialization;

namespace Carbon.Platform
{
    [DataContract]
    public class HostReport
    {
        [DataMember(Order = 1)]
        public long HostId { get; set; }

        [DataMember(Order = 2)]
        public ProcessorStats[] Processors { get; set; }

        [DataMember(Order = 3)]
        public AppStats[] Apps { get; set; }

        [DataMember(Order = 4)]
        public NetworkInterfaceStats[] NetworkInterfaces { get; set; }

        [DataMember(Order = 5)]
        public VolumeStats[] Volumes { get; set; }

        [DataMember(Order = 6)]
        public MemoryStats Memory { get; set; }

        [DataMember(Order = 7)]
        public ReportPeriod Period { get; set; }

        public byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(ms, this);

                return ms.ToArray();
            }
        }

        public static HostReport Deserialize(byte[] data)
        {
            using (var ms = new MemoryStream(data))
            {
                return ProtoBuf.Serializer.Deserialize<HostReport>(ms);
            }
        }
    }
}