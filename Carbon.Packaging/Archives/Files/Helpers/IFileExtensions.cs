using Carbon.Storage;
using System.IO;
using System.Threading.Tasks;

namespace Carbon.Packaging
{
    public static class AssetExtensions
    {
        public static bool IsStatic(this IBlobInfo file)
        {
            var format = Path.GetExtension(file.Name).TrimStart('.');

            return FileHelper.IsStatic(format);
        }

        public static async Task<string> ReadStringAsync(this IBlob blob)
        {
            using (var stream = blob.Open())
            {
                using (var reader = new StreamReader(stream))
                {
                    return await reader.ReadToEndAsync().ConfigureAwait(false);
                }
            }
        }

        public static bool IsHidden(this IBlobInfo blob)
        {
            // contains a period in the path....

            // .something.jpeg
            // /.git

            foreach (var part in blob.Name.Split('/'))
            {
                if (part.Length > 0 && part[0] == '.') return true;
            }

            return false;
        }
    }
}