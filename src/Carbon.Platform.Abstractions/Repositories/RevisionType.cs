namespace Carbon.Platform.Repositories
{
    public enum RevisionType : byte
    {
        Unknown = 0,
        Commit  = 1,
        Head    = 2,
        Tag     = 3
    }
}