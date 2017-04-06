namespace Carbon.VersionControl
{
    public interface ICommit
    {
        string Id { get; } // An SHA1 for GIT

        string Message { get; }
        
        // string[] ParentIds { get; }

        // string TreeId { get; }

        // Author
        // Committer
    }


}