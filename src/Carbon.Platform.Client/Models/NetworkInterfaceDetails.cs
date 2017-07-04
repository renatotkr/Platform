using System.Net;
using System.Runtime.Serialization;

using Carbon.Net;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Networking
{
    public class NetworkInterfaceDetails : INetworkInterface
    {
        [DataMember(Name = "id", Order = 1)]
        public long Id { get; set; }

        [DataMember(Name = "macAddress", Order = 2)]
        public MacAddress MacAddress { get; set; }

        [DataMember(Name = "addresses", Order = 3)]
        public IPAddress[] Addresses { get; set; }

        #region IResource

        [DataMember(Name = "resource", Order = 4, EmitDefaultValue = false)]
        public ManagedResource Resource { get; set; }

        int IManagedResource.LocationId => Resource.LocationId;

        string IManagedResource.ResourceId => Resource.ResourceId;

        int IManagedResource.ProviderId => Resource.ProviderId;

        ResourceType IResource.ResourceType => ResourceTypes.NetworkInterface;

        #endregion
    }
}