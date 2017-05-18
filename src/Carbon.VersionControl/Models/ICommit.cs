namespace Carbon.VersionControl
{
    public interface ICommit
    {
        // (sha-1 || sha3-256)
        string Sha { get; }

        string Message { get; }
        
        IActor Author { get; }

        IActor Committer { get; }

        // string[] ParentIds { get; }

        // string TreeId { get; }
    }
}