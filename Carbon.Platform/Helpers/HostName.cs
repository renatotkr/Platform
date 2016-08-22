using System;

namespace Carbon.Platform
{
    // TODO: Move to core
    // http://tools.ietf.org/html/rfc952

    public class HostName
    {
        public static bool IsValid(string text)
            => Uri.CheckHostName(text) != UriHostNameType.Unknown;

        // ([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*
    }
}
