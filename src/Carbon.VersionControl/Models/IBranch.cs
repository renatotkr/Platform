namespace Carbon.VersionControl
{
    public interface IBranch
    {
        string Name { get; }

        ICommit Commit { get; } // Latest commit
    }
}