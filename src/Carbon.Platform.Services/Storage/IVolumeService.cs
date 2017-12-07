using System.Collections.Generic;
using System.Threading.Tasks;
using Carbon.Platform.Computing;

namespace Carbon.Platform.Storage
{
    public interface IVolumeService
    {
        Task<VolumeInfo> FindAsync(ResourceProvider provider, string resourceId);

        Task<VolumeInfo> GetAsync(long id);

        Task<VolumeInfo> GetAsync(string name);

        Task<IReadOnlyList<VolumeInfo>> ListAsync(long ownerId);

        Task<IReadOnlyList<VolumeInfo>> ListAsync(IHost host);

        Task<VolumeInfo> RegisterAsync(RegisterVolumeRequest request);

        Task<bool> DeleteAsync(IVolume volume);
    }
}