using System;

namespace Carbon.Platform.Resources
{
    internal static class ResourceError
    {
        public static ResourceNotFoundException NotFound( ResourceType type, long id)
        {
            // host#1
            string resource = $"{type.ToString()}#{id}";

            return new ResourceNotFoundException(resource);
        }

        public static ResourceNotFoundException NotFound(ResourceType type, long ownerId, string name)
        {
            // host#1
            string resource = $"{type.ToString()}:{ownerId}#{name}";

            return new ResourceNotFoundException(resource);
        }

        public static ResourceNotFoundException NotFound(ResourceProvider provider, ResourceType type, string resourceName)
        {
            // aws:host/i-13vasds
            string resource = $"{provider.Code}:{type.ToString()}/{resourceName}";

            return new ResourceNotFoundException(resource);
        }
    }

    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException(string resource)
            : base(resource + " not found") { }
    }
}
