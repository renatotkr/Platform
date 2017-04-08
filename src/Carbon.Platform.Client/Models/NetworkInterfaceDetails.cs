using System;
using System.Runtime.Serialization;
using System.Net;

using Carbon.Net;

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
        public long LocationId { get; set; }

        #region IManagedResource

        int IManagedResource.ProviderId => Platform.LocationId.Create(LocationId).ProviderId;

        ResourceType IManagedResource.ResourceType => ResourceType.NetworkInterface;

        string IManagedResource.ResourceId => throw new NotImplementedException();

        #endregion

    }
}
