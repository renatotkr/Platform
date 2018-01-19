using System.Net;
using System.Runtime.Serialization;

using Carbon.Platform.Resources;

namespace Carbon.Platform.Networking
{
    [DataContract]
    public class NetworkDetails : INetwork
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }
        
        [DataMember(Name = "ownerId", EmitDefaultValue = false)]
        public long OwnerId { get; }

        [DataMember(Name = "resource")]
        public ManagedResource Resource { get; set; }

        [DataMember(Name = "gatewayAddress", EmitDefaultValue = false)]
        public IPAddress GatewayAddress { get; set; }

        [DataMember(Name = "addressBlocks")]
        public string[] AddressBlocks { get; set; }
        
        [DataMember(Name = "asn", EmitDefaultValue = false)]
        public int? Asn { get; }

        #region IManagedResource

        ResourceType IResource.ResourceType => ResourceTypes.Network;

        #endregion
    }
}
