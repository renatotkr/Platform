using Carbon.Extensions;

namespace Carbon.Platform.Services
{
    internal static class ResourceName
    {
        public static (ResourceProvider provider, string resourceId) Parse(string name)
        {
            var parts = name.Split(':');

            return (provider: ResourceProvider.Parse(parts[0]), resourceId: parts[1]);
        }
    }
}
