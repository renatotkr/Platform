namespace Carbon.Repositories
{
    public interface ICommit
    {
        string Id { get; } // An SHA1 for GIT

        // Author
        // Committer
    }
}