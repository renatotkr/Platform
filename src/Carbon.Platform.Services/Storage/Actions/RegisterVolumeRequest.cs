using Carbon.Platform.Resources;

namespace Carbon.Platform.Storage
{
    public class RegisterVolumeRequest
    {
        public RegisterVolumeRequest() { }

        public RegisterVolumeRequest(long ownerId, ByteSize size, ManagedResource resource)
        {
            Validate.Id(ownerId, nameof(ownerId));

            OwnerId  = ownerId;
            Size     = size;
            Resource = resource;
        }

        public ByteSize Size { get; set; }
       
        public long OwnerId { get; set; }

        public ManagedResource Resource { get; set; }
        
        public long? HostId { get; set; }
    }
}