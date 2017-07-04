using System;
using System.Net;
using System.Runtime.Serialization;

using Carbon.Platform.Networking;
using Carbon.Platform.Resources;
using Carbon.Platform.Storage;

namespace Carbon.Platform.Computing
{
    public class HostDetails : IHost, IManagedResource
    {
        [DataMember(Name = "id", Order = 1)]
        public long Id { get; set; }

        [DataMember(Name = "type", Order = 2)]
        public HostType Type { get; set; }
        
        [DataMember(Name = "status", Order = 3, EmitDefaultValue = false)]
        public HostStatus Status { get; set; }

        [DataMember(Name = "clusterId", Order = 4, EmitDefaultValue = false)]
        public long ClusterId { get; set; }

        [DataMember(Name = "addresses", Order = 5)]
        public string[] Addresses { get; set; }

        [DataMember(Name = "image", Order = 6, EmitDefaultValue = false)]
        public ImageDetails Image { get; set; }

        [DataMember(Name = "networkInterfaces", Order = 8, EmitDefaultValue = false)]
        public NetworkInterfaceDetails[] NetworkInterfaces { get; set; }

        [DataMember(Name = "volumes", Order = 9, EmitDefaultValue = false)]
        public VolumeDetails[] Volumes { get; set; }

        [DataMember(Name = "machineTypeId", Order = 10, EmitDefaultValue = false)]
        public long MachineTypeId { get; set; }

        [DataMember(Name = "publicKey", EmitDefaultValue = false, Order = 11)]
        public byte[] PublicKey { get; set; }

        [DataMember(Name = "locationId", Order = 7, EmitDefaultValue = false)]
        public int LocationId { get; set; }

        [DataMember(Name = "resource", Order = 20, EmitDefaultValue = false)]
        public ManagedResource Resource { get; set; }

        #region Timestamps

        [DataMember(Name = "heartbeat", EmitDefaultValue = false, Order = 11)]
        public DateTime? Heartbeat { get; set; }

        [DataMember(Name = "terminated", EmitDefaultValue = false, Order = 12)]
        public DateTime? Terminated { get; set; }

        #endregion

        #region IResource

        int IManagedResource.ProviderId    => Resource.ProviderId;

        string IManagedResource.ResourceId => Resource.ResourceId;

        ResourceType IResource.ResourceType => ResourceTypes.Host;

        #endregion

        #region IHost

        IPAddress IHost.Address => IPAddress.Parse(Addresses[0]);
      
        #endregion
    }
}
