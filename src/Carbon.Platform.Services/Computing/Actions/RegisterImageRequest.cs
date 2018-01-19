using System;
using Carbon.Json;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    public class RegisterImageRequest
    {
        public RegisterImageRequest(
            long ownerId,
            string name,
            ManagedResource resource,
            ImageType type = ImageType.Machine,
            JsonObject properties = null)
        {
            Ensure.IsValidId(ownerId, nameof(ownerId));
            Ensure.NotNullOrEmpty(name, nameof(name));
 
            Name       = name;
            OwnerId    = ownerId;
            Resource   = resource;
            Type       = type;
            Properties = properties;
        }

        public long OwnerId { get; }

        public string Name { get; }

        public long Size { get; }

        public ImageType Type { get; }

        public ManagedResource Resource { get; }

        public JsonObject Properties { get; }
    }    
}