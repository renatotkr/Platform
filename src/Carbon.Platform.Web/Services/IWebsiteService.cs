using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Platform.Storage;
using Carbon.Versioning;

namespace Carbon.Platform.Web
{
    public interface IWebsiteService
    {
        Task<WebsiteInfo> CreateAsync(CreateWebsiteRequest request);

        Task<WebsiteRelease> CreateReleaseAsync(
            IWebsite website, 
            SemanticVersion version, 
            IRepositoryCommit commit, 
            byte[] sha256,
            long creatorId
        );

        Task<WebsiteInfo> GetAsync(long id);

        Task<WebsiteInfo> FindAsync(string name);

        Task<IReadOnlyList<WebsiteInfo>> ListAsync(long ownerId);

        Task<WebsiteRelease> GetReleaseAsync(long websiteId, SemanticVersion version);

        Task<IReadOnlyList<WebsiteRelease>> GetReleasesAsync(long websiteId);
    }
}