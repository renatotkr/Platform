using System.Threading.Tasks;
using Carbon.Platform.VersionControl;
using Carbon.Versioning;
using System.Collections.Generic;

namespace Carbon.Platform.Web
{
    public interface IWebsiteService
    {
        Task CompleteDeploymentAsync(WebsiteDeployment deployment, bool successsful);

        Task<WebsiteInfo> CreateAsync(
            string name, 
            IRepository repository,
            IEnvironment env,
            long ownerId
        );

        Task<WebsiteRelease> CreateReleaseAsync(
            IWebsite website, 
            SemanticVersion version, 
            IRepositoryCommit commit, 
            byte[] sha256,
            long creatorId
        );

        Task<WebsiteInfo> GetAsync(long id);

        Task<WebsiteInfo> GetAsync(long ownerId, string name);

        Task<IReadOnlyList<WebsiteInfo>> ListAsync(long ownerId);

        Task<WebsiteRelease> GetReleaseAsync(long websiteId, SemanticVersion version);

        Task<IReadOnlyList<WebsiteRelease>> GetReleasesAsync(long websiteId);

        Task<WebsiteDeployment> StartDeployment(WebsiteRelease release, IEnvironment env, long creatorId);
    }
}