namespace Carbon.Platform.Storage
{
    public interface IRepositoryFile
    {
        long RepositoryId { get; }

        string BranchName { get; }

        string Path { get; }

        long Size { get; }

        byte[] Sha256 { get; }
    }
}