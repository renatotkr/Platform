using System;

namespace Carbon.Platform.Resources
{
    public static class ResourceError
    {
        public static ResourceConflictException Conflict(ResourceType type, long id)
        {
            // host#1
            string resource = $"{type}#{id}";

            return new ResourceConflictException(resource);
        }

        public static ResourceNotFoundException NotFound(ResourceType type, long id)
        {
            // host#1
            string resource = $"{type}#{id}";

            return new ResourceNotFoundException(resource);
        }

        public static ResourceNotFoundException NotFound(ResourceType type, long ownerId, string name)
        {
            // host:1:#1
            string resource = $"{type}:{ownerId}/{name}";

            return new ResourceNotFoundException(resource);
        }

        public static ResourceNotFoundException NotFound(
            ResourceProvider provider,
            ResourceType type, 
            string resourceName)
        {
            // aws:host/i-13vasds
            var resource = $"{provider.Code}:{type}/{resourceName}";

            return new ResourceNotFoundException(resource);
        }
    }

    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException(string resource)
            : base(resource + " not found") { }
    }

    public class ResourceConflictException : Exception
    {
        public ResourceConflictException(string resource)
            : base(resource + " already exists") { }
    }
}
