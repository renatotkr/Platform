using System.Collections.Generic;
using System.Threading.Tasks;
using Carbon.Versioning;

namespace Carbon.Platform.Web
{
    public interface IWebsiteReleaseService
    {
        Task<WebsiteRelease> CreateAsync(CreateWebsiteReleaseRequest request);

        Task<bool> ExistsAsync(long websiteId, SemanticVersion version);

        Task<WebsiteRelease> GetAsync(long websiteId, SemanticVersion version);

        Task<IReadOnlyList<WebsiteRelease>> ListAsync(long websiteId);
    }
}