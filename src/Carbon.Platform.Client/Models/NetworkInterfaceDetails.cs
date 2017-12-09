using System.Net;
using System.Runtime.Serialization;

using Carbon.Net;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Networking
{
    [DataContract]
    public class NetworkInterfaceDetails : INetworkInterface
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "macAddress")]
        public MacAddress MacAddress { get; set; }

        [DataMember(Name = "addresses")]
        public IPAddress[] Addresses { get; set; }
        
        [DataMember(Name = "resource", EmitDefaultValue = false)]
        public ManagedResource Resource { get; set; }

        #region IResource

        ResourceType IResource.ResourceType => ResourceTypes.NetworkInterface;

        #endregion
    }
}