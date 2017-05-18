namespace Carbon.VersionControl
{
    public interface ICommit
    {
        // SHA (sha1 || sha3)
        string Id { get; } // An SHA1 for GIT

        string Message { get; }
        
        // string[] ParentIds { get; }

        // string TreeId { get; }

        // Author
        // Committer
    }


}