namespace Carbon.Platform
{
    using System.IO;
    using System.Threading.Tasks;

    public static class AssetExtensions
    {
        public static bool IsStatic(this IAsset asset)
        {
            var format = Path.GetExtension(asset.Name).TrimStart('.');

            return AssetFormat.IsStatic(format);
        }

        public static async Task<string> ReadStringAsync(this IAsset asset)
        {
            using (var stream = asset.Open())
            {
                using (var reader = new StreamReader(stream))
                {
                    return await reader.ReadToEndAsync().ConfigureAwait(false);
                }
            }
        }

        public static bool IsHidden(this IAsset asset)
        {
            // .monsters.jpeg
            // /.git

            foreach (var part in asset.Name.Split('/'))
            {
                if (part.Length > 0 && part[0] == '.') return true;
            }

            return false;
        }
    }
}