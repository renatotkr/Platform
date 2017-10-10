using System.ComponentModel.DataAnnotations;

using Carbon.Net;
using Carbon.Platform.Resources;
using Carbon.Platform.Sequences;

namespace Carbon.Platform.Networking
{
    public class RegisterNetworkInterfaceRequest
    {
        public RegisterNetworkInterfaceRequest() { }

        public RegisterNetworkInterfaceRequest(
            MacAddress mac,
            long subnetId,
            long[] securityGroupIds,
            ManagedResource resource)
        {
            Mac              = mac;
            SubnetId         = subnetId;
            SecurityGroupIds = securityGroupIds;
            Resource         = resource;
        }

        [Required]
        public MacAddress Mac { get; set; }

        public long SubnetId { get; set; }
        
        public long[] SecurityGroupIds { get; set; }

        [Required]
        public ManagedResource Resource { get; set; }
        
        public long? HostId { get; set; }

        public long NetworkId => ScopedId.GetScope(SubnetId);
    }
}