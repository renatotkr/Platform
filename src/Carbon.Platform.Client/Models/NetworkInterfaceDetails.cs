using System;
using System.Runtime.Serialization;
using System.Net;

using Carbon.Net;
using Carbon.Platform.Resources;

namespace Carbon.Platform
{
    using Networking;

    public class NetworkInterfaceDetails : INetworkInterface
    {
        [DataMember(Name = "id", Order = 1)]
        public long Id { get; set; }

        [DataMember(Name = "macAddress", Order = 2)]
        public MacAddress MacAddress { get; set; }

        [DataMember(Name = "addresses", Order = 3)]
        public IPAddress[] Addresses { get; set; }

        [DataMember(Name = "locationId", Order = 4)]
        public int LocationId { get; set; }

        #region IManagedResource

        int IManagedResource.ProviderId => Platform.LocationId.Create(LocationId).ProviderId;

        ResourceType IResource.ResourceType => ResourceTypes.NetworkInterface;

        string IManagedResource.ResourceId => throw new NotImplementedException();

        #endregion
    }
}