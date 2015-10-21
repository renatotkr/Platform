namespace Carbon.Platform
{
    using System.Collections.Generic;

    using Carbon.Data;

    public class PackageInfo
    {
        public string Name { get; set; }

        public string Version { get; set; }

        public string Description { get; set; }

        public string License { get; set; }

        public string Main { get; set; }

        // public PackageRepository Repository { get; set; }

        public StringMap Dependencies { get; set; }

        public static PackageInfo Parse(string text)
        {            
            return XObject.Parse(text).As<PackageInfo>();
        }

        public static PackageInfo Parse(IAsset asset)
        {
            return Parse(asset.ReadStringAsync().ToString());
        }
    }
}
