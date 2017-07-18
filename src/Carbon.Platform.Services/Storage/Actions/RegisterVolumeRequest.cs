using System.ComponentModel.DataAnnotations;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Storage
{
    public class RegisterVolumeRequest
    {
        public RegisterVolumeRequest() { }

        public RegisterVolumeRequest(ByteSize size, long ownerId, ManagedResource resource)
        {
            Size = size;
            Resource = resource;
            OwnerId = ownerId;
        }

        public ByteSize Size { get; set; }
        
        public long? HostId { get; set; }

        [Range(1, 2_199_023_255_552)]
        public long OwnerId { get; set; }

        public ManagedResource Resource { get; set; }
    }
}