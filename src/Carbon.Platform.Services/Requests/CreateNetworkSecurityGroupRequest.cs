using System;
using System.ComponentModel.DataAnnotations;

using Carbon.Platform.Resources;

namespace Carbon.Platform.Services
{
    public class CreateNetworkSecurityGroupRequest
    {
        public CreateNetworkSecurityGroupRequest() { }

        public CreateNetworkSecurityGroupRequest(
            string name,
            long networkId,
            ManagedResource resource)
        {

            Name      = name ?? throw new ArgumentNullException(nameof(name));
            NetworkId = networkId;
            Resource  = resource;
        }

        [Required]
        public string Name { get; set; }

        public long NetworkId { get; set; }

        public ManagedResource Resource { get; set; }
    }
}