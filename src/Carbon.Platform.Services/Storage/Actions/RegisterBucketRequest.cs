using System;

using Carbon.Platform.Resources;

namespace Carbon.Platform.Storage
{
    public class RegisterBucketRequest
    {
        public RegisterBucketRequest(long ownerId, string name, ManagedResource resource)
        {
            Ensure.Id(ownerId, nameof(ownerId));
            Ensure.NotNullOrEmpty(name, nameof(name));

            if (name.Length > 63)
                throw new ArgumentException("Must be 63 characters or fewer", nameof(name));

            Name     = name;
            Resource = resource;
            OwnerId  = ownerId;
        }

        public long OwnerId { get; }

        public string Name { get; }

        public ManagedResource Resource { get; }
    }
}