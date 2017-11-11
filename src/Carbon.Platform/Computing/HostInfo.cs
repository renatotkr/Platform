using System;
using System.Net;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    using Extensions;

    [Dataset("Hosts", Schema = "Computing")]
    [UniqueIndex("providerId", "resourceId")]
    public class HostInfo : IHost, IManagedResource
    {
        public HostInfo() { }

        public HostInfo(
            long id,
            int locationId,
            string[] addresses,
            long environmentId,
            long clusterId,
            long imageId,
            long machineTypeId,
            long ownerId,
            ManagedResource resource,
            HostType type     = HostType.Virtual,
            HostStatus status = HostStatus.Running)
        {
            Id            = id;
            Type          = type;
            Status        = status;
            Addresses     = addresses;
            ClusterId     = clusterId;
            EnvironmentId = environmentId;
            LocationId    = locationId;
            ImageId       = imageId;
            MachineTypeId = machineTypeId;
            ProviderId    = resource.ProviderId;
            ResourceId    = resource.ResourceId;
            OwnerId       = ownerId;
        }

        // locationId | #
        [Member("id"), Key]
        public long Id { get; }

        [Member("type")]
        public HostType Type { get; }
      
        [Member("locationId")]
        public int LocationId { get; }

        [Member("environmentId"), Indexed]
        public long EnvironmentId { get; }

        [Member("clusterId"), Indexed]
        public long ClusterId { get; }

        [Member("imageId")]
        public long ImageId { get; }

        [Member("addresses")]
        public string[] Addresses { get; }

        [Member("ownerId"), Indexed]
        public long OwnerId { get; }

        [Member("parentId"), Indexed]
        public long? ParentId { get; }

        [Member("machineTypeId")]
        public long MachineTypeId { get; }
            
        #region Health

        [Member("status")]
        public HostStatus Status { get; }

        [Member("heartbeat"), Mutable]
        public DateTime? Heartbeat { get; }

        #endregion

        #region IResource
        
        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; }

        [IgnoreDataMember]
        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; }

        ResourceType IResource.ResourceType => ResourceTypes.Host;

        #endregion

        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        // Initialized...

        [Member("terminated")]
        public DateTime? Terminated { get; }

        #endregion

        #region Helpers

        [IgnoreDataMember]
        public bool IsTerminated => Terminated != null;

        private IPAddress publicIp;
        private IPAddress privateIp;

        [IgnoreDataMember]
        public IPAddress PublicIp
        {
            get => publicIp ?? (publicIp = GetIpAddress(IpAddressType.Public));
        }

        [IgnoreDataMember]
        public IPAddress PrivateIp
        {
            get => privateIp ?? (privateIp = GetIpAddress(IpAddressType.Private));
        }

        private enum IpAddressType
        {
            Public,
            Private
        }

        private IPAddress GetIpAddress(IpAddressType type)
        {
            foreach (var address in Addresses)
            {
                var ip = IPAddress.Parse(address);
                
                if (ip.IsInternal() && type == IpAddressType.Private)
                {
                    return ip;
                }

                if (!ip.IsInternal() && type == IpAddressType.Public)
                {
                    return ip;
                }
            }

            return null;
        }

        [IgnoreDataMember]
        public ResourceProvider Provider => ResourceProvider.Get(ProviderId);

        #endregion

        #region IHost

        IPAddress IHost.Address => PrivateIp;

        #endregion
    }
}

// host|os|runtime|program