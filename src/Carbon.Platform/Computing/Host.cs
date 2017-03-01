using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;

namespace Carbon.Platform.Computing
{
    using Data.Annotations;
    using Extensions;
    using Json;

    // HostInfo?

    [Dataset("Hosts")]
    public class Host : IHost
    {
        public Host() { }

        public Host(HostType type)
        {
            Type = type;
        }

        [Member("id"), Identity]
        public long Id { get; set; }

        [Member("type")] // Physical, Virtual, Container
        public HostType Type { get; set; }

        // Can be used to lookup platform, hypervisor, etc
        [Member("imageId")]
        public long ImageId { get; set; }

        [Member("networkId")]
        public long NetworkId { get; set; }

        // Provider + Region# + Zone#
        // NOTE: Was RegionId...
        [Member("locationId")]
        public long LocationId { get; set; }

        [Member("machineTypeId")]
        public long MachineTypeId { get; set; }

        [Member("status"), Mutable]
        public HostStatus Status { get; set; }
        
        [Member("heartbeat"), Mutable]
        public DateTime? Heartbeat { get; set; }

        [Member("addresses")]
        public List<IPAddress> Addresses { get; set; }
        
        [Member("details")]
        [StringLength(1000)]
        public JsonObject Details { get; set; }

        // Instance Ids
        // google cloud : UInt64
        // aws          : 17-character string
        // azure         : ?

        // e.g. amzn:instance:i-07e6001e0415497e4

        // TODO: Replace with Name & Provider
        [Member("resourceName"), Unique]
        [Ascii, StringLength(100)]
        public string ResourceName { get; set; }

        [Member("modified"), Timestamp]
        public DateTime Modified { get; set; }

        [Member("created")]
        public DateTime Created { get; set; }

        #region Address Helpers

        [IgnoreDataMember]
        public IPAddress PublicIp
        {
            get
            {
                foreach (var address in Addresses)
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

        public CloudProvider Provider =>
            CloudResourceInfo.Parse(ResourceName).Provider;
    }
}

// A host may be a physical or virtual machine -- or even a container

/*
{
    id: 1,
    networkId: 1,
    resource: "amzn:instance/i-34523-4234"
}
*/