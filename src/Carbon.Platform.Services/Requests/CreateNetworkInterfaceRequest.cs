using System;
using System.ComponentModel.DataAnnotations;
using Carbon.Net;
using Carbon.Platform.Resources;
using Carbon.Platform.Sequences;

namespace Carbon.Platform.Services
{
    public class CreateNetworkInterfaceRequest
    {
        public CreateNetworkInterfaceRequest() { }

        public CreateNetworkInterfaceRequest(
            MacAddress mac,
            long subnetId,
            ManagedResource resource)
        {
            Mac      = mac;
            SubnetId = subnetId;
            Resource = resource;
        }

        [Required]
        public MacAddress Mac { get; set; }

        public long SubnetId { get; set; }

        [Required]
        public ManagedResource Resource { get; set; }

        public long NetworkId => ScopedId.GetScope(SubnetId);
    }
}