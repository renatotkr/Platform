using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;

namespace Carbon.Computing
{
    using Data.Annotations;

    [Dataset("Hosts")]
    public class Host : IHost
    {
        [Member(1), Identity]
        public long Id { get; set; }

        [Member(2)]
        public HostStatus Status { get; set; }

        [Member(3)]
        public ComputeProviderId Provider { get; set; }

        [Member(4)]
        [StringLength(50)]
        public string Location { get; set; }  // e.g. us-east-1/A

        [Member(5)]
        public IPAddress PrivateIp { get; set; }

        [Member(6)]
        public IPAddress PublicIp { get; set; }     

        [Member(8)] // volumeId used to create the base image
        public long? ImageId { get; set; }

        // Instance Ids
        // google cloud : UInt64
        // aws          : 17-character string

        [Member(9), Indexed]
        [StringLength(100)]
        public string InstanceId { get; set; }

        [Member(10)]
        [StringLength(100)] // Provides a provider specific map to the underlying hardware (memory, processors, etc)
        public string InstanceType { get; set; }

        [Member(11)] // For nested VMs
        public long? ParentId { get; set; }

        [Member(12), Timestamp]
        public DateTime Created { get; set; }

        #region Helpers

        public IList<VolumeInfo> Volumes { get; set; }

        public IList<NetworkInterface> NetworkInterfaces { get; set; }

        #endregion
    }
}

