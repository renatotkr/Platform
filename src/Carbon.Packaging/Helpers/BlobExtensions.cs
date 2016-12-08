using System.IO;

namespace Carbon.Extensions
{
    using Packaging;
    using Storage;

    public static class BlobExtensions
    {
        public static bool IsStatic(this IBlob file)
        {
            var format = Path.GetExtension(file.Name).TrimStart(Seperators.Period);

            return FileFormat.IsStatic(format);
        }

        public static bool IsHidden(this IBlob blob)
        {
            // contains a period in the path....

            // .something.jpeg
            // /.git
         
            foreach (var part in blob.Name.Split(Seperators.ForwardSlash))
            {
                if (part.Length > 0 && part[0] == '.') return true;
            }

            return false;
        }
    }
}