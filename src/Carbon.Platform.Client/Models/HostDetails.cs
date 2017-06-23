using System;
using System.Net;
using System.Runtime.Serialization;

using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    public class HostDetails : IHost, IResource
    {
        [DataMember(Name = "id", Order = 1)]
        public long Id { get; set; }

        [DataMember(Name = "type", Order = 2)]
        public HostType Type { get; set; }

        [DataMember(Name = "clusterId", Order = 3, EmitDefaultValue = false)]
        public long ClusterId { get; set; }

        [DataMember(Name = "addresses", Order = 4)]
        public string[] Addresses { get; set; }

        [DataMember(Name = "locationId", Order = 5, EmitDefaultValue = false)]
        public int LocationId { get; set; }

        [DataMember(Name = "networkInterfaces", Order = 7, EmitDefaultValue = false)]
        public NetworkInterfaceDetails[] NetworkInterfaces { get; set; }

        [DataMember(Name = "volumes", Order = 8, EmitDefaultValue = false)]
        public VolumeDetails[] Volumes { get; set; }

        [DataMember(Name = "machineTypeId", Order = 9, EmitDefaultValue = false)]
        public long MachineTypeId { get; set; }

        [DataMember(Name = "publicKey", EmitDefaultValue = false, Order = 10)]
        public byte[] PublicKey { get; set; }

        #region Timestamps

        [DataMember(Name = "heartbeat", EmitDefaultValue = false, Order = 11)]
        public DateTime? Heartbeat { get; set; }

        [DataMember(Name = "terminated", EmitDefaultValue = false, Order = 12)]
        public DateTime? Terminated { get; set; }

        #endregion

        #region IResource

        [DataMember(Name = "resource", Order = 20, EmitDefaultValue = false)]
        public ManagedResource? Resource { get; set; }

        ResourceType IResource.ResourceType => ResourceTypes.Host;

        #endregion

        #region IHost

        IPAddress IHost.Address => IPAddress.Parse(Addresses[0]);
      
        #endregion
    }
}
