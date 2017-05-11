using System;

namespace Carbon.Packaging
{
    public static class FileFormat
    {
        private static readonly string[] txtFormats = { "css", "html", "js", "mtpl", "tpl", "txt" };

        private static readonly string[] staticFormats = {
            "css", "eot", "gif", "html", "ico", "jpeg", "jpg", "js", "png", "svg", "swf", "ttf", "webm", "webp", "woff", "woff2"
        };

        public static bool IsText(string format) =>  Array.BinarySearch(txtFormats, format) > 0;

        public static bool IsStatic(string format) => Array.BinarySearch(staticFormats, format) > -1;
    }
}