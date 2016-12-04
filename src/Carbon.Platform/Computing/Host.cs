using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;

namespace Carbon.Platform.Computing
{
    using Data.Annotations;
    using Extensions;
    using Networking;
    using Storage;

    // AKA: Linnode, Droplet, Instance, VM, Container ...

    [Dataset("Hosts")]
    public class Host : IHost
    {
        [Member("id"), Identity]
        public long Id { get; set; }

        [Member("status"), Mutable]
        public InstanceStatus Status { get; set; }

        [Member("description")]
        [StringLength(100)]
        public string Description { get; set; }

        [Member("type")] // Physical, Virtual, Container
        public HostType Type { get; set; }

        [Member("provider")]
        public PlatformProviderId Provider { get; set; }

        [Member("location")]
        [StringLength(50)]
        public string Location { get; set; }  // e.g. us-east-1/A

        [Member("addresses")]
        public List<IPAddress> Addresses { get; set; }

        // Can be used to lookup platform, hypervisor, etc
        [Member("imageId")]
        public long ImageId { get; set; }

        // Memory (512MB)
        // Processors (1)

        #region Address Helpers

        [IgnoreDataMember]
        public IPAddress PublicIp
        {
            get
            {
                foreach(var address in Addresses)
                {
                    if (!address.IsInternal())
                    {
                        return address;
                    }
                }

                return null;
            }
        }

        [IgnoreDataMember]
        public IPAddress PrivateIp
        {
            get
            {
                foreach (var address in Addresses)
                {
                    if (address.IsInternal())
                    {
                        return address;
                    }
                }

                return null;
            }
        }

        #endregion

        #region Provider Instance Details

        // Instance Ids
        // google cloud : UInt64
        // aws          : 17-character string
        // azure         : ?

        [Member("machineType")]
        [Ascii, StringLength(50)] // Provides a provider specific map to the underlying hardware (memory, processors, etc)
        public string MachineType { get; set; }

        #endregion
        
        // TODO: Processors

        public IList<VolumeInfo> Volumes { get; set; }

        public IList<NetworkInterfaceInfo> NetworkInterfaces { get; set; }

        [Member("refId"), Unique]
        [Ascii, StringLength(50)]
        public string RefId { get; set; }

        [Member("modified")]
        public DateTime Modified { get; set; }

        [Member("created")]
        public DateTime Created { get; set; }
    }
}

