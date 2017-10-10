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
       
        public long OwnerId { get; set; }

        public ManagedResource Resource { get; set; }
        
        public long? HostId { get; set; }
    }
}