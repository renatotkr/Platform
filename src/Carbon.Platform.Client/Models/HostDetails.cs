using System;
using System.Net;
using System.Runtime.Serialization;

namespace Carbon.Platform
{
    using Computing;

    public class HostDetails : IHost
    {
        [DataMember(Name = "id", Order = 1)]
        public long Id { get; set; }

        [DataMember(Name = "type", Order = 2)]
        public HostType Type { get; set; }

        [DataMember(Name = "addresses", Order = 3)]
        public string[] Addresses { get; set; }

        [DataMember(Name = "heartbeat", EmitDefaultValue = false, Order = 4)]
        public DateTime? Heartbeat { get; set; }

        [DataMember(Name = "terminated", EmitDefaultValue = false, Order = 5)]
        public DateTime? Terminated { get; set; }

        [DataMember(Name = "networkInterfaces", Order = 6)]
        public NetworkInterfaceDetails[] NetworkInterfaces { get; set; }

        [DataMember(Name = "volumes", Order = 7)]
        public VolumeDetails[] Volumes { get; set; }

        IPAddress IHost.Address => IPAddress.Parse(Addresses[0]);
    }
}
