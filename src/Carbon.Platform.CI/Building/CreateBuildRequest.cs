using Carbon.VersionControl;

namespace Carbon.Platform.CI
{
    public class CreateBuildRequest
    {
        public BuildSource Source { get; set; }

        public long CreatorId { get; set; }
    }

    // TODO: Support named archives...

    public class BuildSource
    {
        public long RepositoryId { get; set; }

        public Revision Revision { get; set; }
    }
}