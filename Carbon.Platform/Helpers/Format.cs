namespace Carbon.Platform
{
    using System;

    public static class AssetFormat
    {
        private static readonly string[] txtFormats = { "css", "html", "js", "mtpl", "tpl", "txt" };

        private static readonly string[] staticFormats = {
            "css", "eot", "gif", "html", "ico", "jpeg", "jpg", "js", "png", "svg", "swf", "ttf", "webm", "webp", "woff"
        };

        public static bool IsText(string format)
        {
            // Array.BinarySearch
            // TODO: Binary search

            return Array.BinarySearch(txtFormats, format) > 0;
        }

        public static bool IsStatic(string format)
        {
            return Array.BinarySearch(staticFormats, format) > -1;
        }
    }
}