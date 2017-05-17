using Carbon.Platform.Resources;

namespace Carbon.Platform.Storage
{
    public class RegisterVolumeRequest
    {
        public RegisterVolumeRequest() { }

        public RegisterVolumeRequest(ByteSize size, ManagedResource resource)
        {
            Size = size;
            Resource = resource;
        }

        public ByteSize Size { get; set; }
        
        public long? HostId { get; set; }

        public ManagedResource Resource { get; set; }
    }
}