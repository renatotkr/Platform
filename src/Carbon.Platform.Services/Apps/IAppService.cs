using System.Collections.Generic;
using System.Threading.Tasks;
using Carbon.Platform.Apps;
using Carbon.Versioning;

namespace Carbon.Platform.Apps
{
    public interface IAppService
    {
        Task<AppInfo> GetAsync(long id);

        Task<AppInfo> FindAsync(string name);

        Task<IReadOnlyList<AppInfo>> ListAsync(long ownerId);

        Task<AppInfo> CreateAsync(CreateAppRequest request);

        Task<EnvironmentInfo> GetEnvironmentAsync(long appId, EnvironmentType type);

        Task<IReadOnlyList<EnvironmentInfo>> GetEnvironmentsAsync(IApp app);

        Task<AppRelease> CreateReleaseAsync(CreateAppReleaseRequest request);

        Task<AppRelease> GetReleaseAsync(long appId, SemanticVersion version);

        Task<IReadOnlyList<AppRelease>> GetReleasesAsync(long appId);
    }
}