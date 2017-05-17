using System.Threading.Tasks;

namespace Carbon.Platform.Storage
{
    public interface IVolumeService
    {
        Task<VolumeInfo> FindAsync(ResourceProvider provider, string resourceId);

        Task<VolumeInfo> GetAsync(long id);

        Task<VolumeInfo> GetAsync(string name);

        Task<VolumeInfo> RegisterAsync(RegisterVolumeRequest request);
    }
}