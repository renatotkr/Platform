using System;

using Carbon.Storage;

namespace Carbon.Building.Web
{
    internal static class FormatHelper
    {
        private static readonly string[] staticFormats = {
            "css", "eot", "gif", "html", "ico", "jpeg", "jpg", "js",
            "png", "svg", "swf", "ttf", "webm", "webp", "woff", "woff2"
        };

        public static string GetFormat(IBlob blob)
        {
            var formatIndex = blob.Key.LastIndexOf('.');

            var format = formatIndex > -1 ? blob.Key.Substring(formatIndex + 1) : string.Empty;

            return format;
        }

        public static bool IsStaticFormat(string format)
        {
            return Array.BinarySearch(staticFormats, format) > -1;
        }
    }
}