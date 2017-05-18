using System;

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

        public long OwnerId { get; set; }

        public ManagedResource Resource { get; set; }
    }
}