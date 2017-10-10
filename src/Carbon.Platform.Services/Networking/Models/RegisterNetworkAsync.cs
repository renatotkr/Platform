using System;
using System.ComponentModel.DataAnnotations;

using Carbon.Platform.Resources;

namespace Carbon.Platform.Networking
{
    public class RegisterNetworkRequest
    {
        public RegisterNetworkRequest(
            string[] addressBlocks,
            long ownerId,
            ManagedResource resource)
        {
            AddressBlocks = addressBlocks ?? throw new ArgumentNullException(nameof(addressBlocks));
            OwnerId       = ownerId; 
            Resource      = resource;
        }
        
        public string[] AddressBlocks { get; }

        [Range(1, 2_199_023_255_552)]
        public long OwnerId { get; }

        public ManagedResource Resource { get; }
    }
}