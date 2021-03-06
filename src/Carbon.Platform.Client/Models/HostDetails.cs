﻿using System;
using System.Net;
using System.Runtime.Serialization;

using Carbon.Jose;
using Carbon.Platform.Networking;
using Carbon.Platform.Resources;
using Carbon.Platform.Storage;

namespace Carbon.Platform.Computing
{
    [DataContract]
    public class HostDetails : IHost, IResource
    {
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public long Id { get; set; }

        [DataMember(Name = "type")]
        public HostType Type { get; set; }
        
        [DataMember(Name = "status", EmitDefaultValue = false)]
        public HostStatus Status { get; set; }

        [DataMember(Name = "environmentId", EmitDefaultValue = false)]
        public long EnvironmentId { get; set; }
        
        [DataMember(Name = "addresses")]
        public string[] Addresses { get; set; }

        [DataMember(Name = "clusterId", EmitDefaultValue = false)]
        public long ClusterId { get; set; }

        [DataMember(Name = "location", EmitDefaultValue = false)]
        public LocationDetails Location { get; set; }

        [DataMember(Name = "image", EmitDefaultValue = false)]
        public ImageDetails Image { get; set; }

        [DataMember(Name = "machineType")]
        public MachineTypeDetails MachineType { get; set; }

        [DataMember(Name = "networkInterfaces", EmitDefaultValue = false)]
        public NetworkInterfaceDetails[] NetworkInterfaces { get; set; }

        [DataMember(Name = "volumes", EmitDefaultValue = false)]
        public VolumeDetails[] Volumes { get; set; }
        
        [DataMember(Name = "publicKey", EmitDefaultValue = false)]
        public Jwk PublicKey { get; set; }

        [DataMember(Name = "resource", EmitDefaultValue = false)]
        public ManagedResource Resource { get; set; }

        #region Timestamps

        [DataMember(Name = "heartbeat", EmitDefaultValue = false)]
        public DateTime? Heartbeat { get; set; }

        [DataMember(Name = "terminated", EmitDefaultValue = false)]
        public DateTime? Terminated { get; set; }

        #endregion

        #region IResource

        ResourceType IResource.ResourceType => ResourceTypes.Host;

        #endregion

        #region IHost

        IPAddress IHost.Address => IPAddress.Parse(Addresses[0]);
      
        #endregion
    }
}
