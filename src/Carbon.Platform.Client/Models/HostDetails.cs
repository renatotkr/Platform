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

        [DataMember(Name = "clusterId", Order = 3)]
        public long ClusterId { get; set; }

        [DataMember(Name = "addresses", Order = 4)]
        public string[] Addresses { get; set; }

        [DataMember(Name = "heartbeat", EmitDefaultValue = false, Order = 5)]
        public DateTime? Heartbeat { get; set; }

        [DataMember(Name = "terminated", EmitDefaultValue = false, Order = 6)]
        public DateTime? Terminated { get; set; }

        [DataMember(Name = "networkInterfaces", Order = 7)]
        public NetworkInterfaceDetails[] NetworkInterfaces { get; set; }

        [DataMember(Name = "volumes", Order = 8)]
        public VolumeDetails[] Volumes { get; set; }

        #region IHost

        IPAddress IHost.Address => IPAddress.Parse(Addresses[0]);

        #endregion
    }
}
