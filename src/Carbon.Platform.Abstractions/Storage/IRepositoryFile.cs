namespace Carbon.Platform.Storage
{
    public interface IRepositoryFile
    {
        long BranchId { get; }

        string Path { get; }

        long Size { get; }

        byte[] Sha256 { get; }
    }
}