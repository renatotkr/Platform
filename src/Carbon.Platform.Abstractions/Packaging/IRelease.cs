using Carbon.Versioning;

namespace Carbon.CI
{
    public interface IRelease
    {
        long Id { get; }

        ReleaseType Type { get; }
        
        SemanticVersion Version { get;  }

        IPackageInfo Package { get; }

        long CommitId { get; }

        long CreatorId { get; }
    }
}