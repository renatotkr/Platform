using System;
using System.Collections.Generic;
using System.Net;

namespace Carbon.Computing
{
    using Data.Annotations;
    using Networking;

    [Dataset("Hosts")]
    public class Host : IHost
    {
        [Member(1), Identity]
        public long Id { get; set; }

        [Member(2)]
        public HostStatus Status { get; set; }

        [Member(3)]
        public long ZoneId { get; set; }

        [Member(4)]
        public IPAddress PrivateIp { get; set; }

        [Member(5), Optional]
        public IPAddress PublicIp { get; set; }

        [Member(6)] // volumeId used to create the base image
        public long ImageId { get; set; }

        [Member(7), StringLength(255), Indexed]
        public string InstanceId { get; set; }

        [Member(9)]
        public long MemoryTotal { get; set; }

        [Member(10)] // For nested VMs
        public long? ParentId { get; set; }

        [Member(11), Timestamp]
        public DateTime Created { get; set; }

        #region Helpers

        public IList<VolumeInfo> Volumes { get; set; }

        public IList<NetworkInterface> NetworkInterfaces { get; set; }

        #endregion
    }
}

//         public IPAddress Address { get; set; }          // IP6, immutable within region ?

// tags?
// hypervisor?

// Instance Ids
// google cloud : UInt64
// aws          : 17-character string
