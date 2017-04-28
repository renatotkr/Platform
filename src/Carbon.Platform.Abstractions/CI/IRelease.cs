using Carbon.Versioning;

namespace Carbon.Platform.CI
{
    public interface IRelease
    {
        long Id { get; }

        // Application | Website
        ReleaseType Type { get; }
        
        SemanticVersion Version { get;  }

        long CommitId { get; }

        long CreatorId { get; }

        byte[] Sha256 { get; }
    }
}