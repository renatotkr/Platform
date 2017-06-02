namespace Carbon.CI
{
    public interface IPackageInfo
    {
        string Name { get; }

        long? DekId { get; }

        byte[] IV { get; }

        byte[] Sha256 { get; }
    }
}