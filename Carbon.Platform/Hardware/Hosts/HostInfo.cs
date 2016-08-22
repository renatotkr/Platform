using System;
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

        [Member(2, MaxLength = 32), Indexed]
        public string InstanceId { get; set; }

        [Member(3)]
        public long ZoneId { get; set; }

        [Member(4)]
        public IPAddress PrivateIp { get; set; }

        [Member(5), Optional]
        public IPAddress PublicIp { get; set; }

        [Version]
        public DateTime Created { get; set; }

        #region Helpers

        [Exclude]
        public IList<VolumeInfo> Volumes { get; set; }

        [Exclude]
        public IList<NetworkInterfaceInfo> NetworkInterfaces { get; set; }

        #endregion
    }
}

// Instance Ids
// google cloud : UInt64
// aws          : 17-character string
