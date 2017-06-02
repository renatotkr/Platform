using Carbon.CI;
using Carbon.Platform.Storage;
using Carbon.Versioning;

namespace Carbon.Platform.Web
{
    public class CreateWebsiteReleaseRequest
    {
        public IWebsite Website { get; set; }

        public SemanticVersion Version { get; set; }

        public IRepositoryCommit Commit { get; set; }

        public IPackageInfo Package { get; set; }

        public long CreatorId { get; set; }
    }
}