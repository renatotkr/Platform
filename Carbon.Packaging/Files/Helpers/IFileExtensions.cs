using System.IO;
using System.Threading.Tasks;

namespace Carbon.Packaging
{
    public static class AssetExtensions
    {
        public static bool IsStatic(this IFile file)
        {
            var format = Path.GetExtension(file.Name).TrimStart('.');

            return FileHelper.IsStatic(format);
        }

        public static async Task<string> ReadStringAsync(this IFile asset)
        {
            using (var stream = asset.Open())
            {
                using (var reader = new StreamReader(stream))
                {
                    return await reader.ReadToEndAsync().ConfigureAwait(false);
                }
            }
        }

        public static bool IsHidden(this IFile asset)
        {
            // contains a period in the path....

            // .something.jpeg
            // /.git

            foreach (var part in asset.Name.Split('/'))
            {
                if (part.Length > 0 && part[0] == '.') return true;
            }

            return false;
        }
    }
}