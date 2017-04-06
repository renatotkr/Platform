namespace Carbon.VersionControl
{
    public interface ITag
    {
        string Name { get; }

        ICommit Commit { get; } // Latest commit
    }
}