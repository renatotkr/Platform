namespace Carbon.Repositories
{
    public interface ITag
    {
        string Name { get; }

        ICommit Commit { get; } // Latest commit
    }
}
