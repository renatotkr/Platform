using System.ComponentModel.DataAnnotations;

using Carbon.Net;
using Carbon.Platform.Resources;
using Carbon.Platform.Sequences;

namespace Carbon.Platform.Networking
{
    public class RegisterNetworkInterfaceRequest
    {
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
        public MacAddress Mac { get; }

        public long SubnetId { get; }
        
        public long[] SecurityGroupIds { get; }

        [Required]
        public ManagedResource Resource { get; }
        
        public long? HostId { get; set; }

        public long NetworkId => ScopedId.GetScope(SubnetId);
    }
}