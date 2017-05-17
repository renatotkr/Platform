using Carbon.Versioning;

namespace Carbon.Platform.Computing
{
    public interface IProgramRelease
    {
        long ProgramId { get; }

        SemanticVersion Version { get; }

        long CommitId { get; }

        long CreatorId { get; }

        byte[] Sha256 { get; }
    }
}