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
            #region Preconditions

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Required", nameof(name));

            if (ownerId <= 0)
                throw new ArgumentException("Must be > 0", nameof(ownerId));

            #endregion

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