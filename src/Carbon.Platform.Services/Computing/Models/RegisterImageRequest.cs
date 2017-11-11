using System;
using Carbon.Json;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    public class RegisterImageRequest
    {
        public RegisterImageRequest(
            string name,
            long ownerId,
            ManagedResource resource,
            ImageType type = ImageType.Machine,
            JsonObject properties = null)
        {
            Validate.NotNullOrEmpty(name, nameof(name));
            Validate.Id(ownerId, nameof(ownerId));

            Name       = name;
            OwnerId    = ownerId;
            Resource   = resource;
            Type       = type;
            Properties = properties;
        }
        
        public string Name { get; }

        public long OwnerId { get; }

        public long Size { get; }

        public ImageType Type { get; }

        public ManagedResource Resource { get; }

        public JsonObject Properties { get; }
    }    
}