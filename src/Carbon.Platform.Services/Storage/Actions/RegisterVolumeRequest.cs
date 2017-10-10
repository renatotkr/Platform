using Carbon.Platform.Resources;

namespace Carbon.Platform.Storage
{
    public class RegisterVolumeRequest
    {
        public RegisterVolumeRequest(ByteSize size, long ownerId, ManagedResource resource)
        {
            Size = size;
            Resource = resource;
            OwnerId = ownerId;
        }

        public ByteSize Size { get; }
       
        public long OwnerId { get; }

        public ManagedResource Resource { get; }
        
        public long? HostId { get; set; }
    }
}