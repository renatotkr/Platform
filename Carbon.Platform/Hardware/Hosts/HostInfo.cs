using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Carbon.Platform
{
    using Data.Annotations;

    [Record(TableName = "Hosts")]
    public class HostInfo : IHost
    {
        [Identity]
        public long Id { get; set; }

        [Indexed]
        [MaxLength(32)]
        public string InstanceId { get; set; }

        public long ZoneId { get; set; }

        public IPAddress PrivateIp { get; set; }

        [Optional]
        public IPAddress PublicIp { get; set; }

        [Timestamp]
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
