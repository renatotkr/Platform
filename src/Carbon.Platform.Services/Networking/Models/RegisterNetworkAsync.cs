using System;

using Carbon.Platform.Resources;

namespace Carbon.Platform.Networking
{
    public class RegisterNetworkAsync
    {
        public RegisterNetworkAsync() { }

        public RegisterNetworkAsync(
            string[] addressBlocks,
            ManagedResource resource)
        {
            AddressBlocks = addressBlocks ?? throw new ArgumentNullException(nameof(addressBlocks));
            Resource     = resource;
        }
        
        public string[] AddressBlocks { get; set; }
        
        public ManagedResource Resource { get; set; }

        // TODO
        public long OwnerId { get; set; }
    }
}