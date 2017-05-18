using Carbon.VersionControl;

namespace Carbon.Platform.CI
{
    public class CreateBuildRequest
    {
        public RevisionSource Source { get; set; }

        public long CreatorId { get; set; }
    }
}