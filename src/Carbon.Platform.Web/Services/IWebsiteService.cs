using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carbon.Platform.Web
{
    public interface IWebsiteService
    {
        Task<WebsiteInfo> CreateAsync(CreateWebsiteRequest request);

        Task<WebsiteInfo> GetAsync(long id);

        Task<WebsiteInfo> FindAsync(string name);

        Task<IReadOnlyList<WebsiteInfo>> ListAsync(long ownerId);
    }
}