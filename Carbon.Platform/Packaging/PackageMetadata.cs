namespace Carbon.Platform
{
    using System.ComponentModel.DataAnnotations;

    using System.Threading.Tasks;

    using Carbon.Data;

    public class PackageMetadata
    {
        [Required]
        public string Name { get; set; }

        public string Version { get; set; }

        public string Description { get; set; }

        public PackageContributor[] Contributors { get; set; }

        public StringMap Scripts { get; set; }

        public string Main { get; set; }

        public string License { get; set; }

        public StringMap Dependencies { get; set; }

        public static PackageMetadata Parse(string text)
        {            
            return XObject.Parse(text).As<PackageMetadata>();
        }

        public static async Task<PackageMetadata> FromAsset(IAsset asset)
        {
            return Parse(await asset.ReadStringAsync().ConfigureAwait(false));
        }
    }

    public class PackageContributor
    {
        public string Name { get; set; }

        public string Email { get; set; }
    }
}
