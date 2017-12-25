using System.ComponentModel.DataAnnotations;

using Carbon.Platform.Resources;

namespace Carbon.Platform.Networking
{
    public class RegisterNetworkSecurityGroupRequest
    {
        public RegisterNetworkSecurityGroupRequest(
            string name,
            long networkId,
            ManagedResource resource)
        {
            Validate.NotNull(name, nameof(name));
            Validate.Id(networkId, nameof(networkId));

            Name      = name;
            NetworkId = networkId;
            Resource  = resource;
        }

        [Required]
        public string Name { get; }

        [Range(1, 2_199_023_255_552)]
        public long NetworkId { get; }

        public ManagedResource Resource { get; }
    }
}