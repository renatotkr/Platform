using System;
using System.ComponentModel.DataAnnotations;

using Carbon.Platform.Resources;

namespace Carbon.Platform.Networking
{
    public class RegisterNetworkAsync
    {
        public RegisterNetworkAsync() { }

        public RegisterNetworkAsync(
            string[] addressBlocks,
            long ownerId,
            ManagedResource resource)
        {
            AddressBlocks = addressBlocks ?? throw new ArgumentNullException(nameof(addressBlocks));
            OwnerId       = ownerId; 
            Resource      = resource;
        }
        
        public string[] AddressBlocks { get; set; }

        [Range(1, 2_199_023_255_552)]
        public long OwnerId { get; set; }

        public ManagedResource Resource { get; set; }
    }
}