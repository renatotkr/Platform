using System;
using System.ComponentModel.DataAnnotations;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Networking
{
    public class RegisterSubnetRequest
    {
        public RegisterSubnetRequest(
            string[] addressBlocks,
            long networkId,
            ManagedResource resource)
        {
            AddressBlocks = addressBlocks ?? throw new ArgumentNullException(nameof(addressBlocks));
            NetworkId     = networkId;
            Resource      = resource;
        }

        public string[] AddressBlocks { get; }

        [Range(1, 2_199_023_255_552)]
        public long NetworkId { get; }

        public ManagedResource Resource { get; }
    }
}