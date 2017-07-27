using System;

using Carbon.Storage;

namespace Carbon.Building.Web
{
    internal static class FormatHelper
    {
        private static readonly string[] staticFormats = {
            "css", "eot", "gif", "html", "ico", "jpeg", "jpg", "js", "png", "svg", "swf", "ttf", "webm", "webp", "woff", "woff2"
        };

        public static string GetFormat(IBlob file)
        {
            var formatIndex = file.Name.LastIndexOf(".");

            var format = formatIndex > -1 ? file.Name.Substring(formatIndex + 1) : "";

            return format;
        }

        public static bool IsStaticFormat(string format)
        {
            return Array.BinarySearch(staticFormats, format) > -1;
        }
    }
}
