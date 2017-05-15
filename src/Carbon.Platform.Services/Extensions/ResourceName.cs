using System;
using System.Collections.Generic;
using System.Text;

using Carbon.Platform.Resources;

namespace Carbon.Platform.Services
{
    internal static class ResourceName
    {
        private static readonly char[] colon = { ':' };

        public static (ResourceProvider provider, string resourceId) Parse(string name)
        {
            var parts = name.Split(colon);

            return (provider: ResourceProvider.Parse(parts[0]), resourceId: parts[1]);
        }
    }
}
