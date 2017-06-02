using Carbon.CI;

namespace Carbon.Platform
{
    public class PackageInfo : IPackageInfo
    {
        public string Name { get; set; }

        public long? DekId { get; set; }

        public byte[] IV { get; set; }

        public byte[] Sha256 { get; set; }
    }
}