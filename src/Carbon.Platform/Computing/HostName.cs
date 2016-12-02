using System;

namespace Carbon.Platform.Computing
{
    // http://tools.ietf.org/html/rfc952

    public static class Hostname
    {
        public static bool IsValid(string text)
            => Uri.CheckHostName(text) != UriHostNameType.Unknown;

        // ([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*
    }
}
