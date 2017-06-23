using Carbon.Extensions;

namespace Carbon.Platform.Services
{
    internal static class ResourceName
    {
        public static (ResourceProvider provider, string resourceId) Parse(string name)
        {
            var parts = name.Split(Seperators.Colon); // :

            return (ResourceProvider.Parse(parts[0]), parts[1]);
        }
    }
}