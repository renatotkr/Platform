using System;
using System.Net;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    using Extensions;

    [Dataset("Hosts")]
    [DataIndex(IndexFlags.Unique, "providerId", "resourceId")]
    public class HostInfo : IHost, IManagedResource
    {
        public HostInfo() { }

        public HostInfo(
            long id, 
            HostType type, 
            HostStatus status, 
            IPAddress[] addresses,
            IEnvironment env,
            DateTime created,
            ManagedResource resource)
        {
            Id            = id;
            Type          = type;
            Status        = status;
            Addresses     = addresses;
            EnvironmentId = env.Id;
            ProviderId    = resource.ProviderId;
            ResourceId    = resource.ResourceId;
            LocationId    = resource.LocationId;
            Created       = created;
        }

        [Member("id"), Key]
        public long Id { get; }

        [Member("type")] // Physical, Virtual, Container
        public HostType Type { get; }
       
        [Member("addresses")]
        public IPAddress[] Addresses { get; }
        
        [Member("networkId")]
        public long NetworkId { get; set; }

        [Member("environmentId")]
        [Indexed]
        public long EnvironmentId { get; set; }
        
        #region App

        [Member("appId")]
        public long AppId { get; set; }

        [Member("revision")]
        public string Revision { get; set; }

        [Member("appPort")]
        public ushort AppPort { get; set; }

        #endregion

        #region Image / Template

        [Member("machineTypeId")]
        public long MachineTypeId { get; set; }
        
        [Member("machineImageId")]
        public long MachineImageId { get; set; }

        // HostTemplateId

        #endregion
            
        #region Health

        [Member("status"), Mutable]
        public HostStatus Status { get; set; }

        [Member("heartbeat"), Mutable]
        public DateTime? Heartbeat { get; set; }

        #endregion

        #region IResource
        
        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; }

        [IgnoreDataMember]
        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; }

        // Provider + Region + Zone
        [Member("locationId")]
        public long LocationId { get; }

        ResourceType IResource.ResourceType => ResourceType.Host;

        #endregion

        #region Timestamps

        [Member("created"), Timestamp]
        [IgnoreDataMember]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        [Member("terminated")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Terminated { get; set; }

        #endregion

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

        [IgnoreDataMember]
        public ResourceProvider Provider => ResourceProvider.Get(ProviderId);

        #endregion

        #region IHost

        IPAddress IHost.Address => PrivateIp;

        ushort IHost.Port => AppPort;

        #endregion
    }
}

// ParentID?