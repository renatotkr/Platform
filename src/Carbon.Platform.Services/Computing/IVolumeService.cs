using System.Threading.Tasks;

namespace Carbon.Platform.Computing
{
    public interface IVolumeService
    {
        Task<VolumeInfo> FindAsync(ResourceProvider provider, string id);

        Task<VolumeInfo> GetAsync(long id);

        Task<VolumeInfo> GetAsync(string name);

        Task<VolumeInfo> RegisterAsync(RegisterVolumeRequest request);
    }
}