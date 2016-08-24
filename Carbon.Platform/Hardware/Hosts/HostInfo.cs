﻿using System;
using System.Collections.Generic;
using System.Net;

namespace Carbon.Platform
{
    using Data.Annotations;

    [Record(TableName = "Hosts")]
    public class HostInfo : IHost
    {
        [Member(1), Identity]
        public long Id { get; set; }

        [Member(2)]
        public HostStatus Status { get; set; }

        [Member(3)]
        public long ZoneId { get; set; }

        [Member(4)]
        public IPAddress PrivateIp { get; set; }

        [Member(5)]
        public long ImageId { get; set; }

        [Member(6, MaxLength = 32), Indexed]
        public string InstanceId { get; set; }

        [Member(7), Timestamp(false)]
        public DateTime Created { get; set; }

        [Member(8, mutable: true)]
        public long MemoryUsed { get; set; }

        [Member(9)]
        public long MemoryTotal { get; set; }

        [Member(10)] // For virtualized hosts
        public long? ParentId { get; set; }

        #region Helpers

        public IList<VolumeInfo> Volumes { get; set; }

        public IList<NetworkInterfaceInfo> NetworkInterfaces { get; set; }

        #endregion
    }
}

//         public IPAddress Address { get; set; }          // IP6, immutable within region ?

// tags?
// hypervisor?

// Instance Ids
// google cloud : UInt64
// aws          : 17-character string
