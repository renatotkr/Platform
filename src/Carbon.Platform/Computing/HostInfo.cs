using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;

namespace Carbon.Platform.Computing
{
    using Data.Annotations;
    using Extensions;
    using Json;

    [Dataset("Hosts")]
    [DataIndex(IndexFlags.Unique, "providerId", "name")]
    public class HostInfo : IHost
    {
        [Member("id"), Identity]
        public long Id { get; set; }

        [Member("providerId")]
        public int ProviderId { get; set; }

        [Member("name")]
        public string Name { get; set; }

        [Member("type")] // Physical, Virtual, Container
        public HostType Type { get; set; }

        /// <summary>
        /// The image used to launch the machine
        /// </summary>
        [Member("imageId")] 
        public long ImageId { get; set; }

        [Member("networkId")]
        public long NetworkId { get; set; }

        // Provider + Region# + Zone#
        [Member("locationId")]
        public long LocationId { get; set; }

        [Member("machineTypeId")]
        public long MachineTypeId { get; set; }

        [Member("addresses")]
        public List<IPAddress> Addresses { get; set; }
        
        [Member("details")]
        [StringLength(1000)]
        public JsonObject Details { get; set; }

        #region Health

        [Member("status"), Mutable]
        public HostStatus Status { get; set; }

        [Member("heartbeat"), Mutable]
        public DateTime? Heartbeat { get; set; }

        #endregion

        // TODO
        // public long? ParentId { get; set; }

        [Member("created"), Timestamp]
        [IgnoreDataMember]
        public DateTime Created { get; set; }

        [Member("terminated")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Terminated { get; set; }
        
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; set; }
        
        #region Helpers

        [IgnoreDataMember]
        public bool IsTerminated => Terminated != null;

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

        [IgnoreDataMember]
        public CloudProvider Provider => CloudProvider.Get(ProviderId);
    }

    // e.g. amzn:instance:i-07e6001e0415497e
}


// Instance Ids
// google cloud : UInt64
// aws          : 17-character string
// azure         : 