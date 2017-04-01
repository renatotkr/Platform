namespace Carbon.Repositories
{
    public interface IBranch
    {
        string Name { get; }

        ICommit Commit { get; } // Latest commit
    }
}
