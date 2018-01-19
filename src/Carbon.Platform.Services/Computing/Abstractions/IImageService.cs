using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carbon.Platform.Computing
{
    public interface IImageService
    {
        Task<IReadOnlyList<ImageInfo>> ListAsync();

        Task<ImageInfo> GetAsync(long id);

        Task<ImageInfo> GetAsync(long ownerId, string name);

        Task<ImageInfo> FindAsync(ResourceProvider provider, string resourceId);

        Task<ImageInfo> GetAsync(ResourceProvider provider, string resourceId);

        Task<bool> ExistsAsync(ResourceProvider provider, string resourceId);

        Task<ImageInfo> RegisterAsync(RegisterImageRequest request);

        Task<bool> DeleteAsync(IImage image);
    }
}